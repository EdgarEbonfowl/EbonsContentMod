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
using BlueprintCore.Blueprints.References;
using EbonsContentMod.Utilities;
using System.IO;
using TabletopTweaks.Core.Utilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.Configurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Spells;
using static TabletopTweaks.Core.Utilities.FeatTools;
using System.Xml.Linq;
using UnityModManagerNet;
using static UnityModManagerNet.UnityModManager;

namespace EbonsContentMod.Feats
{
    internal class FaithMagic
    {
        private const string FaithMagicName = "ArcanistFaithMagic";
        internal const string FaithMagicDisplayName = "FaithMagic.Name";
        private const string FaithMagicDescription = "FaithMagic.Description";
        public static readonly string FaithMagicGuid = "{6B894365-217F-4704-9765-067A303ED5A6}";

        internal static Sprite FaithMagicIcon = BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.AuraOfFaithFeature.ToString()).m_Icon;
        internal static BlueprintCharacterClassReference SpellcasterClass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>(CharacterClassRefs.ArcanistClass.ToString());
        internal static string deityFeatureFilePath = Main.ModEntry.Path + "ToolOutput\\DeityFeatures";
        internal static string spellListFilePath = Main.ModEntry.Path + "ToolOutput\\SpellLists";

        internal static void Configure()
        {
            if (!CheckerUtilities.GetModActive("TabletopTweaks-Base")) return;

            Directory.CreateDirectory(deityFeatureFilePath);
            Directory.CreateDirectory(spellListFilePath);

            var faithMagic = FeatureSelectionConfigurator.New(FaithMagicName, FaithMagicGuid)
                .SetDisplayName(FaithMagicDisplayName)
                .SetDescription(FaithMagicDescription)
                .SetIcon(FaithMagicIcon) // icon used in Mystical Mayhem is: "b356247bc3fe82046af11e0eca266884"
                .AddPrerequisiteClassLevel(BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>(CharacterClassRefs.ArcanistClass.ToString()), 5)
                .AddPrerequisiteNoFeature(BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.AtheismFeature.ToString()))
                .SetIsClassFeature()
                .SetIgnorePrerequisites(false)
                .SetObligatory(false)
                //.SetRanks(20)
                .Configure();

            FeatureSelectionConfigurator.For("45b72087ac2d4554aec4f44be852eddd") // Arcane Discovery feature selection
                .AddToAllFeatures(faithMagic)
                .Configure();

            BlueprintFeatureSelection deitySelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(FeatureSelectionRefs.DeitySelection.ToString());
            
            foreach (BlueprintFeatureReference deity in deitySelection.m_AllFeatures)
            {
                var deityguid = deity.ToString();
                var selection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(deityguid);

                if (selection == null)
                {
                    CreateDeityFeature(deity, deityguid);
                }
                else
                {
                    foreach (BlueprintFeatureReference feature in selection.m_AllFeatures)
                    {
                        var featureguid = feature.ToString();

                        CreateDeityFeature(feature, featureguid);
                    }
                }
            }
        }

        private static void CreateDeityFeature(BlueprintFeature deity, string fguid)
        {
            var deityName = deity.name.Replace("Feature", string.Empty);

            var guid = FaithMagicGuidGetter.GetDeityFeatureGuidFromFeatureGuid(fguid);
            if (guid == "Error")
            {
                var newGuid = BlueprintGuid.NewGuid();
                string newline = "\"" + fguid + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                File.WriteAllLines(deityFeatureFilePath + "\\" + deityName + "-" + fguid + ".txt", [newline]);
            }

            BlueprintSpellList spellList = CreateSpellList(deity, fguid);
            if (spellList == null)
                return;

            if (guid == "Error") return;

            var baseSelection = BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(FaithMagicGuid);
            var name = "FaithMagic" + deityName + "_" + fguid;

            var bp = ParametrizedFeatureConfigurator.New(name, guid)
                .SetDisplayName(FaithMagicDisplayName)
                .SetDescription(FaithMagicDescription)
                .SetIcon(FaithMagicIcon)
                .SetIsClassFeature()
                .SetSpellcasterClass(SpellcasterClass)
                .SetSpellList(spellList)
                .SetHideNotAvailibleInUI()
                .SetRanks(20)
                .SetParameterType(FeatureParameterType.LearnSpell)
                .SetSpecificSpellLevel(false)
                .SetSpellLevelPenalty(2)
                .AddComponent<LearnSpellParametrized>(c =>
                {
                    c.m_SpellcasterClass = SpellcasterClass;
                    c.m_SpellList = new() { deserializedGuid = spellList.AssetGuid };
                    c.SpecificSpellLevel = false;
                    c.SpellLevelPenalty = 1;
                })
                .AddComponent<PrerequisiteFeature>(c => c.m_Feature = new() { deserializedGuid = deity.AssetGuid })
                .Configure();

            FeatureSelectionConfigurator.For(FaithMagicGuid)
                .AddToAllFeatures(bp)
                .Configure();
        }

        private static BlueprintSpellList CreateSpellList(BlueprintFeature deity, string fguid)
        {
            var deityName = deity.name.Replace("Feature", string.Empty);

            var guid = FaithMagicGuidGetter.GetSpellListGuidFromFeatureGuid(fguid);
            if (guid == "Error")
            {
                var newGuid = BlueprintGuid.NewGuid();
                string newline = "\"" + fguid + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                File.WriteAllLines(spellListFilePath + "\\" + deityName + "-" + fguid + ".txt", [newline]);
                return null;
            }

            List<SpellLevelList> levelLists = [];

            IEnumerable<AddFacts> addFacts = deity.GetComponents<AddFacts>();
            List<BlueprintSpellList> spellLists = [];

            foreach (AddFacts addFact in addFacts)
            {
                foreach (Kingmaker.Blueprints.Facts.BlueprintUnitFact fact in addFact.Facts)
                    spellLists.Add(fact.GetComponent<AddSpecialSpellListForArchetype>()?.SpellList);
            }

            if (spellLists == null || spellLists.Empty())
                return null;

            foreach (BlueprintSpellList list in spellLists)
            {
                if (list == null)
                    continue;

                foreach (SpellLevelList sublist in list.SpellsByLevel)
                    levelLists.Add(sublist);
            }

            Dictionary<int, SpellLevelList> finalList = [];
            for (int i = 0; i < 11; i++)
                finalList.Add(i, new(i) { m_Spells = [] });

            foreach (SpellLevelList list in levelLists)
            {
                finalList[list.SpellLevel].m_Spells.AddRange(list.m_Spells);
                finalList[list.SpellLevel].m_Spells = finalList[list.SpellLevel].m_Spells.Distinct().ToList();
            }

            var spellListName = $"FaithMagic{deity.name.Replace("Feature", string.Empty)}SpellList" + "_" + fguid;

            var result = SpellListConfigurator.New(spellListName, guid)
                .SetIsMythic(false)
                .SetMaxLevel(10)
                .SetSpellsByLevel(finalList.Values.ToArray())
                .Configure();

            return result;
        }

    }
}
