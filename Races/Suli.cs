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
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace EbonsContentMod.Races
{
    internal class Suli
    {
        public static List<Color> RaceHeadColors =
        [
            new Color( // Bronze
                RaceRecolorizer.GetColorsFromRGB(126f),
                RaceRecolorizer.GetColorsFromRGB(101f),
                RaceRecolorizer.GetColorsFromRGB(73f)
                ),
            new Color( // Ruddy
                RaceRecolorizer.GetColorsFromRGB(113f),
                RaceRecolorizer.GetColorsFromRGB(83f),
                RaceRecolorizer.GetColorsFromRGB(78f)
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
            new Color( // Sand
                RaceRecolorizer.GetColorsFromRGB(111f),
                RaceRecolorizer.GetColorsFromRGB(89f),
                RaceRecolorizer.GetColorsFromRGB(62f)
                ),
            new Color( // Pink stone
                RaceRecolorizer.GetColorsFromRGB(169f),
                RaceRecolorizer.GetColorsFromRGB(132f),
                RaceRecolorizer.GetColorsFromRGB(132f)
                ),
            new Color( // Deep Earth
                RaceRecolorizer.GetColorsFromRGB(76f),
                RaceRecolorizer.GetColorsFromRGB(59f),
                RaceRecolorizer.GetColorsFromRGB(37f)
                )
        ];

        public static List<Color> RaceEyeColors =
        [
            new Color( // Fire
                RaceRecolorizer.GetColorsFromRGB(245f),
                RaceRecolorizer.GetColorsFromRGB(159f),
                RaceRecolorizer.GetColorsFromRGB(0f)
                ),
            new Color( // Water
                RaceRecolorizer.GetColorsFromRGB(80f),
                RaceRecolorizer.GetColorsFromRGB(80f),
                RaceRecolorizer.GetColorsFromRGB(247f)
                ),
            new Color( // Electric
                RaceRecolorizer.GetColorsFromRGB(153f),
                RaceRecolorizer.GetColorsFromRGB(190f),
                RaceRecolorizer.GetColorsFromRGB(255f)
                ),
            new Color( // Acid
                RaceRecolorizer.GetColorsFromRGB(125f),
                RaceRecolorizer.GetColorsFromRGB(190f),
                RaceRecolorizer.GetColorsFromRGB(64f)
                ),
            new Color( // White
                RaceRecolorizer.GetColorsFromRGB(190f),
                RaceRecolorizer.GetColorsFromRGB(190f),
                RaceRecolorizer.GetColorsFromRGB(190f)
                )
        ];

        public static List<Color> RaceHairColors =
        [
            new Color( // Black
                RaceRecolorizer.GetColorsFromRGB(30f),
                RaceRecolorizer.GetColorsFromRGB(30f),
                RaceRecolorizer.GetColorsFromRGB(30f)
                ),
            new Color( // Brown
                RaceRecolorizer.GetColorsFromRGB(75f),
                RaceRecolorizer.GetColorsFromRGB(48f),
                RaceRecolorizer.GetColorsFromRGB(0f)
                ),
            new Color( // Deep Brown
                RaceRecolorizer.GetColorsFromRGB(58f),
                RaceRecolorizer.GetColorsFromRGB(43f),
                RaceRecolorizer.GetColorsFromRGB(24f)
                ),
            new Color( // Dark Gray
                RaceRecolorizer.GetColorsFromRGB(63f),
                RaceRecolorizer.GetColorsFromRGB(55f),
                RaceRecolorizer.GetColorsFromRGB(47f)
                ),
            new Color( // Deep Ruddy Brown
                RaceRecolorizer.GetColorsFromRGB(101f),
                RaceRecolorizer.GetColorsFromRGB(61f),
                RaceRecolorizer.GetColorsFromRGB(43f)
                )
        ];

        public static EquipmentEntityLink[] FemaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "afa22656ed5030c4ba273583ba2b3a16"}, // Long Wild Tiefling
            new EquipmentEntityLink() {AssetId = "04c3eb6d7570d8d49b686516b7c4a4f8"}, // Long Camelia Hair
            new EquipmentEntityLink() {AssetId = "779458079f7718c4bb960d9cef195339"}, // Long Wavy Braids
            new EquipmentEntityLink() {AssetId = "1762cab3d178f53489f43ab791b87f9c"}, // Noble Braids - Dwarf
            new EquipmentEntityLink() {AssetId = "3897a5ce68b630548bb85db0a68a465a"}  // Tight French Braid
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string SuliName = "SuliRace";

        internal const string SuliDisplayName = "Suli.Name";
        private static readonly string SuliDescription = "Suli.Description";
        public static readonly string RaceGuid = "{D5398269-CC14-42D7-8024-69CBE7FDF151}";

        internal const string EnergyResistanceDisplayName = "Suli.EnergyResistance.Name";
        private static readonly string EnergyResistanceDescription = "Suli.EnergyResistance.Description";

        internal const string NegotiatorDisplayName = "Suli.Negotiator.Name";
        private static readonly string NegotiatorDescription = "Suli.Negotiator.Description";

        internal const string ElementalAssaultDisplayName = "Suli.ElementalAssault.Name";
        private static readonly string ElementalAssaultDescription = "Suli.ElementalAssault.Description";

        internal const string ElementalAssaultFireDisplayName = "Suli.ElementalAssaultFire.Name";
        internal const string ElementalAssaultColdDisplayName = "Suli.ElementalAssaultCold.Name";
        internal const string ElementalAssaultElectricityDisplayName = "Suli.ElementalAssaultElectricity.Name";
        internal const string ElementalAssaultAcidDisplayName = "Suli.ElementalAssaultAcid.Name";

        internal static void Configure()
        {
            var NewFemaleHairArray = FemaleHairs.AppendToArray(CopyRace.FemaleOptions.Hair);

            var EnergyResistance = FeatureConfigurator.New("SuliEnergyResistance", "{C33CFC4E-36DD-46E5-8840-EFD874576FF5}")
                .SetDisplayName(EnergyResistanceDisplayName)
                .SetDescription(EnergyResistanceDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.ProtectionFromEnergy.ToString()).Icon)
                .AddDamageResistanceEnergy(type: DamageEnergyType.Fire, value: 5)
                .AddDamageResistanceEnergy(type: DamageEnergyType.Cold, value: 5)
                .AddDamageResistanceEnergy(type: DamageEnergyType.Electricity, value: 5)
                .AddDamageResistanceEnergy(type: DamageEnergyType.Acid, value: 5)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var Negotiator = FeatureConfigurator.New("SuliNegotiator", "{79BA33CC-B515-42E1-8117-F97390901CA2}")
                .SetDisplayName(NegotiatorDisplayName)
                .SetDescription(NegotiatorDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.Persuasive.ToString()).Icon) // Change
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.CheckDiplomacy, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.CheckBluff, value: 2)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            // Elemental Assault
           var ElementalAssaultResource = AbilityResourceConfigurator.New("SuliElemetnalAssaultResource", "{2429135F-4EDD-4321-854C-B585EF140E19}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var ElementalAssaultFireBuff = BuffConfigurator.New("SuliElementalAssaultFireBuff", "{21B02987-FC5B-42DC-B55E-1A5A415E7AA0}")
                .SetDisplayName(ElementalAssaultFireDisplayName)
                .SetDescription(ElementalAssaultDescription)
                .SetIcon(AbilityRefs.ElementalFistAblilityFire.Reference.Get().Icon)
                .AdditionalDiceOnAttack(applyCriticalModifier: false, damageType: new DamageTypeDescription()
                {
                    Type = DamageType.Energy,
                    Energy = DamageEnergyType.Fire
                }, attackType: AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks, onHit: true, value: new ContextDiceValue()
                {
                    DiceType = DiceType.D6,
                    DiceCountValue = 1,
                    BonusValue = 0
                })
                .Configure();

            var ElementalAssaultFire = AbilityConfigurator.New("SuliElementalAssaultFire", "{8B105FCF-0C98-464A-9CCE-31C5BB56FB3D}")
                .SetDisplayName(ElementalAssaultFireDisplayName)
                .SetDescription(ElementalAssaultDescription)
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: ElementalAssaultResource)
                .SetIcon(AbilityRefs.ElementalFistAblilityFire.Reference.Get().Icon)
                .SetRange(AbilityRange.Personal)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(buff: ElementalAssaultFireBuff, durationValue: new ContextDurationValue() 
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
                }, false, isNotDispelable: true).Build())
                .AddComponent<ContextRankConfig>(c =>
                {
                    c.m_Type = AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                    c.m_Stat = StatType.Unknown;
                    c.m_SpecificModifier = ModifierDescriptor.None;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_StartLevel = 0;
                    c.m_StepLevel = 0;
                    c.m_UseMax = true;
                    c.m_Max = 20;
                })
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .AllowTargeting(false, false, false, true)
                .Configure();

            var ElementalAssaultColdBuff = BuffConfigurator.New("SuliElementalAssaultColdBuff", "{BA6DC8BC-F4B8-4750-AD5C-180A752DA425}")
                .SetDisplayName(ElementalAssaultColdDisplayName)
                .SetDescription(ElementalAssaultDescription)
                .SetIcon(AbilityRefs.ElementalFistAblilityCold.Reference.Get().Icon)
                .AdditionalDiceOnAttack(applyCriticalModifier: false, damageType: new DamageTypeDescription()
                {
                    Type = DamageType.Energy,
                    Energy = DamageEnergyType.Cold
                }, attackType: AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks, onHit: true, value: new ContextDiceValue()
                {
                    DiceType = DiceType.D6,
                    DiceCountValue = 1,
                    BonusValue = 0
                })
                .Configure();

            var ElementalAssaultCold = AbilityConfigurator.New("SuliElementalAssaultCold", "{F84C6816-CB8E-4782-A71B-ECF8139E6071}")
                .SetDisplayName(ElementalAssaultColdDisplayName)
                .SetDescription(ElementalAssaultDescription)
                .SetIcon(AbilityRefs.ElementalFistAblilityCold.Reference.Get().Icon)
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: ElementalAssaultResource)
                .SetRange(AbilityRange.Personal)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(buff: ElementalAssaultColdBuff, durationValue: new ContextDurationValue()
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
                }, false, isNotDispelable: true).Build())
                .AddComponent<ContextRankConfig>(c =>
                {
                    c.m_Type = AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                    c.m_Stat = StatType.Unknown;
                    c.m_SpecificModifier = ModifierDescriptor.None;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_StartLevel = 0;
                    c.m_StepLevel = 0;
                    c.m_UseMax = true;
                    c.m_Max = 20;
                })
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .AllowTargeting(false, false, false, true)
                .Configure();

            var ElementalAssaultElectricityBuff = BuffConfigurator.New("SuliElementalAssaultElectricityBuff", "{F460A255-0B38-4B7F-9368-12C2A168FAD6}")
                .SetDisplayName(ElementalAssaultElectricityDisplayName)
                .SetDescription(ElementalAssaultDescription)
                .SetIcon(AbilityRefs.ElementalFistAblilityElectricity.Reference.Get().Icon)
                .AdditionalDiceOnAttack(applyCriticalModifier: false, damageType: new DamageTypeDescription()
                {
                    Type = DamageType.Energy,
                    Energy = DamageEnergyType.Electricity
                }, attackType: AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks, onHit: true, value: new ContextDiceValue()
                {
                    DiceType = DiceType.D6,
                    DiceCountValue = 1,
                    BonusValue = 0
                })
                .Configure();

            var ElementalAssaultElectricity = AbilityConfigurator.New("SuliElementalAssaultElectricity", "{F9DDE82F-23BE-4362-8BCB-D9A1341AF1A8}")
                .SetDisplayName(ElementalAssaultElectricityDisplayName)
                .SetDescription(ElementalAssaultDescription)
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: ElementalAssaultResource)
                .SetIcon(AbilityRefs.ElementalFistAblilityElectricity.Reference.Get().Icon)
                .SetRange(AbilityRange.Personal)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(buff: ElementalAssaultElectricityBuff, durationValue: new ContextDurationValue()
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
                }, false, isNotDispelable: true).Build())
                .AddComponent<ContextRankConfig>(c =>
                {
                    c.m_Type = AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                    c.m_Stat = StatType.Unknown;
                    c.m_SpecificModifier = ModifierDescriptor.None;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_StartLevel = 0;
                    c.m_StepLevel = 0;
                    c.m_UseMax = true;
                    c.m_Max = 20;
                })
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .AllowTargeting(false, false, false, true)
                .Configure();

            var ElementalAssaultAcidBuff = BuffConfigurator.New("SuliElementalAssaultAcidBuff", "{D83F910C-C323-4701-ADDD-E11C92C9914F}")
                .SetDisplayName(ElementalAssaultAcidDisplayName)
                .SetDescription(ElementalAssaultDescription)
                .SetIcon(AbilityRefs.ElementalFistAblilityAcid.Reference.Get().Icon)
                .AdditionalDiceOnAttack(applyCriticalModifier: false, damageType: new DamageTypeDescription()
                {
                    Type = DamageType.Energy,
                    Energy = DamageEnergyType.Acid
                }, attackType: AdditionalDiceOnAttack.WeaponOptions.OnlyWeaponAttacks, onHit: true, value: new ContextDiceValue()
                {
                    DiceType = DiceType.D6,
                    DiceCountValue = 1,
                    BonusValue = 0
                })
                .Configure();

            var ElementalAssaultAcid = AbilityConfigurator.New("SuliElementalAssaultAcid", "{280F6AEE-30BA-4C82-B749-3E48BD09C4DF}")
                .SetDisplayName(ElementalAssaultAcidDisplayName)
                .SetDescription(ElementalAssaultDescription)
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: ElementalAssaultResource)
                .SetIcon(AbilityRefs.ElementalFistAblilityAcid.Reference.Get().Icon)
                .SetRange(AbilityRange.Personal)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(buff: ElementalAssaultAcidBuff, durationValue: new ContextDurationValue()
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
                }, false, isNotDispelable: true).Build())
                .AddComponent<ContextRankConfig>(c =>
                {
                    c.m_Type = AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                    c.m_Stat = StatType.Unknown;
                    c.m_SpecificModifier = ModifierDescriptor.None;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_StartLevel = 0;
                    c.m_StepLevel = 0;
                    c.m_UseMax = true;
                    c.m_Max = 20;
                })
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .AllowTargeting(false, false, false, true)
                .Configure();

            var ElementalAssaultAbility = AbilityConfigurator.New("SuliElementalAssaultAbility", "{0BDE342F-186D-451C-87F6-9DA75A199652}")
                .SetDisplayName(ElementalAssaultDisplayName)
                .SetDescription(ElementalAssaultDescription)
                .SetIcon(AbilityRefs.ElementalFistAblility.Reference.Get().Icon)
                .AddAbilityVariants([ElementalAssaultFire.ToReference<BlueprintAbilityReference>(), ElementalAssaultCold.ToReference<BlueprintAbilityReference>(), ElementalAssaultElectricity.ToReference<BlueprintAbilityReference>(), ElementalAssaultAcid.ToReference<BlueprintAbilityReference>()])
                .Configure();
            
            var ElementalAssault = FeatureConfigurator.New("SuliElementalAssault", "{1ABD4718-A6B4-4A12-88B3-78FC3883A061}")
                .SetDisplayName(ElementalAssaultDisplayName)
                .SetDescription(ElementalAssaultDescription)
                .SetIcon(AbilityRefs.ElementalFistAblility.Reference.Get().Icon)
                .AddFacts([ElementalAssaultAbility])
                .AddAbilityResources(1, ElementalAssaultResource, true)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var race =
            RaceConfigurator.New(SuliName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(SuliDisplayName)
                .SetDescription(SuliDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(EnergyResistance, Negotiator, ElementalAssault, FeatureRefs.KeenSenses.ToString())
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: -2)
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, eyecolors: RaceEyeColors, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.OreadRace.ToString()), CustomFemaleHairs: NewFemaleHairArray);

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
