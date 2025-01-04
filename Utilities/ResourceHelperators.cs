using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Visual.CharacterSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.UI.GridLayoutGroup;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker;
using BlueprintCore.Blueprints.References;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using UnityEngine.Serialization;
using UnityEngine;


namespace EbonsContentMod.Utilities
{
    public static class ResourceHelperators
    {
        /// <summary>
        /// Spends Arcane Pool points. Implements checks for Dark Codex material.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Owner = UnitEntityData for the unit who is spending the Arcane Pool points.
        /// </para>
        /// <para>
        /// Cost = Integer for the number of Arcane Pool points being spent.
        /// </para>
        /// </remarks>
        public static void SpendArcanePool(UnitEntityData Owner, int Cost)
        {
            if (Owner.HasFact(BlueprintTool.GetRef<BlueprintUnitFactReference>("80648fab6145492b8978c7d9cf86abc6"))) // Limitless Arcane Pool from Dark Codex
            {
                Cost = 0;
            }

            Owner.Descriptor.Resources.Spend(BlueprintTool.GetRef<BlueprintAbilityResourceReference>(AbilityResourceRefs.ArcanePoolResourse.ToString()), Cost);
        }
    }

    public class ContextSpendArcanePool : ContextAction
    {
        public override string GetCaption()
        {
            return "Spend resourse";
        }

        public override void RunAction()
        {
            var value = this.Value.Calculate(base.Context);

            UnitEntityData maybeCaster = base.Context.MaybeCaster;
            if (maybeCaster == null)
            {
                PFLog.Default.Error("Caster is missing", Array.Empty<object>());
                return;
            }
            ResourceHelperators.SpendArcanePool(maybeCaster, value);
        }

        public ContextValue Value;
    }
}
