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
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Visual.CharacterSystem;

namespace EbonsContentMod.Races
{
    internal class Changeling
    {
        public static List<Color> RaceHeadColors =
        [
            new Color( // Near White
                RaceRecolorizer.GetColorsFromRGB(160f),
                RaceRecolorizer.GetColorsFromRGB(160f),
                RaceRecolorizer.GetColorsFromRGB(165f)
                ),
            new Color( // Light Skin
                RaceRecolorizer.GetColorsFromRGB(185f),
                RaceRecolorizer.GetColorsFromRGB(155f),
                RaceRecolorizer.GetColorsFromRGB(145f)
                ),
            new Color( // Ashen Skin Skin
                RaceRecolorizer.GetColorsFromRGB(145f),
                RaceRecolorizer.GetColorsFromRGB(125f),
                RaceRecolorizer.GetColorsFromRGB(120f)
                ),
            new Color( // Light Green Skin
                RaceRecolorizer.GetColorsFromRGB(145f),
                RaceRecolorizer.GetColorsFromRGB(155f),
                RaceRecolorizer.GetColorsFromRGB(145f)
                ),
            new Color( // Lavender
                RaceRecolorizer.GetColorsFromRGB(153f),
                RaceRecolorizer.GetColorsFromRGB(128f),
                RaceRecolorizer.GetColorsFromRGB(161f)
                ),
            new Color( // Very Lgiht Gray
                RaceRecolorizer.GetColorsFromRGB(130f),
                RaceRecolorizer.GetColorsFromRGB(130f),
                RaceRecolorizer.GetColorsFromRGB(135f)
                )
        ];

        public static List<Color> RaceEyeColors =
        [
            new Color( // Green
                RaceRecolorizer.GetColorsFromRGB(166f),
                RaceRecolorizer.GetColorsFromRGB(226f),
                RaceRecolorizer.GetColorsFromRGB(119f)
                ),
            new Color( // Light Blue
                RaceRecolorizer.GetColorsFromRGB(134f),
                RaceRecolorizer.GetColorsFromRGB(173f),
                RaceRecolorizer.GetColorsFromRGB(231f)
                ),
            new Color( // Blue
                RaceRecolorizer.GetColorsFromRGB(76f),
                RaceRecolorizer.GetColorsFromRGB(133f),
                RaceRecolorizer.GetColorsFromRGB(192f)
                ),
            new Color( // Pink/Fuscia
                RaceRecolorizer.GetColorsFromRGB(156f),
                RaceRecolorizer.GetColorsFromRGB(94f),
                RaceRecolorizer.GetColorsFromRGB(166f)
                ),
            new Color( // Purple
                RaceRecolorizer.GetColorsFromRGB(137f),
                RaceRecolorizer.GetColorsFromRGB(84f),
                RaceRecolorizer.GetColorsFromRGB(220f)
                ),
            new Color( // Light Red
                RaceRecolorizer.GetColorsFromRGB(190f),
                RaceRecolorizer.GetColorsFromRGB(85f),
                RaceRecolorizer.GetColorsFromRGB(85f)
                ),
            new Color( // Fiery Orange
                RaceRecolorizer.GetColorsFromRGB(235f),
                RaceRecolorizer.GetColorsFromRGB(119f),
                RaceRecolorizer.GetColorsFromRGB(38f)
                ),
            new Color( // Fiery Red
                RaceRecolorizer.GetColorsFromRGB(235f),
                RaceRecolorizer.GetColorsFromRGB(73f),
                RaceRecolorizer.GetColorsFromRGB(38f)
                ),
            new Color( // Light Yellow
                RaceRecolorizer.GetColorsFromRGB(248f),
                RaceRecolorizer.GetColorsFromRGB(226f),
                RaceRecolorizer.GetColorsFromRGB(174f)
                ),
            new Color( // White
                RaceRecolorizer.GetColorsFromRGB(200f),
                RaceRecolorizer.GetColorsFromRGB(200f),
                RaceRecolorizer.GetColorsFromRGB(200f)
                ),
            new Color( // Turqois
                RaceRecolorizer.GetColorsFromRGB(118f),
                RaceRecolorizer.GetColorsFromRGB(200f),
                RaceRecolorizer.GetColorsFromRGB(184f)
                ),
            new Color( // Blue-Green
                RaceRecolorizer.GetColorsFromRGB(0f),
                RaceRecolorizer.GetColorsFromRGB(100f),
                RaceRecolorizer.GetColorsFromRGB(260f)
                ),
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
            new Color( // Light Purple
                RaceRecolorizer.GetColorsFromRGB(190f),
                RaceRecolorizer.GetColorsFromRGB(150f),
                RaceRecolorizer.GetColorsFromRGB(205f)
                ),
            new Color( // Light Teal
                RaceRecolorizer.GetColorsFromRGB(143f),
                RaceRecolorizer.GetColorsFromRGB(220f),
                RaceRecolorizer.GetColorsFromRGB(203f)
                ),
            new Color( // Light Blue
                RaceRecolorizer.GetColorsFromRGB(144f),
                RaceRecolorizer.GetColorsFromRGB(160f),
                RaceRecolorizer.GetColorsFromRGB(236f)
                ),
            new Color( // Greenish Yellow
                RaceRecolorizer.GetColorsFromRGB(170f),
                RaceRecolorizer.GetColorsFromRGB(200f),
                RaceRecolorizer.GetColorsFromRGB(45f)
                )
        ];

