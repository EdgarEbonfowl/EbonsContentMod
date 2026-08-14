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
using Kingmaker.Craft;
using Kingmaker.ResourceLinks;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using EbonsContentMod.Components;
using Kingmaker.Blueprints.Classes.Spells;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using TabletopTweaks.Core.NewComponents;
using Kingmaker.Designers.Mechanics.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics;
using BlueprintCore.Utils.Types;
using Kingmaker.Utility;
using Kingmaker.Visual.CharacterSystem;
using Kingmaker.Dungeon.Actions;

namespace EbonsContentMod.Races
{
    internal class Astomoi
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
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 4),
        ];

        public static List<Texture2D> CustomEyeRamps =
        [
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 4),
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string AstomoiName = "AstomoiRace";

        internal const string AstomoiDisplayName = "Astomoi.Name";
        private static readonly string AstomoiDescription = "Astomoi.Description";
        public static readonly string RaceGuid = "{582B54CC-2B51-41E3-ACA2-395D966CCA3D}";

        internal const string SensitiveBreathDisplayName = "Astomoi.SensitiveBreath.Name";
        private static readonly string SensitiveBreathDescription = "Astomoi.SensitiveBreath.Description";

        internal const string TelepathicSightDisplayName = "Astomoi.TelepathicSight.Name";
        private static readonly string TelepathicSightDescription = "Astomoi.TelepathicSight.Description";

        internal static void Configure()
        {
            var FemaleSkin = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "513c170fd80260f4c9dcca7e09f36c30" }, CustomHeadRamps, "{F9981D75-7DE4-43C3-92FD-0E7D95591715}", true, BodyPartsToRemove: [BodyPartType.Eyes]);

            var MaleSkin = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "e80a6ac3ad73c6a4fbc711bff9e4021b" }, CustomHeadRamps, "{F0758FF8-59D7-482E-9470-27B8C3E0C87B}", true, BodyPartsToRemove: [BodyPartType.Eyes]);

            var MaleSkinFeature = FeatureConfigurator.New("AstomoiMaleSkin", "{8A484469-2FDB-43AA-AB64-9AC0B9FA92B8}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity(MaleSkin)
                .Configure();

            var FemaleSkinFeature = FeatureConfigurator.New("AstomoiFemaleSkin", "{56BC2599-82C6-43EB-B084-579C831A67B8}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity(FemaleSkin)
                .Configure();

            var SensitiveBreath = FeatureConfigurator.New("AstomoiSensitiveBreath", "{0CF619C9-BA59-4601-B59F-956B631A4CCC}")
                .SetDisplayName(SensitiveBreathDisplayName)
                .SetDescription(SensitiveBreathDescription)
                .SetIcon(AbilityRefs.PoisonBreath.Reference.Get().Icon)
                .AddSavingThrowBonusAgainstSpecificSpells(value: -2, spells: [
                    AbilityRefs.StinkingCloud.ToString(),
                    AbilityRefs.Cloudkill.ToString(),
                    AbilityRefs.MindFog.ToString(),
                    AbilityRefs.GoldGolemDeathThroes.ToString(),
                    AbilityRefs.PlagueStorm.ToString(),
                    AbilityRefs.AcidFog.ToString()
                    ])
                .AddSavingThrowBonusAgainstDescriptor(ContextValues.Constant(-2), modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.Disease)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var TelepathicSight = ProgressionConfigurator.New("AstomoiTelepathicSightProgression", "{9688034E-67F0-4E2C-8BC1-2413B4718AA4}")
                .SetDisplayName(TelepathicSightDisplayName)
                .SetDescription(TelepathicSightDescription)
                .SetIcon(AbilityRefs.Thoughtsense.Reference.Get().Icon)
                .AddBlindsense(60.Feet(), true)
                .AddSpellImmunityToSpellDescriptor(descriptor: SpellDescriptor.GazeAttack)
                .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.GazeAttack)
                .SetGroups(FeatureGroup.Racial)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddToLevelEntries(1, MaleSkinFeature, FemaleSkinFeature)
                .Configure();

            var race = RaceConfigurator.New(AstomoiName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(AstomoiDisplayName)
                .SetDescription(AstomoiDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(SensitiveBreath, TelepathicSight)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: -2)
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, CustomHeadRamps: CustomHeadRamps, CustomEyeRamps: CustomEyeRamps, RemoveHeadParts: [BodyPartType.Eyes, BodyPartType.Ears, BodyPartType.Lashes, BodyPartType.Brows, BodyPartType.Nose, BodyPartType.Teeth], BaldRace: true, NoEyebrows: true, NoBeards: true);

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
            PortraitCreatonator.RegisterRacePortrait("Astomoi_F_01", "{148AF8E8-1ED8-43AB-BC58-4A68601006A6}", race, Gender.Female, "Astomoi_F_01");
            PortraitCreatonator.RegisterRacePortrait("Astomoi_M_01", "{66B55B54-FD31-49F2-AD49-339FD08DD4B7}", race, Gender.Male, "Astomoi_M_01");
        }
    }
}
