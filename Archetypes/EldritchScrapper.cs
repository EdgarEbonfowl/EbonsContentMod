using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UI.MVVM._VM.CharGen.Phases.Class;
using Kingmaker.UI.MVVM._VM.Other.NestedSelectionGroup;
using Kingmaker.UI.MVVM._VM.Tooltip.Templates;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic.Class.LevelUp.Actions;
using Kingmaker.Utility;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.Loot;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.Blueprints.Root.Strings.GameLog;
using Kingmaker.Craft;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using Kingmaker.Designers.Mechanics.EquipmentEnchants;
using Kingmaker.Designers.Mechanics.Prerequisites;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Kingdom.Blueprints;
using Kingmaker.Localization;
using Kingmaker.Localization.Shared;
using Kingmaker.PubSubSystem;
using Kingmaker.ResourceLinks;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI;
using Kingmaker.UI.Log;
using Kingmaker.UI.Models.Log.CombatLog_ThreadSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.AreaEffects;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Abilities.Components.CasterCheckers;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.View.Equipment;
using Kingmaker.Assets.UnitLogic.Mechanics.Properties;
using Kingmaker.Blueprints.Items.Equipment;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using UnityModManagerNet;
using EbonsContentMod.Utilities;

namespace EbonsContentMod.Archetypes
{
    internal class EldritchScrapper
    {
        private const string ArchetypeName = "EldritchScrapper";

        internal const string ArchetypeDisplayName = "EldritchScrapper.Name";
        private const string ArchetypeDescription = "EldritchScrapper.Description";
        public static readonly string ArchetypeGuid = "{6C4429EF-3110-4FC9-A0AB-5C353E0C38B4}";

        private const string EldritchScrapperBonusFeats = "EldritchScrapper.EldritchScrapperBonusFeats";
        internal const string EldritchScrapperBonusFeatsName = "EldritchScrapper.EldritchScrapperBonusFeats.Name";
        private const string EldritchScrapperBonusFeatsDescription = "EldritchScrapper.EldritchScrapperBonusFeats.Description";
        public static readonly string EldritchScrapperBonusFeatsGuid = "{021E3574-126E-4845-A17A-BC5F0054ED10}";

        internal static void EldritchScrapperPatchBloodlines()
        {
            HashSet<LevelEntry> entries = new();
            foreach (BlueprintFeature bloodlineFeature in BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(FeatureSelectionRefs.SorcererBloodlineSelection.ToString()).AllFeatures)
            {
                if (bloodlineFeature is not BlueprintProgression bloodline)
                    continue;

                entries.Add(bloodline.GetLevelEntry(1));
                entries.Add(bloodline.GetLevelEntry(9));
                entries.Add(bloodline.GetLevelEntry(15));
            }

            foreach (LevelEntry entry in entries)
            {
                if (entry.Level == 1)
                {
                    // Powers at level 1 should be the only feature that adds a resource
                    entry.m_FeaturesList
                        .Where(f => f.GetComponent<AddAbilityResources>() != null)
                        .ForEach(f => f.AddComponent<PrerequisiteNoArchetype>(c =>
                        {
                            c.CheckInProgression = true;
                            c.Group = Prerequisite.GroupType.All;
                            c.m_CharacterClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>(CharacterClassRefs.SorcererClass.ToString());
                            c.m_Archetype = BlueprintTools.GetBlueprintReference<BlueprintArchetypeReference>("6C4429EF-3110-4FC9-A0AB-5C353E0C38B4");
                        }));

                    continue;
                }

                // Remaining Features just need to be differentiated from the added spells
                entry.m_FeaturesList
                    .Where(f => f.GetComponent<AddKnownSpell>() == null)
                    .ForEach(f => f.AddComponent<PrerequisiteNoArchetype>(c =>
                    {
                        c.CheckInProgression = true;
                        c.Group = Prerequisite.GroupType.All;
                        c.m_CharacterClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>(CharacterClassRefs.SorcererClass.ToString());
                        c.m_Archetype = BlueprintTools.GetBlueprintReference<BlueprintArchetypeReference>("6C4429EF-3110-4FC9-A0AB-5C353E0C38B4");
                    }));
            }
        }
        
        internal static void Configure()
        {
            var BonusFeatSelection = FeatureSelectionConfigurator.New(EldritchScrapperBonusFeats, EldritchScrapperBonusFeatsGuid)
                .CopyFrom(FeatureSelectionRefs.WarpriestFeatSelection)
                .SetDisplayName(EldritchScrapperBonusFeatsName)
                .SetDescription(EldritchScrapperBonusFeatsDescription)
                .AddToAllFeatures(FeatureRefs.ArcaneStrikeFeature.ToString())
                .AddToAllFeatures(FeatureRefs.CombatCasting.ToString())
                .Configure();

            // Start Archetype Here
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.SorcererClass)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .AddPrerequisiteNoArchetype(BlueprintTools.GetBlueprintReference<BlueprintArchetypeReference>(ArchetypeRefs.SeekerArchetype_0.ToString()), BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>(CharacterClassRefs.SorcererClass.ToString()))

                // Remove features

                // Add features
                .AddToAddFeatures(1, BonusFeatSelection)
                .AddToAddFeatures(9, BonusFeatSelection)
                .AddToAddFeatures(15, BonusFeatSelection)

                // Make it so!
                .Configure();

            EldritchScrapperPatchBloodlines();

            if (CheckerUtilities.GetModActive("MysticalMayhem"))
            {
                ArchetypeConfigurator.For(ArchetypeGuid)
                    .AddPrerequisiteNoArchetype(BlueprintTools.GetBlueprintReference<BlueprintArchetypeReference>("ae5ebed8-a527-48a6-adef-404a0e14151e"), BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>(CharacterClassRefs.SorcererClass.ToString()))
                    .Configure();
            }
        }
    }
}
