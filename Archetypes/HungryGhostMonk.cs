using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Classes.Selection;
using BlueprintCore.Blueprints.Configurators.Classes.Spells;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Spells;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.EntitySystem.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using MicroscopicContentExpansion.Utils;
using BlueprintCore.Utils.Types;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using EbonsContentMod.Utilities;
using EbonsContentMod.Components;
using static EbonsContentMod.Utilities.ActivatableAbilityGroupUtilities;
using BlueprintCore.Conditions.Builder.BasicEx;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder.KingdomEx;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Blueprints.Root;
using TabletopTweaks.Core.NewComponents;

namespace EbonsContentMod.Archetypes
{
    internal class HungryGhostMonk
    {
		// List the strings and keys
		private const string ArchetypeName = "HungryGhostMonk";

		internal const string ArchetypeDisplayName = "HungryGhostMonk.Name";
		private const string ArchetypeDescription = "HungryGhostMonk.Description";
		public static readonly string ArchetypeGuid = "{BE75DAC8-EB2E-4BCF-8139-1819076C4A5E}";

        private const string StealKi = "HungryGhostMonk.StealKi";
        internal const string StealKiName = "HungryGhostMonk.StealKi.Name";
		private const string StealKiDescription = "HungryGhostMonk.StealKi.Description";
		public static readonly string StealKiGuid = "{A16550D3-BA99-4FF9-80FF-674CA42668B8}";

        private const string StealKiToggleAbility = "HungryGhostMonk.StealKiToggleAbility";
        public static readonly string StealKiToggleAbilityGuid = "{D54CBC11-01AE-4B16-BD67-FF8E5E5353DF}";

        private const string StealKiSwitchBuff = "HungryGhostMonk.StealKiSwitchBuff";
        public static readonly string StealKiSwitchBuffGuid = "{94FCE1C6-44D3-497A-A463-2189F0DB8AF9}";

        private const string LifeFunnel = "HungryGhostMonk.LifeFunnel";
        internal const string LifeFunnelName = "HungryGhostMonk.LifeFunnel.Name";
		private const string LifeFunnelDescription = "HungryGhostMonk.LifeFunnel.Description";
		public static readonly string LifeFunnelGuid = "{E19F89E9-0AC9-4355-97F1-4670DA6F2DF8}";

        private const string LifeFunnelToggleAbility = "HungryGhostMonk.LifeFunnelToggleAbility";
        public static readonly string LifeFunnelToggleAbilityGuid = "{D93C7C72-7387-46C8-9C31-5C1331CA475D}";

        private const string LifeFunnelSwitchBuff = "HungryGhostMonk.LifeFunnelSwitchBuff";
        public static readonly string LifeFunnelSwitchBuffGuid = "{7B26BC5E-DFA1-496C-B951-30AE29E537CA}";

        private const string LifeFromAStone = "HungryGhostMonk.LifeFromAStone";
        internal const string LifeFromAStoneName = "HungryGhostMonk.LifeFromAStone.Name";
		private const string LifeFromAStoneDescription = "HungryGhostMonk.LifeFromAStone.Description";
		public static readonly string LifeFromAStoneGuid = "{8564EBBA-71A1-47EC-A0B1-B1CB7ED819A7}";

		private const string SippingDemon = "HungryGhostMonk.SippingDemon";
        internal const string SippingDemonName = "HungryGhostMonk.SippingDemon.Name";
		private const string SippingDemonDescription = "HungryGhostMonk.SippingDemon.Description";
		public static readonly string SippingDemonGuid = "{3C747D48-1166-447F-BA4F-5D5C9387E103}";

        private const string SippingDemonTempHPBuffOne = "HungryGhostMonk.SippingDemonTempHPBuffOne";
        public static readonly string SippingDemonTempHPBuffOneGuid = "{7C19F9FA-954C-4A97-A1DF-EB1D8E1DC062}";

        private const string SippingDemonTempHPBuffTwo = "HungryGhostMonk.SippingDemonTempHPBuffTwo";
        public static readonly string SippingDemonTempHPBuffTwoGuid = "{DD7C4C80-0101-4F3D-B31C-D2C8ABEDE044}";

