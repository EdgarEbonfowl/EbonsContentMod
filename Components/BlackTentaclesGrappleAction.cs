using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.ElementsSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;

namespace EbonsContentMod.Components
{
    [TypeId("e4ccb4b9c9a44a86bb01feffcf38acab")]
    public class BlackTentaclesGrappleAction : ContextAction
    {
        public CombatManeuver Type = CombatManeuver.Grapple;
        public ContextValue Value;
        public ActionList Success;
        public ActionList Failure;
        public int Bonus;


        public override string GetCaption() => "Combat maneuver: " + Type.ToString();

        public override void RunAction()
        {

            if (Target.Unit == null)
            {
                return;
            }
            else if (Context.MaybeCaster == null)
            {
                return;
            }
            if (Target.Unit.IsPlayerFaction) return; // prevent this on allies for this one

            RuleCombatManeuver ruleCombatManeuver = new RuleCombatManeuver(Context.MaybeCaster, Target.Unit, Type);

            ruleCombatManeuver.AdditionalBonus = Bonus;

            try
            {
                ruleCombatManeuver.ReplaceAttackBonus = Context.Params.CasterLevel;
            }
            catch (Exception e)
            {
                Main.log.Log(e.ToString());
            }

            if (Context.TriggerRule(ruleCombatManeuver).Success)
            {
                Success.Run();
            }
            else
            {
                Failure.Run();
            }
        }
    }
}
