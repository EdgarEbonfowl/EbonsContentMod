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

        internal const string SylphEnergyResistanceDisplayName = "Sylph.EnergyResistance.Name";
        private static readonly string SylphEnergyResistanceDescription = "Sylph.EnergyResistance.Description";

        internal const string SylphAirAffinityDisplayName = "Sylph.AirAffinity.Name";
        private static readonly string SylphAirAffinityDescription = "Sylph.AirAffinity.Description";

        internal const string ElementalHeritageDisplayName = "Sylph.ElementalHeritage.Name";
        private static readonly string ElementalHeritageDescription = "Sylph.ElementalHeritage.Description";

        internal const string SylphGeneralDisplayName = "Sylph.SylphGeneral.Name";
        private static readonly string SylphGeneralDescription = "Sylph.SylphGeneral.Description";

        internal const string SmokesoulDisplayName = "Sylph.Smokesoul.Name";
        private static readonly string SmokesoulDescription = "Sylph.Smokesoul.Description";

        internal const string StormsoulDisplayName = "Sylph.Stormsoul.Name";
        private static readonly string StormsoulDescription = "Sylph.Stormsoul.Description";

        internal static void Configure()
        {
            var NewFemaleHairArray = FemaleHairs.AppendToArray(CopyRace.FemaleOptions.Hair);

            var EnergyResistance = FeatureConfigurator.New("SylphEnergyResistance", "{85EBD773-D4CA-4A18-B435-CF65CAB455EA}")
                .SetDisplayName(SylphEnergyResistanceDisplayName)
                .SetDescription(SylphEnergyResistanceDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.BloodlineDraconicBlueResistancesAbilityLevel1.ToString()).Icon)
                .AddDamageResistanceEnergy(type: DamageEnergyType.Electricity, value: 5)
                .Configure();

            var SpellLikeResource = AbilityResourceConfigurator.New("SylphFeatherStepResource", "{847EE70E-F33F-4EC7-9B83-9FA5128FD1E0}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var AirAffinity = FeatureConfigurator.New("SylphAirAffinity", "{A038AC3F-9473-4CC6-B335-95AF8EC2FA7E}")
                .SetDisplayName(SylphAirAffinityDisplayName)
                .SetDescription(SylphAirAffinityDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintProgression>(ProgressionRefs.BloodlineElementalAirProgression.ToString()).Icon)
                .AddIncreaseSpellDescriptorCasterLevel(1, SpellDescriptor.Electricity, modifierDescriptor: ModifierDescriptor.Racial)
                .AddIncreaseSpellDescriptorDC(1, SpellDescriptor.Electricity, modifierDescriptor: ModifierDescriptor.Racial)
                .Configure();

            // Elemental Heritage

            var FeatherStep = AbilityConfigurator.New("SylphFeatherStepAbility", "{8DC5D717-BAAC-4051-92C8-81E25331C614}")
                .CopyFrom(AbilityRefs.FeatherStep, c => c is not (SpellListComponent or CraftInfoComponent or ContextRankConfig))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel())
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 1)
                .Configure();

            var ExpeditiousRetreat = AbilityConfigurator.New("SylphExpeditiousRetreat", "{9AC04F15-06B9-449C-84F1-530DF6CF6BB1}")
                .CopyFrom(AbilityRefs.ExpeditiousRetreat, c => c is not (SpellListComponent or CraftInfoComponent or ContextRankConfig))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel())
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 1)
                .Configure();

            var ShockingGraspEffect = AbilityConfigurator.New("SylphShockingGraspEffect", "{B8BE0AA1-6320-4AA5-8BA7-C151C75AF7CF}")
                .CopyFrom(AbilityRefs.ShockingGraspEffect, c => c is not (SpellListComponent or CraftInfoComponent or ContextRankConfig))
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel())
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 1)
                .Configure();

            var ShockingGrasp = AbilityConfigurator.New("SylphShockingGrasp", "{685A2DD9-DB29-4A9B-9A15-26C4111DD4AB}")
                .CopyFrom(AbilityRefs.ShockingGraspCast, c => c is not (SpellListComponent or CraftInfoComponent or ContextRankConfig or AbilityEffectStickyTouch))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .AddAbilityEffectStickyTouch(touchDeliveryAbility: ShockingGraspEffect)
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel())
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 1)
                .Configure();

            var Smokesoul = FeatureConfigurator.New("SylphSmokesoul", "{E7670769-F7B1-4952-B1C1-BB7AC4BD8764}")
                .SetDisplayName(SmokesoulDisplayName)
                .SetDescription(SmokesoulDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: -2)
                .AddFacts([ExpeditiousRetreat])
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: ExpeditiousRetreat)
                .Configure();

            var Stormsoul = FeatureConfigurator.New("SylphStormsoul", "{492386CC-43B2-4AD3-9050-08EE2A3009BD}")
                .SetDisplayName(StormsoulDisplayName)
                .SetDescription(StormsoulDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: -2)
                .AddFacts([ShockingGrasp])
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: ShockingGrasp)
                .AddReplaceCasterLevelOfAbility(spell: ShockingGraspEffect)
                .Configure();

            var SylphGeneral = FeatureConfigurator.New("SylphGeneral", "{51B6AD50-738B-4DF8-BD75-D0DB4FA21F1C}")
                .SetDisplayName(SylphGeneralDisplayName)
                .SetDescription(SylphGeneralDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: -2)
                .AddFacts([FeatherStep])
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: FeatherStep)
                .Configure();

            var SylphElementalHeritageSelection = FeatureSelectionConfigurator.New("SylphElementalHeritageSelection", "{3B1902B3-B8A7-423B-82FB-3D6EA0220E37}")
                .SetDisplayName(ElementalHeritageDisplayName)
                .SetDescription(ElementalHeritageDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(FeatureSelectionRefs.HalfOrcHeritageSelection.ToString()).Icon)
                .AddToAllFeatures(SylphGeneral, Smokesoul, Stormsoul)
                .SetGroup(FeatureGroup.Racial)
                .Configure();

            var race =
            RaceConfigurator.New(SylphName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(SylphDisplayName)
                .SetDescription(SylphDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(SylphElementalHeritageSelection, EnergyResistance, AirAffinity, FeatureRefs.KeenSenses.ToString())
                .SetRaceId(Race.Human)
                .AddEquipmentEntity("aecc5905323948449b4cd3bfe36e5daf")
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, /*eyecolors: RaceEyeColors,*/ eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.KitsuneRace.ToString()), CustomFemaleHairs: NewFemaleHairArray, CustomEyeRamps: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.OreadRace.ToString()).FemaleOptions.m_Heads[0].Load(true, false).SecondaryColorsProfile.Ramps);

            // Re-order presets to make the race default to skinny and remove fat option
            var FinalRace = RaceConfigurator.For(recoloredrace)
                .SetPresets(recoloredrace.m_Presets[2], recoloredrace.m_Presets[0])
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
