using HarmonyLib;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic;

namespace EbonsContentMod.Rules
{
    internal static class TableTopUMD
    {
        internal static void Configure()
        {
            //
        }
        
        private static int GetUMD(UnitDescriptor unit)
        {
            return unit.Stats
                .GetStat(StatType.SkillUseMagicDevice)
                .ModifiedValue;
        }


        // ============================================================
        // STAT RESTRICTIONS
        // ============================================================

        [HarmonyPatch(
            typeof(EquipmentRestrictionStat),
            nameof(EquipmentRestrictionStat.CanBeEquippedBy))]
        private static class EquipmentRestrictionStat_Patch
        {
            [HarmonyPostfix]
            private static void Postfix(
                EquipmentRestrictionStat __instance,
                UnitDescriptor unit,
                ref bool __result)
            {
                if (!Main.Settings.TableTopUMD)
                {
                    return;
                }

                // Already satisfies the normal restriction
                if (__result)
                    return;

                int umd =
                    GetUMD(unit);

                int statValue =
                    unit.Stats
                        .GetStat(__instance.Stat)
                        .PermanentValue;

                if (statValue >=
                    __instance.MinValue - (umd - 15))
                {
                    __result = true;
                }
            }
        }


        // ============================================================
        // ALIGNMENT RESTRICTIONS
        // ============================================================

        [HarmonyPatch(
            typeof(EquipmentRestrictionAlignment),
            nameof(EquipmentRestrictionAlignment.CanBeEquippedBy))]
        private static class EquipmentRestrictionAlignment_Patch
        {
            [HarmonyPostfix]
            private static void Postfix(
                UnitDescriptor unit,
                ref bool __result)
            {
                if (!Main.Settings.TableTopUMD)
                {
                    return;
                }

                // Actual alignment already qualifies.
                if (__result)
                    return;


                // Emulate Alignment: DC 30
                if (GetUMD(unit) >= 30)
                {
                    __result = true;
                }
            }
        }


        // ============================================================
        // SINGLE CLASS RESTRICTIONS
        // ============================================================

        [HarmonyPatch(
            typeof(EquipmentRestrictionClass),
            nameof(EquipmentRestrictionClass.CanBeEquippedBy))]
        private static class EquipmentRestrictionClass_Patch
        {
            [HarmonyPostfix]
            private static void Postfix(
                EquipmentRestrictionClass __instance,
                UnitDescriptor unit,
                ref bool __result)
            {
                if (!Main.Settings.TableTopUMD)
                {
                    return;
                }

                // Actual class already qualifies.
                if (__result)
                    return;

                // Doesn't allow you to emulate NOT being a class
                if (__instance.Not)
                    return;

                //Emulate Class Feature: DC 20
                if (GetUMD(unit) >= 20)
                {
                    __result = true;
                }
            }
        }


        // ============================================================
        // CLASS LIST RESTRICTIONS
        // ============================================================

        [HarmonyPatch(
            typeof(EquipmentRestrictionHasAnyClassFromList),
            nameof(EquipmentRestrictionHasAnyClassFromList.CanBeEquippedBy))]
        private static class EquipmentRestrictionHasAnyClassFromList_Patch
        {
            [HarmonyPostfix]
            private static void Postfix(
                EquipmentRestrictionHasAnyClassFromList __instance,
                UnitDescriptor unit,
                ref bool __result)
            {
                if (!Main.Settings.TableTopUMD)
                {
                    return;
                }

                // Character actually belongs to one of the classes.
                if (__result)
                    return;

                if (__instance.Not)
                    return;

                if (GetUMD(unit) >= 20)
                {
                    __result = true;
                }
            }
        }
    }
}