using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using EbonsContentMod.Components;
using Kingmaker.Blueprints.Classes.Spells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonsContentMod.Spells
{
    internal class AngelicAspect
    {
        private static readonly string AADescription = "Ebon.AngelicAspect.Description";
        private static readonly string GAADescription = "Ebon.GreaterAngelicAspect.Description";

        internal static void Configure()
        {
            BuffConfigurator.For(BuffRefs.AngelicAspectGreaterBuff)
                .SetDescription(GAADescription)
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

            AbilityConfigurator.For(AbilityRefs.AngelicAspectGreater)
                .SetDescription(GAADescription)
                .Configure();

            BuffConfigurator.For(BuffRefs.AngelicAspectBuff)
                .SetDescription(AADescription)
                .AddACBonusAgainstAttacks(true, descriptor: Kingmaker.Enums.ModifierDescriptor.Dodge, value: 3)
                .AddConditionImmunity(Kingmaker.UnitLogic.UnitCondition.DifficultTerrain)
                .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Ground)
                .Configure();

            AbilityConfigurator.For(AbilityRefs.AngelicAspect)
                .SetDescription(AADescription)
                .Configure();
        }
    }
}
