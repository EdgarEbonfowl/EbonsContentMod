using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Controllers.Projectiles;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.UI.GridLayoutGroup;
using EbonsContentMod.Utilities;

namespace EbonsContentMod.Components
{
    internal class ArcaneDeedDodgingPanache : UnitFactComponentDelegate, ITargetRulebookHandler<RuleAttackWithWeapon>, ITargetRulebookSubscriber
    {
        private bool willSpend = false;
        public void OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
            if (!evt.Weapon.Blueprint.IsMelee || !((base.Owner.Body.Armor.HasArmor && base.Owner.Body.Armor.Armor.Blueprint.ProficiencyGroup == ArmorProficiencyGroup.Light) || !base.Owner.Body.Armor.HasArmor))
                return;

            willSpend = true;

            int bonus = Owner.Stats.Charisma.Bonus > 0 ? Owner.Stats.Charisma.Bonus : 0;

            evt.AddTemporaryModifier(evt.Target.Stats.AC.AddModifier(bonus, base.Fact, ModifierDescriptor.Dodge));
        }

        public void OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            if (willSpend)
            {
                evt.Initiator.CombatState.PreventAttacksOfOpporunityNextFrame = true;
                evt.Target.CombatState.PreventAttacksOfOpporunityNextFrame = true;

                var displacement = (evt.Target.Position - evt.Initiator.Position).normalized;
                var initialPos = evt.Target.Position;
                evt.Target.Position = initialPos + displacement;

                Game.Instance.ProjectileController.Launch(evt.Target, evt.Target, ProjectileRefs.WindProjectile00.Reference.Get(), initialPos, delegate (Projectile p)
                {
                });
            }

            ResourceHelperators.SpendArcanePool(Owner, 1);
            willSpend = false;
        }
    }
}
