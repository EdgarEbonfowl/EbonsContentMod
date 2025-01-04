using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using Kingmaker.Blueprints;
using Kingmaker.Controllers.Rest;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using Kingmaker.RuleSystem.Rules.Damage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UI.CanvasScalerWorkaround;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.UnitLogic.Mechanics.Components;
using static Kingmaker.GameModes.GameModeType;
using Kingmaker.ElementsSystem;
using Kingmaker;
using TabletopTweaks.Core.Utilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Spells;
using System.Drawing;

namespace EbonsContentMod.Components
{
    
    [AllowedOn(typeof(BlueprintBuff))]
    internal class SippingDemonAddTemporaryHP : UnitBuffComponentDelegate<SippingDemonAddTemporaryHP.ComponentData>, ITargetRulebookHandler<RuleDealDamage>, IRulebookHandler<RuleDealDamage>, ISubscriber, ITargetRulebookSubscriber
    {
        public ModifierDescriptor Descriptor = default;

        public override void OnActivate()
        {
            base.OnActivate();

            var willmod = Owner.Stats.Wisdom.Bonus;

            var classlevel = Owner.Progression.GetClassLevel(CharacterClassRefs.MonkClass.Reference);

            var value = Math.Min(willmod, classlevel);

            if (value == 0) return; // Probably don't need this anymore without a context value

            base.Data.Modifier = base.Owner.Stats.TemporaryHitPoints.AddModifier(value, base.Runtime, this.Descriptor);
        }

        public override void OnTurnOff()
        {
            base.OnTurnOff();

            base.Data.Modifier?.Remove();
            base.Data.Modifier = null;
        }

        public void OnEventAboutToTrigger(RuleDealDamage evt)
        {
        }

        public void OnEventDidTrigger(RuleDealDamage evt)
        {
            if (base.Data.Modifier.AppliedTo == null)
            {
                base.Buff.Remove();
            }
        }

        public class ComponentData
        {
            [JsonProperty]
            public ModifiableValue.Modifier Modifier;
        }
    }

    internal class SippingDemonTempHP : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {
        public static readonly string SippingDemonTempHPBuffOneGuid = "{7C19F9FA-954C-4A97-A1DF-EB1D8E1DC062}";

        public static readonly string SippingDemonTempHPBuffTwoGuid = "{DD7C4C80-0101-4F3D-B31C-D2C8ABEDE044}";
        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            var buff1 = BlueprintTools.GetBlueprint<BlueprintBuff>(SippingDemonTempHPBuffOneGuid);

            var buff2 = BlueprintTools.GetBlueprint<BlueprintBuff>(SippingDemonTempHPBuffTwoGuid);

            if (evt.AttackRoll.IsHit && !evt.AttackRoll.IsCriticalConfirmed && !Owner.HasFact(BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(SippingDemonTempHPBuffTwoGuid)))
            {
                GameHelper.ApplyBuff(Owner, buff1, 600.Rounds());
            }
            else if (evt.AttackRoll.IsCriticalConfirmed)
            {
                if (Owner.HasFact(BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(SippingDemonTempHPBuffOneGuid))) GameHelper.RemoveBuff(Owner, buff1);
                GameHelper.ApplyBuff(Owner, buff2, 600.Rounds());
            }
        }
    }
}
