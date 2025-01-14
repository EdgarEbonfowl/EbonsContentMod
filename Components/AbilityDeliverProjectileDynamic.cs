using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers;
using Kingmaker.Controllers.Optimization;
using Kingmaker.Controllers.Projectiles;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.Items.Slots;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Class.Kineticist;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker.Visual.Particles;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Owlcat.QA.Validation;
using Owlcat.Runtime.Core.Math;
using Owlcat.Runtime.Core.Utils;
using Owlcat.Runtime.Visual.RenderPipeline.RendererFeatures.FogOfWar;
using UnityEngine;
using UnityEngine.Serialization;
using TabletopTweaks.Core.NewUnitParts;
using TabletopTweaks.Core.Utilities;
using EbonsContentMod.Utilities;

namespace Kingmaker.UnitLogic.Abilities.Components
{
    [TypeId("a7ec15576fb827040bfe609a904482cd")]
    public class AbilityDeliverProjectileDynamic : AbilityDeliverEffect
    {
        public UnitEntityData currenttarget = null;

        public List<UnitEntityData> TargetList = [];
        
        public ReferenceArrayProxy<BlueprintProjectile, BlueprintProjectileReference> Projectiles
        {
            get
            {
                return this.m_Projectiles;
            }
        }

        public virtual Feet Length
        {
            get
            {
                return this.m_Length;
            }
        }

        public virtual Feet LineWidth
        {
            get
            {
                return this.m_LineWidth;
            }
        }

        public BlueprintItemWeapon Weapon
        {
            get
            {
                BlueprintItemWeaponReference weapon = this.m_Weapon;
                if (weapon == null)
                {
                    return null;
                }
                return weapon.Get();
            }
        }

        [CanBeNull]
        public BlueprintBuff ControlledProjectileHolderBuff
        {
            get
            {
                BlueprintBuffReference controlledProjectileHolderBuff = this.m_ControlledProjectileHolderBuff;
                if (controlledProjectileHolderBuff == null)
                {
                    return null;
                }
                return controlledProjectileHolderBuff.Get();
            }
        }

        protected virtual bool UseFeet
        {
            get
            {
                return true;
            }
        }

        [UsedImplicitly]
        private bool ShowWeapon
        {
            get
            {
                return !this.IsPierceOrCone && this.NeedAttackRoll && !this.IsHandOfTheApprentice;
            }
        }

        [UsedImplicitly]
        private bool ShowAttackRollBonusStat
        {
            get
            {
                return this.NeedAttackRoll && this.ReplaceAttackRollBonusStat;
            }
        }

        private bool IsPierceOrCone
        {
            get
            {
                return this.Type == AbilityProjectileType.Line || this.Type == AbilityProjectileType.Cone;
            }
        }

        [UsedImplicitly]
        private bool IsMultipleProjectiles
        {
            get
            {
                return this.Projectiles.Length > 1;
            }
        }

        private bool GetDistanceBetweenTargetsIsGood(UnitEntityData checkTarget, UnitEntityData originalTarget, float SecondaryTargetMaxRange)
        {
            if (!this.TargetList.Contains(originalTarget))
                this.TargetList = this.TargetList.AddUnique(originalTarget);

            if (this.TargetList.Contains(checkTarget)) return false; // Trying to re-target an old target

            foreach (UnitEntityData target in this.TargetList) // Are no two targets more than SecondaryTargetMaxRange apart?
            {
                if (target.DistanceTo(checkTarget) > SecondaryTargetMaxRange) return false;
            }

            this.TargetList = this.TargetList.AddUnique(checkTarget);
            
            return true;
        }

