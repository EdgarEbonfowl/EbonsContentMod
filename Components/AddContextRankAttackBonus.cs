using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Blueprints.Area.FactHolder;

namespace EbonsContentMod.Components
{
    internal class AddContextRankAttackBonus : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public ContextValue Value;
        public ModifierDescriptor Descriptor = ModifierDescriptor.UntypedStackable;

        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            int bonus = Value.Calculate(Fact.MaybeContext);
            
            evt.AddModifier(bonus * Fact.GetRank(), base.Fact, Descriptor);
        }

        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
        }
    }
}
