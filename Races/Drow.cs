using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using System;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using EbonsContentMod.Utilities;
using UnityEngine;
using Kingmaker.ResourceLinks;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;

namespace EbonsContentMod.Races
{
    internal class Drow
    {
        public static List<Color> RaceHeadColors = [];

        public static List<Color> RaceEyeColors = [];

        public static List<Color> RaceHairColors = [];

        public static List<Texture2D> CustomEyeRamps =
        [
            RaceRecolorizer.GetArmorRampByIndex(4),
            RaceRecolorizer.GetArmorRampByIndex(5),
            RaceRecolorizer.GetArmorRampByIndex(7),
            RaceRecolorizer.GetArmorRampByIndex(10),
            RaceRecolorizer.GetArmorRampByIndex(11),
            RaceRecolorizer.GetArmorRampByIndex(12),
            RaceRecolorizer.GetArmorRampByIndex(13),
            RaceRecolorizer.GetArmorRampByIndex(57),
            RaceRecolorizer.GetArmorRampByIndex(61),
            RaceRecolorizer.GetArmorRampByIndex(62),
            RaceRecolorizer.GetArmorRampByIndex(63),
            RaceRecolorizer.GetArmorRampByIndex(64),
            RaceRecolorizer.GetArmorRampByIndex(65),
            RaceRecolorizer.GetArmorRampByIndex(66),
            RaceRecolorizer.GetArmorRampByIndex(67),
            RaceRecolorizer.GetArmorRampByIndex(68),
            RaceRecolorizer.GetArmorRampByIndex(69)
        ];

