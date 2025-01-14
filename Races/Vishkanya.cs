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

namespace EbonsContentMod.Races
{
    internal class Vishkanya
    {
        public static List<Color> RaceHeadColors =
        [
            new Color( // Light Blue-Green
                RaceRecolorizer.GetColorsFromRGB(110f),
                RaceRecolorizer.GetColorsFromRGB(120f),
                RaceRecolorizer.GetColorsFromRGB(100f)
                ),
            new Color( // Muted Green-Gray
                RaceRecolorizer.GetColorsFromRGB(90f),
                RaceRecolorizer.GetColorsFromRGB(105f),
                RaceRecolorizer.GetColorsFromRGB(80f)
                ),
            new Color( // Light Muted Green-Gray
                RaceRecolorizer.GetColorsFromRGB(120f),
                RaceRecolorizer.GetColorsFromRGB(135f),
                RaceRecolorizer.GetColorsFromRGB(105f)
                ),
            new Color( // Light Green-Tan
                RaceRecolorizer.GetColorsFromRGB(130f),
                RaceRecolorizer.GetColorsFromRGB(125f),
                RaceRecolorizer.GetColorsFromRGB(100f)
                ),
            new Color( // Deep Earth
                RaceRecolorizer.GetColorsFromRGB(76f),
                RaceRecolorizer.GetColorsFromRGB(59f),
                RaceRecolorizer.GetColorsFromRGB(37f)
                ),
            new Color( // Sand
                RaceRecolorizer.GetColorsFromRGB(111f),
                RaceRecolorizer.GetColorsFromRGB(89f),
                RaceRecolorizer.GetColorsFromRGB(62f)
                ),
            new Color( // Deep Bronze
                RaceRecolorizer.GetColorsFromRGB(116f),
                RaceRecolorizer.GetColorsFromRGB(86f),
                RaceRecolorizer.GetColorsFromRGB(50f)
                ),
            new Color( // Gold
                RaceRecolorizer.GetColorsFromRGB(161f),
                RaceRecolorizer.GetColorsFromRGB(139f),
                RaceRecolorizer.GetColorsFromRGB(96f)
                ),
            new Color( // Silver
                RaceRecolorizer.GetColorsFromRGB(136f),
                RaceRecolorizer.GetColorsFromRGB(132f),
                RaceRecolorizer.GetColorsFromRGB(132f)
                ),
            new Color( // Light gray
                RaceRecolorizer.GetColorsFromRGB(139f),
                RaceRecolorizer.GetColorsFromRGB(139f),
                RaceRecolorizer.GetColorsFromRGB(139f)
                ),
            new Color( // Dark Yellow-Green
                RaceRecolorizer.GetColorsFromRGB(60f),
                RaceRecolorizer.GetColorsFromRGB(75f),
                RaceRecolorizer.GetColorsFromRGB(20f)
                ),
            new Color( // Dark Hunter Green
                RaceRecolorizer.GetColorsFromRGB(45f),
                RaceRecolorizer.GetColorsFromRGB(75f),
                RaceRecolorizer.GetColorsFromRGB(35f)
                ),
            new Color( // Very Dark Hunter Green
                RaceRecolorizer.GetColorsFromRGB(25f),
                RaceRecolorizer.GetColorsFromRGB(50f),
                RaceRecolorizer.GetColorsFromRGB(20f)
                )
        ];

        public static List<Color> RaceEyeColors =
        [
            
        ];

        public static List<Color> RaceHairColors =
        [
            new Color(0.2f, 0.2f, 0.23f), // Black
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
            new Color( // Dark Green
                RaceRecolorizer.GetColorsFromRGB(27f),
                RaceRecolorizer.GetColorsFromRGB(37f),
                RaceRecolorizer.GetColorsFromRGB(15f)
                ),
            new Color( // Green-Brown
                RaceRecolorizer.GetColorsFromRGB(60f),
                RaceRecolorizer.GetColorsFromRGB(60f),
                RaceRecolorizer.GetColorsFromRGB(0f)
                ),
            new Color( // Light Brown
                RaceRecolorizer.GetColorsFromRGB(100f),
                RaceRecolorizer.GetColorsFromRGB(85f),
                RaceRecolorizer.GetColorsFromRGB(55f)
                )
        ];

