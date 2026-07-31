using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbonsContentMod.UnitParts;
using Kingmaker.UnitLogic;

namespace EbonsContentMod.Components
{
    public class SetBaseSpeedOverride : UnitFactComponentDelegate
    {
        public int SpeedFeet = 60;
        public int Priority;

        public override void OnTurnOn()
        {
            base.OnTurnOn();

            Owner
                .Ensure<UnitPartBaseSpeedOverride>()
                .Add(Fact, SpeedFeet, Priority);

            Owner.Stats.Speed.UpdateValue();
        }

        public override void OnTurnOff()
        {
            Owner
                .Get<UnitPartBaseSpeedOverride>()
                ?.Remove(Fact);

            Owner.Stats.Speed.UpdateValue();

            base.OnTurnOff();
        }
    }
}
