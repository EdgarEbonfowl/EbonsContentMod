using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using EbonsContentMod.Utilities;

namespace EbonsContentMod.Abilities
{
    internal class MultiProjectileSpellFix
    {
        internal static void Configure()
        {
            // Magic Missile
            MultiProjectileUtilities.MultiProjectileFixer(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.MagicMissile.ToString()), 15f);
            // Magic Missile - Swift
            MultiProjectileUtilities.MultiProjectileFixer(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.MagicMissileSwift.ToString()), 15f);
            // Battering Blast
            MultiProjectileUtilities.MultiProjectileFixer(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.BatteringBlast.ToString()));
            // Scorching Ray
            MultiProjectileUtilities.MultiProjectileFixer(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.ScorchingRay.ToString()), 30f);
            // Scorching Ray - Ki Power
            MultiProjectileUtilities.MultiProjectileFixer(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.KiScorchingRay.ToString()), 30f);
            // Scorching Ray - Drunken Ki Power
            MultiProjectileUtilities.MultiProjectileFixer(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.DrunkenKiScorchingRay.ToString()), 30f);
            // Scorching Ray - Scaled Fist Ki Power
            MultiProjectileUtilities.MultiProjectileFixer(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.ScaledFistScorchingRay.ToString()), 30f);
            // Scorching Ray - Tiefling Racial Ability
            MultiProjectileUtilities.MultiProjectileFixer(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.ScorchingRayTiefling.ToString()), 30f);
            // Scorching Ray - Staff Ability
            MultiProjectileUtilities.MultiProjectileFixer(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.ScorchingRayStaff.ToString()), 30f);
            // Scorching Ray - Acid
            MultiProjectileUtilities.MultiProjectileFixer(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.ScorchingRayAcid.ToString()), 30f);
            // Scorching Ray - Cold
            MultiProjectileUtilities.MultiProjectileFixer(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.ScorchingRayCold.ToString()), 30f);
            // Scorching Ray - Electricity
            MultiProjectileUtilities.MultiProjectileFixer(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.ScorchingRayElecricity.ToString()), 30f);
            // Hellfire Ray
            MultiProjectileUtilities.MultiProjectileFixer(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.HellfireRay.ToString()), 30f);
            // Hellfire Ray - Arcanist Exploit
            MultiProjectileUtilities.MultiProjectileFixer(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.ArcanistExploitHellfireRayAbility.ToString()), 30f);

            if (CheckerUtilities.GetModActive("ExpandedContent"))
            {
                // Gloomblind Bolts
                MultiProjectileUtilities.MultiProjectileFixer(BlueprintTools.GetBlueprint<BlueprintAbility>("e28f4633-c0a2-425d-8895-adf20cb22f8f"), 30f);
                // Steam Ray Fusillade
                MultiProjectileUtilities.MultiProjectileFixer(BlueprintTools.GetBlueprint<BlueprintAbility>("a8be30dd-f370-42d5-b56f-faa8eae976d6"), 30f);
                // Steam Ray Fusillade - Buff Ability
                MultiProjectileUtilities.MultiProjectileFixer(BlueprintTools.GetBlueprint<BlueprintAbility>("786bb462-6a58-4db5-b408-8dd666aece23"), 30f);
            }
        }
    }
}
