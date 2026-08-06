using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace EbonsContentMod.Components
{
    [TypeId("c026dd01a6434ec6b8b45d4e47270218")]
    public class ContextActionRemoveAllNegativeEffects : ContextAction
    {
        public override string GetCaption()
        {
            return "Remove all negative effects";
        }

        public override void RunAction()
        {
            UnitEntityData target = Target.Unit;

            if (target == null)
                return;

            UnitHelper.RemoveNegativeEffects(
                target.Descriptor);
        }
    }
}