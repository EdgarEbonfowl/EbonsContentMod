using Kingmaker.EntitySystem.Entities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace EbonsContentMod.Components
{
    public class ContextActionFullHeal : ContextAction
    {
        public override string GetCaption()
        {
            return "Fully heal target";
        }

        public override void RunAction()
        {
            UnitEntityData target =
                Target.Unit;

            if (target == null)
                return;

            int missingHp =
                target.Descriptor.Damage;

            if (missingHp <= 0)
                return;

            // Use the normal healing rule here so all normal healing events/mechanics still occur.
            Rulebook.Trigger(
                new RuleHealDamage(
                    Context.MaybeCaster,
                    target,
                    missingHp));
        }
    }
}