        public static EquipmentEntityLink[] FemaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "1762cab3d178f53489f43ab791b87f9c"}, // Noble Braids - Dwarf
            new EquipmentEntityLink() {AssetId = "34bb68b3e4f03be44a1f0611a09530fc"}, // Crown Braids - Dwarf
            new EquipmentEntityLink() {AssetId = "779458079f7718c4bb960d9cef195339"}, // Long Wavy Braids
            new EquipmentEntityLink() {AssetId = "04c3eb6d7570d8d49b686516b7c4a4f8"}, // Long Camelia Hair
            new EquipmentEntityLink() {AssetId = "30d504db6b8cbe94dbc82d2437c8b468"}, // Long Wavy Female Human
            new EquipmentEntityLink() {AssetId = "1f19aaaa1870e2b4b8bd99d36211ddf6"}, // Ponytail Upper Female Human
            new EquipmentEntityLink() {AssetId = "fc3fc0e45a70a0e42b6aed10baf794f0"}, // Dread Seelah Female Human
            new EquipmentEntityLink() {AssetId = "d71d2e53fce0f1d4baad8b20c8266676"}, // Slick Female Tiefling
            new EquipmentEntityLink() {AssetId = "afa22656ed5030c4ba273583ba2b3a16"}, // Long Wild Female Tiefling
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"} // Bald
        ];

        public static EquipmentEntityLink[] MaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "88c2650d77d9a3c4a8a861fa0d8d0aae"}, // Military Male Human
            new EquipmentEntityLink() {AssetId = "def666224ba24df4e954c03049b29a53"}, // Short Human
            new EquipmentEntityLink() {AssetId = "329cf540a8faed64284c067bace8bbc9"}, // Trim Male Human
            new EquipmentEntityLink() {AssetId = "609143dbf7607f6419babaf5748b82dc"}, // Medium Bun Male Human
            new EquipmentEntityLink() {AssetId = "303578a648d8d344b8d3a9a94fe24d5a"}, // Long Wavy Male Human
            new EquipmentEntityLink() {AssetId = "acdcfd7609f88ae49833e4f10656190e"}, // Long Bangs Male Human
            new EquipmentEntityLink() {AssetId = "222890293b0f66145a400eae3432868d"}, // Mohawk Male Human
            new EquipmentEntityLink() {AssetId = "50eac92ba30862940be4f70d329d070a"}, // Long Wild Male Tiefling
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"} // Bald
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string VishkanyaName = "VishkanyaRace";

        internal const string VishkanyaDisplayName = "Vishkanya.Name";
        private static readonly string VishkanyaDescription = "Vishkanya.Description";
        public static readonly string RaceGuid = "{700231ED-562E-4B89-A87C-C2C350BEC42E}";

        internal const string PoisonResistanceDisplayName = "Vishkanya.PoisonResistance.Name";
        private static readonly string PoisonResistanceDescription = "Vishkanya.PoisonResistance.Description";

        internal const string LimberDisplayName = "Vishkanya.Limber.Name";
        private static readonly string LimberDescription = "Vishkanya.Limber.Description";

        internal const string WeaponFamiliarityDisplayName = "Vishkanya.WeaponFamiliarity.Name";
        private static readonly string WeaponFamiliarityDescription = "Vishkanya.WeaponFamiliarity.Description";

        internal const string ToxicDisplayName = "Vishkanya.Toxic.Name";
        private static readonly string ToxicDescription = "Vishkanya.Toxic.Description";

        internal static void Configure()
        {
            var ContactEEL = new EquipmentEntityLink() { AssetId = "1aeb459da29dca341a78317170eec262" };

            var PoisonResistance = FeatureConfigurator.New("VishkanyaPoisonResistance", "{0FC3B878-5823-4881-8F8F-EA17AD4B0616}")
                .SetDisplayName(PoisonResistanceDisplayName)
                .SetDescription(PoisonResistanceDescription)
                .SetIcon(FeatureRefs.PoisonImmunity.Reference.Get().Icon)
                .AddSavingThrowBonusAgainstDescriptor(ContextValues.Rank(), modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.Poison)
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel())
                .Configure();

            var Limber = FeatureConfigurator.New("VishkanyaLimber", "{4BE4A9DD-34CF-4228-ABC0-52CBF467AF44}")
                .SetDisplayName(LimberDisplayName)
                .SetDescription(LimberDescription)
                .SetIcon(FeatureRefs.Evasion.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillMobility, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillStealth, value: 2)
                .Configure();

            var ToxicResource = AbilityResourceConfigurator.New("VishkanyaToxicResource", "{F95920B9-AA87-43A1-9EBD-2C57D58A673F}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1).IncreaseByStat(StatType.Constitution))
                .Configure();

            var ToxicPoisonInitialBuff = AbilityConfigurator.New("VishkanyaToxicPoisonInitialBuff", "{B6D8190B-E075-43A6-81CD-470F1994B86E}")
                .SetDisplayName(ToxicDisplayName)
                .SetDescription(ToxicDescription)
                .SetIcon(FeatureRefs.AssassinCreatePoison.Reference.Get().Icon)
                .SetHidden(true)
                .Configure();

            var ToxicPoisonBuff = AbilityConfigurator.New("VishkanyaToxicPoisonBuff", "{63E8A8C8-7316-4AB2-9BFC-F2CDD7773556}")
                .SetDisplayName(ToxicDisplayName)
                .SetDescription(ToxicDescription)
                .SetIcon(FeatureRefs.AssassinCreatePoison.Reference.Get().Icon)
                .AddComponent<BuffPoisonStatDamageContext>(c =>
                {
                    c.Descriptor = ModifierDescriptor.StatDamage;
                    c.Stat = StatType.Dexterity;
                    c.Value = new ContextDiceValue()
                    {
                        DiceType = Kingmaker.RuleSystem.DiceType.D4,
                        DiceCountValue = new ContextValue()
                        {
                            ValueType = ContextValueType.Simple,
                            Value = 1
                        }
                    };
                    c.SaveType = SavingThrowType.Fortitude;
                    c.NoEffectOnFirstTick = false;
                    c.WeaknessForPoison = ToxicPoisonInitialBuff.ToReference<BlueprintBuffReference>();
                })
                .Configure();

            var actions = ActionFlow.DoSingle<Conditional>(ac =>
            {
                ac.ConditionsChecker = ActionFlow.IfAll(
                    new ContextConditionHasFact() // Is not already poisoned
                    {
                        m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("63E8A8C8-7316-4AB2-9BFC-F2CDD7773556"),
                        Not = true
                    }
                );
                ac.IfTrue = ActionFlow.DoSingle<ContextActionSavingThrow>(bc =>
                {
                    bc.m_ConditionalDCIncrease = [];
                    bc.Type = SavingThrowType.Fortitude;
                    bc.HasCustomDC = false;
                    bc.CustomDC = new ContextValue();
                    bc.Actions = ActionFlow.DoSingle<ContextActionConditionalSaved>(cc =>
                    {
                        cc.Succeed = ActionFlow.DoSingle<ContextActionRemoveSelf>();
                        cc.Failed = ActionFlow.DoSingle<Conditional>(dc =>
                        {
                            dc.ConditionsChecker = ActionFlow.IfAll(
                                new ContextConditionHasFact() // Does not have the initial poison damage buff
                                {
                                    m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("B6D8190B-E075-43A6-81CD-470F1994B86E"),
                                    Not = true
                                }
                            );
                            dc.IfTrue = ActionFlow.DoSingle<ContextActionApplyBuff>(ec =>
                            {
                                ec.m_Buff = ToxicPoisonInitialBuff.ToReference<BlueprintBuffReference>();
                                ec.UseDurationSeconds = false;
                                ec.Permanent = false;
                                ec.DurationValue = new ContextDurationValue()
                                {
                                    Rate = DurationRate.Rounds,
                                    DiceType = Kingmaker.RuleSystem.DiceType.Zero
                                };
                                ec.AsChild = true;
                            });
                            dc.IfFalse = ActionFlow.DoNothing();
                        });
                        cc.Failed = ActionFlow.DoSingle<ContextActionApplyBuff>(dc =>
                        {
                            dc.m_Buff = ToxicPoisonBuff.ToReference<BlueprintBuffReference>();
                            dc.UseDurationSeconds = true;
                            dc.DurationSeconds = 36f;
                        });
                    });
                });
                ac.IfFalse = ActionFlow.DoNothing();
            });

            var ToxicBuff = AbilityConfigurator.New("VishkanyaToxicBuff", "{783947AC-F3B9-4A5B-BE6F-415723A9135E}")
                .SetDisplayName(ToxicDisplayName)
                .SetDescription(ToxicDescription)
                .SetIcon(FeatureRefs.AssassinCreatePoison.Reference.Get().Icon)
                .AddInitiatorAttackWithWeaponTrigger(actions, onlyHit: true)
                .AddInitiatorAttackWithWeaponTrigger(ActionsBuilder.New().RemoveSelf().Build(), onlyHit: true)
                .AddContextSetAbilityParams(add10ToDC: true, dC: new ContextValue()
                {
                    ValueType = ContextValueType.Shared,
                    Value = 12,
                    ValueRank = AbilityRankType.Default,
                    ValueShared = Kingmaker.UnitLogic.Abilities.AbilitySharedValue.Damage,
                    m_AbilityParameter = AbilityParameterType.Level
                })
                .Configure();

            var ToxicAbility = AbilityConfigurator.New("VishkanyaToxicAbility", "{3D72A521-6F91-420E-8A46-F49BC6B27CE1}")
                .SetDisplayName(ToxicDisplayName)
                .SetDescription(ToxicDescription)
                .SetIcon(FeatureRefs.AssassinCreatePoison.Reference.Get().Icon)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuffPermanent(ToxicBuff, isNotDispelable: true, toCaster: true).Build())
                .SetRange(AbilityRange.Personal)
                .Configure();
            
            var Toxic = FeatureConfigurator.New("VishkanyaToxic", "{B9410FDF-05B8-4572-A379-2C6A462DD456}")
                .SetDisplayName(ToxicDisplayName)
                .SetDescription(ToxicDescription)
                .SetIcon(FeatureRefs.AssassinCreatePoison.Reference.Get().Icon)
                .AddFacts([ToxicResource, ToxicAbility])
                .Configure();

            var WeaponFamiliarity = FeatureConfigurator.New("VishkanyaWeaponFamiliarity", "{888BA5C3-F92E-4019-B14D-5D52F5FEF38B}")
                .SetDisplayName(WeaponFamiliarityDisplayName)
                .SetDescription(WeaponFamiliarityDescription)
                //.SetIcon(FeatureRefs.ElvenWeaponFamiliarity.Reference.Get().Icon)
                .SetIcon(ItemWeaponRefs.StandardKukri.Reference.Get().Icon)
                .AddProficiencies(weaponProficiencies: [WeaponCategory.Kukri, WeaponCategory.Starknife])
                .Configure();

            var race =
            RaceConfigurator.New(VishkanyaName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(VishkanyaDisplayName)
                .SetDescription(VishkanyaDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(PoisonResistance, Limber, Toxic, WeaponFamiliarity, FeatureRefs.KeenSenses.ToString(), FeatureRefs.ReptilianSubtype.ToString())
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: -2)
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, eyecolors: RaceEyeColors, eyeEE: ContactEEL, CustomFemaleHairs: FemaleHairs, CustomMaleHairs: MaleHairs);

            // Re-order presets to make the race default to skinny and remove fat option
            var FinalRace = RaceConfigurator.For(recoloredrace)
                .SetPresets(recoloredrace.m_Presets[2], recoloredrace.m_Presets[0])
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
