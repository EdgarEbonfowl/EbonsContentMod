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
    internal class Samsaran
    {
        public static List<Color> RaceHeadColors =
        [
            new Color(0.173f, 0.455f, 0.78f),
            new Color(0.118f, 0.573f, 0.824f),
            new Color(0.0f, 0.5f, 1f),
            new Color(0.322f, 0.439f, 0.816f),
            new Color(0.301f, 0.333f, 0.863f)
        ];

        public static List<Color> RaceEyeColors =
        [
            new Color(0.75f, 0.75f, 0.75f)
        ];

        public static List<Color> RaceHairColors =
        [
            new Color(0.11764706f, 0.11764706f, 0.11764706f),
            new Color(0.0f, 0.0f, 0.0f)
        ];

        public static EquipmentEntityLink[] FemaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "04c3eb6d7570d8d49b686516b7c4a4f8"}, // Long Camelia Hair
            new EquipmentEntityLink() {AssetId = "779458079f7718c4bb960d9cef195339"}, // Long Wavy Braids
            new EquipmentEntityLink() {AssetId = "34bb68b3e4f03be44a1f0611a09530fc"}, // Crown Braids - Dwarf
            new EquipmentEntityLink() {AssetId = "1762cab3d178f53489f43ab791b87f9c"}  // Noble Braids - Dwarf
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string SamsaranName = "SamsaranRace";

        internal const string SamsaranDisplayName = "Samsaran.Name";
        private static readonly string SamsaranDescription = "Samsaran.Description";
        public static readonly string RaceGuid = "{69AD3E90-BAF7-442C-9DF9-56170C7206F0}";

        internal const string LifeBoundDisplayName = "Samsaran.LifeBound.Name";
        private static readonly string LifeBoundDescription = "Samsaran.LifeBound.Description";
        public static readonly string LifeBoundGuid = "{AE44DFA7-0CF8-4B21-A47B-ECA49D2B632A}";

        internal const string ShardsOfThePastDisplayName = "Samsaran.ShardsOfThePast.Name";
        private static readonly string ShardsOfThePastDescription = "Samsaran.ShardsOfThePast.Description";
        public static readonly string ShardsOfThePastGuid = "{C3715816-B39D-4EC6-B598-D15AD3B95866}";

        internal const string ShardsOfThePastAthleticsDisplayName = "Samsaran.ShardsOfThePastAthletics.Name";
        internal const string ShardsOfThePastMobilityDisplayName = "Samsaran.ShardsOfThePastMobility.Name";
        internal const string ShardsOfThePastStealthDisplayName = "Samsaran.ShardsOfThePastStealth.Name";
        internal const string ShardsOfThePastTrickeryDisplayName = "Samsaran.ShardsOfThePastTrickery.Name";
        internal const string ShardsOfThePastArcanaDisplayName = "Samsaran.ShardsOfThePastArcana.Name";
        internal const string ShardsOfThePastWorldDisplayName = "Samsaran.ShardsOfThePastWorld.Name";
        internal const string ShardsOfThePastNatureDisplayName = "Samsaran.ShardsOfThePastNature.Name";
        internal const string ShardsOfThePastReligionDisplayName = "Samsaran.ShardsOfThePastReligion.Name";
        internal const string ShardsOfThePastPerceptionDisplayName = "Samsaran.ShardsOfThePastPerception.Name";
        internal const string ShardsOfThePastPersuasionDisplayName = "Samsaran.ShardsOfThePastPersuasion.Name";
        internal const string ShardsOfThePastUMDDisplayName = "Samsaran.ShardsOfThePastUMD.Name";

        internal const string MysticPastLifeDisplayName = "Samsaran.MysticPastLife.Name";
        private static readonly string MysticPastLifeDescription = "Samsaran.MysticPastLife.Description";
        public static readonly string MysticPastLifeGuid = "{9E1033C2-D021-49CC-AB9C-1374078A0E92}";

        internal const string MysticPastLifeBardDisplayName = "Samsaran.MysticPastLifeBard.Name";
        internal const string MysticPastLifeBloodragerDisplayName = "Samsaran.MysticPastLifeBloodrager.Name";
        internal const string MysticPastLifeClericDisplayName = "Samsaran.MysticPastLifeCleric.Name";
        internal const string MysticPastLifeDruidDisplayName = "Samsaran.MysticPastLifeDruid.Name";
        internal const string MysticPastLifeHunterDisplayName = "Samsaran.MysticPastLifeHunter.Name";
        internal const string MysticPastLifeInquisitorDisplayName = "Samsaran.MysticPastLifeInquisitor.Name";
        internal const string MysticPastLifeMagusDisplayName = "Samsaran.MysticPastLifeMagus.Name";
        internal const string MysticPastLifePaladinDisplayName = "Samsaran.MysticPastLifePaladin.Name";
        internal const string MysticPastLifeRangerDisplayName = "Samsaran.MysticPastLifeRanger.Name";
        internal const string MysticPastLifeShamanDisplayName = "Samsaran.MysticPastLifeShaman.Name";
        internal const string MysticPastLifeWarpriestDisplayName = "Samsaran.MysticPastLifeWarpriest.Name";
        internal const string MysticPastLifeWitchDisplayName = "Samsaran.MysticPastLifeWitch.Name";
        internal const string MysticPastLifeWizardDisplayName = "Samsaran.MysticPastLifeWizard.Name";

        public static readonly string RacialHeritageGuid = "{262F7FB5-1501-4783-8B52-C35AD3CBA819}";

        internal const string SamsaranMagicDisplayName = "Samsaran.SamsaranMagic.Name";
        private static readonly string SamsaranMagicDescription = "Samsaran.SamsaranMagic.Description";
        public static readonly string SamsaranMagicGuid = "{E9417F35-6DC0-4C98-9E2F-6A7C7A5899A6}";

        internal const string SamsaranRacialHeritageDisplayName = "Samsaran.RacialHeritage.Name";
        private static readonly string SamsaranRacialHeritageDescription = "Samsaran.RacialHeritage.Description";

        internal static string MysticPastLifeFilePath = Main.ModEntry.Path + "ToolOutput\\MysticPastLifeGuids";

        private static BlueprintFeature CreateLifebound()
        {
            var feat = FeatureConfigurator.New("SamsaranLifeBound", LifeBoundGuid)
                .SetDisplayName(LifeBoundDisplayName)
                .SetDescription(LifeBoundDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(FeatureSelectionRefs.MonkKiPowerSelection.ToString()).Icon)
                .AddSavingThrowBonusAgainstDescriptor(2, modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.Death)
                .AddSavingThrowBonusAgainstDescriptor(2, modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.ChannelNegativeHarm)
                .AddSavingThrowBonusAgainstDescriptor(2, modifierDescriptor: ModifierDescriptor.Racial, spellDescriptor: SpellDescriptor.NegativeLevel)
                .Configure();

            return feat;
        }

        private static BlueprintFeature CreateShardsOfThePastFirst()
        {           
            var Athletics = FeatureConfigurator.New("ShardsOfThePastAthletics1", "{1D3B7E70-D237-4733-B4B4-E3586B778BB9}")
                .SetDisplayName(ShardsOfThePastAthleticsDisplayName)
                .SetDescription(ShardsOfThePastDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.SkillFocusPhysique.ToString()).Icon)
                .AddBuffSkillBonus(StatType.SkillAthletics, 2, ModifierDescriptor.Racial)
                .AddClassSkill(StatType.SkillAthletics)
                .Configure();

            var Mobility = FeatureConfigurator.New("ShardsOfThePastMobility1", "{2A138500-9972-44AA-BA86-DB08BA5BCA44}")
                .SetDisplayName(ShardsOfThePastMobilityDisplayName)
                .SetDescription(ShardsOfThePastDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.SkillFocusAcrobatics.ToString()).Icon)
                .AddBuffSkillBonus(StatType.SkillMobility, 2, ModifierDescriptor.Racial)
                .AddClassSkill(StatType.SkillMobility)
                .Configure();

            var Stealth = FeatureConfigurator.New("ShardsOfThePastStealth1", "{3663414F-44A8-4544-8822-00F001C61E77}")
                .SetDisplayName(ShardsOfThePastStealthDisplayName)
                .SetDescription(ShardsOfThePastDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.SkillFocusStealth.ToString()).Icon)
                .AddBuffSkillBonus(StatType.SkillStealth, 2, ModifierDescriptor.Racial)
                .AddClassSkill(StatType.SkillStealth)
                .Configure();

            var Trickery = FeatureConfigurator.New("ShardsOfThePastTrickery1", "{C665DBD7-68D4-4203-9354-E87DD47B9490}")
                .SetDisplayName(ShardsOfThePastTrickeryDisplayName)
                .SetDescription(ShardsOfThePastDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.SkillFocusThievery.ToString()).Icon)
                .AddBuffSkillBonus(StatType.SkillThievery, 2, ModifierDescriptor.Racial)
                .AddClassSkill(StatType.SkillThievery)
                .Configure();

            var Arcana = FeatureConfigurator.New("ShardsOfThePastArcana1", "{C5E15394-64C9-4931-B45E-EA0AE2C0A1B3}")
                .SetDisplayName(ShardsOfThePastArcanaDisplayName)
                .SetDescription(ShardsOfThePastDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.SkillFocusKnowledgeArcana.ToString()).Icon)
                .AddBuffSkillBonus(StatType.SkillKnowledgeArcana, 2, ModifierDescriptor.Racial)
                .AddClassSkill(StatType.SkillKnowledgeArcana)
                .Configure();

            var World = FeatureConfigurator.New("ShardsOfThePastWorld1", "{D6CB06F4-C330-41E5-9EBF-E20279D242C7}")
                .SetDisplayName(ShardsOfThePastWorldDisplayName)
                .SetDescription(ShardsOfThePastDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.SkillFocusKnowledgeWorld.ToString()).Icon)
                .AddBuffSkillBonus(StatType.SkillKnowledgeWorld, 2, ModifierDescriptor.Racial)
                .AddClassSkill(StatType.SkillKnowledgeWorld)
                .Configure();

            var Nature = FeatureConfigurator.New("ShardsOfThePastNature1", "{07EE34E6-317D-49F0-B029-3ECEC9A994FE}")
                .SetDisplayName(ShardsOfThePastNatureDisplayName)
                .SetDescription(ShardsOfThePastDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.SkillFocusLoreNature.ToString()).Icon)
                .AddBuffSkillBonus(StatType.SkillLoreNature, 2, ModifierDescriptor.Racial)
                .AddClassSkill(StatType.SkillLoreNature)
                .Configure();

            var Religion = FeatureConfigurator.New("ShardsOfThePastReligion1", "{F751C243-325F-4EBA-9F53-EDB716AD840C}")
                .SetDisplayName(ShardsOfThePastReligionDisplayName)
                .SetDescription(ShardsOfThePastDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.SkillFocusLoreReligion.ToString()).Icon)
                .AddBuffSkillBonus(StatType.SkillLoreReligion, 2, ModifierDescriptor.Racial)
                .AddClassSkill(StatType.SkillLoreReligion)
                .Configure();

            var Perception = FeatureConfigurator.New("ShardsOfThePastPerception1", "{7434B857-355A-4B77-9F7E-06ABA6E1EF81}")
                .SetDisplayName(ShardsOfThePastPerceptionDisplayName)
                .SetDescription(ShardsOfThePastDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.SkillFocusPerception.ToString()).Icon)
                .AddBuffSkillBonus(StatType.SkillPerception, 2, ModifierDescriptor.Racial)
                .AddClassSkill(StatType.SkillPerception)
                .Configure();

            var Persuasion = FeatureConfigurator.New("ShardsOfThePastPersuasion1", "{210464C8-EB14-498A-95A9-87904E23D2E0}")
                .SetDisplayName(ShardsOfThePastPersuasionDisplayName)
                .SetDescription(ShardsOfThePastDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.SkillFocusDiplomacy.ToString()).Icon)
                .AddBuffSkillBonus(StatType.SkillPersuasion, 2, ModifierDescriptor.Racial)
                .AddClassSkill(StatType.SkillPersuasion)
                .Configure();

            var UMD = FeatureConfigurator.New("ShardsOfThePastUMD1", "{60C8AFB2-7DA7-4406-9CC3-1264D6478465}")
                .SetDisplayName(ShardsOfThePastUMDDisplayName)
                .SetDescription(ShardsOfThePastDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.SkillFocusUseMagicDevice.ToString()).Icon)
                .AddBuffSkillBonus(StatType.SkillUseMagicDevice, 2, ModifierDescriptor.Racial)
                .AddClassSkill(StatType.SkillUseMagicDevice)
                .Configure();

            var feat = FeatureSelectionConfigurator.New("SamsaranShardsOfThePast1", "{484B5905-3A7C-4DD5-BF61-5112ACFADEF3}")
                .SetDisplayName(ShardsOfThePastDisplayName)
                .SetDescription(ShardsOfThePastDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(FeatureSelectionRefs.SkillFocusSelection.ToString()).Icon)
                .AddToAllFeatures(Athletics, Mobility, Stealth, Trickery, Arcana, World, Nature, Religion, Perception, Persuasion, UMD)
                .SetGroup(FeatureGroup.Feat)
                .SetObligatory(true)
                .Configure();

            return feat;
        }

        private static BlueprintFeature CreateShardsOfThePast()
        {
            var ShardsOfThePast = CreateShardsOfThePastFirst();
            
            var feat = ProgressionConfigurator.New("SamsaranShardsOfThePastProgression", ShardsOfThePastGuid)
                .SetDisplayName(ShardsOfThePastDisplayName)
                .SetDescription(ShardsOfThePastDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(FeatureSelectionRefs.SkillFocusSelection.ToString()).Icon)
                .AddPrerequisiteNoFeature(MysticPastLifeGuid)
                .SetGiveFeaturesForPreviousLevels(true)
                .SetReapplyOnLevelUp(false)
                .AddToLevelEntries(1, ShardsOfThePast, ShardsOfThePast)
                .Configure();

            return feat;
        }

        private static string GetMysticPastLifeDisplayName(BlueprintCharacterClass spellcasterclass)
        {
            var classname = spellcasterclass.name;
            
            return classname switch
            {
                "BardClass" => MysticPastLifeBardDisplayName,
                "BloodragerClass" => MysticPastLifeBloodragerDisplayName,
                "ClericClass" => MysticPastLifeClericDisplayName,
                "DruidClass" => MysticPastLifeDruidDisplayName,
                "HunterClass" => MysticPastLifeHunterDisplayName,
                "InquisitorClass" => MysticPastLifeInquisitorDisplayName,
                "MagusClass" => MysticPastLifeMagusDisplayName,
                "PaladinClass" => MysticPastLifePaladinDisplayName,
                "RangerClass" => MysticPastLifeRangerDisplayName,
                "ShamanClass" => MysticPastLifeShamanDisplayName,
                "WarpriestClass" => MysticPastLifeWarpriestDisplayName,
                "WitchClass" => MysticPastLifeWitchDisplayName,
                "WizardClass" => MysticPastLifeWizardDisplayName,
                _ => "Error"
            };
        }

        private static string GetMysticPastLifeProgressionGuid(BlueprintCharacterClass spellcasterclass)
        {
            var classname = spellcasterclass.name;

            return classname switch
            {
                "BardClass" => "{84534B87-0439-4ECB-9FDC-553C476F5739}",
                "BloodragerClass" => "{DF0A74A1-E6E6-4338-986E-7536178ECCE4}",
                "ClericClass" => "{D46B09EB-7A0B-4DCE-9045-1E8B6E00291A}",
                "DruidClass" => "{91E1518F-EA47-4888-B230-51655680195E}",
                "HunterClass" => "{5D36F1BF-4290-47AB-8D91-1F9A84A3B118}",
                "InquisitorClass" => "{55B2ACDD-8D14-4019-946F-DAF48FA1719F}",
                "MagusClass" => "{6F12B1E5-9E94-40F4-87C0-FDEE92ED4A08}",
                "PaladinClass" => "{AF1847DA-35EC-41EF-86B4-13C407DB951A}",
                "RangerClass" => "{6C342B4B-C7B4-467E-99D0-3CD84904C947}",
                "ShamanClass" => "{59B2B2C3-8AED-411E-BD98-7080009267B1}",
                "WarpriestClass" => "{177EEF7B-33A6-4330-B2B9-3BCD320D3125}",
                "WitchClass" => "{2EA0E24E-3688-424E-A292-3B642BF8312B}",
                "WizardClass" => "{A1D79C8F-5882-4233-A31F-1F5DCDCCFC8E}",
                _ => "Error"
            };
        }

        private static string GetMysticPastLifeSpellList(BlueprintCharacterClass spellcasterclass)
        {
            var classname = spellcasterclass.name;

            return classname switch
            {
                "BardClass" => "25a5013493bdcf74bb2424532214d0c8",
                "BloodragerClass" => "98c05aeff6e3d384f8aec6d584973642",
                "ClericClass" => "8443ce803d2d31347897a3d85cc32f53",
                "DruidClass" => "bad8638d40639d04fa2f80a1cac67d6b",
                "HunterClass" => "d090b791bfe381740b98ed4ff909b1cf",
                "InquisitorClass" => "57c894665b7895c499b3dce058c284b3",
                "MagusClass" => "4d72e1e7bd6bc4f4caaea7aa43a14639",
                "PaladinClass" => "9f5be2f7ea64fe04eb40878347b147bc",
                "RangerClass" => "29f3c338532390546bc5347826a655c4",
                "ShamanClass" => "c0c40e42f07ff104fa85492da464ac69",
                "WarpriestClass" => "c5a1b8df32914d74c9b44052ba3e686a",
                "WitchClass" => "e17df9977b879b64e8a8cbb4b3569f19",
                "WizardClass" => "ba0401fdeb4062f40a7aa95b6f07fe89",
                _ => "Error"
            };
        }

        private static int GetMysticPastGetLevelPenalty(BlueprintCharacterClass spellcasterclass)
        {
            var classname = spellcasterclass.name;

            return classname switch
            {
                "ArcanistClass" => -10,
                "BardClass" => -7,
                "BloodragerClass" => -5,
                "ClericClass" => -10,
                "DruidClass" => -10,
                "HunterClass" => -7,
                "InquisitorClass" => -7,
                "MagusClass" => -7,
                "OracleClass" => -10,
                "PaladinClass" => -5,
                "RangerClass" => -5,
                "ShamanClass" => -10,
                "SkaldClass" => -7,
                "SorcererClass" => -10,
                "WarpriestClass" => -7,
                "WitchClass" => -10,
                "WizardClass" => -10,
                _ => -4
            };
        }

        private static BlueprintParametrizedFeature CreateMysticPastLifeSpellSelection(BlueprintCharacterClass spellcasterclass, BlueprintCharacterClass basecasterclass, int i)
        {            
            var name = "MysticPastLife" + spellcasterclass.name + "SpellSelectFor" + basecasterclass.name + i.ToString();

            var guid = MysticPastLifeFunctions.GetMysticPastLifeSpellSelectionGuidByName(name);

            if (guid == "Error")
            {
                var newGuid = BlueprintGuid.NewGuid();
                string newline = "\"" + name + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                File.WriteAllLines(MysticPastLifeFilePath + "\\" + name + ".txt", [newline]);
                return null;
            }

            var spelllist = GetMysticPastLifeSpellList(spellcasterclass);

            if (spelllist == "Error")
            {
                var newGuid = BlueprintGuid.NewGuid();
                string newline = "\"" + spellcasterclass.name + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                File.WriteAllLines(MysticPastLifeFilePath + "\\" + name + ".txt", [newline]);
                return null;
            }

            var CastingStat = basecasterclass.Spellbook.CastingAttribute;
            var MinimumStat = 10 + i * 2;
            var spelllevelpenalty = GetMysticPastGetLevelPenalty(basecasterclass);

            var feat = ParametrizedFeatureConfigurator.New(name, guid)
                .SetDisplayName(MysticPastLifeDisplayName)
                .SetDescription(MysticPastLifeDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.MagusSpellRecall.ToString()).m_Icon) // Maybe change this
                .SetIsClassFeature(false)
                .SetSpellcasterClass(basecasterclass)
                .SetSpellList(spelllist)
                .SetHideNotAvailibleInUI()
                .SetRanks(1)
                .SetParameterType(FeatureParameterType.LearnSpell)
                .SetSpecificSpellLevel(false)
                .SetSpellLevelPenalty(spelllevelpenalty)
                .AddPrerequisiteClassLevel(basecasterclass.ToReference<BlueprintCharacterClassReference>(), 1, true)
                .AddComponent<LearnSpellParametrized>(c =>
                {
                    c.m_SpellcasterClass = basecasterclass.ToReference<BlueprintCharacterClassReference>();
                    c.m_SpellList = BlueprintTools.GetBlueprintReference<BlueprintSpellListReference>(spelllist);
                    c.SpecificSpellLevel = false;
                    c.SpellLevelPenalty = 0;
                    c.SpellLevel = 0;
                })
                .Configure();

            if (i > 0) ParametrizedFeatureConfigurator.For(guid).AddPrerequisiteStatValue(CastingStat, MinimumStat, true).Configure();

                var returnfeat = ParametrizedFeatureConfigurator.For(guid).Configure().ToReference<BlueprintParametrizedFeatureReference>();

            return BlueprintTools.GetBlueprint<BlueprintParametrizedFeature>(returnfeat.ToString());
        }

        private static BlueprintProgression CreateMysticPastLifeClassFeat(BlueprintCharacterClass spellcasterclass)
        {
            List<BlueprintCharacterClass> AllArcaneCasterClasses =
            [
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.ArcanistClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.BardClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.BloodragerClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.MagusClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.SkaldClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.SorcererClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.WitchClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.WizardClass.ToString())
            ];

            List<BlueprintCharacterClass> AllDivineCasterClasses =
            [
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.ClericClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.DruidClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.HunterClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.InquisitorClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.OracleClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.PaladinClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.RangerClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.ShamanClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.WarpriestClass.ToString())
            ];

            var name = "MysticPastLife" + spellcasterclass.name + "Progression";
            var guid = GetMysticPastLifeProgressionGuid(spellcasterclass);

            if (guid == "Error")
            {
                var newGuid = BlueprintGuid.NewGuid();
                string newline = "\"" + spellcasterclass.name + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                File.WriteAllLines(MysticPastLifeFilePath + "\\" + name + ".txt", [newline]);
                return null;
            }

            var feat = ProgressionConfigurator.New(name, guid)
                .SetDisplayName(GetMysticPastLifeDisplayName(spellcasterclass))
                .SetDescription(MysticPastLifeDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.MagusSpellRecall.ToString()).m_Icon)
                .AddPrerequisiteCasterType(spellcasterclass.IsArcaneCaster)
                .AddPrerequisiteNoClassLevel(spellcasterclass)
                .SetHideNotAvailibleInUI()
                .SetGiveFeaturesForPreviousLevels(true)
                .SetReapplyOnLevelUp(false)
                .Configure();

            BlueprintParametrizedFeature[] ClassFeatures = [];

            if (spellcasterclass.IsArcaneCaster)
            {
                foreach (BlueprintCharacterClass basecasterclass in AllArcaneCasterClasses)
                {
                    if (spellcasterclass == basecasterclass) continue;
                    
                    for (int i = 0; i < 8; i++)
                    {
                        var SpellSelectionFeat = CreateMysticPastLifeSpellSelection(spellcasterclass, basecasterclass, i);

                        if (SpellSelectionFeat != null)
                        {
                            ClassFeatures = ClassFeatures.AppendToArray(SpellSelectionFeat);
                        }
                    }
                }
            }
            else
            {
                foreach (BlueprintCharacterClass basecasterclass in AllDivineCasterClasses)
                {
                    if (spellcasterclass == basecasterclass) continue;

                    for (int i = 0; i < 8; i++)
                    {
                        var SpellSelectionFeat = CreateMysticPastLifeSpellSelection(spellcasterclass, basecasterclass, i);

                        if (SpellSelectionFeat != null)
                        {
                            ClassFeatures = ClassFeatures.AppendToArray(SpellSelectionFeat);
                        }
                    }
                }
            }

            var levelEntry = new LevelEntry()
            {
                Level = 1,
                m_Features = ClassFeatures.Select(f => f.ToReference<BlueprintFeatureBaseReference>()).ToList()
            };

            feat.LevelEntries = feat.LevelEntries ?? [];
            feat.LevelEntries = CommonTool.Append(feat.LevelEntries, levelEntry);

            return feat;
        }

        private static BlueprintFeature CreateMysticPastLife()
        {
            List<BlueprintCharacterClass> MysticPastLifeClasses =
            [
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.BardClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.BloodragerClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.ClericClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.DruidClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.HunterClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.InquisitorClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.MagusClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.PaladinClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.RangerClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.ShamanClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.WarpriestClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.WitchClass.ToString()),
                BlueprintTools.GetBlueprint<BlueprintCharacterClass>(CharacterClassRefs.WizardClass.ToString())
            ];

            FeatureSelectionConfigurator.New("SamsaranMysticPastLifeSelection", MysticPastLifeGuid)
                .SetDisplayName(MysticPastLifeDisplayName)
                .SetDescription(MysticPastLifeDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.MagusSpellRecall.ToString()).m_Icon)
                .SetHideNotAvailibleInUI()
                .Configure();

                foreach (BlueprintCharacterClass spellcasterclass in MysticPastLifeClasses)
                {
                    var ClassSelectionFeat = CreateMysticPastLifeClassFeat(spellcasterclass);

                    if (ClassSelectionFeat != null)
                        FeatureSelectionConfigurator.For(MysticPastLifeGuid).AddToAllFeatures(ClassSelectionFeat).Configure();
                }

            var returnfeat = FeatureSelectionConfigurator.For(MysticPastLifeGuid).Configure().ToReference<BlueprintFeatureReference>();

            return BlueprintTools.GetBlueprint<BlueprintFeature>(returnfeat.ToString());
        }

        private static BlueprintFeature CreateRacialHeritage()
        {
            var shards = CreateShardsOfThePast();
            var lives = CreateMysticPastLife();


            var feat = FeatureSelectionConfigurator.New("SamsaranRacialHeritage", RacialHeritageGuid)
                .SetDisplayName(SamsaranRacialHeritageDisplayName)
                .SetDescription(SamsaranRacialHeritageDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(FeatureSelectionRefs.HalfOrcHeritageSelection.ToString()).Icon)
                .AddToAllFeatures(shards, lives)
                .SetGroup(FeatureGroup.Racial)
                .Configure();

            return feat;
        }

        private static BlueprintFeature CreateSamsaranMagic()
        {
            var resource = AbilityResourceConfigurator.New("SamsaranMagicCureLightWoundsResource", "{AB994D06-ED3F-47A8-8393-0ED65B838489}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var ability = AbilityConfigurator.New("SamsaranMagicCureLightWounds", "{AAAE9C42-FDED-4042-9F9B-7CDA8BE57C90}")
                .CopyFrom(AbilityRefs.CureLightWoundsCast, c => c is not (SpellListComponent or CraftInfoComponent))
                .AddAbilityResourceLogic(1,isSpendResource: true, requiredResource: resource)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 1)
                .Configure();
            
            var feat = FeatureConfigurator.New("SamsaranMagic", SamsaranMagicGuid)
                .SetDisplayName(SamsaranMagicDisplayName)
                .SetDescription(SamsaranMagicDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.GnomeMagic.ToString()).Icon)
                .AddFacts(new() { ability })
                .AddAbilityResources(1, resource, true)
                .AddReplaceCasterLevelOfAbility(spell: ability)
                .Configure();

            return feat;
        }

        public static void Configure()
        {
            Directory.CreateDirectory(MysticPastLifeFilePath);

            var NewFemaleHairArray = FemaleHairs.AppendToArray(CopyRace.FemaleOptions.Hair);

            var heritage = CreateRacialHeritage();
            var lifebound = CreateLifebound();
            var magic = CreateSamsaranMagic();

            var race =
            RaceConfigurator.New(SamsaranName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(SamsaranDisplayName)
                .SetDescription(SamsaranDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(heritage, lifebound, magic, FeatureRefs.KeenSenses.ToString())
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: -2)
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, eyecolors: RaceEyeColors, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.OreadRace.ToString()), CustomFemaleHairs: NewFemaleHairArray);

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
