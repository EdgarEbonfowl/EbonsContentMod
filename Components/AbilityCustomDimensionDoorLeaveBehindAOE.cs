using System;
using System.Collections;
using System.Collections.Generic;
using Kingmaker.Armies.TacticalCombat;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers;
using Kingmaker.Controllers.Projectiles;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.Utility;
using Kingmaker.View;
using Kingmaker.View.MapObjects.SriptZones;
using Kingmaker.Visual.Animation.Actions;
using Kingmaker.Visual.Critters;
using Kingmaker.Visual.Particles;
using Kingmaker.Visual.Particles.FxSpawnSystem;
using Locations.Alushenyrra;
using Owlcat.Runtime.Core.Utils;
using UnityEngine;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.UnitLogic.Mechanics;
using UnityEngine.Serialization;
using BlueprintCore.Blueprints.Configurators.AI;
using BlueprintCore.Blueprints.References;
using System.Runtime.Remoting.Contexts;

namespace EbonsContentMod.Components
{
    internal class AbilityCustomDimensionDoorLeaveBehindAOE : AbilityCustomLogic, IAbilityTargetRestriction
    {
        public static Vector3 InitialCasterPosition = new Vector3() { x = -1, y = -1, z = -1};
        public static bool DidAOE = false;

        public bool CameraShouldFollow
        {
            get
            {
                return this.m_CameraShouldFollow;
            }
        }

        public BlueprintProjectile CasterDisappearProjectile
        {
            get
            {
                BlueprintProjectileReference casterDisappearProjectile = this.m_CasterDisappearProjectile;
                if (casterDisappearProjectile == null)
                {
                    return null;
                }
                return casterDisappearProjectile.Get();
            }
        }

        public BlueprintProjectile CasterAppearProjectile
        {
            get
            {
                BlueprintProjectileReference casterAppearProjectile = this.m_CasterAppearProjectile;
                if (casterAppearProjectile == null)
                {
                    return null;
                }
                return casterAppearProjectile.Get();
            }
        }

        public BlueprintProjectile SideDisappearProjectile
        {
            get
            {
                BlueprintProjectileReference sideDisappearProjectile = this.m_SideDisappearProjectile;
                if (sideDisappearProjectile == null)
                {
                    return null;
                }
                return sideDisappearProjectile.Get();
            }
        }

        public BlueprintProjectile SideAppearProjectile
        {
            get
            {
                BlueprintProjectileReference sideAppearProjectile = this.m_SideAppearProjectile;
                if (sideAppearProjectile == null)
                {
                    return null;
                }
                return sideAppearProjectile.Get();
            }
        }

        public BlueprintAbilityAreaEffect AreaEffect
        {
            get
            {
                BlueprintAbilityAreaEffectReference areaEffect = this.m_AreaEffect;
                if (areaEffect == null)
                {
                    return null;
                }
                return areaEffect.Get();
            }
        }

        public override bool IsEngageUnit
        {
            get
            {
                return true;
            }
        }

        protected virtual bool LookAtTarget
        {
            get
            {
                return false;
            }
        }

        protected virtual bool RelaxPoints
        {
            get
            {
                return true;
            }
        }

        public override IEnumerator<AbilityDeliveryTarget> Deliver(AbilityExecutionContext context, TargetWrapper target)
        {
            DidAOE = false;
            
            DimensionDoorSettings settings = new DimensionDoorSettings
            {
                PortalFromPrefab = this.PortalFromPrefab.Load(false, false),
                PortalToPrefab = this.PortalToPrefab.Load(false, false),
                PortalBone = this.PortalBone,
                CasterDisappearFx = this.CasterDisappearFx.Load(false, false),
                CasterAppearFx = this.CasterAppearFx.Load(false, false),
                SideDisappearFx = this.SideDisappearFx.Load(false, false),
                SideAppearFx = this.SideAppearFx.Load(false, false),
                CasterDisappearProjectile = this.CasterDisappearProjectile,
                CasterAppearProjectile = this.CasterAppearProjectile,
                SideDisappearProjectile = this.SideDisappearProjectile,
                SideAppearProjectile = this.SideAppearProjectile,
                Targets = this.GetTargets(context.Caster),
                LookAtTarget = this.LookAtTarget,
                CameraShouldFollow = this.CameraShouldFollow,
                RelaxPoints = this.RelaxPoints,
                UseAnimation = this.UseAnimations
            };
            if (this.UseAnimations)
            {
                settings.TakeOffAnimation = this.TakeOffAnimation;
                settings.TakeoffTime = this.TakeoffTime;
                settings.CasterDisappearDuration = this.DissapearTime;
                settings.LandingAnimation = this.LandingAnimation;
                settings.LandingTime = this.LandingTime;
                settings.CasterAppearDuration = this.AppearTime;
            }
            TimeSpan? customMaxTime = null;
            if (this.UseAnimations)
            {
                customMaxTime = new TimeSpan?(new TimeSpan(0, 0, 0, 0, (int)(1000f * (this.TakeoffTime + this.LandingTime))));
            }
            IEnumerator<AbilityDeliveryTarget> deliver = Deliver(settings, context.Caster, target, customMaxTime, context, this.AreaEffect);
            while (deliver.MoveNext())
            {
                AbilityDeliveryTarget abilityDeliveryTarget = deliver.Current;
                yield return abilityDeliveryTarget;
            }
            yield break;
        }

