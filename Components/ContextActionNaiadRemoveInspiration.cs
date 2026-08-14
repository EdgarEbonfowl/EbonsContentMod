using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.ElementsSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System.Linq;

namespace EbonsContentMod.Components
{
    public class ContextActionRemoveNaiadInspiration : ContextAction
    {
        public BlueprintBuffReference m_Buff;

        public override string GetCaption()
        {
            return "Remove existing Naiad Inspiration";
        }

        public override void RunAction()
        {
            var caster = Context?.MaybeCaster;
            var blueprint = m_Buff?.Get();

            if (caster == null || blueprint == null)
                return;

            foreach (var unit in Game.Instance.Player.AllCharacters)
            {
                var buffs = unit.Buffs.Enumerable
                    .Where(buff =>
                        buff.Blueprint == blueprint
                        && buff.Context?.MaybeCaster == caster)
                    .ToArray();

                foreach (var buff in buffs)
                {
                    unit.Buffs.RemoveFact(buff);
                }
            }
        }
    }
}