using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using System;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.NewComponents;
using EbonsContentMod.Utilities;
using UnityEngine;
using Kingmaker.ResourceLinks;
using Kingmaker.Enums.Damage;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using EbonsContentMod.Components;
using Kingmaker.UnitLogic.Mechanics;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Craft;
using Kingmaker.UI.SettingsUI;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Mechanics.Components;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Designers.Mechanics.Buffs;
using static Kingmaker.Blueprints.Classes.Spells.SuppressSpellSchool;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.NewUnitParts;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.Visual.Animation.Kingmaker.Actions;

namespace EbonsContentMod.Races
{
    internal class Duskwalker
    {
        public static List<Color> RaceHeadColors =
        [

        ];

        public static List<Color> RaceEyeColors =
        [

        ];

        public static List<Color> RaceHairColors =
        [

        ];

        public static List<Texture2D> CustomEyeRamps =
        [

        ];

        public static List<Texture2D> CustomHeadRamps =
        [
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 13), // Gray
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 4), // Blue-Gray
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 1), // medium Gray
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 0), // Light Gray
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 8), // Very Light Gray
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 5), // Pallid White
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 1), // Rock Gray
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 0), // Medium Gray
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 4), // Very Dark Gray
        ];

        public static List<Texture2D> CustomHairRamps =
        [
            RaceRecolorizer.GetRaceEyeRampByIndex(RaceRefs.OreadRace.Reference.Get(), 3), // Light Red
            RaceRecolorizer.GetRaceEyeRampByIndex(RaceRefs.ElfRace.Reference.Get(), 7), // Very Light Pink
            RaceRecolorizer.GetRaceEyeRampByIndex(RaceRefs.OreadRace.Reference.Get(), 4), // Very Light Blue
            RaceRecolorizer.GetRaceHairRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 15), // Red
            RaceRecolorizer.GetRaceEyeRampByIndex(RaceRefs.OreadRace.Reference.Get(), 2), // Light Pink
            RaceRecolorizer.GetRaceEyeRampByIndex(RaceRefs.OreadRace.Reference.Get(), 5), // Light Blue
            RaceRecolorizer.GetRaceHairRampByIndex(RaceRefs.OreadRace.Reference.Get(), 27), // White
            RaceRecolorizer.GetRaceHairRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 3), // Light Gray
            
            
            RaceRecolorizer.GetRaceHairRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 5), // Light Gray
            
        ];

        public static EquipmentEntityLink[] FemaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "30d504db6b8cbe94dbc82d2437c8b468"}, // Long Wavy Female Human
            new EquipmentEntityLink() {AssetId = "779458079f7718c4bb960d9cef195339"}, // Long Wavy Braids
            new EquipmentEntityLink() {AssetId = "afa22656ed5030c4ba273583ba2b3a16"}, // Long Wild Female Tiefling
            new EquipmentEntityLink() {AssetId = "04c3eb6d7570d8d49b686516b7c4a4f8"}, // Long Camelia Hair
            new EquipmentEntityLink() {AssetId = "1f19aaaa1870e2b4b8bd99d36211ddf6"}, // Ponytail Upper Female Human
            new EquipmentEntityLink() {AssetId = "fc3fc0e45a70a0e42b6aed10baf794f0"}, // Dread Seelah Female Human
            new EquipmentEntityLink() {AssetId = "d71d2e53fce0f1d4baad8b20c8266676"}, // Slick Female Tiefling
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"}  // Bald
        ];

        public static EquipmentEntityLink[] MaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "acdcfd7609f88ae49833e4f10656190e"}, // Long Bangs Male Human
            new EquipmentEntityLink() {AssetId = "303578a648d8d344b8d3a9a94fe24d5a"}, // Long Wavy Male Human
            new EquipmentEntityLink() {AssetId = "88c2650d77d9a3c4a8a861fa0d8d0aae"}, // Military Male Human
            new EquipmentEntityLink() {AssetId = "def666224ba24df4e954c03049b29a53"}, // Short Human
            new EquipmentEntityLink() {AssetId = "329cf540a8faed64284c067bace8bbc9"}, // Trim Male Human
            new EquipmentEntityLink() {AssetId = "609143dbf7607f6419babaf5748b82dc"}, // Medium Bun Male Human
            new EquipmentEntityLink() {AssetId = "222890293b0f66145a400eae3432868d"}, // Mohawk Male Human
            new EquipmentEntityLink() {AssetId = "50eac92ba30862940be4f70d329d070a"}, // Long Wild Male Tiefling
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"}  // Bald
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string DuskwalkerName = "DuskwalkerRace";

        internal const string DuskwalkerDisplayName = "Duskwalker.Name";
        private static readonly string DuskwalkerDescription = "Duskwalker.Description";
        public static readonly string RaceGuid = "{A351EF30-461C-463F-A570-93F92AFF09B0}";

        internal const string GhostHunterDisplayName = "Duskwalker.GhostHunter.Name";
        private static readonly string GhostHunterDescription = "Duskwalker.GhostHunter.Description";

        internal const string WardAgainstCorruptionDisplayName = "Duskwalker.WardAgainstCorruption.Name";
        private static readonly string WardAgainstCorruptionDescription = "Duskwalker.WardAgainstCorruption.Description";

        internal const string DuskwalkerSkilledDisplayName = "Duskwalker.Skilled.Name";
        private static readonly string DuskwalkerSkilledDescription = "Duskwalker.Skilled.Description";

        internal static void Configure()
        {
            // Crystalline Dust

            var GhostHunterBuff = BuffConfigurator.New("GhostHunterBuff", "{2C42607A-93EC-494F-B623-B5D95E98B8A5}")
                .SetDisplayName(GhostHunterDisplayName)
                .SetDescription(GhostHunterDescription)
                .SetIcon(AbilityRefs.DisruptingWeapon.Reference.Get().Icon)
                .AddBuffEnchantAnyWeapon(enchantmentBlueprint: WeaponEnchantmentRefs.GhostTouch.ToString())
                .Configure();


            var GhostHunterResource = AbilityResourceConfigurator.New("GhostHunterResource", "{D76A0439-0BB2-48C4-9D0B-ED8E9CEF14E1}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1)
                )
                .Configure();

            var GhostHunterAbility = AbilityConfigurator.New("GhostHunterAblity", "{1059E694-4D45-4EBB-94CC-416066ADE9AC}")
                .SetDisplayName(GhostHunterDisplayName)
                .SetDescription(GhostHunterDescription)
                .SetIcon(AbilityRefs.DisruptingWeapon.Reference.Get().Icon)
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: GhostHunterResource)
                .SetRange(AbilityRange.Personal)
                .SetCanTargetSelf()
                .SetType(AbilityType.Supernatural)
                .SetActionType(UnitCommand.CommandType.Standard)
                .SetAnimation(UnitAnimationActionCastSpell.CastAnimationStyle.EnchantWeapon)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(buff: GhostHunterBuff, durationValue: new ContextDurationValue()
                {
                    Rate = DurationRate.Minutes,
                    DiceType = DiceType.Zero,
                    DiceCountValue = new ContextValue()
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0,
                        ValueRank = AbilityRankType.Default,
                        ValueShared = AbilitySharedValue.Damage,
                        Property = UnitProperty.None
                    },
                    BonusValue = new ContextValue()
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 1
                    },
                    m_IsExtendable = false,
                }, false, isNotDispelable: true).Build())
                .Configure();

            var GhostHunter = FeatureConfigurator.New("DuskwalkerGhostHunter", "{3A405713-83F7-4537-AC5C-E58B44E86BC4}")
                .SetDisplayName(GhostHunterDisplayName)
                .SetDescription(GhostHunterDescription)
                .SetIcon(AbilityRefs.DisruptingWeapon.Reference.Get().Icon)
                .SetGroups(FeatureGroup.Racial)
                .AddFacts([GhostHunterAbility])
                .AddAbilityResources(resource: GhostHunterResource, restoreAmount: true)
                .Configure();

            // Ward Against Corruption

            var WardAgainstCorruption = FeatureConfigurator.New("DuskwalkerWardAgainstCorruption", "{7492B829-6841-4A59-8ED6-20E099BAD061}")
                .SetDisplayName(WardAgainstCorruptionDisplayName)
                .SetDescription(WardAgainstCorruptionDescription)
                .SetIcon(FeatureRefs.CelestialResistance.Reference.Get().Icon)
                .AddSavingThrowBonusAgainstFact(checkedFact: FeatureRefs.UndeadType.ToString(), descriptor: ModifierDescriptor.Racial, value: 2)
                .AddSavingThrowBonusAgainstDescriptor(2, modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.Death)
                .AddSavingThrowBonusAgainstDescriptor(2, modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.NegativeLevel)
                .AddSavingThrowBonusAgainstDescriptor(2, modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.ChannelNegativeHarm)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            // Skilled

            var Skilled = FeatureConfigurator.New("DuskwalkerSkilled", "{E4B76374-A08E-4EF0-9227-CB7396F7A78B}")
                .SetDisplayName(DuskwalkerSkilledDisplayName)
                .SetDescription(DuskwalkerSkilledDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HumanSkilled.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillLoreReligion, value: 2)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var race =
            RaceConfigurator.New(DuskwalkerName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(DuskwalkerDisplayName)
                .SetDescription(DuskwalkerDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(FeatureRefs.KeenSenses.ToString(), Skilled, WardAgainstCorruption, GhostHunter)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: -2)
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, RaceEyeColors, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DhampirRace.ToString()), CustomHeadRamps: CustomHeadRamps, CustomHairRamps: CustomHairRamps, CustomEyeRamps: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DwarfRace.ToString()).FemaleOptions.m_Heads[0].Load(true, false).SecondaryColorsProfile.Ramps, CustomFemaleHairs: FemaleHairs, CustomMaleHairs: MaleHairs);

            // Add race to mount fixes
            RaceMountFixerizer.AddRaceToMountFixes(recoloredrace, CopyRace);

            // Fix Odds and Ends
            RaceOddsAndEndsFixerizer.FixRace(recoloredrace);

            // Add race to race list
            var raceRef = recoloredrace.ToReference<BlueprintRaceReference>();
            ref var races = ref BlueprintRoot.Instance.Progression.m_CharacterRaces;

            var length = races.Length;
            Array.Resize(ref races, length + 1);
            races[length] = raceRef;

            // Add portraits
            PortraitCreatonator.RegisterRacePortrait("Duskwalker_F_01", "{7D42A574-CDE0-4C0D-9F3E-B880C85C186B}", race, Gender.Female, "Duskwalker_F_01");
            PortraitCreatonator.RegisterRacePortrait("Duskwalker_M_01", "{3E40C51E-CFBB-421A-81C3-1D690B694832}", race, Gender.Male, "Duskwalker_M_01");
        }
    }
}
