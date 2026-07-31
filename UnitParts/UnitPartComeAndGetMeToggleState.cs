using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.UnitLogic;
using Newtonsoft.Json;

namespace EbonsContentMod.UnitParts
{
    internal class UnitPartComeAndGetMeToggleState : OldStyleUnitPart
    {
        [JsonProperty]
        private bool m_HasSavedState;

        [JsonProperty]
        private bool m_ShouldBeActive;

        public bool HasSavedState => m_HasSavedState;

        public bool ShouldBeActive => m_ShouldBeActive;

        public void SaveState(bool active)
        {
            m_HasSavedState = true;
            m_ShouldBeActive = active;
        }
    }
}
