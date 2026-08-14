using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Enums;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using EbonsContentMod.Components;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using TabletopTweaks.Core.Utilities;
using Kingmaker.Blueprints.Classes;

namespace EbonsContentMod.WildTalents
{
    internal class EarthWalk
    {
        private const string EarthWalkNameKey =
            "EbonsContentMod.EarthWalk.Name";

        private const string EarthWalkDescriptionKey =
            "EbonsContentMod.EarthWalk.Description";

        internal static void Configure()
        {
            FeatureConfigurator.New("EarthWalkFeature", "{09CD3921-05D6-43A9-89B4-6CCB359C79F6}", FeatureGroup.KineticWildTalent)
                .SetDisplayName(EarthWalkNameKey)
                .SetDescription(EarthWalkDescriptionKey)
                .SetIcon(FeatureRefs.BloodlineElementalEarthElementalMovementFeature.Reference.Get().Icon)
                .AddComponent<CMDBonusFromKineticOverflow>(c =>
                {
                    c.Descriptor = ModifierDescriptor.UntypedStackable;
                    c.Maneuvers =
                    [
                        CombatManeuver.BullRush,
                        CombatManeuver.Trip,
                        CombatManeuver.Pull
                    ];
                })
                .AddConditionImmunity(UnitCondition.DifficultTerrain)
                .SetIsClassFeature()
                .AddPrerequisiteFeaturesFromList(
                [
                    ProgressionRefs.ElementalFocusEarth.ToString(),
                    ProgressionRefs.SecondaryElementEarth.ToString(),
                    ProgressionRefs.ThirdElementEarth.ToString(),
                    ProgressionRefs.KineticKnightElementalFocusEarth.ToString(),
                    FeatureRefs.EarthBlastFeature.ToString()
                ])
                .AddPrerequisiteClassLevel(CharacterClassRefs.KineticistClass.ToString(), 1)
                .Configure();
        }
    }
}
