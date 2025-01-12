using HarmonyLib;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EbonsContentMod.Utilities.ActivatableAbilityGroupUtilities;

namespace EbonsContentMod.Utilities
{
    public static class ActivatableAbilityGroupUtilities
    {
        // Add new ActivatableAbilityGroups here
        public enum ECActivatableAbilityGroup : int
        {
            HungryGhostMonkAbilities = 1313,
            BleedingWoundAbilities = 1414,
            SkinwalkerChangeShapeAbilities = 1515
        }

        private static bool IsECGroup(this ActivatableAbilityGroup group)
        {
            return Enum.IsDefined(typeof(ECActivatableAbilityGroup), (int)group);
        }

        [HarmonyPatch(typeof(UnitPartActivatableAbility), nameof(UnitPartActivatableAbility.IncreaseGroupSize), new Type[] { typeof(ActivatableAbilityGroup) })]
        static class UnitPartActivatableAbility_IncreaseGroupSize_Patch
        {

            static bool Prefix(UnitPartActivatableAbility __instance, ActivatableAbilityGroup group)
            {
                if (group.IsECGroup())
                {
                    var extensionPart = __instance.Owner.Ensure<UnitPartActivatableAbilityGroupEC>();
                    extensionPart.IncreaseGroupSize((ECActivatableAbilityGroup)group);
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(UnitPartActivatableAbility), nameof(UnitPartActivatableAbility.DecreaseGroupSize), new Type[] { typeof(ActivatableAbilityGroup) })]
        static class UnitPartActivatableAbility_DecreaseGroupSize_Patch
        {

            static bool Prefix(UnitPartActivatableAbility __instance, ActivatableAbilityGroup group)
            {
                if (group.IsECGroup())
                {
                    var extensionPart = __instance.Owner.Ensure<UnitPartActivatableAbilityGroupEC>();
                    extensionPart.DecreaseGroupSize((ECActivatableAbilityGroup)group);
                    return false;
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(UnitPartActivatableAbility), nameof(UnitPartActivatableAbility.GetGroupSize), new Type[] { typeof(ActivatableAbilityGroup) })]
        static class UnitPartActivatableAbility_GetGroupSize_Patch
        {

            static bool Prefix(UnitPartActivatableAbility __instance, ActivatableAbilityGroup group, ref int __result)
            {
                if (group.IsECGroup())
                {
                    var extensionPart = __instance.Owner.Ensure<UnitPartActivatableAbilityGroupEC>();
                    __result = extensionPart.GetGroupSize((ECActivatableAbilityGroup)group);
                    return false;
                }
                return true;
            }
        }
    }

    internal class UnitPartActivatableAbilityGroupEC : OldStyleUnitPart
    {
        public void IncreaseGroupSize(ECActivatableAbilityGroup group)
        {
            if (m_GroupsSizeIncreases.ContainsKey(group))
            {
                this.m_GroupsSizeIncreases[group] += 1;
            }
            else
            {
                m_GroupsSizeIncreases.Add(group, 1);
            }
        }

        public void DecreaseGroupSize(ECActivatableAbilityGroup group)
        {
            if (m_GroupsSizeIncreases.ContainsKey(group))
            {
                this.m_GroupsSizeIncreases[group] -= 1;
            }
        }

        public int GetGroupSize(ECActivatableAbilityGroup group)
        {
            this.m_GroupsSizeIncreases.TryGetValue(group, out int result);
            return result + 1;
        }

        private SortedDictionary<ECActivatableAbilityGroup, int> m_GroupsSizeIncreases = new SortedDictionary<ECActivatableAbilityGroup, int>();
    }
}
