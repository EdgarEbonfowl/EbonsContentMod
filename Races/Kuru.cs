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

namespace EbonsContentMod.Races
{
    internal class Kuru
    {
        public static List<Color> RaceHeadColors =
        [
            new Color( // Dark Gray
                RaceRecolorizer.GetColorsFromRGB(65f),
                RaceRecolorizer.GetColorsFromRGB(65f),
                RaceRecolorizer.GetColorsFromRGB(65f)
                ),
            new Color( // Less Dark Gray
                RaceRecolorizer.GetColorsFromRGB(90f),
                RaceRecolorizer.GetColorsFromRGB(90f),
                RaceRecolorizer.GetColorsFromRGB(95f)
                ),
            new Color( // Dark Red
                RaceRecolorizer.GetColorsFromRGB(199f),
                RaceRecolorizer.GetColorsFromRGB(79f),
                RaceRecolorizer.GetColorsFromRGB(65f)
                ),
            new Color( // Less Dark Red
                RaceRecolorizer.GetColorsFromRGB(186f),
                RaceRecolorizer.GetColorsFromRGB(105f),
                RaceRecolorizer.GetColorsFromRGB(97f)
                )
        ];

        public static List<Color> RaceEyeColors =
        [
            new Color( // Fiery Orange
                RaceRecolorizer.GetColorsFromRGB(235f),
                RaceRecolorizer.GetColorsFromRGB(119f),
                RaceRecolorizer.GetColorsFromRGB(38f)
                ),
            new Color( // Fiery Red
                RaceRecolorizer.GetColorsFromRGB(235f),
                RaceRecolorizer.GetColorsFromRGB(73f),
                RaceRecolorizer.GetColorsFromRGB(38f)
                )
        ];

        public static List<Color> RaceHairColors =
        [
            new Color(0.11764706f, 0.11764706f, 0.11764706f),
            new Color(0.0f, 0.0f, 0.0f),
            new Color( // Dark Brown
                RaceRecolorizer.GetColorsFromRGB(50f),
                RaceRecolorizer.GetColorsFromRGB(35f),
                RaceRecolorizer.GetColorsFromRGB(10f)
                ),
            new Color( // Brown
                RaceRecolorizer.GetColorsFromRGB(72f),
                RaceRecolorizer.GetColorsFromRGB(53f),
                RaceRecolorizer.GetColorsFromRGB(13f)
                ),
            new Color( // Medium Brown
                RaceRecolorizer.GetColorsFromRGB(85f),
                RaceRecolorizer.GetColorsFromRGB(75f),
                RaceRecolorizer.GetColorsFromRGB(55f)
                ),
            new Color( // Deep Red
                RaceRecolorizer.GetColorsFromRGB(120f),
                RaceRecolorizer.GetColorsFromRGB(55f),
                RaceRecolorizer.GetColorsFromRGB(50f)
                ),
            new Color( // Deep Red-Brown
                RaceRecolorizer.GetColorsFromRGB(100f),
                RaceRecolorizer.GetColorsFromRGB(50f),
                RaceRecolorizer.GetColorsFromRGB(0f)
                ),
            new Color( // Orange
                RaceRecolorizer.GetColorsFromRGB(140f),
                RaceRecolorizer.GetColorsFromRGB(80f),
                RaceRecolorizer.GetColorsFromRGB(0f)
                ),

        ];