        private UnitEntityData GetNewTarget(AbilityExecutionContext context, UnitEntityData OriginalTarget, float AbilityRange, float SecondaryTargetMaxRange = 0f)
        {
            UnitEntityData caster = context.MaybeCaster;
            AbilityExecutionContext sourceAbilityContext = context.SourceAbilityContext;
            var Ability = sourceAbilityContext.Ability;
            var SecondaryTargetSearchRadius = SecondaryTargetMaxRange;

            if (SecondaryTargetMaxRange != 0f)
            {
                List<UnitEntityData> list = EntityBoundsHelper.FindUnitsInRange(OriginalTarget.Position, SecondaryTargetSearchRadius.Feet().Meters);
                list.Remove(OriginalTarget);
                list.RemoveAll((UnitEntityData x) => x.Faction != OriginalTarget.Faction || !Ability.CanTarget(x));
                if (list.Count <= 0) return null;
                list.OrderBy(t => t.DistanceTo(OriginalTarget));
                foreach (UnitEntityData unit in list)
                {
                    if (unit.DistanceTo(caster) <= AbilityRange && GetDistanceBetweenTargetsIsGood(unit, OriginalTarget, SecondaryTargetMaxRange))
                    {
                        return unit;
                    }
                }
            }
            else
            {
                List<UnitEntityData> list = EntityBoundsHelper.FindUnitsInRange(caster.Position, AbilityRange.Feet().Meters);
                list.Remove(OriginalTarget);
                list.RemoveAll((UnitEntityData x) => x.Faction != OriginalTarget.Faction || !Ability.CanTarget(x));
                if (list.Count <= 0) return null;
                return list.OrderBy(t => t.DistanceTo(caster)).First();
            }
            return null;
        }

        public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper target)
        {
            Vector3 castPosition = context.Caster.EyePosition;
            BlueprintBuff controlledProjectileBuff = this.ControlledProjectileHolderBuff;
            if (controlledProjectileBuff != null)
            {
                ControlledProjectileHolder.Runtime runtime = this.FindControlledProjectileRuntime(context.Caster);
                Vector3? vector = (runtime != null) ? runtime.GetActiveProjectilePosition() : null;
                if (vector != null)
                {
                    castPosition = vector.Value;
                    IEnumerator<object> handleControlledProjectile = AbilityDeliverProjectile.HandleControlledProjectile(runtime, context);
                    while (handleControlledProjectile.MoveNext())
                    {
                        yield return null;
                    }
                    context.Caster.Buffs.RemoveFact(controlledProjectileBuff);
                    handleControlledProjectile = null;
                }
            }
            ReferenceArrayProxy<BlueprintProjectile, BlueprintProjectileReference> referenceArrayProxy = this.Projectiles;
            AbilityExecutionContext sourceAbilityContext = context.SourceAbilityContext;
            if (((sourceAbilityContext != null) ? sourceAbilityContext.Ability.MagicHackData : null) != null)
            {
                MagicHackSpellbookComponent component = context.Ability.SpellbookBlueprint.GetComponent<MagicHackSpellbookComponent>();
                BlueprintProjectile blueprintProjectile = (component != null) ? component.GetProjectile(context.Ability.GetDeliverBlueprint(false)) : null;
                if (blueprintProjectile != null)
                {
                    referenceArrayProxy = new BlueprintProjectileReference[1];
                    referenceArrayProxy[0] = blueprintProjectile;
                }
            }
            int count = this.UseMaxProjectilesCount ? context[this.MaxProjectilesCountRank] : referenceArrayProxy.Length;
            List<IEnumerator<AbilityDeliveryTarget>> deliveryProcesses = referenceArrayProxy.Take(count).Select((BlueprintProjectile p, int i) => this.Deliver(context, castPosition, target, p, i, controlledProjectileBuff != null)).ToList<IEnumerator<AbilityDeliveryTarget>>();
            while (deliveryProcesses.Count > 0)
            {
                int num;
                for (int j = 0; j < deliveryProcesses.Count; j = num)
                {
                    IEnumerator<AbilityDeliveryTarget> p2 = deliveryProcesses[j];
                    bool flag;
                    while ((flag = p2.MoveNext()) && p2.Current != null)
                    {
                        yield return p2.Current;
                    }
                    if (!flag)
                    {
                        deliveryProcesses[j] = null;
                    }
                    p2 = null;
                    num = j + 1;
                }
                deliveryProcesses.RemoveAll((IEnumerator<AbilityDeliveryTarget> i) => i == null);
                yield return null;
            }
            yield break;
        }

