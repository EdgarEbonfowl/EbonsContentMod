using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EbonsContentMod.WildTalents.SparkOfLife;
using TabletopTweaks.Core.Utilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.References;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes;
using Kingmaker.UnitLogic.Class.LevelUp;
using static EbonsContentMod.Utilities.Clonerators;
using static UnityEngine.Networking.UnityWebRequest;
using BlueprintCore.Utils;
using EbonsContentMod.Components;
using Kingmaker.Localization;

namespace EbonsContentMod.WildTalents
{
    internal class SparkOfInnovation
    {
        private const string SparkOfInnovationFeatureGuid =
            "{D1D761B3-A986-4B26-850B-F4062EEC00D0}";

        private const string SparkOfInnovationAbilityGuid =
            "{BB50E3C9-1BAF-4115-A1DE-43943CC5B7AD}";

        private const string SparkOfInnovationPoolGuid =
            "{BDF81B4F-6F18-4950-9775-30F43BBBA575}";

        private const string SparkOfInnovationMediumUnitGuid =
            "{85ABAA6D-42C0-43B9-A927-F73CFB968717}";

        private const string SparkOfInnovationLargeUnitGuid =
            "{17388700-FDB6-4738-BEE2-3E210A96F5DD}";

        private const string SparkOfInnovationHugeUnitGuid =
            "{89795B0A-C080-41BF-8646-3B8F4E18311D}";

        private const string SparkOfInnovationGreaterUnitGuid =
            "{46E779BF-69CA-4D3F-9135-9D1900E8937F}";

        private const string SparkOfInnovationElderUnitGuid =
            "{659C4563-D6E0-4EA0-9B9C-A9DBCCD58144}";

        internal const string SparkOfInnovationDisplayName =
            "SparkOfInnovation.Name";

        private static readonly string SparkOfInnovationDescription =
            "SparkOfInnovation.Description";

        private const string IronGolemUnitGuid =
            "6a706bf502d310a49b4fdd9e45b199b1";

        private const string ElementalSubtypeGuid =
            "198fd8924dabcb5478d0f78bd453c586";

        private const string EarthSubtypeGuid =
            "e147258e5b7c40643893d80c9f2816e8";

        private const string ExtraplanarSubtypeGuid =
            "136fa0343d5b4b348bdaa05d83408db3";

        private const string ConstructTraitsGuid =
            "4ccca90d0556b554eb6e7dbd665c4d41"; // Iron Golem Immunity

        private const string OutsiderTraitGuid =
            "e2986f96fa1cd3b4f8d9dfd8a9907731";

        internal const string ClockworkGolemDisplayName =
            "ClockworkGolem.Name";

        private const string MediumScaleFeatureGuid =
            "{9A9EFF8A-032F-4C07-9ED6-3FF5F6E239D8}";

        private const string LargeScaleFeatureGuid =
            "{7A2EAAAF-8E34-4CD2-9B0D-59B0B100554D}";

        private const string HugeScaleFeatureGuid =
            "{D0339114-C753-44DE-BC1B-78DD375BC94A}";

        private const string GreaterScaleFeatureGuid =
            "{16A9817E-3C12-403E-BCA5-3544EB37DEE5}";

        private const string ElderScaleFeatureGuid =
            "{93409080-B91E-43E2-A649-449AF48AD4CF}";

        private static LocalizedString ClockworkGolemName = 
            LocalizationTool.GetString(ClockworkGolemDisplayName);

