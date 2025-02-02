using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using EbonsContentMod.Components;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using EbonsContentMod.Utilities;

namespace EbonsContentMod.Bloodlines
{
    internal class BloodlineMutations
    {
        // Bloodline powers at 1st, 3rd, 9th, 15th, 20th
        // Bloodline feats at 7th, 13th, 19th

        private const string BloodHavoc = "BloodHavocBloodlineMutation";
        internal const string BloodHavocName = "BloodlineMutation.BloodHavoc.Name";
        private const string BloodHavocDescription = "BloodlineMutation.BloodHavoc.Description";
        public static readonly string BloodHavocGuid = "{5D61FCD8-C3D5-45DA-9F97-E1B2FDA8218B}";

        private const string BloodragerBloodHavoc = "BloodragerBloodHavocBloodlineMutation";
        public static readonly string BloodragerBloodHavocGuid = "{C33EDDCE-EB3D-4600-BE91-39037E6015B6}";

        private const string BloodIntensity = "BloodIntensityBloodlineMutation";
        internal const string BloodIntensityName = "BloodlineMutation.BloodIntensity.Name";
        private const string BloodIntensityDescription = "BloodlineMutation.BloodIntensity.Description";
        public static readonly string BloodIntensityGuid = "{AD27605E-3111-41F0-A925-5C2B27EEE7B6}";

        private const string BloodragerBloodIntensity = "BloodragerBloodIntensityBloodlineMutation";
        public static readonly string BloodragerBloodIntensityGuid = "{C0076BA5-A8CC-4A6A-8271-9B8032314E34}";

        private const string BloodPiercing = "BloodPiercingBloodlineMutation";
        internal const string BloodPiercingName = "BloodlineMutation.BloodPiercing.Name";
        private const string BloodPiercingDescription = "BloodlineMutation.BloodPiercing.Description";
        public static readonly string BloodPiercingGuid = "{6E9996C9-1430-4316-9574-573584F6010D}";

        private const string BloodragerBloodPiercing = "BloodragerBloodPiercingBloodlineMutation";
        public static readonly string BloodragerBloodPiercingGuid = "{D2ADC41F-4681-4714-B78F-4DA07F51DA38}";

