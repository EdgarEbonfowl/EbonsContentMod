using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.References;
using Kingmaker.Designers.Mechanics.Facts;
using HarmonyLib;
using Kingmaker;
using Kingmaker.Assets.UnitLogic.Mechanics.Properties;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Controllers.Units;
using Kingmaker.Designers;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Items;
using Kingmaker.Localization;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI.MVVM._VM.ActionBar;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Alignments;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Conditions;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;
using UnityEngine;
using System.IO;
using static KRXLib.BlueprintRepository.Owlcat.CharacterClassRefs;
using static KRXLib.BlueprintRepository.Owlcat.FeatureReplaceSpellbookRefs;
using static KRXLib.BlueprintRepository.Owlcat.FeatureSelectionRefs;
using static KRXLib.BlueprintRepository.Owlcat.SpellListRefs;
using TabletopTweaks.Core.Utilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Spells;
using static Kingmaker.Kingdom.Settlements.SettlementGridTopology;
using EbonsContentMod.Utilities;
using BlueprintCore.Blueprints.Configurators.Classes.Selection;
using static TabletopTweaks.Core.Utilities.SpellTools;
using System.Xml.Linq;

namespace EbonsContentMod.Archetypes
{
    internal class CollegiateInitiate
    {
        private const string ArchetypeName = "CollegiateInitiate";

        internal const string ArchetypeDisplayName = "CollegiateInitiate.Name";
        private const string ArchetypeDescription = "CollegiateInitiate.Description";
        public static readonly string ArchetypeGuid = "{47F16522-200E-4827-A350-39820AAE2A8D}";

        private const string HalcyonSpellLore = "CollegiateInitiate.HalcyonSpellLore";
        internal const string HalcyonSpellLoreName = "CollegiateInitiate.HalcyonSpellLore.Name";
        private const string HalcyonSpellLoreDescription = "CollegiateInitiate.HalcyonSpellLore.Description";
        public static readonly string HalcyonSpellLoreGuid = "{16FBF3CB-72E2-4BB5-A5AB-65F22015DEC2}";

        private const string HalcyonSpellLoreList = "CollegiateInitiate.HalcyonSpellLoreList";
        public static readonly string HalcyonSpellLoreListGuid = "{4CCB33A3-5581-4F93-B7F5-6304AC179F7B}";

        internal static string HalcyonListFilePath = Main.ModEntry.Path + "ToolOutput\\HalcyonSpellIdentifiers";
        internal static string HalcyonListVariantsFilePath = Main.ModEntry.Path + "ToolOutput\\HalcyonSpellVariantIdentifiers";

        

        internal static bool HandleDarkCodex(BlueprintAbility spell)
        {
            var limitless = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("02a9eec3a3a043478c7ba9284fc08be6");
            var logic = spell.GetComponent<AbilityResourceLogic>();

            int total = logic.Amount + logic.ResourceCostIncreasingFacts.Count;
            for (int n = 0; n < total; n++)
                logic.ResourceCostDecreasingFacts.Add(limitless);

            return true;
        }
        