        public static List<Color> RaceHairColors =
        [
            new Color( // Black
                RaceRecolorizer.GetColorsFromRGB(40f),
                RaceRecolorizer.GetColorsFromRGB(40f),
                RaceRecolorizer.GetColorsFromRGB(45f)
                ),
            new Color( // Dark Gray
                RaceRecolorizer.GetColorsFromRGB(70f),
                RaceRecolorizer.GetColorsFromRGB(70f),
                RaceRecolorizer.GetColorsFromRGB(75f)
                ),
            new Color( // Dark Blue
                RaceRecolorizer.GetColorsFromRGB(35f),
                RaceRecolorizer.GetColorsFromRGB(55f),
                RaceRecolorizer.GetColorsFromRGB(115f)
                ),
            new Color( // Purple
                RaceRecolorizer.GetColorsFromRGB(80f),
                RaceRecolorizer.GetColorsFromRGB(55f),
                RaceRecolorizer.GetColorsFromRGB(115f)
                ),
            new Color( // Dark Purple
                RaceRecolorizer.GetColorsFromRGB(50f),
                RaceRecolorizer.GetColorsFromRGB(30f),
                RaceRecolorizer.GetColorsFromRGB(75f)
                ),
            new Color( // Nearly Black Purple
                RaceRecolorizer.GetColorsFromRGB(30f),
                RaceRecolorizer.GetColorsFromRGB(15f),
                RaceRecolorizer.GetColorsFromRGB(45f)
                ),
            new Color( // Very Dark Brown
                RaceRecolorizer.GetColorsFromRGB(35f),
                RaceRecolorizer.GetColorsFromRGB(25f),
                RaceRecolorizer.GetColorsFromRGB(10f)
                ),
            new Color( // Very Lgiht Gray
                RaceRecolorizer.GetColorsFromRGB(130f),
                RaceRecolorizer.GetColorsFromRGB(130f),
                RaceRecolorizer.GetColorsFromRGB(135f)
                ),
            new Color( // Light Blue
                RaceRecolorizer.GetColorsFromRGB(125f),
                RaceRecolorizer.GetColorsFromRGB(150f),
                RaceRecolorizer.GetColorsFromRGB(190f)
                ),
            new Color( // Gray
                RaceRecolorizer.GetColorsFromRGB(100f),
                RaceRecolorizer.GetColorsFromRGB(100f),
                RaceRecolorizer.GetColorsFromRGB(105f)
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

        // Good amount of clipping in all these, but oh well
        public static EquipmentEntityLink[] FemaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "57c595a0dece66f4283e888dc52d9df1"}, // Long Female Elf
            new EquipmentEntityLink() {AssetId = "c8edacbc502d42242a5911ba000a411e"}, // Long Wavy Braids Female Half-Elf
            new EquipmentEntityLink() {AssetId = "db566d919de425443bd9ae0a37de3ec9"}, // French Braid Female Elf
            new EquipmentEntityLink() {AssetId = "de99a284a347d35438251f35fd12d63b"}, // Long Wavy Female Half-Elf
            new EquipmentEntityLink() {AssetId = "ab19b4ef03602ed439eb87552e0ca67b"}, // Ponytail Classic Female Half-Elf
            new EquipmentEntityLink() {AssetId = "21099c170f7b8344d90d6f034fb5554c"}, // Pompadour Female Elf
            new EquipmentEntityLink() {AssetId = "9ec441743ea20c5488e7f497992042ed"}, // Ponytail Lush Female Elf
            new EquipmentEntityLink() {AssetId = "304b84351ffbde24190e56724178df5b"}, // Long Ember Female Elf
            new EquipmentEntityLink() {AssetId = "e557a6ffddb60c04191dee26d69f3c01"}, // Slick Female Half-Elf
            new EquipmentEntityLink() {AssetId = "a5efed3983dd97342a04e8335b9a8bdc"}, // Medium Anevia Female Half-Elf
            new EquipmentEntityLink() {AssetId = "19411a87224e19540beaa6ef2d4ec8dd"}, // Side Kare Female Half-Elf
            new EquipmentEntityLink() {AssetId = "918f52fa9e49f6d439c70abc652f89f7"}, // Long Curly Female Half-Elf
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"}  // Bald
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HalfElfRace.ToString());

