﻿using BlueprintCore.Blueprints.Configurators.Classes;
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
using Kingmaker.UnitLogic.FactLogic;
using EbonsContentMod.Components;
using BlueprintCore.Utils.Assets;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;

namespace EbonsContentMod.Races
{
    internal class Strix
    {
        public static List<Color> RaceHeadColors =
        [
            new Color( // Nearly Black
                RaceRecolorizer.GetColorsFromRGB(60f),
                RaceRecolorizer.GetColorsFromRGB(60f),
                RaceRecolorizer.GetColorsFromRGB(60f)
                ),
            new Color( // Very Dark Gray
                RaceRecolorizer.GetColorsFromRGB(75f),
                RaceRecolorizer.GetColorsFromRGB(75f),
                RaceRecolorizer.GetColorsFromRGB(75f)
                ),
            new Color( // Dark Gray
                RaceRecolorizer.GetColorsFromRGB(90f),
                RaceRecolorizer.GetColorsFromRGB(90f),
                RaceRecolorizer.GetColorsFromRGB(90f)
                )
        ];

        public static List<Color> RaceEyeColors =
        [
            new Color( // Red
                RaceRecolorizer.GetColorsFromRGB(177f),
                RaceRecolorizer.GetColorsFromRGB(6f),
                RaceRecolorizer.GetColorsFromRGB(1f)
                ),
            new Color( // Dark Red
                RaceRecolorizer.GetColorsFromRGB(130f),
                RaceRecolorizer.GetColorsFromRGB(6f),
                RaceRecolorizer.GetColorsFromRGB(1f)
                ),
            new Color( // Dark Red
                RaceRecolorizer.GetColorsFromRGB(100f),
                RaceRecolorizer.GetColorsFromRGB(6f),
                RaceRecolorizer.GetColorsFromRGB(1f)
                ),
        ];

        public static List<Color> RaceHairColors =
        [
            new Color(0.75f, 0.75f, 0.78f), // White
            new Color(0.86f, 0.86f, 0.89f) // Super White
        ];

