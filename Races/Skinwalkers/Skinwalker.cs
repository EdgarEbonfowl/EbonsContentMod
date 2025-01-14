using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.RuleSystem;
using System;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using EbonsContentMod.Utilities;
using UnityEngine;
using Kingmaker.ResourceLinks;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Utils;
using EbonsContentMod.Races.Skinwalkers.SkinwalkerHeritage;
using Kingmaker.Blueprints.Classes.Selection;
using BlueprintCore.Blueprints.Configurators.Items.Weapons;

namespace EbonsContentMod.Races.Skinwalkers
{
    internal class Skinwalker
    {
        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string SkinwalkerName = "SkinwalkerRace";

        internal const string SkinwalkerDisplayName = "Skinwalker.Name";
        private static readonly string SkinwalkerDescription = "Skinwalker.Description";
        public static readonly string RaceGuid = "{0542E029-3247-49CD-A445-3D13B34A31A8}";

        internal const string SkinwalkerHeritageDisplayName = "Skinwalker.Heritage.Name";
        private static readonly string SkinwalkerHeritageDescription = "Skinwalker.Heritage.Description";

        internal static void Configure()
        {
            Blueprint<BlueprintFeatureReference>[] HeritageList =
            [
                ClassicSkinwalker.Configure().ToReference<BlueprintFeatureReference>(),
                Bloodmarked.Configure().ToReference<BlueprintFeatureReference>(),
                Coldborn.Configure().ToReference<BlueprintFeatureReference>(),
                Fanglord.Configure().ToReference<BlueprintFeatureReference>(),
                Nightskulk.Configure().ToReference<BlueprintFeatureReference>(),
                Ragebred.Configure().ToReference<BlueprintFeatureReference>(),
                Scaleheart.Configure().ToReference<BlueprintFeatureReference>(),
                Witchwolf.Configure().ToReference<BlueprintFeatureReference>()
            ];

            var BiteWeapon = ItemWeaponConfigurator.New("SkinwalkerBiteWeapon", "{8D16F61E-D521-4153-89C3-C3163016DBAF}")
                .CopyFrom(ItemWeaponRefs.Bite1d6)
                //.SetAlwaysPrimary(true)
                .Configure();

            var ClawWeapon = ItemWeaponConfigurator.New("SkinwalkerClawWeapon", "{925F0A7D-259B-4781-B1D0-08EF832CCE59}")
                .CopyFrom(ItemWeaponRefs.Claw1d4)
                .Configure();

            var GoreWeapon = ItemWeaponConfigurator.New("SkinwalkerGoreWeapon", "{FA68944E-650C-4CAD-AEBC-EAC07EC0F93B}")
                .CopyFrom(ItemWeaponRefs.Gore1d6)
                //.SetAlwaysPrimary(true)
                //.SetDamageDice(new DiceFormula() { m_Rolls = 1, m_Dice = DiceType.D6 })
                .Configure();

            var SkinwalkerHeritage = FeatureSelectionConfigurator.New("SkinwalkerRacialHeritage", "{1899AAE7-5E43-40DB-AEFF-8AF06ED4CA38}")
                .SetDisplayName(SkinwalkerHeritageDisplayName)
                .SetDescription(SkinwalkerHeritageDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(FeatureSelectionRefs.HalfOrcHeritageSelection.ToString()).Icon)
                .AddToAllFeatures(HeritageList)
                .SetGroup(FeatureGroup.Racial)
                .Configure();

            var race =
            RaceConfigurator.New(SkinwalkerName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(SkinwalkerDisplayName)
                .SetDescription(SkinwalkerDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(SkinwalkerHeritage)
                .SetRaceId(Race.Human)
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
