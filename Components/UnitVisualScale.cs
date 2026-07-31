using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using UnityEngine;

namespace EbonsContentMod.Components
{
    [TypeId("BDA18528047447FE8A54243EC1A8377B")]
    internal class UnitVisualScale : UnitFactComponentDelegate
    {
        public float Scale = 1.0f;

        public override void OnTurnOn()
        {
            ApplyScale();
        }

        public override void OnActivate()
        {
            ApplyScale();
        }

        private void ApplyScale()
        {
            Main.log.Log(
                $"Applying scale {Scale}; " +
                $"View={(Owner?.View == null ? "NULL" : "present")}");

            if (Owner?.View?.transform == null)
                return;

            Owner.View.transform.localScale = Vector3.one * Scale;
        }
    }
}
