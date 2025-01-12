using Kingmaker.Utility;
using System.Collections.Generic;
using System.Linq;
using Kingmaker.Enums;
using JetBrains.Annotations;
using EbonsContentMod.UnitParts;

namespace EbonsContentMod.UnitParts
{
    // This was inspired by Holic75's "UnitPartIgnoreFogConcealement" but I wanted something more versatile that could be used for any ConcealmentDescriptor.
    public class UnitPartIgnoreConcealmentDescriptor : AdditiveUnitPart
    {
        public override CountableFlag UnitPartFlag
        {
            get
            {
                return IgnoreConcealmentType;
            }
        }

        public bool Active()
        {
            bool UnitPartIsActive = false;
            if (this.UnitPartFlag.Value)
            {
                if (this.m_IgnoredConcealmentDescriptors.Any())
                {
                    UnitPartIsActive = true;
                }
            }

            return UnitPartIsActive;
        }

        public void AddIgnoreConcealmentDescriptor(UnitPartIgnoreConcealmentDescriptor.IgnoredConcealmentDescriptorEntry entry)
        {
            if (this.m_IgnoredConcealmentDescriptors == null)
            {
                this.m_IgnoredConcealmentDescriptors = new List<UnitPartIgnoreConcealmentDescriptor.IgnoredConcealmentDescriptorEntry>();
            }
            this.m_IgnoredConcealmentDescriptors.Add(entry);
        }

        public void RemoveIgnoreConcealmentDescriptor(UnitPartIgnoreConcealmentDescriptor.IgnoredConcealmentDescriptorEntry entry)
        {
            if (this.m_IgnoredConcealmentDescriptors == null)
            {
                return;
            }
            foreach (UnitPartIgnoreConcealmentDescriptor.IgnoredConcealmentDescriptorEntry icd_entry in this.m_IgnoredConcealmentDescriptors)
            {
                if (icd_entry.Type == entry.Type)
                {
                    this.m_IgnoredConcealmentDescriptors.Remove(icd_entry);
                    if (this.m_IgnoredConcealmentDescriptors.Count <= 0)
                    {
                        this.m_IgnoredConcealmentDescriptors = null;
                    }
                    break;
                }
            }
        }


        public bool IsIgnoringConcealmentDescriptor(ConcealmentDescriptor type)
        {
            foreach (UnitPartIgnoreConcealmentDescriptor.IgnoredConcealmentDescriptorEntry ig_cc_ds_entry in this.m_IgnoredConcealmentDescriptors)
            {
                if (type == ig_cc_ds_entry.Type)
                {
                    return true;
                }
            }

            return false;

        }


        public CountableFlag IgnoreConcealmentType = new CountableFlag();

        [CanBeNull]
        private List<UnitPartIgnoreConcealmentDescriptor.IgnoredConcealmentDescriptorEntry> m_IgnoredConcealmentDescriptors;

        public class IgnoredConcealmentDescriptorEntry
        {
            public ConcealmentDescriptor Type;
        }

    }
}