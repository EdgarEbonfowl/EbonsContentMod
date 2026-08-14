using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
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
    internal class IncreaseAllSpellsDCAgainstAlignment : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAbilityParams>, IRulebookHandler<RuleCalculateAbilityParams>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleCalculateAbilityParams evt)
        {
            if (this.SpellsOnly && evt.Spellbook == null)
            {
                return;
            }
            evt.AddBonusDC(this.Value.Calculate(base.Context), this.Descriptor);
        }

        public void OnEventDidTrigger(RuleCalculateAbilityParams evt)
        {
        }

        public ContextValue Value;

        public AlignmentComponent Alignment;

        public ModifierDescriptor Descriptor = ModifierDescriptor.UntypedStackable;

        [InfoBox("Turn on to increase DC only for spells from spellbook")]
        public bool SpellsOnly;
    }
}
