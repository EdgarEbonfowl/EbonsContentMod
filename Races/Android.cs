using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using System;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using EbonsContentMod.Utilities;
using UnityEngine;
using Kingmaker.ResourceLinks;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Mechanics;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Abilities;

namespace EbonsContentMod.Races
{
    internal class Android
    {
        public static List<Color> RaceHeadColors =
        [
            new Color( // Gray Blue
                RaceRecolorizer.GetColorsFromRGB(115f),
                RaceRecolorizer.GetColorsFromRGB(115f),
                RaceRecolorizer.GetColorsFromRGB(145f)
                ),
            new Color( // Gray
                RaceRecolorizer.GetColorsFromRGB(125f),
                RaceRecolorizer.GetColorsFromRGB(130f),
                RaceRecolorizer.GetColorsFromRGB(135f)
                ),
            new Color( // Gray Tan
                RaceRecolorizer.GetColorsFromRGB(125f),
                RaceRecolorizer.GetColorsFromRGB(110f),
                RaceRecolorizer.GetColorsFromRGB(110f)
                ),
            new Color( // Gray Purple
                RaceRecolorizer.GetColorsFromRGB(120f),
                RaceRecolorizer.GetColorsFromRGB(110f),
                RaceRecolorizer.GetColorsFromRGB(145f)
                ),
            new Color( // Deep Purple Gray
                RaceRecolorizer.GetColorsFromRGB(127f),
                RaceRecolorizer.GetColorsFromRGB(93f),
                RaceRecolorizer.GetColorsFromRGB(109f)
                ),
            new Color( // Deep Blue Gray
                RaceRecolorizer.GetColorsFromRGB(80f),
                RaceRecolorizer.GetColorsFromRGB(93f),
                RaceRecolorizer.GetColorsFromRGB(109f)
                ),
        ];

        public static List<Color> RaceEyeColors =
        [
            new Color( // Pink/Fuscia
                RaceRecolorizer.GetColorsFromRGB(156f),
                RaceRecolorizer.GetColorsFromRGB(94f),
                RaceRecolorizer.GetColorsFromRGB(166f)
                ),
            new Color( // Blue
                RaceRecolorizer.GetColorsFromRGB(76f),
                RaceRecolorizer.GetColorsFromRGB(133f),
                RaceRecolorizer.GetColorsFromRGB(192f)
                ),
            new Color( // Light Blue
                RaceRecolorizer.GetColorsFromRGB(134f),
                RaceRecolorizer.GetColorsFromRGB(173f),
                RaceRecolorizer.GetColorsFromRGB(231f)
                ),
            new Color( // Purple
                RaceRecolorizer.GetColorsFromRGB(137f),
                RaceRecolorizer.GetColorsFromRGB(84f),
                RaceRecolorizer.GetColorsFromRGB(220f)
                ),
            new Color( // Light Red
                RaceRecolorizer.GetColorsFromRGB(190f),
                RaceRecolorizer.GetColorsFromRGB(85f),
                RaceRecolorizer.GetColorsFromRGB(85f)
                ),
            new Color( // Light Yellow
                RaceRecolorizer.GetColorsFromRGB(248f),
                RaceRecolorizer.GetColorsFromRGB(226f),
                RaceRecolorizer.GetColorsFromRGB(174f)
                ),
            new Color( // Green
                RaceRecolorizer.GetColorsFromRGB(166f),
                RaceRecolorizer.GetColorsFromRGB(226f),
                RaceRecolorizer.GetColorsFromRGB(119f)
                ),
            new Color( // White
                RaceRecolorizer.GetColorsFromRGB(200f),
                RaceRecolorizer.GetColorsFromRGB(200f),
                RaceRecolorizer.GetColorsFromRGB(200f)
                ),
            new Color( // Turqois
                RaceRecolorizer.GetColorsFromRGB(118f),
                RaceRecolorizer.GetColorsFromRGB(200f),
                RaceRecolorizer.GetColorsFromRGB(184f)
                ),
        ];

