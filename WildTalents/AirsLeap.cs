using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace EbonsContentMod.WildTalents
{
    internal class AirsLeap
    {
        internal const string AirsLeapDisplayName = "AirsLeap.Name";
        private static readonly string AirsLeapDescription = "AirsLeap.Description";

        internal static void Configure()
        {
            var AirsLeapFeature = FeatureConfigurator.New("AirsLeapFeature", "{A0415C92-593D-42DD-B63F-A84D19837A3D}", FeatureGroup.KineticWildTalent)
                .SetDisplayName(AirsLeapDisplayName)
                .SetDescription(AirsLeapDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.FeatherStep.ToString()).Icon)
                .SetIsClassFeature()
                .AddPrerequisiteClassLevel(CharacterClassRefs.KineticistClass.ToString(), level: 1)
                .AddPrerequisiteFeaturesFromList(new List<BlueprintCore.Utils.Blueprint<BlueprintFeatureReference>> { 
                    ProgressionRefs.ElementalFocusAir.ToString(),
                    ProgressionRefs.SecondaryElementAir.ToString(),
                    ProgressionRefs.ThirdElementAir.ToString(),
                    ProgressionRefs.KineticKnightElementalFocusAir.ToString(),
                    FeatureRefs.AirBlastFeature.ToString(),
                    FeatureRefs.ElectricBlastFeature.ToString()
                })
                .AddContextStatBonus(Kingmaker.EntitySystem.Stats.StatType.SkillMobility, ContextValues.Rank(), Kingmaker.Enums.ModifierDescriptor.UntypedStackable)
                .AddContextRankConfig(ContextRankConfigs.ClassLevel([CharacterClassRefs.KineticistClass.ToString()]))
                .Configure();
        }
    }
}
