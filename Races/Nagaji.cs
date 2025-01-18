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
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Mechanics.Components;
using BlueprintCore.Utils.Types;
using System.Linq;
using Kingmaker.Visual.CharacterSystem;

namespace EbonsContentMod.Races
{
    internal class Nagaji
    {
        public static List<Color> RaceHeadColors = [];

        public static List<Color> RaceEyeColors = [];

        public static List<Color> RaceHairColors =
        [
            new Color( // Black
                RaceRecolorizer.GetColorsFromRGB(30f),
                RaceRecolorizer.GetColorsFromRGB(30f),
                RaceRecolorizer.GetColorsFromRGB(35f)
                )
        ];

        public static EquipmentEntityLink[] MaleHeads =
        [
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).MaleOptions.Heads[0],
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).MaleOptions.Heads[3]
        ];

        public static EquipmentEntityLink[] FemaleHeads =
        [
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).FemaleOptions.Heads[0],
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).FemaleOptions.Heads[3]
        ];

        public static EquipmentEntityLink[] MaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"}  // Bald
        ];

        // Good amount of clipping in all these, but oh well
        public static EquipmentEntityLink[] FemaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"}  // Bald
        ];

        public static List<Texture2D> CustomEyeRamps =
        [
            RaceRecolorizer.GetArmorRampByIndex(25),
            RaceRecolorizer.GetArmorRampByIndex(26),
            RaceRecolorizer.GetArmorRampByIndex(31),
            RaceRecolorizer.GetArmorRampByIndex(32),
            RaceRecolorizer.GetArmorRampByIndex(33),
            RaceRecolorizer.GetArmorRampByIndex(38),
            RaceRecolorizer.GetArmorRampByIndex(39),
            RaceRecolorizer.GetArmorRampByIndex(40),
            RaceRecolorizer.GetArmorRampByIndex(7),
            RaceRecolorizer.GetArmorRampByIndex(73),
            RaceRecolorizer.GetArmorRampByIndex(74),
            RaceRecolorizer.GetArmorRampByIndex(17),
            RaceRecolorizer.GetArmorRampByIndex(18),
            RaceRecolorizer.GetArmorRampByIndex(19)
        ];

        public static List<Texture2D> CustomSkinRamps =
        [
            RaceRecolorizer.GetArmorRampByIndex(28),
            RaceRecolorizer.GetArmorRampByIndex(36),
            RaceRecolorizer.GetArmorRampByIndex(37),
            RaceRecolorizer.GetArmorRampByIndex(27),
            RaceRecolorizer.GetArmorRampByIndex(29),
            RaceRecolorizer.GetArmorRampByIndex(30),
            RaceRecolorizer.GetArmorRampByIndex(74),
            RaceRecolorizer.GetArmorRampByIndex(75),
            RaceRecolorizer.GetArmorRampByIndex(76),
            RaceRecolorizer.GetArmorRampByIndex(77),
            RaceRecolorizer.GetArmorRampByIndex(78),
            RaceRecolorizer.GetArmorRampByIndex(79)
        ];

        public static List<BodyPartType> HeadPartsToRemove =
        [
            BodyPartType.Ears,
        ];

        public static List<BodyPartType> SkinPartsToRemove =
        [
            BodyPartType.Eyes,
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string NagajiName = "NagajiRace";

        internal const string NagajiDisplayName = "Nagaji.Name";
        private static readonly string NagajiDescription = "Nagaji.Description";
        public static readonly string RaceGuid = "{4E6E156B-707F-47C5-993A-C9262CA19A56}";

        internal const string ArmoredScalesDisplayName = "Nagaji.ArmoredScales.Name";
        private static readonly string ArmoredScalesDescription = "Nagaji.ArmoredScales.Description";

        internal const string ResistantDisplayName = "Nagaji.Resistant.Name";
        private static readonly string ResistantDescription = "Nagaji.Resistant.Description";

        internal const string SerpentSenseDisplayName = "Nagaji.SerpentSense.Name";
        private static readonly string SerpentSenseDescription = "Nagaji.SerpentSense.Description";

        internal static void Configure()
        {
            var LizardSkin1 = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "86127616283ae7741ae3e813904865cc" }, CustomSkinRamps, "{314B3133-8593-4731-9783-C796E01BAA1C}", true, BodyPartsToRemove: SkinPartsToRemove);
            var LizardSkin2 = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "7109791d63944254589b908564604c79" }, CustomSkinRamps, "{98F0E9D3-E5B2-45B1-8902-F9F558B12FCB}", true, BodyPartsToRemove: SkinPartsToRemove);

            List<BlueprintCharacterClassReference> AllClasses = [];

            EquipmentEntityLink[] SkinLinks = [LizardSkin1, LizardSkin2];

            var ArmoredScales = FeatureConfigurator.New("NagajiArmoredScales", "{8D9CB4EE-7A05-44AB-9D0B-6A20AE51B2C8}")
                .SetDisplayName(ArmoredScalesDisplayName)
                .SetDescription(ArmoredScalesDescription)
                .SetIcon(FeatureRefs.InvulnerableDefensesShifterFeature.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.NaturalArmor, stat: StatType.AC, value: 1)
                .Configure();

            var Resistant = FeatureConfigurator.New("NagajiResistant", "{DDB7A933-6546-4865-BD0F-FFFA755556D7}")
                .SetDisplayName(ResistantDisplayName)
                .SetDescription(ResistantDescription)
                .SetIcon(FeatureRefs.ResistNaturesLure.Reference.Get().Icon)
                .AddSavingThrowBonusAgainstDescriptor(2, modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.MindAffecting)
                .AddSavingThrowBonusAgainstDescriptor(2, modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.Poison)
                .Configure();

            var SerpentSense = FeatureConfigurator.New("SerpentSenseNagaji", "{9179643D-AFCC-40EE-91EF-42114B10C434}")
                .SetDisplayName(SerpentSenseDisplayName)
                .SetDescription(SerpentSenseDescription)
                .SetIcon(AbilityRefs.Poison.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillPerception, value: 4)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillLoreNature, value: 2)
                .Configure();

            var race =
            RaceConfigurator.New(NagajiName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(NagajiDisplayName)
                .SetDescription(NagajiDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(ArmoredScales, Resistant, SerpentSense)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: -2)
                .SetRaceId(Race.Human)
                .AddEquipmentEntity(LizardSkin1)
                .AddEquipmentEntity(LizardSkin2)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, BaldRace: true, NoEyebrows: true, NoBeards: true, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()), CustomEyeRamps: CustomEyeRamps, CustomMaleHeads: MaleHeads, CustomFemaleHeads: FemaleHeads, CustomHeadRamps: CustomSkinRamps, RemoveHeadParts: HeadPartsToRemove);

            // Register linked EEs
            EELinker.RegisterSkinLink(recoloredrace, SkinLinks);

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
