using BlueprintCore.Blueprints.Configurators.Items.Equipment;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Items.Components;
using Kingmaker.UnitLogic.Commands.Base;
using System.Data;

namespace EbonsContentMod.Equipment
{
    internal class BaneOfSpirit
    {
        private const string BaneOfSpiritDescription = "BaneOfSpirit.Description";

        internal static void Configure()
        {
            ItemEquipmentRingConfigurator.For(ItemEquipmentRingRefs.BrokenPhylacterySoulRingItem)
                .SetDescriptionText(BaneOfSpiritDescription)
                .RemoveComponents(c => c is CustomItemAbilityActionType)
                .Configure();

            AbilityConfigurator.For(AbilityRefs.BrokenPhylacterySoulRingAbility)
                .SetDescription(BaneOfSpiritDescription)
                .Configure();

            ItemEquipmentRingConfigurator.For(ItemEquipmentRingRefs.BrokenPhylacterySoulRingItem)
                .AddCustomItemAbilityActionType(UnitCommand.CommandType.Free)
                .Configure(delayed: true);
        }
    }
}
