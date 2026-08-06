using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Actions.Builder;
using EbonsContentMod.Components;
using EbonsContentMod.Utilities;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UI.ActionBar;
using Kingmaker.UI.MVVM._VM.ActionBar;
using Kingmaker.UI.UnitSettings;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using TabletopTweaks.Core.Utilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Blueprints.Classes;

namespace EbonsContentMod.Abilities
{
    internal static class MiracleWish
    {
        private const string MiracleName =
            "EbonsContentModMiracleAbility";

        private const string MiracleGuid =
            "355a9447-47b2-4ffc-8e2d-7759380e63b4";

        private const string MiracleNameKey =
            "EbonsContentMod.Miracle.Name";

        private const string MiracleCleanseNameKey =
            "EbonsContentMod.Miracle.RemoveNegativeEffects.Name";

        private const string MiracleDescriptionKey =
            "EbonsContentMod.Miracle.Description";

        private static readonly string[] LevelAbilityGuids =
        {
            null,
            "8eb3f54d-ba5b-4065-b043-dc1c3fb656d8",
            "a409fa6f-0c44-4180-aa4b-a09681ea5c2f",
            "f88a29e2-e475-49bc-a176-ca789e4e62ee",
            "67bcc1a0-b531-444d-a46f-dc6686fc716d",
            "cff7d8ec-e536-41b2-af9d-8eee3826ef30",
            "3e8d6d3d-a560-450d-a848-4c0c0225f07a",
            "82694583-078e-48d0-934b-5e79e9d877dc",
            "937fe725-1312-4dba-ba2e-1f970f82db7d"
        };

        private static readonly Metamagic[] AvailableMetamagic =
        {
            Metamagic.Quicken,
            Metamagic.Heighten,
            Metamagic.Empower,
            Metamagic.Extend,
            Metamagic.Maximize,
            Metamagic.Reach,
            Metamagic.CompletelyNormal,
            Metamagic.Persistent,
            Metamagic.Bolstered,
            Metamagic.Selective,
            Metamagic.Intensified,
            Metamagic.Piercing
        };

        #region Blueprint creation