        protected IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, Vector3 castPosition, TargetWrapper target, BlueprintProjectile projectile, int index, bool isControlledProjectile)
        {
            TimeSpan startTime = Game.Instance.TimeController.GameTime;
            UnitEntityData caster = context.MaybeCaster;
            if (caster == null)
            {
                PFLog.Default.Error(this, "Caster is missing", Array.Empty<object>());
                yield break;
            }
            Vector3 casterPosition = this.UseCastPositionInsteadCasterPosition ? castPosition : caster.Position;
            Vector3 vector = target.IsUnit ? target.Point : AbilityDeliverProjectile.CorrectPosition(target.Point, casterPosition);
            Vector3 direction = (vector - castPosition).ToXZ().normalized;
            Debug.DrawLine(castPosition, vector, Color.yellow, 3f);
            if (this.IsPierceOrCone)
            {
                if (!isControlledProjectile || castPosition.ToXZ() == caster.Position.ToXZ())
                {
                    castPosition += direction * caster.Corpulence;
                }
                target = new TargetWrapper(castPosition + direction * this.Length.Meters, new float?(target.Orientation), null);
            }
            Debug.DrawLine(castPosition, target.Point, Color.red, 3f);
            while (Game.Instance.TimeController.GameTime - startTime < (this.DelayBetweenProjectiles * (float)index).Seconds())
            {
                yield return null;
            }
            if (index == 0) // Just to be sure, set everything back to null/empty on the first shot
            {
                this.currenttarget = null;
                this.TargetList = [];
            }
            else if (target.IsUnit && index > 0 && Type == AbilityProjectileType.Simple) // Are we targeting a unit and is it not the first projectile and is it simple type?
            {
                var AbilityRange = this.GetProjectileRange(context, context.HasMetamagic(Metamagic.Reach));
                var SecondaryTargetMaxRange = m_SecondaryTargetMaxDistance;
                var originaltarget = target.Unit;
                if (this.currenttarget == null) this.currenttarget = originaltarget;

                if (this.currenttarget.HPLeft <= 0 && this.currenttarget != null) // Is the current target real and dead?
                {
                    // Find the next target within m_SecondaryTargetMaxDistance from original target, or within range of caster if m_SecondaryTargetMaxDistance = 0f
                    var newtarget = GetNewTarget(context, originaltarget, AbilityRange, SecondaryTargetMaxRange);
                    if (newtarget != null)
                    {
                        target = newtarget;
                        this.currenttarget = target.Unit;
                    }
                    else if (this.currenttarget == null) target = originaltarget; // This should never be the case since we set currenttarget above
                    else target = this.currenttarget; // Dump into dead current target
                }
                else if (this.currenttarget == null) target = originaltarget; // If we don't have a current target for whatever reason, stick to the original
                else target = this.currenttarget; // Not dead so stay with the new target
            }
            TargetWrapper launcher = isControlledProjectile ? castPosition : context.Caster;
            Projectile proj = Game.Instance.ProjectileController.Launch(launcher, target, projectile, null);
            bool reach = context.HasMetamagic(Metamagic.Reach);
            proj.MaxRange = this.GetProjectileRange(context, reach);
            WeaponSlot threatHandMelee = context.Caster.GetThreatHandMelee(false);
            ItemEntityWeapon itemEntityWeapon = (threatHandMelee != null) ? threatHandMelee.MaybeWeapon : null;
            if (!this.IsPierceOrCone && this.IsHandOfTheApprentice)
            {
                if (itemEntityWeapon == null)
                {
                    PFLog.Default.Error(context.AbilityBlueprint, "Weapon for Hand of the Apprentice is missing", Array.Empty<object>());
                    yield break;
                }
                GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(itemEntityWeapon.WeaponVisualParameters.Model, proj.View.transform);
                gameObject.transform.localPosition = Vector3.zero;
                gameObject.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
                gameObject.transform.localScale = Vector3.one;
            }
            if (index == 0)
            {
                proj.IsFirstProjectile = true;
            }
            bool flag = context.AssociatedBlueprint.GetComponent<AbilityIsBomb>() != null;
            RuleAttackRoll attackRoll = null;
            if (!this.IsPierceOrCone && this.NeedAttackRoll && (!flag || target.Unit != null))
            {
                if (target.Unit == null)
                {
                    PFLog.Default.Error(context.AbilityBlueprint, "Can't do ranged touch to point", Array.Empty<object>());
                    yield break;
                }
                bool flag2 = AbilityKineticist.IsArcherBlast(context.Ability) && this.Type == AbilityProjectileType.Simple;
                ItemEntityWeapon weapon = this.IsHandOfTheApprentice ? itemEntityWeapon : (flag2 ? context.Caster.Body.PrimaryHand.Weapon : this.Weapon.CreateEntity<ItemEntityWeapon>());
                StatType stat = this.AttackRollBonusStat;
                if (context.Caster.State.Features.DomainsByIntelligenceFeature)
                {
                    stat = StatType.Intelligence;
                }
                using (this.ReplaceAttackRollBonusStat ? ContextData<AttackBonusStatReplacement>.Request().Setup(stat) : null)
                {
                    attackRoll = context.AttackRoll;
                    if (attackRoll == null)
                    {
                        if (flag2)
                        {
                            RuleAttackWithWeapon ruleAttackWithWeapon = new RuleAttackWithWeapon(caster, target.Unit, weapon, 0)
                            {
                                IsFirstAttack = true,
                                SkipAnimation = true,
                                SkipMainDamage = true
                            };
                            if (this.Weapon.Type.AttackType == AttackType.RangedTouch)
                            {
                                ruleAttackWithWeapon.ReplaceAttackType = true;
                                ruleAttackWithWeapon.AttackTypeReplacement = AttackType.Touch;
                            }
                            RuleAttackWithWeapon ruleAttackWithWeapon2 = context.TriggerRule<RuleAttackWithWeapon>(ruleAttackWithWeapon);
                            attackRoll = ruleAttackWithWeapon2.AttackRoll;
                        }
                        else
                        {
                            attackRoll = new RuleAttackRoll(caster, target.Unit, weapon, 0)
                            {
                                SuspendCombatLog = true
                            };
                            attackRoll = context.TriggerRule<RuleAttackRoll>(attackRoll);
                        }
                    }
                }
                if (context.ForceAlwaysHit)
                {
                    attackRoll.SetFake(AttackResult.Hit);
                }
                proj.AttackRoll = attackRoll;
                proj.MissTarget = context.MissTarget;
            }
            yield return null;
            AbilityProjectileType type = this.Type;
            if (type != AbilityProjectileType.Simple)
            {
                if (type - AbilityProjectileType.Line > 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                IEnumerator<AbilityDeliveryTarget> deliver = this.StepBasedDeliver(context, castPosition, direction, proj, caster);
                while (deliver.MoveNext())
                {
                    AbilityDeliveryTarget abilityDeliveryTarget = deliver.Current;
                    yield return abilityDeliveryTarget;
                }
            }
            else
            {
                while (!proj.IsHit && !proj.Cleared)
                {
                    yield return null;
                }
                RuleAttackRoll ruleAttackRoll = attackRoll;
                if (ruleAttackRoll != null)
                {
                    ruleAttackRoll.ConsumeMirrorImageIfNecessary();
                }
                if (proj.Cleared)
                {
                    yield break;
                }
                target = proj.Target;
                yield return new AbilityDeliveryTarget(target)
                {
                    AttackRoll = attackRoll,
                    Projectile = proj
                };
            }
            yield break;
        }

        protected virtual float GetProjectileRange(AbilityExecutionContext context, bool reach)
        {
            return context.AbilityBlueprint.GetRange(reach, context.Ability).Meters;
        }

        protected virtual IEnumerator<AbilityDeliveryTarget> StepBasedDeliver(AbilityExecutionContext context, Vector3 castPosition, Vector3 direction, Projectile proj, UnitEntityData caster)
        {
            Vector2 dir2d = direction.To2D().normalized;
            AbilityEffectRunActionOnClickedPoint component = context.AbilityBlueprint.GetComponent<AbilityEffectRunActionOnClickedPoint>();
            if (component != null)
            {
                Vector3 vector = context.ClickedTarget.Point;
                if ((this.Type == AbilityProjectileType.Line && !this.WouldTargetPointLine(vector, 0.01f, castPosition, dir2d, proj.MaxRange)) || (this.Type == AbilityProjectileType.Cone && !this.WouldTargetPointCone(castPosition, vector, 0.01f, dir2d, proj.MaxRange)))
                {
                    vector = castPosition + dir2d.To3D() * proj.MaxRange / 2f;
                }
                component.Apply(context, vector);
            }
            HashSet<UnitEntityData> usedUnits = new HashSet<UnitEntityData>();
            for (; ; )
            {
                yield return null;
                if (proj.Cleared)
                {
                    break;
                }
                foreach (UnitEntityData unitEntityData in Game.Instance.State.Units)
                {
                    if (!unitEntityData.Descriptor.State.IsDead && !(unitEntityData == context.MaybeCaster))
                    {
                        float passedDistance = proj.PassedDistance;
                        if ((this.Type != AbilityProjectileType.Line || this.WouldTargetUnitLine(context, unitEntityData, castPosition, dir2d, passedDistance)) && (this.Type != AbilityProjectileType.Cone || this.WouldTargetUnitCone(context, caster, unitEntityData, castPosition, dir2d, passedDistance)) && !usedUnits.Contains(unitEntityData))
                        {
                            usedUnits.Add(unitEntityData);
                            yield return new AbilityDeliveryTarget(unitEntityData)
                            {
                                Projectile = proj
                            };
                        }
                    }
                }
                EntityPoolEnumerator<UnitEntityData> entityPoolEnumerator = default(EntityPoolEnumerator<UnitEntityData>);
                if (proj.IsHit)
                {
                    goto Block_7;
                }
            }
            yield break;
        Block_7:
            yield break;
        }

        private static bool IsLineAndCircleIntersects(Vector2 start, Vector2 dir, float distance, Vector2 circleOrigin, float radius)
        {
            Vector2 vector = start + dir * distance;
            return (vector - circleOrigin).sqrMagnitude <= radius * radius || VectorMath.SqrDistancePointSegment(start, vector, circleOrigin) <= radius * radius;
        }

        private static Vector2 RotateVec2D(Vector2 v, float a)
        {
            return Quaternion.Euler(0f, 0f, a) * v;
        }

        private bool WouldTargetUnitCone(AbilityExecutionContext contextAbility, UnitEntityData caster, UnitEntityData unit, Vector3 launchPos, Vector2 castDir, float distance)
        {
            return !(caster == unit) && this.CanTargetUnit(contextAbility, unit) && this.WouldTargetPointCone(launchPos, unit.EyePosition, unit.View.Corpulence, castDir, distance);
        }

        public bool WouldTargetPointCone(Vector3 launchPos, Vector3 targetPos, float targetRadius, Vector2 castDir, float distance)
        {
            if (LineOfSightGeometry.Instance.HasObstacle(launchPos, targetPos, 0f))
            {
                return false;
            }
            Vector2 vector = targetPos.To2D();
            Vector2 vector2 = launchPos.To2D();
            return Vector2.Distance(vector, vector2) <= distance + targetRadius && (Mathf.Abs(Vector2.Angle(vector - vector2, castDir)) <= 45f || AbilityDeliverProjectile.IsLineAndCircleIntersects(vector2, AbilityDeliverProjectile.RotateVec2D(castDir, 45f), distance, vector, targetRadius) || AbilityDeliverProjectile.IsLineAndCircleIntersects(vector2, AbilityDeliverProjectile.RotateVec2D(castDir, -45f), distance, vector, targetRadius));
        }

        private bool WouldTargetUnitLine(AbilityExecutionContext contextAbility, UnitEntityData unit, Vector3 launchPos, Vector2 castDir, float distance)
        {
            return this.CanTargetUnit(contextAbility, unit) && this.WouldTargetPointLine(unit.EyePosition, unit.Corpulence, launchPos, castDir, distance);
        }

        private bool WouldTargetPointLine(Vector3 targetPos, float targetRadius, Vector3 launchPos, Vector2 castDir, float distance)
        {
            float num = this.LineWidth.Meters * 0.5f + targetRadius;
            float num2 = Vector2.Dot((targetPos - launchPos).To2D(), castDir);
            if (num2 <= 0f || num2 >= distance + targetRadius)
            {
                return false;
            }
            Vector2 b = castDir * num2;
            return ((targetPos - launchPos).To2D() - b).sqrMagnitude <= num * num;
        }

        public override void ApplyValidation(ValidationContext context, int parentIndex)
        {
            base.ApplyValidation(context, parentIndex);
            if (!this.IsPierceOrCone && this.NeedAttackRoll && !this.Weapon && !this.IsHandOfTheApprentice)
            {
                context.AddError("'NeedAttackRoll' is true but 'Weapon' is not specified", Array.Empty<object>());
            }
        }

        public static Vector3 CorrectPosition(Vector3 resultPoint, Vector3 casterPosition)
        {
            Vector3 vector = resultPoint - casterPosition;
            if (vector == Vector3.zero)
            {
                vector = Vector3.forward;
                resultPoint += vector;
            }
            if (vector.sqrMagnitude > 0.09f)
            {
                resultPoint = casterPosition + vector.normalized * 0.3f;
            }
            return resultPoint;
        }

        public virtual bool WouldTargetUnit(AbilityData ability, Vector3 targetPos, UnitEntityData unit, AbilityParams cachedParams = null)
        {
            UnitEntityData unit2 = ability.Caster.Unit;
            Vector3 normalized = (targetPos - unit2.Position).normalized;
            Vector3 launchPos = unit2.EyePosition + normalized * unit2.Corpulence;
            float meters = ability.GetDeliverBlueprint(false).GetRange(ability.HasMetamagic(Metamagic.Reach), ability).Meters;
            AbilityExecutionContext contextAbility = ability.CreateExecutionContext(new TargetWrapper(targetPos, null, null), cachedParams);
            AbilityProjectileType type = this.Type;
            if (type != AbilityProjectileType.Line)
            {
                return type == AbilityProjectileType.Cone && this.WouldTargetUnitCone(contextAbility, ability.Caster.Unit, unit, launchPos, normalized.To2D(), meters);
            }
            return this.WouldTargetUnitLine(contextAbility, unit, launchPos, normalized.To2D(), meters);
        }

        [CanBeNull]
        public ControlledProjectileHolder.Runtime FindControlledProjectileRuntime(UnitEntityData caster)
        {
            BlueprintBuff controlledProjectileHolderBuff = this.ControlledProjectileHolderBuff;
            if (controlledProjectileHolderBuff == null)
            {
                return null;
            }
            Buff buff = caster.Buffs.GetBuff(controlledProjectileHolderBuff);
            return ((buff != null) ? buff.GetComponentWithRuntime<ControlledProjectileHolder>().Runtime : null) as ControlledProjectileHolder.Runtime;
        }

        private bool CanTargetUnit(AbilityExecutionContext context, UnitEntityData unit)
        {
            return !context.HasMetamagic(Metamagic.Selective) || !context.Ability.Caster.Unit.IsAlly(unit) || (((!context.Ability.Blueprint.CanTargetFriends || !context.Ability.Blueprint.CanTargetEnemies) && !context.Ability.CanTargetPoint) || base.HasIsAllyConditions);
        }

        private static IEnumerator<object> HandleControlledProjectile([NotNull] ControlledProjectileHolder.Runtime runtime, [NotNull] AbilityExecutionContext context)
        {
            IEnumerator prepareLaunch = AbilityDeliverProjectile.PrepareControlledProjectileLaunch(context.MainTarget, runtime.Owner, runtime.BuffReference);
            while (prepareLaunch.MoveNext())
            {
                yield return null;
            }
            yield break;
        }

        private static IEnumerator PrepareControlledProjectileLaunch(TargetWrapper target, UnitEntityData owner, BlueprintBuffReference buffReference)
        {
            ControllableProjectileContainerPart container = owner.Get<ControllableProjectileContainerPart>();
            if (container == null)
            {
                yield break;
            }
            ControllableProjectile projectile = container.Get(buffReference);
            if (projectile.ObjectHandle == null)
            {
                yield break;
            }
            while (!projectile.ObjectHandle.IsSpawned)
            {
                yield return null;
            }
            container.Remove(buffReference);
            Quaternion initialRotation = projectile.ObjectHandle.SpawnedObject.transform.rotation;
            Vector3 initialPosition = projectile.ObjectHandle.SpawnedObject.transform.position;
            Vector3 projectileUp = projectile.ObjectHandle.SpawnedObject.transform.up;
            float currentRotationLifetime = 0f;
            BlueprintControllableProjectile blueprintControllableProjectile = projectile.ControllableProjectileReference.Get();
            AnimationCurve rotationCurve = blueprintControllableProjectile.RotationCurve;
            float rotationLifetime = blueprintControllableProjectile.RotationLifetime;
            projectile.OnPreparationStart();
            do
            {
                float time = currentRotationLifetime / rotationLifetime;
                float t = rotationCurve.Evaluate(time);
                Vector3 toDirection = target.Point - initialPosition;
                Quaternion b = Quaternion.FromToRotation(projectileUp, toDirection) * initialRotation;
                Quaternion rotation = Quaternion.Lerp(initialRotation, b, t);
                projectile.ObjectHandle.SpawnedObject.transform.rotation = rotation;
                yield return null;
                currentRotationLifetime += Game.Instance.TimeController.GameDeltaTime;
            }
            while (currentRotationLifetime <= rotationLifetime);
            projectile.OnPreparationFinished();
            FxHelper.Destroy(projectile.ObjectHandle, true);
            yield break;
        }

        private const float ConeAngle = 90f;

        [SerializeField]
        [FormerlySerializedAs("Projectiles")]
        public BlueprintProjectileReference[] m_Projectiles;

        public AbilityProjectileType Type;

        public bool UseCastPositionInsteadCasterPosition;

        [HideIf("IsPierceOrCone")]
        public bool IsHandOfTheApprentice;

        [ShowIf("UseFeet")]
        [SerializeField]
        [FormerlySerializedAs("Length")]
        public Feet m_Length;

        [ShowIf("UseFeet")]
        [SerializeField]
        [FormerlySerializedAs("LineWidth")]
        public Feet m_LineWidth = 5.Feet();

        [HideIf("IsPierceOrCone")]
        [FormerlySerializedAs("IsRangedTouch")]
        public bool NeedAttackRoll;

        [ShowIf("ShowWeapon")]
        [FormerlySerializedAs("TouchWeapon")]
        [SerializeField]
        [FormerlySerializedAs("Weapon")]
        public BlueprintItemWeaponReference m_Weapon;

        [ShowIf("NeedAttackRoll")]
        public bool ReplaceAttackRollBonusStat;

        [ShowIf("ShowAttackRollBonusStat")]
        public StatType AttackRollBonusStat;

        public bool UseMaxProjectilesCount;

        [ShowIf("UseMaxProjectilesCount")]
        public AbilityRankType MaxProjectilesCountRank;

        [ShowIf("IsMultipleProjectiles")]
        public float DelayBetweenProjectiles;

        [SerializeField]
        [FormerlySerializedAs("ControlledProjectileHolderBuff")]
        public BlueprintBuffReference m_ControlledProjectileHolderBuff;

        public float m_SecondaryTargetMaxDistance = 0f;
    }
}
