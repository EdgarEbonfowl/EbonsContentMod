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
    internal class AddEnergyDamageToSpellsByDescriptor : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            MechanicsContext context = evt.Reason.Context;
            if (((context != null) ? context.SourceAbility : null) == null)
            {
                return;
            }
            if (!context.SpellDescriptor.HasAnyFlag(this.SpellDescriptor) || (!context.SourceAbility.IsSpell && this.SpellsOnly) || context.SourceAbility.Type == AbilityType.Physical)
            {
                return;
            }
            foreach (BaseDamage baseDamage in evt.DamageBundle)
            {
                if (!baseDamage.Precision)
                {
                    DiceFormula modifiedValue = baseDamage.Dice.ModifiedValue;
                    int bonus = this.UseContextBonus ? this.Value.Calculate(context) : modifiedValue.Rolls;
                    baseDamage.AddModifier(bonus, base.Fact);
                }
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt)
        {
        }

        public SpellDescriptorWrapper SpellDescriptor;

        public bool SpellsOnly = true;

        public bool UseContextBonus;

        [ShowIf("UseContextBonus")]
        public ContextValue Value;
    }

    internal class PyrophileDamageBonus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateDamage>, IRulebookHandler<RuleCalculateDamage>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCalculateDamage evt)
        {
            MechanicsContext context = evt.Reason.Context;
            if (((context != null) ? context.SourceAbility : null) == null)
            {
                return;
            }
            if (!context.SpellDescriptor.HasAnyFlag(this.SpellDescriptor) || (!context.SourceAbility.IsSpell && this.SpellsOnly) || context.SourceAbility.Type == AbilityType.Physical)
            {
                return;
            }
            foreach (BaseDamage baseDamage in evt.DamageBundle)
            {
                if (!baseDamage.Precision)
                {
                    int bonus = 1 + (evt.Initiator.Progression.CharacterLevel - 1) / 4;
                    baseDamage.AddModifier(bonus, base.Fact);
                }
            }
        }

        public void OnEventDidTrigger(RuleCalculateDamage evt)
        {
        }

        public SpellDescriptorWrapper SpellDescriptor;

        public bool SpellsOnly = true;
    }
}
