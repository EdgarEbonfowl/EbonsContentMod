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

namespace EbonsContentMod.Races
{
    internal class Shabti
    {
        public static List<Color> RaceHeadColors =
        [
            new Color( // Gold
                RaceRecolorizer.GetColorsFromRGB(120f),
                RaceRecolorizer.GetColorsFromRGB(100f),
                RaceRecolorizer.GetColorsFromRGB(50f)
                ),
            new Color( // Orange Gold
                RaceRecolorizer.GetColorsFromRGB(140f),
                RaceRecolorizer.GetColorsFromRGB(100f),
                RaceRecolorizer.GetColorsFromRGB(50f)
                ),
            new Color( // Deep Gold
                RaceRecolorizer.GetColorsFromRGB(110f),
                RaceRecolorizer.GetColorsFromRGB(90f),
                RaceRecolorizer.GetColorsFromRGB(0f)
                ),
            new Color( // Golden Tan
                RaceRecolorizer.GetColorsFromRGB(135f),
                RaceRecolorizer.GetColorsFromRGB(105f),
                RaceRecolorizer.GetColorsFromRGB(65f)
                ),
            new Color( // Deep Golden Bronze
                RaceRecolorizer.GetColorsFromRGB(110f),
                RaceRecolorizer.GetColorsFromRGB(75f),
                RaceRecolorizer.GetColorsFromRGB(0f)
                ),
            new Color( // Deep Earth
                RaceRecolorizer.GetColorsFromRGB(76f),
                RaceRecolorizer.GetColorsFromRGB(59f),
                RaceRecolorizer.GetColorsFromRGB(37f)
                ),
            new Color( // Sand
                RaceRecolorizer.GetColorsFromRGB(111f),
                RaceRecolorizer.GetColorsFromRGB(89f),
                RaceRecolorizer.GetColorsFromRGB(62f)
                ),
            new Color( // Deep Bronze
                RaceRecolorizer.GetColorsFromRGB(116f),
                RaceRecolorizer.GetColorsFromRGB(86f),
                RaceRecolorizer.GetColorsFromRGB(50f)
                ),
            new Color( // Gold
                RaceRecolorizer.GetColorsFromRGB(161f),
                RaceRecolorizer.GetColorsFromRGB(139f),
                RaceRecolorizer.GetColorsFromRGB(96f)
                ),
            new Color( // Silver
                RaceRecolorizer.GetColorsFromRGB(136f),
                RaceRecolorizer.GetColorsFromRGB(132f),
                RaceRecolorizer.GetColorsFromRGB(132f)
                )
        ];

        public static List<Color> RaceEyeColors =
        [

        ];

        public static List<Color> RaceHairColors =
        [

        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string ShabtiName = "ShabtiRace";

        internal const string ShabtiDisplayName = "Shabti.Name";
        private static readonly string ShabtiDescription = "Shabti.Description";
        public static readonly string RaceGuid = "{6D12B8E7-4AAC-4843-AEB2-9C0EDB37E991}";

        internal const string ImmortalDisplayName = "Shabti.Immortal.Name";
        private static readonly string ImmortalDescription = "Shabti.Immortal.Description";

        internal const string SpellLikeAbilityDisplayName = "Shabti.SpellLikeAbility.Name";
        private static readonly string SpellLikeAbilityDescription = "Shabti.SpellLikeAbility.Description";

        internal const string ResistLevelDrainDisplayName = "Shabti.ResistLevelDrain.Name";
        private static readonly string ResistLevelDrainDescription = "Shabti.ResistLevelDrain.Description";

        internal const string PastLifeKnowledgeDisplayName = "Shabti.PastLifeKnowledge.Name";
        private static readonly string PastLifeKnowledgeDescription = "Shabti.PastLifeKnowledge.Description";

        internal static void Configure()
        {
            var ContactEEL = new EquipmentEntityLink() { AssetId = "a47bac4deb099fc4b86a2e01bb425cc5" };

            var Immortal = FeatureConfigurator.New("ShabtiImmortal", "{EA574DC3-67C1-4035-AC12-294A10DDC59E}")
                .SetDisplayName(ImmortalDisplayName)
                .SetDescription(ImmortalDescription)
                .SetIcon(FeatureSelectionRefs.MonkKiPowerSelection.Reference.Get().Icon)
                .AddComponent<AddAgeNegate>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.MiddleAge;
                    c.Type = UnitPartAgeTTT.NegateType.Physical;
                })
                .AddComponent<AddAgeNegate>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.OldAge;
                    c.Type = UnitPartAgeTTT.NegateType.Physical;
                })
                .AddComponent<AddAgeNegate>(c => {
                    c.Age = UnitPartAgeTTT.AgeLevel.Venerable;
                    c.Type = UnitPartAgeTTT.NegateType.Physical;
                })
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var ResistLevelDrain = FeatureConfigurator.New("ShabtiResistLevelDrain", "{7008F988-5BDA-4ED5-9FC8-E1A003AB4974}")
                .SetDisplayName(ResistLevelDrainDisplayName)
                .SetDescription(ResistLevelDrainDescription)
                .SetIcon(AbilityRefs.DeathWard.Reference.Get().Icon)
                .AddImmunityToEnergyDrain()
                .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.NegativeLevel)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var PastLifeKnowledge = FeatureConfigurator.New("ShabtiPastLifeKnowledge", "{DF99A87F-A44D-4E24-83FF-B9D9E0B6F98C}")
                .SetDisplayName(PastLifeKnowledgeDisplayName)
                .SetDescription(PastLifeKnowledgeDescription)
                .SetIcon(AbilityRefs.Guidance.Reference.Get().Icon)
                .AddClassSkill(StatType.SkillKnowledgeArcana)
                .AddClassSkill(StatType.SkillKnowledgeWorld)
                .AddClassSkill(StatType.SkillLoreNature)
                .AddClassSkill(StatType.SkillLoreReligion)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var SpellLikeResource = AbilityResourceConfigurator.New("ShabtiSpellLikeResource", "{3FBA18E1-10BA-4942-97E4-BE4F9D31AAB8}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var Castigate = AbilityConfigurator.New("ShabtiCastigate", "{578AFC1A-F345-48C2-AA1F-701299D2E13D}")
                .CopyFrom(AbilityRefs.Castigate, c => c is not (SpellListComponent or CraftInfoComponent or ContextRankConfig))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel())
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 2)
                .Configure();

            var SpellLikeAbility = FeatureConfigurator.New("ShabtiSpellLikeFeature", "{4C44BA22-2928-4C16-BDA1-ADAAAE6D3C9E}")
                .SetDisplayName(SpellLikeAbilityDisplayName)
                .SetDescription(SpellLikeAbilityDescription)
                .SetIcon(AbilityRefs.Castigate.Reference.Get().Icon)
                .AddFacts([Castigate])
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: Castigate)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            var race =
            RaceConfigurator.New(ShabtiName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(ShabtiDisplayName)
                .SetDescription(ShabtiDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(Immortal, ResistLevelDrain, PastLifeKnowledge, SpellLikeAbility, FeatureRefs.KeenSenses.ToString())
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2)
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, BaldRace: true, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.KitsuneRace.ToString()), CustomHairRamps: CopyRace.FemaleOptions.m_Hair[1].Load(true, false).PrimaryColorsProfile.Ramps, CustomEyeRamps: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.GnomeRace.ToString()).FemaleOptions.m_Heads[0].Load(true, false).SecondaryColorsProfile.Ramps /*eyeEE: ContactEEL*/);

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
