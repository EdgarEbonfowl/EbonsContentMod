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
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Enums.Damage;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using EbonsContentMod.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using static TabletopTweaks.Core.MechanicsChanges.MetamagicExtention;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Craft;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using static EbonsContentMod.UnitParts.UnitPartTouchCharges;
using static Kingmaker.Kingdom.Settlements.SettlementGridTopology;
using Kingmaker.Utility;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using EbonsContentMod.Components;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace EbonsContentMod.Races
{
    internal class Naiad
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
            ColorRampGenerator.CreateHumanSkinRamp(new Color( // Seafoam
                RaceRecolorizer.GetColorsFromRGB(147f),
                RaceRecolorizer.GetColorsFromRGB(212f),
                RaceRecolorizer.GetColorsFromRGB(195f)
                ),
                new Color( // Light Seafoam
                RaceRecolorizer.GetColorsFromRGB(147f * 3/5),
                RaceRecolorizer.GetColorsFromRGB(212f * 3/5),
                RaceRecolorizer.GetColorsFromRGB(195f * 3/5)
                )),
            ColorRampGenerator.CreateHumanSkinRamp(new Color( // Light Seafoam
                RaceRecolorizer.GetColorsFromRGB(134f),
                RaceRecolorizer.GetColorsFromRGB(182f),
                RaceRecolorizer.GetColorsFromRGB(164f)
                ),
                new Color( // Light Seafoam
                RaceRecolorizer.GetColorsFromRGB(134f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(182f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(164f * 4/5)
                )),
            ColorRampGenerator.CreateHumanSkinRamp(new Color( // Dark Seafoam
                RaceRecolorizer.GetColorsFromRGB(66f),
                RaceRecolorizer.GetColorsFromRGB(179f),
                RaceRecolorizer.GetColorsFromRGB(122f)
                ),
                new Color( // Light Seafoam
                RaceRecolorizer.GetColorsFromRGB(66f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(179f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(122f * 4/5)
                )),
            ColorRampGenerator.CreateHumanSkinRamp(new Color( // Light Water
                RaceRecolorizer.GetColorsFromRGB(110f),
                RaceRecolorizer.GetColorsFromRGB(143f),
                RaceRecolorizer.GetColorsFromRGB(202f)
                ),
                new Color( // Light Water
                RaceRecolorizer.GetColorsFromRGB(110f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(143f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(202f * 4/5)
                )),
            
            ColorRampGenerator.CreateHumanSkinRamp(new Color( // Deep Water //
                RaceRecolorizer.GetColorsFromRGB(71f),
                RaceRecolorizer.GetColorsFromRGB(114f),
                RaceRecolorizer.GetColorsFromRGB(154f)
                ),
                new Color( // Deep Water //
                RaceRecolorizer.GetColorsFromRGB(71f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(114f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(154f * 4/5)
                ))
        ];

        public static List<Texture2D> CustomEyeRamps =
        [
            ColorRampGenerator.CreateOreadEyeRamp(new Color( // White
                RaceRecolorizer.GetColorsFromRGB(182f),
                RaceRecolorizer.GetColorsFromRGB(182f),
                RaceRecolorizer.GetColorsFromRGB(182f)
                ),
                new Color( // White
                RaceRecolorizer.GetColorsFromRGB(182f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(182f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(182f * 4/5)
                )),
            ColorRampGenerator.CreateOreadEyeRamp(new Color( // Light Purple
                RaceRecolorizer.GetColorsFromRGB(190f),
                RaceRecolorizer.GetColorsFromRGB(150f),
                RaceRecolorizer.GetColorsFromRGB(205f)
                ),
                new Color( // Light Purple
                RaceRecolorizer.GetColorsFromRGB(190f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(150f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(205f * 4/5)
                )),
            ColorRampGenerator.CreateOreadEyeRamp(new Color( // Light Teal
                RaceRecolorizer.GetColorsFromRGB(143f),
                RaceRecolorizer.GetColorsFromRGB(220f),
                RaceRecolorizer.GetColorsFromRGB(203f)
                ),
                new Color( // Light Teal
                RaceRecolorizer.GetColorsFromRGB(143f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(220f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(203f * 4/5)
                )),
            ColorRampGenerator.CreateGnomeEyeRamp(new Color( // Light Blue
                RaceRecolorizer.GetColorsFromRGB(133f),
                RaceRecolorizer.GetColorsFromRGB(154f),
                RaceRecolorizer.GetColorsFromRGB(190f)
                ),
                new Color( // Light Blue
                RaceRecolorizer.GetColorsFromRGB(133f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(154f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(190f * 4/5)
                )),
            ColorRampGenerator.CreateGnomeEyeRamp(new Color( // Perrywinkle
                RaceRecolorizer.GetColorsFromRGB(119f),
                RaceRecolorizer.GetColorsFromRGB(125f),
                RaceRecolorizer.GetColorsFromRGB(237f)
                ),
                new Color( // Perrywinkle
                RaceRecolorizer.GetColorsFromRGB(119f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(125f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(237f * 4/5)
                )),
            ColorRampGenerator.CreateGnomeEyeRamp(new Color( // Very light fuscia
                RaceRecolorizer.GetColorsFromRGB(233f),
                RaceRecolorizer.GetColorsFromRGB(196f),
                RaceRecolorizer.GetColorsFromRGB(241f)
                ),
                new Color( // Very light fuscia
                RaceRecolorizer.GetColorsFromRGB(233f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(196f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(241f * 4/5)
                )),
            ColorRampGenerator.CreateGnomeEyeRamp(new Color( // Light turqois
                RaceRecolorizer.GetColorsFromRGB(149f),
                RaceRecolorizer.GetColorsFromRGB(199f),
                RaceRecolorizer.GetColorsFromRGB(186f)
                ),
                new Color( // Light turqois
                RaceRecolorizer.GetColorsFromRGB(149f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(199f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(186f * 4/5)
                )),
            ColorRampGenerator.CreateOreadEyeRamp(new Color( // Light Blue
                RaceRecolorizer.GetColorsFromRGB(144f),
                RaceRecolorizer.GetColorsFromRGB(160f),
                RaceRecolorizer.GetColorsFromRGB(236f)
                ),
                new Color( // Light Blue
                RaceRecolorizer.GetColorsFromRGB(144f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(160f * 4/5),
                RaceRecolorizer.GetColorsFromRGB(236f * 4/5)
                ))
        ];

        public static List<Texture2D> CustomHairRamps =
        [
            ColorRampGenerator.CreateHumanHairRamp(new Color( // Seaweed
                RaceRecolorizer.GetColorsFromRGB(26f),
                RaceRecolorizer.GetColorsFromRGB(179f),
                RaceRecolorizer.GetColorsFromRGB(58f)
                ),
                new Color( // Dark blue
                RaceRecolorizer.GetColorsFromRGB(26f * 3/5),
                RaceRecolorizer.GetColorsFromRGB(179f * 3/5),
                RaceRecolorizer.GetColorsFromRGB(58f * 3/5)
                )),
            ColorRampGenerator.CreateHumanHairRamp(new Color( // Dark seaweed
                RaceRecolorizer.GetColorsFromRGB(18f),
                RaceRecolorizer.GetColorsFromRGB(133f),
                RaceRecolorizer.GetColorsFromRGB(42f)
                ),
                new Color( // Dark blue
                RaceRecolorizer.GetColorsFromRGB(18f * 3/5),
                RaceRecolorizer.GetColorsFromRGB(179f * 3/5),
                RaceRecolorizer.GetColorsFromRGB(42f * 3/5)
                )),
            ColorRampGenerator.CreateHumanHairRamp(new Color( // Dark blue
                RaceRecolorizer.GetColorsFromRGB(50f),
                RaceRecolorizer.GetColorsFromRGB(81f),
                RaceRecolorizer.GetColorsFromRGB(200f)
                ),
                new Color( // Dark blue
                RaceRecolorizer.GetColorsFromRGB(50f * 3/5),
                RaceRecolorizer.GetColorsFromRGB(81f * 3/5),
                RaceRecolorizer.GetColorsFromRGB(200f * 3/5)
                )),
            ColorRampGenerator.CreateHumanHairRamp(new Color( // Very blue
                RaceRecolorizer.GetColorsFromRGB(68f),
                RaceRecolorizer.GetColorsFromRGB(106f),
                RaceRecolorizer.GetColorsFromRGB(200f)
                ),
                new Color( // Very blue
                RaceRecolorizer.GetColorsFromRGB(68f * 3/5),
                RaceRecolorizer.GetColorsFromRGB(106f * 3/5),
                RaceRecolorizer.GetColorsFromRGB(200f * 3/5)
                )),
            ColorRampGenerator.CreateHumanHairRamp(new Color( // Teal Seafoam
                RaceRecolorizer.GetColorsFromRGB(116f),
                RaceRecolorizer.GetColorsFromRGB(196f),
                RaceRecolorizer.GetColorsFromRGB(213f)
                ),
                new Color( // Teal Seafoam
                RaceRecolorizer.GetColorsFromRGB(116f * 3/5),
                RaceRecolorizer.GetColorsFromRGB(196f * 3/5),
                RaceRecolorizer.GetColorsFromRGB(213f * 3/5)
                )),
            ColorRampGenerator.CreateHumanHairRamp(new Color( // Very Dark Blue
                RaceRecolorizer.GetColorsFromRGB(6f),
                RaceRecolorizer.GetColorsFromRGB(28f),
                RaceRecolorizer.GetColorsFromRGB(109f)
                ),
                new Color( // Very Dark Blue
                RaceRecolorizer.GetColorsFromRGB(6f * 3/5),
                RaceRecolorizer.GetColorsFromRGB(28f * 3/5),
                RaceRecolorizer.GetColorsFromRGB(109f * 3/5)
                )),
            ColorRampGenerator.CreateHumanHairRamp(new Color( // Sea Green
                RaceRecolorizer.GetColorsFromRGB(38f),
                RaceRecolorizer.GetColorsFromRGB(155f),
                RaceRecolorizer.GetColorsFromRGB(143f)
                ),
                new Color( // Sea Green
                RaceRecolorizer.GetColorsFromRGB(38f * 3/5),
                RaceRecolorizer.GetColorsFromRGB(155f * 3/5),
                RaceRecolorizer.GetColorsFromRGB(143f * 3/5)
                )),
            ColorRampGenerator.CreateHumanHairRamp(new Color( // Light Blue
                RaceRecolorizer.GetColorsFromRGB(134f),
                RaceRecolorizer.GetColorsFromRGB(153f),
                RaceRecolorizer.GetColorsFromRGB(236f)
                ),
                new Color( // Light Blue
                RaceRecolorizer.GetColorsFromRGB(134f * 3/5),
                RaceRecolorizer.GetColorsFromRGB(153f * 3/5),
                RaceRecolorizer.GetColorsFromRGB(236f * 3/5)
                ))
        ];

        public static EquipmentEntityLink[] FemaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "779458079f7718c4bb960d9cef195339"}, // Long Wavy Braids
            new EquipmentEntityLink() {AssetId = "04c3eb6d7570d8d49b686516b7c4a4f8"}, // Long Camelia Hair
            new EquipmentEntityLink() {AssetId = "30d504db6b8cbe94dbc82d2437c8b468"}, // Long Wavy Female Human
            new EquipmentEntityLink() {AssetId = "1762cab3d178f53489f43ab791b87f9c"}, // Noble Braids - Dwarf
            new EquipmentEntityLink() {AssetId = "34bb68b3e4f03be44a1f0611a09530fc"}, // Crown Braids - Dwarf
            new EquipmentEntityLink() {AssetId = "1f19aaaa1870e2b4b8bd99d36211ddf6"}, // Ponytail Upper Female Human
            new EquipmentEntityLink() {AssetId = "fc3fc0e45a70a0e42b6aed10baf794f0"}, // Dread Seelah Female Human
            new EquipmentEntityLink() {AssetId = "d71d2e53fce0f1d4baad8b20c8266676"}, // Slick Female Tiefling
            new EquipmentEntityLink() {AssetId = "afa22656ed5030c4ba273583ba2b3a16"}, // Long Wild Female Tiefling
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"}  // Bald
        ];

        public static EquipmentEntityLink[] MaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "acdcfd7609f88ae49833e4f10656190e"}, // Long Bangs Male Human
            new EquipmentEntityLink() {AssetId = "303578a648d8d344b8d3a9a94fe24d5a"}, // Long Wavy Male Human
            new EquipmentEntityLink() {AssetId = "609143dbf7607f6419babaf5748b82dc"}, // Medium Bun Male Human
            new EquipmentEntityLink() {AssetId = "88c2650d77d9a3c4a8a861fa0d8d0aae"}, // Military Male Human
            new EquipmentEntityLink() {AssetId = "def666224ba24df4e954c03049b29a53"}, // Short Human
            new EquipmentEntityLink() {AssetId = "329cf540a8faed64284c067bace8bbc9"}, // Trim Male Human
            new EquipmentEntityLink() {AssetId = "222890293b0f66145a400eae3432868d"}, // Mohawk Male Human
            new EquipmentEntityLink() {AssetId = "50eac92ba30862940be4f70d329d070a"}, // Long Wild Male Tiefling
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"}  // Bald
        ];

        public static EquipmentEntityLink[] FemaleHeads =
        [
            new EquipmentEntityLink() {AssetId = "52a7bdb6964f65a41b735635a2513edd"}, // Female Human Head 14
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string NaiadName = "NaiadRace";

        internal const string NaiadDisplayName = "Naiad.Name";
        private static readonly string NaiadDescription = "Naiad.Description";
        public static readonly string RaceGuid = "{0ABA4D55-2498-4ED6-B69A-E9B5094172DD}";

        internal const string FeyNatureDisplayName = "Naiad.FeyNature.Name";
        private static readonly string FeyNatureDescription = "Naiad.FeyNature.Description";

        internal const string WaterBondDisplayName = "Naiad.WaterBond.Name";
        private static readonly string WaterBondDescription = "Naiad.WaterBond.Description";

        internal const string InspirationDisplayName = "Naiad.Inspiration.Name";
        private static readonly string InspirationDescription = "Naiad.Inspiration.Description";

        internal const string GrantInspirationDisplayName = "Naiad.GrantInspiration.Name";
        internal const string RevokeInspirationDisplayName = "Naiad.RevokeInspiration.Name";

        internal static void Configure()
        {
            var FinalFemaleHeads = FemaleHeads.AppendToArray(CopyRace.FemaleOptions.Heads);

            // Inspiration

            var InspirationBuff = BuffConfigurator.New("NaiadInspirationBuff", "{45AA1D9F-7DD7-4078-BB4B-94A1335D08A7}")
                .SetDisplayName(InspirationDisplayName)
                .SetDescription(InspirationDescription)
                .SetIcon(AbilityRefs.BestowGraceOfTheChampion.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.Insight, stat: StatType.SaveWill, value: 1)
                .AddStatBonus(ModifierDescriptor.Insight, stat: StatType.SkillKnowledgeWorld, value: 1)
                .AddStatBonus(ModifierDescriptor.Insight, stat: StatType.SkillPersuasion, value: 1)
                .Configure();

            var RevokeInspirationAbility = AbilityConfigurator.New("NaiadRevokeInspirationAbility", "{CE1F880C-DBDA-4762-BC49-326637BEE396}")
                .SetDisplayName(InspirationDisplayName)
                .SetDescription(InspirationDescription)
                .SetIcon(AbilityRefs.BestowGraceOfTheChampion.Reference.Get().Icon)
                .SetRange(AbilityRange.Personal)
                .SetActionType(CommandType.Free)
                .AddAbilityEffectRunAction(
                    ActionsBuilder.New()
                        .Add<ContextActionRemoveNaiadInspiration>(a =>
                        {
                            a.m_Buff = InspirationBuff.ToReference<BlueprintBuffReference>();
                        }))
                .Configure();

            var ApplyInspirationAbility = AbilityConfigurator.New("NaiadApplyInspirationAbility", "{89F4C557-56B1-4FB3-807E-68DAE37F7249}")
                .SetDisplayName(InspirationDisplayName)
                .SetDescription(InspirationDescription)
                .SetIcon(AbilityRefs.BestowGraceOfTheChampion.Reference.Get().Icon)
                .SetRange(AbilityRange.Touch)
                .SetActionType(CommandType.Standard)
                .SetCanTargetFriends(true)
                .SetCanTargetSelf(false)
                .SetAnimation(CastAnimationStyle.Touch)
                .AddAbilityEffectRunAction(
                    ActionsBuilder.New()
                        .Add<ContextActionRemoveNaiadInspiration>(a =>
                        {
                            a.m_Buff = InspirationBuff.ToReference<BlueprintBuffReference>();
                        })
                        .ApplyBuffPermanent(
                            InspirationBuff,
                            isNotDispelable: true))
                .Configure();
            
            var Inspiration = FeatureConfigurator.New("NaiadInspiration", "{24E0507C-DD89-4E2D-A402-7B06CA75BA83}")
                .SetDisplayName(InspirationDisplayName)
                .SetDescription(InspirationDescription)
                .SetIcon(AbilityRefs.BestowGraceOfTheChampion.Reference.Get().Icon)
                .AddFacts([ApplyInspirationAbility, RevokeInspirationAbility])
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            // Water Bond
            var WaterBond = FeatureConfigurator.New("NaiadWaterBond", "{8ECBFD5E-A17B-4AB4-86E2-2479AAA524DA}")
                .SetDisplayName(WaterBondDisplayName)
                .SetDescription(WaterBondDescription)
                .SetIcon(FeatureRefs.WaterBlastFeature.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.Insight, stat: StatType.AC, value: 1)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SaveFortitude, value: 1)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SaveReflex, value: 1)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SaveWill, value: 1)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            // Fey Nature
            var FeyNature = FeatureConfigurator.New("NaiadFeyNature", "{9FAC935A-268B-4A4D-92C3-E20D7E2E5EE3}")
                .SetDisplayName(FeyNatureDisplayName)
                .SetDescription(FeyNatureDescription)
                .SetIcon(ProgressionRefs.BloodlineFeyProgression.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillMobility, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillLoreNature, value: 2)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var race =
            RaceConfigurator.New(NaiadName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(NaiadDisplayName)
                .SetDescription(NaiadDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(FeyNature, Inspiration, WaterBond, FeatureRefs.KeenSenses.ToString(), FeatureRefs.FeyType.ToString(), FeatureRefs.SubtypeWater.ToString())
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: -2)
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, CustomHeadRamps: CustomHeadRamps, CustomEyeRamps: CustomEyeRamps, CustomHairRamps: CustomHairRamps, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.OreadRace.ToString()), CustomFemaleHairs: FemaleHairs, CustomMaleHairs: MaleHairs, CustomFemaleHeads: FinalFemaleHeads);

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

            // Add portraits
            PortraitCreatonator.RegisterRacePortrait("Naiad_F_01", "{F58AA56D-D2F1-4C4E-81A2-531A957FAC6D}", race, Gender.Female, "Naiad_F_01");
            PortraitCreatonator.RegisterRacePortrait("Naiad_M_01", "{58996970-D302-4891-8B69-18FA6448D1CC}", race, Gender.Male, "Naiad_M_01");
        }
    }
}