        public static EquipmentEntityLink[] NewEyeLinkedEEs =
        [
            new EquipmentEntityLink() { AssetId = "d73e1538363d26645aaeba0beaa4d250" } // Circuitry
        ];

        public static List<Color> RaceHairColors = [];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DhampirRace.ToString());

        private static readonly string AndroidName = "AndroidRace";

        internal const string AndroidDisplayName = "Android.Name";
        private static readonly string AndroidDescription = "Android.Description";
        public static readonly string RaceGuid = "{D805F2CF-9C7F-4C21-82AB-98F8AF92CE3E}";

        internal const string AlertDisplayName = "Android.Alert.Name";
        private static readonly string AlertDescription = "Android.Alert.Description";

        internal const string EmotionlessDisplayName = "Android.Emotionless.Name";
        private static readonly string EmotionlessDescription = "Android.Emotionless.Description";

        internal const string ConstructedDisplayName = "Android.Constructed.Name";
        private static readonly string ConstructedDescription = "Android.Constructed.Description";

        internal const string NaniteSurgeDisplayName = "Android.NaniteSurge.Name";
        private static readonly string NaniteSurgeDescription = "Android.NaniteSurge.Description";

        internal static void Configure()
        {
            var Alert = FeatureConfigurator.New("AndroidAlert", "{50D50AB3-D6DA-4D62-8C4B-E46DE01FB632}")
                .SetDisplayName(AlertDisplayName)
                .SetDescription(AlertDescription)
                .SetIcon(FeatureRefs.HunterTactics.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillPerception, value: 4)
                .Configure();
             
            var Emotionless = FeatureConfigurator.New("AndroidEmotionless", "{B0D49F9E-16EE-4C0F-8419-8E5B2E619445}") // Icon
                .SetDisplayName(EmotionlessDisplayName)
                .SetDescription(EmotionlessDescription)
                .SetIcon(FeatureRefs.MindGamesFeature.Reference.Get().Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.CheckBluff, value: -4)
                .Configure();
            
            var Constructed = FeatureConfigurator.New("AndroidConstructed", "{64168E5C-8C84-4ECD-B2B2-556A9662F288}") // Icon
                .SetDisplayName(ConstructedDisplayName)
                .SetDescription(ConstructedDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.Stoneskin.ToString()).Icon)
                .AddSavingThrowBonusAgainstDescriptor(4, modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.MindAffecting)
                .AddSavingThrowBonusAgainstDescriptor(4, modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.Paralysis)
                .AddSavingThrowBonusAgainstDescriptor(4, modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.Poison)
                .AddSavingThrowBonusAgainstDescriptor(4, modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.Stun)
                .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Fatigue)
                .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Exhausted)
                .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Disease)
                .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Sleep)
                .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Fear)
                .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Emotion)
                .Configure();

            // Nanite Surge
            var NaniteSurgeResource = AbilityResourceConfigurator.New("AndroidNaniteSurgeResource", "{4C8E5E38-53C7-4972-96EF-71FF981F7378}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var NaniteSurgeBuff = BuffConfigurator.New("AndroidNaniteSurgeBuff", "{2FB436D7-5A48-4104-8A20-016DCA4EA105}")
                .SetDisplayName(NaniteSurgeDisplayName)
                .SetDescription(NaniteSurgeDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.RemoveDisease.ToString()).Icon) // Change
                .AddModifyD20(ActionsBuilder.New().RemoveSelf().Build(), addBonus: true, bonus: new ContextValue()
                {
                    ValueType = ContextValueType.Shared,
                    ValueRank = AbilityRankType.Default,
                    ValueShared = AbilitySharedValue.Damage
                }, bonusDescriptor: ModifierDescriptor.UntypedStackable)
                .AddComponent<ContextCalculateSharedValue>(c =>
                {
                    c.ValueType = AbilitySharedValue.Damage;
                    c.Value = new ContextDiceValue()
                    {
                        DiceType = Kingmaker.RuleSystem.DiceType.One,
                        DiceCountValue = new ContextValue()
                        {
                            ValueType = ContextValueType.Rank
                        },
                        BonusValue = 3,
                    };
                    c.Modifier = 1.0;
                })
                .AddComponent<ContextRankConfig>(c =>
                {
                    c.m_Type = AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                    c.m_Stat = StatType.Unknown;
                    c.m_SpecificModifier = ModifierDescriptor.None;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_StartLevel = 0;
                    c.m_StepLevel = 0;
                    c.m_UseMax = true;
                    c.m_Max = 20;
                    /*c.m_CustomProgression = new ContextRankConfig.CustomProgressionItem[] { // There has to be a better way...
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 1, ProgressionValue = 4 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 2, ProgressionValue = 5 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 3, ProgressionValue = 6 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 4, ProgressionValue = 7 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 5, ProgressionValue = 8 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 6, ProgressionValue = 9 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 7, ProgressionValue = 10 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 8, ProgressionValue = 11 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 9, ProgressionValue = 12 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 10, ProgressionValue = 13 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 11, ProgressionValue = 14 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 12, ProgressionValue = 15 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 13, ProgressionValue = 16 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 14, ProgressionValue = 17 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 15, ProgressionValue = 18 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 16, ProgressionValue = 19 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 17, ProgressionValue = 20 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 18, ProgressionValue = 21 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 19, ProgressionValue = 22 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 20, ProgressionValue = 23 },
                        new ContextRankConfig.CustomProgressionItem(){ BaseValue = 100, ProgressionValue = 23 }
                    };*/
                })
                .SetFxOnStart("3cf209e5299921349a1c159f35cfa369") // Trying faerie fire
                .Configure();

            var NaniteSurgeAbility = AbilityConfigurator.New("AndroidNaniteSurgeAbility", "{CF067A9E-2A1A-438D-9279-43ADB4FB2D28}")
                .SetDisplayName(NaniteSurgeDisplayName)
                .SetDescription(NaniteSurgeDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.RemoveDisease.ToString()).Icon) // Change, maybe Lore Master, Indomitable Will, or Cantrip Master
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuffWithDurationSeconds(NaniteSurgeBuff, 6.0f).Build())
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: NaniteSurgeResource)
                .SetRange(AbilityRange.Personal)
                .SetAnimation(Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell.CastAnimationStyle.Self)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Swift)
                .AllowTargeting(false, false, false, true)
                .Configure();

            var NaniteSurgeFeat = FeatureConfigurator.New("AndroidNaniteSurgeFeature", "{3984BC80-C075-43C8-802A-D8C4E6A01636}")
                .SetDisplayName(NaniteSurgeDisplayName)
                .SetDescription(NaniteSurgeDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.RemoveDisease.ToString()).Icon) // Change
                .AddFacts(new() { NaniteSurgeAbility })
                .AddAbilityResources(1, NaniteSurgeResource, true)
                .Configure();

            var race =
            RaceConfigurator.New(AndroidName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(AndroidDisplayName)
                .SetDescription(AndroidDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(Alert, Emotionless, Constructed, NaniteSurgeFeat)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: -2)
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, eyecolors: RaceEyeColors, BaldRace: true, EyeLinkedEEs: NewEyeLinkedEEs);

            // Add race to mount fixes
            RaceMountFixerizer.AddRaceToMountFixes(recoloredrace, CopyRace);

            // Add race to race list
            var raceRef = recoloredrace.ToReference<BlueprintRaceReference>();
            ref var races = ref BlueprintRoot.Instance.Progression.m_CharacterRaces;

            var length = races.Length;
            Array.Resize(ref races, length + 1);
            races[length] = raceRef;
        }
    }
}
