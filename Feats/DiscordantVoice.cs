using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Classes.Selection;
using BlueprintCore.Blueprints.Configurators.Classes.Spells;
using BlueprintCore.Blueprints.Configurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Spells;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums.Damage;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.ActivatableAbilities.Restrictions;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonsContentMod.Feats
{
    internal class DiscordantVoice
    {
        private const string DiscordantVoiceDescription = "DiscordantVoice.Description";

        internal static void Configure()
        {
            FeatureConfigurator.For(FeatureRefs.DiscordantVoice)
                .SetDescription(DiscordantVoiceDescription)
                .Configure();
            
            AbilityAreaEffectConfigurator.For(AbilityAreaEffectRefs.FascinateArea)
                .AddAbilityAreaEffectBuff(BuffRefs.DiscordantVoiceBuff.ToString(), 
                    checkConditionEveryRound: false, 
                    condition: ConditionsBuilder.New().IsAlly().CasterHasFact(FeatureRefs.DiscordantVoice.ToString()))
                .Configure();

            AbilityAreaEffectConfigurator.For(AbilityAreaEffectRefs.DirgeOfDoomArea)
                .AddAbilityAreaEffectBuff(BuffRefs.DiscordantVoiceBuff.ToString(),
                    checkConditionEveryRound: false,
                    condition: ConditionsBuilder.New().IsAlly().CasterHasFact(FeatureRefs.DiscordantVoice.ToString()))
                .Configure();

            AbilityAreaEffectConfigurator.For(AbilityAreaEffectRefs.FrighteningTuneArea)
                .AddAbilityAreaEffectBuff(BuffRefs.DiscordantVoiceBuff.ToString(),
                    checkConditionEveryRound: false,
                    condition: ConditionsBuilder.New().IsAlly().CasterHasFact(FeatureRefs.DiscordantVoice.ToString()))
                .Configure();

            AbilityAreaEffectConfigurator.For(AbilityAreaEffectRefs.StormCallArea)
                .AddAbilityAreaEffectBuff(BuffRefs.DiscordantVoiceBuff.ToString(),
                    checkConditionEveryRound: false,
                    condition: ConditionsBuilder.New().IsAlly().CasterHasFact(FeatureRefs.DiscordantVoice.ToString()))
                .Configure();

            AbilityAreaEffectConfigurator.For(AbilityAreaEffectRefs.TragedyOfFalseHopeArea)
                .AddAbilityAreaEffectBuff(BuffRefs.DiscordantVoiceBuff.ToString(),
                    checkConditionEveryRound: false,
                    condition: ConditionsBuilder.New().IsAlly().CasterHasFact(FeatureRefs.DiscordantVoice.ToString()))
                .Configure();

            AbilityAreaEffectConfigurator.For(AbilityAreaEffectRefs.BlazingRondoAreaEffect)
                .AddAbilityAreaEffectBuff(BuffRefs.DiscordantVoiceBuff.ToString(),
                    checkConditionEveryRound: false,
                    condition: ConditionsBuilder.New().IsAlly().CasterHasFact(FeatureRefs.DiscordantVoice.ToString()))
                .Configure();

            AbilityAreaEffectConfigurator.For(AbilityAreaEffectRefs.BansheesRequiemAreaEffect)
                .AddAbilityAreaEffectBuff(BuffRefs.DiscordantVoiceBuff.ToString(),
                    checkConditionEveryRound: false,
                    condition: ConditionsBuilder.New().IsAlly().CasterHasFact(FeatureRefs.DiscordantVoice.ToString()))
                .Configure();
        }
    }
}
