using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;

namespace EbonsContentMod.Traits
{
    internal static class GhostHunterPatches
    {
        private const string GhostHunterGuid =
            "3A405713-83F7-4537-AC5C-E58B44E86BC4";

        private static BlueprintFeature GhostHunter =>
            BlueprintTools.GetBlueprint<BlueprintFeature>(
                GhostHunterGuid);

        private static bool HasGhostHunter(
            RuleCalculateDamage evt)
        {
            return evt?.Initiator != null
                && evt.Initiator.HasFact(GhostHunter);
        }

        private static bool IsUsingEnchantedWeapon(
            RuleCalculateDamage evt)
        {
            return evt?.DamageBundle?.WeaponDamage
                is PhysicalDamage weaponDamage
                && weaponDamage.EnchantmentTotal > 0;
        }

        [HarmonyPatch(
            typeof(GhostCriticalAndPrecisionImmunity),
            nameof(GhostCriticalAndPrecisionImmunity.OnEventAboutToTrigger))]
        private static class
            GhostCriticalAndPrecisionImmunity_Patch
        {
            [HarmonyPrefix]
            private static bool Prefix(
                GhostCriticalAndPrecisionImmunity __instance,
                RuleCalculateDamage evt)
            {
                if (!HasGhostHunter(evt)
                    || !IsUsingEnchantedWeapon(evt))
                {
                    return true;
                }

                if (evt.DamageBundle.WeaponDamage != null
                    && (evt.DamageBundle.WeaponDamage.Reality
                        & DamageRealityType.Ghost) != 0)
                {
                    return false;
                }

                foreach (BaseDamage damage in
                         evt.DamageBundle)
                {
                    EnergyDamage energyDamage =
                        damage as EnergyDamage;

                    if (damage.Type == DamageType.Force
                        || damage.Type == DamageType.Direct
                        || (energyDamage != null
                            && energyDamage.EnergyType
                                == DamageEnergyType.PositiveEnergy)
                        || (energyDamage != null
                            && energyDamage.EnergyType
                                == DamageEnergyType.Holy)
                        || (energyDamage != null
                            && energyDamage.EnergyType
                                == DamageEnergyType.Unholy)
                        || (energyDamage != null
                            && energyDamage.EnergyType
                                == DamageEnergyType.Divine))
                    {
                        continue;
                    }

                    damage.AddDecline(
                        new DamageDecline(
                            DamageDeclineType.ByHalf,
                            __instance.Fact));
                }

                return false;
            }
        }

        [HarmonyPatch(
            typeof(AddIncorporealDamageDivisor),
            nameof(AddIncorporealDamageDivisor.OnEventAboutToTrigger))]
        private static class
            AddIncorporealDamageDivisor_Patch
        {
            [HarmonyPrefix]
            private static bool Prefix(
                AddIncorporealDamageDivisor __instance,
                RuleCalculateDamage evt)
            {
                if (!HasGhostHunter(evt))
                {
                    return true;
                }

                bool weaponAttack =
                    evt.DamageBundle.WeaponDamage != null;

                foreach (BaseDamage damage in
                         evt.DamageBundle)
                {
                    if (damage.Reality
                        == DamageRealityType.Ghost)
                    {
                        continue;
                    }

                    PhysicalDamage physicalDamage =
                        damage as PhysicalDamage;

                    if (physicalDamage != null)
                    {
                        bool countsAsEnchanted =
                            physicalDamage.EnchantmentTotal > 0
                            || weaponAttack;

                        DamageDeclineType declineType =
                            countsAsEnchanted
                                ? DamageDeclineType.ByHalf
                                : DamageDeclineType.Total;

                        damage.AddDecline(
                            new DamageDecline(
                                declineType,
                                __instance.Fact)
                            {
                                IncorporealDamageDivisor = true
                            });

                        continue;
                    }

                    EnergyDamage energyDamage =
                        damage as EnergyDamage;

                    if (energyDamage != null
                        && energyDamage.EnergyType
                            != DamageEnergyType.Holy
                        && energyDamage.EnergyType
                            != DamageEnergyType.Unholy
                        && energyDamage.EnergyType
                            != DamageEnergyType.PositiveEnergy
                        && energyDamage.EnergyType
                            != DamageEnergyType.NegativeEnergy
                        && energyDamage.EnergyType
                            != DamageEnergyType.Divine)
                    {
                        damage.AddDecline(
                            new DamageDecline(
                                DamageDeclineType.ByHalf,
                                __instance.Fact)
                            {
                                IncorporealDamageDivisor = true
                            });
                    }
                }

                return false;
            }
        }
    }
}