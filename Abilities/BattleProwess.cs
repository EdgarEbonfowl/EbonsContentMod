using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Area;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace EbonsContentMod.Abilities
{
    internal class BattleProwess
    {
        private static readonly string BattleProwessDescription = "Ebon.BattleProwess.Description";

        internal static void Configure()
        {
            AbilityAreaEffectConfigurator.For(AbilityAreaEffectRefs.BattleProwessArea.ToString())
                .SetSize(50.0f.Feet())
                .SetFx("948e6476e9d49d0429452ce1db0c224d")
                .Configure();

            BuffConfigurator.For(BuffRefs.BattleProwessEffectBuff.ToString())
                .RemoveComponents(c => c is AddFactContextActions)
                .SetDescription(BattleProwessDescription)
                .AddFactContextActions(
                    activated:
                        ActionsBuilder.New()
                            .Conditional(
                                ConditionsBuilder.New()
                                    .CasterHasFact(
                                        FeatureRefs.MythicInspire.ToString()),
                                ifTrue:
                                    ActionsBuilder.New()
                                        .ApplyBuffPermanent(
                                            BuffRefs.InspiredRageEffectBuffMythic.ToString(),
                                            isNotDispelable: true),
                                ifFalse:
                                    ActionsBuilder.New()
                                        .ApplyBuffPermanent(
                                            BuffRefs.InspiredRageEffectBuff.ToString(),
                                            isNotDispelable: true)),
                    deactivated:
                        ActionsBuilder.New()
                            .RemoveBuff(
                                BuffRefs.InspiredRageEffectBuff.ToString())
                            .RemoveBuff(
                                BuffRefs.InspiredRageEffectBuffMythic.ToString()))
                .Configure();

            ActivatableAbilityConfigurator
                .For(ActivatableAbilityRefs.BattleProwessAbility.ToString())
                .SetDescription(BattleProwessDescription)
                .OnConfigure(bp =>
                {
                    var resourceLogic =
                        bp.GetComponent<ActivatableAbilityResourceLogic>();

                    var battleProwess =
                        FeatureRefs.BattleProwessFeature.Reference;

                    if (!resourceLogic.m_ResourceCostIncreasingFacts.Any(
                            fact => fact.Guid == battleProwess.Guid))
                    {
                        resourceLogic.m_ResourceCostIncreasingFacts.Add(BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(battleProwess.Guid));
                    }
                })
                .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.BattleProwessSelection.ToString())
                .SetDescription(BattleProwessDescription)
                .Configure();

            BuffConfigurator.For(BuffRefs.BattleProwessBuff.ToString())
                .SetDescription(BattleProwessDescription)
                .Configure();

            FeatureConfigurator.For(FeatureRefs.BattleProwessFeature.ToString())
                .SetDescription(BattleProwessDescription)
                .Configure();

            FeatureConfigurator.For(FeatureRefs.HatOfHearteningSongFeature)
                .AddBuffExtraEffects(BuffRefs.BattleProwessBuff.ToString(), extraEffectBuff: BuffRefs.HatOfHearteningSongBuff.ToString())
                .Configure();
        }
    }
}