        private static readonly string ChangelingName = "ChangelingRace";

        internal const string ChangelingDisplayName = "Changeling.Name";
        private static readonly string ChangelingDescription = "Changeling.Description";
        public static readonly string RaceGuid = "{BC9B8D87-9D10-4455-895C-98E31F8D8503}";

        internal const string WitchBornDisplayName = "Changeling.WitchBorn.Name";
        private static readonly string WitchBornDescription = "Changeling.WitchBorn.Description";

        internal const string ChangelingGeneralDisplayName = "Changeling.ChangelingGeneral.Name";
        private static readonly string ChangelingGeneralDescription = "Changeling.ChangelingGeneral.Description";

        internal const string HagHeritageDisplayName = "Changeling.HagHeritage.Name";
        private static readonly string HagHeritageDescription = "Changeling.HagHeritage.Description";

        internal const string ClawsDisplayName = "Changeling.Claws.Name";
        private static readonly string ClawsDescription = "Changeling.Claws.Description";

        internal const string NaturalArmorDisplayName = "Changeling.NaturalArmor.Name";
        private static readonly string NaturalArmorDescription = "Changeling.NaturalArmor.Description";

        internal const string HulkingChangelingDisplayName = "Changeling.HulkingChangeling.Name";
        private static readonly string HulkingChangelingDescription = "Changeling.HulkingChangeling.Description";

        internal const string PyrophileDisplayName = "Changeling.Pyrophile.Name";
        private static readonly string PyrophileDescription = "Changeling.Pyrophile.Description";

        internal const string GreenWidowDisplayName = "Changeling.GreenWidow.Name";
        private static readonly string GreenWidowDescription = "Changeling.GreenWidow.Description";

        internal const string GazeBlindnessDisplayName = "Changeling.GazeBlindness.Name";
        private static readonly string GazeBlindnessDescription = "Changeling.GazeBlindness.Description";

        internal const string HeartstoneHeritorDisplayName = "Changeling.HeartstoneHeritor.Name";
        private static readonly string HeartstoneHeritorDescription = "Changeling.HeartstoneHeritor.Description";

        internal const string HagRacialTraitDisplayName = "Changeling.HagRacialTrait.Name";
        private static readonly string HagRacialTraitDescription = "Changeling.HagRacialTrait.Description";

