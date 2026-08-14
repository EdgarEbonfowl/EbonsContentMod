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
using EbonsContentMod.Components;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.CharacterSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.FactLogic;

namespace EbonsContentMod.Races
{
    internal class Triaxian
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

        public static List<Texture2D> CustomHeadRamps =
        [
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 6), // Salmon-Coral            
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 7), // Rust
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 11), // Red
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 12), // Very Red
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 9), // Light Bronze
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 8), // Sand Gray
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 0), // Sand Brown
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 2), // Light Brown
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 3), // Gray Brown
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 9), // Red Brown
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 10), // Dark Brown
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.ElfRace.Reference.Get(), 6), // Chestnut
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.ElfRace.Reference.Get(), 8), // Dark Chestnut
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.DwarfRace.Reference.Get(), 6), // Muted Brown
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 7), // Deep Coral
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 8), // Dark Coral
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 1), // Light Gray-Brown
        ];

        public static List<Texture2D> CustomHairRamps =
        [

        ];

        public static EquipmentEntityLink[] MaleHairs =
        [

        ];

        // Good amount of clipping in all these, but oh well
        public static EquipmentEntityLink[] FemaleHairs =
        [

        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HalfElfRace.ToString());

        private static readonly string TriaxianName = "TriaxianRace";

        internal const string TriaxianDisplayName = "Triaxian.Name";
        private static readonly string TriaxianDescription = "Triaxian.Description";
        public static readonly string RaceGuid = "{0DD8AE1B-D7F5-43E6-A75F-0B5B661A0C42}";

        internal const string SeasonedDisplayName = "Triaxian.Seasoned.Name";
        private static readonly string SeasonedDescription = "Triaxian.Seasoned.Description";

        internal const string SummerDisplayName = "Triaxian.Summer.Name";

        internal const string WinterDisplayName = "Triaxian.Winter.Name";

        internal const string ExceptionalSensesDisplayName = "Triaxian.ExceptionalSenses.Name";
        private static readonly string ExceptionalSensesDescription = "Triaxian.ExceptionalSenses.Description";

        internal static void Configure()
        {
            var winterSkin = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "1aeb459da29dca341a78317170eec262" },
                RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Dark Gray
                    RaceRecolorizer.GetColorsFromRGB(100f),
                    RaceRecolorizer.GetColorsFromRGB(100f),
                    RaceRecolorizer.GetColorsFromRGB(130f)
                    )}
            ), "{E5398996-144F-47C5-AA92-BE8106A9268B}", true, true,
            RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Near White
                    RaceRecolorizer.GetColorsFromRGB(204f),
                    RaceRecolorizer.GetColorsFromRGB(212f),
                    RaceRecolorizer.GetColorsFromRGB(219f)
                    )}), BodyPartsToRemove: [BodyPartType.Eyes]);

            // Seasoned
            var winter = FeatureConfigurator.New("TriaxianWinterFeature", "{D57FE4DE-170E-4D63-B934-25B448B38FBD}")
                .SetDisplayName(WinterDisplayName)
                .SetDescription(SeasonedDescription)
                .SetIcon(AbilityRefs.BreakEnchantment.Reference.Get().Icon)
                .AddResistEnergy(type: DamageEnergyType.Cold, value: 2)
                .AddEquipmentEntity(winterSkin)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var summer = FeatureConfigurator.New("TriaxianSummerFeature", "{477806C0-9CFC-49AB-90BD-7DAFEC1D6A53}")
                .SetDisplayName(SummerDisplayName)
                .SetDescription(SeasonedDescription)
                .SetIcon(AbilityRefs.Sunbeam.Reference.Get().Icon)
                .AddResistEnergy(type: DamageEnergyType.Fire, value: 2)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var seasoned = FeatureSelectionConfigurator.New("TriaxianSeasonedFeatureSelection", "{52CBBA96-7629-4D25-9AF2-97934DC98DF6}")
                .SetDisplayName(SeasonedDisplayName)
                .SetDescription(SeasonedDescription)
                .SetIcon(AbilityRefs.RainbowDome.Reference.Get().Icon)
                .AddToAllFeatures(summer, winter)
                .SetGroup(FeatureGroup.Racial)
                .Configure();

            var exceptionalSenses = FeatureConfigurator.New("TriaxianExceptionalSenses", "{8593D7A4-7645-4294-A253-64A824CFC65F}")
                .CopyFrom(FeatureRefs.KeenSenses, c => c is not AddStatBonus)
                .SetDisplayName(ExceptionalSensesDisplayName)
                .SetDescription(ExceptionalSensesDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillPerception, value: 4)
                .Configure();
            
            var race =
            RaceConfigurator.New(TriaxianName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(TriaxianDisplayName)
                .SetDescription(TriaxianDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(FeatureSelectionRefs.BasicFeatSelection.ToString(), seasoned, exceptionalSenses)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: -2)
                .SetRaceId(Race.HalfElf)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, CustomHeadRamps: CustomHeadRamps, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString()), BaldRace: true, NoBeards: true, NoEyebrows: true);

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

            // Add Portraits
            PortraitCreatonator.RegisterRacePortrait("Triaxian_F_01", "{30EA0979-00B2-4E27-9AEB-65ED737CEB65}", race, Gender.Female, "Triaxian_F_01");
            PortraitCreatonator.RegisterRacePortrait("Triaxian_M_01", "{12FE4F2C-1B34-4AB5-AED6-C8F666A3031B}", race, Gender.Male, "Triaxian_M_01");
        }
    }
}
