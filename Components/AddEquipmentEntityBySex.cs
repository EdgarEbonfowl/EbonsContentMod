using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Persistence;
using Kingmaker.PubSubSystem;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic;
using Kingmaker.View;
using Kingmaker.Visual.CharacterSystem;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonsContentMod.Components
{
    public class AddEquipmentEntityBySex : UnitFactComponentDelegate, IUnitViewAttachedHandler, IUnitSubscriber, ISubscriber
    {
        public override void OnTurnOn()
        {
            if (base.IsReapplying || Game.Instance.SaveManager.CurrentState == SaveManager.State.Saving)
            {
                return;
            }
            UnitEntityView view = base.Owner.View;
            if (view == null)
            {
                return;
            }
            Character characterAvatar = view.CharacterAvatar;
            if (characterAvatar == null)
            {
                return;
            }
            if (base.Owner.Gender == Kingmaker.Blueprints.Gender.Male) characterAvatar.AddEquipmentEntity(this.MaleEquipmentEntity, false, -1, -1);
            else if(base.Owner.Gender == Kingmaker.Blueprints.Gender.Female) characterAvatar.AddEquipmentEntity(this.FemaleEquipmentEntity, false, -1, -1);
        }

        public override void OnTurnOff()
        {
            if (base.IsReapplying || Game.Instance.SaveManager.CurrentState == SaveManager.State.Saving)
            {
                return;
            }
            UnitEntityView view = base.Owner.View;
            if (view == null)
            {
                return;
            }
            Character characterAvatar = view.CharacterAvatar;
            if (characterAvatar == null)
            {
                return;
            }
            if (base.Owner.Gender == Kingmaker.Blueprints.Gender.Male) characterAvatar.RemoveEquipmentEntity(this.MaleEquipmentEntity, false);
            else if (base.Owner.Gender == Kingmaker.Blueprints.Gender.Female) characterAvatar.RemoveEquipmentEntity(this.FemaleEquipmentEntity, false);
        }

        public void HandleUnitViewAttached()
        {
            this.OnTurnOn();
        }

        public EquipmentEntityLink MaleEquipmentEntity;
        public EquipmentEntityLink FemaleEquipmentEntity;
    }
}
