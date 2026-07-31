using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components;

using System.Linq;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic;

namespace EbonsContentMod.Components
{
    [TypeId("7fe92cad9ebd4b45b87ad72116bdf98e")]
    internal class AbilityShowIfCasterHasAnyFact :
        BlueprintComponent,
        IAbilityVisibilityProvider
    {
        public BlueprintFeatureReference[] m_Facts;

        public bool IsAbilityVisible(AbilityData ability)
        {
            return ability?.Caster != null
                && m_Facts != null
                && m_Facts.Any(fact =>
                    fact != null && ability.Caster.HasFact(fact));
        }
    }
}
