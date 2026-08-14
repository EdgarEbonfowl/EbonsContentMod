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
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.Utility;
using Kingmaker.Blueprints.Items;
using Kingmaker.UnitLogic.Mechanics.Components;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using BlueprintCore.Utils.Types;
using BlueprintCore.Actions.Builder.AVEx;
using Kingmaker.UnitLogic;
using Kingmaker.Craft;
using Kingmaker.View.Animation;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.RuleSystem.Rules;

namespace EbonsContentMod.Spells
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
        
        private const string MiracleRaiseTheFallenNameKey =
            "EbonsContentMod.Miracle.RaiseTheFallen.Name";

        private const string MiracleRenewalNameKey =
            "EbonsContentMod.Miracle.Renewal.Name";

        private const string MiracleDescriptionKey =
            "EbonsContentMod.Miracle.Description";

        const string wishName =
                "EbonsContentModWishAbility";

        const string wishGuid =
            "f2c67a37-d726-4fd7-a242-50ed1e0ff184";

        const string wishNameKey =
            "EbonsContentMod.Wish.Name";

        const string wishDescriptionKey =
            "EbonsContentMod.Wish.Description";

        const string wishAbilityScoresNameKey =
            "EbonsContentMod.Wish.AbilityScores.Name";

        const string wishRemoveAfflictionsNameKey =
            "EbonsContentMod.Wish.RemoveAfflictions.Name";

        const string wishHealAlliesNameKey =
            "EbonsContentMod.Wish.HealAllies.Name";

        const string limitedWishName =
            "EbonsContentLimitedModWishAbility";

        const string limitedWishGuid =
            "{A6ABFD65-5666-403C-A78B-072D6D0509D6}";

        const string limitedWishNameKey =
            "EbonsContentMod.LimitedWish.Name";

        const string limitedWishDescriptionKey =
            "EbonsContentMod.LimitedWish.Description";

        const string limitedWishRemoveAfflictionsNameKey =
            "EbonsContentMod.LimitedWish.RemoveAfflictions.Name";

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

        private static readonly string[] wishLevelGuids =
        {
                null,
                "16096ebf-c90b-463a-82c4-e587dc61cc57",
                "a2701100-7f81-430e-bb43-fae4e7c6732d",
                "9fae2634-f85d-4fa0-8a0a-a1ba10589bc1",
                "9836cd37-1c27-4891-8266-9709455a9389",
                "e8ede857-fa83-4cbd-9576-706e0b522621",
                "45727b35-cfc8-4ee5-bb54-221b8b34b5ee",
                "665cca33-4e19-4891-89b8-7de298d8a564",
                "63d267b2-0ec5-4a08-9501-6922b158f264"
            };

        private static readonly string[][] inherentRankGuids =
        {
                new[]
                {
                    "2a9250bd-4cc7-4df5-a520-1bef3b85dfaa",
                    "f77fa1e8-7d0b-4e2d-bc10-1c9b1e128a4c",
                    "4da35e27-0634-4e36-b086-f190214b3dc3",
                    "186e4692-2fd7-4bd8-b502-84486ce199f0",
                    "29f55aee-fc22-4edb-a0cf-077a1c076d1c"
                },
                new[]
                {
                    "c3326a9b-d081-44e6-bb91-4145cbf917c0",
                    "34b20de1-06ec-4f4f-bd79-7d4a3de3ef7b",
                    "066c439a-75b8-4ace-b444-b00c46fd6d59",
                    "2b517991-b30b-42d1-9c89-5ccf52932710",
                    "d15a6add-be6e-4e52-adf0-965e43905832"
                },
                new[]
                {
                    "74d3ebde-bc39-45df-8c25-ec68b3a3b790",
                    "24374c5b-b10b-4305-ae03-bb1250d97083",
                    "d5cc7d5d-5560-4eef-8e69-f882eb17347d",
                    "4cb26daf-7d48-4db7-ba83-be103a486674",
                    "2f12cd61-a3bf-4229-8aa6-993369972348"
                },
                new[]
                {
                    "cf6068b9-6ef5-4c17-85fc-6c1f71354d5c",
                    "bd3787af-182b-4349-ae2b-15494fa7f052",
                    "3f451a93-bc9d-4176-a4a0-456064bab0a6",
                    "6507fbb3-4fac-4c6d-bed3-87f17cdd4165",
                    "5c7bdc7f-6e41-4dd1-98e1-c18132a9f2e3"
                },
                new[]
                {
                    "a575b626-7fd9-4417-b35d-2d204dbfc7ed",
                    "a873c3ee-7528-4740-9fd3-c948e24a6a2e",
                    "2cb9c527-ac01-4a73-934f-37df317bf5b1",
                    "fab70887-6e67-43a6-b7d9-1d2cdbc68dfa",
                    "d933e874-7a21-4e1f-9554-a537e98aaa15"
                },
                new[]
                {
                    "04b765b6-03ad-4b2b-9b3f-9abb4bd39487",
                    "da1b7461-59f7-4990-916f-bbe0ff7df986",
                    "8b6f2387-901b-490c-87d2-79474cc20b48",
                    "c84cd3f7-8a10-4f9d-a714-4349a9ec241b",
                    "8ce7240f-0f28-4f0e-b5b4-282e289c360d"
                }
            };

        private static readonly string[] statAbilityGuids =
        {
                "b8e578e2-8685-4b1c-a5c2-a183aee9a1a7",
                "724b3af4-5cf5-4c60-be0d-ddd2aaaaa5c0",
                "7d063de3-07cc-4e2e-bf27-905b3af6d5cf",
                "422a10a8-7607-4235-856f-bbd242c45212",
                "2cd25f4e-69f0-403f-bb63-679ef85b65b3",
                "8f6c3ef2-00f6-47bc-8739-de1f01e88bef"
            };

        private static readonly StatType[] stats =
        {
                StatType.Strength,
                StatType.Dexterity,
                StatType.Constitution,
                StatType.Intelligence,
                StatType.Wisdom,
                StatType.Charisma
            };

        private static readonly string[] statNames =
        {
                "Strength",
                "Dexterity",
                "Constitution",
                "Intelligence",
                "Wisdom",
                "Charisma"
            };

        private static readonly string[] statNameKeys =
        {
                "EbonsContentMod.Wish.Strength.Name",
                "EbonsContentMod.Wish.Dexterity.Name",
                "EbonsContentMod.Wish.Constitution.Name",
                "EbonsContentMod.Wish.Intelligence.Name",
                "EbonsContentMod.Wish.Wisdom.Name",
                "EbonsContentMod.Wish.Charisma.Name"
            };

        private static readonly string[] limitedWishLevelGuids =
        {
                null,
                "{B6B99FE1-D64D-40E4-9C29-E8C1A6FA48D3}",
                "{3865F875-3306-4F1F-BE5F-3469203CEC65}",
                "{9C196388-9B10-4C38-B90C-2DC78181F232}",
                "{949E38B4-7D60-427B-BDD9-0EAF5BEBE562}",
                "{6D8BC375-E056-4CF3-8541-B7F2FF25A071}",
                "{785D3228-DD53-44D7-BEAF-6D804CF9D305}"
            };

        #region Blueprint creation

        /// <summary>
        /// Creates this hierarchy:
        ///
        /// Miracle
        /// ├── Miracle — 1st-Level Spells
        /// ├── Miracle — 2nd-Level Spells
        /// ├── ...
        /// ├── Miracle — 8th-Level Spells
        /// └── Miracle — Other Effects
        ///
        /// Each static level selector contains AbilityDuplicateSpell and
        /// dynamically supplies only the spells assigned to that level.
        /// </summary>
        internal static void Configure()
        {
            if (Main.Settings.Miracle)
            {
                CreateMiracle();
            }

            if (Main.Settings.Wish)
            {
                CreateWish();
            }

            if (Main.Settings.LimitedWish)
            {
                CreateLimitedWish();
            }
        }

        internal static void ExportSprites()
        {
            var miracleSprite = BlueprintTools.GetBlueprint<BlueprintAbility>(MiracleGuid).Icon;
            var wishSprite = BlueprintTools.GetBlueprint<BlueprintAbility>(wishGuid).Icon;
            var limitedWishSprite = BlueprintTools.GetBlueprint<BlueprintAbility>(limitedWishGuid).Icon;

            var wishScrollSprite = ItemEquipmentUsableRefs.ScrollOfGeniekindDjinni.Reference.Get().Icon;

            SpriteHelperators.ExportSprite(wishSprite, "WishSprite");
            SpriteHelperators.ExportSprite(limitedWishSprite, "LimitedWishSprite");
            SpriteHelperators.ExportSprite(wishScrollSprite, "WishScrollSprite");
            SpriteHelperators.ExportSprite(wishScrollSprite, "limitedWishScrollSprite");
        }

        internal static BlueprintAbility CreateWish()
        {
            

            Main.log.Log("[Wish] Entering CreateWish().");

            try
            {
                if (BlueprintTool.TryGet<BlueprintAbility>(
                        wishGuid,
                        out BlueprintAbility existingWish))
                {
                    Main.log.Warning(
                        $"[Wish] Blueprint already exists: " +
                        $"{existingWish.name}, {existingWish.AssetGuid}");

                    return existingWish;
                }

                var wishIcon = SpriteHelperators.LoadSprite(
                    "Sprites\\WishSprite.png");

                var wishSpellIcon = AbilityRefs.RemoveFear
                    .Reference
                    .Get()?.Icon;

                var wishStatBonusIcon = AbilityRefs.StunningBarrier
                    .Reference
                    .Get()?.Icon;

                // Reserved for the next Wish powers.
                var wishCleanseIcon = AbilityRefs.RemoveCurse
                    .Reference
                    .Get()?.Icon;

                var wishHealIcon = AbilityRefs.BreakEnchantment
                    .Reference
                    .Get()?.Icon;

                BlueprintItem diamonds =
                    BlueprintTools.GetBlueprint<BlueprintItem>(
                        "6a7cdeb14fc6ef44580cf639c5cdc113");

                BlueprintSpellListReference wizardSpellList =
                    SpellListRefs.WizardSpellList
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

                    SpellListRefs.ClericSpellList
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
                        .Reference
                };

                var prohibitedSpells =
                    new List<BlueprintAbilityReference>
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
                            .GetBlueprintReference<BlueprintAbilityReference>(
                                "d934f706-a12b-40ec-87a9-c8baf221b8a9"));

                    prohibitedSpells.Add(
                        BlueprintTools
                            .GetBlueprintReference<BlueprintAbilityReference>(
                                "ba079628-2748-4eb3-8bf0-b6aadd9f5f22"));
                }

                BlueprintAbility wish =
                    AbilityConfigurator
                        .NewSpell(
                            wishName,
                            wishGuid,
                            SpellSchool.Universalist,
                            canSpecialize: false)
                        .SetDisplayName(wishNameKey)
                        .SetDescription(wishDescriptionKey)
                        .SetIcon(wishIcon)
                        .SetActionType(
                            UnitCommand.CommandType.Standard)
                        .SetRange(AbilityRange.Personal)
                        .SetCanTargetSelf()
                        .SetNotOffensive()
                        .SetShowNameForVariant(false)
                        .SetAvailableMetamagic(
                            AvailableMetamagic)
                        .SetMaterialComponent(
                            new BlueprintAbility.MaterialComponentData
                            {
                                Count = 5,
                                m_Item = diamonds.ToReference<BlueprintItemReference>()
                            })
                        .AddToSpellLists(
                            9,
                            SpellList.Wizard)
                        .Configure();

                // Wish: Remove Afflictions

                BlueprintAbility removeAfflictions =
                    AbilityConfigurator
                        .NewSpell(
                            "EbonsContentModWishRemoveAfflictionsAbility",
                            "{7A821188-74FE-4524-94CA-90C250769E6D}",
                            SpellSchool.Universalist,
                            canSpecialize: false)
                        .CopyFrom(
                            AbilityRefs.HealMass,
                            c => c is not AbilityEffectRunAction or SpellListComponent or AbilityUseOnRest or SpellComponent)
                        .SetDisplayName(wishRemoveAfflictionsNameKey)
                        .SetDescription(wishDescriptionKey)
                        .SetIcon(wishCleanseIcon)
                        .AddSpellComponent(school: SpellSchool.Universalist)
                        .SetParent(wish)
                        .SetActionType(
                            UnitCommand.CommandType.Standard)
                        .SetRange(AbilityRange.Personal)
                        .SetCanTargetSelf()
                        .SetNotOffensive()
                        .SetShowNameForVariant(false)
                        .SetActionBarAutoFillIgnored()
                        .SetAvailableMetamagic(
                            AvailableMetamagic)
                        .AddAbilityEffectRunAction(
                            actions: ActionsBuilder
                                .New()
                                .Add<ContextActionOnPartyMembers>(
                                    action =>
                                    {
                                        action.Actions =
                                            ActionsBuilder
                                                .New()
                                                .Add<ContextActionRemoveAllNegativeEffects>()
                                                .HealStatDamage(ContextActionHealStatDamage.StatDamageHealType.HealAllDamage, ContextActionHealStatDamage.StatClass.Any, true)
                                                .HealStatDamage(ContextActionHealStatDamage.StatDamageHealType.HealAllDamage, ContextActionHealStatDamage.StatClass.Any, false)
                                                .DispelMagic(ContextActionDispelMagic.BuffType.All, RuleDispelMagic.CheckType.None, 0, descriptor: SpellDescriptor.StatDebuff)
                                                .HealEnergyDrain(EnergyDrainHealType.All, EnergyDrainHealType.All)
                                                .RemoveDeathDoor()
                                                .RemoveBuff(BuffRefs.Ecorche_Buff_SeizeSkin.Reference.Get())
                                                .RemoveBuff(BuffRefs.BythosAgeBuff1.Reference.Get())
                                                .RemoveBuff(BuffRefs.BythosAgeBuff2.Reference.Get())
                                                .RemoveBuff(BuffRefs.BythosAgeBuff3.Reference.Get())
                                                .RemoveBuff(BuffRefs.DLC3_ArcaneModDescription.Reference.Get())
                                                .RemoveBuff(BuffRefs.DLC3_HasteIslandStacks.Reference.Get())
                                                .RemoveBuff(BuffRefs.DLC3_HasteIslandAge1.Reference.Get())
                                                .RemoveBuff(BuffRefs.DLC3_HasteIslandAge2.Reference.Get())
                                                .RemoveBuff(BuffRefs.DLC3_HasteIslandAge3.Reference.Get())
                                                .SpawnFx("4d48e7ee3db59444d9b1dca869989b94") // True Form fx
                                                .Build();
                                    }))
                        .AddAbilitySpawnFx(anchor: AbilitySpawnFxAnchor.Caster, time: AbilitySpawnFxTime.OnStart, prefabLink: "96833db7343822040ada9d039f2fe738") // Zone Of Predetermination Fx
                        .AddAbilityDeliverDelay(0.3f)
                        .SetMaterialComponent(
                            new BlueprintAbility.MaterialComponentData
                            {
                                Count = 5,
                                m_Item = diamonds.ToReference<BlueprintItemReference>()
                            })
                        .Configure();

                // Wish: Heal Allies

                BlueprintAbility healAllies =
                    AbilityConfigurator
                        .NewSpell(
                            "EbonsContentModWishHealAlliesAbility",
                            "{4397AB0B-0C43-443F-BDC3-CEE4B2F174CE}",
                            SpellSchool.Universalist,
                            canSpecialize: false)
                        .CopyFrom(
                            AbilityRefs.HealMass,
                            c => c is not AbilityEffectRunAction or SpellListComponent or AbilityUseOnRest or SpellComponent)
                        .SetDisplayName(wishHealAlliesNameKey)
                        .SetDescription(wishDescriptionKey)
                        .SetIcon(wishHealIcon)
                        .AddSpellComponent(school: SpellSchool.Universalist)
                        .SetParent(wish)
                        .SetActionType(
                            UnitCommand.CommandType.Standard)
                        .SetRange(AbilityRange.Personal)
                        .SetCanTargetSelf()
                        .SetNotOffensive()
                        .SetShowNameForVariant(false)
                        .SetActionBarAutoFillIgnored()
                        .SetAvailableMetamagic(
                            AvailableMetamagic)
                        .AddAbilityEffectRunAction(
                            actions: ActionsBuilder
                                .New()
                                .Add<ContextActionOnPartyMembers>(
                                    action =>
                                    {
                                        action.Actions =
                                            ActionsBuilder
                                                .New()
                                                .Add<ContextActionFullHeal>()
                                                .SpawnFx("4d48e7ee3db59444d9b1dca869989b94") // True Form fx
                                                .Build();
                                    }))
                        .AddAbilitySpawnFx(anchor: AbilitySpawnFxAnchor.Caster, time: AbilitySpawnFxTime.OnStart, prefabLink: "96833db7343822040ada9d039f2fe738") // Zone Of Predetermination Fx
                        .AddAbilityDeliverDelay(0.3f)
                        .SetMaterialComponent(
                            new BlueprintAbility.MaterialComponentData
                            {
                                Count = 5,
                                m_Item = diamonds.ToReference<BlueprintItemReference>()
                            })
                        .Configure();

                Main.log.Log(
                    $"[Wish] Root created: " +
                    $"{wish.name}, {wish.AssetGuid}");

                for (int statIndex = 0;
                     statIndex < stats.Length;
                     statIndex++)
                {
                    for (int rank = 1;
                         rank <= 5;
                         rank++)
                    {
                        FeatureConfigurator
                            .New(
                                $"EbonsContentModWish{statNames[statIndex]}" +
                                $"InherentRank{rank}Feature",
                                inherentRankGuids[statIndex][rank - 1])
                            .SetDisplayName(
                                statNameKeys[statIndex])
                            .SetHideInUI(true)
                            .AddStatBonus(
                                ModifierDescriptor.Inherent,
                                stat: stats[statIndex],
                                value: rank)
                            .Configure();
                    }
                }

                ActionsBuilder BuildInherentBonusActions(
                    string[] rankGuids)
                {
                    return ActionsBuilder
                        .New()
                        .Conditional(
                            ConditionsBuilder
                                .New()
                                .HasFact(rankGuids[4]),
                            ifFalse:
                                ActionsBuilder
                                    .New()
                                    .Conditional(
                                        ConditionsBuilder
                                            .New()
                                            .HasFact(rankGuids[3]),
                                        ifTrue:
                                            ActionsBuilder
                                                .New()
                                                .AddFeature(rankGuids[4]),
                                        ifFalse:
                                            ActionsBuilder
                                                .New()
                                                .Conditional(
                                                    ConditionsBuilder
                                                        .New()
                                                        .HasFact(rankGuids[2]),
                                                    ifTrue:
                                                        ActionsBuilder
                                                            .New()
                                                            .AddFeature(rankGuids[3]),
                                                    ifFalse:
                                                        ActionsBuilder
                                                            .New()
                                                            .Conditional(
                                                                ConditionsBuilder
                                                                    .New()
                                                                    .HasFact(rankGuids[1]),
                                                                ifTrue:
                                                                    ActionsBuilder
                                                                        .New()
                                                                        .AddFeature(rankGuids[2]),
                                                                ifFalse:
                                                                    ActionsBuilder
                                                                        .New()
                                                                        .Conditional(
                                                                            ConditionsBuilder
                                                                                .New()
                                                                                .HasFact(rankGuids[0]),
                                                                            ifTrue:
                                                                                ActionsBuilder
                                                                                    .New()
                                                                                    .AddFeature(rankGuids[1]),
                                                                            ifFalse:
                                                                                ActionsBuilder
                                                                                    .New()
                                                                                    .AddFeature(rankGuids[0]))))));
                }

                // Create the first-level stat submenu before its six child abilities.
                BlueprintAbility statBonusMenu =
                    AbilityConfigurator
                        .NewSpell(
                            "EbonsContentModWishAbilityScoreBonusesAbility",
                            "4e78ce69-26af-4c50-853a-52dee370a342",
                            SpellSchool.Universalist,
                            canSpecialize: false)
                        .SetDisplayName(
                            wishAbilityScoresNameKey)
                        .SetDescription(
                            wishDescriptionKey)
                        .SetIcon(
                            wishStatBonusIcon)
                        .SetParent(wish)
                        .SetActionType(
                            UnitCommand.CommandType.Standard)
                        .SetRange(AbilityRange.Personal)
                        .SetCanTargetSelf()
                        .SetNotOffensive()
                        .SetShowNameForVariant(false)
                        .SetActionBarAutoFillIgnored()
                        .SetAvailableMetamagic(
                            AvailableMetamagic)
                        .Configure();

                var statVariants =
                    new List<Blueprint<BlueprintAbilityReference>>(
                        capacity: 6);

                for (int statIndex = 0;
                     statIndex < stats.Length;
                     statIndex++)
                {
                    string[] rankGuids =
                        inherentRankGuids[statIndex];

                    BlueprintAbility statAbility =
                        AbilityConfigurator
                            .NewSpell(
                                $"EbonsContentModWish{statNames[statIndex]}" +
                                "InherentBonusAbility",
                                statAbilityGuids[statIndex],
                                SpellSchool.Universalist,
                                canSpecialize: false)
                            .SetDisplayName(
                                statNameKeys[statIndex])
                            .SetDescription(
                                wishDescriptionKey)
                            .SetIcon(
                                wishStatBonusIcon)
                            .SetParent(
                                statBonusMenu)
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
                            .AddAbilityTargetHasFact(
                                checkedFacts:
                                    new List<Blueprint<BlueprintUnitFactReference>>
                                    {
                                rankGuids[4]
                                    },
                                inverted: true)
                            .AddAbilityEffectRunAction(
                                actions:
                                    BuildInherentBonusActions(
                                        rankGuids))
                            .AddAbilitySpawnFx(anchor: AbilitySpawnFxAnchor.ClickedTarget, time: AbilitySpawnFxTime.OnApplyEffect, prefabLink: "c428d8171749226429a9e3eeafad7ef2") // Edict of Perseverance Fx
                            .SetMaterialComponent(
                                new BlueprintAbility.MaterialComponentData
                                {
                                    Count = 5,
                                    m_Item = diamonds
                                        .ToReference<BlueprintItemReference>()
                                })
                            .Configure();

                    statVariants.Add(
                        statAbility);
                }

                AbilityConfigurator
                    .For(statBonusMenu)
                    .AddAbilityVariants(
                        statVariants)
                    .Configure();

                var rootVariants =
                    new List<Blueprint<BlueprintAbilityReference>>(
                        capacity: 9);

                for (int level = 1;
                     level <= 8;
                     level++)
                {
                    int selectedLevel =
                        level;

                    BlueprintAbility levelAbility =
                        AbilityConfigurator
                            .NewSpell(
                                $"EbonsContentModWishSpellLevel" +
                                $"{selectedLevel}",
                                wishLevelGuids[selectedLevel],
                                SpellSchool.Universalist,
                                canSpecialize: false)
                            .SetDisplayName(
                                $"EbonsContentMod.Miracle.Level" +
                                $"{selectedLevel}.Name")
                            .SetDescription(
                                wishDescriptionKey)
                            .SetIcon(
                                wishSpellIcon)
                            .SetParent(
                                wish)
                            .SetActionType(
                                UnitCommand.CommandType.Standard)
                            .SetRange(
                                AbilityRange.Personal)
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
                                        wizardSpellList;

                                    component.PrimaryMaxSpellLevel =
                                        8;

                                    component.m_OtherSpellLists =
                                        otherSpellLists.ToArray();

                                    component.OtherMaxSpellLevel =
                                        7;

                                    component.PrimaryOppositionMaxSpellLevel =
                                        7;

                                    component.OtherOppositionMaxSpellLevel =
                                        6;

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
                                        10000;
                                })
                            .Configure();

                    rootVariants.Add(
                        levelAbility);
                }

                // Ninth first-level icon: Ability Score Bonuses.
                rootVariants.Add(
                    statBonusMenu);

                rootVariants.Add(removeAfflictions);
                rootVariants.Add(healAllies);

                AbilityConfigurator
                    .For(wish)
                    .AddAbilityVariants(
                        rootVariants)
                    .Configure();

                Main.log.Log(
                    $"[Wish] Finished with " +
                    $"{rootVariants.Count} first-level variants and " +
                    $"{statVariants.Count} ability-score variants.");

                // Add scroll
                UsableItemsHelperators.CreateScrollFromSpell(wish, 9, "{019F0547-EEE7-4CC9-BE6F-20C19EA1B7A2}", SpriteHelperators.LoadSprite("Sprites\\WishScrollSprite.png"));

                return wish;
            }
            catch (Exception ex)
            {
                Main.log.Error(
                    "[Wish] CreateWish failed:\n" + ex);

                throw;
            }
        }

        internal static BlueprintAbility CreateLimitedWish()
        {
            Main.log.Log(
                "[Limited Wish] Entering CreateLimitedWish().");

            try
            {
                if (BlueprintTool.TryGet<BlueprintAbility>(
                        limitedWishGuid,
                        out BlueprintAbility existingLimitedWish))
                {
                    Main.log.Warning(
                        $"[Limited Wish] Blueprint already exists: " +
                        $"{existingLimitedWish.name}, " +
                        $"{existingLimitedWish.AssetGuid}");

                    return existingLimitedWish;
                }

                var limitedWishIcon =
                    SpriteHelperators.LoadSprite(
                        "Sprites\\LimitedWishSprite.png");

                var limitedWishSpellIcon =
                    AbilityRefs.RemoveFear
                        .Reference
                        .Get()?.Icon;

                var limitedWishCleanseIcon =
                    AbilityRefs.RemoveCurse
                        .Reference
                        .Get()?.Icon;

                BlueprintItem diamonds =
                    BlueprintTools.GetBlueprint<BlueprintItem>(
                        "6a7cdeb14fc6ef44580cf639c5cdc113");

                BlueprintSpellListReference wizardSpellList =
                    SpellListRefs.WizardSpellList
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

                    SpellListRefs.ClericSpellList
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
                        .Reference
                };

                var prohibitedSpells =
                    new List<BlueprintAbilityReference>
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

                BlueprintAbility limitedWish =
                    AbilityConfigurator
                        .NewSpell(
                            limitedWishName,
                            limitedWishGuid,
                            SpellSchool.Universalist,
                            canSpecialize: false)
                        .SetDisplayName(
                            limitedWishNameKey)
                        .SetDescription(
                            limitedWishDescriptionKey)
                        .SetIcon(
                            limitedWishIcon)
                        .SetActionType(
                            UnitCommand.CommandType.Standard)
                        .SetRange(
                            AbilityRange.Personal)
                        .SetCanTargetSelf()
                        .SetNotOffensive()
                        .SetShowNameForVariant(false)
                        .SetAvailableMetamagic(
                            AvailableMetamagic)
                        .SetMaterialComponent(
                            new BlueprintAbility.MaterialComponentData
                            {
                                Count = 1,
                                m_Item = diamonds
                                    .ToReference<BlueprintItemReference>()
                            })
                        .AddToSpellLists(
                            7,
                            SpellList.Wizard)
                        .Configure();

                // Limited Wish Remove Afflictions

                BlueprintAbility limitedWishRemoveAfflictions =
                    AbilityConfigurator
                        .NewSpell(
                            "EbonsContentModLimitedWishRemoveAfflictionsAbility",
                            "{17703500-EF01-4139-88FC-45AF535DCE35}",
                            SpellSchool.Universalist,
                            canSpecialize: false)
                        .CopyFrom(
                            AbilityRefs.RestorationGreater,
                            c => c is not AbilitySpawnFx or SpellListComponent or AbilityUseOnRest or SpellComponent or CraftInfoComponent or AbilityDeliverTouch)
                        .SetDisplayName(limitedWishRemoveAfflictionsNameKey)
                        .SetDescription(limitedWishDescriptionKey)
                        .SetIcon(limitedWishCleanseIcon)
                        .AddSpellComponent(school: SpellSchool.Universalist)
                        .SetParent(limitedWish)
                        .SetActionType(
                            UnitCommand.CommandType.Standard)
                        .SetRange(AbilityRange.Medium)
                        .SetCanTargetSelf()
                        .SetCanTargetFriends()
                        .SetNotOffensive()
                        .SetShowNameForVariant(false)
                        .SetActionBarAutoFillIgnored()
                        .SetAvailableMetamagic(
                            AvailableMetamagic)
                        .AddAbilitySpawnFx(prefabLink: "4d48e7ee3db59444d9b1dca869989b94", time: AbilitySpawnFxTime.OnApplyEffect, anchor: AbilitySpawnFxAnchor.SelectedTarget) // True Form Fx
                        .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Directional)
                        .SetMaterialComponent(
                            new BlueprintAbility.MaterialComponentData
                            {
                                Count = 1,
                                m_Item = diamonds.ToReference<BlueprintItemReference>()
                            })
                        .Configure();

                Main.log.Log(
                    $"[Limited Wish] Root created: " +
                    $"{limitedWish.name}, " +
                    $"{limitedWish.AssetGuid}");

                var rootVariants =
                    new List<Blueprint<BlueprintAbilityReference>>(
                        capacity: 6);

                for (int level = 1;
                     level <= 6;
                     level++)
                {
                    int selectedLevel =
                        level;

                    BlueprintAbility levelAbility =
                        AbilityConfigurator
                            .NewSpell(
                                $"EbonsContentModLimitedWishSpellLevel" +
                                $"{selectedLevel}",
                                limitedWishLevelGuids[selectedLevel],
                                SpellSchool.Universalist,
                                canSpecialize: false)
                            .SetDisplayName(
                                $"EbonsContentMod.Miracle.Level" +
                                $"{selectedLevel}.Name")
                            .SetDescription(
                                limitedWishDescriptionKey)
                            .SetIcon(
                                limitedWishSpellIcon)
                            .SetParent(
                                limitedWish)
                            .SetActionType(
                                UnitCommand.CommandType.Standard)
                            .SetRange(
                                AbilityRange.Personal)
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
                                        wizardSpellList;

                                    component.PrimaryMaxSpellLevel =
                                        6;

                                    component.PrimaryOppositionMaxSpellLevel =
                                        5;

                                    component.m_OtherSpellLists =
                                        otherSpellLists.ToArray();

                                    component.OtherMaxSpellLevel =
                                        5;

                                    component.OtherOppositionMaxSpellLevel =
                                        4;

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
                                        1000;
                                })
                            .Configure();

                    rootVariants.Add(
                        levelAbility);
                }

                rootVariants.Add(limitedWishRemoveAfflictions);

                AbilityConfigurator
                    .For(limitedWish)
                    .AddAbilityVariants(
                        rootVariants)
                    .Configure();

                Main.log.Log(
                    $"[Limited Wish] Finished with " +
                    $"{rootVariants.Count} spell-level variants.");

                // Add scroll
                UsableItemsHelperators.CreateScrollFromSpell(limitedWish, 7, "{D8B5B02D-C63A-4D86-B249-2B682C3E4B48}", SpriteHelperators.LoadSprite("Sprites\\LimitedWishScrollSprite.png"));

                return limitedWish;
            }
            catch (Exception ex)
            {
                Main.log.Error(
                    "[Limited Wish] CreateLimitedWish failed:\n" +
                    ex);

                throw;
            }
        }

        internal static BlueprintAbility CreateMiracle()
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

                var miracleIcon =
                    FeatureRefs.DomainMastery
                        .Reference
                        .Get()?.Icon;

                var miracleSpellIcon =
                    AbilityRefs.RemoveParalysis
                        .Reference
                        .Get()?.Icon;

                BlueprintItem goldCoins =
                    BlueprintTools.GetBlueprint<BlueprintItem>(
                        "f2bc0997c24e573448c6c91d2be88afa");

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

                BlueprintAbility miracle =
                    AbilityConfigurator
                        .NewSpell(
                            MiracleName,
                            MiracleGuid,
                            SpellSchool.Evocation,
                            canSpecialize: false)
                        .SetDisplayName(MiracleNameKey)
                        .SetDescription(MiracleDescriptionKey)
                        .SetIcon(miracleIcon)
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

                // Miracle: Cleanse Afflictions

                BlueprintAbility removeNegativeEffects =
                    AbilityConfigurator
                        .NewSpell(
                            "EbonsContentModMiracleRemoveNegativeEffectsAbility",
                            "d8ab8fac-44bc-4bc8-b69b-beff3735838d",
                            SpellSchool.Evocation,
                            canSpecialize: false)
                        .SetDisplayName(MiracleCleanseNameKey)
                        .SetDescription(MiracleDescriptionKey)
                        .SetIcon(
                            AbilityRefs.AngelPureForm
                                .Reference
                                .Get()
                                .Icon)
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
                                .Add<ContextActionRemoveAllNegativeEffects>()
                                .HealStatDamage(ContextActionHealStatDamage.StatDamageHealType.HealAllDamage, ContextActionHealStatDamage.StatClass.Any, true)
                                .HealStatDamage(ContextActionHealStatDamage.StatDamageHealType.HealAllDamage, ContextActionHealStatDamage.StatClass.Any, false)
                                .DispelMagic(ContextActionDispelMagic.BuffType.All, RuleDispelMagic.CheckType.None, 0, descriptor: SpellDescriptor.StatDebuff)
                                .HealEnergyDrain(EnergyDrainHealType.All, EnergyDrainHealType.All)
                                .RemoveDeathDoor()
                                .RemoveBuff(BuffRefs.Ecorche_Buff_SeizeSkin.Reference.Get())
                                .RemoveBuff(BuffRefs.BythosAgeBuff1.Reference.Get())
                                .RemoveBuff(BuffRefs.BythosAgeBuff2.Reference.Get())
                                .RemoveBuff(BuffRefs.BythosAgeBuff3.Reference.Get())
                                .RemoveBuff(BuffRefs.DLC3_ArcaneModDescription.Reference.Get())
                                .RemoveBuff(BuffRefs.DLC3_HasteIslandStacks.Reference.Get())
                                .RemoveBuff(BuffRefs.DLC3_HasteIslandAge1.Reference.Get())
                                .RemoveBuff(BuffRefs.DLC3_HasteIslandAge2.Reference.Get())
                                .RemoveBuff(BuffRefs.DLC3_HasteIslandAge3.Reference.Get())
                                .SpawnFx("3d7eb0d78be40b94dadb6ec05dea0c9a"))
                        .Configure();

                // Miracle: Rasie the Fallen

                BlueprintAbility raiseTheFallen =
                    AbilityConfigurator
                        .NewSpell(
                            "EbonsContentModMiracleRaiseTheFallenAbility",
                            "9eb9855a-03a4-45e4-b8c3-082cbb455f49",
                            SpellSchool.Evocation,
                            canSpecialize: false)
                        .CopyFrom(
                            AbilityRefs.AngelRekindle, c => c is not SpellListComponent or SpellComponent)
                        .SetDisplayName(MiracleRaiseTheFallenNameKey)
                        .SetDescription(MiracleDescriptionKey)
                        .AddSpellComponent(school: SpellSchool.Evocation)
                        .SetParent(miracle)
                        .SetActionType(
                            UnitCommand.CommandType.Standard)
                        .SetRange(AbilityRange.Medium)
                        .SetCanTargetSelf()
                        .SetCanTargetFriends()
                        .SetCanTargetPoint()
                        .SetNotOffensive()
                        .SetShowNameForVariant(false)
                        .SetActionBarAutoFillIgnored()
                        .SetAvailableMetamagic(
                            AvailableMetamagic)
                        .AddAbilityCanTargetDead()
                        .SetMaterialComponent(
                            new BlueprintAbility.MaterialComponentData
                            {
                                Count = 25000,
                                m_Item = goldCoins
                                    .ToReference<BlueprintItemReference>()
                            })
                        .Configure();

                // Miracle: Renewal

                BlueprintAbility renewal =
                    AbilityConfigurator
                        .NewSpell(
                            "EbonsContentModMiracleRenewalAbility",
                            "bd701ac3-7a9c-42d1-b678-e336bc81d484",
                            SpellSchool.Evocation,
                            canSpecialize: false)
                        .CopyFrom(
                            AbilityRefs.HealMass,
                            c => c is not AbilityEffectRunAction or SpellListComponent or AbilityUseOnRest or SpellComponent)
                        .SetDisplayName(MiracleRenewalNameKey)
                        .SetDescription(MiracleDescriptionKey)
                        .SetIcon(
                            AbilityRefs.HealMass
                                .Reference
                                .Get()
                                .Icon)
                        .AddSpellComponent(school: SpellSchool.Evocation)
                        .SetParent(miracle)
                        .SetActionType(
                            UnitCommand.CommandType.Standard)
                        .SetRange(AbilityRange.Medium)
                        .SetCanTargetSelf()
                        .SetCanTargetFriends()
                        .SetCanTargetPoint()
                        .SetNotOffensive()
                        .SetShowNameForVariant(false)
                        .SetActionBarAutoFillIgnored()
                        .SetAvailableMetamagic(
                            AvailableMetamagic)
                        .AddAbilityTargetsAround(
                            radius: 30.Feet(),
                            targetType: TargetType.Ally,
                            includeDead: false)
                        .AddAbilityEffectRunAction(
                            actions: ActionsBuilder
                                .New()
                                .Conditional(
                                    ConditionsBuilder.New().IsAlly(),
                                    ifTrue: ActionsBuilder.New().Add<ContextActionFullHeal>()
                                        .Add<ContextActionRemoveAllNegativeEffects>()
                                        .SpawnFx("319f507857aeb58499713b105df2bf29")))
                        .AddAbilitySpawnFx(AbilitySpawnFxAnchor.Caster, 0.0f, false, AbilitySpawnFxAnchor.None, AbilitySpawnFxOrientation.Copy, AbilitySpawnFxAnchor.None, "67ed10195139aa649ab2acad14575986", AbilitySpawnFxTime.OnPrecastStart, AbilitySpawnFxWeaponTarget.None)
                        .AddAbilityDeliverDelay(0.4f)
                        .SetMaterialComponent(
                            new BlueprintAbility.MaterialComponentData
                            {
                                Count = 25000,
                                m_Item = goldCoins
                                    .ToReference<BlueprintItemReference>()
                            })
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
                            .SetIcon(miracleSpellIcon)
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
                levelVariants.Add(raiseTheFallen);
                levelVariants.Add(renewal);

                AbilityConfigurator
                    .For(miracle)
                    .AddAbilityVariants(levelVariants)
                    .Configure();

                Main.log.Log(
                    $"[Miracle] Finished with " +
                    $"{levelVariants.Count} level variants.");

                Main.log.Log(
                    $"[Miracle Meta] Root AvailableMetamagic = " +
                    $"{miracle.AvailableMetamagic}");

                AbilityVariants miracleVariants =
                    miracle.GetComponent<AbilityVariants>();

                foreach (BlueprintAbilityReference variantReference in
                         miracleVariants?.m_Variants
                         ?? Array.Empty<BlueprintAbilityReference>())
                {
                    BlueprintAbility variant =
                        variantReference?.Get();

                    if (variant == null)
                        continue;

                    Main.log.Log(
                        $"[Miracle Meta] Variant " +
                        $"{variant.name}: " +
                        $"AvailableMetamagic={variant.AvailableMetamagic}, " +
                        $"DuplicateSpell=" +
                        $"{(variant.GetComponent<AbilityDuplicateSpell>() != null)}");
                }

                BlueprintAbility shades =
                    AbilityRefs.Shades.Reference.Get();

                Main.log.Log(
                    $"[Miracle Meta] Shades AvailableMetamagic = " +
                    $"{shades?.AvailableMetamagic}");

                // Add scroll
                UsableItemsHelperators.CreateScrollFromSpell(miracle, 9, "{361B7F4B-9917-4796-BCA2-EB03B9ECC539}", ItemEquipmentUsableRefs.ScrollOfLitanyofMadness.Reference.Get().Icon);

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

        internal static void FixMiracleWishSpells()
        {
            string[] spellGuids =
            {
                MiracleGuid,
                wishGuid,
                limitedWishGuid
            };

            BlueprintAbility limitedWish =
                BlueprintTools.GetBlueprint<BlueprintAbility>(
                    limitedWishGuid);

            // Collect limited wish and all sub-abilities
            var prohibitedSpells =
                new HashSet<BlueprintAbilityReference>();

            CollectAbilityTree(
                limitedWish,
                prohibitedSpells,
                new HashSet<BlueprintAbility>());

            foreach (string guid in spellGuids)
            {
                if (!BlueprintTool.TryGet<BlueprintAbility>(
                        guid,
                        out BlueprintAbility spell))
                {
                    continue;
                }

                // Remove limited wish from duplication menus
                if (spell.AssetGuid.ToString() != limitedWishGuid)
                {
                    AddDuplicateSpellExclusionsRecursive(
                        spell,
                        prohibitedSpells,
                        new HashSet<BlueprintAbility>());
                }

                FixMetamagicRecursive(spell);
            }
        }

        private static void CollectAbilityTree(
            BlueprintAbility ability,
            ISet<BlueprintAbilityReference> abilities,
            ISet<BlueprintAbility> visited)
        {
            if (ability == null)
                return;

            if (!visited.Add(ability))
                return;

            abilities.Add(
                ability.ToReference<BlueprintAbilityReference>());

            AbilityVariants variants =
                ability.GetComponent<AbilityVariants>();

            if (variants?.m_Variants == null)
                return;

            foreach (BlueprintAbilityReference variantRef
                     in variants.m_Variants)
            {
                BlueprintAbility variant =
                    variantRef?.Get();

                if (variant == null)
                    continue;

                CollectAbilityTree(
                    variant,
                    abilities,
                    visited);
            }
        }

        private static void AddDuplicateSpellExclusionsRecursive(
            BlueprintAbility ability,
            IEnumerable<BlueprintAbilityReference> prohibitedSpells,
            ISet<BlueprintAbility> visited)
        {
            if (ability == null)
                return;

            if (!visited.Add(ability))
                return;

            AbilityDuplicateSpell duplicate =
                ability.GetComponent<AbilityDuplicateSpell>();

            if (duplicate != null)
            {
                duplicate.m_ExcludedSpells =
                    (duplicate.m_ExcludedSpells
                     ?? Array.Empty<BlueprintAbilityReference>())
                    .Concat(prohibitedSpells)
                    .Distinct()
                    .ToArray();

                duplicate.ClearCache();
            }

            AbilityVariants variants =
                ability.GetComponent<AbilityVariants>();

            if (variants?.m_Variants == null)
                return;

            foreach (BlueprintAbilityReference variantRef
                     in variants.m_Variants)
            {
                BlueprintAbility variant =
                    variantRef?.Get();

                if (variant == null)
                    continue;

                AddDuplicateSpellExclusionsRecursive(
                    variant,
                    prohibitedSpells,
                    visited);
            }
        }

        private static void FixMetamagicRecursive(
            BlueprintAbility spell)
        {
            if (spell == null)
                return;

            AbilityConfigurator
                .For(spell)
                .SetAvailableMetamagic(AvailableMetamagic)
                .Configure();

            AbilityVariants variants =
                spell.GetComponent<AbilityVariants>();

            if (variants?.m_Variants == null)
                return;

            foreach (BlueprintAbilityReference variantRef
                     in variants.m_Variants)
            {
                BlueprintAbility variant =
                    variantRef?.Get();

                if (variant == null)
                    continue;

                FixMetamagicRecursive(variant);
            }
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
        /// A duplicated spell normally checks only its own material component.
        ///
        /// Make it additionally require the material component of the root
        /// duplicating spell. For Wish this is the five-diamond component.
        ///
        /// The duplicated spell's own component continues to be handled normally,
        /// including AbilityDuplicateSpell's material-cost waiver.
        /// </summary>
        [HarmonyPatch(
            typeof(AbilityData),
            nameof(AbilityData.IsAvailableForCast),
            MethodType.Getter)]
        private static class
            AbilityData_IsAvailableForCast_RootMaterial_Patch
        {
            [HarmonyPostfix]
            private static void Postfix(
                AbilityData __instance,
                ref bool __result)
            {
                if (!__result)
                    return;

                if (!TryGetRootDuplicatingSource(
                        __instance,
                        out AbilityData rootSource))
                {
                    return;
                }

                if (rootSource.Caster?.IsPlayerFaction != true)
                    return;

                /*
                 IMPORTANT:
                 
                 Ask the normal AbilityData API whether the root's material component is actually required. This allows things such as Mythic Eschew Materials to suppress the requirement normally.
                 */
                if (rootSource.RequireMaterialComponent
                    && !rootSource.HasEnoughMaterialComponent)
                {
                    __result = false;
                }
            }
        }

        /// <summary>
        /// After the duplicated spell pays any material component of its own,
        /// also pay the root duplicating spell's material component.
        ///
        /// For Wish:
        ///     duplicated spell's component, if > 10,000 gp
        ///     + five diamonds for Wish
        ///
        /// For a component costing <= 10,000 gp, the existing
        /// RequireMaterialComponent patch suppresses the duplicated spell's
        /// component, while the five Wish diamonds are still paid here.
        /// </summary>
        [HarmonyPatch(
            typeof(AbilityData),
            nameof(AbilityData.SpendMaterialComponent))]
        private static class AbilityData_SpendMaterialComponent_RootMaterial_Patch
        {
            [HarmonyPostfix]
            private static void Postfix(
                AbilityData __instance)
            {
                if (!TryGetRootDuplicatingSource(
                        __instance,
                        out AbilityData rootSource))
                {
                    return;
                }

                /*
                 The duplicated spell has already processed its own material component through its normal SpendMaterialComponent() call.
                 
                 Now process the root Miracle/Wish component through THE SAME normal AbilityData API.
                 
                 Do not remove inventory items directly. This lets other mods such as DarkCodex's Mythic Eschew Materials modify or suppress the cost normally.
                 */
                rootSource.SpendMaterialComponent();
            }
        }

        private static bool TryGetRootDuplicatingSource(
            AbilityData ability,
            out AbilityData rootSource)
        {
            rootSource = null;

            if (!AbilityDuplicateSpell.TryGetConversionSource(
                    ability,
                    out AbilityData duplicateSource,
                    out _))
            {
                return false;
            }

            rootSource =
                GetRootConversionSource(duplicateSource);

            return rootSource != null
                && !ReferenceEquals(rootSource, ability);
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

        private static bool IsDuplicateSpellRootMenu(
            BlueprintAbility blueprint)
        {
            if (blueprint == null)
                return false;

            AbilityVariants variants =
                blueprint.GetComponent<AbilityVariants>();

            if (variants?.m_Variants == null)
                return false;

            return variants.m_Variants.Any(
                variantReference =>
                {
                    BlueprintAbility variant =
                        variantReference?.Get();

                    return variant
                        ?.GetComponent<AbilityDuplicateSpell>()
                        != null;
                });
        }

        private static bool IsResourceOverriddenNestedMenu(
            AbilityData ability)
        {
            if (ability == null
                || ability.Blueprint?.HasVariants != true)
            {
                return false;
            }

            if (!(ability.ResourceLogic
                  is AbilityResourceOverride))
            {
                return false;
            }

            AbilityData root =
                GetRootConversionSource(
                    ability);

            return root != null
                && IsDuplicateSpellRootMenu(
                    root.Blueprint);
        }

        [HarmonyPatch(
            typeof(AbilityData),
            nameof(AbilityData.GetAvailableForCastCount))]
        private static class AbilityData_GetAvailableForCastCount_VariationalResourceOverride_Patch
        {
            [HarmonyPrefix]
            private static bool Prefix(
                AbilityData __instance,
                ref int __result)
            {
                if (!IsResourceOverriddenNestedMenu(
                        __instance))
                {
                    return true;
                }

                __result = 1;

                return false;
            }
        }

        [HarmonyPatch(
            typeof(
                MechanicActionBarSlotSpontaneusConvertedSpell),
            "GetResource")]
        private static class MechanicActionBarSlotSpontaneusConvertedSpell_GetResource_Patch
        {
            [HarmonyPrefix]
            private static bool Prefix(
                MechanicActionBarSlotSpontaneusConvertedSpell __instance,
                ref int __result)
            {
                AbilityData ability =
                    __instance?.Spell;

                if (!IsResourceOverriddenNestedMenu(
                        ability))
                {
                    return true;
                }

                __result = -1;

                return false;
            }
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
                    __result = false;
                }
            }
        }

        #endregion

        #region MVVM nested conversion menu patch

        private static bool s_ActionBarSlotVMMainPatchApplied;
        private static bool s_ActionBarSlotVMShowConvertPatchApplied;

        /*
         The current Wrath UI displays action-bar conversions through ActionBarSlotVM.
         
         ActionBarSlotVM.OnMainClick() does not treat MechanicActionBarSlotSpontaneusConvertedSpell as a slot that can open another conversion menu. We add that missing branch for the Miracle level selectors.
         
         The small conversion arrow invokes OnShowConvertRequest() directly, whereas clicking the square icon reaches it through OnMainClick(). Recording the Miracle root from both paths makes either control work on the first click and after action-bar or area UI reconstruction.
         */

        private static ActionBarSlotVM s_MiracleRootSlot;
        private static ActionBarConvertedVM s_MiracleRootMenu;

        private static bool CloseConvertedMenuIfOpen(
            ActionBarSlotVM slot)
        {
            if (slot?.ConvertedVm.Value == null)
                return false;

            slot.CloseConvert();

            if (ReferenceEquals(
                    s_MiracleRootSlot,
                    slot))
            {
                s_MiracleRootSlot = null;
                s_MiracleRootMenu = null;
            }

            Main.log.Log(
                "[Miracle/Wish UI] Closed existing conversion menu.");

            return true;
        }

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
                    prefix: new HarmonyMethod(
                        AccessTools.Method(
                            typeof(MiracleWish),
                            nameof(
                                ActionBarSlotVMOnShowConvertRequestPrefix))),
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

        private static IEnumerable<AbilityData> GetNestedSelectorConversions(
            AbilityData source,
            AbilityDuplicateSpell provider,
            AbilityVariants staticVariants)
        {
            if (source == null)
            {
                return Enumerable.Empty<AbilityData>();
            }

            if (provider != null)
            {
                return provider.GetOrCreateConversions(
                    source);
            }

            if (staticVariants?.m_Variants == null)
            {
                return Enumerable.Empty<AbilityData>();
            }

            var conversions =
                new List<AbilityData>(
                    staticVariants.m_Variants.Length);

            foreach (BlueprintAbilityReference variantRef
                     in staticVariants.m_Variants)
            {
                BlueprintAbility variant =
                    variantRef?.Get();

                if (variant == null)
                    continue;

                var conversion =
                    new AbilityData(
                        source,
                        variant)
                    {
                        SaveSpellbookSlot =
                            source.SaveSpellbookSlot
                    };

                if (source.OverridenResourceLogic != null)
                {
                    conversion.OverrideResourceLogic(
                        source.OverridenResourceLogic);
                }

                conversions.Add(
                    conversion);
            }

            return conversions;
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

            MechanicActionBarSlot mechanic =
                __instance.MechanicActionBarSlot;

            AbilityData levelAbility =
                GetAbilityFromActionBarSlot(
                    __instance);

            if (levelAbility == null)
                return true;

            if (IsDuplicateSpellRootMenu(
                levelAbility.Blueprint))
            {
                if (CloseConvertedMenuIfOpen(
                        __instance))
                {
                    return false;
                }

                Main.log.Log(
                    $"[Miracle/Wish UI] Root click: " +
                    $"{levelAbility.Blueprint.name}; " +
                    $"mechanic=" +
                    $"{__instance.MechanicActionBarSlot?.GetType().FullName ?? "<null>"}; " +
                    $"SaveSpellbookSlot={levelAbility.SaveSpellbookSlot}; " +
                    $"RequiredResource=" +
                    $"{levelAbility.RequiredResource?.name ?? "<none>"}.");

                OpenDuplicateSpellRootMenu(
                    __instance,
                    levelAbility);

                return false;
            }

            var selectorMechanic =
                mechanic
                as MechanicActionBarSlotSpontaneusConvertedSpell;

            if (selectorMechanic == null)
                return true;

            AbilityDuplicateSpell provider =
                levelAbility.Blueprint
                    ?.GetComponent<AbilityDuplicateSpell>();

            AbilityVariants staticVariants =
                levelAbility.Blueprint
                    ?.GetComponent<AbilityVariants>();

            if (provider == null
                && staticVariants == null)
            {
                return true;
            }

            string selectorLabel =
                provider != null
                    ? $"level={provider.SelectedSpellLevel}"
                    : "static variants";

            try
            {
                Main.log.Log(
                    $"[Miracle/Wish UI] MVVM selector click: " +
                    $"{levelAbility.Blueprint.name}, " +
                    $"{selectorLabel}.");

                ActionBarSlotVM rootSlot =
                    s_MiracleRootSlot;

                ActionBarConvertedVM currentMenu =
                    rootSlot?.ConvertedVm.Value;

                bool selectorBelongsToCurrentMenu =
                    currentMenu?.Slots.Any(
                        slot => ReferenceEquals(
                            slot,
                            __instance))
                    == true;

                ActionBarSlotVM hostSlot;

                if (selectorBelongsToCurrentMenu)
                {
                    hostSlot =
                        rootSlot;
                }
                else
                {
                    hostSlot =
                        __instance;

                    Main.log.Log(
                        $"[Miracle/Wish UI] " +
                        $"{levelAbility.Blueprint.name} is a standalone " +
                        $"quick-slot selector; using its own slot as menu host.");

                    if (CloseConvertedMenuIfOpen(
                            hostSlot))
                    {
                        return false;
                    }
                }

                IEnumerable<AbilityData> conversionSource =
                    GetNestedSelectorConversions(
                        levelAbility,
                        provider,
                        staticVariants);

                List<AbilityData> conversions =
                    conversionSource
                        .Where(
                            ability =>
                                ability != null
                                && ability.IsVisible())
                        .ToList();

                Main.log.Log(
                    $"[Miracle/Wish UI] " +
                    $"{levelAbility.Blueprint.name} produced " +
                    $"{conversions.Count} visible conversions.");

                if (conversions.Count == 0)
                {
                    Main.log.Warning(
                        $"[Miracle UI] " +
                        $"{levelAbility.Blueprint.name} produced no " +
                        "visible conversions.");

                    return false;
                }

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

                var replacementMenu =
                    new ActionBarConvertedVM(
                        mechanicSlots,
                        hostSlot.CloseConvert);

                foreach (ActionBarSlotVM slot in
                         replacementMenu.Slots)
                {
                    slot.UpdateResource();
                }

                hostSlot.CloseConvert();

                hostSlot.ConvertedVm.Value =
                    replacementMenu;

                if (ReferenceEquals(
                    hostSlot,
                    s_MiracleRootSlot))
                {
                    s_MiracleRootMenu =
                        null;
                }

                Main.log.Log(
                    $"[Miracle/Wish UI] Replaced nested menu for " +
                    $"{levelAbility.Blueprint.name} with " +
                    $"{mechanicSlots.Count} entries.");

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

        private static bool ActionBarSlotVMOnShowConvertRequestPrefix(
            ActionBarSlotVM __instance)
        {
            if (__instance == null)
                return true;

            AbilityData rootAbility =
                GetAbilityFromActionBarSlot(
                    __instance);

            if (rootAbility == null
                || !IsDuplicateSpellRootMenu(
                    rootAbility.Blueprint))
            {
                return true;
            }

            if (CloseConvertedMenuIfOpen(
                    __instance))
            {
                return false;
            }

            try
            {
                Main.log.Log(
                    $"[Miracle/Wish UI] Root arrow click: " +
                    $"{rootAbility.Blueprint.name}; " +
                    $"mechanic=" +
                    $"{__instance.MechanicActionBarSlot?.GetType().FullName ?? "<null>"}.");

                List<AbilityData> conversions =
                    rootAbility
                        .GetConversions()
                        .Where(
                            ability =>
                                ability != null
                                && ability.IsVisible())
                        .ToList();

                Main.log.Log(
                    $"[Miracle/Wish UI] Root arrow for " +
                    $"{rootAbility.Blueprint.name} produced " +
                    $"{conversions.Count} visible conversions.");

                if (conversions.Count == 0)
                {
                    Main.log.Warning(
                        $"[Miracle/Wish UI] Root arrow for " +
                        $"{rootAbility.Blueprint.name} produced no " +
                        $"visible conversions.");

                    return false;
                }

                UnitEntityData unit =
                    rootAbility.Caster?.Unit;

                if (unit == null)
                {
                    Main.log.Error(
                        $"[Miracle/Wish UI] Root arrow for " +
                        $"{rootAbility.Blueprint.name} had no caster unit.");

                    return false;
                }

                List<MechanicActionBarSlot> mechanicSlots =
                    new SlotConversion(conversions)
                        .GetMechanicSlots(unit)
                        .ToList();

                Main.log.Log(
                    $"[Miracle/Wish UI] Root arrow for " +
                    $"{rootAbility.Blueprint.name} produced " +
                    $"{mechanicSlots.Count} mechanic slots.");

                if (mechanicSlots.Count == 0)
                {
                    Main.log.Warning(
                        $"[Miracle/Wish UI] Root arrow for " +
                        $"{rootAbility.Blueprint.name} produced no " +
                        $"mechanic slots.");

                    return false;
                }

                var menu =
                    new ActionBarConvertedVM(
                        mechanicSlots,
                        __instance.CloseConvert);

                foreach (ActionBarSlotVM slot in
                         menu.Slots)
                {
                    slot.UpdateResource();
                }

                __instance.CloseConvert();

                __instance.ConvertedVm.Value =
                    menu;

                s_MiracleRootSlot =
                    __instance;

                s_MiracleRootMenu =
                    menu;

                Main.log.Log(
                    $"[Miracle/Wish UI] Opened full root conversion " +
                    $"menu for {rootAbility.Blueprint.name} with " +
                    $"{mechanicSlots.Count} entries.");

                return false;
            }
            catch (Exception ex)
            {
                Main.log.Error(
                    "[Miracle/Wish UI] Root arrow handling failed:\n" +
                    ex);

                return true;
            }
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

        private static void OpenDuplicateSpellRootMenu(
            ActionBarSlotVM rootSlot,
            AbilityData rootAbility)
        {
            if (rootSlot == null
                || rootAbility == null)
            {
                return;
            }

            try
            {
                AbilityVariants variants =
                    rootAbility.Blueprint
                        ?.GetComponent<AbilityVariants>();

                if (variants?.m_Variants == null
                    || variants.m_Variants.Length == 0)
                {
                    Main.log.Error(
                        $"[Miracle/Wish UI] Root " +
                        $"{rootAbility.Blueprint?.name} had no variants.");

                    return;
                }

                var conversions =
                    new List<AbilityData>(
                        variants.m_Variants.Length);

                foreach (BlueprintAbilityReference variantRef
                         in variants.m_Variants)
                {
                    BlueprintAbility variant =
                        variantRef?.Get();

                    if (variant == null)
                        continue;

                    var conversion =
                        new AbilityData(
                            rootAbility,
                            variant)
                        {
                            SaveSpellbookSlot =
                                rootAbility.SaveSpellbookSlot
                        };

                    if (rootAbility.ResourceLogic != null)
                    {
                        conversion.OverrideResourceLogic(
                            rootAbility.ResourceLogic);
                    }

                    conversions.Add(
                        conversion);
                }

                Main.log.Log(
                    $"[Miracle/Wish UI] Building root menu for " +
                    $"{rootAbility.Blueprint.name}: " +
                    $"variants={conversions.Count}; " +
                    $"SaveSpellbookSlot={rootAbility.SaveSpellbookSlot}; " +
                    $"RequiredResource=" +
                    $"{rootAbility.RequiredResource?.name ?? "<none>"}.");

                if (conversions.Count == 0)
                {
                    Main.log.Error(
                        $"[Miracle/Wish UI] Root " +
                        $"{rootAbility.Blueprint.name} produced no menu entries.");

                    return;
                }

                UnitEntityData unit =
                    rootAbility.Caster?.Unit;

                if (unit == null)
                {
                    Main.log.Error(
                        $"[Miracle/Wish UI] Root " +
                        $"{rootAbility.Blueprint.name} had no caster unit.");

                    return;
                }

                List<MechanicActionBarSlot> mechanicSlots =
                    new SlotConversion(conversions)
                        .GetMechanicSlots(unit)
                        .ToList();

                if (mechanicSlots.Count == 0)
                {
                    Main.log.Error(
                        $"[Miracle/Wish UI] Root " +
                        $"{rootAbility.Blueprint.name} produced no mechanic slots.");

                    return;
                }

                var menu =
                    new ActionBarConvertedVM(
                        mechanicSlots,
                        rootSlot.CloseConvert);

                foreach (ActionBarSlotVM slot in
                         menu.Slots)
                {
                    slot.UpdateResource();
                }

                rootSlot.CloseConvert();

                rootSlot.ConvertedVm.Value =
                    menu;

                s_MiracleRootSlot =
                    rootSlot;

                s_MiracleRootMenu =
                    menu;

                Main.log.Log(
                    $"[Miracle/Wish UI] Opened root menu for " +
                    $"{rootAbility.Blueprint.name} with " +
                    $"{mechanicSlots.Count} entries.");
            }
            catch (Exception ex)
            {
                Main.log.Error(
                    "[Miracle/Wish UI] Failed to open root menu:\n" +
                    ex);
            }
        }

        private static AbilityData GetAbilityFromActionBarSlot(
            ActionBarSlotVM slot)
        {
            MechanicActionBarSlot mechanic =
                slot?.MechanicActionBarSlot;

            if (mechanic == null)
                return null;

            if (mechanic is
                MechanicActionBarSlotSpontaneusConvertedSpell converted)
            {
                return converted.Spell;
            }

            if (mechanic is
                MechanicActionBarSlotMemorizedSpell memorized)
            {
                return memorized.Spell;
            }

            if (mechanic is
                MechanicActionBarSlotSpontaneousSpell spontaneous)
            {
                return spontaneous.Spell;
            }

            if (mechanic is
                MechanicActionBarSlotAbility ability)
            {
                return ability.Ability;
            }

            return null;
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