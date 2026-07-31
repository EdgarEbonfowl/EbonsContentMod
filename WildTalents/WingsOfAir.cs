using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using EbonsContentMod.Components;
using EbonsContentMod.UnitParts;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace EbonsContentMod.WildTalents
{
    internal class WingsOfAir
    {
        internal const string WingsOfAirDisplayName = "WingsOfAir.Name";
        private static readonly string WingsOfAirDescription = "WingsOfAir.Description";

        internal static void Configure()
        {
            var WingsOfAirBuff = BuffConfigurator.New("WingsOfAirBuff", "{33A46AE6-4729-41EE-889C-2A8A3C7E86DF}")
                .SetDisplayName(WingsOfAirDisplayName)
                .SetDescription(WingsOfAirDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.FeatureWingsAngel.ToString()).Icon)
                .AddComponent<SetBaseSpeedOverride>(component =>
                {
                    component.SpeedFeet = 60;
                    component.Priority = 10;
                })
                .AddACBonusAgainstAttacks(true, descriptor: Kingmaker.Enums.ModifierDescriptor.Dodge, value: 3)
                .AddConditionImmunity(Kingmaker.UnitLogic.UnitCondition.DifficultTerrain)
                .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Ground)
                .AddFormationACBonus(3)
                .Configure();
            
            var WingsOfAirActivatableAbility = ActivatableAbilityConfigurator.New("WingsOfAirActivatableAbility", "{342FF12D-AF1F-497C-BDC4-BA1CCC646092}")
                .SetDisplayName(WingsOfAirDisplayName)
                .SetDescription(WingsOfAirDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.FeatureWingsAngel.ToString()).Icon)
                .SetBuff(WingsOfAirBuff)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfOwnerDisabled(true)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Standard)
                .Configure();
            
            var WingsOfAirFeature = FeatureConfigurator.New("WingsOfAirFeature", "{86DFA097-F847-4424-86FC-D1D77D6CB1D7}", FeatureGroup.KineticWildTalent)
                .SetDisplayName(WingsOfAirDisplayName)
                .SetDescription(WingsOfAirDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.FeatureWingsAngel.ToString()).Icon)
                .SetIsClassFeature()
                .AddPrerequisiteClassLevel(CharacterClassRefs.KineticistClass.ToString(), level: 6)
                .AddPrerequisiteFeature("A0415C92-593D-42DD-B63F-A84D19837A3D")
                .AddPrerequisiteFeaturesFromList(new List<BlueprintCore.Utils.Blueprint<BlueprintFeatureReference>> {
                    ProgressionRefs.ElementalFocusAir.ToString(),
                    ProgressionRefs.SecondaryElementAir.ToString(),
                    ProgressionRefs.ThirdElementAir.ToString(),
                    ProgressionRefs.KineticKnightElementalFocusAir.ToString(),
                    FeatureRefs.AirBlastFeature.ToString(),
                    FeatureRefs.ElectricBlastFeature.ToString()
                })
                .AddFacts([WingsOfAirActivatableAbility])
                .Configure();
        }
    }

    [HarmonyPatch(typeof(ModifiableValue), "CalculateBaseValue", typeof(int))]
    internal static class ModifiableValue_CalculateBaseValue_Patch
    {
        private static void Postfix(
            ModifiableValue __instance,
            ref int __result)
        {
            if (__instance is not ModifiableValueSpeed speedStat)
                return;

            var unit = speedStat.Owner?.Unit;
            if (unit == null)
                return;

            var part = unit.Get<UnitPartBaseSpeedOverride>();

            if (part?.TryGetSpeed(out int speedFeet) == true)
            {
                __result = speedFeet;
            }
        }
    }
}
