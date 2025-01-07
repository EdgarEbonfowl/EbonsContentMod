using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Owlcat.QA.Validation;
using UnityEngine;

namespace EbonsContentMod.Components
{
    [AllowMultipleComponents]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("85af7290b5fa5d943968ea17ae8c3b27")]
    public class DuergarImmunitiesComponent : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSavingThrow>, IRulebookHandler<RuleSavingThrow>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleSavingThrow evt)
        {
            MechanicsContext context = evt.Reason.Context;
            AbilityData ability = evt.Reason.Ability;
            BlueprintAbility blueprintAbility = ((ability != null) ? ability.Blueprint.Or(null) : null) ?? ((context != null) ? context.SourceAbility : null);
            if (blueprintAbility != null && blueprintAbility.IsSpell)
            {
                EntityFact source = base.Fact;
                int bonus = 2;
                evt.AddModifier(bonus, source, ModifierDescriptor.Racial);
            }
        }

        public void OnEventDidTrigger(RuleSavingThrow evt)
        {
        }
    }
}