        /// <summary>
        /// Creates this hierarchy:
        ///
        /// Miracle
        /// ├── Miracle — 1st-Level Spells
        /// ├── Miracle — 2nd-Level Spells
        /// ├── ...
        /// └── Miracle — 8th-Level Spells
        ///
        /// Each static level selector contains AbilityDuplicateSpell and
        /// dynamically supplies only the spells assigned to that level.
        /// </summary>
        internal static BlueprintAbility Configure()
        {
            Main.log.Log("[Miracle] Entering Configure().");

            try
            {
                if (BlueprintTool.TryGet<BlueprintAbility>(
                        MiracleGuid,
                        out BlueprintAbility existingMiracle))
                {
                    Main.log.Warning(
                        $"[Miracle] Blueprint already exists: " +
                        $"{existingMiracle.name}, " +
                        $"{existingMiracle.AssetGuid}");

                    return existingMiracle;
                }

                BlueprintFeature restorationGreater =
                    FeatureRefs.DomainMastery
                        .Reference
                        .Get();

                var icon =
                    restorationGreater?.Icon;

                BlueprintSpellListReference clericSpellList =
                    SpellListRefs.ClericSpellList
                        .Cast<BlueprintSpellListReference>()
                        .Reference;

                BlueprintSpellListReference[] otherSpellLists =
                {
                    SpellListRefs.BardSpellList
                        .Cast<BlueprintSpellListReference>()
                        .Reference,

                    SpellListRefs.BloodragerSpellList
                        .Cast<BlueprintSpellListReference>()
                        .Reference,

                    SpellListRefs.DruidSpellList
                        .Cast<BlueprintSpellListReference>()
                        .Reference,

                    SpellListRefs.HunterSpelllist
                        .Cast<BlueprintSpellListReference>()
                        .Reference,

                    SpellListRefs.InquisitorSpellList
                        .Cast<BlueprintSpellListReference>()
                        .Reference,

                    SpellListRefs.MagusSpellList
                        .Cast<BlueprintSpellListReference>()
                        .Reference,

                    SpellListRefs.PaladinSpellList
                        .Cast<BlueprintSpellListReference>()
                        .Reference,

                    SpellListRefs.RangerSpellList
                        .Cast<BlueprintSpellListReference>()
                        .Reference,

                    SpellListRefs.ShamanSpelllist
                        .Cast<BlueprintSpellListReference>()
                        .Reference,

                    SpellListRefs.WarpriestSpelllist
                        .Cast<BlueprintSpellListReference>()
                        .Reference,

                    SpellListRefs.WitchSpellList
                        .Cast<BlueprintSpellListReference>()
                        .Reference,

                    SpellListRefs.WizardSpellList
                        .Cast<BlueprintSpellListReference>()
                        .Reference
                };

                var prohibitedSpells = new List<BlueprintAbilityReference>
                {
                    AbilityRefs.ShadowConjuration
                        .Reference
                        .Get()
                        .ToReference<BlueprintAbilityReference>(),

                    AbilityRefs.Shades
                        .Reference
                        .Get()
                        .ToReference<BlueprintAbilityReference>(),

                    AbilityRefs.ShadowConjurationGreater
                        .Reference
                        .Get()
                        .ToReference<BlueprintAbilityReference>(),

                    AbilityRefs.ShadowConjurationDoublesMghtyAnkou
                        .Reference
                        .Get()
                        .ToReference<BlueprintAbilityReference>(),

                    AbilityRefs.ShadowEvocation
                        .Reference
                        .Get()
                        .ToReference<BlueprintAbilityReference>(),

                    AbilityRefs.ShadowEvocationGreater
                        .Reference
                        .Get()
                        .ToReference<BlueprintAbilityReference>()
                };

                if (CheckerUtilities.GetModActive(
                        "TabletopTweaks-Base"))
                {
                    prohibitedSpells.Add(
                        BlueprintTools
                            .GetBlueprintReference<
                                BlueprintAbilityReference>(
                                "d934f706-a12b-40ec-87a9-c8baf221b8a9"));

                    prohibitedSpells.Add(
                        BlueprintTools
                            .GetBlueprintReference<
                                BlueprintAbilityReference>(
                                "ba079628-2748-4eb3-8bf0-b6aadd9f5f22"));
                }

                /*
                 * The root spell has only the eight ordinary static variants.
                 * AbilityDuplicateSpell belongs on the level selectors, not
                 * on the root Miracle blueprint.
                 */
                BlueprintAbility miracle =
                    AbilityConfigurator
                        .NewSpell(
                            MiracleName,
                            MiracleGuid,
                            SpellSchool.Evocation,
                            canSpecialize: false)
                        .SetDisplayName(MiracleNameKey)
                        .SetDescription(MiracleDescriptionKey)
                        .SetIcon(icon)
                        .SetActionType(
                            UnitCommand.CommandType.Standard)
                        .SetRange(AbilityRange.Personal)
                        .SetCanTargetSelf()
                        .SetNotOffensive()
                        .SetShowNameForVariant(false)
                        .SetAvailableMetamagic(
                            AvailableMetamagic)
                        .AddToSpellLists(
                            9,
                            SpellList.Cleric)
                        .Configure();

                BlueprintAbility removeNegativeEffects =
                    AbilityConfigurator
                        .NewSpell(
                            "EbonsContentModMiracleRemoveNegativeEffectsAbility",
                            "d8ab8fac-44bc-4bc8-b69b-beff3735838d",
                            SpellSchool.Evocation,
                            canSpecialize: false)
                        .SetDisplayName(MiracleCleanseNameKey)
                        .SetDescription(MiracleDescriptionKey)
                        .SetIcon(icon)
                        .SetParent(miracle)
                        .SetActionType(
                            UnitCommand.CommandType.Standard)
                        .SetRange(
                            AbilityRange.Close)
                        .SetCanTargetSelf()
                        .SetCanTargetFriends()
                        .SetNotOffensive()
                        .SetShowNameForVariant(false)
                        .SetActionBarAutoFillIgnored()
                        .SetAvailableMetamagic(
                            AvailableMetamagic)
                        .AddAbilityEffectRunAction(
                            actions: ActionsBuilder
                                .New()
                                .Add<ContextActionRemoveAllNegativeEffects>())
                        .AddAbilitySpawnFx(AbilitySpawnFxAnchor.SelectedTarget, 0.0f, false, AbilitySpawnFxAnchor.None, AbilitySpawnFxOrientation.Copy, AbilitySpawnFxAnchor.None, "621885df4b6add9489c8edd14b844ad6", AbilitySpawnFxTime.OnApplyEffect, AbilitySpawnFxWeaponTarget.None)
                        .Configure();

                Main.log.Log(
                    $"[Miracle] Root created: " +
                    $"{miracle.name}, {miracle.AssetGuid}");

                var levelVariants =
                    new List<
                        Blueprint<BlueprintAbilityReference>>(
                        capacity: 8);

                for (int level = 1; level <= 8; level++)
                {
                    int selectedLevel =
                        level;

                    string levelAbilityName =
                        GetLevelAbilityName(selectedLevel);

                    string levelAbilityGuid =
                        LevelAbilityGuids[selectedLevel];

                    BlueprintAbility levelAbility =
                        AbilityConfigurator
                            .NewSpell(
                                levelAbilityName,
                                levelAbilityGuid,
                                SpellSchool.Evocation,
                                canSpecialize: false)
                            .SetDisplayName(
                                GetLevelNameKey(selectedLevel))
                            .SetDescription(
                                MiracleDescriptionKey)
                            .SetIcon(icon)
                            .SetParent(miracle)
                            .SetActionType(
                                UnitCommand.CommandType.Standard)
                            .SetRange(AbilityRange.Personal)
                            .SetCanTargetSelf()
                            .SetNotOffensive()
                            .SetShowNameForVariant(false)
                            .SetActionBarAutoFillIgnored()
                            .SetAvailableMetamagic(
                                AvailableMetamagic)
                            .AddComponent<AbilityDuplicateSpell>(
                                component =>
                                {
                                    component.SelectedSpellLevel =
                                        selectedLevel;

                                    component.m_PrimarySpellList =
                                        clericSpellList;

                                    component.PrimaryMaxSpellLevel =
                                        8;

                                    component.m_OtherSpellLists =
                                        otherSpellLists.ToArray();

                                    component.OtherMaxSpellLevel =
                                        7;

                                    component.m_ExcludedSpells =
                                        prohibitedSpells.ToArray();

                                    component.IncludeCantrips =
                                        false;

                                    component
                                        .ExcludeSpellsWithResourceLogic =
                                        true;

                                    component.FlattenVariants =
                                        true;

                                    component.UseSourceAbilityDC =
                                        true;

                                    component.UseSourceActionType =
                                        true;

                                    component
                                        .IgnoreMaterialComponentCostUpTo =
                                        100;
                                })
                            .Configure();

                    levelVariants.Add(levelAbility);

                    Main.log.Log(
                        $"[Miracle] Level {selectedLevel} " +
                        $"created: {levelAbility.AssetGuid}");
                }

                levelVariants.Add(removeNegativeEffects);

                AbilityConfigurator
                    .For(miracle)
                    .AddAbilityVariants(levelVariants)
                    .Configure();

                Main.log.Log(
                    $"[Miracle] Finished with " +
                    $"{levelVariants.Count} level variants.");

                return miracle;
            }
            catch (Exception ex)
            {
                Main.log.Error(
                    "[Miracle] Configure failed:\n" + ex);

                throw;
            }
        }

