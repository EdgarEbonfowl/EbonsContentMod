using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.ActivatableAbilities;
using BlueprintCore.Actions.Builder.ContextEx;
using EbonsContentMod.Components;

namespace EbonsContentMod.Abilities
{
    internal class FlamboyantArcana
    {
        private const string FlamboyantArcanaName = "FlamboyantArcana";
        internal const string FlamboyantArcanaDisplayName = "FlamboyantArcana.Name";
        private const string FlamboyantArcanaDescription = "FlamboyantArcana.Description";
        public static readonly string FlamboyantArcanaGuid = "{4FEF6080-4942-44EC-91B3-DA915E324675}";

        private const string FlamboyantParryAndRiposteName = "FlamboyantParryAndRiposte";
        private const string FlamboyantParryAndRiposteBuffName = "FlamboyantParryAndRiposteBuff";
        internal const string FlamboyantParryAndRiposteDisplayName = "FlamboyantParryAndRiposte.Name";
        public static readonly string FlamboyantParryAndRiposteAbilityGuid = "{9CC1F3D2-7870-47FD-AFBA-CDB54C9F5474}";
        public static readonly string FlamboyantParryAndRiposteBuffGuid = "{8814EF99-1EEA-4101-B7AA-E70AD8F0C612}";

        private const string FlamboyantDerringDoName = "FlamboyantDerringDo";
        private const string FlamboyantDerringDoBuffName = "FlamboyantDerringDoBuff";
        internal const string FlamboyantDerringDoDisplayName = "FlamboyantDerringDo.Name";
        public static readonly string FlamboyantDerringDoAbilityGuid = "{183B03F8-7F3B-496A-B8D3-6BAAFC3C3381}";
        public static readonly string FlamboyantDerringDoBuffGuid = "{22F89ED0-DBE1-4835-A453-6B18561B7675}";

        internal static void Configure()
        {
            var icon = ActivatableAbilityRefs.DuelistParrySelfAbility.Reference.Get().Icon;

            var parry_buff = BuffConfigurator.New(FlamboyantParryAndRiposteBuffName, FlamboyantParryAndRiposteBuffGuid)
                .SetDisplayName(FlamboyantParryAndRiposteDisplayName)
                .SetDescription(FlamboyantArcanaDescription)
                .SetIcon(icon)
                .AddNotDispelable()
                .AddComponent<FlamboyantArcanaParryAndRiposte>()
                .AddComponent<AddAbilityResourceDepletedTrigger>(c => { c.m_Resource = BlueprintTool.GetRef<BlueprintAbilityResourceReference>(AbilityResourceRefs.ArcanePoolResourse.ToString()); c.Action = ActionsBuilder.New().RemoveSelf().Build(); c.Cost = 1; })
                .Configure();

            var parry_ability = ActivatableAbilityConfigurator.New(FlamboyantParryAndRiposteName, FlamboyantParryAndRiposteAbilityGuid)
                .SetDisplayName(FlamboyantParryAndRiposteDisplayName)
                .SetDescription(FlamboyantArcanaDescription)
                .SetIcon(icon)
                .AddActivatableAbilityResourceLogic(requiredResource: AbilityResourceRefs.ArcanePoolResourse.ToString(), spendType: ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                .SetBuff(parry_buff)
                .SetDeactivateImmediately()
                .Configure();

            var derring_buff = BuffConfigurator.New(FlamboyantDerringDoBuffName, FlamboyantDerringDoBuffGuid)
                .SetDisplayName(FlamboyantDerringDoDisplayName)
                .SetDescription(FlamboyantArcanaDescription)
                .SetIcon(FeatureRefs.BloodlineFeyWoodlandStride.Reference.Get().Icon)
                .AddNotDispelable()
                .AddComponent<FlamboyantArcanaDerringDo>()
                .AddComponent<AddAbilityResourceDepletedTrigger>(c => { c.m_Resource = BlueprintTool.GetRef<BlueprintAbilityResourceReference>(AbilityResourceRefs.ArcanePoolResourse.ToString()); c.Action = ActionsBuilder.New().RemoveSelf().Build(); c.Cost = 1; })
                .Configure();

            var derring_ability = ActivatableAbilityConfigurator.New(FlamboyantDerringDoName, FlamboyantDerringDoAbilityGuid)
                .SetDisplayName(FlamboyantDerringDoDisplayName)
                .SetDescription(FlamboyantArcanaDescription)
                .SetIcon(FeatureRefs.BloodlineFeyWoodlandStride.Reference.Get().Icon)
                .AddActivatableAbilityResourceLogic(requiredResource: AbilityResourceRefs.ArcanePoolResourse.ToString(), spendType: ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                .SetBuff(derring_buff)
                .SetDeactivateImmediately()
                .Configure();

            FeatureConfigurator.New(FlamboyantArcanaName, FlamboyantArcanaGuid, FeatureGroup.MagusArcana, FeatureGroup.EldritchScionArcana)
              .SetDisplayName(FlamboyantArcanaDisplayName)
              .SetDescription(FlamboyantArcanaDescription)
              .SetIcon(icon)
              .AddFacts(new() { parry_ability })
              .AddFacts(new() { derring_ability })
              .SetIsClassFeature()
              .Configure();
        }
    }
}