        public static IEnumerator<AbilityDeliveryTarget> Deliver(DimensionDoorSettings settings, UnitEntityData caster, TargetWrapper target, TimeSpan? customMaxTime = null, AbilityExecutionContext context = null, BlueprintAbilityAreaEffect aoe = null)
        {
            Vector3 CasterLocation = caster.Position;
            InitialCasterPosition = CasterLocation;
            List<UnitEntityData> targets = settings.Targets;
            AddMounts(targets);
            Vector3 vector = target.Point;
            if (target.Unit != null)
            {
                float d = caster.View.Corpulence + target.Unit.View.Corpulence + GameConsts.MinWeaponRange.Meters;
                vector += Quaternion.Euler(0f, target.Unit.Orientation, 0f) * Vector3.forward * d;
            }
            Vector3[] points = new Vector3[targets.Count];
            float[] array = new float[targets.Count];
            for (int i = 0; i < targets.Count; i++)
            {
                points[i] = targets[i].Position - caster.Position + vector;
                array[i] = targets[i].View.Corpulence;
            }
            if (settings.RelaxPoints)
            {
                FreePlaceSelector.RelaxPoints(points, array, targets.Count, null);
            }
            Vector3 targetPoint = points[0];
            IFxHandle portalFrom = FxHelper.SpawnFxOnUnit(settings.PortalFromPrefab, caster.View, null, null, default(Vector3), FxPriority.EventuallyImportant);
            IFxHandle portalTo = FxHelper.SpawnFxOnUnit(settings.PortalToPrefab, caster.View, null, null, default(Vector3), FxPriority.EventuallyImportant);
            for (; ; )
            {
                IFxHandle fxHandle = portalTo;
                if (fxHandle == null || fxHandle.IsSpawned)
                {
                    IFxHandle fxHandle2 = portalFrom;
                    if (fxHandle2 == null || fxHandle2.IsSpawned)
                    {
                        break;
                    }
                }
                yield return null;
            }
            if (portalTo != null)
            {
                portalTo.SpawnedObject.transform.position = targetPoint;
            }
            IFxHandle fxHandle3 = portalFrom;
            Transform unityObject;
            if (fxHandle3 == null)
            {
                unityObject = null;
            }
            else
            {
                GameObject gameObject = fxHandle3.SpawnedObject.Or(null);
                unityObject = ((gameObject != null) ? gameObject.transform.FindChildRecursive(settings.PortalBone) : null);
            }
            Transform transform = unityObject.Or(null);
            Vector3 value = (transform != null) ? transform.transform.position : caster.Position;
            IFxHandle fxHandle4 = portalTo;
            Transform unityObject2;
            if (fxHandle4 == null)
            {
                unityObject2 = null;
            }
            else
            {
                GameObject gameObject2 = fxHandle4.SpawnedObject.Or(null);
                unityObject2 = ((gameObject2 != null) ? gameObject2.transform.FindChildRecursive(settings.PortalBone) : null);
            }
            Transform transform2 = unityObject2.Or(null);
            Vector3 value2 = (transform2 != null) ? transform2.transform.position : targetPoint;
            List<IEnumerator> teleportationRoutines = new List<IEnumerator>();
            for (int j = 0; j < targets.Count; j++)
            {
                UnitEntityData unitEntityData = targets[j];
                Vector3 targetPosition = points[j];
                unitEntityData.Wake(10f, false);
                IFxHandle fxHandle5 = portalFrom;
                Vector3? intermediateFromPosition = (((fxHandle5 != null) ? fxHandle5.SpawnedObject : null) != null) ? new Vector3?(value) : null;
                IFxHandle fxHandle6 = portalTo;
                Vector3? intermediateToPosition = (((fxHandle6 != null) ? fxHandle6.SpawnedObject : null) != null) ? new Vector3?(value2) : null;
                IEnumerator item = CreateTeleportationRoutine(settings, unitEntityData, target, targetPosition, intermediateFromPosition, intermediateToPosition, j == 0, customMaxTime);
                teleportationRoutines.Add(item);
            }
            TimeSpan maxTime = customMaxTime ?? MaxTeleportationDuration;
            TimeSpan startTime = Game.Instance.TimeController.GameTime;
            bool needStop = false;
            do
            {
                for (int k = 0; k < teleportationRoutines.Count; k++)
                {
                    if (!teleportationRoutines[k].MoveNext())
                    {
                        teleportationRoutines.RemoveAt(k);
                        k--;
                    }
                }
                needStop = (teleportationRoutines.Count == 0 || Game.Instance.TimeController.GameTime - startTime >= maxTime);
                
                yield return null;
            }
            while (!needStop);

            // Do the AOE!
            if (DidAOE == false)
            {
                DidAOE = true;
                
                Rounds rounds = 1.Rounds();

                List<ClassData> classDatas = caster.Descriptor.Progression.Classes;
                foreach (ClassData classdata in classDatas)
                {
                    if (classdata.CharacterClass == CharacterClassRefs.SorcererClass.Reference.Get())
                    {
                        rounds = classdata.Level.Rounds();
                        break;
                    }
                }

                TimeSpan seconds = rounds.Seconds;
                AreaEffectEntityData areaEffectEntityData = AreaEffectsController.Spawn(context, aoe, InitialCasterPosition, new TimeSpan?(seconds));
                if (areaEffectEntityData != null && context != null)
                {
                    using (EntityPoolEnumerator<UnitEntityData> enumerator = Game.Instance.State.Units.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            UnitEntityData u = enumerator.Current;
                            if (!u.Descriptor.State.IsDead && u.IsInGame && areaEffectEntityData.Blueprint.Shape != AreaEffectShape.AllArea && areaEffectEntityData.View.Contains(u) && (areaEffectEntityData.AffectEnemies || !context.Caster.IsEnemy(u)))
                            {
                                EventBus.RaiseEvent<IApplyAbilityEffectHandler>(delegate (IApplyAbilityEffectHandler h)
                                {
                                    h.OnTryToApplyAbilityEffect(context, u);
                                }, true);
                            }
                        }
                    }
                }
            }
            
