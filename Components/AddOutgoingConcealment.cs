using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Enums;
using Kingmaker.Utility;
using EbonsContentMod.UnitParts;
using Kingmaker.Blueprints.JsonSystem;

namespace EbonsContentMod.Components
{
    [TypeId("22AEC25204A240708F327E9C1FB747FD")]
    [AllowedOn(typeof(BlueprintUnitFact))]
    public class AddOutgoingConcealment : UnitFactComponentDelegate
    {
        public override void OnTurnOn()
        {
            base.Owner.Ensure<UnitPartOutgoingConcealment>().AddConcealment(this.CreateConcealmentEntry());
        }

        public override void OnTurnOff()
        {
            base.Owner.Ensure<UnitPartOutgoingConcealment>().RemoveConcealement(this.CreateConcealmentEntry());
        }

        private UnitPartConcealment.ConcealmentEntry CreateConcealmentEntry()
        {
            UnitPartConcealment.ConcealmentEntry concealmentEntry = new UnitPartConcealment.ConcealmentEntry
            {
                Concealment = this.Concealment,
                Descriptor = this.Descriptor
            };
            if (this.CheckDistance)
            {
                concealmentEntry.DistanceGreater = this.DistanceGreater;
            }
            if (this.CheckWeaponRangeType)
            {
                concealmentEntry.RangeType = new WeaponRangeType?(this.RangeType);
            }
            concealmentEntry.OnlyForAttacks = this.OnlyForAttacks;
            return concealmentEntry;
        }

        public ConcealmentDescriptor Descriptor;

        public Concealment Concealment;

        public bool CheckWeaponRangeType;

        public bool CheckDistance;


        [ShowIf("CheckWeaponRangeType")]
        public WeaponRangeType RangeType;

        [ShowIf("CheckDistance")]
        public Feet DistanceGreater;

        public bool OnlyForAttacks;


    }
}