        internal static void Configure()
		{
            // Make Sipping Demon buffs
            var icon = AbilityRefs.ConsumeFear.Reference.Get().Icon;

            BuffConfigurator.New(SippingDemonTempHPBuffOne, SippingDemonTempHPBuffOneGuid)
                .SetDisplayName(SippingDemonName)
                .SetDescription(SippingDemonDescription)
                .SetIcon(icon)
                .AddTemporaryHitPointsFromAbilityValue(removeWhenHitPointsEnd: true, value: 1)
                .SetStacking(StackingType.Replace)
                .Configure();

            BuffConfigurator.New(SippingDemonTempHPBuffTwo, SippingDemonTempHPBuffTwoGuid)
                .SetDisplayName(SippingDemonName)
                .SetDescription(SippingDemonDescription)
                .SetIcon(icon)
                .AddComponent<SippingDemonAddTemporaryHP>()
                .SetStacking(StackingType.Replace)
                .Configure();

            // Start Archetype Here
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.MonkClass)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)

                // Remove features
                .AddToRemoveFeatures(5, FeatureRefs.PurityOfBody.ToString())
                .AddToRemoveFeatures(6, FeatureSelectionRefs.MonkKiPowerSelection.ToString())
                .AddToRemoveFeatures(10, FeatureSelectionRefs.MonkKiPowerSelection.ToString())
                .AddToRemoveFeatures(12, FeatureSelectionRefs.MonkKiPowerSelection.ToString())

                // Add features
                .AddToAddFeatures(5, CreateStealKi())
                .AddToAddFeatures(6, CreateLifeFunnel())
                .AddToAddFeatures(10, CreateLifeFromAStone())
                .AddToAddFeatures(12, CreateSippingDemon())