        public static List<Texture2D> CustomSkinRamps =
        [
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString()).MaleOptions.Heads[0].Load().PrimaryColorsProfile.Ramps[11],
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString()).MaleOptions.Heads[0].Load().PrimaryColorsProfile.Ramps[8],
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString()).MaleOptions.Heads[0].Load().PrimaryColorsProfile.Ramps[10],
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString()).MaleOptions.Heads[0].Load().PrimaryColorsProfile.Ramps[3],
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString()).MaleOptions.Heads[0].Load().PrimaryColorsProfile.Ramps[4],
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString()).MaleOptions.Heads[0].Load().PrimaryColorsProfile.Ramps[0]
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

        public static List<Texture2D> CustomHairRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString()).MaleOptions.Hair[1].Load(true, false).m_PrimaryRamps;

        public static EquipmentEntityLink[] MaleHeads =
        [
            new EquipmentEntityLink() {AssetId = "80f9866c56cea9342a354636154d0fae"}, // 11 Evil Male Human
            new EquipmentEntityLink() {AssetId = "554fe1a46ebf83d40a461eb355c290f1"}, // 09 Male Human
            new EquipmentEntityLink() {AssetId = "913b11311ec6b9f48bfa23d1f3423154"}, // 05 Male Human
            new EquipmentEntityLink() {AssetId = "9149601d28921464ab3e8fe0ecd3b462"} // 04 Good Male Human
        ];

        public static EquipmentEntityLink[] FemaleHeads =
        [
            new EquipmentEntityLink() {AssetId = "f0ae81a538f807f46816f8cb78709115"}, // 08 Female Human
            new EquipmentEntityLink() {AssetId = "5bc92526cbe57564f96530aefe05e4c6"}, // 04 Female Human
            new EquipmentEntityLink() {AssetId = "d4dc8479fcc84784c92e6e779dd25493"}, // 09 Seelah Female Human
            new EquipmentEntityLink() {AssetId = "1709c29af3fd6634cbcc4cf1a24dd8ee"} // 06 Female Human
        ];

        public static EquipmentEntityLink[] MaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "329cf540a8faed64284c067bace8bbc9"}, // Trim Male Human
            new EquipmentEntityLink() {AssetId = "acdcfd7609f88ae49833e4f10656190e"}, // Long Bangs Male Human
            new EquipmentEntityLink() {AssetId = "222890293b0f66145a400eae3432868d"}, // Mohawk Male Human
            new EquipmentEntityLink() {AssetId = "303578a648d8d344b8d3a9a94fe24d5a"}, // Long Wavy male Human
            new EquipmentEntityLink() {AssetId = "d174619e428101e41b5675bd6286b1d4"}, // Medium Male Tiefling
            new EquipmentEntityLink() {AssetId = "def666224ba24df4e954c03049b29a53"}, // Short Male Human
            new EquipmentEntityLink() {AssetId = "50eac92ba30862940be4f70d329d070a"}, // Long Wild Male Tiefling
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"} // Bald
        ];

        public static EquipmentEntityLink[] FemaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "fc3fc0e45a70a0e42b6aed10baf794f0"}, // Dread Seelah Female Human
            new EquipmentEntityLink() {AssetId = "30d504db6b8cbe94dbc82d2437c8b468"}, // Long Wavy Female Human
            new EquipmentEntityLink() {AssetId = "d71d2e53fce0f1d4baad8b20c8266676"}, // Slick Female Tiefling
            new EquipmentEntityLink() {AssetId = "afa22656ed5030c4ba273583ba2b3a16"}, // Long Wild Female Tiefling
            new EquipmentEntityLink() {AssetId = "ad6c23c1e8e7e374b9864dea8fcc381d"}, // Mohawk Wenduag Female Mongrel
            new EquipmentEntityLink() {AssetId = "f32da5106fa223844b88c426e36b5821"}, // Ponytail Classic Female Human
            new EquipmentEntityLink() {AssetId = "d90a0bf179ad5884a98092b58d8f76ad"}, // Mohawk Female Tiefling
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"} // Bald
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string KuruName = "KuruRace";

        internal const string KuruDisplayName = "Kuru.Name";
        private static readonly string KuruDescription = "Kuru.Description";
        public static readonly string RaceGuid = "{000616AC-4BC4-4E26-9063-215D094CD6FF}";

        internal const string BiteAttackDisplayName = "Kuru.BiteAttack.Name";
        private static readonly string BiteAttackDescription = "Kuru.BiteAttack.Description";

        internal const string BloodCourageDisplayName = "Kuru.BloodCourage.Name";
        private static readonly string BloodCourageDescription = "Kuru.BloodCourage.Description";

        internal const string CannabalisticVitalityDisplayName = "Kuru.CannabalisticVitality.Name";
        private static readonly string CannabalisticVitalityDescription = "Kuru.CannabalisticVitality.Description";

        internal static void Configure()
        {
            var BiteWeapon = ItemWeaponConfigurator.New("KuruBiteWeapon", "{EBD45E84-C7F5-4B3A-AAE1-B3524FC68B20}")
                .CopyFrom(ItemWeaponRefs.Bite1d6)
                //.SetAlwaysPrimary(true)
                .Configure();

            var ContactEEL = new EquipmentEntityLink() { AssetId = "c5487e7e903d25a40be683767f3df0b4" };

            var FirstMaleTattoo = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "8128c475040f4144ea08e8d27c2a06c2" }, new List<Texture2D>() { RaceRecolorizer.GetArmorRampByIndex(79) }, "{66081D2B-F9FB-42FF-AD0A-71A387A8F361}", true);
            var SecondMaleTattoo = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "993321276ea944f46b00a376a8847c1a" }, new List<Texture2D>() { RaceRecolorizer.GetArmorRampByIndex(79) }, "{D5AE805C-EA0F-478D-A1B2-111F870669C3}", true);
            var ThirdMaleTattoo = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "85e30880982534646acf10ba33a09c08" }, new List<Texture2D>() { RaceRecolorizer.GetArmorRampByIndex(79) }, "{BC72B0FA-477A-416D-96F5-A0B67D5D33F5}", true);

            var FirstFemaleTattoo = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "2c8bf4780120d4246ad32fcfee23dd59" }, new List<Texture2D>() { RaceRecolorizer.GetArmorRampByIndex(79) }, "{FCB14F35-BCAD-4FA3-BB04-340D0F65FE1A}", true);
            var SecondFemaleTattoo = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "c5ffacf65c9869244bed1a264b98ef5f" }, new List<Texture2D>() { RaceRecolorizer.GetArmorRampByIndex(79) }, "{B33A1F19-0CAE-434D-9C53-8C23A9EB0BD2}", true);
            var ThirdFemaleTattoo = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "8e817f91fd12d7645bc775c57b4ca91a" }, new List<Texture2D>() { RaceRecolorizer.GetArmorRampByIndex(79) }, "{BC8DF1D2-09CE-439B-B61D-24F4B4E3357C}", true);

            var MaleTattoos = FeatureConfigurator.New("KuruMaleTattoos", "{4B787DB3-55A5-4034-B95B-32762AFE48C4}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity(FirstMaleTattoo)
                .AddEquipmentEntity(SecondMaleTattoo)
                .AddEquipmentEntity(ThirdMaleTattoo)
                .Configure();

            var FemaleTattoos = FeatureConfigurator.New("KuruFemaleTattoos", "{3394C486-5B76-42BD-A7DB-8CFE37F42908}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity(FirstFemaleTattoo)
                .AddEquipmentEntity(SecondFemaleTattoo)
                .AddEquipmentEntity(ThirdFemaleTattoo)
                .Configure();

            var BiteAttack = ProgressionConfigurator.New("KuruBiteAttack", "{C5D45F5B-2AD8-4E59-B830-C279D63045A4}")
                .SetDisplayName(BiteAttackDisplayName)
                .SetDescription(BiteAttackDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.AcidMaw.ToString()).Icon)
                .AddAdditionalLimb(BiteWeapon)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddToLevelEntries(1, MaleTattoos, FemaleTattoos)
                .Configure();

            var BloodCourage = FeatureConfigurator.New("KuruBloodCourage", "{4D84001A-CC7C-4D07-8773-635B67DC881F}")
                .SetDisplayName(BloodCourageDisplayName)
                .SetDescription(BloodCourageDescription)
                .SetIcon(FeatureRefs.AuraOfCourageFeature.Reference.Get().Icon)
                .AddSavingThrowBonusAgainstDescriptor(2, modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.Fear)
                .Configure();

            var TempHPBuff = BuffConfigurator.New("KuruCannabalisticVitalityBuff", "{DA56164F-52F6-448E-BF3B-972911432762}")
                .SetDisplayName(CannabalisticVitalityDisplayName)
                .SetDescription(CannabalisticVitalityDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.CrushAndTearFeature.ToString()).Icon)
                .AddTemporaryHitPointsFromAbilityValue(removeWhenHitPointsEnd: true, value: ContextValues.Constant(1))
                .SetStacking(Kingmaker.UnitLogic.Buffs.Blueprints.StackingType.Replace)
                .Configure();
            
            var actions = ActionFlow.DoSingle<Conditional>(ac =>
            {
                ac.ConditionsChecker = ActionFlow.IfAny(
                    new ContextConditionHasFact()
                    {
                        m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("734a29b693e9ec346ba2951b27987e33"),
                        Not = true,
                    },
                    new ContextConditionHasFact()
                    {
                        m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("fd389783027d63343b4a5634bd81645f"),
                        Not = true
                    }
                );
                ac.IfTrue = ActionFlow.DoSingle<ContextActionOnContextCaster>(bc =>
                {
                    bc.Actions = ActionFlow.DoSingle<ContextActionApplyBuff>(cc =>
                    {
                        cc.m_Buff = TempHPBuff.ToReference<BlueprintBuffReference>();
                        cc.UseDurationSeconds = false;
                        cc.DurationValue = new ContextDurationValue()
                        {
                            Rate = DurationRate.Hours,
                            DiceType = DiceType.Zero,
                            DiceCountValue = 0,
                            BonusValue = 24,
                            m_IsExtendable = true
                        };
                        cc.DurationSeconds = 0;
                    });
                });
                ac.IfFalse = ActionFlow.DoNothing();
            });

            var CannabalisticVitality = FeatureConfigurator.New("KuruCannabalisticVitality", "{C7B3C169-66C9-4E79-844E-0A335E8C0094}")
                .SetDisplayName(CannabalisticVitalityDisplayName)
                .SetDescription(CannabalisticVitalityDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.CrushAndTearFeature.ToString()).Icon)
                .AddComponent<AddInitiatorAttackWithWeaponTrigger>(c =>
                {
                    c.OnlyHit = true;
                    c.CheckWeaponBlueprint = true;
                    c.m_WeaponBlueprint = BiteWeapon.ToReference<BlueprintItemWeaponReference>();
                    c.Action = actions;
                })
                .Configure();

            var LightSensitivity = BlueprintTools.GetBlueprint<BlueprintFeature>("{BBA86DB1-B3EF-4AB5-A412-A5910F2B8947}");

            var race =
            RaceConfigurator.New(KuruName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(KuruDisplayName)
                .SetDescription(KuruDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(BiteAttack, BloodCourage, CannabalisticVitality, FeatureRefs.KeenSenses.ToString(), LightSensitivity)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: -2)
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, eyeEE: ContactEEL, CustomMaleHeads: MaleHeads, CustomFemaleHeads: FemaleHeads, CustomFemaleHairs: FemaleHairs, CustomMaleHairs: MaleHairs, CustomEyeRamps: CustomEyeRamps, CustomHeadRamps: CustomSkinRamps);

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
