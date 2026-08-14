using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace EbonsContentMod.Components
{
    public class ContextActionOnPartyMembers : ContextAction
    {
        public ActionList Actions;

        public bool targetDead = false;

        public override string GetCaption()
        {
            return "Run actions on all party members";
        }

        public override void RunAction()
        {
            foreach (UnitEntityData unit in
                     Game.Instance.Player.PartyAndPets)
            {
                if (unit == null) continue;

                if (!targetDead
                    && unit.Descriptor.State.IsDead)
                {
                    continue;
                }

                using (Context.GetDataScope(unit))
                {
                    Actions.Run();
                }
            }
        }
    }
}
