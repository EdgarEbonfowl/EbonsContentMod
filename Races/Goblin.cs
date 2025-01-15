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
using static Kingmaker.Kingdom.Settlements.SettlementGridTopology;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Visual.CharacterSystem;
using Kingmaker.Blueprints.CharGen;

namespace EbonsContentMod.Races
{
    internal class Goblin
    {
        public static List<Color> RaceHeadColors =
        [
            new Color( // Green with a little yellow
                RaceRecolorizer.GetColorsFromRGB(80f),
                RaceRecolorizer.GetColorsFromRGB(90f),
                RaceRecolorizer.GetColorsFromRGB(0f)
                )
        ];

        public static List<Color> RaceEyeColors =
        [
            new Color( // Red
                RaceRecolorizer.GetColorsFromRGB(140f),
                RaceRecolorizer.GetColorsFromRGB(6f),
                RaceRecolorizer.GetColorsFromRGB(1f)
                )
        ];

        public static List<Color> RaceHairColors = [];

        public static EquipmentEntityLink[] GoblinHeads =
        [
            new EquipmentEntityLink() {AssetId = "41fb4c393c6a67141b6e7d48911e38a1"}
        ];

        public static List<Texture2D> CustomSkinRamps =
        [
            GoblinHeads[0].Load().PrimaryColorsProfile.Ramps[6],
            GoblinHeads[0].Load().PrimaryColorsProfile.Ramps[1],
            GoblinHeads[0].Load().PrimaryColorsProfile.Ramps[0],
            GoblinHeads[0].Load().PrimaryColorsProfile.Ramps[2],
            GoblinHeads[0].Load().PrimaryColorsProfile.Ramps[5],
            GoblinHeads[0].Load().PrimaryColorsProfile.Ramps[7],
            GoblinHeads[0].Load().PrimaryColorsProfile.Ramps[8]
        ];

        public static List<Texture2D> CustomEyeRamps =
        [
            GoblinHeads[0].Load().SecondaryColorsProfile.Ramps[2],
            GoblinHeads[0].Load().SecondaryColorsProfile.Ramps[3],
            GoblinHeads[0].Load().SecondaryColorsProfile.Ramps[4],
            GoblinHeads[0].Load().SecondaryColorsProfile.Ramps[5],
            GoblinHeads[0].Load().SecondaryColorsProfile.Ramps[1],
            GoblinHeads[0].Load().SecondaryColorsProfile.Ramps[0]
        ];

        public static EquipmentEntityLink[] NewEyeLinkedEEs =
        [
            new EquipmentEntityLink() { AssetId = "c5487e7e903d25a40be683767f3df0b4" } // Contact lenses
        ];

        public static EquipmentEntityLink GoblinBody = new EquipmentEntityLink() { AssetId = "a89270e53d026e347978e03fc397bd8e" };

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HalflingRace.ToString());

        private static readonly string GoblinName = "EbonsGoblinRace";

        internal const string GoblinDisplayName = "Goblin.Name";
        private static readonly string GoblinDescription = "Goblin.Description";
        public static readonly string RaceGuid = "{93FB4931-C7B3-4EC4-A023-F429E3B16239}";

        internal const string FastMovementDisplayName = "Goblin.FastMovement.Name";
        private static readonly string FastMovementDescription = "Goblin.FastMovement.Description";

        internal const string GoblinRacialTraitDisplayName = "Goblin.GoblinRacialTrait.Name";
        private static readonly string GoblinRacialTraitDescription = "Goblin.GoblinRacialTrait.Description";

        internal const string SkilledDisplayName = "Goblin.Skilled.Name";
        private static readonly string SkilledDescription = "Goblin.Skilled.Description";

        internal const string CityScavengerDisplayName = "Goblin.CityScavenger.Name";
        private static readonly string CityScavengerDescription = "Goblin.CityScavenger.Description";

        internal const string EatAnythingDisplayName = "Goblin.EatAnything.Name";
        private static readonly string EatAnythingDescription = "Goblin.EatAnything.Description";

        internal const string HardHeadBigTeethDisplayName = "Goblin.HardHeadBigTeeth.Name";
        private static readonly string HardHeadBigTeethDescription = "Goblin.HardHeadBigTeeth.Description";

        internal const string OversizedEarsDisplayName = "Goblin.OversizedEars.Name";
        private static readonly string OversizedEarsDescription = "Goblin.OversizedEars.Description";

