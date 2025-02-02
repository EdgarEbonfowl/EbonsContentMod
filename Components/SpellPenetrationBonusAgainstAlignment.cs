using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;

namespace EbonsContentMod.Components
{
    public class SpellPenetrationBonusAgainstAlignment : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSpellResistanceCheck>, IRulebookHandler<RuleSpellResistanceCheck>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public void OnEventAboutToTrigger(RuleSpellResistanceCheck evt)
        {
            if (evt.Target.Descriptor.Alignment.ValueRaw.HasComponent(this.Alignment))
            {
                int bonus = this.Value.Calculate(base.Context);
                evt.AddSpellPenetration(bonus, this.Descriptor);
            }
        }

        public void OnEventDidTrigger(RuleSpellResistanceCheck evt)
        {
        }

        public ContextValue Value;

        public AlignmentComponent Alignment;

        public ModifierDescriptor Descriptor = ModifierDescriptor.UntypedStackable;
    }
}
