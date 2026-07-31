using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.References;
using EbonsContentMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints;
using TabletopTweaks.Core.Utilities;

namespace EbonsContentMod.Bloodlines
{
    internal class ConfigureBloodlines
    {
        internal static void Configure()
        {
            try
            {
                ProteanSorcererBloodline.Configure();
                OrcSorcererBloodline.Configure();
            }
            finally
            {
                try
                {
                    ComponentHelperators.CreateBloodlineSpellList();
                }
                finally
                {
                    try
                    {
                        LogOrcLevelOne("Before BloodlineMutations");

                        BloodlineMutations.Configure();

                        LogOrcLevelOne("Immediately after BloodlineMutations returned");
                    }
                    finally
                    {
                        VerifyBloodlineMutations();
                    }
                }
            }
            

            /*bool bloodlinesadded = false;

            // This is dangerous, but just to be sure...
            while (!bloodlinesadded)
            {
                if (FeatureSelectionRefs.SorcererBloodlineSelection.Reference.Get().Features.Contains(ProteanBloodline[0]) &&
                    FeatureSelectionRefs.SorcererBloodlineSelection.Reference.Get().Features.Contains(OrcBloodline[0]) &&
                    FeatureSelectionRefs.CrossbloodedSecondaryBloodlineSelection.Reference.Get().Features.Contains(ProteanBloodline[1]) &&
                    FeatureSelectionRefs.CrossbloodedSecondaryBloodlineSelection.Reference.Get().Features.Contains(OrcBloodline[1]) &&
                    FeatureSelectionRefs.SeekerBloodlineSelection.Reference.Get().Features.Contains(ProteanBloodline[2]) &&
                    FeatureSelectionRefs.SeekerBloodlineSelection.Reference.Get().Features.Contains(OrcBloodline[2])) 
                    bloodlinesadded = true;
            }*/

            
        }

        private static void VerifyBloodlineMutations()
        {
            var bloodline =
                BlueprintTools.GetBlueprint<BlueprintProgression>(
                    OrcSorcererBloodline.OrcSorcererBloodlineGuid);

            LevelEntry levelEntry = bloodline.LevelEntries
                .FirstOrDefault(entry => entry.Level == 1);

            Main.log.Log(
                $"Post-configuration verification for {bloodline.name}, level 1:");

            foreach (BlueprintFeatureBaseReference featureReference
                in levelEntry.m_Features)
            {
                BlueprintFeatureBase feature = featureReference?.Get();

                Main.log.Log(
                    $"    {feature?.name}, " +
                    $"GUID={feature?.AssetGuid}, " +
                    $"Type={feature?.GetType().Name}");
            }
        }

        private static void LogOrcLevelOne(string stage)
        {
            BlueprintProgression bloodline =
                BlueprintTools.GetBlueprint<BlueprintProgression>(
                    OrcSorcererBloodline.OrcSorcererBloodlineGuid);

            LevelEntry entry = bloodline.LevelEntries
                .FirstOrDefault(e => e.Level == 1);

            Main.log.Log($"{stage}:");

            foreach (BlueprintFeatureBaseReference reference in entry.m_Features)
            {
                BlueprintFeatureBase feature = reference?.Get();

                Main.log.Log(
                    $"    {feature?.name}, Type={feature?.GetType().Name}");
            }
        }
    }
}