        public static List<Texture2D> CustomHeadRamps =
        [
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 4),
            RaceRecolorizer.CreateSingleRampFromColor(new Color( // Near Black
                RaceRecolorizer.GetColorsFromRGB(45f),
                RaceRecolorizer.GetColorsFromRGB(45f),
                RaceRecolorizer.GetColorsFromRGB(50f)
                )),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 6),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 3),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 4),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 5),
            RaceRecolorizer.CreateSingleRampFromColor(new Color( // Dark Blue - slight gray
                RaceRecolorizer.GetColorsFromRGB(74f),
                RaceRecolorizer.GetColorsFromRGB(74f),
                RaceRecolorizer.GetColorsFromRGB(120f)
                )),
            RaceRecolorizer.CreateSingleRampFromColor(new Color( // Medium Blue
                RaceRecolorizer.GetColorsFromRGB(94f),
                RaceRecolorizer.GetColorsFromRGB(102f),
                RaceRecolorizer.GetColorsFromRGB(175f)
                )),
            RaceRecolorizer.CreateSingleRampFromColor(new Color( // Light Gray-Blue
                RaceRecolorizer.GetColorsFromRGB(128f),
                RaceRecolorizer.GetColorsFromRGB(136f),
                RaceRecolorizer.GetColorsFromRGB(190f)
                )),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 13),
            RaceRecolorizer.CreateSingleRampFromColor(new Color( // Dark Gray
                RaceRecolorizer.GetColorsFromRGB(90f),
                RaceRecolorizer.GetColorsFromRGB(90f),
                RaceRecolorizer.GetColorsFromRGB(95f)
                )),
            RaceRecolorizer.CreateSingleRampFromColor(new Color( // Medium Dark Gray with Purple
                RaceRecolorizer.GetColorsFromRGB(75f),
                RaceRecolorizer.GetColorsFromRGB(61f),
                RaceRecolorizer.GetColorsFromRGB(68f)
                )),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 7),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 8),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 9),
            RaceRecolorizer.CreateSingleRampFromColor(new Color( // Blue Gray
                RaceRecolorizer.GetColorsFromRGB(87f),
                RaceRecolorizer.GetColorsFromRGB(91f),
                RaceRecolorizer.GetColorsFromRGB(125f)
                )),
            RaceRecolorizer.CreateSingleRampFromColor(new Color( // Very Dark Blue
                RaceRecolorizer.GetColorsFromRGB(44f),
                RaceRecolorizer.GetColorsFromRGB(44f),
                RaceRecolorizer.GetColorsFromRGB(99f)
                )),
            RaceRecolorizer.CreateSingleRampFromColor(new Color( // Medium Dark Gray-Blue
                RaceRecolorizer.GetColorsFromRGB(77f),
                RaceRecolorizer.GetColorsFromRGB(82f),
                RaceRecolorizer.GetColorsFromRGB(103f)
                )),
            RaceRecolorizer.CreateSingleRampFromColor(new Color( // Dark Purple-Blue
                RaceRecolorizer.GetColorsFromRGB(52f),
                RaceRecolorizer.GetColorsFromRGB(45f),
                RaceRecolorizer.GetColorsFromRGB(75f)
                ))
        ];

        public static List<Texture2D> CustomHairRamps =
        [
            RaceRecolorizer.GetRaceHairRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 0),
            RaceRecolorizer.CreateSingleRampFromColor(new Color(0.75f, 0.75f, 0.78f)), // White
            RaceRecolorizer.CreateSingleRampFromColor(new Color(0f, 0f, 0f)), // Black
            RaceRecolorizer.CreateSingleRampFromColor(new Color(0.86f, 0.86f, 0.89f)) // Super White
        ];

        public static EquipmentEntityLink[] MaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "d69743688fc27584887a7c29a774289e"}, // Long Straight Male Elf
            new EquipmentEntityLink() {AssetId = "e6cb686fb8663734f96ceeacfa2e2400"}, // Long Braids Male Elf
            new EquipmentEntityLink() {AssetId = "8e9394a7a860ead42b2d5acdfb35e3f5"}, // Ponytail Classic Male Elf
            new EquipmentEntityLink() {AssetId = "76cfc339d08471f4da919dcbdd2fceb1"}, // Short Male Half-Elf
            new EquipmentEntityLink() {AssetId = "4da3c565974930a40b51950ee671895e"}, // Slick Male Elf
            new EquipmentEntityLink() {AssetId = "54aae291e3449c14792bbe2592228d4d"}, // Medium Tiny Braid Male Elf
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"}  // Bald
        ];

        public static EquipmentEntityLink[] FemaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "db566d919de425443bd9ae0a37de3ec9"}, // French Braid Female Elf
            new EquipmentEntityLink() {AssetId = "21099c170f7b8344d90d6f034fb5554c"}, // Pompadour Female Elf
            new EquipmentEntityLink() {AssetId = "57c595a0dece66f4283e888dc52d9df1"}, // Long Female Elf
            new EquipmentEntityLink() {AssetId = "c8edacbc502d42242a5911ba000a411e"}, // Long Wavy Braids Female Half-Elf
            new EquipmentEntityLink() {AssetId = "131955108c91c2448a78f8021ca358a9"}, // Medium Anevia Female Elf
            new EquipmentEntityLink() {AssetId = "304b84351ffbde24190e56724178df5b"}, // Long Ember Female Elf
            new EquipmentEntityLink() {AssetId = "9ec441743ea20c5488e7f497992042ed"}, // Ponytail Lush Female Elf
            new EquipmentEntityLink() {AssetId = "39d65ebde5c324f41821b36258791ee5"}, // Side Kare Female Elf
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"}  // Bald
        ];

        // Need to implement this in the recolorizer first
        public static EquipmentEntityLink[] NewSkinLinkedEEs =
        [
            new EquipmentEntityLink() { AssetId = "fe3418fd8c435cf4384b0d7053283871" } // Lipstick
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.ElfRace.ToString());

        private static readonly string DrowName = "EbonsDrowRace";

        internal const string DrowDisplayName = "Drow.Name";
        private static readonly string DrowDescription = "Drow.Description";
        public static readonly string RaceGuid = "{5D357AB2-BA68-4B76-B7F1-3E8F3FE441C4}";

        internal const string WeaponFamiliarityDisplayName = "Drow.WeaponFamiliarity.Name";
        private static readonly string WeaponFamiliarityDescription = "Drow.WeaponFamiliarity.Description";

        internal const string ImmunitiesDisplayName = "Drow.Immunities.Name";
        private static readonly string ImmunitiesDescription = "Drow.Immunities.Description";

        internal const string SpellResistanceDisplayName = "Drow.SpellResistance.Name";
        private static readonly string SpellResistanceDescription = "Drow.SpellResistance.Description";

        internal const string SpellLikeDisplayName = "Drow.SpellLike.Name";
        private static readonly string SpellLikeDescription = "Drow.SpellLike.Description";

        internal const string AncestralGrudgeDisplayName = "Drow.AncestralGrudge.Name";
        private static readonly string AncestralGrudgeDescription = "Drow.AncestralGrudge.Description";

        internal static void Configure()
        {
            if (CheckerUtilities.GetModActive("DP_WOTR_PlayableRaceExp")) return;
            
            var ContactEEL = new EquipmentEntityLink() { AssetId = "c5487e7e903d25a40be683767f3df0b4" };

            var WeaponFamiliarity = FeatureConfigurator.New("EbonsDrowWeaponFamiliarity", "{3C5719FD-D3D9-49A6-884B-5A164DA52161}")
                .SetDisplayName(WeaponFamiliarityDisplayName)
                .SetDescription(WeaponFamiliarityDescription)
                .SetIcon(FeatureRefs.ElvenWeaponFamiliarity.Reference.Get().Icon)
                .AddProficiencies(weaponProficiencies: [WeaponCategory.HandCrossbow, WeaponCategory.Rapier, WeaponCategory.Shortsword])
                .SetGroups(FeatureGroup.Racial)
                .Configure();
            
            var Immunities = FeatureConfigurator.New("EbonsDrowImmunities", "{63E861DE-09DC-4343-8506-AD51E4B996DF}")
                .CopyFrom(FeatureRefs.ElvenImmunities)
                .SetDisplayName(ImmunitiesDisplayName)
                .SetDescription(ImmunitiesDescription)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var SpellResistance = FeatureConfigurator.New("EbonsDrowSpellResistance", "{67E1769C-C6A9-4009-A6AF-1F7CA903F0C5}")
                .SetDisplayName(SpellResistanceDisplayName)
                .SetDescription(SpellResistanceDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.SpellResistance.ToString()).Icon)
                .AddComponent<AddSpellResistance>(c =>
                {
                    c.Value = new ContextValue()
                    {
                        ValueType = ContextValueType.Shared,
                        ValueRank = AbilityRankType.Default,
                        ValueShared = AbilitySharedValue.Damage
                    };
                })
                .AddComponent<ContextCalculateSharedValue>(c =>
                {
                    c.ValueType = AbilitySharedValue.Damage;
                    c.Value = new ContextDiceValue()
                    {
                        DiceType = Kingmaker.RuleSystem.DiceType.One,
                        DiceCountValue = new ContextValue()
                        {
                            ValueType = ContextValueType.Rank
                        },
                        BonusValue = 6,
                    };
                    c.Modifier = 1.0;
                })
                .AddContextRankConfig(new ContextRankConfig()
                {
                    m_BaseValueType = ContextRankBaseValueType.CharacterLevel,
                    m_SpecificModifier = ModifierDescriptor.None,
                    m_Progression = ContextRankProgression.AsIs
                })
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            // Spell-Like Abilities
            var SpellLikeResource = AbilityResourceConfigurator.New("EbonsDrowSpellLikeResource", "{D6CA536A-0965-4BFE-861D-88A6147A31D9}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var SpellLikeAbility = AbilityConfigurator.New("EbonsDrowSpellLikeAbility", "{AE018970-F315-4795-9CB7-BDDFB74CD1B2}")
                .CopyFrom(AbilityRefs.FaerieFire, c => c is not (SpellListComponent or CraftInfoComponent))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 1)
                .Configure();

            var SpellLikeFeat = FeatureConfigurator.New("EbonsDrowSpellLikeFeature", "{19F08BF1-3B22-4ADF-8737-7AA9D48599D0}")
                .SetDisplayName(SpellLikeDisplayName)
                .SetDescription(SpellLikeDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.FaerieFire.ToString()).Icon)
                .AddFacts(new() { SpellLikeAbility })
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: SpellLikeAbility)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var AncestralGrudge = FeatureConfigurator.New("EbonsDrowAncestralGrudge", "{238942FB-B8FD-490D-9E3D-AF89E83AC695}")
                .SetDisplayName(AncestralGrudgeDisplayName)
                .SetDescription(AncestralGrudgeDescription)
                .SetIcon(FeatureRefs.FavoriteEnemyElfs.Reference.Get().Icon)
                .AddAttackBonusAgainstFactOwner(1, checkedFact: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.ElfRace.ToString()))
                .AddAttackBonusAgainstFactOwner(1, checkedFact: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HalfElfRace.ToString()))
                .AddAttackBonusAgainstFactOwner(1, checkedFact: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DwarfRace.ToString()))
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var LightSensitivity = BlueprintTools.GetBlueprint<BlueprintFeature>("{BBA86DB1-B3EF-4AB5-A412-A5910F2B8947}");

            var race =
            RaceConfigurator.New(DrowName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(DrowDisplayName)
                .SetDescription(DrowDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(WeaponFamiliarity, Immunities, SpellResistance, FeatureRefs.KeenSenses.ToString(), SpellLikeFeat, AncestralGrudge, LightSensitivity)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: -2)
                .SetRaceId(Race.Elf)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, CustomEyeRamps: CustomEyeRamps, CustomHeadRamps: CustomHeadRamps, CustomHairRamps: CustomHairRamps, eyeEE: ContactEEL, CustomFemaleHairs: FemaleHairs, CustomMaleHairs: MaleHairs);

            // Add race to mount fixes
            RaceMountFixerizer.AddRaceToMountFixes(recoloredrace, CopyRace);

            // Add race to race list
            var raceRef = recoloredrace.ToReference<BlueprintRaceReference>();
            ref var races = ref BlueprintRoot.Instance.Progression.m_CharacterRaces;

            var length = races.Length;
            Array.Resize(ref races, length + 1);
            races[length] = raceRef;
        }
    }
}
