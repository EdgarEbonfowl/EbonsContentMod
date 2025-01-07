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
    internal class Suli
    {
        public static List<Color> RaceHeadColors =
            [
                new Color( // Bronze
                    RaceRecolorizer.GetColorsFromRGB(126f),
                    RaceRecolorizer.GetColorsFromRGB(101f),
                    RaceRecolorizer.GetColorsFromRGB(73f)
                    ),
                new Color( // Ruddy
                    RaceRecolorizer.GetColorsFromRGB(113f),
                    RaceRecolorizer.GetColorsFromRGB(83f),
                    RaceRecolorizer.GetColorsFromRGB(78f)
                    ),
                new Color( // Deep Bronze
                    RaceRecolorizer.GetColorsFromRGB(116f),
                    RaceRecolorizer.GetColorsFromRGB(86f),
                    RaceRecolorizer.GetColorsFromRGB(50f)
                    ),
                new Color( // Gold
                    RaceRecolorizer.GetColorsFromRGB(161f),
                    RaceRecolorizer.GetColorsFromRGB(139f),
                    RaceRecolorizer.GetColorsFromRGB(96f)
                    ),
                new Color( // Silver
                    RaceRecolorizer.GetColorsFromRGB(136f),
                    RaceRecolorizer.GetColorsFromRGB(132f),
                    RaceRecolorizer.GetColorsFromRGB(132f)
                    ),
                new Color( // Sand
                    RaceRecolorizer.GetColorsFromRGB(111f),
                    RaceRecolorizer.GetColorsFromRGB(89f),
                    RaceRecolorizer.GetColorsFromRGB(62f)
                    ),
                new Color( // Pink stone
                    RaceRecolorizer.GetColorsFromRGB(169f),
                    RaceRecolorizer.GetColorsFromRGB(132f),
                    RaceRecolorizer.GetColorsFromRGB(132f)
                    ),
                new Color( // Deep Earth
                    RaceRecolorizer.GetColorsFromRGB(76f),
                    RaceRecolorizer.GetColorsFromRGB(59f),
                    RaceRecolorizer.GetColorsFromRGB(37f)
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
                new EquipmentEntityLink() {AssetId = "afa22656ed5030c4ba273583ba2b3a16"}, // Long Wild Tiefling
                new EquipmentEntityLink() {AssetId = "04c3eb6d7570d8d49b686516b7c4a4f8"}, // Long Camelia Hair
                new EquipmentEntityLink() {AssetId = "779458079f7718c4bb960d9cef195339"}, // Long Wavy Braids
                new EquipmentEntityLink() {AssetId = "1762cab3d178f53489f43ab791b87f9c"}, // Noble Braids - Dwarf
                new EquipmentEntityLink() {AssetId = "3897a5ce68b630548bb85db0a68a465a"}  // Tight French Braid
            ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string SuliName = "SuliRace";

        internal const string SuliDisplayName = "Suli.Name";
        private static readonly string SuliDescription = "Suli.Description";
        public static readonly string RaceGuid = "{D5398269-CC14-42D7-8024-69CBE7FDF151}";

        internal static void Configure()
        {
            var NewFemaleHairArray = FemaleHairs.AppendToArray(CopyRace.FemaleOptions.Hair);

            var race =
            RaceConfigurator.New(SuliName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(SuliDisplayName)
                .SetDescription(SuliDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures()
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: -2)
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