                // Make it so!
                .Configure();
		}

        // Steal Ki
        private static BlueprintFeature CreateStealKi()
		{
			var icon = AbilityRefs.RayOfSickening.Reference.Get().Icon;

            var actions = ActionFlow.DoSingle<Conditional>(ac =>
            {
                ac.ConditionsChecker = ActionFlow.IfAll(
                    new ContextConditionCasterHasFact() // If caster has Life From A Stone
                    {
                        m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("8564EBBA-71A1-47EC-A0B1-B1CB7ED819A7"),
                        Not = false
                    },
                    new ContextConditionCasterHasResource()
                    {
                        m_Resource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("9d9c90a9a1f52d04799294bf91c80a82"),
                    }
                );
                ac.IfTrue = ActionFlow.DoSingle<ContextActionOnContextCaster>(bc =>
                {
                    bc.Actions = ActionFlow.DoSingle<ContextRestoreResource>(cc =>
                    {
                        cc.m_IsFullRestoreAllResources = false;
                        cc.m_Resource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("9d9c90a9a1f52d04799294bf91c80a82");
                        cc.Value = 1;
                    });
                });
                ac.IfFalse = ActionFlow.DoSingle<Conditional>(xc =>
                {
                    xc.ConditionsChecker = ActionFlow.IfAll( //not undead, not construct
                        new ContextConditionHasFact()
                        {
                            m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("734a29b693e9ec346ba2951b27987e33"),
                            Not = true,
                        },
                        new ContextConditionHasFact()
                        {
                            m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("fd389783027d63343b4a5634bd81645f"),
                            Not = true
                        },
                        new ContextConditionCasterHasResource()
                        {
                            m_Resource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("9d9c90a9a1f52d04799294bf91c80a82"),
                        }
                    );
                    xc.IfTrue = ActionFlow.DoSingle<ContextActionOnContextCaster>(yc =>
                    {
                        yc.Actions = ActionFlow.DoSingle<ContextRestoreResource>(zc =>
                        {
                            zc.m_IsFullRestoreAllResources = false;
                            zc.m_Resource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("9d9c90a9a1f52d04799294bf91c80a82");
                            zc.Value = 1;
                        });
                    });
                    xc.IfFalse = ActionFlow.DoNothing();
                });
            });

            var actions2 = ActionFlow.DoSingle<Conditional>(ac =>
            {
                ac.ConditionsChecker = ActionFlow.IfAll(
                    new ContextConditionCasterHasFact() // If caster has Life From A Stone
                    {
                        m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("8564EBBA-71A1-47EC-A0B1-B1CB7ED819A7"),
                        Not = false
                    },
                    new ContextConditionCasterHasResource()
                    {
                        m_Resource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("9d9c90a9a1f52d04799294bf91c80a82"),
                    }
                );
                ac.IfTrue = ActionFlow.DoSingle<ContextActionOnContextCaster>(bc =>
                {
                    bc.Actions = ActionFlow.DoSingle<ContextActionDispelMagic>(cc =>
                    {
                        cc.m_StopAfterCountRemoved = true;
                        cc.m_CountToRemove = 1;
                        cc.m_BuffType = ContextActionDispelMagic.BuffType.All;
                        cc.m_CheckType = Kingmaker.RuleSystem.Rules.RuleDispelMagic.CheckType.DC;
                        cc.m_Skill = StatType.Unknown;
                        cc.m_MaxSpellLevel = 9;
                        cc.m_UseMaxCasterLevel = false;
                        cc.m_MaxCasterLevel = new ContextValue()
                        {
                            ValueType = ContextValueType.Simple,
                            Value = 0,
                            ValueRank = AbilityRankType.Default,
                            ValueShared = AbilitySharedValue.Damage,
                            Property = UnitProperty.None
                        };
                        cc.Descriptor = SpellDescriptor.Disease;
                        cc.CheckBonus = 0;
                        cc.ContextBonus = ContextValues.Rank();
                        cc.Descriptor = SpellDescriptor.Staggered;
                        cc.OnSuccess = Helpers.CreateActionList();
                        cc.OnFail = Helpers.CreateActionList();
                        cc.OnlyTargetEnemyBuffs = false;
                        cc.CheckSchoolOrDescriptor = false;
                    });
                });
                ac.IfFalse = ActionFlow.DoNothing();
            });

            var buff = BuffConfigurator.New(StealKiSwitchBuff, StealKiSwitchBuffGuid)
              .SetDisplayName(StealKiName)
              .SetDescription(StealKiDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.Wisdom))
              .AddComponent<AddInitiatorAttackWithWeaponTrigger>(c =>
              {
                  c.OnlyHit = true;
                  c.CriticalHit = true;
                  c.Action = actions;
              })
			  .AddComponent<AddInitiatorAttackWithWeaponTrigger>(c =>
			  {
				  c.OnlyHit = true;
				  c.NotCriticalHit = true;
				  c.ReduceHPToZero = true;
				  c.Action = actions;
			  })
              .AddComponent<AddInitiatorAttackWithWeaponTrigger>(c =>
              {
                  c.OnlyHit = true;
                  c.CriticalHit = true;
                  c.Action = actions2;
              })
              .AddComponent<AddInitiatorAttackWithWeaponTrigger>(c =>
              {
                  c.OnlyHit = true;
                  c.NotCriticalHit = true;
                  c.ReduceHPToZero = true;
                  c.Action = actions2;
              })
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(StealKiToggleAbility, StealKiToggleAbilityGuid)
              .SetDisplayName(StealKiName)
              .SetDescription(StealKiDescription)
              .SetIcon(icon)
			  .SetBuff(buff)
              .SetGroup((ActivatableAbilityGroup)ECActivatableAbilityGroup.HungryGhostMonkAbilities)
              .SetActivationType(AbilityActivationType.Immediately)
              .SetDeactivateImmediately()
              .Configure();

            return FeatureConfigurator.New(StealKi, StealKiGuid)
			  .SetDisplayName(StealKiName)
			  .SetDescription(StealKiDescription)
			  .SetIcon(icon)
			  .SetIsClassFeature(true)
              .AddFacts(new() { ability })
              .Configure();
		}

        // Life Funnel
        private static BlueprintFeature CreateLifeFunnel()
		{
			var icon = AbilityRefs.WrackingRay.Reference.Get().Icon;

            var actions = ActionFlow.DoSingle<Conditional>(ac =>
            {
                ac.ConditionsChecker = ActionFlow.IfAll(
                    new ContextConditionCasterHasFact() // If caster has Life From A Stone and at least one ki point left
                    {
                        m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("8564EBBA-71A1-47EC-A0B1-B1CB7ED819A7"),
                        Not = false
                    },
                    new ContextConditionCasterHasResource()
                    {
                        m_Resource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("9d9c90a9a1f52d04799294bf91c80a82"),
                    }
                );
                ac.IfTrue = ActionFlow.DoSingle<ContextActionOnContextCaster>(bc =>
                {
                    bc.Actions = ActionFlow.DoSingle<ContextActionHealTarget>(cc => 
                    {
                        cc.Value = new ContextDiceValue()
                        {
                            DiceType = DiceType.Zero,
                            DiceCountValue = 0,
                            BonusValue = ContextValues.Rank()
                        };
                    });
                });
                ac.IfFalse = ActionFlow.DoSingle<Conditional>(xc =>
                {
                    xc.ConditionsChecker = ActionFlow.IfAll( //not undead, not construct, at least one ki point left
                        new ContextConditionHasFact()
                        {
                            m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("734a29b693e9ec346ba2951b27987e33"),
                            Not = true,
                        },
                        new ContextConditionHasFact()
                        {
                            m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("fd389783027d63343b4a5634bd81645f"),
                            Not = true
                        },
                        new ContextConditionCasterHasResource()
                        {
                            m_Resource = BlueprintTools.GetBlueprintReference<BlueprintAbilityResourceReference>("9d9c90a9a1f52d04799294bf91c80a82"),
                        }
                    );
                    xc.IfTrue = ActionFlow.DoSingle<ContextActionOnContextCaster>(yc =>
                    {
                        yc.Actions = ActionFlow.DoSingle<ContextActionHealTarget>(cc => 
                        {
                            cc.Value = new ContextDiceValue()
                            {
                                DiceType = DiceType.Zero,
                                DiceCountValue = 0,
                                BonusValue = ContextValues.Rank()
                            };
                        });
                    });
                    xc.IfFalse = ActionFlow.DoNothing();
                });
            });

            var buff = BuffConfigurator.New(LifeFunnelSwitchBuff, LifeFunnelSwitchBuffGuid)
              .SetDisplayName(LifeFunnelName)
              .SetDescription(LifeFunnelDescription)
              .SetIcon(icon)
              .SetIsClassFeature(true)
              .SetFlags(BlueprintBuff.Flags.HiddenInUi)
              .AddComponent<AddInitiatorAttackWithWeaponTrigger>(c =>
              {
                  c.OnlyHit = true;
                  c.CriticalHit = true;
                  c.Action = actions;
              })
              .AddComponent<AddInitiatorAttackWithWeaponTrigger>(c =>
              {
                  c.OnlyHit = true;
                  c.NotCriticalHit = true;
                  c.ReduceHPToZero = true;
                  c.Action = actions;
              })
              .AddContextRankConfig(ContextRankConfigs.ClassLevel([CharacterClassRefs.MonkClass.ToString()]))
              .Configure();

            var ability = ActivatableAbilityConfigurator.New(LifeFunnelToggleAbility, LifeFunnelToggleAbilityGuid)
              .SetDisplayName(LifeFunnelName)
              .SetDescription(LifeFunnelDescription)
              .SetIcon(icon)
              .SetBuff(buff)
              .SetGroup((ActivatableAbilityGroup)ECActivatableAbilityGroup.HungryGhostMonkAbilities)
              .SetActivationType(AbilityActivationType.Immediately)
              .SetDeactivateImmediately()
              .Configure();

            return FeatureConfigurator.New(LifeFunnel, LifeFunnelGuid)
			  .SetDisplayName(LifeFunnelName)
			  .SetDescription(LifeFunnelDescription)
			  .SetIcon(icon)
			  .SetIsClassFeature(true)
              .AddFacts(new() { ability })
			  .Configure();
		}

        // Life From A Stone
		private static BlueprintFeature CreateLifeFromAStone()
		{
			var icon = AbilityRefs.ConsumeUndead.Reference.Get().Icon;

			return FeatureConfigurator.New(LifeFromAStone, LifeFromAStoneGuid)
			  .SetDisplayName(LifeFromAStoneName)
			  .SetDescription(LifeFromAStoneDescription)
			  .SetIcon(icon)
			  .SetIsClassFeature(true)
			  .Configure();
		}
        
        // Sipping Demon
		private static BlueprintFeature CreateSippingDemon()
		{
			var icon = AbilityRefs.ConsumeFear.Reference.Get().Icon;

            return FeatureConfigurator.New(SippingDemon, SippingDemonGuid)
			  .SetDisplayName(SippingDemonName)
			  .SetDescription(SippingDemonDescription)
			  .SetIcon(icon)
			  .SetIsClassFeature(true)
              .AddComponent<SippingDemonTempHP>()
              .Configure();
		}

	}

}
