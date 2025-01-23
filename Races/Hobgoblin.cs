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
using Kingmaker.Visual.CharacterSystem;
using Kingmaker.Blueprints.Root.Strings;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.RuleSystem.Rules;
using BlueprintCore.Blueprints.Configurators.Classes.Selection;

namespace EbonsContentMod.Races
{
    internal class Hobgoblin
    {
        public static List<Color> RaceHeadColors = [
            new Color(0.173f, 0.455f, 0.78f),
            new Color(0.118f, 0.573f, 0.824f),
            new Color(0.0f, 0.5f, 1f),
            new Color(0.322f, 0.439f, 0.816f),
            new Color(0.301f, 0.333f, 0.863f)];

        public static List<Color> RaceEyeColors = [];

        public static List<Color> RaceHairColors = [];

        public static EquipmentEntityLink[] MaleHeads =
        [
            new EquipmentEntityLink() {AssetId = "717e2297037cfe747b0dc68e30a29eed"}, // 01 Male Half-Orc
            new EquipmentEntityLink() {AssetId = "eab410cf83fc51d40a8d3e602a15d821"}, // 02 Asian Male Half-Orc
        ];

        public static EquipmentEntityLink[] FemaleHeads =
        [
            new EquipmentEntityLink() {AssetId = "263deaab2915da84d93e4c2733d1eda4"}, // 01 Female Half-Orc
            new EquipmentEntityLink() {AssetId = "3395eea497b4c2f43a2044b82a4eb3c1"}, // 02 Asian Female Half-Orc
        ];

