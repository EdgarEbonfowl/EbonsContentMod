using HarmonyLib;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.Class;
using Kingmaker.UI.MVVM._VM.Other.NestedSelectionGroup;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Utility;
using Kingmaker.Designers.Mechanics.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using TabletopTweaks.Core.NewComponents;
using TabletopTweaks.Core.Config;
using Kingmaker.UnitLogic.Abilities.Components;
using BlueprintCore.Blueprints.References;
using TabletopTweaks.Core.Utilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;

namespace EbonsContentMod.Utilities
{
    internal class MultiProjectileUtilities
    {
        /// <summary>
        /// Enables target switching for a multi-projectile spell after a current target dies.
        /// </summary>
        /// <remarks>
        /// <para>
        /// SecondarTargetMaxDistance = The max distance any two targets can be apart and still be targeted by a projectile. Implemented as radius (half given distance) from the original target.
        /// </para>
        /// <para>
        /// Note: If no delay between projectiles exists, as with Magic Missile, a 0.1 second delay is added.
        /// </para>
        /// </remarks>
        public static void MultiProjectileFixer(BlueprintAbility spell, float SecondaryTargetMaxDistance = 0f)
        {
            var component = spell.GetComponent<AbilityDeliverProjectile>();

            var projectiles = component.m_Projectiles;
            var type = component.Type;
            var length = component.m_Length;
            var linewidth = component.m_LineWidth;
            var UseCastPositionInsteadCasterPosition = component.UseCastPositionInsteadCasterPosition;
            var IsHandOfTheApprentice = component.IsHandOfTheApprentice;
            var NeedAttackRoll = component.NeedAttackRoll;
            var Weapon = component.m_Weapon;
            var ReplaceAttackRollBonusStat = component.ReplaceAttackRollBonusStat;
            var AttackRollBonusStat = component.AttackRollBonusStat;
            var UseMaxProjectilesCount = component.UseMaxProjectilesCount;
            var MaxProjectilesCountRank = component.MaxProjectilesCountRank;
            var DelayBetweenProjectiles = component.DelayBetweenProjectiles;
            if (DelayBetweenProjectiles < 0.1f) DelayBetweenProjectiles = 0.1f; // Adding this to allow for better target checking between projectiles, hopefully it doesn't look ridiculous - should only affect Magic Missile and Gloomblind Bolts
            var ControlledProjectileHolderBuff = component.m_ControlledProjectileHolderBuff;

            AbilityConfigurator.For(spell)
                .RemoveComponents(c => c is AbilityDeliverProjectile)
                .AddComponent<AbilityDeliverProjectileDynamic>(c =>
                {
                    c.m_Projectiles = projectiles;
                    c.Type = type;
                    c.UseCastPositionInsteadCasterPosition = UseCastPositionInsteadCasterPosition;
                    c.IsHandOfTheApprentice = IsHandOfTheApprentice;
                    c.m_Length = length;
                    c.m_LineWidth = linewidth;
                    c.NeedAttackRoll = NeedAttackRoll;
                    c.m_Weapon = Weapon;
                    c.ReplaceAttackRollBonusStat = ReplaceAttackRollBonusStat;
                    c.AttackRollBonusStat = AttackRollBonusStat;
                    c.UseMaxProjectilesCount = UseMaxProjectilesCount;
                    c.MaxProjectilesCountRank = MaxProjectilesCountRank;
                    c.DelayBetweenProjectiles = DelayBetweenProjectiles;
                    c.m_ControlledProjectileHolderBuff = ControlledProjectileHolderBuff;
                    c.m_SecondaryTargetMaxDistance = SecondaryTargetMaxDistance;
                })
                .Configure();
        }
    }
}
