using Kingmaker.UnitLogic;
using Kingmaker.Utility;

// Holic75_PT
// This part is a port from Holic, but owes a lot to Vek17's extraordinary CustomMechanics code and to Kurofivne's help with the suggestions on how to set the accessibilty parameters!!
namespace EbonsContentMod.UnitParts
{
    public abstract class AdditiveUnitPart : OldStyleUnitPart
    {
        public virtual void AddUnitPartFlag()
        {
            this.UnitPartFlag.Retain();
        }


        public virtual void RemoveUnitPartFlag()
        {
            this.UnitPartFlag.Release();
        }

        public virtual void ClearUnitPartFlag()
        {
            this.UnitPartFlag.ReleaseAll();
        }

        public abstract CountableFlag UnitPartFlag
        {
            get;
        }

    }
}