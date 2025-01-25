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
using Kingmaker.UnitLogic.FactLogic;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using TabletopTweaks.Core.NewComponents;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Visual.CharacterSystem;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;

namespace EbonsContentMod.Races
{
    internal class Ganzi
    {
        public static List<Color> RaceHeadColors = [];

        public static List<Color> RaceEyeColors = [];

        public static List<Color> RaceHairColors = [];

        public static EquipmentEntityLink[] FemaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "fc3fc0e45a70a0e42b6aed10baf794f0"}, // Dread Seelah Female Human
            new EquipmentEntityLink() {AssetId = "779458079f7718c4bb960d9cef195339"}, // Long Wavy Braids
            new EquipmentEntityLink() {AssetId = "04c3eb6d7570d8d49b686516b7c4a4f8"}, // Long Camelia Hair
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
            new EquipmentEntityLink() {AssetId = "222890293b0f66145a400eae3432868d"}, // Mohawk Male Human
            new EquipmentEntityLink() {AssetId = "88c2650d77d9a3c4a8a861fa0d8d0aae"}, // Military Male Human
            new EquipmentEntityLink() {AssetId = "def666224ba24df4e954c03049b29a53"}, // Short Human
            new EquipmentEntityLink() {AssetId = "329cf540a8faed64284c067bace8bbc9"}, // Trim Male Human
            new EquipmentEntityLink() {AssetId = "609143dbf7607f6419babaf5748b82dc"}, // Medium Bun Male Human
            new EquipmentEntityLink() {AssetId = "303578a648d8d344b8d3a9a94fe24d5a"}, // Long Wavy Male Human
            new EquipmentEntityLink() {AssetId = "acdcfd7609f88ae49833e4f10656190e"}, // Long Bangs Male Human
            new EquipmentEntityLink() {AssetId = "50eac92ba30862940be4f70d329d070a"}, // Long Wild Male Tiefling
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"}  // Bald
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string GanziName = "GanziRace";

        internal const string GanziDisplayName = "Ganzi.Name";
        private static readonly string GanziDescription = "Ganzi.Description";
        public static readonly string RaceGuid = "{14BE0C29-67A8-42FE-BD85-3380AD785CE5}";

        internal const string MaelstromResistanceDisplayName = "Ganzi.MaelstromResistance.Name";
        private static readonly string MaelstromResistanceDescription = "Ganzi.MaelstromResistance.Description";

        internal const string SkilledDisplayName = "Ganzi.Skilled.Name";
        private static readonly string SkilledDescription = "Ganzi.Skilled.Description";

        internal const string OddityDisplayName = "Ganzi.Oddity.Name";
        private static readonly string OddityDescription = "Ganzi.Oddity.Description";

        internal const string PrehensileTailDisplayName = "Ganzi.PrehensileTail.Name";
        private static readonly string PrehensileTailDescription = "Ganzi.PrehensileTail.Description";

        internal const string EntropicFleshDisplayName = "Ganzi.EntropicFlesh.Name";
        private static readonly string EntropicFleshDescription = "Ganzi.EntropicFlesh.Description";

        internal const string WeaponPlayDisplayName = "Ganzi.WeaponPlay.Name";
        private static readonly string WeaponPlayDescription = "Ganzi.WeaponPlay.Description";

        internal const string SLADisplayName = "Ganzi.SLA.Name";
        private static readonly string SLADescription = "Ganzi.SLA.Description";

        internal const string RacingMindDisplayName = "Ganzi.RacingMind.Name";
        private static readonly string RacingMindDescription = "Ganzi.RacingMind.Description";

        internal const string AmorphousLimbsDisplayName = "Ganzi.AmorphousLimbs.Name";
        private static readonly string AmorphousLimbsDescription = "Ganzi.AmorphousLimbs.Description";

        internal const string VestigialWingsDisplayName = "Ganzi.VestigialWings.Name";
        private static readonly string VestigialWingsDescription = "Ganzi.VestigialWings.Description";

        internal const string UncannyAuraDisplayName = "Ganzi.UncannyAura.Name";
        private static readonly string UncannyAuraDescription = "Ganzi.UncannyAura.Description";

        internal static void Configure()
        {
            var DhampirSkinColors = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DhampirRace.ToString()).MaleOptions.Heads[0].Load().PrimaryRamps;