        internal static void Configure()
        {
            var SorcererFeatSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>("3a60f0c0442acfb419b0c03b584e1394");
            var SorcererBloodlineSelection = FeatureSelectionRefs.SorcererBloodlineSelection.Reference.Get();
            var SorcerBloodlines = SorcererBloodlineSelection.Features;

            // Get all Bloodrager Feat selections
            var BloodragerBloodlines = FeatureSelectionRefs.BloodragerBloodlineSelection.Reference.Get().Features;

            List<BlueprintFeatureSelection> AllBloodragerFeatSelections = [];

            foreach (BlueprintFeature bl in BloodragerBloodlines)
            {
                if (bl is not BlueprintProgression bloodline)
                    continue;

                foreach (LevelEntry le in bloodline.LevelEntries)
                {
                    if (le.Features == null) continue;

                    foreach (BlueprintFeatureBase bfb in le.Features)
                    {
                        if (bfb is not BlueprintFeatureSelection selection)
                            continue;

                        AllBloodragerFeatSelections.AddUnique(selection);
                    }
                    
                    //var FeatureSelections = le.m_FeaturesList.Where(f => f is BlueprintFeatureSelection);
                    //foreach (BlueprintFeatureSelection bfs in FeatureSelections) AllBloodragerFeatSelections.AddUnique(bfs);
                }
            }

            // Make the feats
            var bloodHavoc = FeatureConfigurator.New(BloodHavoc, BloodHavocGuid)
                .SetDisplayName(BloodHavocName)
                .SetDescription(BloodHavocDescription)
                .SetIcon(AbilityRefs.TricksterSummonBeerElementalLarge.Reference.Get().Icon)
                .AddComponent<BloodHavoc>()
                .AddPrerequisiteClassLevel(CharacterClassRefs.SorcererClass.Reference.Get(), 1, hideInUI: true)
                .AddPrerequisiteNoFeature(BloodragerBloodHavocGuid, hideInUI: true)
                .Configure();

            var bloodHavocBloodrager = FeatureConfigurator.New(BloodragerBloodHavoc, BloodragerBloodHavocGuid)
                .SetDisplayName(BloodHavocName)
                .SetDescription(BloodHavocDescription)
                .SetIcon(AbilityRefs.TricksterSummonBeerElementalLarge.Reference.Get().Icon)
                .AddComponent<BloodHavoc>()
                .AddPrerequisiteClassLevel(CharacterClassRefs.BloodragerClass.Reference.Get(), 4, hideInUI: true)
                .AddPrerequisiteNoFeature(bloodHavoc, hideInUI: true)
                .Configure();

            var bloodIntensity = FeatureConfigurator.New(BloodIntensity, BloodIntensityGuid)
                .SetDisplayName(BloodIntensityName)
                .SetDescription(BloodIntensityDescription)
                .Configure();

            var bloodPiercing = FeatureConfigurator.New(BloodPiercing, BloodPiercingGuid)
                .SetDisplayName(BloodPiercingName)
                .SetDescription(BloodPiercingDescription)
                .Configure();

            // Add the feats to the bloodline feat selections
            FeatureSelectionConfigurator.For(SorcererFeatSelection)
                .AddToAllFeatures([bloodHavoc])
                .Configure();

            foreach (BlueprintFeatureSelection bfs in AllBloodragerFeatSelections)
            {
                FeatureSelectionConfigurator.For(bfs)
                .AddToAllFeatures([bloodHavocBloodrager])
                .Configure();
            }

            // Replacing Bloodline Powers (may not be possible because of Second Bloodline, at least no this way) - Commenting out for now
            // Could try making a new component that only removes the replaced power from the first bloodline, somehow...
            /*BlueprintFeatureBase[] LevelOnePowers = [];
            BlueprintFeatureBase[] LevelThreePowers = [];
            BlueprintFeatureBase[] LevelNinePowers = [];
            BlueprintFeatureBase[] LevelThirteenPowers = [];
            BlueprintFeatureBase[] LevelTwentyPowers = [];

            foreach (BlueprintFeature bl in SorcerBloodlines)
            {
                if (bl is not BlueprintProgression bloodline)
                    continue;

                var LevelOnePower = bloodline.GetLevelEntry(1).m_FeaturesList.Where(f => f.GetComponent<AddAbilityResources>() != null);

                foreach (BlueprintFeatureBase bfb in LevelOnePower) LevelOnePowers = LevelOnePowers.AppendToArray(bfb);

                var LevelThreePower = bloodline.GetLevelEntry(3).m_FeaturesList.Where(f => f.GetComponent<AddKnownSpell>() == null);

                foreach (BlueprintFeatureBase bfb in LevelThreePower) LevelThreePowers = LevelThreePowers.AppendToArray(bfb);

                var LevelNinePower = bloodline.GetLevelEntry(9).m_FeaturesList.Where(f => f.GetComponent<AddKnownSpell>() == null);

                foreach (BlueprintFeatureBase bfb in LevelNinePower) LevelNinePowers = LevelNinePowers.AppendToArray(bfb);

                var LevelThirteenPower = bloodline.GetLevelEntry(13).m_FeaturesList.Where(f => f.GetComponent<AddKnownSpell>() == null);

                foreach (BlueprintFeatureBase bfb in LevelThirteenPower) LevelThirteenPowers = LevelThirteenPowers.AppendToArray(bfb);

                var LevelTwentyPower = bloodline.GetLevelEntry(20).m_FeaturesList.Where(f => f.GetComponent<AddKnownSpell>() == null);

                foreach (BlueprintFeatureBase bfb in LevelTwentyPower) LevelTwentyPowers = LevelTwentyPowers.AppendToArray(bfb);
            }*/

            // Replacing Bloodline Powers
        }
    }
}
