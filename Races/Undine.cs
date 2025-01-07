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

namespace EbonsContentMod.Races
{
    internal class Undine
    {
        public static List<Color> RaceHeadColors =
            [
                new Color( // Light Seafoam
                    RaceRecolorizer.GetColorsFromRGB(134f),
                    RaceRecolorizer.GetColorsFromRGB(162f),
                    RaceRecolorizer.GetColorsFromRGB(164f)
                    )
            ];

        public static List<Color> RaceEyeColors =
            [
                new Color( // White
                    RaceRecolorizer.GetColorsFromRGB(182f),
                    RaceRecolorizer.GetColorsFromRGB(182f),
                    RaceRecolorizer.GetColorsFromRGB(182f)
                    )
            ];

        public static List<Color> RaceHairColors =
            [
                new Color( // Very blue
                    RaceRecolorizer.GetColorsFromRGB(68f),
                    RaceRecolorizer.GetColorsFromRGB(106f),
                    RaceRecolorizer.GetColorsFromRGB(200f)
                    ),
                new Color( // Very light blue
                    RaceRecolorizer.GetColorsFromRGB(183f),
                    RaceRecolorizer.GetColorsFromRGB(196f),
                    RaceRecolorizer.GetColorsFromRGB(213f)
                    ),
                new Color( // Very light purple
                    RaceRecolorizer.GetColorsFromRGB(222f),
                    RaceRecolorizer.GetColorsFromRGB(196f),
                    RaceRecolorizer.GetColorsFromRGB(241f)
                    )
            ];

        public static EquipmentEntityLink[] MaleHeads =
            [
                BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).MaleOptions.Heads[0],
                BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).MaleOptions.Heads[3]
            ];

        public static EquipmentEntityLink[] FemaleHeads =
            [
                BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).FemaleOptions.Heads[0],
                BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).FemaleOptions.Heads[3]
            ];

        public static EquipmentEntityLink[] FemaleHairs =
            [
                new EquipmentEntityLink() {AssetId = "afa22656ed5030c4ba273583ba2b3a16"}, // Long Wild Tiefling
                new EquipmentEntityLink() {AssetId = "3897a5ce68b630548bb85db0a68a465a"}  // Tight French Braid
            ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string UndineName = "UndineRace";

        internal const string UndineDisplayName = "Undine.Name";
        private static readonly string UndineDescription = "Undine.Description";
        public static readonly string RaceGuid = "{E7D00F27-6230-42D2-896F-D5215F5876D8}";

        internal static void Configure()
        {
            var NewFemaleHairArray = FemaleHairs.AppendToArray(CopyRace.FemaleOptions.Hair);

            var race =
            RaceConfigurator.New(UndineName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(UndineDisplayName)
                .SetDescription(UndineDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures()
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: -2)
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, eyecolors: RaceEyeColors, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.OreadRace.ToString()), CustomMaleHeads: MaleHeads, CustomFemaleHeads: FemaleHeads, CustomFemaleHairs: NewFemaleHairArray);

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
