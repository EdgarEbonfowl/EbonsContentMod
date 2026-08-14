using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EbonsContentMod.WildTalents
{
    internal class EarthGlide
    {
        private const string EarthGlideNameKey =
            "EbonsContentMod.EarthGlide.Name";

        private const string EarthGlideDescriptionKey =
            "EbonsContentMod.EarthGlide.Description";

        private static Sprite EarthGlideIcon = FeatureRefs.BloodlineElementalEarthElementalMovementFeature.Reference.Get().Icon;

        internal static void Configure()
        {
            var buff = BuffConfigurator.New("EarthGlideBuff", "{339D52FD-AD59-48A3-93BD-A4DB79B96B8A}")
                .SetDisplayName(EarthGlideNameKey)
                .SetDescription(EarthGlideDescriptionKey)
                .SetIcon(EarthGlideIcon)
                .AddCondition(UnitCondition.CanNotAttack)
                .AddStatBonus(ModifierDescriptor.UntypedStackable, stat: Kingmaker.EntitySystem.Stats.StatType.AC, value: 8)
                .AddConcealment(false, false, Concealment.Total, ConcealmentDescriptor.WindsOfVengenance, onlyForAttacks: false)
                .SetIsClassFeature()
                .SetFlags(BlueprintBuff.Flags.StayOnDeath)
                .Configure();
            
            var ability = ActivatableAbilityConfigurator.New("EarthGlideActivatableAbility", "{EBB6E591-C451-40DE-AA27-C41F7E311B6D}")
                //.CopyFrom(ActivatableAbilityRefs.BloodlineElementalEarthElementalMovementBurrowAbility)
                .SetDisplayName(EarthGlideNameKey)
                .SetDescription(EarthGlideDescriptionKey)
                .SetIcon(EarthGlideIcon)
                .SetBuff(buff)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfCombatEnded(false)
                .SetDeactivateIfOwnerDisabled(true)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Standard)
                .Configure();
            
            FeatureConfigurator.New("EarthGlideFeature", "{3BCC97D9-2580-4215-92AA-478B594353A3}", FeatureGroup.KineticWildTalent)
                .SetDisplayName(EarthGlideNameKey)
                .SetDescription(EarthGlideDescriptionKey)
                .SetIcon(EarthGlideIcon)
                .AddFacts([ability])
                .SetIsClassFeature()
                .AddPrerequisiteFeaturesFromList(
                [
                    ProgressionRefs.ElementalFocusEarth.ToString(),
                    ProgressionRefs.SecondaryElementEarth.ToString(),
                    ProgressionRefs.ThirdElementEarth.ToString(),
                    ProgressionRefs.KineticKnightElementalFocusEarth.ToString(),
                    FeatureRefs.EarthBlastFeature.ToString()
                ])
                .AddPrerequisiteClassLevel(CharacterClassRefs.KineticistClass.ToString(), 10)
                .Configure();
        }
    }
}
