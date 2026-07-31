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
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;

namespace EbonsContentMod.Races
{
    internal class AquaticElf
    {
        public static List<Color> RaceHeadColors = [];

        public static List<Color> RaceEyeColors = [];

        public static List<Color> RaceHairColors = [];

        public static List<Texture2D> CustomEyeRamps =
        [
            RaceRecolorizer.GetArmorRampByIndex(31),
            RaceRecolorizer.GetArmorRampByIndex(32),
            RaceRecolorizer.GetArmorRampByIndex(33),
            RaceRecolorizer.GetArmorRampByIndex(38),
            RaceRecolorizer.GetArmorRampByIndex(39),
            RaceRecolorizer.GetArmorRampByIndex(45),
            RaceRecolorizer.GetArmorRampByIndex(46),
            RaceRecolorizer.GetArmorRampByIndex(52),
            RaceRecolorizer.GetArmorRampByIndex(59),
            RaceRecolorizer.GetArmorRampByIndex(66)
        ];

        public static List<Texture2D> CustomHeadRamps =
        [
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 13),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 2),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 4),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 9),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 0),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 6),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 7),           
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 3),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.HalfOrcRace.Reference.Get(), 2),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.HalfOrcRace.Reference.Get(), 4),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 0),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 7),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 8),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 10),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 11),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 12),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 9),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 10),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.AasimarRace.Reference.Get(), 3),
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 8),
        ];

        public static List<Texture2D> CustomHairRamps =
        [
            //RaceRecolorizer.GetRaceHairRampByIndex(RaceRefs.HalfOrcRace.Reference.Get(), 2), // light green
            //RaceRecolorizer.GetRaceHairRampByIndex(RaceRefs.HalfOrcRace.Reference.Get(), 4), // dark green
            //RaceRecolorizer.GetArmorRampByIndex(8),
            RaceRecolorizer.GetArmorRampByIndex(54),
            RaceRecolorizer.GetArmorRampByIndex(33),
            RaceRecolorizer.GetArmorRampByIndex(31),                        
            RaceRecolorizer.GetArmorRampByIndex(35),           
            RaceRecolorizer.GetArmorRampByIndex(38),
            RaceRecolorizer.GetArmorRampByIndex(40),
            RaceRecolorizer.GetArmorRampByIndex(41),
            RaceRecolorizer.GetArmorRampByIndex(46),
            RaceRecolorizer.GetArmorRampByIndex(39),
            RaceRecolorizer.GetArmorRampByIndex(47),
            RaceRecolorizer.GetArmorRampByIndex(36),
            RaceRecolorizer.GetArmorRampByIndex(52),
            RaceRecolorizer.GetArmorRampByIndex(34),
            RaceRecolorizer.GetArmorRampByIndex(53),
            RaceRecolorizer.GetArmorRampByIndex(32),
            RaceRecolorizer.GetRaceHairRampByIndex(RaceRefs.OreadRace.Reference.Get(), 2)
        ];

        public static EquipmentEntityLink[] MaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "d69743688fc27584887a7c29a774289e"}, // Long Straight Male Elf
            new EquipmentEntityLink() {AssetId = "e6cb686fb8663734f96ceeacfa2e2400"}, // Long Braids Male Elf
            new EquipmentEntityLink() {AssetId = "8e9394a7a860ead42b2d5acdfb35e3f5"}, // Ponytail Classic Male Elf
            new EquipmentEntityLink() {AssetId = "76cfc339d08471f4da919dcbdd2fceb1"}, // Short Male Half-Elf
            new EquipmentEntityLink() {AssetId = "4da3c565974930a40b51950ee671895e"}, // Slick Male Elf
            new EquipmentEntityLink() {AssetId = "54aae291e3449c14792bbe2592228d4d"}, // Medium Tiny Braid Male Elf
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"}  // Bald
        ];

        public static EquipmentEntityLink[] FemaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "db566d919de425443bd9ae0a37de3ec9"}, // French Braid Female Elf
            new EquipmentEntityLink() {AssetId = "21099c170f7b8344d90d6f034fb5554c"}, // Pompadour Female Elf
            new EquipmentEntityLink() {AssetId = "57c595a0dece66f4283e888dc52d9df1"}, // Long Female Elf
            new EquipmentEntityLink() {AssetId = "c8edacbc502d42242a5911ba000a411e"}, // Long Wavy Braids Female Half-Elf
            new EquipmentEntityLink() {AssetId = "131955108c91c2448a78f8021ca358a9"}, // Medium Anevia Female Elf
            new EquipmentEntityLink() {AssetId = "304b84351ffbde24190e56724178df5b"}, // Long Ember Female Elf
            new EquipmentEntityLink() {AssetId = "9ec441743ea20c5488e7f497992042ed"}, // Ponytail Lush Female Elf
            new EquipmentEntityLink() {AssetId = "39d65ebde5c324f41821b36258791ee5"}, // Side Kare Female Elf
            new EquipmentEntityLink() {AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502"}  // Bald
        ];

        // Need to implement this in the recolorizer first
        public static EquipmentEntityLink[] NewSkinLinkedEEs =
        [
            new EquipmentEntityLink() { AssetId = "fe3418fd8c435cf4384b0d7053283871" } // Lipstick
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.ElfRace.ToString());

        private static readonly string AquaticElfName = "EbonsAquaticElfRace";

        internal const string AquaticElfDisplayName = "AquaticElf.Name";
        private static readonly string AquaticElfDescription = "AquaticElf.Description";
        public static readonly string RaceGuid = "{F547CA63-A435-4F61-9122-454F61159DAF}";

        internal const string WeaponFamiliarityDisplayName = "AquaticElf.WeaponFamiliarity.Name";
        private static readonly string WeaponFamiliarityDescription = "AquaticElf.WeaponFamiliarity.Description";

        internal const string SurfaceDwellerAntagonistDisplayName = "AquaticElf.SurfaceDwellerAntagonist.Name";
        private static readonly string SurfaceDwellerAntagonistDescription = "AquaticElf.SurfaceDwellerAntagonist.Description";

        internal const string AquaticElfRacialHeritageDisplayName = "AquaticElf.RacialHeritage.Name";
        private static readonly string AquaticElfRacialHeritageDescription = "AquaticElf.RacialHeritage.Description";

        //internal const string AncestralGrudgeDisplayName = "AquaticElf.AncestralGrudge.Name";
        //private static readonly string AncestralGrudgeDescription = "AquaticElf.AncestralGrudge.Description";

        internal static void Configure()
        {
            //var ContactEEL = new EquipmentEntityLink() { AssetId = "c5487e7e903d25a40be683767f3df0b4" };

            var Eyes = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "c5487e7e903d25a40be683767f3df0b4" }, CustomEyeRamps, "{582B399A-BC3A-43E2-AB7F-9E2FC0D381AD}", true, true,
                    eyeEE: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.DhampirRace.ToString()).MaleOptions.Heads[0]);

            var WeaponFamiliarity = FeatureConfigurator.New("EbonsAquaticElfWeaponFamiliarity", "{58DCC3AE-1A31-4BDC-A44A-C21B55A6F8AF}")
                .CopyFrom(FeatureRefs.ElvenWeaponFamiliarity.ToString(), c => c is not AddProficiencies)
                .SetDisplayName(WeaponFamiliarityDisplayName)
                .SetDescription(WeaponFamiliarityDescription)
                .SetIcon(FeatureRefs.ElvenWeaponFamiliarity.Reference.Get().Icon)
                .AddProficiencies(weaponProficiencies: [WeaponCategory.Rapier, WeaponCategory.Shortsword, WeaponCategory.Trident])
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var SurfaceDwellerAntagonist = FeatureConfigurator.New("SurfaceDwellerAntagonist", "{00529DE8-9C08-406C-87A2-4611217A78EB}")
                .SetDisplayName(SurfaceDwellerAntagonistDisplayName)
                .SetDescription(SurfaceDwellerAntagonistDescription)
                .SetIcon(FeatureRefs.FavoriteEnemyHuman.Reference.Get().Icon)
                .AddAttackBonusAgainstFactOwner(1, checkedFact: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString()))
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var RacialHeritage = FeatureSelectionConfigurator.New("AquaticElfRacialHeritage", "{ED4B8FFF-6636-4AEF-953D-590D6367108D}")
                .SetDisplayName(AquaticElfRacialHeritageDisplayName)
                .SetDescription(AquaticElfRacialHeritageDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(FeatureSelectionRefs.HalfOrcHeritageSelection.ToString()).Icon)
                .AddToAllFeatures(FeatureRefs.ElvenMagic.ToString(), SurfaceDwellerAntagonist)
                .SetGroup(FeatureGroup.Racial)
                .Configure();

            var race =
            RaceConfigurator.New(AquaticElfName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(AquaticElfDisplayName)
                .SetDescription(AquaticElfDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(RacialHeritage, FeatureRefs.KeenSenses.ToString(), FeatureRefs.ElvenImmunities.ToString(), WeaponFamiliarity, FeatureRefs.SubtypeAquatic.ToString())
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: -2)
                //.AddEquipmentEntity(Eyes)
                .SetRaceId(Race.Elf)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, CustomEyeRamps: CustomEyeRamps, CustomHeadRamps: CustomHeadRamps, CustomHairRamps: CustomHairRamps, eyeEE: Eyes, CustomFemaleHairs: FemaleHairs, CustomMaleHairs: MaleHairs);

            // Add race to mount fixes
            RaceMountFixerizer.AddRaceToMountFixes(recoloredrace, CopyRace);

            // Add race to race list
            var raceRef = recoloredrace.ToReference<BlueprintRaceReference>();
            ref var races = ref BlueprintRoot.Instance.Progression.m_CharacterRaces;

            var length = races.Length;
            Array.Resize(ref races, length + 1);
            races[length] = raceRef;

            // Add portraits
            //PortraitCreatonator.RegisterRacePortrait("AquaticElf_F_01", "{3D3C1DF3-A347-4BD5-B278-68F220C17918}", race, Gender.Female, "AquaticElf_F_01");
            //PortraitCreatonator.RegisterRacePortrait("AquaticElf_M_01", "{25B7718E-0941-4830-9BC1-49A1CAF12B0D}", race, Gender.Male, "AquaticElf_M_01");
        }
    }
}