        private static string GetLevelAbilityName(
            int level)
        {
            return $"MiracleSpellLevel{level}";
        }

        private static string GetLevelNameKey(
            int level)
        {
            return
                $"EbonsContentMod.Miracle.Level{level}.Name";
        }

        #endregion

        #region AbilityData patches

        /// <summary>
        /// Marks abilities containing AbilityDuplicateSpell as variable so
        /// other UI and availability checks recognize that they have runtime
        /// conversions.
        /// </summary>
        [HarmonyPatch(
            typeof(AbilityData),
            nameof(AbilityData.IsVariable),
            MethodType.Getter)]
        private static class
            AbilityData_IsVariable_Patch
        {
            [HarmonyPostfix]
            private static void Postfix(
                AbilityData __instance,
                ref bool __result)
            {
                if (__result)
                    return;

                __result =
                    __instance.Blueprint
                        ?.GetComponent<AbilityDuplicateSpell>()
                    != null;
            }
        }

        /// <summary>
        /// Supplies the cached runtime conversions for a level selector.
        /// The root Miracle blueprint has no AbilityDuplicateSpell, so its
        /// ordinary static AbilityVariants continue through Owlcat's method.
        /// </summary>
        [HarmonyPatch(
            typeof(AbilityData),
            nameof(AbilityData.GetConversions))]
        private static class
            AbilityData_GetConversions_Patch
        {
            [HarmonyPrefix]
            private static bool Prefix(
                AbilityData __instance,
                ref IEnumerable<AbilityData> __result)
            {
                AbilityDuplicateSpell provider =
                    __instance.Blueprint
                        ?.GetComponent<AbilityDuplicateSpell>();

                if (provider == null)
                    return true;

                __result =
                    provider.GetOrCreateConversions(
                        __instance);

                return false;
            }
        }

