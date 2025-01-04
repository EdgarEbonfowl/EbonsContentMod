using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UnitLogic.ActivatableAbilities.Restrictions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonsContentMod.Abilities
{
    internal class ComeAndGetMe
    {
		internal static void Configure()
		{
            ActivatableAbilityConfigurator.For(ActivatableAbilityRefs.ComeAndGetMeToggleAbility)
				.RemoveComponents(c => c is RestrictionHasFact)
				.SetDoNotTurnOffOnRest()
				.Configure();

			BuffConfigurator.For(BuffRefs.ComeAndGetMeSwitchBuff)
				.AddFactContextActions(
					activated:
						ActionsBuilder.New()
							.Conditional(
								ConditionsBuilder.New().HasFact(BuffRefs.InspiredRageEffectBuffMythic.ToString()),
								ifTrue: ActionsBuilder.New().ApplyBuffPermanent(BuffRefs.ComeAndGetMeEffectBuff.ToString(), isNotDispelable: true)))
				.Configure();
		}
	}
}
