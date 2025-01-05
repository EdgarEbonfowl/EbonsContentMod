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

namespace EbonsContentMod.Races
{
    internal class Svirfneblin
    {
        public static List<Color> RaceHeadColors =
            [
                new Color(
                    RaceRecolorizer.GetColorsFromRGB(125f),
                    RaceRecolorizer.GetColorsFromRGB(102f),
                    RaceRecolorizer.GetColorsFromRGB(123f)
                    )
            ];

        public static List<Color> RaceEyeColors =
            [];

        public static List<Color> RaceHairColors =
            [
                new Color(0.75f, 0.75f, 0.75f)
            ];

        private static readonly string SvirfneblinName = "SvirfneblinRace";

        internal const string SvirfneblinDisplayName = "Svirfneblin.Name";
        private static readonly string SvirfneblinDescription = "Svirfneblin.Description";
        public static readonly string RaceGuid = "{EE7B945A-0CF0-4C9F-A6CB-2C29AFF3F4A8}";

        internal static void Configure()
        {
            var race =
            RaceConfigurator.New(SvirfneblinName, RaceGuid)
                .CopyFrom(RaceRefs.GnomeRace)
                .SetDisplayName(SvirfneblinDisplayName)
                .SetDescription(SvirfneblinDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures()
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: -2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: -4)
                .SetRaceId(Race.Gnome)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, BaldRace: true, OnlyMalesBald: true);

            // Add race to race list
            var raceRef = recoloredrace.ToReference<BlueprintRaceReference>();
            ref var races = ref BlueprintRoot.Instance.Progression.m_CharacterRaces;

            var length = races.Length;
            Array.Resize(ref races, length + 1);
            races[length] = raceRef;
        }
    }
}