        public static EquipmentEntityLink[] FemaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "fb79b0f415a030f4b9983e61b7d480fa"}, // Arueshalae Hair
            new EquipmentEntityLink() {AssetId = "04c3eb6d7570d8d49b686516b7c4a4f8"}, // Long Camelia Hair
            new EquipmentEntityLink() {AssetId = "fc3fc0e45a70a0e42b6aed10baf794f0"}, // Dread Seelah Female Human
            new EquipmentEntityLink() {AssetId = "779458079f7718c4bb960d9cef195339"}, // Long Wavy Braids
            new EquipmentEntityLink() {AssetId = "30d504db6b8cbe94dbc82d2437c8b468"}, // Long Wavy Female Human
            new EquipmentEntityLink() {AssetId = "1f19aaaa1870e2b4b8bd99d36211ddf6"}, // Ponytail Upper Female Human
            new EquipmentEntityLink() {AssetId = "d71d2e53fce0f1d4baad8b20c8266676"}, // Slick Female Tiefling
            new EquipmentEntityLink() {AssetId = "afa22656ed5030c4ba273583ba2b3a16"}, // Long Wild Female Tiefling
            new EquipmentEntityLink() {AssetId = "1762cab3d178f53489f43ab791b87f9c"}, // Noble Braids - Dwarf
            new EquipmentEntityLink() {AssetId = "34bb68b3e4f03be44a1f0611a09530fc"}, // Crown Braids - Dwarf
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"}  // Bald
        ];

        public static EquipmentEntityLink[] MaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "329cf540a8faed64284c067bace8bbc9"}, // Trim Male Human
            new EquipmentEntityLink() {AssetId = "88c2650d77d9a3c4a8a861fa0d8d0aae"}, // Military Male Human
            new EquipmentEntityLink() {AssetId = "def666224ba24df4e954c03049b29a53"}, // Short Human
            new EquipmentEntityLink() {AssetId = "609143dbf7607f6419babaf5748b82dc"}, // Medium Bun Male Human
            new EquipmentEntityLink() {AssetId = "303578a648d8d344b8d3a9a94fe24d5a"}, // Long Wavy Male Human
            new EquipmentEntityLink() {AssetId = "acdcfd7609f88ae49833e4f10656190e"}, // Long Bangs Male Human
            new EquipmentEntityLink() {AssetId = "50eac92ba30862940be4f70d329d070a"}, // Long Wild Male Tiefling
            new EquipmentEntityLink() {AssetId = "222890293b0f66145a400eae3432868d"}, // Mohawk Male Human
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"}  // Bald
        ];

        public static List<Texture2D> DhampirRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DhampirRace.ToString()).MaleOptions.Heads[0].Load(true, false).PrimaryColorsProfile.Ramps;
        public static List<Texture2D> TieflingRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).MaleOptions.Heads[0].Load(true, false).PrimaryColorsProfile.Ramps;

        public static List<Texture2D> CustomSkinRamps =
        [
            TieflingRamps[13],
            TieflingRamps[4],
            TieflingRamps[5],
            TieflingRamps[7],
            TieflingRamps[8],
            DhampirRamps[0],
            DhampirRamps[1],
            DhampirRamps[3],
            DhampirRamps[8],
            DhampirRamps[9],
            DhampirRamps[10]
        ];

        public static List<Texture2D> CustomEyeRamps =
        [
            RaceRecolorizer.GetArmorRampByIndex(13),
            RaceRecolorizer.GetArmorRampByIndex(12),
            RaceRecolorizer.GetArmorRampByIndex(11),
            RaceRecolorizer.GetArmorRampByIndex(14),
            RaceRecolorizer.GetArmorRampByIndex(67),
            RaceRecolorizer.GetArmorRampByIndex(68),
            RaceRecolorizer.GetArmorRampByIndex(69),
            RaceRecolorizer.GetArmorRampByIndex(70)
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string HobgoblinName = "Hobgoblin";

        internal const string HobgoblinDisplayName = "Hobgoblin.Name";
        private static readonly string HobgoblinDescription = "Hobgoblin.Description";
        public static readonly string RaceGuid = "{BE0A8E97-1F8E-4AB6-9751-54DADE7A2446}";

        internal const string SneakyDisplayName = "Hobgoblin.Sneaky.Name";
        private static readonly string SneakyDescription = "Hobgoblin.Sneaky.Description";

        internal const string AuthoritativeDisplayName = "Hobgoblin.Authoritative.Name";
        private static readonly string AuthoritativeDescription = "Hobgoblin.Authoritative.Description";

        internal const string BattleHardenedDisplayName = "Hobgoblin.BattleHardened.Name";
        private static readonly string BattleHardenedDescription = "Hobgoblin.BattleHardened.Description";

        internal const string FearsomeDisplayName = "Hobgoblin.Fearsome.Name";
        private static readonly string FearsomeDescription = "Hobgoblin.Fearsome.Description";

        internal const string MagehunterDisplayName = "Hobgoblin.Magehunter.Name";
        private static readonly string MagehunterDescription = "Hobgoblin.Magehunter.Description";

        internal const string PitBossDisplayName = "Hobgoblin.PitBoss.Name";
        private static readonly string PitBossDescription = "Hobgoblin.PitBoss.Description";

        internal const string SlaveHunterDisplayName = "Hobgoblin.SlaveHunter.Name";
        private static readonly string SlaveHunterDescription = "Hobgoblin.SlaveHunter.Description";

        internal const string UnfitDisplayName = "Hobgoblin.Unfit.Name";
        private static readonly string UnfitDescription = "Hobgoblin.Unfit.Description";

        internal const string HobgoblinRacialTraitDisplayName = "Hobgoblin.HobgoblinRacialTrait.Name";
        private static readonly string HobgoblinRacialTraitDescription = "Hobgoblin.HobgoblinRacialTrait.Description";

        internal static void Configure()
        {
            var ContactEEL = new EquipmentEntityLink() { AssetId = "c5487e7e903d25a40be683767f3df0b4" };
            var RampEquipmentEEL = new EquipmentEntityLink() { AssetId = "ded027dd3a059ae4aa1e8cd93e450b03" };

            EquipmentEntityLink[] ContactLinks = [new EquipmentEntityLink() { AssetId = "c5487e7e903d25a40be683767f3df0b4" }];

            var HobgoblinMaleEarsEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "b5b2e84af4b3f4e429efaba22fb53789" }, CustomSkinRamps, "{FE9898ED-857C-4B9E-8B09-558A0CB26793}", true);
            var HobgoblinFemaleEarsEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "88fcdcdc8e9e8d24f9a7d7c91a6054a5" }, CustomSkinRamps, "{B62F8FC6-CC9B-4532-A546-35CA266CE5D1}", true);

            var Contacts = RaceRecolorizer.RecolorEELink(ContactEEL, CustomEyeRamps, "{0176AF08-4B11-4D45-A492-D5B3EA3BBEAE}", true, true);

            var HobgoblinFemaleEars = FeatureConfigurator.New("HobgoblinFemaleEars", "{332BB404-7169-4BA9-ABB7-C14FA245EB7D}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity(HobgoblinFemaleEarsEE)
                .Configure();

            var HobgoblinMaleEars = FeatureConfigurator.New("HobgoblinMaleEars", "{5C572267-E667-494B-B46C-2E446084563D}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity(HobgoblinMaleEarsEE)
                .Configure();

            var Authoritative = FeatureConfigurator.New("HobgoblinAuthoritative", "{3C186AA9-F829-4275-ADF6-3924CF78FA6E}")
                .SetDisplayName(AuthoritativeDisplayName)
                .SetDescription(AuthoritativeDescription)
                .SetIcon(AbilityRefs.Command.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.CheckDiplomacy, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.CheckIntimidate, value: 2)
                .AddClassSkill(StatType.SkillPersuasion)
                .Configure();

            var BattleHardened = FeatureConfigurator.New("HobgoblinBattleHardened", "{7E1599E7-0E40-462B-B079-E5F81204F061}")
                .SetDisplayName(BattleHardenedDisplayName)
                .SetDescription(BattleHardenedDescription)
                .SetIcon(FeatureRefs.ArmoredHulkIndomitableStance.Reference.Get().Icon) // Also: Self Sufficient
                .AddStatBonus(ModifierDescriptor.UntypedStackable, stat: StatType.AdditionalCMD, value: 1)
                .Configure();

            var Fearsome = FeatureConfigurator.New("HobgoblinFearsome", "{A493335F-8207-431B-A692-9F97B2E3560F}")
                .SetDisplayName(FearsomeDisplayName)
                .SetDescription(FearsomeDescription)
                .SetIcon(AbilityRefs.Rage.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.CheckIntimidate, value: 4)
                .Configure();

            var Magehunter = FeatureConfigurator.New("HobgoblinMagehunter", "{D3CCF252-5107-4607-AA70-6BE2EEEDDE4D}")
                .SetDisplayName(MagehunterDisplayName)
                .SetDescription(MagehunterDescription)
                .SetIcon(FeatureRefs.SpellCombatFeature.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillKnowledgeArcana, value: 2)
                .AddAttackBonusConditional(1, false, ConditionsBuilder.New().TargetIsArcaneCaster().Build(), ModifierDescriptor.Racial)
                .Configure();

            var PitBoss = FeatureConfigurator.New("HobgoblinPitBoss", "{AF9B3B08-DD2A-415F-8F1B-2475E11CDECD}")
                .SetDisplayName(PitBossDisplayName)
                .SetDescription(PitBossDescription)
                .SetIcon(FeatureRefs.BowlingInfusionFeature.Reference.Get().Icon)
                .AddCMBBonusForManeuver(descriptor: ModifierDescriptor.Racial, maneuvers: [CombatManeuver.Disarm, CombatManeuver.Trip], value: 1)
                .Configure();

            var SlaveHunter = FeatureConfigurator.New("HobgoblinSlaveHunter", "{8BDCDEE9-260E-442C-948F-70F694305161}")
                .SetDisplayName(SlaveHunterDisplayName)
                .SetDescription(SlaveHunterDescription)
                .SetIcon(FeatureRefs.UrbanHunterCaptor.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillLoreNature, value: 2)
                .AddSavingThrowBonusAgainstDescriptor(modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.Disease, value: 2)
                .Configure();

            var Sneaky = FeatureConfigurator.New("HobgoblinStealthy", "{2B94C089-DE7F-454E-969A-B6326694C818}")
                .SetDisplayName(SneakyDisplayName)
                .SetDescription(SneakyDescription)
                .SetIcon(FeatureSelectionRefs.RogueTalentSelection.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillStealth, value: 4)
                .Configure();

            var Unfit = ParametrizedFeatureConfigurator.New("HobgoblinUnfit", "{3C7D2711-719F-43B5-89B0-E0334AADE848}")
                .SetDisplayName(UnfitDisplayName)
                .SetDescription(UnfitDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.CheckBluff, value: 1)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.CheckDiplomacy, value: 1)
                .SetIcon(FeatureSelectionRefs.WeaponTrainingSelection.Reference.Get().Icon)
                .AddComponent<AddParametrizedWeaponProficiency>()
                .SetParameterType(FeatureParameterType.WeaponCategory)
                .SetWeaponSubCategory(WeaponSubCategory.Martial)
                .SetRequireProficiency(false)
                .Configure();

            /*var Unfit = ProgressionConfigurator.New("HobgoblinUnfit", "{311C8411-CECC-42EA-8802-E0812D1D1788}")
                .SetDisplayName(UnfitDisplayName)
                .SetDescription(UnfitDescription)
                .SetIcon(FeatureSelectionRefs.SwordSaintChosenWeaponSelection.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.CheckBluff, value: 1)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.CheckDiplomacy, value: 1)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddToLevelEntries(1, UnfitProficiency)
                .Configure();*/

            var HobgoblinRacialTrait = FeatureSelectionConfigurator.New("HobgoblinRacialTraitSelection", "{523505E3-194B-4FFF-8082-CAF6CC12ACE4}")
                .SetDisplayName(HobgoblinRacialTraitDisplayName)
                .SetDescription(HobgoblinRacialTraitDescription)
                .SetIcon(FeatureSelectionRefs.HalfOrcHeritageSelection.Reference.Get().Icon)
                .AddToAllFeatures(Authoritative, BattleHardened, Fearsome, Magehunter, PitBoss, SlaveHunter, Sneaky, Unfit)
                .SetGroup(FeatureGroup.Racial)
                .Configure();

            var KeenSenses = ProgressionConfigurator.New("HobgoblinKeenSenses", "{AE76CB04-7CDA-4859-903D-72E0EE476033}")
                .SetDisplayName(FeatureRefs.KeenSenses.Reference.Get().m_DisplayName)
                .SetDescription(FeatureRefs.KeenSenses.Reference.Get().m_Description)
                .SetIcon(FeatureRefs.KeenSenses.Reference.Get().Icon)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillPerception, value: 2)
                .AddToLevelEntries(1, HobgoblinFemaleEars, HobgoblinMaleEars)
                .Configure();

            var race =
            RaceConfigurator.New(HobgoblinName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(HobgoblinDisplayName)
                .SetDescription(HobgoblinDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(HobgoblinRacialTrait, KeenSenses)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: 2)
                .AddEquipmentEntity(Contacts)
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.OreadRace.ToString()), CustomMaleHeads: MaleHeads, CustomFemaleHeads: FemaleHeads, BaldRace: true, NoEyebrows: true, NoBeards: true, CustomEyeRamps: CustomEyeRamps, CustomHeadRamps: CustomSkinRamps);

            // Mess around with the bones
            var standardmaleskeleton = GameObject.Instantiate(recoloredrace.m_Presets[0].Get().MaleSkeleton);

            standardmaleskeleton.Bones[103].ApplyOffset = true; // Head
            standardmaleskeleton.Bones[103].Offset = new Vector3() { x = -0.094f, y = 0f, z = 0f };
            standardmaleskeleton.Bones[103].Scale = new Vector3() { x = 1.1f, y = 1.1f, z = 1.13f };
            standardmaleskeleton.Bones[105].ApplyOffset = true; // L_Eye
            standardmaleskeleton.Bones[105].Offset = new Vector3() { x = -0.0325f, y = 0.01055644f, z = 0.012237987f };
            standardmaleskeleton.Bones[112].ApplyOffset = true; // R_Eye
            standardmaleskeleton.Bones[112].Offset = new Vector3() { x = 0.0325f, y = 0.01055644f, z = 0.012237987f };
            standardmaleskeleton.Bones[119].ApplyOffset = true; // Head_Scale
            standardmaleskeleton.Bones[119].Offset = new Vector3() { x = 0f, y = 0f, z = 0f };
            standardmaleskeleton.Bones[119].Scale = new Vector3() { x = 1.1f, y = 1.1f, z = 1.1f };
            standardmaleskeleton.Bones[122].ApplyOffset = true; // Skull Scale
            standardmaleskeleton.Bones[122].Offset = new Vector3() { x = -0.12834f, y = -0.04414f, z = 0f };

            var fatmaleskeleton = GameObject.Instantiate(recoloredrace.m_Presets[1].Get().MaleSkeleton);

            fatmaleskeleton.Bones[103].ApplyOffset = true; // Head
            fatmaleskeleton.Bones[103].Offset = new Vector3() { x = -0.094f, y = 0f, z = 0f };
            fatmaleskeleton.Bones[103].Scale = new Vector3() { x = 1.1f, y = 1.1f, z = 1.13f };
            fatmaleskeleton.Bones[105].ApplyOffset = true; // L_Eye
            fatmaleskeleton.Bones[105].Offset = new Vector3() { x = -0.0325f, y = 0.01055644f, z = 0.012237987f };
            fatmaleskeleton.Bones[112].ApplyOffset = true; // R_Eye
            fatmaleskeleton.Bones[112].Offset = new Vector3() { x = 0.0325f, y = 0.01055644f, z = 0.012237987f };
            fatmaleskeleton.Bones[119].ApplyOffset = true; // Head_Scale
            fatmaleskeleton.Bones[119].Offset = new Vector3() { x = 0f, y = 0f, z = 0f };
            fatmaleskeleton.Bones[119].Scale = new Vector3() { x = 1.1f, y = 1.1f, z = 1.1f };
            fatmaleskeleton.Bones[122].ApplyOffset = true; // Skull Scale
            fatmaleskeleton.Bones[122].Offset = new Vector3() { x = -0.12834f, y = -0.04414f, z = 0f };

            var thinmaleskeleton = GameObject.Instantiate(recoloredrace.m_Presets[2].Get().MaleSkeleton);

            thinmaleskeleton.Bones[103].ApplyOffset = true; // Head
            thinmaleskeleton.Bones[103].Offset = new Vector3() { x = -0.094f, y = 0f, z = 0f };
            thinmaleskeleton.Bones[103].Scale = new Vector3() { x = 1.1f, y = 1.1f, z = 1.13f };
            thinmaleskeleton.Bones[105].ApplyOffset = true; // L_Eye
            thinmaleskeleton.Bones[105].Offset = new Vector3() { x = -0.0325f, y = 0.01055644f, z = 0.012237987f };
            thinmaleskeleton.Bones[112].ApplyOffset = true; // R_Eye
            thinmaleskeleton.Bones[112].Offset = new Vector3() { x = 0.0325f, y = 0.01055644f, z = 0.012237987f };
            thinmaleskeleton.Bones[119].ApplyOffset = true; // Head_Scale
            thinmaleskeleton.Bones[119].Offset = new Vector3() { x = 0f, y = 0f, z = 0f };
            thinmaleskeleton.Bones[119].Scale = new Vector3() { x = 1.1f, y = 1.1f, z = 1.1f };
            thinmaleskeleton.Bones[122].ApplyOffset = true; // Skull Scale
            thinmaleskeleton.Bones[122].Offset = new Vector3() { x = -0.12834f, y = -0.04414f, z = 0f };

            var standardfemaleskeleton = GameObject.Instantiate(recoloredrace.m_Presets[0].Get().FemaleSkeleton);

            standardfemaleskeleton.Bones[106].ApplyOffset = true; // L Neck Muscles
            standardfemaleskeleton.Bones[106].Offset = new Vector3() { x = -0.4089472f, y = -0.043815f, z = -0.05601762f };
            standardfemaleskeleton.Bones[106].Scale = new Vector3() { x = 1f, y = 1f, z = 1f };
            standardfemaleskeleton.Bones[107].ApplyOffset = true; // L Neck Muscle Adj
            standardfemaleskeleton.Bones[107].Offset = new Vector3() { x = 0f, y = 0f, z = 0f };
            standardfemaleskeleton.Bones[107].Scale = new Vector3() { x = 1.6f, y = 1.5f, z = 1.9f };
            standardfemaleskeleton.Bones[108].ApplyOffset = true; // Neck
            standardfemaleskeleton.Bones[108].Offset = new Vector3() { x = -0.200134f, y = -0.07070383f, z = 0f }; // went higher with y .002 starting, higher with x second number from 1 to 5 { x = -0.200134f, y = -0.02770383f, z = 0f }
            standardfemaleskeleton.Bones[108].Scale = new Vector3() { x = 1f, y = 1f, z = 1f }; // went from .9 up to 1.1
            standardfemaleskeleton.Bones[109].ApplyOffset = true; // Head
            standardfemaleskeleton.Bones[109].Offset = new Vector3() { x = -0.094f, y = 0.04f, z = 0f }; // { x = -0.094f, y = 0f, z = 0f }
            standardfemaleskeleton.Bones[109].Scale = new Vector3() { x = 1f, y = 1f, z = 1f };
            standardfemaleskeleton.Bones[110].ApplyOffset = true; // Face_Scale
            standardfemaleskeleton.Bones[110].Offset = new Vector3() { x = -0.06385604f, y = -0.04712171f, z = 0f };
            standardfemaleskeleton.Bones[111].ApplyOffset = true; // L_Eye
            standardfemaleskeleton.Bones[111].Offset = new Vector3() { x = -0.02800881f, y = -0.05451841f, z = -0.03410934f };
            standardfemaleskeleton.Bones[113].ApplyOffset = true; // L_EyeBrow
            standardfemaleskeleton.Bones[113].Offset = new Vector3() { x = -0.04247847f, y = -0.05651564f, z = -0.03293313f };
            standardfemaleskeleton.Bones[114].ApplyOffset = true; // L_EyelashBottom
            standardfemaleskeleton.Bones[114].Offset = new Vector3() { x = -0.02680067f, y = -0.05451842f, z = -0.03410935f };
            standardfemaleskeleton.Bones[116].ApplyOffset = true; // L_Eyelash_Top
            standardfemaleskeleton.Bones[116].Offset = new Vector3() { x = -0.02932997f, y = -0.05451842f, z = -0.03410935f };
            standardfemaleskeleton.Bones[118].ApplyOffset = true; // R_Eye
            standardfemaleskeleton.Bones[118].Offset = new Vector3() { x = -0.02800881f, y = -0.05451841f, z = 0.03410934f };
            standardfemaleskeleton.Bones[120].ApplyOffset = true; // R_Eyebrow
            standardfemaleskeleton.Bones[120].Offset = new Vector3() { x = -0.04247847f, y = -0.05651564f, z = 0.03293313f };
            standardfemaleskeleton.Bones[121].ApplyOffset = true; // R_EyelashBottom
            standardfemaleskeleton.Bones[121].Offset = new Vector3() { x = -0.02680065f, y = -0.05451841f, z = 0.03410935f };
            standardfemaleskeleton.Bones[123].ApplyOffset = true; // R_EyelashTop
            standardfemaleskeleton.Bones[123].Offset = new Vector3() { x = -0.02932995f, y = -0.05451841f, z = 0.03410934f };
            standardfemaleskeleton.Bones[127].ApplyOffset = true; // Jaw Scale
            standardfemaleskeleton.Bones[127].Offset = new Vector3() { x = -0.007503859f, y = -0.05313138f, z = 0f };
            standardfemaleskeleton.Bones[128].ApplyOffset = true; // Skull Scale
            standardfemaleskeleton.Bones[128].Offset = new Vector3() { x = -0.1283521f, y = -0.02632656f, z = 0.00f };
            standardfemaleskeleton.Bones[129].ApplyOffset = true; // Neck Adjust
            standardfemaleskeleton.Bones[129].Offset = new Vector3() { x = 0.00f, y = 0.045f, z = 0.00f }; // { x = 0.00f, y = -0.025f, z = 0.00f }
            standardfemaleskeleton.Bones[129].Scale = new Vector3() { x = 1f, y = 0.8f, z = 0.8f };

            var fatfemaleskeleton = GameObject.Instantiate(recoloredrace.m_Presets[1].Get().FemaleSkeleton);

            fatfemaleskeleton.Bones[106].ApplyOffset = true; // L Neck Muscles
            fatfemaleskeleton.Bones[106].Offset = new Vector3() { x = -0.4089472f, y = -0.043815f, z = -0.05601762f };
            fatfemaleskeleton.Bones[106].Scale = new Vector3() { x = 1f, y = 1f, z = 1f };
            fatfemaleskeleton.Bones[107].ApplyOffset = true; // L Neck Muscle Adj
            fatfemaleskeleton.Bones[107].Offset = new Vector3() { x = 0f, y = 0f, z = 0f };
            fatfemaleskeleton.Bones[107].Scale = new Vector3() { x = 1.6f, y = 1.5f, z = 1.9f };
            fatfemaleskeleton.Bones[108].ApplyOffset = true; // Neck
            fatfemaleskeleton.Bones[108].Offset = new Vector3() { x = -0.200134f, y = -0.07070383f, z = 0f }; // went higher with y .002 starting, higher with x second number from 1 to 5 { x = -0.200134f, y = -0.02770383f, z = 0f }
            fatfemaleskeleton.Bones[108].Scale = new Vector3() { x = 1f, y = 1f, z = 1f }; // went from .9 up to 1.1
            fatfemaleskeleton.Bones[109].ApplyOffset = true; // Head
            fatfemaleskeleton.Bones[109].Offset = new Vector3() { x = -0.094f, y = 0.04f, z = 0f }; // { x = -0.094f, y = 0f, z = 0f }
            fatfemaleskeleton.Bones[109].Scale = new Vector3() { x = 1f, y = 1f, z = 1f };
            fatfemaleskeleton.Bones[110].ApplyOffset = true; // Face_Scale
            fatfemaleskeleton.Bones[110].Offset = new Vector3() { x = -0.06385604f, y = -0.04712171f, z = 0f };
            fatfemaleskeleton.Bones[111].ApplyOffset = true; // L_Eye
            fatfemaleskeleton.Bones[111].Offset = new Vector3() { x = -0.02800881f, y = -0.05451841f, z = -0.03410934f };
            fatfemaleskeleton.Bones[113].ApplyOffset = true; // L_EyeBrow
            fatfemaleskeleton.Bones[113].Offset = new Vector3() { x = -0.04247847f, y = -0.05651564f, z = -0.03293313f };
            fatfemaleskeleton.Bones[114].ApplyOffset = true; // L_EyelashBottom
            fatfemaleskeleton.Bones[114].Offset = new Vector3() { x = -0.02680067f, y = -0.05451842f, z = -0.03410935f };
            fatfemaleskeleton.Bones[116].ApplyOffset = true; // L_Eyelash_Top
            fatfemaleskeleton.Bones[116].Offset = new Vector3() { x = -0.02932997f, y = -0.05451842f, z = -0.03410935f };
            fatfemaleskeleton.Bones[118].ApplyOffset = true; // R_Eye
            fatfemaleskeleton.Bones[118].Offset = new Vector3() { x = -0.02800881f, y = -0.05451841f, z = 0.03410934f };
            fatfemaleskeleton.Bones[120].ApplyOffset = true; // R_Eyebrow
            fatfemaleskeleton.Bones[120].Offset = new Vector3() { x = -0.04247847f, y = -0.05651564f, z = 0.03293313f };
            fatfemaleskeleton.Bones[121].ApplyOffset = true; // R_EyelashBottom
            fatfemaleskeleton.Bones[121].Offset = new Vector3() { x = -0.02680065f, y = -0.05451841f, z = 0.03410935f };
            fatfemaleskeleton.Bones[123].ApplyOffset = true; // R_EyelashTop
            fatfemaleskeleton.Bones[123].Offset = new Vector3() { x = -0.02932995f, y = -0.05451841f, z = 0.03410934f };
            fatfemaleskeleton.Bones[127].ApplyOffset = true; // Jaw Scale
            fatfemaleskeleton.Bones[127].Offset = new Vector3() { x = -0.007503859f, y = -0.05313138f, z = 0f };
            fatfemaleskeleton.Bones[128].ApplyOffset = true; // Skull Scale
            fatfemaleskeleton.Bones[128].Offset = new Vector3() { x = -0.1283521f, y = -0.02632656f, z = 0.00f };
            fatfemaleskeleton.Bones[129].ApplyOffset = true; // Neck Adjust
            fatfemaleskeleton.Bones[129].Offset = new Vector3() { x = 0.00f, y = 0.045f, z = 0.00f }; // { x = 0.00f, y = -0.025f, z = 0.00f }
            fatfemaleskeleton.Bones[129].Scale = new Vector3() { x = 1f, y = 0.8f, z = 0.8f };

            var thinfemaleskeleton = GameObject.Instantiate(recoloredrace.m_Presets[2].Get().FemaleSkeleton);

            thinfemaleskeleton.Bones[106].ApplyOffset = true; // L Neck Muscles
            thinfemaleskeleton.Bones[106].Offset = new Vector3() { x = -0.4089472f, y = -0.043815f, z = -0.05601762f };
            thinfemaleskeleton.Bones[106].Scale = new Vector3() { x = 1f, y = 1f, z = 1f };
            thinfemaleskeleton.Bones[107].ApplyOffset = true; // L Neck Muscle Adj
            thinfemaleskeleton.Bones[107].Offset = new Vector3() { x = 0f, y = 0f, z = 0f };
            thinfemaleskeleton.Bones[107].Scale = new Vector3() { x = 1.6f, y = 1.5f, z = 1.9f };
            thinfemaleskeleton.Bones[108].ApplyOffset = true; // Neck
            thinfemaleskeleton.Bones[108].Offset = new Vector3() { x = -0.200134f, y = -0.07070383f, z = 0f }; // went higher with y .002 starting, higher with x second number from 1 to 5 { x = -0.200134f, y = -0.02770383f, z = 0f }
            thinfemaleskeleton.Bones[108].Scale = new Vector3() { x = 1f, y = 1f, z = 1f }; // went from .9 up to 1.1
            thinfemaleskeleton.Bones[109].ApplyOffset = true; // Head
            thinfemaleskeleton.Bones[109].Offset = new Vector3() { x = -0.094f, y = 0.04f, z = 0f }; // { x = -0.094f, y = 0f, z = 0f }
            thinfemaleskeleton.Bones[109].Scale = new Vector3() { x = 1f, y = 1f, z = 1f };
            thinfemaleskeleton.Bones[110].ApplyOffset = true; // Face_Scale
            thinfemaleskeleton.Bones[110].Offset = new Vector3() { x = -0.06385604f, y = -0.04712171f, z = 0f };
            thinfemaleskeleton.Bones[111].ApplyOffset = true; // L_Eye
            thinfemaleskeleton.Bones[111].Offset = new Vector3() { x = -0.02800881f, y = -0.05451841f, z = -0.03410934f };
            thinfemaleskeleton.Bones[113].ApplyOffset = true; // L_EyeBrow
            thinfemaleskeleton.Bones[113].Offset = new Vector3() { x = -0.04247847f, y = -0.05651564f, z = -0.03293313f };
            thinfemaleskeleton.Bones[114].ApplyOffset = true; // L_EyelashBottom
            thinfemaleskeleton.Bones[114].Offset = new Vector3() { x = -0.02680067f, y = -0.05451842f, z = -0.03410935f };
            thinfemaleskeleton.Bones[116].ApplyOffset = true; // L_Eyelash_Top
            thinfemaleskeleton.Bones[116].Offset = new Vector3() { x = -0.02932997f, y = -0.05451842f, z = -0.03410935f };
            thinfemaleskeleton.Bones[118].ApplyOffset = true; // R_Eye
            thinfemaleskeleton.Bones[118].Offset = new Vector3() { x = -0.02800881f, y = -0.05451841f, z = 0.03410934f };
            thinfemaleskeleton.Bones[120].ApplyOffset = true; // R_Eyebrow
            thinfemaleskeleton.Bones[120].Offset = new Vector3() { x = -0.04247847f, y = -0.05651564f, z = 0.03293313f };
            thinfemaleskeleton.Bones[121].ApplyOffset = true; // R_EyelashBottom
            thinfemaleskeleton.Bones[121].Offset = new Vector3() { x = -0.02680065f, y = -0.05451841f, z = 0.03410935f };
            thinfemaleskeleton.Bones[123].ApplyOffset = true; // R_EyelashTop
            thinfemaleskeleton.Bones[123].Offset = new Vector3() { x = -0.02932995f, y = -0.05451841f, z = 0.03410934f };
            thinfemaleskeleton.Bones[127].ApplyOffset = true; // Jaw Scale
            thinfemaleskeleton.Bones[127].Offset = new Vector3() { x = -0.007503859f, y = -0.05313138f, z = 0f };
            thinfemaleskeleton.Bones[128].ApplyOffset = true; // Skull Scale
            thinfemaleskeleton.Bones[128].Offset = new Vector3() { x = -0.1283521f, y = -0.02632656f, z = 0.00f };
            thinfemaleskeleton.Bones[129].ApplyOffset = true; // Neck Adjust
            thinfemaleskeleton.Bones[129].Offset = new Vector3() { x = 0.00f, y = 0.045f, z = 0.00f }; // { x = 0.00f, y = -0.025f, z = 0.00f }
            thinfemaleskeleton.Bones[129].Scale = new Vector3() { x = 1f, y = 0.8f, z = 0.8f };

            var newstandardpreset = AltHelpers.CreateCopyAlt(recoloredrace.m_Presets[0].Get(), p =>
            {
                p.name = recoloredrace.name + "_NewOffsets_" + p.name;
                p.MaleSkeleton = standardmaleskeleton;
                p.FemaleSkeleton = standardfemaleskeleton;
                p.AssetGuid = BlueprintGuid.Parse("{C3BBF266-6461-4910-A9AB-3E7B7C4D4F4A}");
                RaceRecolorizer.AddBlueprint(p, p.AssetGuid);
            });

            var newfatpreset = AltHelpers.CreateCopyAlt(recoloredrace.m_Presets[1].Get(), p =>
            {
                p.name = recoloredrace.name + "_NewOffsets_" + p.name;
                p.MaleSkeleton = fatmaleskeleton;
                p.FemaleSkeleton = fatfemaleskeleton;
                p.AssetGuid = BlueprintGuid.Parse("{1B2E3ECC-12D8-4183-BDE2-08669A64611C}");
                RaceRecolorizer.AddBlueprint(p, p.AssetGuid);
            });

            var newthinpreset = AltHelpers.CreateCopyAlt(recoloredrace.m_Presets[2].Get(), p =>
            {
                p.name = recoloredrace.name + "_NewOffsets_" + p.name;
                p.MaleSkeleton = thinmaleskeleton;
                p.FemaleSkeleton = thinfemaleskeleton;
                p.AssetGuid = BlueprintGuid.Parse("{D822C0AA-631D-48D7-B95C-10C67E736254}");
                RaceRecolorizer.AddBlueprint(p, p.AssetGuid);
            });

            var FinalRace = RaceConfigurator.For(recoloredrace)
                .SetPresets(newstandardpreset, newfatpreset, newthinpreset)
                .Configure();

            EquipmentEntityLink[] SkinLinks = [HobgoblinMaleEarsEE, HobgoblinFemaleEarsEE];
            EquipmentEntityLink[] EyeLinks = [Contacts];

            // Register linked EEs
            EELinker.RegisterSkinLink(FinalRace, SkinLinks);
            EELinker.RegisterEyeLink(FinalRace, EyeLinks);

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