        internal const string TreeRunnerDisplayName = "Goblin.TreeRunner.Name";
        private static readonly string TreeRunnerDescription = "Goblin.TreeRunner.Description";

        internal static void Configure()
        {
            var ContactEEL = new EquipmentEntityLink() { AssetId = "c5487e7e903d25a40be683767f3df0b4" };
            var RampEquipmentEEL = new EquipmentEntityLink() { AssetId = "ded027dd3a059ae4aa1e8cd93e450b03" };

            // Make goblin contact lenses
            //var Contacts = RaceRecolorizer.RecolorEELink(ContactEEL, new List<Texture2D>() { RaceRecolorizer.GetArmorRampByIndex(13) }, "{D130526D-84E5-4DA4-B2D4-B99CF0C7784A}", true, true);

            EquipmentEntityLink[] ContactLinks = [new EquipmentEntityLink() { AssetId = "c5487e7e903d25a40be683767f3df0b4" }];

            // Fast Movement - this is a dummy feature
            var FastMovement = FeatureConfigurator.New("EbonsGoblinFastMovement", "{EF193BF2-4B32-42E4-85C3-69B14FA0F302}")
                .SetDisplayName(FastMovementDisplayName)
                .SetDescription(FastMovementDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureSelectionRefs.RogueTalentSelection.ToString()).Icon)
                .Configure();

            // Goblin racial traits

            // Skilled