            yield return new AbilityDeliveryTarget(target);
            yield break;
        }

        private static IEnumerator CreateTeleportationRoutine(DimensionDoorSettings settings, UnitEntityData unit, TargetWrapper spellTarget, Vector3 targetPosition, Vector3? intermediateFromPosition, Vector3? intermediateToPosition, bool isCaster, TimeSpan? customMaxTime = null)
        {
            GameObject prefab = isCaster ? settings.CasterDisappearFx : settings.SideDisappearFx;
            GameObject appearFx = isCaster ? settings.CasterAppearFx : settings.SideAppearFx;
            BlueprintProjectile disappearProjectile = isCaster ? settings.CasterDisappearProjectile : settings.SideDisappearProjectile;
            BlueprintProjectile appearProjectile = isCaster ? settings.CasterAppearProjectile : settings.SideAppearProjectile;
            BlueprintProjectile teleportationProjectile = isCaster ? settings.CasterTeleportationProjectile : settings.SideTeleportationProjectile;
            float appearDuration = isCaster ? settings.CasterAppearDuration : settings.SideAppearDuration;
            float disappearDuration = isCaster ? settings.CasterDisappearDuration : settings.SideDisappearDuration;
            bool useAnimation = settings.UseAnimation;
            UnitAnimationActionLink takeOffAnimation = settings.TakeOffAnimation;
            float takeoffTime = settings.TakeoffTime;
            UnitAnimationActionLink landingAnimation = settings.LandingAnimation;
            float landingTime = settings.LandingTime;
            appearDuration = Math.Max(appearDuration, 0.3f);
            unit.View.StopMoving();
            unit.AddBuff(BlueprintRoot.Instance.SystemMechanics.DimensionDoorBuff, null, new TimeSpan?(customMaxTime ?? MaxTeleportationDuration));
            FxHelper.SpawnFxOnUnit(prefab, unit.View, null, null, default(Vector3), FxPriority.EventuallyImportant);
            bool wasHidden = false;
            if (useAnimation)
            {
                AnimationActionHandle handle2 = unit.View.AnimationManager.CreateHandle(takeOffAnimation.Load(false, false));
                unit.View.AnimationManager.Execute(handle2);
                TimeSpan startTime = Game.Instance.TimeController.GameTime;
                while (Game.Instance.TimeController.GameTime - startTime < takeoffTime.Seconds())
                {
                    if (!wasHidden && Game.Instance.TimeController.GameTime - startTime >= disappearDuration.Seconds())
                    {
                        unit.View.Fader.Visible = false;
                        unit.View.Fader.Force();
                        wasHidden = true;
                    }
                    yield return null;
                }
                startTime = default(TimeSpan);
            }
            else if (disappearDuration > 0.01f)
            {
                if (TacticalCombatHelper.IsActive)
                {
                    yield return new WaitForSeconds(disappearDuration);
                }
                else
                {
                    TimeSpan startTime = Game.Instance.TimeController.GameTime;
                    while (Game.Instance.TimeController.GameTime - startTime < disappearDuration.Seconds())
                    {
                        yield return null;
                    }
                    startTime = default(TimeSpan);
                }
            }
            if (disappearProjectile != null && intermediateFromPosition != null)
            {
                IEnumerator projectileRoutine = CreateProjectileRoutine(disappearProjectile, unit, new Vector3?(unit.Position), intermediateFromPosition.Value);
                while (projectileRoutine.MoveNext())
                {
                    yield return null;
                }
                projectileRoutine = null;
            }
            if (teleportationProjectile != null)
            {
                Vector3 targetPosition2 = intermediateToPosition ?? targetPosition;
                IEnumerator projectileRoutine = CreateProjectileRoutine(teleportationProjectile, unit, intermediateFromPosition, targetPosition2);
                while (projectileRoutine.MoveNext())
                {
                    yield return null;
                }
                projectileRoutine = null;
            }
            unit.CombatState.PreventAttacksOfOpporunityNextFrame = true;
            unit.Position = targetPosition;
            if (settings.LookAtTarget)
            {
                unit.ForceLookAt(spellTarget.Point);
            }
            if (settings.CameraShouldFollow)
            {
                Game.Instance.UI.GetCameraRig().ScrollTo(spellTarget.Point, false);
            }
            if (appearProjectile != null && intermediateToPosition != null)
            {
                IEnumerator projectileRoutine = CreateProjectileRoutine(appearProjectile, unit, intermediateToPosition, targetPosition);
                while (projectileRoutine.MoveNext())
                {
                    yield return null;
                }
                projectileRoutine = null;
            }
            else
            {
                yield return null;
            }
            FxHelper.SpawnFxOnUnit(appearFx, unit.View, null, null, default(Vector3), FxPriority.EventuallyImportant);
            if (useAnimation)
            {
                AnimationActionHandle handle = unit.View.AnimationManager.CreateHandle(landingAnimation.Load(false, false));
                unit.View.AnimationManager.Execute(handle);
                TimeSpan startTime = Game.Instance.TimeController.GameTime;
                while (!handle.IsFinished && Game.Instance.TimeController.GameTime - startTime < landingTime.Seconds())
                {
                    if (Game.Instance.TurnBasedCombatController.WaitingForUI)
                    {
                        yield return null;
                    }
                    else
                    {
                        if (wasHidden && Game.Instance.TimeController.GameTime - startTime >= appearDuration.Seconds())
                        {
                            unit.View.Fader.Visible = true;
                            unit.View.Fader.Force();
                            wasHidden = false;
                        }
                        unit.CombatState.PreventAttacksOfOpporunityNextFrame = true;
                        yield return null;
                    }
                }
                if (wasHidden)
                {
                    unit.View.Fader.Visible = true;
                    unit.View.Fader.Force();
                }
                handle = null;
                startTime = default(TimeSpan);
            }
            else if (TacticalCombatHelper.IsActive)
            {
                yield return new WaitForSeconds(appearDuration);
            }
            else
            {
                TimeSpan startTime = Game.Instance.TimeController.GameTime;
                while (Game.Instance.TimeController.GameTime - startTime < appearDuration.Seconds())
                {
                    yield return null;
                }
                startTime = default(TimeSpan);
            }
            unit.Buffs.RemoveFact(BlueprintRoot.Instance.SystemMechanics.DimensionDoorBuff);
            using (List<Familiar>.Enumerator enumerator = unit.Familiars.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Familiar unityObject = enumerator.Current;
                    Familiar familiar = unityObject.Or(null);
                    if (familiar != null)
                    {
                        familiar.TeleportToMaster(false);
                    }
                }
                yield break;
            }
            yield break;
        }

        private static IEnumerator CreateProjectileRoutine(BlueprintProjectile blueprint, UnitEntityData unit, Vector3? sourcePosition, Vector3 targetPosition)
        {
            bool projectileLanded = false;
            if (sourcePosition != null)
            {
                Game.Instance.ProjectileController.Launch(unit, targetPosition, blueprint, sourcePosition.Value, delegate (Projectile p)
                {
                    projectileLanded = true;
                });
            }
            else
            {
                Game.Instance.ProjectileController.Launch(unit, targetPosition, blueprint, delegate (Projectile p)
                {
                    projectileLanded = true;
                });
            }
            while (!projectileLanded)
            {
                yield return null;
            }
            yield break;
        }

        public override void Cleanup(AbilityExecutionContext context)
        {
            var caster = context.Caster;

            caster.Buffs.RemoveFact(BlueprintRoot.Instance.SystemMechanics.DimensionDoorBuff);
            
        }

        protected virtual List<UnitEntityData> GetTargets(UnitEntityData caster)
        {
            List<UnitEntityData> list = new List<UnitEntityData>
            {
                caster
            };
            if (this.Radius > 0.Feet())
            {
                for (int i = 0; i < caster.Group.Count; i++)
                {
                    UnitEntityData unitEntityData = caster.Group[i];
                    if (!(unitEntityData == null) && !(unitEntityData == caster) && unitEntityData.DistanceTo(caster) <= this.Radius.Meters + unitEntityData.View.Corpulence + caster.View.Corpulence)
                    {
                        list.Add(unitEntityData);
                    }
                }
            }
            return list;
        }

        private static void AddMounts(List<UnitEntityData> targets)
        {
            int count = targets.Count;
            for (int i = 0; i < count; i++)
            {
                UnitEntityData saddledUnit = targets[i].GetSaddledUnit();
                if (saddledUnit != null && !targets.Contains(saddledUnit))
                {
                    targets.Add(saddledUnit);
                }
            }
        }

        public bool IsTargetRestrictionPassed(UnitEntityData caster, TargetWrapper target)
        {
            return base.OwnerBlueprint.GetComponent<DimensionDoorRestrictionIgnorance>() != null || (!CheckTargetIsOnDisabledIsland(target) && ObstacleAnalyzer.IsPointInsideNavMesh(target.Point) && !FogOfWarController.IsInFogOfWar(target.Point));
        }

        public static bool CheckTargetIsOnDisabledIsland(TargetWrapper target)
        {
            foreach (IsleStateControllerView.IsleData isleData in Game.Instance.State.FlyingIsles)
            {
                ScriptZone controllingZone = isleData.ControllerView.ControllingZone;
                if (controllingZone != null && controllingZone.ContainsPosition(target.Point) && !isleData.ControllerView.IsInZeroState)
                {
                    return true;
                }
            }
            return false;
        }

        public string GetAbilityTargetRestrictionUIText(UnitEntityData caster, TargetWrapper target)
        {
            return LocalizedTexts.Instance.Reasons.TargetIsInvalid;
        }

        protected static readonly TimeSpan MaxTeleportationDuration = 3f.Seconds();

        public Feet Radius;

        public PrefabLink PortalFromPrefab;

        public PrefabLink PortalToPrefab;

        public string PortalBone = "";

        public PrefabLink CasterDisappearFx;

        public PrefabLink CasterAppearFx;

        public PrefabLink SideDisappearFx;

        public PrefabLink SideAppearFx;

        [SerializeField]
        public BlueprintAbilityAreaEffectReference m_AreaEffect;

        public ContextDurationValue DurationValue;

        [SerializeField]
        public BlueprintProjectileReference m_CasterDisappearProjectile;

        [SerializeField]
        public BlueprintProjectileReference m_CasterAppearProjectile;

        [SerializeField]
        public BlueprintProjectileReference m_SideDisappearProjectile;

        [SerializeField]
        public BlueprintProjectileReference m_SideAppearProjectile;

        [SerializeField]
        public bool m_CameraShouldFollow;

        public bool UseAnimations;

        [ShowIf("UseAnimations")]
        public UnitAnimationActionLink TakeOffAnimation;

        [ShowIf("UseAnimations")]
        public float TakeoffTime = 1f;

        [ShowIf("UseAnimations")]
        public float DissapearTime = 1f;

        [ShowIf("UseAnimations")]
        public UnitAnimationActionLink LandingAnimation;

        [ShowIf("UseAnimations")]
        public float LandingTime = 1f;

        [ShowIf("UseAnimations")]
        public float AppearTime = 1f;
    }
}
