using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics.Components;

namespace EbonsContentMod.Components
{
    public class SetBaseMovementSpeed : UnitFactComponentDelegate
    {
        public int SpeedFeet = 30;

        private int? m_PreviousBaseValue;

        public override void OnTurnOn()
        {
            base.OnTurnOn();

            var speed = Owner?.Stats?.Speed;
            if (speed == null)
                return;

            // Only capture it once for this component instance.
            m_PreviousBaseValue ??= speed.BaseValue;

            speed.BaseValue = SpeedFeet;
        }

        public override void OnTurnOff()
        {
            var speed = Owner?.Stats?.Speed;
            if (speed != null && m_PreviousBaseValue.HasValue)
            {
                speed.BaseValue = m_PreviousBaseValue.Value;
            }

            m_PreviousBaseValue = null;

            base.OnTurnOff();
        }
    }
}