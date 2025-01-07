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
    internal class Drow
    {
        public static List<Color> RaceHeadColors =
            [
                new Color( // Near Black
                    RaceRecolorizer.GetColorsFromRGB(45f),
                    RaceRecolorizer.GetColorsFromRGB(45f),
                    RaceRecolorizer.GetColorsFromRGB(50f)
                    ),
                new Color( // Dark Blue - slight gray
                    RaceRecolorizer.GetColorsFromRGB(74f),
                    RaceRecolorizer.GetColorsFromRGB(74f),
                    RaceRecolorizer.GetColorsFromRGB(120f)
                    ),
                new Color( // Dark Gray
                    RaceRecolorizer.GetColorsFromRGB(90f),
                    RaceRecolorizer.GetColorsFromRGB(90f),
                    RaceRecolorizer.GetColorsFromRGB(95f)
                    ),
                new Color( // Medium Blue
                    RaceRecolorizer.GetColorsFromRGB(94f),
                    RaceRecolorizer.GetColorsFromRGB(102f),
                    RaceRecolorizer.GetColorsFromRGB(175f)
                    ),
                new Color( // Light Gray-Blue
                    RaceRecolorizer.GetColorsFromRGB(128f),
                    RaceRecolorizer.GetColorsFromRGB(136f),
                    RaceRecolorizer.GetColorsFromRGB(190f)
                    ),
                new Color( // Medium Dark Gray with Purple
                    RaceRecolorizer.GetColorsFromRGB(75f),
                    RaceRecolorizer.GetColorsFromRGB(61f),
                    RaceRecolorizer.GetColorsFromRGB(68f)
                    ),
                new Color( // Blue Gray
                    RaceRecolorizer.GetColorsFromRGB(87f),
                    RaceRecolorizer.GetColorsFromRGB(91f),
                    RaceRecolorizer.GetColorsFromRGB(125f)
                    ),
                new Color( // Very Dark Blue
                    RaceRecolorizer.GetColorsFromRGB(44f),
                    RaceRecolorizer.GetColorsFromRGB(44f),
                    RaceRecolorizer.GetColorsFromRGB(99f)
                    ),
                new Color( // Medium Dark Gray-Blue
                    RaceRecolorizer.GetColorsFromRGB(77f),
                    RaceRecolorizer.GetColorsFromRGB(82f),
                    RaceRecolorizer.GetColorsFromRGB(103f)
                    ),
                new Color( // Dark Purple-Blue
                    RaceRecolorizer.GetColorsFromRGB(52f),
                    RaceRecolorizer.GetColorsFromRGB(45f),
                    RaceRecolorizer.GetColorsFromRGB(75f)
                    )
            ];

        public static List<Color> RaceEyeColors =
            [
                new Color( // White
                    RaceRecolorizer.GetColorsFromRGB(182f),
                    RaceRecolorizer.GetColorsFromRGB(182f),
                    RaceRecolorizer.GetColorsFromRGB(182f)
                    ),
                new Color( // Red
                    RaceRecolorizer.GetColorsFromRGB(140f),
                    RaceRecolorizer.GetColorsFromRGB(6f),
                    RaceRecolorizer.GetColorsFromRGB(1f)
                    ),
                new Color( // Very light fuscia
                    RaceRecolorizer.GetColorsFromRGB(233f),
                    RaceRecolorizer.GetColorsFromRGB(196f),
                    RaceRecolorizer.GetColorsFromRGB(241f)
                    ),
                new Color( // Perrywinkle
                    RaceRecolorizer.GetColorsFromRGB(119f),
                    RaceRecolorizer.GetColorsFromRGB(125f),
                    RaceRecolorizer.GetColorsFromRGB(237f)
                    ),
                new Color( // Medium Deep Blue-Purple
                    RaceRecolorizer.GetColorsFromRGB(102f),
                    RaceRecolorizer.GetColorsFromRGB(78f),
                    RaceRecolorizer.GetColorsFromRGB(175f)
                    ),
                new Color( // Light turqois
                    RaceRecolorizer.GetColorsFromRGB(149f),
                    RaceRecolorizer.GetColorsFromRGB(199f),
                    RaceRecolorizer.GetColorsFromRGB(186f)
                    )
            ];

        public static List<Color> RaceHairColors =
            [
                new Color(0.75f, 0.75f, 0.78f), // White
                new Color(0f, 0f, 0f), // Black
                new Color(0.86f, 0.86f, 0.89f) // Super White
            ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.ElfRace.ToString());

        private static readonly string DrowName = "EbonsDrowRace";

        internal const string DrowDisplayName = "Drow.Name";
        private static readonly string DrowDescription = "Drow.Description";
        public static readonly string RaceGuid = "{5D357AB2-BA68-4B76-B7F1-3E8F3FE441C4}";

        internal static void Configure()
        {
            var race =
            RaceConfigurator.New(DrowName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(DrowDisplayName)
                .SetDescription(DrowDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures()
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: -2)
                .SetRaceId(Race.Elf)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, eyecolors: RaceEyeColors, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.OreadRace.ToString()));

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