        /// <summary>
        /// Walks through every ConvertedFrom link to the actual spellbook
        /// ability that began the conversion chain.
        ///
        /// For a duplicated Miracle spell the normal chain is:
        ///
        /// duplicated spell -> level selector -> Miracle
        ///
        /// The intermediate selector can temporarily report incomplete
        /// runtime timing data while its popup is being constructed. The root
        /// Miracle AbilityData has the authoritative action type, metamagic,
        /// and full-round status.
        /// </summary>
        private static AbilityData GetRootConversionSource(
            AbilityData ability)
        {
            AbilityData current =
                ability;

            /*
             * Conversion chains are expected to be only a few entries long.
             * The depth guard also protects against a malformed circular
             * ConvertedFrom chain.
             */
            for (int depth = 0;
                 depth < 16 && current != null;
                 depth++)
            {
                AbilityData parent =
                    current.ConvertedFrom;

                if (parent == null
                    || ReferenceEquals(parent, current))
                {
                    break;
                }

                current =
                    parent;
            }

            return current;
        }

        /// <summary>
        /// Uses the ultimate duplicating spell's action type for the
        /// substituted spell.
        /// </summary>
        [HarmonyPatch(
            typeof(AbilityData),
            nameof(AbilityData.ActionType),
            MethodType.Getter)]
        private static class
            AbilityData_ActionType_Patch
        {
            [HarmonyPostfix]
            private static void Postfix(
                AbilityData __instance,
                ref UnitCommand.CommandType __result)
            {
                if (!AbilityDuplicateSpell
                        .TryGetConversionSource(
                            __instance,
                            out AbilityData source,
                            out AbilityDuplicateSpell provider))
                {
                    return;
                }

                if (!provider.UseSourceActionType)
                    return;

                AbilityData rootSource =
                    GetRootConversionSource(source);

                if (rootSource != null)
                {
                    __result =
                        rootSource.ActionType;
                }
            }
        }

        /// <summary>
        /// ActionType and full-round status are separate AbilityData
        /// properties, so preserve the source spell's full-round status too.
        /// </summary>
        [HarmonyPatch(
            typeof(AbilityData),
            nameof(AbilityData.RequireFullRoundAction),
            MethodType.Getter)]
        private static class
            AbilityData_RequireFullRoundAction_Patch
        {
            [HarmonyPostfix]
            private static void Postfix(
                AbilityData __instance,
                ref bool __result)
            {
                if (!AbilityDuplicateSpell
                        .TryGetConversionSource(
                            __instance,
                            out AbilityData source,
                            out AbilityDuplicateSpell provider))
                {
                    return;
                }

                if (!provider.UseSourceActionType)
                    return;

                AbilityData rootSource =
                    GetRootConversionSource(source);

                if (rootSource != null)
                {
                    __result =
                        rootSource.RequireFullRoundAction;
                }
            }
        }

