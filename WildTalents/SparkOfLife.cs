using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.UnitLogic.Abilities;
using BlueprintCore.Utils.Types;

using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.ElementsSystem;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;

using System;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using TabletopTweaks.Core.Utilities;
using BlueprintCore.Blueprints.References;
using EbonsContentMod.Components;
using static Kingmaker.EntitySystem.Properties.BaseGetter.PropertyContextAccessor;

namespace EbonsContentMod.WildTalents
{
    internal static class SparkOfLife
    {
        private static string KineticistClassGuid = CharacterClassRefs.KineticistClass.ToString();

        private static readonly BlueprintCharacterClassReference KineticistClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>(CharacterClassRefs.KineticistClass.ToString());

        private const string SparkOfLifeFeatureGuid = "{591F5D0C-BC2A-47A8-8BF7-9B03ABB8C222}";
        internal const string SparkOfLifeDisplayName = "SparkOfLife.Name";
        private static readonly string SparkOfLifeDescription = "SparkOfLife.Description";

        private const string SparkOfLifeAirAbilityGuid =
            "{6EEDE324-B637-4F61-A6F5-2E69EF168610}";
        private const string SparkOfLifeAirPoolGuid =
            "{2F4362C7-EAAD-480E-819A-AB9BE434A2F1}";
        internal const string SparkOfLifeAirDisplayName = "SparkOfLifeAir.Name";
        private static readonly string SparkOfLifeAirDescription = "SparkOfLifeAir.Description";

        private const string SparkOfLifeEarthAbilityGuid =
            "{08945FEF-5784-4ABF-A2C9-1BF56AB01DE3}";
        private const string SparkOfLifeEarthPoolGuid =
            "{D0098237-6B5F-4F29-B8D0-93B2F9E8FF5A}";
        internal const string SparkOfLifeEarthDisplayName = "SparkOfLifeEarth.Name";
        private static readonly string SparkOfLifeEarthDescription = "SparkOfLifeEarth.Description";

        private const string SparkOfLifeFireAbilityGuid =
            "{0E00644D-AC81-4805-93E6-A6AAA9C1ECB3}";
        private const string SparkOfLifeFirePoolGuid =
            "{61EDA532-B949-4DED-B4B6-E913DF661C8F}";
        internal const string SparkOfLifeFireDisplayName = "SparkOfLifeFire.Name";
        private static readonly string SparkOfLifeFireDescription = "SparkOfLifeFire.Description";

        private const string SparkOfLifeWaterAbilityGuid =
            "{C4E2CE60-BFA0-4502-85FB-EF9CD061C24B}";
        private const string SparkOfLifeWaterPoolGuid =
            "{D44652F8-E068-4CBC-84D9-57FBE09FD1A6}";
        internal const string SparkOfLifeWaterDisplayName = "SparkOfLifeWater.Name";
        private static readonly string SparkOfLifeWaterDescription = "SparkOfLifeWater.Description";

        /*
         * Replace every placeholder below with the actual unit GUID.
         *
         * You can get these from BlueprintDump, BubblePrints, or your own
         * blueprint logging.
         */
        public static readonly ElementalSet AirUnits = new(
            medium: "676f8b7d0a170674cb6e504e0e30b4f0",
            large: "3764b43791a00e1468257adbca43ce9b",
            huge: "2e24256e459468743b91fbb9aa85e1ab",
            greater: "e770cfbb96b528c4db258d7d03fe6533",
            elder: "33bb90ffd13c87b4c8e45d920313752a");

        public static readonly ElementalSet EarthUnits = new(
            medium: "812c9a0348e004242ba4e46efa91e38e",
            large: "d3d9ab560534bd948b10ac00abbff083",
            huge: "3b86a449e7264174eaccef9b8f02fe20",
            greater: "cda7013db24f4c547b79bfc5c617066b",
            elder: "6b4cb9b6116f2194192e1e7e379c48d7");

        public static readonly ElementalSet FireUnits = new(
            medium: "a0ab0c31b1a92554291a82e598f39ba4",
            large: "ba5026596b06b204eb2efed2b411c5b9",
            huge: "640fb7efb7c916945837bbcab995267e",
            greater: "b0b4091bdaebb464e903857a95189dea",
            elder: "ea0f0bbc6e5e471428d535501b21eb26");

        public static readonly ElementalSet WaterUnits = new(
            medium: "62a3e860e6e72e6499c38bb8b2fe303e",
            large: "680b5b61c80af664daec46af7644486c",
            huge: "877c154a296ee8e45be1a00668319923",
            greater: "fcc939e3acf355b458ddf9617d8c6c28",
            elder: "3bd31a0b4d800f04a8c5b7b1a6d7061e");

