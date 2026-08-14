using System.Linq;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers.Rest;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using static UnityEngine.UI.GridLayoutGroup;

namespace EbonsContentMod.Components
{
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [TypeId("{0180EB7C-396D-4D40-A02F-622ECBCF2A9A}")]
    public class RemoveGrantedBuffOnRest :
    UnitFactComponentDelegate,
    IRestFinishedHandler
    {
        public BlueprintBuffReference m_Buff;

        public BlueprintBuff Buff =>
            m_Buff?.Get();

        public void HandleRestFinished(
            RestStatus status)
        {
            BlueprintBuff blueprint =
                Buff;

            if (blueprint == null)
                return;

            foreach (UnitEntityData unit in
                     Game.Instance.State.Units)
            {
                foreach (Buff buff in
                         unit.Buffs.RawFacts
                             .OfType<Buff>()
                             .ToArray())
                {
                    if (buff.Blueprint != blueprint)
                        continue;

                    if (buff.Owner == Owner)
                        continue;

                    if (buff.Context?.MaybeCaster != Owner)
                        continue;

                    buff.Remove();
                }
            }
        }
    }

}