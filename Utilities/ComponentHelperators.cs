using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.ResourceLinks;
using Kingmaker.Settings;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace EbonsContentMod.Utilities
{
    internal class ComponentHelperators
    {
        private static readonly Dictionary<BlueprintProgression, List<BlueprintAbility>> SpellsByBloodline = new();

        internal static void CreateBloodlineSpellList()
        {
            var SorcerBloodlines = FeatureSelectionRefs.SorcererBloodlineSelection.Reference.Get().Features;
            var BloodragerBloodlines = FeatureSelectionRefs.BloodragerBloodlineSelection.Reference.Get().Features;

            BlueprintFeatureReference[] AllBloodlines = SorcerBloodlines;
            AllBloodlines = AllBloodlines.AppendToArray(BloodragerBloodlines);

            List<BlueprintAbility> spells = [];

            foreach (BlueprintFeature bl in AllBloodlines)
            {
                if (bl is not BlueprintProgression bloodline)
                    continue;

                Main.log.Log("CreateBloodlineSpellList: Found a bloodline - " + bloodline.NameForAcronym);

                spells = [];

                foreach (var le in bloodline.LevelEntries)
                {
                    if (le.Features == null) continue;

                    //BlueprintFeatureBase[] SpellFeatures = [];

                    foreach (var bfb in le.Features)
                    {
                        if (bfb is not BlueprintFeature feature)
                            continue;

                        foreach (var c in feature.ComponentsArray)
                        {
                            if (c is not AddKnownSpell spellcomponent)
                                continue;

                            //var spell = BlueprintTools.GetBlueprint<BlueprintAbility>(spellcomponent.m_Spell.ToString());
                            var spell = spellcomponent.Spell;

                            if (spellcomponent.Spell != null && !spells.Contains(spell)) spells.Add(spell);
                        }
                    }
                }

                Main.log.Log("CreateBloodlineSpellList: adding " + spells.Count.ToString() + " spells to the dictionary for the key " + bloodline.NameForAcronym);

                SpellsByBloodline[bloodline] = spells;
            }
        }

        public static bool GetHasFocusForSpell(BlueprintAbility spell, UnitEntityData unit)
        {
            var school = spell.School;
            if (school != SpellSchool.None)
            {
                foreach (Feature feature in unit.Progression.Features)
                {
                    if (feature.Blueprint == ParametrizedFeatureRefs.SpellFocus.Reference.Get())
                    {
                        FeatureParam param = feature.Param;
                        if (param != null && param.SpellSchool != null)
                        {
                            SpellSchool? spellSchool = feature.Param.SpellSchool;
                            if (spellSchool == school) return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool GetIsBloodlineSpell(BlueprintAbility spell, UnitEntityData unit)
        {
            var SorcerBloodlines = FeatureSelectionRefs.SorcererBloodlineSelection.Reference.Get().Features.ToList();
            var BloodragerBloodlines = FeatureSelectionRefs.BloodragerBloodlineSelection.Reference.Get().Features.ToList();
            var AllBloodlines = SorcerBloodlines;
            AllBloodlines.AddRange(BloodragerBloodlines);

            foreach (Feature feature in unit.Progression.Features)
            {
                if (feature.Blueprint is not BlueprintProgression mybloodline)
                    continue;
                
                foreach (BlueprintFeature bl in AllBloodlines)
                {
                    if (bl is not BlueprintProgression checkbloodline)
                        continue;

                    if (mybloodline == checkbloodline && SpellsByBloodline.TryGetValue(checkbloodline, out var blspells))
                    {
                        if (blspells.Contains(spell)) return true;
                    }
                }
            }
            return false;
        }
    }
}