        /// <summary>
        /// Suppresses the duplicated spell's material component when its
        /// value falls within the Miracle/Wish provider's threshold.
        /// </summary>
        [HarmonyPatch(
            typeof(AbilityData),
            nameof(AbilityData.RequireMaterialComponent),
            MethodType.Getter)]
        private static class
            AbilityData_RequireMaterialComponent_Patch
        {
            [HarmonyPostfix]
            private static void Postfix(
                AbilityData __instance,
                ref bool __result)
            {
                if (!__result)
                    return;

                if (!AbilityDuplicateSpell
                        .TryGetConversionSource(
                            __instance,
                            out _,
                            out AbilityDuplicateSpell provider))
                {
                    return;
                }

                if (provider.ShouldIgnoreMaterialComponent(
                        __instance.Blueprint))
                {
                    __result =
                        false;
                }
            }
        }

        #endregion

        #region MVVM nested conversion menu patch

        private static bool s_ActionBarSlotVMMainPatchApplied;
        private static bool s_ActionBarSlotVMShowConvertPatchApplied;

        /*
         * The current Wrath UI displays action-bar conversions through
         * ActionBarSlotVM.
         *
         * ActionBarSlotVM.OnMainClick() does not treat
         * MechanicActionBarSlotSpontaneusConvertedSpell as a slot that can
         * open another conversion menu. We add that missing branch for the
         * Miracle level selectors.
         *
         * The small conversion arrow invokes OnShowConvertRequest() directly,
         * whereas clicking the square icon reaches it through OnMainClick().
         * Recording the Miracle root from both paths makes either control work
         * on the first click and after action-bar or area UI reconstruction.
         */

        private static ActionBarSlotVM s_MiracleRootSlot;
        private static ActionBarConvertedVM s_MiracleRootMenu;

        internal static void ApplyNestedMenuUIPatches(
            Harmony harmony)
        {
            if (harmony == null)
            {
                Main.log.Error(
                    "[Miracle UI] Harmony instance was null.");

                return;
            }

            PatchActionBarSlotVMMainClick(harmony);
            PatchActionBarSlotVMShowConvertRequest(harmony);
        }

        private static void PatchActionBarSlotVMMainClick(
            Harmony harmony)
        {
            if (s_ActionBarSlotVMMainPatchApplied)
                return;

            MethodInfo method =
                AccessTools.Method(
                    typeof(ActionBarSlotVM),
                    nameof(ActionBarSlotVM.OnMainClick),
                    Type.EmptyTypes);

            if (method == null)
            {
                Main.log.Error(
                    "[Miracle UI] Could not locate " +
                    "ActionBarSlotVM.OnMainClick().");

                return;
            }

            try
            {
                harmony.Patch(
                    method,
                    prefix: new HarmonyMethod(
                        AccessTools.Method(
                            typeof(MiracleWish),
                            nameof(
                                ActionBarSlotVMOnMainClickPrefix))),
                    postfix: new HarmonyMethod(
                        AccessTools.Method(
                            typeof(MiracleWish),
                            nameof(
                                ActionBarSlotVMOnMainClickPostfix))));

                s_ActionBarSlotVMMainPatchApplied =
                    true;

                Main.log.Log(
                    "[Miracle UI] Patched MVVM main click: " +
                    DescribeMethod(method));
            }
            catch (Exception ex)
            {
                Main.log.Error(
                    "[Miracle UI] Failed to patch MVVM main click:\n" +
                    ex);
            }
        }

        private static void PatchActionBarSlotVMShowConvertRequest(
            Harmony harmony)
        {
            if (s_ActionBarSlotVMShowConvertPatchApplied)
                return;

            MethodInfo method =
                AccessTools.Method(
                    typeof(ActionBarSlotVM),
                    nameof(
                        ActionBarSlotVM.OnShowConvertRequest),
                    Type.EmptyTypes);

            if (method == null)
            {
                Main.log.Error(
                    "[Miracle UI] Could not locate " +
                    "ActionBarSlotVM.OnShowConvertRequest().");

                return;
            }

            try
            {
                harmony.Patch(
                    method,
                    postfix: new HarmonyMethod(
                        AccessTools.Method(
                            typeof(MiracleWish),
                            nameof(
                                ActionBarSlotVMOnShowConvertRequestPostfix))));

                s_ActionBarSlotVMShowConvertPatchApplied =
                    true;

                Main.log.Log(
                    "[Miracle UI] Patched MVVM conversion request: " +
                    DescribeMethod(method));
            }
            catch (Exception ex)
            {
                Main.log.Error(
                    "[Miracle UI] Failed to patch MVVM conversion " +
                    "request:\n" + ex);
            }
        }

