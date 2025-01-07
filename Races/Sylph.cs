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
    internal class Sylph
    {
        public static List<Color> RaceHeadColors =
            [
                new Color( // Light gray with slight blue
                    RaceRecolorizer.GetColorsFromRGB(161f),
                    RaceRecolorizer.GetColorsFromRGB(171f),
                    RaceRecolorizer.GetColorsFromRGB(190f)
                    ),
                new Color( // Light Seafoam
                    RaceRecolorizer.GetColorsFromRGB(134f),
                    RaceRecolorizer.GetColorsFromRGB(162f),
                    RaceRecolorizer.GetColorsFromRGB(164f)
                    ),
                new Color( // Light gray with slight green
                    RaceRecolorizer.GetColorsFromRGB(161f),
                    RaceRecolorizer.GetColorsFromRGB(171f),
                    RaceRecolorizer.GetColorsFromRGB(163f)
                    ),
                new Color( // Light gray
                    RaceRecolorizer.GetColorsFromRGB(139f),
                    RaceRecolorizer.GetColorsFromRGB(139f),
                    RaceRecolorizer.GetColorsFromRGB(139f)
                    ),
                new Color( // Light greige
                    RaceRecolorizer.GetColorsFromRGB(143f),
                    RaceRecolorizer.GetColorsFromRGB(121f),
                    RaceRecolorizer.GetColorsFromRGB(114f)
                    ),
                new Color( // Light gray with yellow
                    RaceRecolorizer.GetColorsFromRGB(162f),
                    RaceRecolorizer.GetColorsFromRGB(146f),
                    RaceRecolorizer.GetColorsFromRGB(136f)
                    ),
                new Color( // Light gray with red
                    RaceRecolorizer.GetColorsFromRGB(170f),
                    RaceRecolorizer.GetColorsFromRGB(146f),
                    RaceRecolorizer.GetColorsFromRGB(146f)
                    )
            ];

        public static List<Color> RaceEyeColors =
            [
                new Color( // Light Blue
                    RaceRecolorizer.GetColorsFromRGB(133f),
                    RaceRecolorizer.GetColorsFromRGB(154f),
                    RaceRecolorizer.GetColorsFromRGB(190f)
                    ),
                new Color( // Perrywinkle
                    RaceRecolorizer.GetColorsFromRGB(119f),
                    RaceRecolorizer.GetColorsFromRGB(125f),
                    RaceRecolorizer.GetColorsFromRGB(237f)
                    ),
                new Color( // Very light fuscia
                    RaceRecolorizer.GetColorsFromRGB(233f),
                    RaceRecolorizer.GetColorsFromRGB(196f),
                    RaceRecolorizer.GetColorsFromRGB(241f)
                    ),
                new Color( // Light turqois
                    RaceRecolorizer.GetColorsFromRGB(149f),
                    RaceRecolorizer.GetColorsFromRGB(199f),
                    RaceRecolorizer.GetColorsFromRGB(186f)
                    ),
                new Color( // Gray
                    RaceRecolorizer.GetColorsFromRGB(131f),
                    RaceRecolorizer.GetColorsFromRGB(131f),
                    RaceRecolorizer.GetColorsFromRGB(131f)
                    ),
                new Color( // White
                    RaceRecolorizer.GetColorsFromRGB(182f),
                    RaceRecolorizer.GetColorsFromRGB(182f),
                    RaceRecolorizer.GetColorsFromRGB(182f)
                    )
            ];

        public static List<Color> RaceHairColors =
            [
                new Color(0.83f, 0.83f, 0.83f), // White
                new Color(0.89f, 0.89f, 0.91f), // Super White
                new Color( // White with slight blue
                    RaceRecolorizer.GetColorsFromRGB(204f),
                    RaceRecolorizer.GetColorsFromRGB(199f),
                    RaceRecolorizer.GetColorsFromRGB(234f)
                    ),
                new Color( // White with slight red
                    RaceRecolorizer.GetColorsFromRGB(234f),
                    RaceRecolorizer.GetColorsFromRGB(199f),
                    RaceRecolorizer.GetColorsFromRGB(213f)
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

        public static EquipmentEntityLink[] FemaleHairs =
            [
                new EquipmentEntityLink() {AssetId = "779458079f7718c4bb960d9cef195339"},
                new EquipmentEntityLink() {AssetId = "04c3eb6d7570d8d49b686516b7c4a4f8"},
                new EquipmentEntityLink() {AssetId = "3897a5ce68b630548bb85db0a68a465a"}
            ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string SylphName = "SylphRace";

        internal const string SylphDisplayName = "Sylph.Name";
        private static readonly string SylphDescription = "Sylph.Description";
        public static readonly string RaceGuid = "{8EFDA376-63BD-45F9-A17E-5FB180074ABD}";

        internal static void Configure()
        {
            var NewFemaleHairArray = FemaleHairs.AppendToArray(CopyRace.FemaleOptions.Hair);

            var AirTattoos = FeatureConfigurator.New("SylphAirMarkings", "{39368AA3-FD47-4B2F-945B-A3E14CA80EEE}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddEquipmentEntity("aecc5905323948449b4cd3bfe36e5daf")
                .Configure();

            var race =
            RaceConfigurator.New(SylphName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(SylphDisplayName)
                .SetDescription(SylphDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(AirTattoos)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: -2)
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, eyecolors: RaceEyeColors, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.KitsuneRace.ToString()), CustomFemaleHairs: NewFemaleHairArray);

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
