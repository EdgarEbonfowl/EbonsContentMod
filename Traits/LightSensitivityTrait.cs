using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using EbonsContentMod.Components;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace EbonsContentMod.Traits
{
    internal class LightSensitivityTrait
    {
        internal const string LightSensitivityDisplayName = "LightSensitivity.Name";
        private static readonly string LightSensitivityDescription = "LightSensitivity.Description";

        internal const string LightSensitivityImmunityDisplayName = "LightSensitivityImmunity.Name";
        private static readonly string LightSensitivityImmunityDescription = "LightSensitivityImmunity.Description";

        internal static void Configure()
        {
            var icon = BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.Blindness.ToString()).Icon;

            FeatureConfigurator.New("LightSensitivityImmunityFeature", "{4EA922BA-F88E-4B8E-A378-50C3D0E2DA2E}")
                .SetDisplayName(LightSensitivityImmunityDisplayName)
                .SetDescription(LightSensitivityImmunityDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.RemoveBlindness.ToString()).Icon)
                .Configure();

            BuffConfigurator.New("LightSensitivityBuff", "{FCA0ADBC-EB20-40D9-A854-0F7AAC45B659}")
                .SetDisplayName(LightSensitivityDisplayName)
                .SetDescription(LightSensitivityDescription)
                .SetIcon(icon)
                .AddBuffStatusCondition(Kingmaker.UnitLogic.UnitCondition.Dazzled)
                .Configure();

            FeatureConfigurator.New("LightSensitivityFeature", "{BBA86DB1-B3EF-4AB5-A412-A5910F2B8947}")
                .SetDisplayName(LightSensitivityDisplayName)
                .SetDescription(LightSensitivityDescription)
                .SetIcon(icon)
                .AddComponent<LightSensitivity>()
                .SetGroups(Kingmaker.Blueprints.Classes.FeatureGroup.Trait)
                .Configure();
        }
    }
}