        internal static BlueprintAbilityReference AddReservoirCost(BlueprintAbilityReference spell, int i)
        {
            BlueprintAbility NewSpell = null;
            
            var spellability = BlueprintTools.GetBlueprint<BlueprintAbility>(spell.ToString());           

            var VariantComponent = spellability.GetComponent<AbilityVariants>();
            BlueprintAbilityReference[] VariantRefs = [];
            AbilityVariants NewVariants = new();

            if (VariantComponent != null)
            {
                var Variants = VariantComponent.m_Variants.ToList();
                var name = "Halcyon" + spellability.name + "Base" + i.ToString();
                var guid = HalcyonSpellFunctions.GetHalcyonSpellGuid(spell);
                var school = spellability.School;

                if (guid == "Error")
                {
                    var newGuid = BlueprintGuid.NewGuid();
                    string newline = "\"" + spell.ToString() + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                    File.WriteAllLines(HalcyonListFilePath + "\\" + name + ".txt", [newline]);
                    return null;
                }

                NewSpell = AbilityConfigurator.NewSpell(name, guid, school, false)
                    .CopyFrom(spellability, c => c is not (AbilityVariants or SpellListComponent))
                    .Configure();

                foreach (BlueprintAbilityReference variant in Variants)
                {                   
                    var variantability = BlueprintTools.GetBlueprint<BlueprintAbility>(variant.ToString());
                    var variantguid = HalcyonSpellFunctions.GetHalcyonSpellVariantGuid(variant);
                    var variantname = "Halcyon" + variantability.name + "Variant" + i.ToString();
                    var variantschool = variantability.School;

                    if (variantguid == "Error")
                    {
                        var newGuid = BlueprintGuid.NewGuid();
                        string newline = "\"" + variant.ToString() + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                        File.WriteAllLines(HalcyonListVariantsFilePath + "\\" + variantname + ".txt", [newline]);
                        continue;
                    }

                    var NewSpellVariant = AbilityConfigurator.NewSpell(variantname, variantguid, variantschool, false)
                        .CopyFrom(variantability, c => c is not (AbilityVariants or SpellListComponent))
                        .AddAbilityResourceLogic(Math.Max((i + 1)/ 2, 1), isSpendResource: true, requiredResource: AbilityResourceRefs.ArcanistArcaneReservoirResource.ToString())
                        .Configure();

                    bool handled = false;
                    
                    // Handle Dark Codex stuff
                    if (CheckerUtilities.GetModActive("DarkCodex")) handled = HandleDarkCodex(NewSpellVariant);

                    if (NewSpellVariant != null)
                    {
                        NewVariants.m_Variants = NewVariants.m_Variants.AppendToArray(NewSpellVariant.ToReference<BlueprintAbilityReference>());
                        VariantRefs = VariantRefs.AppendToArray(NewSpellVariant.ToReference<BlueprintAbilityReference>());
                    }

                }

                // Remove old variants and add new ones
                var ReturnSpell = AbilityConfigurator.For(NewSpell.ToString())
                    .SetAbilityVariants(NewVariants)
                    .AddComponent<AbilityVariants>(c =>
                    {
                        c.m_Variants = VariantRefs;
                    })
                    .Configure()
                    .ToReference<BlueprintAbilityReference>();

                return ReturnSpell;
            }
            else
            {
                var name = "Halcyon" + spellability.name + i.ToString();
                var guid = HalcyonSpellFunctions.GetHalcyonSpellGuid(spell);
                var school = spellability.School;

                if (guid == "Error")
                {
                    var newGuid = BlueprintGuid.NewGuid();
                    string newline = "\"" + spell.ToString() + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                    File.WriteAllLines(HalcyonListFilePath + "\\" + name + ".txt", [newline]);
                    return null;
                }

                NewSpell = AbilityConfigurator.NewSpell(name, guid, school, false)
                    .CopyFrom(spellability, c => c is not (AbilityVariants or SpellListComponent))
                    .AddAbilityResourceLogic(Math.Max((i + 1) / 2, 1), isSpendResource: true, requiredResource: AbilityResourceRefs.ArcanistArcaneReservoirResource.ToString())
                    .Configure();

                bool handled = false;

                // Handle Dark Codex stuff
                if (CheckerUtilities.GetModActive("DarkCodex")) handled = HandleDarkCodex(NewSpell);

                return NewSpell.ToReference<BlueprintAbilityReference>();
            }
        }

        internal static BlueprintSpellList BuildHalcyonSpellLoreList()
        {
            List<SpellLevelList> levelLists = [];
            List<BlueprintAbilityReference> ArcanistSpells = [];

            // Make the list of all Arcanist spells
            for (int n = 0; n < 11; n++)
            {
                var WizardSpellLevelList = WizardSpellList.Get().SpellsByLevel[n];
                foreach (BlueprintAbilityReference spell in WizardSpellLevelList.m_Spells.ToList())
                {
                    ArcanistSpells.Add(spell);
                }
            }

            for (int i = 0; i < 11; i++)
            {
                SpellLevelList HalcyonSpells = DruidSpellList.Get().SpellsByLevel[i];

                IEnumerable<BlueprintAbilityReference> cSpells = ClericSpellList.Get().SpellsByLevel[i].m_Spells
                    .Where(s => s.Get().SpellDescriptor.HasFlag(SpellDescriptor.Good));
                HalcyonSpells.m_Spells.AddRange(cSpells);
                HalcyonSpells.m_Spells = HalcyonSpells.m_Spells.Distinct().ToList();

                // Remove Arcanist spells - probably quicker to just check the spellcomponent list
                List<BlueprintAbilityReference> TrimmedHalcyonSpells = [];

                bool skip = false;

                foreach (BlueprintAbilityReference halcyonspell in HalcyonSpells.m_Spells.ToList())
                {
                    foreach (BlueprintAbilityReference wizardspell in ArcanistSpells)
                    {
                        if (wizardspell.ToString() == halcyonspell.ToString() || BlueprintTools.GetBlueprint<BlueprintAbility>(wizardspell.ToString()).m_DisplayName == BlueprintTools.GetBlueprint<BlueprintAbility>(halcyonspell.ToString()).m_DisplayName)
                        {
                            skip = true;
                            break;
                        }
                    }

                    if (skip == false) TrimmedHalcyonSpells.Add(halcyonspell);
                    else skip = false;
                }

                HalcyonSpells.m_Spells = TrimmedHalcyonSpells.Distinct().ToList();

                List<BlueprintAbilityReference> ModifiedSpells = [];

                // Add resource logic to spells
                foreach (BlueprintAbilityReference spell in HalcyonSpells.m_Spells.ToList())
                {
                    var NewSpell = AddReservoirCost(spell, i);
                    if (NewSpell != null) ModifiedSpells.Add(NewSpell);
                }

                HalcyonSpells.m_Spells = ModifiedSpells.Distinct().ToList();

                levelLists.Add(HalcyonSpells);
            }

            Dictionary<int, SpellLevelList> finalList = [];
            for (int i = 0; i < 11; i++)
                finalList.Add(i, new(i) { m_Spells = [] });

            foreach (SpellLevelList list in levelLists)
            {
                finalList[list.SpellLevel].m_Spells.AddRange(list.m_Spells);
                finalList[list.SpellLevel].m_Spells = finalList[list.SpellLevel].m_Spells.Distinct().ToList();
            }

            var result = SpellListConfigurator.New(HalcyonSpellLoreList, HalcyonSpellLoreListGuid)
                .SetIsMythic(false)
                .SetMaxLevel(10)
                .SetSpellsByLevel(finalList.Values.ToArray())
                .Configure();

            return result;
        }

