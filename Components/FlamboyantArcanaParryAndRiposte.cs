using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.Blueprints.Area.FactHolder;
using static UnityEngine.UI.GridLayoutGroup;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using BlueprintCore.Blueprints.References;
using UnityModManagerNet;
using EbonsContentMod.Utilities;

namespace EbonsContentMod.Components
{
    internal class FlamboyantArcanaParryAndRiposte : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackRoll>, ITargetRulebookSubscriber
    {
        private bool willSpend = false;
        public void OnEventAboutToTrigger(RuleAttackRoll evt)
        {
            if (!evt.Weapon.Blueprint.IsMelee || evt.Parry != null || !Owner.IsReach(evt.Target, Owner.Body.PrimaryHand))
                return;

            evt.TryParry(Owner, Owner.Body.PrimaryHand.Weapon, 2 * (evt.Initiator.Descriptor.State.Size - Owner.State.Size));

            willSpend = true;
        }

        public void OnEventDidTrigger(RuleAttackRoll evt)
        {
            if (willSpend)
            {
                willSpend = false;
                int cost = 1;

                // If no parry get out
                if (!evt.Parry.IsTriggered)
                    return;

                // If parried, do the riposte
                if (evt.Result == AttackResult.Parried && evt.Target.Descriptor.Resources.GetResourceAmount(BlueprintTool.GetRef<BlueprintAbilityResourceReference>(AbilityResourceRefs.ArcanePoolResourse.ToString())) >= 1 + cost)
                {
                    Game.Instance.CombatEngagementController.ForceAttackOfOpportunity(Owner, evt.Initiator);
                }

                // Points are spent either way
                ResourceHelperators.SpendArcanePool(Owner, cost);
                //Owner.Descriptor.Resources.Spend(BlueprintTool.GetRef<BlueprintAbilityResourceReference>(AbilityResourceRefs.ArcanePoolResourse.ToString()), cost);
            }
        }
    }
}
