using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonsContentMod.Abilities
{
    internal class ShamanFriendToAnimalsHex
    {
        internal static void Configure()
        {
            BuffConfigurator.For(BuffRefs.ShamanHexFriendToAnimalsEffectBuff)
                .RemoveComponents(c => c is ContextRankConfig or AddContextStatBonus)
                .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.Charisma))
                .AddContextStatBonus(StatType.SaveFortitude, ContextValues.Rank(), ModifierDescriptor.Sacred)
                .AddContextStatBonus(StatType.SaveReflex, ContextValues.Rank(), ModifierDescriptor.Sacred)
                .AddContextStatBonus(StatType.SaveWill, ContextValues.Rank(), ModifierDescriptor.Sacred)
                .AddRemoveBuffIfCasterIsMissing(removeOnCasterDeath: true)
                .AddRecalculateOnStatChange(stat: StatType.Charisma, useKineticistMainStat: false)
                .Configure();
        }
    }
}
