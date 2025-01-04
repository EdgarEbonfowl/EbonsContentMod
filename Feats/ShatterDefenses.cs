using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using MicroscopicContentExpansion.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
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
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.EntitySystem.Stats;
using TabletopTweaks.Core.Utilities;
using BlueprintCore.Utils.Types;
using BlueprintCore.Actions.Builder.ContextEx;
using TabletopTweaks.Core.NewComponents.OwlcatReplacements;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using EbonsContentMod.Utilities;
using BlueprintCore.Conditions.Builder.BasicEx;
using Kingmaker.Designers.EventConditionActionSystem.Evaluators;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder.KingdomEx;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Blueprints.Root;
using TabletopTweaks.Core.NewComponents;

namespace EbonsContentMod.Feats
{
    internal class ShatterDefenses
    {
    
        internal static void Configure()
        {
            // Here we apply the flat-footed buff BEFORE the attack if the target is shaken/frightened
            var actions1 = ActionFlow.DoSingle<Conditional>(ac =>
            {
                ac.ConditionsChecker = ActionFlow.IfAny(
                    new ContextConditionHasFact() // If target is shaken or frightened
                    {
                        m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("c4648cecc52c32945a5748098a2b9b32"), // Frightened
                        Not = false
                    },
                    new ContextConditionHasFact()
                    {
                        m_Fact = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("54754f00bb628d547a089d7c94ee3c68"), // Shaken
                        Not = false
                    }
                );
                ac.IfTrue = ActionFlow.DoSingle<ContextActionApplyBuff>(bc =>
                {
                    bc.m_Buff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("49d6db4e19684eddb14ed4db90f8884b"); // Shatter Defenses Flat-Footed Buff
                    bc.AsChild = true;
                });
                ac.IfFalse = ActionFlow.DoNothing();
            });

            // Here we remove the flat-footed buff AFTER EVERY attack so that the character remains flat-footed only against that attacker
            var actions2 = ActionFlow.DoSingle<Conditional>(ac =>
            {
                ac.ConditionsChecker = ActionFlow.IfAll(
                    new ContextConditionHasBuffFromCaster() // If target has the flat-footed buff
                    {
                        m_Buff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("49d6db4e19684eddb14ed4db90f8884b"), // Shatter Defenses Flat-Footed Buff
                        Not = false
                    }
                );
                ac.IfTrue = ActionFlow.DoSingle<ContextActionRemoveBuff>(bc =>
                {
                    bc.m_Buff = BlueprintTools.GetBlueprintReference<BlueprintBuffReference>("49d6db4e19684eddb14ed4db90f8884b"); // Shatter Defenses Flat-Footed Buff
                    bc.OnlyFromCaster = true;
                });
                ac.IfFalse = ActionFlow.DoNothing();
            });

            FeatureConfigurator.For(FeatureRefs.ShatterDefenses)
                .RemoveComponents(c => c is AddInitiatorAttackWithWeaponTrigger)
                .AddComponent<AddInitiatorAttackWithWeaponTrigger>(c =>
                {
                    c.TriggerBeforeAttack = true;
                    c.OnlyHit = true;
                    c.RangeType = WeaponRangeType.Melee;
                    c.Action = actions1;
                })
                .AddComponent<AddInitiatorAttackWithWeaponTrigger>(c =>
                {
                    c.TriggerBeforeAttack = false;
                    c.OnlyHit = false;
                    c.RangeType = WeaponRangeType.Melee;
                    c.Action = actions2;
                })
                .Configure();
        }
    
    }
}