        private static ElementalSet CreateSparkOfInnovationUnits()
        {
            var ironGolem =
                BlueprintTools.GetBlueprint<BlueprintUnit>(IronGolemUnitGuid);

            var elementalTraits = new BlueprintUnitFactReference[] {
                BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(
                    ElementalSubtypeGuid),
                BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(
                    EarthSubtypeGuid),
                BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(
                    ExtraplanarSubtypeGuid),
                BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(
                    OutsiderTraitGuid)
            };

            var constructTraits =
                BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(
                    ConstructTraitsGuid);

            var mediumScale = CreateScaleFeature(
                "SparkOfInnovationMediumScale",
                MediumScaleFeatureGuid,
                0.0f);

            var largeScale = CreateScaleFeature(
                "SparkOfInnovationLargeScale",
                LargeScaleFeatureGuid,
                0.15f);

            var hugeScale = CreateScaleFeature(
                "SparkOfInnovationHugeScale",
                HugeScaleFeatureGuid,
                0.3f);

            var greaterScale = CreateScaleFeature(
                "SparkOfInnovationGreaterScale",
                GreaterScaleFeatureGuid,
                0.45f);

            var elderScale = CreateScaleFeature(
                "SparkOfInnovationElderScale",
                ElderScaleFeatureGuid,
                0.6f);

            return new ElementalSet(
                medium: CreateInnovationUnit(
                    "SparkOfInnovationMediumUnit",
                    SparkOfInnovationMediumUnitGuid,
                    EarthUnits.Medium,
                    ironGolem,
                    elementalTraits,
                    constructTraits,
                    mediumScale),

                large: CreateInnovationUnit(
                    "SparkOfInnovationLargeUnit",
                    SparkOfInnovationLargeUnitGuid,
                    EarthUnits.Large,
                    ironGolem,
                    elementalTraits,
                    constructTraits,
                    largeScale),

                huge: CreateInnovationUnit(
                    "SparkOfInnovationHugeUnit",
                    SparkOfInnovationHugeUnitGuid,
                    EarthUnits.Huge,
                    ironGolem,
                    elementalTraits,
                    constructTraits,
                    hugeScale),

                greater: CreateInnovationUnit(
                    "SparkOfInnovationGreaterUnit",
                    SparkOfInnovationGreaterUnitGuid,
                    EarthUnits.Greater,
                    ironGolem,
                    elementalTraits,
                    constructTraits,
                    greaterScale),

                elder: CreateInnovationUnit(
                    "SparkOfInnovationElderUnit",
                    SparkOfInnovationElderUnitGuid,
                    EarthUnits.Elder,
                    ironGolem,
                    elementalTraits,
                    constructTraits,
                    elderScale));
        }

        private static BlueprintUnitReference CreateInnovationUnit(
            string name,
            string guid,
            BlueprintUnitReference earthElemental,
            BlueprintUnit ironGolem,
            BlueprintUnitFactReference[] elementalTraits,
            BlueprintUnitFactReference constructTraits,
            BlueprintFeatureReference scaleFeature)
        {
            var source = earthElemental.Get();

            var unit = BlueprintConfigurator<BlueprintUnit>
                .New(name, guid)
                .CopyFrom(source, component => true)
                .OnConfigure(bp =>
                {
                    CopyUnitFields(bp, source);

                    // Visuals
                    
                    bp.Prefab = ironGolem.Prefab;
                    bp.Visual = ironGolem.Visual;
                    bp.m_Portrait = ironGolem.m_Portrait;
                    bp.m_DisplayName = ClockworkGolemName;
                    bp.LocalizedName = new SharedStringAsset { String = ClockworkGolemName };
                    bp.Color = ironGolem.Color;

                    // Class levels

                    var oldClassLevels = source.GetComponent<AddClassLevels>();

                    if (oldClassLevels != null)
                    {
                        var newClassLevels = new AddClassLevels
                        {
                            m_CharacterClass = CharacterClassRefs.ConstructClass.Reference.Get().ToReference<BlueprintCharacterClassReference>(),
                            //m_CharacterClass = BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.ConstructClass.ToString()),

                            Levels = oldClassLevels.Levels,
                            RaceStat = oldClassLevels.RaceStat,
                            LevelsStat = oldClassLevels.LevelsStat,
                            Skills = oldClassLevels.Skills?.ToArray(),
                            Selections = oldClassLevels.Selections?.ToArray(),
                            DoNotApplyAutomatically = oldClassLevels.DoNotApplyAutomatically
                        };

                        bp.RemoveComponents<AddClassLevels>();
                        bp.AddComponent(newClassLevels);
                    }

                    // Features

                    var facts = bp.m_AddFacts?.ToList()
                        ?? new List<BlueprintUnitFactReference>();

                    RemoveFacts(facts, elementalTraits);

                    AddFactIfMissing(facts, constructTraits);
                    AddFactIfMissing(facts, scaleFeature.Get().ToReference<BlueprintUnitFactReference>());

                    bp.m_AddFacts = facts.ToArray();
                })
                .Configure();

            Main.log.Log(
                $"{name}: " +
                $"Prefab={(unit.Prefab == null ? "NULL" : "present")}, " +
                $"Faction={(unit.m_Faction == null ? "NULL" : "present")}, " +
                $"Brain={(unit.m_Brain == null ? "NULL" : "present")}, " +
                $"Facts={unit.m_AddFacts?.Length ?? 0}, " +
                $"Templates={unit.m_AdditionalTemplates?.Length ?? 0}, " +
                $"Size={unit.Size}, " +
                $"Components={unit.ComponentsArray?.Length ?? 0}");

            return unit.ToReference<BlueprintUnitReference>();
        }