        /// <summary>
        /// Handles a level selector before ActionBarSlotVM falls through to
        /// MechanicActionBarSlotSpontaneusConvertedSpell.OnClick().
        /// </summary>
        private static bool ActionBarSlotVMOnMainClickPrefix(
            ActionBarSlotVM __instance)
        {
            if (__instance == null)
                return true;

            var selectorMechanic =
                __instance.MechanicActionBarSlot
                as MechanicActionBarSlotSpontaneusConvertedSpell;

            AbilityData levelAbility =
                selectorMechanic?.Spell;

            AbilityDuplicateSpell provider =
                levelAbility?.Blueprint
                    ?.GetComponent<AbilityDuplicateSpell>();

            /*
             * Ordinary conversion entries and final duplicated spells should
             * continue through Owlcat's original click handler.
             */
            if (provider == null)
                return true;

            try
            {
                Main.log.Log(
                    $"[Miracle UI] MVVM selector click: " +
                    $"{levelAbility.Blueprint.name}, " +
                    $"level={provider.SelectedSpellLevel}.");

                ActionBarSlotVM rootSlot =
                    s_MiracleRootSlot;

                if (rootSlot == null)
                {
                    Main.log.Error(
                        "[Miracle UI] No root Miracle ActionBarSlotVM " +
                        "was recorded.");

                    return false;
                }

                ActionBarConvertedVM currentMenu =
                    rootSlot.ConvertedVm.Value;

                if (currentMenu == null)
                {
                    Main.log.Error(
                        "[Miracle UI] The recorded Miracle root slot " +
                        "no longer has an open conversion menu.");

                    return false;
                }

                bool selectorBelongsToCurrentMenu =
                    currentMenu.Slots.Any(
                        slot => ReferenceEquals(
                            slot,
                            __instance));

                if (!selectorBelongsToCurrentMenu)
                {
                    Main.log.Error(
                        "[Miracle UI] The clicked selector was not in " +
                        "the recorded Miracle root menu.");

                    return false;
                }

                List<AbilityData> conversions =
                    provider
                        .GetOrCreateConversions(levelAbility)
                        .Where(
                            ability =>
                                ability != null
                                && ability.IsVisible())
                        .ToList();

                Main.log.Log(
                    $"[Miracle UI] Level " +
                    $"{provider.SelectedSpellLevel} produced " +
                    $"{conversions.Count} visible spells.");

                if (conversions.Count == 0)
                {
                    Main.log.Warning(
                        $"[Miracle UI] " +
                        $"{levelAbility.Blueprint.name} produced no " +
                        "visible conversions.");

                    return false;
                }

                /*
                 * Resolve timing now, while the complete conversion chain is
                 * intact. This also ensures every tooltip created below sees
                 * the root Miracle action type immediately.
                 */
                foreach (AbilityData conversion in conversions)
                {
                    UnitCommand.CommandType ignoredActionType =
                        conversion.ActionType;

                    bool ignoredFullRound =
                        conversion.RequireFullRoundAction;
                }

                UnitEntityData unit =
                    selectorMechanic.Unit;

                if (unit == null)
                {
                    Main.log.Error(
                        $"[Miracle UI] " +
                        $"{levelAbility.Blueprint.name} had no unit.");

                    return false;
                }

                List<MechanicActionBarSlot> mechanicSlots =
                    new SlotConversion(conversions)
                        .GetMechanicSlots(unit)
                        .ToList();

                if (mechanicSlots.Count == 0)
                {
                    Main.log.Error(
                        $"[Miracle UI] " +
                        $"{levelAbility.Blueprint.name} produced no " +
                        "mechanic slots.");

                    return false;
                }

                /*
                 * Construct the replacement before disposing the old menu.
                 * Closing the old menu disposes this selector VM, but all
                 * required state has already been copied to local variables.
                 */
                var replacementMenu =
                    new ActionBarConvertedVM(
                        mechanicSlots,
                        rootSlot.CloseConvert);

                /*
                 * Owlcat normally refreshes child slots on the next LateUpdate.
                 * Refresh them immediately so action timing, resources, and
                 * availability are correct on the first rendered frame.
                 */
                foreach (ActionBarSlotVM slot in
                         replacementMenu.Slots)
                {
                    slot.UpdateResource();
                }

                rootSlot.CloseConvert();
                rootSlot.ConvertedVm.Value =
                    replacementMenu;

                /*
                 * The currently open menu is now the leaf-spell menu rather
                 * than the root selector menu. A later arrow or square click
                 * will reopen and record a fresh root menu.
                 */
                s_MiracleRootMenu =
                    null;

                Main.log.Log(
                    $"[Miracle UI] Replaced the Miracle level menu " +
                    $"with {mechanicSlots.Count} level-" +
                    $"{provider.SelectedSpellLevel} spell entries.");

                /*
                 * Suppress Owlcat's original OnMainClick(), which would call
                 * the selector mechanic's OnClick() and attempt to cast the
                 * empty selector ability.
                 */
                return false;
            }
            catch (Exception ex)
            {
                Main.log.Error(
                    "[Miracle UI] MVVM selector handling failed:\n" +
                    ex);

                return false;
            }
        }

