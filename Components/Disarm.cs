using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers;
using Kingmaker.Items;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;

namespace EbonsContentMod.Components
{
    internal class Disarm : ContextAction
    {
        public override string GetCaption()
        {
            return "Disarmed";
        }

        public override void RunAction()
        {
            if (base.Target.Unit == null)
            {
                return;
            }

            ItemEntityWeapon maybeWeapon = base.Target.Unit.Body.PrimaryHand.MaybeWeapon;
            ItemEntityWeapon maybeWeapon2 = base.Target.Unit.Body.SecondaryHand.MaybeWeapon;
            if (maybeWeapon != null && !maybeWeapon.Blueprint.IsUnarmed && !maybeWeapon.Blueprint.IsNatural)
            {
                base.Target.Unit.Descriptor.AddBuff(BlueprintRoot.Instance.SystemMechanics.DisarmMainHandBuff, this.Context, TimeSpanExtension.Seconds(12));
            }
            else if (maybeWeapon2 != null && !maybeWeapon2.Blueprint.IsUnarmed && !maybeWeapon2.Blueprint.IsNatural)
            {
                this.Target.Unit.Descriptor.AddBuff(BlueprintRoot.Instance.SystemMechanics.DisarmOffHandBuff, this.Context, TimeSpanExtension.Seconds(12));
            }
        }
    }
}