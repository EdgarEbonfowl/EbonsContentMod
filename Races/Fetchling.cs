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
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Enums.Damage;

namespace EbonsContentMod.Races
{
    internal class Fetchling
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
                RaceRecolorizer.GetColorsFromRGB(230f),
                RaceRecolorizer.GetColorsFromRGB(220f),
                RaceRecolorizer.GetColorsFromRGB(20f)
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
            new Color( // White
                RaceRecolorizer.GetColorsFromRGB(200f),
                RaceRecolorizer.GetColorsFromRGB(200f),
                RaceRecolorizer.GetColorsFromRGB(205f)
                ),
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

        public static EquipmentEntityLink[] FemaleHairs =
        [
                
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DhampirRace.ToString());

        private static readonly string FetchlingName = "FetchlingRace";

        internal const string FetchlingDisplayName = "Fetchling.Name";
        private static readonly string FetchlingDescription = "Fetchling.Description";
        public static readonly string RaceGuid = "{29454C0E-C539-46C4-8CD3-4BCAD4311AB7}";

        internal const string FetchlingShadowBlendingDisplayName = "Fetchling.ShadowBlending.Name";
        private static readonly string FetchlingShadowBlendingDescription = "Fetchling.ShadowBlending.Description";

        internal const string FetchlingShadowyResistanceDisplayName = "Fetchling.ShadowyResistance.Name";
        private static readonly string FetchlingShadowyResistanceDescription = "Fetchling.ShadowyResistance.Description";

        internal const string FetchlingSkilledDisplayName = "Fetchling.Skilled.Name";
        private static readonly string FetchlingSkilledDescription = "Fetchling.Skilled.Description";

        internal static void Configure()
        {
            var ShadowBlendingBuff = BuffConfigurator.New("FetchlingShadowBlendingBuff", "{BF850E19-F900-4D74-8A64-3BA518D17323}")
                .SetDisplayName(FetchlingShadowBlendingDisplayName)
                .SetDescription(FetchlingShadowBlendingDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.Blur.ToString()).Icon)
                .AddSetAttackerMissChance(type: SetAttackerMissChance.Type.All, value: 50)
                .Configure();

            var ShadowBlending = FeatureConfigurator.New("FetchlingShadowBlending", "{101D8C00-8323-4C50-9388-F3C836B9F487}")
                .SetDisplayName(FetchlingShadowBlendingDisplayName)
                .SetDescription(FetchlingShadowBlendingDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.Blur.ToString()).Icon)
                .AddComponent<ShadowBlending>()
                .Configure();

            var ShadowyResistance = FeatureConfigurator.New("FetchlingShadowyResistance", "{14F40656-D3DF-474A-BC7E-2A29424BC1AB}")
                .SetDisplayName(FetchlingShadowyResistanceDisplayName)
                .SetDescription(FetchlingShadowyResistanceDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.ProtectionFromEnergy.ToString()).Icon)
                .AddDamageResistanceEnergy(type: DamageEnergyType.Cold, value: 5)
                .AddDamageResistanceEnergy(type: DamageEnergyType.Electricity, value: 5)
                .Configure();

            var Skilled = FeatureConfigurator.New("FetchlingSkilled", "{A2D69EDD-FFE3-48A2-B8F9-341C045CF8F6}")
                .SetDisplayName(FetchlingSkilledDisplayName)
                .SetDescription(FetchlingSkilledDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HumanSkilled.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillKnowledgeArcana, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillStealth, value: 2)
                .Configure();

            var race =
            RaceConfigurator.New(FetchlingName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(FetchlingDisplayName)
                .SetDescription(FetchlingDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(ShadowBlending, ShadowyResistance, Skilled, FeatureRefs.KeenSenses.ToString())
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: -2)
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, eyecolors: RaceEyeColors, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.AasimarRace.ToString())/*, CustomFemaleHairs: FemaleHairs, CustomMaleHairs: MaleHairs*/);

            // Re-order presets to make the race default to skinny
            var FinalRace = RaceConfigurator.For(recoloredrace)
                .SetPresets(recoloredrace.m_Presets[2], recoloredrace.m_Presets[0], recoloredrace.m_Presets[1])
                .Configure();

            // Add race to mount fixes
            RaceMountFixerizer.AddRaceToMountFixes(FinalRace, CopyRace);

            // Add race to race list
            var raceRef = FinalRace.ToReference<BlueprintRaceReference>();
            ref var races = ref BlueprintRoot.Instance.Progression.m_CharacterRaces;

            var length = races.Length;
            Array.Resize(ref races, length + 1);
            races[length] = raceRef;
        }
    }
}
