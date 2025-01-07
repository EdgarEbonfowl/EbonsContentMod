using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using static Kingmaker.UnitLogic.FactLogic.AddMechanicsFeature;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using Kingmaker.Localization;
using Kingmaker.Designers.Mechanics.Facts;
using static Kingmaker.EntitySystem.Properties.BaseGetter.ListPropertyGetter;
using static Kingmaker.Armies.TacticalCombat.Grid.TacticalCombatGrid;
using System.IO;
using Kingmaker.Craft;
using EbonsContentMod.Utilities;
using Kingmaker.ResourceLinks;
using UnityEngine;
using HarmonyLib;
using Kingmaker.Blueprints.CharGen;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.Visual.CharacterSystem;
using BlueprintCore.Blueprints.Configurators.AI;
using JetBrains.Annotations;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization.Shared;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Runtime.Remoting.Contexts;
using Kingmaker.UnitLogic.Abilities;

namespace EbonsContentMod.Races
{
    internal class Svirfneblin
    {
        public static List<Color> RaceHeadColors =
            [
                new Color( // Deep Gray-Purple
                    RaceRecolorizer.GetColorsFromRGB(117f),
                    RaceRecolorizer.GetColorsFromRGB(102f),
                    RaceRecolorizer.GetColorsFromRGB(117f)
                    ),
                new Color( // Medium Gray
                    RaceRecolorizer.GetColorsFromRGB(125f),
                    RaceRecolorizer.GetColorsFromRGB(125f),
                    RaceRecolorizer.GetColorsFromRGB(130f)
                    ),
                new Color( // Deep Gray-Blue
                    RaceRecolorizer.GetColorsFromRGB(95f),
                    RaceRecolorizer.GetColorsFromRGB(102f),
                    RaceRecolorizer.GetColorsFromRGB(130f)
                    ),
                new Color( // Light Gray
                    RaceRecolorizer.GetColorsFromRGB(150f),
                    RaceRecolorizer.GetColorsFromRGB(150f),
                    RaceRecolorizer.GetColorsFromRGB(155f)
                    ),
                new Color( // Dark Gray
                    RaceRecolorizer.GetColorsFromRGB(100f),
                    RaceRecolorizer.GetColorsFromRGB(100f),
                    RaceRecolorizer.GetColorsFromRGB(105f)
                    ),
                new Color( // Dark Gray-Blue
                    RaceRecolorizer.GetColorsFromRGB(79f),
                    RaceRecolorizer.GetColorsFromRGB(85f),
                    RaceRecolorizer.GetColorsFromRGB(89f)
                    ),
                new Color( // Light Gray-Purple
                    RaceRecolorizer.GetColorsFromRGB(142f),
                    RaceRecolorizer.GetColorsFromRGB(125f),
                    RaceRecolorizer.GetColorsFromRGB(144f)
                    )
            ];

        public static List<Color> RaceEyeColors =
            [];

        public static List<Color> RaceHairColors =
            [
                new Color(0.75f, 0.75f, 0.78f), // White
                new Color(0.86f, 0.86f, 0.89f) // Super White
            ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.GnomeRace.ToString());

        private static readonly string SvirfneblinName = "SvirfneblinRace";

        internal const string SvirfneblinDisplayName = "Svirfneblin.Name";
        private static readonly string SvirfneblinDescription = "Svirfneblin.Description";
        public static readonly string RaceGuid = "{EE7B945A-0CF0-4C9F-A6CB-2C29AFF3F4A8}";

        internal const string SvirfneblinDefensiveTrainingDisplayName = "Svirfneblin.DefensiveTraining.Name";
        private static readonly string SvirfneblinDefensiveTrainingDescription = "Svirfneblin.DefensiveTraining.Description";

        internal const string SvirfneblinFortunateDisplayName = "Svirfneblin.Fortunate.Name";
        private static readonly string SvirfneblinFortunateDescription = "Svirfneblin.Fortunate.Description";

        internal const string SvirfneblinSpellResistanceDisplayName = "Svirfneblin.SpellResistance.Name";
        private static readonly string SvirfneblinSpellResistanceDescription = "Svirfneblin.SpellResistance.Description";

        internal const string SvirfneblinSkilledDisplayName = "Svirfneblin.Skilled.Name";
        private static readonly string SvirfneblinSkilledDescription = "Svirfneblin.Skilled.Description";

        internal const string SvirfneblinSvirfneblinMagicDisplayName = "Svirfneblin.SvirfneblinMagic.Name";
        private static readonly string SvirfneblinSvirfneblinMagicDescription = "Svirfneblin.SvirfneblinMagic.Description";

        internal const string SvirfneblinHatredDisplayName = "Svirfneblin.Hatred.Name";
        private static readonly string SvirfneblinHatredDescription = "Svirfneblin.Hatred.Description";

