using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using System.Linq;
using UnityModManagerNet;
using EbonsContentMod.Utilities;

namespace EbonsContentMod.Components
{
    internal class FlamboyantArcanaDerringDo : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleSkillCheck>, IInitiatorRulebookSubscriber
    {
        private StatType[] stats = { StatType.SkillAthletics, StatType.SkillMobility };
        private bool willSpend = false;

        public int CalculateExplodingDice(RuleSkillCheck evt)
        {
            RuleRollDice rule = new RuleRollDice(evt.Initiator, new DiceFormula(1, DiceType.D6));

            int roll = this.Fact.MaybeContext.TriggerRule<RuleRollDice>(rule).Result;

            int total = roll;

            if (roll == 6)
            {
                int attempts = Owner.Stats.Dexterity.Bonus > 0 ? Owner.Stats.Dexterity.Bonus : 1;

                for (int x = 0; x < attempts; x++)
                {
                    roll = this.Fact.MaybeContext.TriggerRule<RuleRollDice>(rule).Result;
                    total += roll;
                    if (roll != 6)
                        break;
                }
            }
            return total;
        }

        public void OnEventAboutToTrigger(RuleSkillCheck evt)
        {
            if (!stats.Contains(evt.StatType))
                return;

            evt.Bonus.AddModifier(CalculateExplodingDice(evt), base.Fact, ModifierDescriptor.UntypedStackable);
            willSpend = true;

        }

        public void OnEventDidTrigger(RuleSkillCheck evt)
        {
            if (willSpend)
            {
                int cost = 1;

                ResourceHelperators.SpendArcanePool(Owner, cost);
                //Owner.Descriptor.Resources.Spend(BlueprintTool.GetRef<BlueprintAbilityResourceReference>(AbilityResourceRefs.ArcanePoolResourse.ToString()), cost);
                willSpend = false;
            }
        }
    }
}