        internal static BlueprintParametrizedFeature CreateHalcyonSpellLore(BlueprintSpellList list)
        {
            BlueprintCharacterClassReference SpellcasterClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>(CharacterClassRefs.ArcanistClass.ToString());
            
            return ParametrizedFeatureConfigurator.New(HalcyonSpellLore, HalcyonSpellLoreGuid)
                .SetDisplayName(HalcyonSpellLoreName)
                .SetDescription(HalcyonSpellLoreDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.AuraOfFaithFeature.ToString()).m_Icon)
                .SetIsClassFeature()
                .SetSpellcasterClass(SpellcasterClass)
                .SetSpellList(list)
                .SetHideNotAvailibleInUI()
                .SetRanks(20)
                .SetParameterType(FeatureParameterType.LearnSpell)
                .SetSpecificSpellLevel(false)
                .SetSpellLevelPenalty(0)
                .AddComponent<LearnSpellParametrized>(c =>
                {
                    c.m_SpellcasterClass = SpellcasterClass;
                    c.m_SpellList = new() { deserializedGuid = list.AssetGuid };
                    c.SpecificSpellLevel = false;
                    c.SpellLevelPenalty = 0;
                    c.SpellLevel = 0;
                })
                .Configure();
        }
        
        internal static void Configure()
        {
            Directory.CreateDirectory(HalcyonListFilePath);
            Directory.CreateDirectory(HalcyonListVariantsFilePath);

            var HalcyonSpellList = BuildHalcyonSpellLoreList();
            var HalcyonSpellLoreSelection = CreateHalcyonSpellLore(HalcyonSpellList);
            
            ArchetypeConfigurator.New(ArchetypeName, ArchetypeGuid, CharacterClassRefs.ArcanistClass)
                .SetLocalizedName(ArchetypeDisplayName)
                .SetLocalizedDescription(ArchetypeDescription)
                .AddPrerequisiteAlignment(AlignmentMaskType.Good)

                // Remove Features
                .AddToRemoveFeatures(5, FeatureSelectionRefs.ArcanistExploitSelection.ToString())
                .AddToRemoveFeatures(9, FeatureSelectionRefs.ArcanistExploitSelection.ToString())
                .AddToRemoveFeatures(17, FeatureSelectionRefs.ArcanistExploitSelection.ToString())

                // Add Features
                //.AddToAddFeatures(1, HalcyonSpellLoreSelection)
                .AddToAddFeatures(2, HalcyonSpellLoreSelection)
                .AddToAddFeatures(3, HalcyonSpellLoreSelection)
                .AddToAddFeatures(4, HalcyonSpellLoreSelection)
                .AddToAddFeatures(5, HalcyonSpellLoreSelection)
                .AddToAddFeatures(6, HalcyonSpellLoreSelection)
                .AddToAddFeatures(7, HalcyonSpellLoreSelection)
                .AddToAddFeatures(8, HalcyonSpellLoreSelection)
                .AddToAddFeatures(9, HalcyonSpellLoreSelection)
                .AddToAddFeatures(10, HalcyonSpellLoreSelection)
                .AddToAddFeatures(11, HalcyonSpellLoreSelection)
                .AddToAddFeatures(12, HalcyonSpellLoreSelection)
                .AddToAddFeatures(13, HalcyonSpellLoreSelection)
                .AddToAddFeatures(14, HalcyonSpellLoreSelection)
                .AddToAddFeatures(15, HalcyonSpellLoreSelection)
                .AddToAddFeatures(16, HalcyonSpellLoreSelection)
                .AddToAddFeatures(17, HalcyonSpellLoreSelection)
                .AddToAddFeatures(18, HalcyonSpellLoreSelection)
                .AddToAddFeatures(19, HalcyonSpellLoreSelection)
                .AddToAddFeatures(20, HalcyonSpellLoreSelection)

                // Make it so!
                .Configure();
        }
    }
}
