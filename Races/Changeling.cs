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

namespace EbonsContentMod.Races
{
    internal class Changeling
    {
        public static List<Color> RaceHeadColors =
            [
                new Color( // Near White
                    RaceRecolorizer.GetColorsFromRGB(160f),
                    RaceRecolorizer.GetColorsFromRGB(160f),
                    RaceRecolorizer.GetColorsFromRGB(165f)
                    ),
                new Color( // Very Lgiht Gray
                    RaceRecolorizer.GetColorsFromRGB(130f),
                    RaceRecolorizer.GetColorsFromRGB(130f),
                    RaceRecolorizer.GetColorsFromRGB(135f)
                    ),
                new Color( // Gray
                    RaceRecolorizer.GetColorsFromRGB(100f),
                    RaceRecolorizer.GetColorsFromRGB(100f),
                    RaceRecolorizer.GetColorsFromRGB(105f)
                    ),
                new Color( // Dark Gray
                    RaceRecolorizer.GetColorsFromRGB(70f),
                    RaceRecolorizer.GetColorsFromRGB(70f),
                    RaceRecolorizer.GetColorsFromRGB(75f)
                    ),
                new Color( // Black
                    RaceRecolorizer.GetColorsFromRGB(40f),
                    RaceRecolorizer.GetColorsFromRGB(40f),
                    RaceRecolorizer.GetColorsFromRGB(45f)
                    )
            ];

        public static List<Color> RaceEyeColors =
            [
                new Color( // Yellow
                    RaceRecolorizer.GetColorsFromRGB(220f),
                    RaceRecolorizer.GetColorsFromRGB(220f),
                    RaceRecolorizer.GetColorsFromRGB(45f)
                    ),
                new Color( // Greenish Yellow
                    RaceRecolorizer.GetColorsFromRGB(170f),
                    RaceRecolorizer.GetColorsFromRGB(200f),
                    RaceRecolorizer.GetColorsFromRGB(45f)
                    ),
                new Color( // Blue-Green
                    RaceRecolorizer.GetColorsFromRGB(0f),
                    RaceRecolorizer.GetColorsFromRGB(100f),
                    RaceRecolorizer.GetColorsFromRGB(260f)
                    )
            ];

        public static List<Color> RaceHairColors =
            [
                new Color( // Very Lgiht Gray
                    RaceRecolorizer.GetColorsFromRGB(130f),
                    RaceRecolorizer.GetColorsFromRGB(130f),
                    RaceRecolorizer.GetColorsFromRGB(135f)
                    ),
                new Color( // Black
                    RaceRecolorizer.GetColorsFromRGB(40f),
                    RaceRecolorizer.GetColorsFromRGB(40f),
                    RaceRecolorizer.GetColorsFromRGB(45f)
                    ),
                new Color( // Gray
                    RaceRecolorizer.GetColorsFromRGB(100f),
                    RaceRecolorizer.GetColorsFromRGB(100f),
                    RaceRecolorizer.GetColorsFromRGB(105f)
                    ),
                new Color( // Dark Gray
                    RaceRecolorizer.GetColorsFromRGB(70f),
                    RaceRecolorizer.GetColorsFromRGB(70f),
                    RaceRecolorizer.GetColorsFromRGB(75f)
                    ),
                new Color( // Near White
                    RaceRecolorizer.GetColorsFromRGB(160f),
                    RaceRecolorizer.GetColorsFromRGB(160f),
                    RaceRecolorizer.GetColorsFromRGB(165f)
                    )
            ];

        public static EquipmentEntityLink[] MaleHairs =
            [

            ];

        // Good amount of clipping in all these, but oh well
        public static EquipmentEntityLink[] FemaleHairs =
            [

            ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DhampirRace.ToString());

        private static readonly string ChangelingName = "ChangelingRace";

        internal const string ChangelingDisplayName = "Changeling.Name";
        private static readonly string ChangelingDescription = "Changeling.Description";
        public static readonly string RaceGuid = "{BC9B8D87-9D10-4455-895C-98E31F8D8503}";

        internal static void Configure()
        {
            var race =
            RaceConfigurator.New(ChangelingName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(ChangelingDisplayName)
                .SetDescription(ChangelingDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures()
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: -2)
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, eyecolors: RaceEyeColors, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.AasimarRace.ToString())/*, CustomFemaleHairs: FemaleHairs, CustomMaleHairs: MaleHairs*/);

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