        private static void CopyUnitFields(
            BlueprintUnit target,
            BlueprintUnit source)
        {
            target.m_DisplayName = source.m_DisplayName;
            target.m_Description = source.m_Description;
            target.m_Icon = source.m_Icon;
            target.LocalizedName = source.LocalizedName;

            target.m_Type = source.m_Type;
            target.m_Portrait = source.m_Portrait;
            target.m_Faction = source.m_Faction;
            target.FactionOverrides = source.FactionOverrides;
            target.m_Brain = source.m_Brain;

            target.Prefab = source.Prefab;
            target.Visual = source.Visual;
            target.Body = source.Body;

            target.Gender = source.Gender;
            target.Size = source.Size;
            target.IsLeftHanded = source.IsLeftHanded;
            target.Color = source.Color;
            target.Alignment = source.Alignment;

            target.Strength = source.Strength;
            target.Dexterity = source.Dexterity;
            target.Constitution = source.Constitution;
            target.Intelligence = source.Intelligence;
            target.Wisdom = source.Wisdom;
            target.Charisma = source.Charisma;

            target.Speed = source.Speed;
            target.Skills = source.Skills;

            target.m_AddFacts = source.m_AddFacts?.ToArray()
                ?? Array.Empty<BlueprintUnitFactReference>();

            target.m_AdditionalTemplates =
                source.m_AdditionalTemplates?.ToArray()
                ?? Array.Empty<BlueprintUnitTemplateReference>();
        }

        private static void RemoveFacts(
            List<BlueprintUnitFactReference> facts,
            params BlueprintUnitFactReference[] remove)
        {
            facts.RemoveAll(candidate =>
                remove.Any(removal =>
                    removal != null
                    && candidate != null
                    && candidate.deserializedGuid
                        == removal.deserializedGuid));
        }

        private static void AddFactIfMissing(
            List<BlueprintUnitFactReference> facts,
            BlueprintUnitFactReference fact)
        {
            if (fact == null)
                return;

            if (!facts.Any(existing =>
                    existing != null
                    && existing.deserializedGuid == fact.deserializedGuid))
            {
                facts.Add(fact);
            }
        }

        private static BlueprintFeatureReference CreateScaleFeature(
            string name,
            string guid,
            float scale)
        {
            var feature = FeatureConfigurator
                .New(name, guid)
                .SetHideInUI()
                .SetHideInCharacterSheetAndLevelUp()
                /*.AddComponent<UnitVisualScale>(component =>
                {
                    component.Scale = scale;
                })*/
                .AddUnitScale(scaleIncreaseCoefficient: scale)
                .Configure()
                .ToReference<BlueprintFeatureReference>();

            return feature;
        }

        internal static void Configure()
        {
            var InnovationUnits = CreateSparkOfInnovationUnits();

            var InnovationAbility = CreateSpark(
                name: "SparkOfInnovation",
                abilityGuid: SparkOfInnovationAbilityGuid,
                poolGuid: SparkOfInnovationPoolGuid,
                displayName: SparkOfInnovationDisplayName,
                description: SparkOfInnovationDescription,
                icon: BlueprintTools
                    .GetBlueprint<BlueprintFeature>(
                        FeatureRefs.ArmoredHulkIndomitableStance.ToString())
                    .Icon,
                units: InnovationUnits,
                prereqs: new BlueprintFeatureReference[]
                {
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(
                        FeatureRefs.MetalBlastFeature.ToString())
                });

            FeatureConfigurator
            .New("SparkOfInnovationFeature", SparkOfInnovationFeatureGuid, FeatureGroup.KineticWildTalent)
            .SetDisplayName(SparkOfInnovationDisplayName)
            .SetDescription(SparkOfInnovationDescription)
            .SetIcon(
                BlueprintTools
                    .GetBlueprint<BlueprintFeature>(
                        FeatureRefs.ArmoredHulkIndomitableStance.ToString())
                    .Icon)
            .SetIsClassFeature()
            .AddFacts([
                InnovationAbility
            ])
            .AddPrerequisiteClassLevel(
                characterClass: BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>(CharacterClassRefs.KineticistClass.ToString()),
                level: 10)
            .AddPrerequisiteFeature(
                BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(
                    FeatureRefs.MetalBlastFeature.ToString()))
            .Configure();
        }
    }
}