        internal static void Configure()
        {
            //var SlowSpeedGnome = BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.SlowSpeedGnome.ToString());
            var DefensiveTraining = FeatureConfigurator.New("SvirfneblinDefensiveTraining", "{7A3023F2-713C-4148-97EF-C23D72211BBA}")
                .SetDisplayName(SvirfneblinDefensiveTrainingDisplayName)
                .SetDescription(SvirfneblinDefensiveTrainingDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.GnomeDefensiveTrainingGiants.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Dodge, stat: StatType.AC, value: 2)
                .Configure();

            var Fortunate = FeatureConfigurator.New("SvirfneblinFortunate", "{E197124E-1474-4534-89BD-A8CE175ADFE9}")
                .SetDisplayName(SvirfneblinFortunateDisplayName)
                .SetDescription(SvirfneblinFortunateDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HalflingLuck.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SaveFortitude, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SaveReflex, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SaveWill, value: 2)
                .Configure();

            var SpellResistance = FeatureConfigurator.New("SvirfneblinSpellResistance", "{C22DCC71-17EA-4EEC-9E1E-AC66C003D0C2}")
                .SetDisplayName(SvirfneblinSpellResistanceDisplayName)
                .SetDescription(SvirfneblinSpellResistanceDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.SpellResistance.ToString()).Icon)
                .AddComponent<AddSpellResistance>(c =>
                {
                    c.Value = new ContextValue()
                    {
                        ValueType = ContextValueType.Shared,
                        ValueRank = AbilityRankType.Default,
                        ValueShared = AbilitySharedValue.Damage
                    };
                })
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
                        BonusValue = 11,
                    };
                    c.Modifier = 1.0;
                })
                .AddContextRankConfig(new ContextRankConfig()
                {
                    m_BaseValueType = ContextRankBaseValueType.CharacterLevel,
                    m_SpecificModifier = ModifierDescriptor.None,
                    m_Progression = ContextRankProgression.AsIs
                })
                .Configure();

            // Add the underground enhancement to stealth
            var Skilled = FeatureConfigurator.New("SvirfneblinSkilled", "{5A0117B1-097E-4B73-9797-016B9D72F0B9}")
                .SetDisplayName(SvirfneblinSkilledDisplayName)
                .SetDescription(SvirfneblinSkilledDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HumanSkilled.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillStealth, value: 2)
                .Configure();

            var blurresource = AbilityResourceConfigurator.New("SvirfneblinBlurResource", "{28F9F9CB-3F1D-4BB1-B534-ED53673F0A87}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var blur = AbilityConfigurator.New("SvirfneblinMagicBlur", "{D63B42FA-3B61-44EA-AE84-4D5E1C875CD7}")
                .CopyFrom(AbilityRefs.Blur, c => c is not (SpellListComponent or CraftInfoComponent))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: blurresource)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 2)
                .Configure();

            var blindnessresource = AbilityResourceConfigurator.New("SamsaranBlindnessResource", "{BF6CDACE-5F9E-4234-9B99-DB552093E382}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var blindness = AbilityConfigurator.New("SvirfneblinMagicBlindness", "{1C318A90-2A40-4188-8A59-BE9F912250AF}")
                .CopyFrom(AbilityRefs.Blindness, c => c is not (SpellListComponent or CraftInfoComponent))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: blindnessresource)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 2)
                .Configure();

            var SvirfneblinMagic = FeatureConfigurator.New("SvirfneblinSvirfneblinMagic", "{0688A64D-3EB7-4942-BE08-55DFD7ECC0EC}")
                .SetDisplayName(SvirfneblinSvirfneblinMagicDisplayName)
                .SetDescription(SvirfneblinSvirfneblinMagicDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.GnomeMagic.ToString()).Icon)
                .AddIncreaseSpellSchoolDC(1, school: SpellSchool.Illusion)
                .AddFacts(new() { blur, blindness })
                .AddAbilityResources(1, blurresource, true)
                .AddAbilityResources(1, blindnessresource, true)
                .Configure();

            var Hatred = FeatureConfigurator.New("SvirfneblinHatred", "{5A7FB4E5-1D45-4EE6-9EBC-9EC9D224FE79}")
                .SetDisplayName(SvirfneblinHatredDisplayName)
                .SetDescription(SvirfneblinHatredDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HatredReptilian.ToString()).Icon)
                .AddAttackBonusAgainstFactOwner(1, checkedFact: BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.ReptilianSubtype.ToString()))
                .AddAttackBonusAgainstFactOwner(1, checkedFact: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DwarfRace.ToString()))
                .AddAttackBonusAgainstFactOwner(1, checkedFact: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DuergarRace.ToString()))
                .AddAttackBonusAgainstFactOwner(1, checkedFact: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DuergarTyrantRace.ToString()))
                .Configure();

            var race =
            RaceConfigurator.New(SvirfneblinName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(SvirfneblinDisplayName)
                .SetDescription(SvirfneblinDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(DefensiveTraining, Fortunate, SpellResistance, Skilled, SvirfneblinMagic, Hatred, FeatureRefs.KeenSenses.ToString(), FeatureRefs.SlowSpeedGnome.ToString())
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: -2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: -4)
                .SetRaceId(Race.Gnome)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, BaldRace: true, OnlyMalesBald: true);

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