            var Skilled = FeatureConfigurator.New("EbonsGoblinSkilled", "{FACE5BBE-734D-4A90-97D5-3B1C350FB788}")
                .SetDisplayName(SkilledDisplayName)
                .SetDescription(SkilledDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HumanSkilled.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillMobility, value: 4)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillStealth, value: 4)
                .Configure();

            // City Scavenger

            var CityScavenger = FeatureConfigurator.New("EbonsGoblinCityScavenger", "{171D4A77-9E16-445B-A8DC-FD7A3D6645AF}")
                .SetDisplayName(CityScavengerDisplayName)
                .SetDescription(CityScavengerDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.ArchaeologistConfidentExplorerFeature.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillPerception, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillLoreNature, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillKnowledgeWorld, value: 2)
                .Configure();

            // Eat Anything

            var EatAnything = FeatureConfigurator.New("EbonsGoblinEatAnything", "{0540867B-575F-4F6D-878F-B1FA423BC8D0}")
                .SetDisplayName(EatAnythingDisplayName)
                .SetDescription(EatAnythingDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.CrushAndTearFeature.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillLoreNature, value: 4)
                .AddSavingThrowBonusAgainstDescriptor(4, modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.Sickened)
                .AddSavingThrowBonusAgainstDescriptor(4, modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.Nauseated)
                .Configure();

            // Hard Head, Big Teeth

            var HardHeadBigTeeth = FeatureConfigurator.New("EbonsGoblinHardHeadBigTeeth", "{82B55400-24F6-4E9F-AB42-137113AC8C1B}")
                .SetDisplayName(HardHeadBigTeethDisplayName)
                .SetDescription(HardHeadBigTeethDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.AcidMaw.ToString()).Icon)
                .AddAdditionalLimb(weapon: ItemWeaponRefs.Bite1d4.ToString())
                .Configure();

            // Oversized Ears

            var OversizedEars = FeatureConfigurator.New("EbonsGoblinOversizedEars", "{E290A6FA-3DC5-4AA9-BAF3-9AE24B0E4B70}")
                .SetDisplayName(OversizedEarsDisplayName)
                .SetDescription(OversizedEarsDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.SoundBurst.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillPerception, value: 4)
                .Configure();

            // Tree Runner

            var TreeRunner = FeatureConfigurator.New("EbonsGoblinTreeRunner", "{B47C9AA3-51B7-4E4E-A872-F09E2C3F8265}")
                .SetDisplayName(TreeRunnerDisplayName)
                .SetDescription(TreeRunnerDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.DruidWoodlandStride.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillMobility, value: 4)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillAthletics, value: 4)
                .Configure();

            var GoblinRacialTraitSelection = FeatureSelectionConfigurator.New("EbonsGoblinRacialTraitSelection", "{5AAD6811-E1E5-47A8-B4A3-1943ABE39685}")
                .SetDisplayName(GoblinRacialTraitDisplayName)
                .SetDescription(GoblinRacialTraitDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(FeatureSelectionRefs.HalfOrcHeritageSelection.ToString()).Icon)
                .AddToAllFeatures(Skilled, CityScavenger, EatAnything, HardHeadBigTeeth, OversizedEars, TreeRunner)
                .SetGroup(FeatureGroup.Racial)
                .Configure();

            var race =
            RaceConfigurator.New(GoblinName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(GoblinDisplayName)
                .SetDescription(GoblinDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(GoblinRacialTraitSelection, FastMovement, FeatureRefs.KeenSenses.ToString())
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 4)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: -2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: -2)
                .SetRaceId(Race.Halfling)
                .Configure();

            // Recolor Race
            //var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, /*eyecolors: RaceEyeColors, */BaldRace: true, NoEyebrows: true, NoBeards: true, /*eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.OreadRace.ToString()),*/ /*CustomMaleHeads: GoblinHeads, CustomFemaleHeads: GoblinHeads,*/ CustomMaleHeadsNoRecolor: GoblinHeads, CustomFemaleHeadsNoRecolor: GoblinHeads, CustomBodyNoRecolor: GoblinBody/*, EyeLinkedEEs: NewEyeLinkedEEs*/);
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, CustomMaleHeads: GoblinHeads, /*CustomFemaleHeads: GoblinHeads,*/ CustomBody: GoblinBody, BaldRace: true, NoEyebrows: true, NoBeards: true, CustomEyeRamps: CustomEyeRamps, CustomHeadRamps: CustomSkinRamps, eyeEE: ContactEEL);

            // Mess around with the bones
            var standardskeleton = GameObject.Instantiate(recoloredrace.m_Presets[0].Get().MaleSkeleton);

            standardskeleton.Bones[105].ApplyOffset = false;
            standardskeleton.Bones[112].ApplyOffset = false;

            var fatskeleton = GameObject.Instantiate(recoloredrace.m_Presets[1].Get().MaleSkeleton);

            fatskeleton.Bones[105].ApplyOffset = false;
            fatskeleton.Bones[112].ApplyOffset = false;

            var thinskeleton = GameObject.Instantiate(recoloredrace.m_Presets[2].Get().MaleSkeleton);

            thinskeleton.Bones[105].ApplyOffset = false;
            thinskeleton.Bones[112].ApplyOffset = false;

            var newstandardpreset = AltHelpers.CreateCopyAlt(recoloredrace.m_Presets[0].Get(), p =>
            {
                p.name = recoloredrace.name + "_NewOffsets_" + p.name;
                p.MaleSkeleton = standardskeleton;
                //p.MaleSkeleton = Kingmaker.Blueprints.JsonSystem.Converters.UnityObjectConverter.AssetList.Get("12131b81b481f624abeb1303a0c69ad5", 11400000) as Skeleton;
                p.AssetGuid = BlueprintGuid.Parse("{FBC83831-315A-4A39-874B-327061EF28B0}");
                RaceRecolorizer.AddBlueprint(p, p.AssetGuid);
            });

            var newfatpreset = AltHelpers.CreateCopyAlt(recoloredrace.m_Presets[1].Get(), p =>
            {
                p.name = recoloredrace.name + "_NewOffsets_" + p.name;
                p.MaleSkeleton = fatskeleton;
                //p.MaleSkeleton = Kingmaker.Blueprints.JsonSystem.Converters.UnityObjectConverter.AssetList.Get("12131b81b481f624abeb1303a0c69ad5", 11400000) as Skeleton;
                p.AssetGuid = BlueprintGuid.Parse("{F5345D58-CEC5-4B38-B5C9-61DEC42EDEC8}");
                RaceRecolorizer.AddBlueprint(p, p.AssetGuid);
            });

            var newthinpreset = AltHelpers.CreateCopyAlt(recoloredrace.m_Presets[2].Get(), p =>
            {
                p.name = recoloredrace.name + "_NewOffsets_" + p.name;
                p.MaleSkeleton = thinskeleton;
                //p.MaleSkeleton = Kingmaker.Blueprints.JsonSystem.Converters.UnityObjectConverter.AssetList.Get("12131b81b481f624abeb1303a0c69ad5", 11400000) as Skeleton;
                p.AssetGuid = BlueprintGuid.Parse("{A63DEF31-65D6-4B07-A8AC-7EA5D17C0485}");
                RaceRecolorizer.AddBlueprint(p, p.AssetGuid);
            });

            var FinalRace = RaceConfigurator.For(recoloredrace)
                .SetPresets(newthinpreset, newstandardpreset, newfatpreset)
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