        internal static void Configure()
        {
            var HalfElfFemaleBody = new EquipmentEntityLink() { AssetId = "861171cdd3930a84faab08ab85ba924a" };
            var HalfElfMaleBody = new EquipmentEntityLink() { AssetId = "39763b45a3c0ff94ea6acbba28024168" };
            var MonoContact = RaceRecolorizer.SwitchEEEyeTexture(new EquipmentEntityLink() { AssetId = "a47bac4deb099fc4b86a2e01bb425cc5" }, "{861AF139-E068-40F7-B4F9-48FBBA297ACF}", eyeEE: new EquipmentEntityLink() { AssetId = "86127616283ae7741ae3e813904865cc" });
            var FemaleClawsEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "0fe6ca359f6292540a5430327647dc01" }, RaceRecolorizer.CreateRampsFromColorsSimple(RaceHeadColors), "{A3156602-36D4-4FCC-9CC2-D865774C6C1E}",
                BodyPartsToRemove: [BodyPartType.Torso, BodyPartType.UpperArms, BodyPartType.Forearms, BodyPartType.UpperLegs, BodyPartType.LowerLegs, BodyPartType.Spaulders, BodyPartType.Skirt, BodyPartType.Cuffs, BodyPartType.Belt, BodyPartType.BeltBag, BodyPartType.Feet], SetLayer: 209,
                HandCopyEE: HalfElfFemaleBody);
            var MaleClawsEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "3c638ac1505198b4fa587f04ca655718" }, RaceRecolorizer.CreateRampsFromColorsSimple(RaceHeadColors), "{55224D54-F61C-4F8D-9EA0-A47B6206FF31}",
                BodyPartsToRemove: [BodyPartType.Torso, BodyPartType.UpperArms, BodyPartType.Forearms, BodyPartType.UpperLegs, BodyPartType.LowerLegs, BodyPartType.Spaulders, BodyPartType.Skirt, BodyPartType.Cuffs, BodyPartType.Belt, BodyPartType.BeltBag, BodyPartType.Feet], SetLayer: 209,
                HandCopyEE: HalfElfMaleBody);

            var ClawRef = ItemWeaponRefs.Claw1d4.Reference.Get();
            
            var NaturalArmor = FeatureConfigurator.New("ChangelingNaturalArmor", "{C46FDAB4-87F9-4481-8D4A-4AFFFFD74200}")
                .SetDisplayName(NaturalArmorDisplayName)
                .SetDescription(NaturalArmorDescription)
                .SetIcon(FeatureRefs.InvulnerableDefensesShifterFeature.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.NaturalArmor, stat: StatType.AC, value: 1)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var MaleClaws = FeatureConfigurator.New("MaleChangelingClaws", "{F75349F5-15CC-4F95-BE30-CE485A13F44A}")
                .SetDisplayName(ClawsDisplayName)
                .SetDescription(ClawsDescription)
                .SetIcon(FeatureRefs.HagRivenClawsFeatureLevel1.Reference.Get().Icon)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity(MaleClawsEE)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var FemaleClaws = FeatureConfigurator.New("FemaleChangelingClaws", "{E6FEB67C-3F0C-4D2E-ADDA-876165387BBD}")
                .SetDisplayName(ClawsDisplayName)
                .SetDescription(ClawsDescription)
                .SetIcon(FeatureRefs.HagRivenClawsFeatureLevel1.Reference.Get().Icon)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity(FemaleClawsEE)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var Claws = ProgressionConfigurator.New("ChangelingClaws", "{CB631D55-9A5E-451D-BC81-1D053C7A8CDF}")
                .SetDisplayName(ClawsDisplayName)
                .SetDescription(ClawsDescription)
                .SetIcon(FeatureRefs.HagRivenClawsFeatureLevel1.Reference.Get().Icon)
                .AddEmptyHandWeaponOverride(false, false, weapon: ClawRef).AddEmptyHandWeaponOverride(false, false, weapon: ClawRef)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddToLevelEntries(1, MaleClaws, FemaleClaws)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            // Hag Heritage
            var WitchBorn = FeatureConfigurator.New("ChangelingWitchBorn", "{4609F5F1-5168-4E5D-BE96-9479518A57E6}")
                .SetDisplayName(WitchBornDisplayName)
                .SetDescription(WitchBornDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: -2)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var ChangelingGeneral = FeatureConfigurator.New("ChangelingGeneral", "{0EBC1384-1A22-4E38-8144-BDE1FF481358}")
                .SetDisplayName(ChangelingGeneralDisplayName)
                .SetDescription(ChangelingGeneralDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: -2)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var HagHeritageSelection = FeatureSelectionConfigurator.New("ChngelingHagHeritageSelection", "{FEBF7D59-84F8-41F0-B1FE-F5D31BBE009C}")
                .SetDisplayName(HagHeritageDisplayName)
                .SetDescription(HagHeritageDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(FeatureSelectionRefs.HalfOrcHeritageSelection.ToString()).Icon)
                .AddToAllFeatures(ChangelingGeneral, WitchBorn)
                .SetGroup(FeatureGroup.Racial)
                .Configure();

            // Hag Racial Trait
            var HulkingChangeling = FeatureConfigurator.New("ChangelingHulkingChangeling", "{26216E90-A3C8-42F3-A1B2-1885B21BB67D}")
                .SetDisplayName(HulkingChangelingDisplayName)
                .SetDescription(HulkingChangelingDescription)
                .SetIcon(FeatureRefs.HagboundWitchHunchedMusclesFeature.Reference.Get().Icon) // check
                .AddWeaponParametersDamageBonus(ranged: false)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var Pyrophile = FeatureConfigurator.New("ChangelingPyrophile", "{0086F0FC-0F0E-45FE-ABDF-046F87AB9799}")
                .SetDisplayName(PyrophileDisplayName)
                .SetDescription(PyrophileDescription)
                .SetIcon(FeatureRefs.FiresFuryFeature.Reference.Get().Icon)
                .AddComponent<PyrophileDamageBonus>(c =>
                {
                    c.SpellDescriptor = SpellDescriptor.Fire;
                    c.SpellsOnly = true;
                })
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel().WithStartPlusDivStepProgression(4))
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var GreenWidow = FeatureConfigurator.New("ChangelingGreenWidow", "{73F7D34A-4CDA-4E81-9DA1-2CEF29BA61F4}")
                .SetDisplayName(GreenWidowDisplayName)
                .SetDescription(GreenWidowDescription)
                .SetIcon(FeatureRefs.CurdleThoughtsFeature.Reference.Get().Icon) // check
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.CheckBluff, value: 2)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var GazeBlindness = FeatureConfigurator.New("ChangelingGazeBlindness", "{250330A5-83F2-492A-93CF-E966C1BC1853}")
                .SetDisplayName(GazeBlindnessDisplayName)
                .SetDescription(GazeBlindnessDescription)
                .SetIcon(AbilityRefs.Blindness.Reference.Get().Icon) // change
                .AddSavingThrowBonusAgainstDescriptor(2, modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.GazeAttack)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var HeartstoneHeritor = FeatureConfigurator.New("ChangelingHeartstoneHeritor", "{F281CDC1-F6C0-4052-A42A-559D0E8D21F1}")
                .SetDisplayName(HeartstoneHeritorDisplayName)
                .SetDescription(HeartstoneHeritorDescription)
                .SetIcon(FeatureRefs.PurityOfBody.Reference.Get().Icon) // change
                .AddSavingThrowBonusAgainstDescriptor(2, modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.Disease)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var HagRacialTrait = FeatureSelectionConfigurator.New("ChngelingHagRacialTraitSelection", "{B3F25336-EA68-42E9-8C4D-4ADDCE8957E1}")
                .SetDisplayName(HagRacialTraitDisplayName)
                .SetDescription(HagRacialTraitDescription)
                .SetIcon(FeatureRefs.HagCronyFeature.Reference.Get().Icon)
                .AddToAllFeatures(HulkingChangeling, Pyrophile, GreenWidow, GazeBlindness, HeartstoneHeritor)
                .SetGroup(FeatureGroup.Racial)
                .Configure();

            var race =
            RaceConfigurator.New(ChangelingName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(ChangelingDisplayName)
                .SetDescription(ChangelingDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(HagHeritageSelection, HagRacialTrait, NaturalArmor, Claws, FeatureRefs.KeenSenses.ToString())
                .AddEquipmentEntity(MonoContact)
                .SetRaceId(Race.HalfElf)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, eyecolors: RaceEyeColors, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DhampirRace.ToString()), CustomFemaleHairs: FemaleHairs/*, CustomMaleHairs: MaleHairs*/);

            // Add race to mount fixes
            RaceMountFixerizer.AddRaceToMountFixes(recoloredrace, CopyRace);

            // Register linked EEs
            EquipmentEntityLink[] SkinLinks = [FemaleClawsEE, MaleClawsEE];

            EELinker.RegisterSkinLink(recoloredrace, SkinLinks);

            // Add race to race list
            var raceRef = recoloredrace.ToReference<BlueprintRaceReference>();
            ref var races = ref BlueprintRoot.Instance.Progression.m_CharacterRaces;

            var length = races.Length;
            Array.Resize(ref races, length + 1);
            races[length] = raceRef;
        }
    }
}
