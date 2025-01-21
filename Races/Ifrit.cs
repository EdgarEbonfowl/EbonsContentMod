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

namespace EbonsContentMod.Races
{
    internal class Ifrit
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
            new Color( // Bright Yellow Orange
                RaceRecolorizer.GetColorsFromRGB(255f),
                RaceRecolorizer.GetColorsFromRGB(165f),
                RaceRecolorizer.GetColorsFromRGB(0f)
                ),
            new Color( // Bright Orange
                RaceRecolorizer.GetColorsFromRGB(255f),
                RaceRecolorizer.GetColorsFromRGB(125f),
                RaceRecolorizer.GetColorsFromRGB(0f)
                ),
            new Color( // Fiery Red
                RaceRecolorizer.GetColorsFromRGB(235f),
                RaceRecolorizer.GetColorsFromRGB(73f),
                RaceRecolorizer.GetColorsFromRGB(38f)
                ),
            new Color( // Bright Red Orange
                RaceRecolorizer.GetColorsFromRGB(255f),
                RaceRecolorizer.GetColorsFromRGB(90f),
                RaceRecolorizer.GetColorsFromRGB(0f)
                ),
            new Color( // Bright Yellow
                RaceRecolorizer.GetColorsFromRGB(255f),
                RaceRecolorizer.GetColorsFromRGB(215f),
                RaceRecolorizer.GetColorsFromRGB(0f)
                ),
        ];

        public static List<Color> RaceHairColors =
        [
            new Color( // Fiery Orange
                RaceRecolorizer.GetColorsFromRGB(235f),
                RaceRecolorizer.GetColorsFromRGB(119f),
                RaceRecolorizer.GetColorsFromRGB(38f)
                ),
            new Color( // Bright Yellow Orange
                RaceRecolorizer.GetColorsFromRGB(255f),
                RaceRecolorizer.GetColorsFromRGB(165f),
                RaceRecolorizer.GetColorsFromRGB(0f)
                ),
            new Color( // Bright Orange
                RaceRecolorizer.GetColorsFromRGB(255f),
                RaceRecolorizer.GetColorsFromRGB(125f),
                RaceRecolorizer.GetColorsFromRGB(0f)
                ),
            new Color( // Fiery Red
                RaceRecolorizer.GetColorsFromRGB(235f),
                RaceRecolorizer.GetColorsFromRGB(73f),
                RaceRecolorizer.GetColorsFromRGB(38f)
                ),
            new Color( // Bright Red Orange
                RaceRecolorizer.GetColorsFromRGB(255f),
                RaceRecolorizer.GetColorsFromRGB(90f),
                RaceRecolorizer.GetColorsFromRGB(0f)
                ),
            new Color( // Bright Yellow
                RaceRecolorizer.GetColorsFromRGB(255f),
                RaceRecolorizer.GetColorsFromRGB(215f),
                RaceRecolorizer.GetColorsFromRGB(0f)
                ),
        ];

        public static EquipmentEntityLink[] MaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "d0d147e4a9ff9c345b336501018c17b2"}, // Aasimar Medium Bun
            new EquipmentEntityLink() {AssetId = "76cad0a86acd5ce4d925992877bcd4fd"}, // Aasimar Slick
            new EquipmentEntityLink() {AssetId = "3040534437e729945a629892a687e578"}, // Aasimar Trim
            new EquipmentEntityLink() {AssetId = "b34b1532310a01440ac95dea9d0956e1"}, // Aasimar Ponytail Classic
            new EquipmentEntityLink() {AssetId = "6f7020d7788477d498233b40095320ff"}, // Aasimar Medium Mess
            new EquipmentEntityLink() {AssetId = "82378e98f50571f40a27188815b12973"}, // Aasimar Daeran
            new EquipmentEntityLink() {AssetId = "4049f905c9dc7714793c654a577774b4"}, // Aasimar Military
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"}  // Bald
        ];

        // Good amount of clipping in all these, but oh well
        public static EquipmentEntityLink[] FemaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "62dfe086393d26943b7e756fb2569247"}, // Aasimar Long Wavy Braids
            new EquipmentEntityLink() {AssetId = "7522188768d3bcc4f9691eb3d0e68cfc"}, // Aasimar Long Back
            new EquipmentEntityLink() {AssetId = "e3a41513e3314e047ae3bbb41a00e7e4"}, // Aasimar Medium Anevia
            new EquipmentEntityLink() {AssetId = "5ad5704dd51861a4b80931cd4d33ebe6"}, // Aasimar Slick
            new EquipmentEntityLink() {AssetId = "b6ed73aa8db434a48afab56b6296181d"}, // Aasimar Long Camelia
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"}  // Bald
        ];

        public static List<Texture2D> TieflingSkinRamps = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).MaleOptions.Heads[0].Load(true, false).PrimaryRamps;


        public static List<Texture2D> CustomSkinRamps =
        [
            RaceRecolorizer.GetArmorRampByIndex(3),
            RaceRecolorizer.GetArmorRampByIndex(1),
            RaceRecolorizer.GetArmorRampByIndex(4),
            RaceRecolorizer.GetArmorRampByIndex(5),
            RaceRecolorizer.GetArmorRampByIndex(9),
            TieflingSkinRamps[11],
            RaceRecolorizer.GetArmorRampByIndex(13),
            RaceRecolorizer.GetArmorRampByIndex(14),
            RaceRecolorizer.GetArmorRampByIndex(15),
            RaceRecolorizer.GetArmorRampByIndex(16),
            RaceRecolorizer.GetArmorRampByIndex(22),
            RaceRecolorizer.GetArmorRampByIndex(23),
            RaceRecolorizer.GetArmorRampByIndex(77),
            RaceRecolorizer.GetArmorRampByIndex(78),
            RaceRecolorizer.GetArmorRampByIndex(79)
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string IfritName = "IfritRace";

        internal const string IfritDisplayName = "Ifrit.Name";
        private static readonly string IfritDescription = "Ifrit.Description";
        public static readonly string RaceGuid = "{87E8AA20-F969-479F-A73B-559C6912FC74}";

        internal const string FireAffinityDisplayName = "Ifrit.FireAffinity.Name";
        private static readonly string FireAffinityDescription = "Ifrit.FireAffinity.Description";

        internal const string IfritEnergyResistanceDisplayName = "Ifrit.EnergyResistance.Name";
        private static readonly string IfritEnergyResistanceDescription = "Ifrit.EnergyResistance.Description";

        internal const string ElementalHeritageDisplayName = "Ifrit.ElementalHeritage.Name";
        private static readonly string ElementalHeritageDescription = "Ifrit.ElementalHeritage.Description";

        internal const string IfritGeneralDisplayName = "Ifrit.IfritGeneral.Name";
        private static readonly string IfritGeneralDescription = "Ifrit.IfritGeneral.Description";

        internal const string LavasoulDisplayName = "Ifrit.Lavasoul.Name";
        private static readonly string LavasoulDescription = "Ifrit.Lavasoul.Description";

        internal const string SunsoulDisplayName = "Ifrit.Sunsoul.Name";
        private static readonly string SunsoulDescription = "Ifrit.Sunsoul.Description";

        internal static void Configure()
        {
            //var MaleHornsEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "cecad00f0d5d83f4ba37aa45c11c1bbe" }, RaceRecolorizer.CreateRampsFromColorsSimple(RaceHeadColors), "{AD94B25F-00F8-41D7-9D1B-5CD56015844A}", true);
            //var FemaleHornsEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "ccd35c8508752f04582e7d3a55248afe" }, RaceRecolorizer.CreateRampsFromColorsSimple(RaceHeadColors), "{3206E92A-6AE6-4394-A313-C06E1F666A70}", true);

            var MaleHornsEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "cecad00f0d5d83f4ba37aa45c11c1bbe" }, CustomSkinRamps, "{AD94B25F-00F8-41D7-9D1B-5CD56015844A}", true);
            var FemaleHornsEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "ccd35c8508752f04582e7d3a55248afe" }, CustomSkinRamps, "{3206E92A-6AE6-4394-A313-C06E1F666A70}", true);

            List<BlueprintCharacterClassReference> AllClasses = [];

            foreach (var item in CharacterClassRefs.All)
            {
                var itemref = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>(item.ToString());
                AllClasses.Add(itemref);
            }

            EquipmentEntityLink[] HornsLinks = [MaleHornsEE, FemaleHornsEE];

            var MaleIfritHorns = FeatureConfigurator.New("MaleIfritHorns", "{4D10C551-11D4-46B8-88A0-004791433047}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Male;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity(MaleHornsEE)
                .Configure();

            var FemaleIfritHorns = FeatureConfigurator.New("FemaleIfritHorns", "{85255FD2-3D85-457D-AF63-EF51CFDF117C}")
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .AddComponent<PrerequisiteSex>(c =>
                {
                    c.gender = Gender.Female;
                    c.CheckInProgression = true;
                })
                .AddEquipmentEntity(FemaleHornsEE)
                .Configure();

            var FireAffinity = ProgressionConfigurator.New("IfritFireAffinity", "{25A52335-E1D0-4A21-9F8C-4DE52C8E8970}")
                .SetDisplayName(FireAffinityDisplayName)
                .SetDescription(FireAffinityDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.FiresFuryFeature.ToString()).Icon)
                .AddIncreaseSpellDescriptorCasterLevel(1, SpellDescriptor.Fire, modifierDescriptor: ModifierDescriptor.Racial)
                .AddIncreaseSpellDescriptorDC(1, SpellDescriptor.Fire, modifierDescriptor: ModifierDescriptor.Racial)
                .SetGiveFeaturesForPreviousLevels(true)
                .AddToLevelEntries(1, MaleIfritHorns, FemaleIfritHorns)
                .Configure();

            var EnergyResistance = FeatureConfigurator.New("IfritEnergyResistance", "{2AEECCDD-CA8B-4AC6-B8A9-C00B08E7F75F}")
                .SetDisplayName(IfritEnergyResistanceDisplayName)
                .SetDescription(IfritEnergyResistanceDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.BloodlineDraconicRedResistancesAbilityLevel1.ToString()).Icon)
                .AddDamageResistanceEnergy(type: DamageEnergyType.Fire, value: 5)
                .Configure();

            var SpellLikeResource = AbilityResourceConfigurator.New("IfritSpellLikeResource", "{5CB64827-9781-4C59-A030-4219BC7E5197}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var BurningHands = AbilityConfigurator.New("IfritBurningHands", "{5D81DBB2-5752-4CDB-9007-8CD385DEC889}")
                .CopyFrom(AbilityRefs.BurningHands, c => c is not (SpellListComponent or CraftInfoComponent or ContextRankConfig))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel())
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 1)
                .Configure();

            var FlareBurst = AbilityConfigurator.New("IfritFlareBurst", "{AA12F380-A66E-40D6-8E31-3D5F8EDF9BF3}")
                .CopyFrom(AbilityRefs.FlareBurst, c => c is not (SpellListComponent or CraftInfoComponent or ContextRankConfig))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel())
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 1)
                .Configure();

            var FireBelly = AbilityConfigurator.New("IfritFirebelly", "{E7EB31D3-B016-4056-BE8E-F5CEA8F4CBEF}")
                .CopyFrom(AbilityRefs.FirebellyAbility, c => c is not (SpellListComponent or CraftInfoComponent or ContextRankConfig))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel())
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 1)
                .Configure();

            // Elemental Heritage

            var Lavasoul = FeatureConfigurator.New("IfritLavasoul", "{59BA37D5-D73F-4FE4-8FEA-77BA93C809FE}")
                .SetDisplayName(LavasoulDisplayName)
                .SetDescription(LavasoulDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: -2)
                .AddFacts([FireBelly])
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: FireBelly)
                .Configure();

            var Sunsoul = FeatureConfigurator.New("IfritSunsoul", "{0F452365-699D-48D1-8D5A-5402353D9F88}")
                .SetDisplayName(SunsoulDisplayName)
                .SetDescription(SunsoulDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: -2)
                .AddFacts([FlareBurst])
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: FlareBurst)
                .Configure();

            var IfritGeneral = FeatureConfigurator.New("IfritGeneral", "{FB5A6297-42CE-4EBA-973A-02FFD98B1ACE}")
                .SetDisplayName(IfritGeneralDisplayName)
                .SetDescription(IfritGeneralDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: -2)
                .AddFacts([BurningHands])
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: BurningHands)
                .Configure();

            var IfritElementalHeritageSelection = FeatureSelectionConfigurator.New("IfritElementalHeritageSelection", "{5AE79B6B-57C3-42A0-8714-829A5299825F}")
                .SetDisplayName(ElementalHeritageDisplayName)
                .SetDescription(ElementalHeritageDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(FeatureSelectionRefs.HalfOrcHeritageSelection.ToString()).Icon)
                .AddToAllFeatures(IfritGeneral, Lavasoul, Sunsoul)
                .SetGroup(FeatureGroup.Racial)
                .Configure();

            var race =
            RaceConfigurator.New(IfritName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(IfritDisplayName)
                .SetDescription(IfritDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(IfritElementalHeritageSelection, EnergyResistance, FireAffinity, FeatureRefs.KeenSenses.ToString())
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, eyecolors: RaceEyeColors, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.OreadRace.ToString()), CustomFemaleHairs: FemaleHairs, CustomMaleHairs: MaleHairs, CustomHeadRamps: CustomSkinRamps);

            // Register linked EEs
            EELinker.RegisterSkinLink(recoloredrace, HornsLinks);

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
