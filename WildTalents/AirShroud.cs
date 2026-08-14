using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using EbonsContentMod.Components;
using EbonsContentMod.Feats;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EbonsContentMod.WildTalents
{
    internal class AirShroud
    {
        private const string AirShroudNameKey =
            "EbonsContentMod.AirShroud.Name";

        private const string AirShroudDescriptionKey =
            "EbonsContentMod.AirShroud.Description";

        private const string GreaterAirShroudNameKey =
            "EbonsContentMod.GreaterAirShroud.Name";

        private const string GreaterAirShroudDescriptionKey =
            "EbonsContentMod.GreaterAirShroud.Description";

        private static Sprite AirShroudIcon = AbilityRefs.LifeBubble.Reference.Get().Icon;

        internal static void Configure()
        {
            // Buffs

            var airShroudBuff = BuffConfigurator.New("AirShroudBuff", "{E86A33AF-E217-496D-8FE2-AE9171B81846}")
                .CopyFrom(BuffRefs.LifeBubbleBuff)
                .SetDisplayName(AirShroudNameKey)
                .SetDescription(AirShroudDescriptionKey)
                .SetIcon(AirShroudIcon)
                .Configure();

            // Abilities

            /*var greaterAirShroudAbility = AbilityConfigurator.New("GreaterAirShroudAbility", "{E27C5CA0-A4CF-4948-80ED-F85DA507E133}")
                .SetDisplayName(GreaterAirShroudNameKey)
                .SetDescription(GreaterAirShroudDescriptionKey)
                .SetIcon(AirShroudIcon)
                .SetType(AbilityType.SpellLike)
                .SetRange(AbilityRange.Personal)
                .SetActionType(UnitCommand.CommandType.Standard)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Kineticist)
                .AddAbilityTargetsAround(
                    radius: 30.Feet(),
                    targetType: TargetType.Ally,
                    includeDead: false)
                .AddAbilityEffectRunAction(
                    actions: ActionsBuilder
                        .New()
                        .Conditional(
                            ConditionsBuilder.New().IsAlly().IsCaster(negate: true),
                            ifTrue: ActionsBuilder.New().ApplyBuffPermanent(airShroudBuff)))
                .AddAbilityKineticist(
                    infusionBurnCost: 0,
                    blastBurnCost: 0,
                    wildTalentBurnCost: 1)
                .Configure();*/

            var airShroudAbility = AbilityConfigurator.New("AirShroudAbility", "{BD4E7949-FA20-455E-8BD5-DA8097B88ECF}")
                .SetDisplayName(AirShroudNameKey)
                .SetDescription(AirShroudDescriptionKey)
                .SetIcon(AirShroudIcon)
                .SetType(AbilityType.SpellLike)
                .SetRange(AbilityRange.Personal)
                .SetActionType(UnitCommand.CommandType.Standard)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Kineticist)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel([CharacterClassRefs.KineticistClass.ToString()]))
                .AddAbilityTargetsAround(
                    radius: 30.Feet(),
                    targetType: TargetType.Ally,
                    includeDead: false)
                .AddAbilityEffectRunAction(
                    actions: ActionsBuilder
                        .New()
                        .Conditional(
                            ConditionsBuilder.New()
                                .IsAlly()
                                .IsCaster(negate: true),
                            ifTrue: ActionsBuilder.New()
                                .Conditional(
                                    ConditionsBuilder.New()
                                        .CasterHasFact(
                                            "{5C45A3EB-95BC-484A-99EE-5BEC1A8607E7}"),
                                    ifTrue: ActionsBuilder.New()
                                        .ApplyBuffPermanent(
                                            airShroudBuff),
                                    ifFalse: ActionsBuilder.New()
                                        .ApplyBuff(
                                            airShroudBuff,
                                            durationValue:
                                                new ContextDurationValue()
                                                {
                                                    Rate = DurationRate.Minutes,
                                                    DiceType = DiceType.Zero,

                                                    DiceCountValue =
                                                        new ContextValue()
                                                        {
                                                            ValueType =
                                                                ContextValueType.Simple,
                                                            Value = 0
                                                        },

                                                    BonusValue =
                                                        new ContextValue()
                                                        {
                                                            ValueType =
                                                                ContextValueType.Rank
                                                        }
                                                }))))
                .AddAbilityKineticist(
                    infusionBurnCost: 0,
                    blastBurnCost: 0,
                    wildTalentBurnCost: 1)
                .Configure();

            // Features

            var airShroud = FeatureConfigurator.New("AirShroudFeature", "{FEE93D9D-4306-4DD0-9A78-EB0291D113AF}", FeatureGroup.KineticWildTalent)
                .SetDisplayName(AirShroudNameKey)
                .SetDescription(AirShroudDescriptionKey)
                .SetIcon(AirShroudIcon)
                .SetIsClassFeature()
                .AddPrerequisiteFeaturesFromList(
                [
                    ProgressionRefs.ElementalFocusAir.ToString(),
                    ProgressionRefs.SecondaryElementAir.ToString(),
                    ProgressionRefs.ThirdElementAir.ToString(),
                    ProgressionRefs.KineticKnightElementalFocusAir.ToString(),
                    FeatureRefs.AirBlastFeature.ToString(),
                    FeatureRefs.ElectricBlastFeature.ToString()
                ])
                .AddPrerequisiteClassLevel(CharacterClassRefs.KineticistClass.ToString(), 1)
                .AddFacts([airShroudBuff, airShroudAbility])
                .Configure();

            FeatureConfigurator.New("GreaterAirShroudFeature", "{5C45A3EB-95BC-484A-99EE-5BEC1A8607E7}", FeatureGroup.KineticWildTalent)
                .SetDisplayName(GreaterAirShroudNameKey)
                .SetDescription(GreaterAirShroudDescriptionKey)
                .SetIcon(AirShroudIcon)
                .SetIsClassFeature()
                .AddPrerequisiteFeature(airShroud)
                .AddPrerequisiteFeaturesFromList(
                [
                    ProgressionRefs.ElementalFocusAir.ToString(),
                    ProgressionRefs.SecondaryElementAir.ToString(),
                    ProgressionRefs.ThirdElementAir.ToString(),
                    ProgressionRefs.KineticKnightElementalFocusAir.ToString(),
                    FeatureRefs.AirBlastFeature.ToString(),
                    FeatureRefs.ElectricBlastFeature.ToString()
                ])
                .AddPrerequisiteClassLevel(CharacterClassRefs.KineticistClass.ToString(), 10)
                .AddComponent<RemoveGrantedBuffOnRest>(c =>
                {
                    c.m_Buff =
                        airShroudBuff
                            .ToReference<BlueprintBuffReference>();
                })
                .Configure();
        }
    }
}