        internal static void Configure()
        {
            var AirAbility = CreateSpark(
                name: "SparkOfLifeAir",
                abilityGuid: SparkOfLifeAirAbilityGuid,
                poolGuid: SparkOfLifeAirPoolGuid,
                displayName: SparkOfLifeAirDisplayName,
                description: SparkOfLifeAirDescription,
                icon: BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.SummonElementalLargeAir.ToString()).Icon,
                units: AirUnits,
                prereqs: new BlueprintFeatureReference[] {
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(ProgressionRefs.ElementalFocusAir.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(ProgressionRefs.SecondaryElementAir.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(ProgressionRefs.ThirdElementAir.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(ProgressionRefs.KineticKnightElementalFocusAir.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.AirBlastFeature.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.ElectricBlastFeature.ToString())
                });

            var EarthAbility = CreateSpark(
                name: "SparkOfLifeEarth",
                abilityGuid: SparkOfLifeEarthAbilityGuid,
                poolGuid: SparkOfLifeEarthPoolGuid,
                displayName: SparkOfLifeEarthDisplayName,
                description: SparkOfLifeEarthDescription,
                icon: BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.SummonElementalLargeEarth.ToString()).Icon,
                units: EarthUnits,
                prereqs: new BlueprintFeatureReference[] {
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(ProgressionRefs.ElementalFocusEarth.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(ProgressionRefs.SecondaryElementEarth.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(ProgressionRefs.ThirdElementEarth.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(ProgressionRefs.KineticKnightElementalFocusEarth.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.EarthBlastFeature.ToString())
                });

            var FireAbility = CreateSpark(
                name: "SparkOfLifeFire",
                abilityGuid: SparkOfLifeFireAbilityGuid,
                poolGuid: SparkOfLifeFirePoolGuid,
                displayName: SparkOfLifeFireDisplayName,
                description: SparkOfLifeFireDescription,
                icon: BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.SummonElementalLargeFire.ToString()).Icon,
                units: FireUnits,
                prereqs: new BlueprintFeatureReference[] {
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(ProgressionRefs.ElementalFocusFire.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(ProgressionRefs.SecondaryElementFire.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(ProgressionRefs.ThirdElementFire.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(ProgressionRefs.KineticKnightElementalFocusFire.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.FireBlastFeature.ToString())
                });

            var WaterAbility = CreateSpark(
                name: "SparkOfLifeWater",
                abilityGuid: SparkOfLifeWaterAbilityGuid,
                poolGuid: SparkOfLifeWaterPoolGuid,
                displayName: SparkOfLifeWaterDisplayName,
                description: SparkOfLifeWaterDescription,
                icon: BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.SummonElementalLargeWater.ToString()).Icon,
                units: WaterUnits,
                prereqs: new BlueprintFeatureReference[] {
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(ProgressionRefs.ElementalFocusWater.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(ProgressionRefs.SecondaryElementWater.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(ProgressionRefs.ThirdElementWater.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(ProgressionRefs.KineticKnightElementalFocusWater.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.WaterBlastFeature.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.ColdBlastFeature.ToString())
                });

            /*
             * Later, Spark of Innovation can use the same factory:
             *
             * CreateSpark(
             *     name: "SparkOfInnovation",
             *     displayName: "Spark of Innovation",
             *     description: "...",
             *     units: SparkOfInnovationUnits);
             */

            FeatureConfigurator
                .New(
                    "SparkOfLifeFeature",
                    SparkOfLifeFeatureGuid,
                    FeatureGroup.KineticWildTalent)
                .SetDisplayName(SparkOfLifeDisplayName)
                .SetDescription(SparkOfLifeDescription)
                .SetIcon(
                    BlueprintTools
                        .GetBlueprint<BlueprintAbility>(
                            AbilityRefs.SummonElementalLargeBase.ToString())
                        .Icon)
                .SetIsClassFeature()
                .AddFacts([
                    AirAbility,
                    EarthAbility,
                    FireAbility,
                    WaterAbility
                ])
                .AddPrerequisiteClassLevel(
                    characterClass: KineticistClass,
                    level: 10)
                .Configure();
        }

        public static BlueprintCore.Utils.Blueprint<BlueprintUnitFactReference> CreateSpark(
            string name,
            string abilityGuid,
            string poolGuid,
            string displayName,
            string description,
            BlueprintCore.Utils.Assets.Asset<UnityEngine.Sprite> icon,
            ElementalSet units,
            BlueprintFeatureReference[] prereqs)
        {
            string poolName = $"{name}Pool";
            string abilityName = $"{name}Ability";

            var pool = BlueprintConfigurator<BlueprintSummonPool>
                .New(poolName, poolGuid)
                .OnConfigure(bp =>
                {
                    bp.Limit = 1;
                    bp.DoNotRemoveDeadUnits = false;
                })
                .Configure()
                .ToReference<BlueprintSummonPoolReference>();

            return AbilityConfigurator
                .New(abilityName, abilityGuid)
                .SetDisplayName(displayName)
                .SetDescription(description)
                .SetIcon(icon)
                .SetType(AbilityType.SpellLike)
                .SetRange(AbilityRange.Close)
                .SetActionType(UnitCommand.CommandType.Standard)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Kineticist)
                .AllowTargeting(
                    point: true,
                    enemies: false,
                    friends: false,
                    self: false)
                .AddAbilityKineticist(
                    infusionBurnCost: 0,
                    blastBurnCost: 0,
                    wildTalentBurnCost: 1)
                .AddContextRankConfig(
                    ContextRankConfigs.ClassLevel([KineticistClassGuid]))
                .AddAbilityEffectRunAction(
                    actions: CreateSummonActions(units, pool))
                .AddComponent<AbilityShowIfCasterHasAnyFact>(c =>
                {
                    c.m_Facts = prereqs;
                })
                .Configure();
        }

        private static ActionsBuilder CreateSummonActions(
            ElementalSet units,
            BlueprintSummonPoolReference pool)
        {
            return ActionsBuilder.New()
                .Add(CreateLevelConditional(
                    level: 18,
                    ifTrue: Spawn(units.Elder, pool),
                    ifFalse: CreateLevelConditional(
                        level: 16,
                        ifTrue: Spawn(units.Greater, pool),
                        ifFalse: CreateLevelConditional(
                            level: 14,
                            ifTrue: Spawn(units.Huge, pool),
                            ifFalse: CreateLevelConditional(
                                level: 12,
                                ifTrue: Spawn(units.Large, pool),
                                ifFalse: Spawn(units.Medium, pool))))));
        }

        private static Conditional CreateLevelConditional(
            int level,
            GameAction ifTrue,
            GameAction ifFalse)
        {
            return new Conditional
            {
                ConditionsChecker = new ConditionsChecker
                {
                    Operation = Operation.And,
                    Conditions = new Condition[]
                    {
                        new ContextConditionCompare
                        {
                            m_Type = ContextConditionCompare.Type.GreaterOrEqual,

                            TargetValue = new ContextValue
                            {
                                ValueType = ContextValueType.Simple,
                                Value = level
                            },

                            CheckValue = new ContextValue
                            {
                                ValueType = ContextValueType.Rank,
                                ValueRank = AbilityRankType.Default
                            }
                        }
                    }
                },

                IfTrue = new ActionList
                {
                    Actions = new[] { ifTrue }
                },

                IfFalse = new ActionList
                {
                    Actions = new[] { ifFalse }
                }
            };
        }

        private static ContextActionSpawnMonster Spawn(
            BlueprintUnitReference unit,
            BlueprintSummonPoolReference pool)
        {
            return new ContextActionSpawnMonster
            {
                m_Blueprint = unit,
                m_SummonPool = pool,

                CountValue = new ContextDiceValue
                {
                    DiceType = Kingmaker.RuleSystem.DiceType.Zero,
                    DiceCountValue = SimpleValue(0),
                    BonusValue = SimpleValue(1)
                },

                DurationValue = new ContextDurationValue
                {
                    Rate = DurationRate.Rounds,
                    DiceType = Kingmaker.RuleSystem.DiceType.Zero,
                    DiceCountValue = SimpleValue(0),

                    // Uses the ContextRankConfig on the ability.
                    BonusValue = new ContextValue
                    {
                        ValueType = ContextValueType.Rank,
                        ValueRank = AbilityRankType.Default
                    }
                },

                UseLimitFromSummonPool = true,
                AfterSpawn = new ActionList
                {
                    Actions = Array.Empty<GameAction>()
                }
            };
        }

        private static ContextValue SimpleValue(int value)
        {
            return new ContextValue
            {
                ValueType = ContextValueType.Simple,
                Value = value
            };
        }

        internal sealed class ElementalSet
        {
            internal BlueprintUnitReference Medium { get; }
            internal BlueprintUnitReference Large { get; }
            internal BlueprintUnitReference Huge { get; }
            internal BlueprintUnitReference Greater { get; }
            internal BlueprintUnitReference Elder { get; }

            internal ElementalSet(
                string medium,
                string large,
                string huge,
                string greater,
                string elder)
            {
                Medium = BlueprintTools.GetBlueprintReference<BlueprintUnitReference>(medium);

                Large = BlueprintTools.GetBlueprintReference<BlueprintUnitReference>(large);

                Huge = BlueprintTools.GetBlueprintReference<BlueprintUnitReference>(huge);

                Greater = BlueprintTools.GetBlueprintReference<BlueprintUnitReference>(greater);

                Elder = BlueprintTools.GetBlueprintReference<BlueprintUnitReference>(elder);
            }

            internal ElementalSet(
                BlueprintUnitReference medium,
                BlueprintUnitReference large,
                BlueprintUnitReference huge,
                BlueprintUnitReference greater,
                BlueprintUnitReference elder)
            {
                Medium = medium;
                Large = large;
                Huge = huge;
                Greater = greater;
                Elder = elder;
            }
        }
    }
}
