using System;
using System.Linq;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.Kineticist;

namespace EbonsContentMod.Components
{
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("{2985DC9D-F2DE-479B-ACA7-6861B56242A8}")]
    public class CMDBonusFromKineticOverflow :
        UnitFactComponentDelegate,
        ITargetRulebookHandler<RuleCalculateCMD>,
        IRulebookHandler<RuleCalculateCMD>,
        ISubscriber,
        ITargetRulebookSubscriber
    {
        public ModifierDescriptor Descriptor;

        public CombatManeuver[] Maneuvers;

        public void OnEventAboutToTrigger(
            RuleCalculateCMD evt)
        {
            foreach (CombatManeuver maneuver in Maneuvers)
            {
                if (evt.Type != maneuver)
                    continue;

                UnitPartKineticist kineticist =
                    Owner.Get<UnitPartKineticist>();

                if (!kineticist)
                    return;

                int bonus =
                    Math.Min(
                        kineticist.ClassLevel / 3,
                        kineticist.AcceptedBurn);

                evt.AddModifier(
                    bonus,
                    Fact,
                    Descriptor);

                return;
            }
        }

        public void OnEventDidTrigger(
            RuleCalculateCMD evt)
        {
        }
    }
}