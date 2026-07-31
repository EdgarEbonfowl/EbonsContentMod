using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Blueprints.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace EbonsContentMod.WildTalents
{
    internal class ClockworkHeart
    {
        internal const string ClockworkHeartDisplayName = "ClockworkHeart.Name";
        private static readonly string ClockworkHeartDescription = "ClockworkHeart.Description";

        internal static void Configure()
        {
            var ClockworkHeartFeature = FeatureConfigurator.New("ClockworkHeartFeature", "{06508D7D-04E1-43DD-A034-FDDC1B37468F}", FeatureGroup.KineticWildTalent)
                .SetDisplayName(ClockworkHeartDisplayName)
                .SetDescription(ClockworkHeartDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.IronBody.ToString()).Icon)
                .AddFacts([FeatureRefs.Improved_Initiative.ToString(), FeatureRefs.LightningReflexes.ToString()])
                .SetIsClassFeature()
                .AddPrerequisiteClassLevel(CharacterClassRefs.KineticistClass.ToString(), level: 6)
                .AddPrerequisiteFeature(FeatureRefs.MetalBlastFeature.ToString())
                .Configure();
        }
    }
}
