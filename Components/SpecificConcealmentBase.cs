using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EbonsContentMod.UnitParts;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Blueprints;
using static Kingmaker.UnitLogic.Parts.UnitPartConcealment;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;


namespace EbonsContentMod.Components
{
    // Holic75_PT
    [TypeId("23D94962EE084D7A96F2DE9BEA682A79")]
    public abstract class SpecificConcealmentBase : UnitBuffComponentDelegate
    {

        public abstract bool WorksAgainst(UnitEntityData attacker);

        public override void OnTurnOn()
        {
            this.Owner.Ensure<UnitPartSpecificConcealment>().AddUnitPartFlag();
            this.Owner.Get<UnitPartSpecificConcealment>().AddEntry(this.Fact, this);
        }

        public override void OnTurnOff()
        {
            this.Owner.Ensure<UnitPartSpecificConcealment>().RemoveEntry(this.Fact, this);

        }

        public UnitPartConcealment.ConcealmentEntry CreateConcealmentEntry()
        {
            UnitPartConcealment.ConcealmentEntry NewConcealmentEntry = new UnitPartConcealment.ConcealmentEntry()
            {
                Concealment = this.Concealment,
                Descriptor = this.Descriptor
            };
            if (this.CheckDistance)
            {
                NewConcealmentEntry.DistanceGreater = this.DistanceGreater;
            }
            if (this.CheckDistance)
            {
                NewConcealmentEntry.RangeType = new WeaponRangeType?(this.RangeType);

            }
            NewConcealmentEntry.OnlyForAttacks = this.OnlyForAttacks;

            return NewConcealmentEntry;
        }

        public Concealment Concealment;
        public ConcealmentDescriptor Descriptor;
        public bool CheckWeaponRangeType;
        public bool CheckDistance;

        [ShowIf("CheckWeaponRangeType")]
        public WeaponRangeType RangeType;

        [ShowIf("CheckDistance")]
        public Feet DistanceGreater;

        public bool OnlyForAttacks;


    }
}