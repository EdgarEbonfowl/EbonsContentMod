using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonsContentMod.Components
{
    internal class OrcBloodlineArcana : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            MechanicsContext context = evt.Reason.Context;
            if ((context?.SourceAbility) == null)
            {
                return;
            }
            if ((!context.SourceAbility.IsSpell && this.SpellsOnly) || context.SourceAbility.Type == AbilityType.Physical)
            {
                return;
            }
            foreach (BaseDamage baseDamage in evt.DamageBundle)
            {
                if (!baseDamage.Precision)
                {
                    DiceFormula modifiedValue = baseDamage.Dice.ModifiedValue;
                    int bonus = this.UseContextBonus ? (this.Value.Calculate(context) * modifiedValue.Rolls) : modifiedValue.Rolls;
                    baseDamage.AddModifier(bonus, base.Fact);
                }
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt)
        {
        }

        public bool SpellsOnly = true;

        public bool UseContextBonus;

        [ShowIf("UseContextBonus")]
        public ContextValue Value;
    }
}