        public static EquipmentEntityLink[] MaleHeads =
        [
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).MaleOptions.Heads[2],
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).MaleOptions.Heads[1]
        ];

        public static EquipmentEntityLink[] FemaleHeads =
        [
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).FemaleOptions.Heads[2],
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).FemaleOptions.Heads[1]
        ];

        public static EquipmentEntityLink[] FemaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "3897a5ce68b630548bb85db0a68a465a"},
            new EquipmentEntityLink() {AssetId = "04c3eb6d7570d8d49b686516b7c4a4f8"}
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string StrixName = "StrixRace";

        internal const string StrixDisplayName = "Strix.Name";
        private static readonly string StrixDescription = "Strix.Description";
        public static readonly string RaceGuid = "{BA18E67E-636B-4470-890D-A4C5AD0642B4}";

        internal const string StrixWingsDisplayName = "Strix.StrixWings.Name";
        private static readonly string StrixWingsDescription = "Strix.StrixWings.Description";

        internal const string StrixHatredDisplayName = "Strix.Hatred.Name";
        private static readonly string StrixHatredDescription = "Strix.Hatred.Description";

        internal const string StrixSuspiciousDisplayName = "Strix.Suspicious.Name";
        private static readonly string StrixSuspiciousDescription = "Strix.Suspicious.Description";

        internal const string StrixNocturnalDisplayName = "Strix.Nocturnal.Name";
        private static readonly string StrixNocturnalDescription = "Strix.Nocturnal.Description";

        internal static void Configure()
        {
            var NewFemaleHairArray = FemaleHairs.AppendToArray(CopyRace.FemaleOptions.Hair);

            //var MaleWingEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "876fbc0d239695a4790358a3be5d7c53" }, RaceRecolorizer.CreateRampsFromColorsSimple(RaceHeadColors), "{DBFF89D1-B634-47BC-9F8F-99CC55EC5835}");
            //var FemaleWingEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "da9f766f4de989f4e865a2d019b55098" }, RaceRecolorizer.CreateRampsFromColorsSimple(RaceHeadColors), "{40ACE0C0-BE1B-4ADD-8186-456BFC5EE3AC}");

            //EquipmentEntityLink[] WingLinks = [MaleWingEE, FemaleWingEE];

            var MaleWings = FeatureConfigurator.New("MaleStrixWings", "{F4A3FE52-7F4E-4D83-890F-7A00194B2AB2}")
                .SetDisplayName(StrixWingsDisplayName)
                .SetDescription(StrixWingsDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.FeatureWingsDraconicWhite.ToString()).Icon)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("876fbc0d239695a4790358a3be5d7c53")
                //.AddEquipmentEntity(MaleWingEE)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var FemaleWings = FeatureConfigurator.New("FemaleStrixWings", "{3752502C-0616-474C-9AE1-A5349A51EC78}")
                .SetDisplayName(StrixWingsDisplayName)
                .SetDescription(StrixWingsDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.FeatureWingsDraconicWhite.ToString()).Icon)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("da9f766f4de989f4e865a2d019b55098")
                //.AddEquipmentEntity(FemaleWingEE)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var StrixWingsBuffingFeature = FeatureConfigurator.New("StrixWingBuffingFeature", "{72D7FB33-9E78-49D6-BF5C-5A300C2445B5}")
                .CopyFrom(FeatureRefs.ShifterGriffonWingsFeature, c => c is not AddEquipmentEntity)
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .Configure();

            var StrixWings = ProgressionConfigurator.New("StrixWings", "{A76CE8F7-A8B9-4333-8F0E-B6C98332D176}")
                .SetDisplayName(StrixWingsDisplayName)
                .SetDescription(StrixWingsDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.FeatureWingsDraconicWhite.ToString()).Icon)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddToLevelEntries(1, MaleWings, FemaleWings, StrixWingsBuffingFeature)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            // Nocturnal

            var NocturnalBuff = BuffConfigurator.New("StrixNocturnalBuff", "{C1A8AB56-409D-4DC6-8D64-D191D38CCC09}")
                .SetDisplayName(StrixNocturnalDisplayName)
                .SetDescription(StrixNocturnalDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.Doom.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.UntypedStackable, stat: StatType.SkillPerception, value: 2)
                .AddStatBonus(ModifierDescriptor.UntypedStackable, stat: StatType.SkillStealth, value: 2)
                .Configure();

            var Nocturnal = FeatureConfigurator.New("StrixNocturnal", "{2031DB1D-DFB8-443B-BF8D-1EFA7EB26D06}")
                .SetDisplayName(StrixNocturnalDisplayName)
                .SetDescription(StrixNocturnalDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.Doom.ToString()).Icon)
                .AddComponent<Nocturnal>()
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            // Suspicious

            var Suspicious = FeatureConfigurator.New("StrixSuspicious", "{347FFCF3-974E-4275-A826-D0D82E9D2960}")
                .SetDisplayName(StrixSuspiciousDisplayName)
                .SetDescription(StrixSuspiciousDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.IllusionResistance.ToString()).Icon)
                .AddSavingThrowBonusAgainstSchool(modifierDescriptor: ModifierDescriptor.Racial, school: SpellSchool.Illusion, value: 2)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            // Hatred

            var Hatred = FeatureConfigurator.New("StrixHatred", "{B9BFA52D-C33B-45CF-A5F3-1654210AC2DD}")
                .SetDisplayName(StrixHatredDisplayName)
                .SetDescription(StrixHatredDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HatredGoblinoidOrc.ToString()).Icon)
                .AddAttackBonusAgainstFactOwner(1, checkedFact: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString()))
                .AddAttackBonusAgainstFactOwner(1, checkedFact: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HalfElfRace.ToString()))
                .AddAttackBonusAgainstFactOwner(1, checkedFact: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HalfOrcRace.ToString()))
                .SetGroups(FeatureGroup.Racial)
                .Configure();
            
            var race =
            RaceConfigurator.New(StrixName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(StrixDisplayName)
                .SetDescription(StrixDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(StrixWings, Nocturnal, Suspicious, Hatred, FeatureRefs.KeenSenses.ToString())
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: -2)
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, eyecolors: RaceEyeColors, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.OreadRace.ToString()), NoEyebrows: true, CustomMaleHeads: MaleHeads, CustomFemaleHeads: FemaleHeads, CustomFemaleHairs: NewFemaleHairArray);

            // Register linked EEs
            //EELinker.RegisterSkinLink(recoloredrace, WingLinks);

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