            List<Texture2D> CustomSkinColors = 
            [
                DhampirSkinColors[9],
                DhampirSkinColors[10],
                DhampirSkinColors[0],
                DhampirSkinColors[1],
                DhampirSkinColors[3],
                DhampirSkinColors[4],
                DhampirSkinColors[5]
            ];

            CustomSkinColors.AddRange(BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).MaleOptions.Heads[0].Load().PrimaryRamps);
            CustomSkinColors.AddRange(BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HalfElfRace.ToString()).MaleOptions.Heads[0].Load().PrimaryRamps);

            var CustomHairColors = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.GnomeRace.ToString()).MaleOptions.Hair[0].Load().PrimaryRamps;
            CustomHairColors.AddRange(BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DhampirRace.ToString()).MaleOptions.Hair[0].Load().PrimaryRamps);
            CustomHairColors.AddRange(BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HalfElfRace.ToString()).MaleOptions.Hair[0].Load().PrimaryRamps);

            var CustomEyeColors = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.KitsuneRace.ToString()).MaleOptions.Heads[0].Load().SecondaryRamps;
            CustomEyeColors.AddRange(BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.OreadRace.ToString()).MaleOptions.Heads[0].Load().SecondaryRamps);
            CustomEyeColors.AddRange(BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.GnomeRace.ToString()).MaleOptions.Heads[0].Load().SecondaryRamps);
            CustomEyeColors.AddRange(BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DhampirRace.ToString()).MaleOptions.Heads[0].Load().SecondaryRamps);

            var ContactEEL = new EquipmentEntityLink() { AssetId = "c5487e7e903d25a40be683767f3df0b4" };

            var SpellLikeResource = AbilityResourceConfigurator.New("GanziSpellLikeResource", "{C2229862-3749-454F-8E81-AE322D2BF28A}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var PrehensileTail = FeatureConfigurator.New("GanziPrehensileTail", "{339A8657-B384-4076-B666-569B36A93399}")
                .SetDisplayName(PrehensileTailDisplayName)
                .SetDescription(PrehensileTailDescription)
                .SetIcon(FeatureRefs.ChokingBombFeature.Reference.Get().Icon)
                .AddMechanicsFeature(AddMechanicsFeature.MechanicsFeatureType.FastPotion)
                .AddEquipmentEntity("ab9ab7d63fb738249a1a1e9ec6b6c4ff")
                .Configure();

            var EntropicFleshEEL = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "33c1acb9aaa60dc448691b6d63fcb22f" }, RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>() { new Color( // Pink-Purple
                RaceRecolorizer.GetColorsFromRGB(145f), // 140
                RaceRecolorizer.GetColorsFromRGB(75f),
                RaceRecolorizer.GetColorsFromRGB(100f) // 75
                )}), "{DC1E4CE3-6291-440C-816F-222E7D0D62CA}", true, true);

            var EntropicFlesh = FeatureConfigurator.New("GanziEntropicFlesh", "{EE019616-298A-4EFF-AB9F-ED79C4AAE0B0}") // Need to add the transmutation re-roll or just give a bonus to saves
                .SetDisplayName(EntropicFleshDisplayName)
                .SetDescription(EntropicFleshDescription)
                .SetIcon(AbilityRefs.PhantasmalPutrefaction.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.UntypedStackable, stat: StatType.SkillMobility, value: 2)
                .AddCMDBonusAgainstManeuvers(ModifierDescriptor.UntypedStackable, [CombatManeuver.Grapple], 2)
                .AddEquipmentEntity(EntropicFleshEEL)
                .Configure();

            var MaleWeaponPlayEEL = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "21f2c5004173a514982d8491a55734d4" }, new List<Texture2D>(), "{827DBF12-5D56-4A5B-8F57-A31BA319FCAF}", true, true, SkipPrimaryColors: true,
                BodyPartsToRemove: new List<BodyPartType>() { BodyPartType.Eyes, BodyPartType.Head, BodyPartType.Ears, BodyPartType.UpperLegs, BodyPartType.LowerLegs, BodyPartType.Feet });

            var FemaleWeaponPlayEEL = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "8c1f1801126dc2444ab2ea2312f28b03" }, new List<Texture2D>(), "{88BF8EE3-0993-42F9-A509-4593F6BC3C59}", true, true, SkipPrimaryColors: true,
                BodyPartsToRemove: new List<BodyPartType>() { BodyPartType.Eyes, BodyPartType.Head, BodyPartType.Ears, BodyPartType.UpperLegs, BodyPartType.LowerLegs, BodyPartType.Feet });

            var MaleWeaponPlay = FeatureConfigurator.New("GanziMaleWeaponPlay", "{3CD7AAF4-F849-43D9-AF5A-6D546959A64C}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity(MaleWeaponPlayEEL)
                .Configure();

            var FemaleWeaponPlay = FeatureConfigurator.New("GanziFemaleWeaponPlay", "{DA58B3C1-01F7-4C61-8EA9-D6B03897C7DF}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity(FemaleWeaponPlayEEL)
                .Configure();

            var WeaponPlay = ProgressionConfigurator.New("GanziWeaponPlay", "{C0159091-492A-4C29-9AD6-34497277AB33}")
                .SetDisplayName(WeaponPlayDisplayName)
                .SetDescription(WeaponPlayDescription)
                .SetIcon(FeatureRefs.WeaponMastery.Reference.Get().Icon)
                .AddFacts([FeatureRefs.MartialWeaponProficiency.ToString(), FeatureRefs.SimpleWeaponProficiency.ToString()])
                .AddClassLevelsForPrerequisites(fakeClass: CharacterClassRefs.FighterClass.Reference.Get(), forSelection: FeatureSelectionRefs.BasicFeatSelection.Reference.Get(), modifier: 1.0, summand: 0)
                .AddToLevelEntries(1, FemaleWeaponPlay, MaleWeaponPlay)
                .Configure();

            // May need to go up or down one with ramp index for best color
            var SLAEyesEE = RaceRecolorizer.RecolorEELink(ContactEEL, new List<Texture2D>() { RaceRecolorizer.GetArmorRampByIndex(67) }, "{8E78F88F-2ECF-4A37-A30B-78A3876BBBA1}", true, true);

            var HideousLaughter = AbilityConfigurator.New("GanziHideousLaughterAbility", "{3E77573F-A35E-442E-B57B-73051F2DD3CB}")
                .CopyFrom(AbilityRefs.HideousLaughterTiefling, c => c is not (SpellListComponent or CraftInfoComponent))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 1)
                .Configure();

            var HideousLaughterFeat = FeatureConfigurator.New("GanziHideousLaughter", "{00B807CF-00E1-4A53-A53D-75868D0E3F5A}")
                .SetDisplayName(AbilityRefs.HideousLaughter.Reference.Get().m_DisplayName)
                .SetDescription(AbilityRefs.HideousLaughter.Reference.Get().m_Description)
                .SetIcon(AbilityRefs.HideousLaughter.Reference.Get().Icon)
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: HideousLaughter)
                .AddEquipmentEntity(SLAEyesEE)
                .Configure();

            var Blur = AbilityConfigurator.New("GanziBlurAbility", "{51E8D931-1FDD-4C22-BED4-A38F3147DE95}")
                .CopyFrom(AbilityRefs.BlurTiefling, c => c is not (SpellListComponent or CraftInfoComponent))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 1)
                .Configure();

            var BlurFeat = FeatureConfigurator.New("GanziBlur", "{6CF62EFE-8022-48CB-8820-CDC923BCAF98}")
                .SetDisplayName(AbilityRefs.Blur.Reference.Get().m_DisplayName)
                .SetDescription(AbilityRefs.Blur.Reference.Get().m_Description)
                .SetIcon(AbilityRefs.Blur.Reference.Get().Icon)
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: Blur)
                .AddEquipmentEntity(SLAEyesEE)
                .Configure();

            var AcidArrow = AbilityConfigurator.New("GanziAcidArrowAbility", "{9407B82B-6249-410A-9219-8A1276FE7BC6}")
                .CopyFrom(AbilityRefs.AcidArrow, c => c is not (SpellListComponent or CraftInfoComponent))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 2)
                .Configure();

            var AcidArrowFeat = FeatureConfigurator.New("GanziAcidArrow", "{3CF144F7-1E45-4D21-BB6D-909CD289420C}")
                .SetDisplayName(AbilityRefs.AcidArrow.Reference.Get().m_DisplayName)
                .SetDescription(AbilityRefs.AcidArrow.Reference.Get().m_Description)
                .SetIcon(AbilityRefs.AcidArrow.Reference.Get().Icon)
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: AcidArrow)
                .AddEquipmentEntity(SLAEyesEE)
                .Configure();

            var MirrorImage = AbilityConfigurator.New("GanziMirrorImageAbility", "{AD9486C0-4CD6-4FD2-9FBD-8615D5B643C2}")
                .CopyFrom(AbilityRefs.MirrorImage, c => c is not (SpellListComponent or CraftInfoComponent))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 2)
                .Configure();

            var MirrorImageFeat = FeatureConfigurator.New("GanziMirrorImage", "{2112EBC1-7A56-4082-9F7E-32ED8248AE5A}")
                .SetDisplayName(AbilityRefs.MirrorImage.Reference.Get().m_DisplayName)
                .SetDescription(AbilityRefs.MirrorImage.Reference.Get().m_Description)
                .SetIcon(AbilityRefs.MirrorImage.Reference.Get().Icon)
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: MirrorImage)
                .AddEquipmentEntity(SLAEyesEE)
                .Configure();

            var ScorchingRay = AbilityConfigurator.New("GanziScorchingRayAbility", "{58046244-920F-43AD-B473-6E190ECCDC20}")
                .CopyFrom(AbilityRefs.ScorchingRay, c => c is not (SpellListComponent or CraftInfoComponent))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 2)
                .Configure();

            var ScorchingRayFeat = FeatureConfigurator.New("GanziScorchingRay", "{EE473F53-F08A-4E38-88FB-59BAD7F1327F}")
                .SetDisplayName(AbilityRefs.ScorchingRay.Reference.Get().m_DisplayName)
                .SetDescription(AbilityRefs.ScorchingRay.Reference.Get().m_Description)
                .SetIcon(AbilityRefs.ScorchingRay.Reference.Get().Icon)
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: ScorchingRay)
                .AddEquipmentEntity(SLAEyesEE)
                .Configure();

            var ResistEnergy = AbilityConfigurator.New("GanziResistEnergyAbility", "{262974ED-CD5D-4501-9B36-10CF59BCEF36}")
                .CopyFrom(AbilityRefs.ResistEnergy, c => c is not (SpellListComponent or CraftInfoComponent))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 2)
                .Configure();

            var ResistEnergyFeat = FeatureConfigurator.New("GanziResistEnergy", "{EFF6BEF5-E8FC-4001-8E31-E03506181BAA}")
                .SetDisplayName(AbilityRefs.ResistEnergy.Reference.Get().m_DisplayName)
                .SetDescription(AbilityRefs.ResistEnergy.Reference.Get().m_Description)
                .SetIcon(AbilityRefs.ResistEnergy.Reference.Get().Icon)
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: ResistEnergy)
                .AddEquipmentEntity(SLAEyesEE)
                .Configure();

            var SeeInvisibility = AbilityConfigurator.New("GanziSeeInvisibilityAbility", "{0C7DDBB7-1BD9-4C3B-9356-55B9D3A1586A}")
                .CopyFrom(AbilityRefs.SeeInvisibility, c => c is not (SpellListComponent or CraftInfoComponent))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 2)
                .Configure();

            var SeeInvisibilityFeat = FeatureConfigurator.New("GanziSeeInvisibility", "{1D627D9A-08FD-4BE5-B077-6F5EEC61ACDC}")
                .SetDisplayName(AbilityRefs.SeeInvisibility.Reference.Get().m_DisplayName)
                .SetDescription(AbilityRefs.SeeInvisibility.Reference.Get().m_Description)
                .SetIcon(AbilityRefs.SeeInvisibility.Reference.Get().Icon)
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: SeeInvisibility)
                .AddEquipmentEntity(SLAEyesEE)
                .Configure();

            var SLA = FeatureSelectionConfigurator.New("GanziSLASelection", "{51E53370-8281-42F6-B0E1-A0B2EAC3B862}")
                .SetDisplayName(SLADisplayName)
                .SetDescription(SLADescription)
                .SetIcon(FeatureRefs.GnomeMagic.Reference.Get().Icon)
                .AddToAllFeatures(HideousLaughterFeat, BlurFeat, AcidArrowFeat, MirrorImageFeat, ScorchingRayFeat, ResistEnergyFeat, SeeInvisibilityFeat)
                .Configure();

            var RacingMindEyesEE = RaceRecolorizer.RecolorEELink(ContactEEL, new List<Texture2D>() { RaceRecolorizer.GetArmorRampByIndex(25) }, "{BDA49850-96F7-427F-907A-4B6E4D84B3D9}", true, true);

            var RacingMindHeadEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "7109791d63944254589b908564604c79" }, RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>() { new Color( // Pink-Purple
                RaceRecolorizer.GetColorsFromRGB(140f), // 140
                RaceRecolorizer.GetColorsFromRGB(0f),
                RaceRecolorizer.GetColorsFromRGB(90f) // 75
                )}), "{FC1822EA-CE30-4148-A85B-E7264010DB5D}", true, true,
                BodyPartsToRemove: new List<BodyPartType>() { BodyPartType.Eyes, BodyPartType.UpperLegs, BodyPartType.LowerLegs, BodyPartType.Feet });

            var RacingMind = FeatureConfigurator.New("GanziRacingMind", "{0F7BD838-67FC-48B6-9224-A3F152EFA9DF}")
                .SetDisplayName(RacingMindDisplayName)
                .SetDescription(RacingMindDescription)
                .SetIcon(AbilityRefs.MindBlank.Reference.Get().Icon)
                .AddSavingThrowBonusAgainstDescriptor(2, modifierDescriptor: ModifierDescriptor.UntypedStackable, spellDescriptor: SpellDescriptor.MindAffecting)
                .AddEquipmentEntity(RacingMindEyesEE)
                .AddEquipmentEntity(RacingMindHeadEE)
                .Configure();

            var AmorphousLimbsFurEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "1aeb459da29dca341a78317170eec262" },
                RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Dark Pink
                    RaceRecolorizer.GetColorsFromRGB(90f),
                    RaceRecolorizer.GetColorsFromRGB(40f),
                    RaceRecolorizer.GetColorsFromRGB(60f)
                    )}
                ), "{2C096337-B17E-45CE-A3F4-DDC2BD4877D6}", true, true,
                RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                    {new Color( // Pink
                        RaceRecolorizer.GetColorsFromRGB(145f),
                        RaceRecolorizer.GetColorsFromRGB(75f),
                        RaceRecolorizer.GetColorsFromRGB(100f)
                        )}), BodyPartsToRemove: new List<BodyPartType>() { BodyPartType.Eyes });

            var AmorphousLimbs = FeatureConfigurator.New("GanziAmorphousLimbs", "{36B31D24-9DFD-44BD-BA6F-D404C2C66D40}")
                .SetDisplayName(AmorphousLimbsDisplayName)
                .SetDescription(AmorphousLimbsDescription)
                .SetIcon(ItemWeaponRefs.Claw1d6.Reference.Get().Icon)
                .AddEmptyHandWeaponOverride(false, false, weapon: ItemWeaponRefs.Claw1d6.Reference.Get()).AddEmptyHandWeaponOverride(false, false, weapon: ItemWeaponRefs.Claw1d6.Reference.Get())
                //.AddFacts(["b09147e9b63b49b89c90361fbad90a68"]) // Shifter claws look dumb
                //.AddEquipmentEntity(RightAmorphousLimbsEE)
                //.AddEquipmentEntity(LeftAmorphousLimbsEE)
                .AddEquipmentEntity(AmorphousLimbsFurEE)
                .Configure();

            var MaleVestigialWings = FeatureConfigurator.New("GanziMaleVestigialWings", "{2C28ECB6-2A6A-4735-9BF3-0D202A9DA944}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("89343a8dc170abb46ae15c11775e867a")
                .Configure();

            var FemaleVestigialWings = FeatureConfigurator.New("GanziFemaleVestigialWings", "{29AF389C-9805-41CB-BA06-5FEF80490E5C}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity("96122c0bb0c483e46a53b8c9d05a1c39")
                .Configure();

            var VestigialWings = ProgressionConfigurator.New("GanziVestigialWings", "{1ABB191C-AA30-448F-AEA6-629ADCEDC1B2}")
                .SetDisplayName(VestigialWingsDisplayName)
                .SetDescription(VestigialWingsDescription)
                .SetIcon(FeatureRefs.FeralWings.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.UntypedStackable, stat: StatType.SaveReflex, value: 1)
                .AddStatBonus(ModifierDescriptor.UntypedStackable, stat: StatType.SkillMobility, value: 2)
                .AddToLevelEntries(1, FemaleVestigialWings, MaleVestigialWings)
                .Configure();

            var UncannyAuraEyesEE = RaceRecolorizer.RecolorEELink(ContactEEL, new List<Texture2D>() { RaceRecolorizer.GetArmorRampByIndex(13) }, "{179FDD12-AE6B-425A-ABE8-39A4AEC84688}", true, true);

            var UncannyAuraCooldownBuff = BuffConfigurator.New("GanziUncannyAuraCooldownBuff", "{3E882DBC-5B3D-4BE7-8AD6-9E109CF624F0}")
                .SetDisplayName(UncannyAuraDisplayName)
                .SetDescription(UncannyAuraDescription)
                .SetIcon(AbilityRefs.RemoveCurse.Reference.Get().Icon)
                .Configure();

            var actions2 = ActionsBuilder.New().Conditional(
                ConditionsBuilder.New().IsEnemy().HasBuff(UncannyAuraCooldownBuff, true).Build(),
                    ifTrue: ActionsBuilder.New().SavingThrow(SavingThrowType.Will).ConditionalSaved(
                        failed: ActionsBuilder.New().ApplyBuffWithDurationSeconds(BuffRefs.Frightened.Reference.Get(), 6f)
                            .ApplyBuff(UncannyAuraCooldownBuff, new ContextDurationValue()
                            {
                                Rate = DurationRate.Days,
                                DiceType = DiceType.Zero,
                                DiceCountValue = new ContextValue()
                                {
                                    ValueType = ContextValueType.Simple,
                                    Value = 0,
                                    ValueRank = AbilityRankType.Default,
                                    ValueShared = AbilitySharedValue.Damage,
                                    Property = UnitProperty.None
                                },
                                BonusValue = new ContextValue()
                                {
                                    ValueType = ContextValueType.Simple,
                                    Value = 1,
                                    ValueRank = AbilityRankType.Default,
                                    ValueShared = AbilitySharedValue.Damage,
                                    Property = UnitProperty.None
                                },
                                m_IsExtendable = false
                            }).Build(),
                        succeed: ActionsBuilder.New().ApplyBuff(UncannyAuraCooldownBuff, new ContextDurationValue()
                        {
                            Rate = DurationRate.Days,
                            DiceType = DiceType.Zero,
                            DiceCountValue = new ContextValue()
                            {
                                ValueType = ContextValueType.Simple,
                                Value = 0,
                                ValueRank = AbilityRankType.Default,
                                ValueShared = AbilitySharedValue.Damage,
                                Property = UnitProperty.None
                            },
                            BonusValue = new ContextValue()
                            {
                                ValueType = ContextValueType.Simple,
                                Value = 1,
                                ValueRank = AbilityRankType.Default,
                                ValueShared = AbilitySharedValue.Damage,
                                Property = UnitProperty.None
                            },
                            m_IsExtendable = false
                        }).Build())).Build();


            var UncannyAuraArea = AbilityAreaEffectConfigurator.New("GanziUncannyAuraArea", "{2C128A37-3DE6-4EED-8EF3-B185EEBA066C}")
                .AddAbilityAreaEffectRunAction(unitEnter: actions2)
                .SetSize(30.Feet())
                .SetShape(AreaEffectShape.Cylinder)
                .SetAffectEnemies(true)
                .SetAggroEnemies(true)
                .SetSpellResistance(false)
                .Configure();

            var UncannyAuraBuff = BuffConfigurator.New("GanziUncannyAuraBuff", "{664745A0-17AE-4C10-B42D-5B9DDCCB0F19}")
                .SetDisplayName(UncannyAuraDisplayName)
                .SetDescription(UncannyAuraDescription)
                .SetIcon(AbilityRefs.RemoveCurse.Reference.Get().Icon)
                .AddAreaEffect(UncannyAuraArea)
                .AddSpellDescriptorComponent(SpellDescriptor.Fear)
                .AddSpellDescriptorComponent(SpellDescriptor.MindAffecting)
                .Configure();

            var actions = ActionsBuilder.New().ApplyBuff(UncannyAuraBuff, durationValue: new ContextDurationValue()
            {
                Rate = DurationRate.Rounds,
                DiceType = DiceType.Zero,
                DiceCountValue = new ContextValue()
                {
                    ValueType = ContextValueType.Simple,
                    Value = 0,
                    ValueRank = AbilityRankType.Default,
                    ValueShared = AbilitySharedValue.Damage,
                    Property = UnitProperty.None
                },
                BonusValue = new ContextValue()
                {
                    ValueType = ContextValueType.Rank,
                    Value = 0,
                    ValueRank = AbilityRankType.Default,
                    ValueShared = AbilitySharedValue.Damage,
                    Property = UnitProperty.None
                },
                m_IsExtendable = false,
            }, false, isNotDispelable: true).Build();

            var UncannyAuraAbility = AbilityConfigurator.New("GanziUncannyAuraAbility", "{C72E93BB-F02B-4EF1-9177-3F8BE9AE0D39}")
                .SetDisplayName(UncannyAuraDisplayName)
                .SetDescription(UncannyAuraDescription)
                .SetIcon(AbilityRefs.RemoveCurse.Reference.Get().Icon)
                .AddAbilityEffectRunAction(actions)
                .SetRange(AbilityRange.Personal)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .AllowTargeting(false, false, false, true)
                .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.Charisma, min: 1))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .SetType(AbilityType.Supernatural)
                .Configure();
            
            var UncannyAura = FeatureConfigurator.New("GanziUncannyAuraFeature", "{35EABE3C-30EA-42A2-BDFC-33499A969077}")
                .SetDisplayName(UncannyAuraDisplayName)
                .SetDescription(UncannyAuraDescription)
                .SetIcon(AbilityRefs.RemoveCurse.Reference.Get().Icon)
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddFacts([UncannyAuraAbility])
                .AddEquipmentEntity(UncannyAuraEyesEE)
                .Configure();

            var Oddity = FeatureSelectionConfigurator.New("GanziOdditySelection", "{F96E9F98-28E7-440F-BED8-2C57060A8883}")
                .SetDisplayName(OddityDisplayName)
                .SetDescription(OddityDescription)
                .SetIcon(AbilityRefs.MagusSpellRecall.Reference.Get().Icon)
                .SetObligatory(true)
                .AddToAllFeatures(PrehensileTail, EntropicFlesh, WeaponPlay, SLA, RacingMind, AmorphousLimbs, VestigialWings, UncannyAura)
                .SetGroup(FeatureGroup.Racial)
                .Configure();
            
            var MaelstromResistance = FeatureConfigurator.New("GanziMaelstromResistance", "{ACB72A68-9C31-4909-A77A-74141070596F}")
                .SetDisplayName(MaelstromResistanceDisplayName)
                .SetDescription(MaelstromResistanceDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.ProtectionFromEnergy.ToString()).Icon)
                .AddDamageResistanceEnergy(type: DamageEnergyType.Electricity, value: 5)
                .AddDamageResistanceEnergy(type: DamageEnergyType.Acid, value: 5)
                .AddDamageResistanceEnergy(type: DamageEnergyType.Sonic, value: 5)
                .AddSavingThrowBonusAgainstDescriptor(2, modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.Polymorph)
                .Configure();

            var Skilled = FeatureConfigurator.New("GanziSkilled", "{06E961F8-4C64-4726-BE50-F4167B1A9C83}")
                .SetDisplayName(SkilledDisplayName)
                .SetDescription(SkilledDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HumanSkilled.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.CheckBluff, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillLoreNature, value: 2)
                .Configure();

            var race =
            RaceConfigurator.New(GanziName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(GanziDisplayName)
                .SetDescription(GanziDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(MaelstromResistance, Skilled, Oddity, FeatureRefs.KeenSenses.ToString())
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: -2)
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.KitsuneRace.ToString()), CustomFemaleHairs: FemaleHairs, CustomMaleHairs: MaleHairs, CustomHeadRamps: CustomSkinColors, CustomHairRamps: CustomHairColors, CustomEyeRamps: CustomEyeColors);

            // Add race to mount fixes
            RaceMountFixerizer.AddRaceToMountFixes(recoloredrace, CopyRace);
            //RaceMountFixerizer.AddRaceToMountFixes(race, CopyRace);

            // Add race to race list
            var raceRef = recoloredrace.ToReference<BlueprintRaceReference>();
            //var raceRef = race.ToReference<BlueprintRaceReference>();
            ref var races = ref BlueprintRoot.Instance.Progression.m_CharacterRaces;

            var length = races.Length;
            Array.Resize(ref races, length + 1);
            races[length] = raceRef;
        }
    }
}
