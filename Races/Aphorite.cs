using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using System;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.NewComponents;
using EbonsContentMod.Utilities;
using UnityEngine;
using Kingmaker.ResourceLinks;
using Kingmaker.Enums.Damage;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using EbonsContentMod.Components;
using Kingmaker.UnitLogic.Mechanics;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.Craft;
using Kingmaker.UI.SettingsUI;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Mechanics.Components;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.Utility;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.Designers.Mechanics.Buffs;
using static Kingmaker.Blueprints.Classes.Spells.SuppressSpellSchool;
using TabletopTweaks.Core.NewComponents.AbilitySpecific;
using TabletopTweaks.Core.NewUnitParts;
using Kingmaker.UnitLogic.Commands.Base;

namespace EbonsContentMod.Races
{
    internal class Aphorite
    {
        public static List<Color> RaceHeadColors =
        [

        ];

        public static List<Color> RaceEyeColors =
        [

        ];

        public static List<Color> RaceHairColors =
        [

        ];

        public static List<Texture2D> CustomEyeRamps =
        [
            
        ];

        public static List<Texture2D> CustomHeadRamps =
        [
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 13), // Gray            
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 1), // Rock Gray
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 0), // Medium Gray
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 6), // Salmon-Coral            
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 7), // Deep Coral
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 8), // Dark Coral
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 0), // Light Gray
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.ElfRace.Reference.Get(), 3), // Golden Copper
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 0), // Sand Brown
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 8), // Light Bronze
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.OreadRace.Reference.Get(), 7), // Rust
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 1), // Light Gray-Brown
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 4), // Blue-Gray
            RaceRecolorizer.GetRaceSkinRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 5), // Pallid White
        ];

        public static List<Texture2D> CustomHairRamps =
        [
            RaceRecolorizer.GetRaceHairRampByIndex(RaceRefs.AasimarRace.Reference.Get(), 20),
            RaceRecolorizer.GetRaceHairRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 20),
            RaceRecolorizer.GetRaceHairRampByIndex(RaceRefs.DhampirRace.Reference.Get(), 3),
            RaceRecolorizer.GetRaceHairRampByIndex(RaceRefs.AasimarRace.Reference.Get(), 29),
            RaceRecolorizer.GetRaceHairRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 18),
            RaceRecolorizer.GetRaceHairRampByIndex(RaceRefs.AasimarRace.Reference.Get(), 7),
            RaceRecolorizer.GetRaceHairRampByIndex(RaceRefs.AasimarRace.Reference.Get(), 18),
            RaceRecolorizer.GetRaceHairRampByIndex(RaceRefs.TieflingRace.Reference.Get(), 17),
            RaceRecolorizer.GetRaceHairRampByIndex(RaceRefs.AasimarRace.Reference.Get(), 19),
        ];

        public static EquipmentEntityLink[] FemaleHairs =
        [
            
        ];

        public static EquipmentEntityLink[] MaleHairs =
        [
            
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string AphoriteName = "AphoriteRace";

        internal const string AphoriteDisplayName = "Aphorite.Name";
        private static readonly string AphoriteDescription = "Aphorite.Description";
        public static readonly string RaceGuid = "{B0164530-01BB-4496-837E-CC24C99694F6}";

        internal const string CrystallineDustDisplayName = "Aphorite.CrystallineDust.Name";
        private static readonly string CrystallineDustDescription = "Aphorite.CrystallineDust.Description";

        internal const string SpellLikeAbilityDisplayName = "Aphorite.SpellLikeAbility.Name";
        private static readonly string SpellLikeAbilityDescription = "Aphorite.SpellLikeAbility.Description";

        internal const string AphoriteResistancesDisplayName = "Aphorite.AphoriteResistances.Name";
        private static readonly string AphoriteResistancesDescription = "Aphorite.AphoriteResistances.Description";

        internal const string AphoriteSkilledDisplayName = "Aphorite.Skilled.Name";
        private static readonly string AphoriteSkilledDescription = "Aphorite.Skilled.Description";

        internal static void Configure()
        {
            //var ContactEEL = new EquipmentEntityLink() { AssetId = "a47bac4deb099fc4b86a2e01bb425cc5" };

            // Crystalline Dust

            var CrystallineDustBuff = BuffConfigurator.New("CrystallineDustBuff", "{26E5E3AC-AE74-45A1-B94E-426D5D23FB33}")
                .SetDisplayName(CrystallineDustDisplayName)
                .SetDescription(CrystallineDustDescription)
                .SetIcon(AbilityRefs.Glitterdust.Reference.Get().Icon)
                .AddSetAttackerMissChance(type: SetAttackerMissChance.Type.All, value: 20)
                .SetFxOnStart("3cf209e5299921349a1c159f35cfa369")
                .Configure();


            var CrystallineDustResource = AbilityResourceConfigurator.New("CrystallineDustResource", "{D051544C-DEEC-4371-84F9-2668BF409398}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0).IncreaseByLevelStartPlusDivStep([], 1.0f, 1, 1, 1, 1)
                )
                .Configure();
            
            var CrystallineDustActivatableAblity = ActivatableAbilityConfigurator.New("CrystallineDustActivatableAblity", "{985A9681-2C39-4488-902A-3818E9B19B06}")
                .SetDisplayName(CrystallineDustDisplayName)
                .SetDescription(CrystallineDustDescription)
                .SetIcon(AbilityRefs.Glitterdust.Reference.Get().Icon)
                .AddActivatableAbilityResourceLogic(requiredResource: CrystallineDustResource, spendType: ActivatableAbilityResourceLogic.ResourceSpendType.NewRound)
                .SetBuff(CrystallineDustBuff)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfOwnerDisabled(true)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Move)
                .Configure();
            
            var CrystallineDust = FeatureConfigurator.New("AphoriteCrystallineDust", "{99476FA9-D1EC-4003-B7DE-4737BDE9B3C6}")
                .SetDisplayName(CrystallineDustDisplayName)
                .SetDescription(CrystallineDustDescription)
                .SetIcon(AbilityRefs.Glitterdust.Reference.Get().Icon)
                .SetGroups(FeatureGroup.Racial)
                .AddFacts([CrystallineDustActivatableAblity])
                .AddAbilityResources(resource: CrystallineDustResource, restoreAmount: true)
                .Configure();

            // Aphorite Resistances

            var AphoriteResistances = FeatureConfigurator.New("AphoriteResistances", "{4D3B098D-D104-4052-A5BB-66C9ADBE9C63}")
                .SetDisplayName(AphoriteResistancesDisplayName)
                .SetDescription(AphoriteResistancesDescription)
                .SetIcon(FeatureRefs.CelestialResistance.Reference.Get().Icon)
                .AddImmunityToEnergyDrain()
                .AddDamageResistanceEnergy(type: DamageEnergyType.Electricity, value: 5)
                .AddSavingThrowBonusAgainstDescriptor(2, spellDescriptor: SpellDescriptor.Poison)
                .AddSavingThrowBonusAgainstDescriptor(2, spellDescriptor: SpellDescriptor.MindAffecting)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            // Spell-like Ability

            var SpellLikeResource = AbilityResourceConfigurator.New("AphoriteSpellLikeResource", "{DDD91A07-ADB9-47E9-A980-F69A4760B0CE}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var ProtectionFromChaos = AbilityConfigurator.New("AphoriteProtectionFromChaos", "{7CBD6D9A-B364-4FD2-AC77-F700746AB834}")
                .CopyFrom(AbilityRefs.ProtectionFromChaos, c => c is not (SpellListComponent or CraftInfoComponent or ContextRankConfig))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel())
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 1)
                .Configure();

            var SpellLikeAbility = FeatureConfigurator.New("AphoriteSpellLikeFeature", "{17890546-2E91-4EA3-B04A-360313067F1B}")
                .SetDisplayName(SpellLikeAbilityDisplayName)
                .SetDescription(SpellLikeAbilityDescription)
                .SetIcon(AbilityRefs.ProtectionFromChaos.Reference.Get().Icon)
                .AddFacts([ProtectionFromChaos])
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: ProtectionFromChaos)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            // Skilled

            var Skilled = FeatureConfigurator.New("AphoriteSkilled", "{D11B1BA0-BA08-47EB-8E2C-0CEE7D1D00AD}")
                .SetDisplayName(AphoriteSkilledDisplayName)
                .SetDescription(AphoriteSkilledDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HumanSkilled.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillKnowledgeArcana, value: 2)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var race =
            RaceConfigurator.New(AphoriteName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(AphoriteDisplayName)
                .SetDescription(AphoriteDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(FeatureRefs.KeenSenses.ToString(), Skilled, SpellLikeAbility, AphoriteResistances, CrystallineDust)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: -2)
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.AasimarRace.ToString()), CustomHeadRamps: CustomHeadRamps, CustomHairRamps: CustomHairRamps, CustomEyeRamps: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.GnomeRace.ToString()).FemaleOptions.m_Heads[0].Load(true, false).SecondaryColorsProfile.Ramps, CustomFemaleHairs: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.AasimarRace.ToString()).FemaleOptions.m_Hair, CustomMaleHairs: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.AasimarRace.ToString()).MaleOptions.m_Hair /*eyeEE: ContactEEL*/);

            // Add race to mount fixes
            RaceMountFixerizer.AddRaceToMountFixes(recoloredrace, CopyRace);

            // Fix Odds and Ends
            RaceOddsAndEndsFixerizer.FixRace(recoloredrace);

            // Add race to race list
            var raceRef = recoloredrace.ToReference<BlueprintRaceReference>();
            ref var races = ref BlueprintRoot.Instance.Progression.m_CharacterRaces;

            var length = races.Length;
            Array.Resize(ref races, length + 1);
            races[length] = raceRef;

            // Add portraits
            PortraitCreatonator.RegisterRacePortrait("Aphorite_F_01", "{452AE5B7-9374-4444-8B21-5DAB550A6CF7}", race, Gender.Female, "Aphorite_F_01");
            PortraitCreatonator.RegisterRacePortrait("Aphorite_M_01", "{460B67DF-FBE5-4496-A26B-E373EA279F47}", race, Gender.Male, "Aphorite_M_01");
        }
    }
}
