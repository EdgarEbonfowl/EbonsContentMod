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

        internal const string BloodlineMutationName = "BloodlineMutation.PowerSelection.Name";
        private const string BloodlineMutationDescription = "BloodlineMutation.PowerSelection.Description";

        // Helpers

        private static string CreateDeterministicGuid(string stableKey)
        {
            if (string.IsNullOrWhiteSpace(stableKey))
            {
                throw new ArgumentException(
                    "The deterministic GUID key cannot be null or empty.",
                    nameof(stableKey));
            }

            byte[] keyBytes = Encoding.UTF8.GetBytes(stableKey);

            using var sha256 =
                System.Security.Cryptography.SHA256.Create();

            byte[] hash = sha256.ComputeHash(keyBytes);

            byte[] guidBytes = new byte[16];
            Array.Copy(hash, guidBytes, guidBytes.Length);

            return new Guid(guidBytes).ToString();
        }



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

            // Replacing Bloodline Powers
            int[] BloodlinePowerLevels = [1, 3, 9, 15, 20];

            foreach (BlueprintFeature bl in SorcerBloodlines)
            {
                if (bl is not BlueprintProgression bloodlineFromSelection)
                    continue;

                BlueprintProgression bloodline =
                    BlueprintTools.GetBlueprint<BlueprintProgression>(
                        bloodlineFromSelection.AssetGuid);

                if (bloodline == null)
                {
                    Main.log.Log(
                        $"Bloodline Mutations: Could not retrieve canonical blueprint " +
                        $"for {bloodlineFromSelection.name} " +
                        $"({bloodlineFromSelection.AssetGuid}).");

                    continue;
                }

                Main.log.Log(
                    $"Bloodline Mutations: Selection instance equals canonical instance: " +
                    $"{ReferenceEquals(bloodlineFromSelection, bloodline)}");

                /*
                * Create one configurator and apply all level-entry replacements before
                * configuring the progression.
                */
                ProgressionConfigurator progressionConfigurator =
                    ProgressionConfigurator.For(bloodline);

                bool progressionChanged = false;

                foreach (int level in BloodlinePowerLevels)
                {
                    LevelEntry levelEntry = bloodline.GetLevelEntry(level);

                    if (levelEntry == null ||
                        levelEntry.m_Features == null ||
                        levelEntry.m_FeaturesList == null)
                    {
                        continue;
                    }

                    /*
                     * Find the bloodline power at this level.
                     *
                     * Bloodline powers add an ability resource, while the bloodline
                     * spell granted at the same level has AddKnownSpell.
                     */
                    List<BlueprintFeatureBase> matchingPowers = levelEntry.m_FeaturesList
                        .Where(candidate =>
                            candidate != null &&
                            candidate.GetComponent<AddAbilityResources>() != null &&
                            candidate.GetComponent<AddKnownSpell>() == null)
                        .ToList();

                    /*
                     * Do not patch this level if its structure is different from what
                     * we expect. The log will identify any unusual bloodlines.
                     */
                    if (matchingPowers.Count != 1)
                    {
                        Main.log.Log(
                            $"Bloodline Mutations: Expected exactly one bloodline power " +
                            $"in {bloodline.name} at level {level}, but found " +
                            $"{matchingPowers.Count}.");

                        foreach (BlueprintFeatureBase feature in levelEntry.m_FeaturesList)
                        {
                            Main.log.Log(
                                $"    {feature.name}: " +
                                $"AddAbilityResources=" +
                                $"{feature.GetComponent<AddAbilityResources>() != null}, " +
                                $"AddKnownSpell=" +
                                $"{feature.GetComponent<AddKnownSpell>() != null}");
                        }

                        continue;
                    }

                    BlueprintFeatureBase bloodlinePower = matchingPowers[0];

                    /*
                     * A BlueprintFeatureSelection expects BlueprintFeature options.
                     */
                    if (bloodlinePower is not BlueprintFeature bloodlinePowerFeature)
                    {
                        Main.log.Log(
                            $"Bloodline Mutations: {bloodlinePower.name} in " +
                            $"{bloodline.name} at level {level} is not a " +
                            $"{nameof(BlueprintFeature)}.");

                        continue;
                    }

                    /*
                     * The internal blueprint name and deterministic GUID are unique to
                     * this bloodline and level.
                     */
                    string selectionName =
                        $"{bloodline.name}Level{level}BloodlineMutationSelection";

                    string selectionGuid = CreateDeterministicGuid(
                        $"EbonsContentMod.BloodlineMutationSelection." +
                        $"{bloodline.AssetGuid}.Level{level}");

                    /*
                     * The player may retain the original power or replace it with one
                     * of the bloodline mutations.
                     *
                     * Prerequisites on the mutation features can control whether each
                     * mutation is legal at a particular level.
                     */
                    BlueprintFeatureSelection powerSelection =
                        FeatureSelectionConfigurator.New(selectionName, selectionGuid)
                            .SetDisplayName(BloodlineMutationName)
                            .SetDescription(BloodlineMutationDescription)
                            .AddToAllFeatures(
                            [
                                bloodlinePowerFeature,
                                bloodHavoc
                                //bloodIntensity,
                                //bloodPiercing
                            ])
                            .Configure();

                    /*
                    * Locate the serialized reference by GUID. Resolved blueprint instances
                    * are not necessarily reference-equal.
                    */
                    List<BlueprintFeatureBaseReference> rebuiltFeatures =
                        levelEntry.m_Features.ToList();

                    int powerIndex = rebuiltFeatures.FindIndex(reference =>
                    {
                        BlueprintFeatureBase referencedFeature = reference?.Get();

                        return referencedFeature != null &&
                               referencedFeature.AssetGuid == bloodlinePower.AssetGuid;
                    });

                    if (powerIndex < 0)
                    {
                        Main.log.Log(
                            $"Bloodline Mutations: Could not find the backing reference for " +
                            $"{bloodlinePower.name} ({bloodlinePower.AssetGuid}) in " +
                            $"{bloodline.name} at level {level}.");

                        continue;
                    }

                    rebuiltFeatures[powerIndex] =
                        powerSelection.ToReference<BlueprintFeatureBaseReference>();

                    progressionConfigurator.RemoveLevelEntry(level);

                    foreach (BlueprintFeatureBaseReference rebuiltFeature in rebuiltFeatures)
                    {
                        progressionConfigurator.AddToLevelEntry(
                            level,
                            rebuiltFeature);
                    }

                    progressionChanged = true;

                    Main.log.Log(
                        $"Bloodline Mutations: Queued replacement of " +
                        $"{bloodlinePower.name} in {bloodline.name} at level {level} " +
                        $"with {selectionName}. Selection GUID: {selectionGuid}");
                }

                if (!progressionChanged)
                    continue;

                BlueprintProgression configuredBloodline =
                    progressionConfigurator.Configure();

                foreach (int level in BloodlinePowerLevels)
                {
                    LevelEntry configuredEntry =
                        configuredBloodline.GetLevelEntry(level);

                    if (configuredEntry?.m_Features == null)
                        continue;

                    Main.log.Log(
                        $"Bloodline Mutations: Final entry for " +
                        $"{configuredBloodline.name}, level {level}:");

                    foreach (BlueprintFeatureBaseReference reference
                        in configuredEntry.m_Features)
                    {
                        BlueprintFeatureBase configuredFeature = reference?.Get();

                        Main.log.Log(
                            $"    {configuredFeature?.name}, " +
                            $"GUID={configuredFeature?.AssetGuid}, " +
                            $"Type={configuredFeature?.GetType().Name}");
                    }
                }
            }
        }
    }
}
