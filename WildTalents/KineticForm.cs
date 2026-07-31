using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.NewComponents;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.UnitLogic.ActivatableAbilities;
using static EbonsContentMod.Utilities.ActivatableAbilityGroupUtilities;

namespace EbonsContentMod.WildTalents
{
    internal class KineticForm
    {
        internal const string KineticFormDisplayName = "KineticForm.Name";
        private static readonly string KineticFormDescription = "KineticForm.Description";

        internal const string KineticFormAcceptBurnDisplayName = "KineticFormAcceptBurn.Name";
        internal const string KineticFormLargeDisplayName = "KineticFormLarge.Name";
        internal const string KineticFormHugeDisplayName = "KineticFormHuge.Name";

        internal static void Configure()
        {
            var KineticFormBuff1 = BuffConfigurator.New("KineticFormBuff1", "{5F8D0121-D978-4C7C-8A1A-2873352119DB}")
                .SetDisplayName(KineticFormLargeDisplayName)
                .SetDescription(KineticFormDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.EnlargePerson.ToString()).Icon)
                .AddChangeUnitSize(type: Kingmaker.Designers.Mechanics.Buffs.ChangeUnitSize.ChangeType.Value, size: Kingmaker.Enums.Size.Large)
                .SetFxOnStart("352469f228a3b1f4cb269c7ab0409b8e")
                .SetFxOnRemove("352469f228a3b1f4cb269c7ab0409b8e")
                .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.RemoveOnRest)
                .AddRestTrigger(action: ActionsBuilder.New().RemoveSelf())
                .Configure();

            var KineticFormBuff2 = BuffConfigurator.New("KineticFormBuff2", "{6F09BAD9-ABD4-480E-8930-900BD1F6C145}")
                .SetDisplayName(KineticFormHugeDisplayName)
                .SetDescription(KineticFormDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.FocusedRageFeature.ToString()).Icon)
                .AddChangeUnitSize(type: Kingmaker.Designers.Mechanics.Buffs.ChangeUnitSize.ChangeType.Value, size: Kingmaker.Enums.Size.Huge)
                .SetFxOnStart("352469f228a3b1f4cb269c7ab0409b8e")
                .SetFxOnRemove("352469f228a3b1f4cb269c7ab0409b8e")
                .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.RemoveOnRest)
                .AddRestTrigger(action: ActionsBuilder.New().RemoveSelf())
                .Configure();

            // Make the abilities only work when burn is accepted
            var KineticFormActivatableAbility1 = ActivatableAbilityConfigurator.New("KineticFormActivatableAbility1", "{4F8E4CEF-C0B9-49D4-AF86-1B0C8FF35CD1}")
                .SetDisplayName(KineticFormLargeDisplayName)
                .SetDescription(KineticFormDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.EnlargePerson.ToString()).Icon)
                .SetBuff(KineticFormBuff1)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfOwnerDisabled(true)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Standard)
                .SetGroup((ActivatableAbilityGroup)ECActivatableAbilityGroup.KineticFormAbilities)
                .Configure();

            var KineticFormActivatableAbility2 = ActivatableAbilityConfigurator.New("KineticFormActivatableAbility2", "{DA0988D5-4B00-4180-AF46-C232566C38B2}")
                .SetDisplayName(KineticFormHugeDisplayName)
                .SetDescription(KineticFormDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.FocusedRageFeature.ToString()).Icon)
                .SetBuff(KineticFormBuff2)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfOwnerDisabled(true)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Standard)
                .SetGroup((ActivatableAbilityGroup)ECActivatableAbilityGroup.KineticFormAbilities)
                .Configure();

            // Progression adds abilty to accept burn with 2 resource points (second point added at 16th level), first time gives a buff that adds a fact (ability 1, WT burn = 1), however, conditional checks for that ability and if it exists, it adds a buff that adds the second fact (ability 2, accepts 1 burn on use), ability 1&2 are mutually exclusive and both fact adding buffs are cleared on rest

            var KineticFormBaseBuff1 = BuffConfigurator.New("KineticFormBaseBuff1", "{FB7E67C8-8A57-4F7A-B286-F50D34316D2A}")
                .SetDisplayName(KineticFormLargeDisplayName)
                .SetDescription(KineticFormDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.EnlargePerson.ToString()).Icon)
                .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.RemoveOnRest, Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi, Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
                .SetIsClassFeature()
                .AddFacts([KineticFormActivatableAbility1])
                .SetStacking(Kingmaker.UnitLogic.Buffs.Blueprints.StackingType.Replace)
                .AddRestTrigger(action: ActionsBuilder.New().RemoveSelf())
                .Configure();

            var KineticFormBaseBuff2 = BuffConfigurator.New("KineticFormBaseBuff2", "{002C4758-78DC-4A0F-BBB4-A4A4AC9D7CDD}")
                .SetDisplayName(KineticFormHugeDisplayName)
                .SetDescription(KineticFormDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.FocusedRageFeature.ToString()).Icon)
                .SetFlags(Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.RemoveOnRest, Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.HiddenInUi, Kingmaker.UnitLogic.Buffs.Blueprints.BlueprintBuff.Flags.StayOnDeath)
                .SetIsClassFeature()
                .AddFacts([KineticFormActivatableAbility2])
                .SetStacking(Kingmaker.UnitLogic.Buffs.Blueprints.StackingType.Replace)
                .AddRestTrigger(action: ActionsBuilder.New().RemoveSelf())
                .Configure();

            var KineticFormResource = AbilityResourceConfigurator.New("KineticFormResource", "{BC9ADB72-89DA-42A8-85C1-F89D8CCEB389}")
                .SetMaxAmount(ResourceAmountBuilder.New(1).IncreaseByLevelStartPlusDivStep([CharacterClassRefs.KineticistClass.ToString()], levelsPerStep: 16, bonusPerStep: 1))
                .SetUseMax()
                .SetMax(2)
                .Configure();

            var AbilityAction = ActionsBuilder.New().Conditional(conditions: ConditionsBuilder.New().HasBuff(KineticFormBaseBuff1), ifTrue: ActionsBuilder.New().ApplyBuffPermanent(KineticFormBaseBuff2, isNotDispelable: true, toCaster: true), ifFalse: ActionsBuilder.New().ApplyBuffPermanent(KineticFormBaseBuff1, isNotDispelable: true, toCaster: true));

            var KineticFormAcceptBurnAbility = AbilityConfigurator.New("KineticFormAcceptBurnAbility", "{8DE03342-B73F-46F6-9991-CB960F2C7B04}")
                .SetDisplayName(KineticFormAcceptBurnDisplayName)
                .SetDescription(KineticFormDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.ElementalBodyIVBase.ToString()).Icon)
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: KineticFormResource)
                .AddAbilityEffectRunAction(AbilityAction)
                .AddAbilityAcceptBurnOnCast(burnValue: 1)
                .Configure();

            var KineticFormFeature = FeatureConfigurator.New("KineticFormFeature", "{C747663E-07E8-4D6A-829E-F21EC54292BC}", FeatureGroup.KineticWildTalent)
                .SetDisplayName(KineticFormDisplayName)
                .SetDescription(KineticFormDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.ElementalBodyIVBase.ToString()).Icon)
                .AddFacts([KineticFormAcceptBurnAbility])
                .AddAbilityResources(resource: KineticFormResource, restoreAmount: true)
                .SetIsClassFeature()
                .AddPrerequisiteClassLevel(CharacterClassRefs.KineticistClass.ToString(), level: 10)
                .Configure();
        }
    }
}
