using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using static UnityEngine.UI.GridLayoutGroup;

namespace EbonsContentMod.UnitParts
{
    public class UnitPartBaseSpeedOverride : UnitPart
    {
        private readonly List<Entry> m_Entries = new();

        public void Add(EntityFact source, int speedFeet, int priority = 0)
        {
            if (source == null)
                return;

            RemoveInternal(source);

            m_Entries.Add(new Entry
            {
                Source = source,
                SpeedFeet = speedFeet,
                Priority = priority
            });
        }

        public void Remove(EntityFact source)
        {
            if (source == null)
                return;

            RemoveInternal(source);

            if (m_Entries.Count == 0)
            {
                Owner.Remove<UnitPartBaseSpeedOverride>();
            }
        }

        public bool TryGetSpeed(out int speedFeet)
        {
            Entry selected = m_Entries
                .Where(entry => entry.Source != null)
                .OrderByDescending(entry => entry.Priority)
                .ThenByDescending(entry => entry.SpeedFeet)
                .FirstOrDefault();

            if (selected == null)
            {
                speedFeet = default;
                return false;
            }

            speedFeet = selected.SpeedFeet;
            return true;
        }

        private void RemoveInternal(EntityFact source)
        {
            m_Entries.RemoveAll(entry => entry.Source == source);
        }

        private class Entry
        {
            public EntityFact Source;
            public int SpeedFeet;
            public int Priority;
        }
    }
}
