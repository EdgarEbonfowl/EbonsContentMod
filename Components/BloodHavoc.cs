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
using EbonsContentMod.Utilities;

namespace EbonsContentMod.Components
{
    internal class BloodHavoc : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            MechanicsContext context = evt.Reason.Context;
            if ((context?.SourceAbility) == null)
            {
                return;
            }
            if ((!context.SourceAbility.IsSpell) || context.SourceAbility.Type == AbilityType.Physical)
            {
                return;
            }
            if (!ComponentHelperators.GetHasFocusForSpell(context.SourceAbility, context.MaybeCaster) && !ComponentHelperators.GetIsBloodlineSpell(context.SourceAbility, context.MaybeCaster))
            {
                return;
            }
            foreach (BaseDamage baseDamage in evt.DamageBundle)
            {
                if (!baseDamage.Precision)
                {
                    DiceFormula modifiedValue = baseDamage.Dice.ModifiedValue;
                    int bonus = modifiedValue.Rolls;
                    baseDamage.AddModifier(bonus, base.Fact);
                }
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt)
        {
        }
    }
}
