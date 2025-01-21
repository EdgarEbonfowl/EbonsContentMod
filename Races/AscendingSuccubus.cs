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
using Kingmaker.Blueprints.Classes.Spells;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Abilities;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using BlueprintCore.Utils.Types;
using EbonsContentMod.Components;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.FactLogic;

namespace EbonsContentMod.Races
{
    
    internal class AscendingSuccubus
    {
        public static List<Color> RaceHeadColors = [];

        public static List<Color> RaceEyeColors = [];

        public static List<Color> RaceHairColors = [];

        public static EquipmentEntityLink[] MaleHeads =
        [
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.AscendingSuccubus.ToString()).MaleOptions.Heads[0],
            new EquipmentEntityLink() {AssetId = "80f9866c56cea9342a354636154d0fae"}, // 11 Evil Male Human
            new EquipmentEntityLink() {AssetId = "554fe1a46ebf83d40a461eb355c290f1"}, // 09 Male Human
        ];

        public static EquipmentEntityLink[] FemaleHeads =
        [
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.AscendingSuccubus.ToString()).FemaleOptions.Heads[0],
            new EquipmentEntityLink() {AssetId = "f0ae81a538f807f46816f8cb78709115"}, // 08 Female Human
            new EquipmentEntityLink() {AssetId = "5bc92526cbe57564f96530aefe05e4c6"}, // 04 Female Human
        ];

        public static EquipmentEntityLink[] FemaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "fb79b0f415a030f4b9983e61b7d480fa"}, // Arueshalae Hair
            new EquipmentEntityLink() {AssetId = "04c3eb6d7570d8d49b686516b7c4a4f8"}, // Long Camelia Hair
            new EquipmentEntityLink() {AssetId = "fc3fc0e45a70a0e42b6aed10baf794f0"}, // Dread Seelah Female Human
            new EquipmentEntityLink() {AssetId = "779458079f7718c4bb960d9cef195339"}, // Long Wavy Braids
            new EquipmentEntityLink() {AssetId = "30d504db6b8cbe94dbc82d2437c8b468"}, // Long Wavy Female Human
            new EquipmentEntityLink() {AssetId = "1f19aaaa1870e2b4b8bd99d36211ddf6"}, // Ponytail Upper Female Human
            new EquipmentEntityLink() {AssetId = "d71d2e53fce0f1d4baad8b20c8266676"}, // Slick Female Tiefling
            new EquipmentEntityLink() {AssetId = "afa22656ed5030c4ba273583ba2b3a16"}, // Long Wild Female Tiefling
            new EquipmentEntityLink() {AssetId = "1762cab3d178f53489f43ab791b87f9c"}, // Noble Braids - Dwarf
            new EquipmentEntityLink() {AssetId = "34bb68b3e4f03be44a1f0611a09530fc"}, // Crown Braids - Dwarf
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"}  // Bald
        ];

        public static EquipmentEntityLink[] MaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "329cf540a8faed64284c067bace8bbc9"}, // Trim Male Human
            new EquipmentEntityLink() {AssetId = "88c2650d77d9a3c4a8a861fa0d8d0aae"}, // Military Male Human
            new EquipmentEntityLink() {AssetId = "def666224ba24df4e954c03049b29a53"}, // Short Human
            new EquipmentEntityLink() {AssetId = "609143dbf7607f6419babaf5748b82dc"}, // Medium Bun Male Human
            new EquipmentEntityLink() {AssetId = "303578a648d8d344b8d3a9a94fe24d5a"}, // Long Wavy Male Human
            new EquipmentEntityLink() {AssetId = "acdcfd7609f88ae49833e4f10656190e"}, // Long Bangs Male Human
            new EquipmentEntityLink() {AssetId = "50eac92ba30862940be4f70d329d070a"}, // Long Wild Male Tiefling
            new EquipmentEntityLink() {AssetId = "222890293b0f66145a400eae3432868d"}, // Mohawk Male Human
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"}  // Bald
        ];
        public static List<Texture2D> CustomEyeRamps = [];


        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.AscendingSuccubus.ToString());

        private static readonly string AscendingSuccubusName = "AscendingSuccubusRace";

        internal const string AscendingSuccubusDisplayName = "AscendingSuccubus.Name";
        private static readonly string AscendingSuccubusDescription = "AscendingSuccubus.Description";
        public static readonly string RaceGuid = "{D2F15E42-167F-4210-AC04-349329A523B5}";

        internal const string SuccubusWingsDisplayName = "AscendingSuccubus.SuccubusWings.Name";
        private static readonly string SuccubusWingsDescription = "AscendingSuccubus.SuccubusWings.Description";

        internal static void Configure()
        {
            var CustomHeadColors = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).MaleOptions.Heads[0].Load().PrimaryRamps;

            var CustomHairColors = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString()).MaleOptions.Hair[0].Load().PrimaryRamps;
            CustomHairColors.AddRange(BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HalfElfRace.ToString()).MaleOptions.Hair[0].Load().PrimaryRamps);

            var MaleHornsEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "cecad00f0d5d83f4ba37aa45c11c1bbe" }, CustomHeadColors, "{43E940B4-86B6-4EB3-8793-D5B9129D8A0B}", true);
            var FemaleHornsEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "ccd35c8508752f04582e7d3a55248afe" }, CustomHeadColors, "{9F03F3C7-EB29-4F0A-A2DF-90C106E0CF7A}", true);

            EquipmentEntityLink[] HornsLinks = [MaleHornsEE, FemaleHornsEE];

            var NewSuccubusWingsBuff = BuffConfigurator.New("NewAscendingSuccubusWingsBuff", "{88E549DE-478E-4589-AF36-51C8EB63E06C}")
                .CopyFrom(BuffRefs.BuffWingsDemon, c => c is not AddEquipmentEntity)
                .Configure();

            var MaleWings = FeatureConfigurator.New("MaleAscendingSuccubusWings", "{519F0A93-22EC-4300-89AE-97D4E558034C}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("23b3eed8f78e69c40a2c6d416cac2f9e")
                .AddEquipmentEntity(MaleHornsEE)
                .Configure();

            var FemaleWings = FeatureConfigurator.New("FemaleAscendingSuccubusWings", "{21D50270-33B5-40AD-9D3A-B31B83C56C59}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("317c89eb4850e0a45a5eb3e4ae0124a9")
                .AddEquipmentEntity(FemaleHornsEE)
                .Configure();

            var NewSuccubusWingsFeature = ProgressionConfigurator.New("NewAscendingSuccubusWingsFeature", "{31DF9402-18E4-4FB8-86B9-DDAF1A2F3733}")
                .SetDisplayName(SuccubusWingsDisplayName)
                .SetDescription(SuccubusWingsDescription)
                .SetIcon(FeatureRefs.FeatureWingsDraconicWhite.Reference.Get().Icon)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddToLevelEntries(1, MaleWings, FemaleWings)
                .AddFacts([NewSuccubusWingsBuff])
                .Configure();

            FeatureConfigurator.For(FeatureRefs.AscendingSuccubusDR)
                .SetIcon(AbilityRefs.ProtectionFromEnergy.Reference.Get().Icon)
                .Configure();

            FeatureConfigurator.For(FeatureRefs.AscendingSuccubusVampiricTouchFeature)
                .SetDisplayName(AbilityRefs.VampiricTouch.Reference.Get().m_DisplayName)
                .SetIcon(AbilityRefs.VampiricTouch.Reference.Get().Icon)
                .Configure();

            var race =
            RaceConfigurator.New(AscendingSuccubusName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(AscendingSuccubusDisplayName)
                .SetDescription(AscendingSuccubusDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(FeatureRefs.AscendingSuccubusDR.ToString(), FeatureRefs.AscendingSuccubusVampiricTouchFeature.ToString(), NewSuccubusWingsFeature)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2)
                .SetRaceId(Race.Tiefling)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.SuccubusIncubusRace.ToString()), CustomMaleHeads: MaleHeads, CustomFemaleHeads: FemaleHeads, CustomFemaleHairs: FemaleHairs, CustomMaleHairs: MaleHairs, CustomHairRamps: CustomHairColors, CustomHeadRamps: CustomHeadColors, CustomEyeRamps: CustomEyeRamps, RaceHasTail: true);

            // Register linked EEs
            EELinker.RegisterSkinLink(recoloredrace, HornsLinks);

            // Add race to mount fixes
            RaceMountFixerizer.AddRaceToMountFixes(recoloredrace, BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()));

            // Add race to race list
            var raceRef = recoloredrace.ToReference<BlueprintRaceReference>();
            ref var races = ref BlueprintRoot.Instance.Progression.m_CharacterRaces;

            var length = races.Length;
            Array.Resize(ref races, length + 1);
            races[length] = raceRef;
        }
    }
}