        /// <summary>
        /// Fallback for the square-icon route. OnMainClick normally reaches
        /// OnShowConvertRequest(), but keeping this postfix also protects
        /// against UI paths that open the conversion menu internally.
        /// </summary>
        private static void ActionBarSlotVMOnMainClickPostfix(
            ActionBarSlotVM __instance)
        {
            TryRecordMiracleRootSlot(
                __instance);
        }

        /// <summary>
        /// Handles the small conversion-arrow route directly.
        /// </summary>
        private static void
            ActionBarSlotVMOnShowConvertRequestPostfix(
                ActionBarSlotVM __instance)
        {
            TryRecordMiracleRootSlot(
                __instance);
        }

        /// <summary>
        /// Identifies an opened eight-entry Miracle level menu and remembers
        /// the root ActionBarSlotVM that owns it.
        /// </summary>
        private static void TryRecordMiracleRootSlot(
            ActionBarSlotVM slot)
        {
            if (slot == null)
                return;

            try
            {
                ActionBarConvertedVM menu =
                    slot.ConvertedVm.Value;

                if (menu == null)
                    return;

                int selectorCount =
                    menu.Slots.Count(
                        child =>
                        {
                            var mechanic =
                                child?.MechanicActionBarSlot
                                as MechanicActionBarSlotSpontaneusConvertedSpell;

                            return mechanic?.Spell?.Blueprint
                                ?.GetComponent<AbilityDuplicateSpell>()
                                != null;
                        });

                if (selectorCount == 0)
                    return;

                /*
                 * Both OnShowConvertRequest and OnMainClick may observe the
                 * same newly opened menu. Avoid duplicate log noise.
                 */
                if (ReferenceEquals(
                        s_MiracleRootSlot,
                        slot)
                    && ReferenceEquals(
                        s_MiracleRootMenu,
                        menu))
                {
                    return;
                }

                s_MiracleRootSlot =
                    slot;

                s_MiracleRootMenu =
                    menu;

                Main.log.Log(
                    $"[Miracle UI] Recorded MVVM Miracle root menu " +
                    $"with {selectorCount} selectors.");
            }
            catch (Exception ex)
            {
                Main.log.Error(
                    "[Miracle UI] Failed to inspect the opened MVVM " +
                    "conversion menu:\n" + ex);
            }
        }

        private static string DescribeMethod(
            MethodBase method)
        {
            if (method == null)
                return "<null>";

            string parameterList =
                string.Join(
                    ", ",
                    method
                        .GetParameters()
                        .Select(parameter =>
                            parameter.ParameterType.FullName));

            return
                $"{method.DeclaringType?.FullName}." +
                $"{method.Name}({parameterList})";
        }

        #endregion
    }
}