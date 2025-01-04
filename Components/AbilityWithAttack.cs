using Kingmaker.Blueprints;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.Utility;
using Kingmaker.Visual.Animation.Kingmaker;
using System.Collections.Generic;
using System.Linq;

namespace EbonsContentMod.Components
{
    internal class AttackAnimation : BlueprintComponent, IAbilityCustomAnimation
    {
        public UnitAnimationAction GetAbilityAction(UnitEntityData caster)
        {
            return caster.Descriptor.Unit.View.AnimationManager.CreateHandle(UnitAnimationType.MainHandAttack).Action;
        }
    }

    public class AbilityDeliverAttackWithWeaponOnHit : AbilityDeliverEffect
    {
        public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper target)
        {
            if (target.Unit == null)
            {
                yield break;
            }
            UnitAttack cmd = new UnitAttack(target.Unit)
            {
                IsSingleAttack = true
            };
            cmd.Init(context.Caster);
            cmd.Start();
            AttackHandInfo attackHandInfo = cmd.AllAttacks.FirstOrDefault<AttackHandInfo>();
            ItemEntityWeapon weapon = (attackHandInfo != null) ? attackHandInfo.Weapon : null;
            if (weapon == null)
            {
                cmd.Interrupt();
                yield break;
            }
            bool hitHandled = false;
            bool isMelee = weapon.Blueprint.IsMelee;
            for (; ; )
            {
                if (cmd.IsFinished)
                {
                    RuleAttackWithWeapon lastAttackRule = cmd.LastAttackRule;
                    if (((lastAttackRule != null) ? lastAttackRule.LaunchedProjectiles : null) == null || cmd.LastAttackRule.LaunchedProjectiles.All(f => f.IsHit) || cmd.LastAttackRule.LaunchedProjectiles.All(f => f.Cleared) || cmd.LastAttackRule.LaunchedProjectiles.All(f => f.Destroyed))
                    {
                        break;
                    }
                }
                bool wasActed = cmd.IsActed;
                if (!cmd.IsFinished)
                {
                    cmd.Tick();
                }
                RuleAttackWithWeapon lastAttackRule2 = cmd.LastAttackRule;
                if (!wasActed && cmd.IsActed && isMelee)
                {
                    hitHandled = true;
                    if (lastAttackRule2.AttackRoll.IsHit)
                    {
                        yield return new AbilityDeliveryTarget(target);
                    }
                }
                yield return null;
            }
            if (!hitHandled && !isMelee)
            {
                RuleAttackWithWeapon lastAttackRule3 = cmd.LastAttackRule;
                bool? flag3 = (lastAttackRule3 != null) ? new bool?(lastAttackRule3.AttackRoll.IsHit) : null;
                if (flag3 != null && flag3.Value)
                {
                    yield return new AbilityDeliveryTarget(target);
                }
            }
            yield break;
        }
    }
}