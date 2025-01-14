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
using EbonsContentMod.Components;

namespace EbonsContentMod.Races
{
    internal class Duergar
    {
        public static List<Color> RaceHeadColors =
            [
                new Color( // Dark Gray
                    RaceRecolorizer.GetColorsFromRGB(90f),
                    RaceRecolorizer.GetColorsFromRGB(90f),
                    RaceRecolorizer.GetColorsFromRGB(95f)
                    ),
                new Color( // Dark Gray with green
                    RaceRecolorizer.GetColorsFromRGB(90f),
                    RaceRecolorizer.GetColorsFromRGB(96f),
                    RaceRecolorizer.GetColorsFromRGB(90f)
                    ),
                new Color( // Lighter Medium Gray
                    RaceRecolorizer.GetColorsFromRGB(120f),
                    RaceRecolorizer.GetColorsFromRGB(120f),
                    RaceRecolorizer.GetColorsFromRGB(125f)
                    ),
                new Color( // Very Dark Gray
                    RaceRecolorizer.GetColorsFromRGB(75f),
                    RaceRecolorizer.GetColorsFromRGB(75f),
                    RaceRecolorizer.GetColorsFromRGB(80f)
                    ),
                new Color( // Medium Gray with green
                    RaceRecolorizer.GetColorsFromRGB(110f),
                    RaceRecolorizer.GetColorsFromRGB(116f),
                    RaceRecolorizer.GetColorsFromRGB(110f)
                    ),
                new Color( // Lightest Medium Gray
                    RaceRecolorizer.GetColorsFromRGB(135f),
                    RaceRecolorizer.GetColorsFromRGB(135f),
                    RaceRecolorizer.GetColorsFromRGB(140f)
                    ),
                new Color( // Medium Gray
                    RaceRecolorizer.GetColorsFromRGB(105f),
                    RaceRecolorizer.GetColorsFromRGB(105f),
                    RaceRecolorizer.GetColorsFromRGB(110f)
                    )
            ];

        public static List<Color> RaceEyeColors =
            [];

        public static List<Color> RaceHairColors =
            [
                new Color(0.75f, 0.75f, 0.78f), // White
                new Color(0.86f, 0.86f, 0.89f), // Super White
                new Color( // Lightest Medium Gray
                    RaceRecolorizer.GetColorsFromRGB(135f),
                    RaceRecolorizer.GetColorsFromRGB(135f),
                    RaceRecolorizer.GetColorsFromRGB(140f)
                    ),
                new Color( // Medium Gray
                    RaceRecolorizer.GetColorsFromRGB(115f),
                    RaceRecolorizer.GetColorsFromRGB(115f),
                    RaceRecolorizer.GetColorsFromRGB(120f)
                    ),
                new Color( // Very Dark Gray
                    RaceRecolorizer.GetColorsFromRGB(75f),
                    RaceRecolorizer.GetColorsFromRGB(75f),
                    RaceRecolorizer.GetColorsFromRGB(80f)
                    )
            ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DwarfRace.ToString());

        private static readonly string DuergarName = "DuergarRace";

        internal const string DuergarDisplayName = "Duergar.Name";
        private static readonly string DuergarDescription = "Duergar.Description";
        public static readonly string RaceGuid = "{AC2584F8-67F2-4C84-99B8-C77572DD4A61}";

        internal const string DuergarImmunitiesDisplayName = "Duergar.DuergarImmunities.Name";
        private static readonly string DuergarImmunitiesDescription = "Duergar.DuergarImmunities.Description";

        internal const string DuergarEnlargePersonDisplayName = "Duergar.EnlargePerson.Name";
        private static readonly string DuergarEnlargePersonDescription = "Duergar.EnlargePerson.Description";

        internal const string DuergarInvisibilityDisplayName = "Duergar.Invisibility.Name";
        private static readonly string DuergarInvisibilityDescription = "Duergar.Invisibility.Description";

        internal static void Configure()
        {
            var LightSensitivity = BlueprintTools.GetBlueprint<BlueprintFeature>("{BBA86DB1-B3EF-4AB5-A412-A5910F2B8947}");

            var DuergarImmunities = FeatureConfigurator.New("DuergarImmunities", "{C36883B5-F3F2-49A1-BFF2-DD3B6E05009C}")
                .SetDisplayName(DuergarImmunitiesDisplayName)
                .SetDescription(DuergarImmunitiesDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.Hardy.ToString()).Icon)
                .AddConditionImmunity(Kingmaker.UnitLogic.UnitCondition.Paralyzed)
                .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Poison)
                .AddComponent<DuergarImmunitiesComponent>()
                .Configure();

            var enlargepersonresource = AbilityResourceConfigurator.New("DuergarEnlargePersonResource", "{8EA4B4C6-CB46-4E9E-8113-F4A0ABFE661A}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var enlargepersonability = AbilityConfigurator.New("DuergarEnlargePersonAbility", "{3D98E624-C964-496B-ABBF-16F44A837B89}")
                .CopyFrom(AbilityRefs.EnlargeSelf, c => c is not (SpellListComponent or CraftInfoComponent))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: enlargepersonresource)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 1)
                .Configure();

            var enlargepersonfeat = FeatureConfigurator.New("DuergarEnlargePerson", "{54B13D22-1251-4CAF-BCA8-9F793573F976}")
                .SetDisplayName(DuergarEnlargePersonDisplayName)
                .SetDescription(DuergarEnlargePersonDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.EnlargePerson.ToString()).Icon)
                .AddFacts(new() { enlargepersonability })
                .AddAbilityResources(1, enlargepersonresource, true)
                .AddReplaceCasterLevelOfAbility(spell: enlargepersonability)
                .Configure();

            var invisibilityresource = AbilityResourceConfigurator.New("DuergarInvisibilityResource", "{5E90DBEB-27FF-4E35-A77F-D27FA0CEEA13}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var invisibilityability = AbilityConfigurator.New("DuergarInvisibilityAbility", "{A849EB50-54A1-42BE-BB32-1CF3A044C7BC}")
                .CopyFrom(AbilityRefs.CureLightWoundsCast, c => c is not (SpellListComponent or CraftInfoComponent))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: invisibilityresource)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 2)
                .Configure();

            var invisibilityfeat = FeatureConfigurator.New("DuergarInvisibility", "{8528D021-3C86-4B71-940D-D375B50D6619}")
                .SetDisplayName(DuergarInvisibilityDisplayName)
                .SetDescription(DuergarInvisibilityDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.GnomeMagic.ToString()).Icon)
                .AddFacts(new() { invisibilityability })
                .AddAbilityResources(1, invisibilityresource, true)
                .Configure();

            var race =
            RaceConfigurator.New(DuergarName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(DuergarDisplayName)
                .SetDescription(DuergarDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(DuergarImmunities, FeatureRefs.Stability.ToString(), enlargepersonfeat, invisibilityfeat, FeatureRefs.KeenSenses.ToString(), FeatureRefs.SlowAndSteady.ToString(), LightSensitivity)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: -4)
                .SetRaceId(Race.Dwarf)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, BaldRace: true, StartMalesWithBeard: true);

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
