using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using EbonsContentMod.Utilities;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Root;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
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
using TabletopTweaks.Core.Utilities;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using static Kingmaker.UnitLogic.FactLogic.AddMechanicsFeature;

namespace EbonsContentMod.Races
{
    internal class Orc
    {
        private static readonly string OrcName = "EbonsOrcRace";
        private static readonly string OrcFerocityName = "EbonsOrcFerocity";

        internal const string OrcDisplayName = "Orc.Name";
        private static readonly string OrcDescription = "Orc.Description";
        internal const string OrcFerocityDisplayName = "Orc.Ferocity.Name";
        private static readonly string OrcFerocityDescription = "Orc.Ferocity.Description";

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HalfOrcRace.ToString());

        public static void Configure()
        {
            var LightSensitivity = BlueprintTools.GetBlueprint<BlueprintFeature>("{BBA86DB1-B3EF-4AB5-A412-A5910F2B8947}");
            
            var ferocity =
                FeatureConfigurator.New(OrcFerocityName, "{8411D53B-25A9-4481-9E60-7598BAD2F483}")
                .SetDisplayName(OrcFerocityDisplayName)
                .SetDescription(OrcFerocityDescription)
                .AddMechanicsFeature(MechanicsFeatureType.Ferocity)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HalfOrcFerocity.ToString()).Icon)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var race =
            RaceConfigurator.New(OrcName, "{7088A348-EF06-46DA-BDB3-900FB187FB21}")
                .CopyFrom(CopyRace)
                .SetDisplayName(OrcDisplayName)
                .SetDescription(OrcDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(ferocity, FeatureRefs.OrcWeaponFamiliarity.ToString(), FeatureRefs.KeenSenses.ToString(), LightSensitivity)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: 4)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: -2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: -2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: -2)
                .SetRaceId(Race.HalfOrc)
                .Configure();

            // Add race to mount fixes
            RaceMountFixerizer.AddRaceToMountFixes(race, CopyRace);

            // Add race to race list
            var raceRef = race.ToReference<BlueprintRaceReference>();
            ref var races = ref BlueprintRoot.Instance.Progression.m_CharacterRaces;

            var length = races.Length;
            Array.Resize(ref races, length + 1);
            races[length] = raceRef;
        }
    }
}
