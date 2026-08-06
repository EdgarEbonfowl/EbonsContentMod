using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.Utility;
using Owlcat.QA.Validation;
using UnityEngine;

namespace EbonsContentMod.Components
{
    /// <summary>
    /// Dynamically supplies spells that another spell may duplicate.
    ///
    /// Harmony patches to AbilityData.GetConversions() use this component
    /// to create runtime AbilityData conversions. No duplicated spell
    /// blueprints are created.
    ///
    /// This component is generic enough to support Miracle, Wish, and
    /// Limited Wish.
    /// </summary>
    [AllowedOn(typeof(BlueprintAbility), false)]
    [TypeId("ff2679a56baf4eceaa471bb8144c9db2")]
    public class AbilityDuplicateSpell : AbilityCustomLogic
    {
        #region Blueprint configuration

        /// <summary>
        /// The exact spell level represented by this menu node.
        ///
        /// Set this to a negative value to include every eligible spell
        /// level.
        /// </summary>
        public int SelectedSpellLevel;

        /// <summary>
        /// The spell list that receives the higher duplication limit.
        ///
        /// Miracle: Cleric
        /// Wish/Limited Wish: Wizard
        /// </summary>
        [ValidateNotNull]
        public BlueprintSpellListReference m_PrimarySpellList;

        /// <summary>
        /// Highest level that may be duplicated from the primary spell list.
        ///
        /// Miracle: 8
        /// Wish: 8
        /// Limited Wish: 6
        /// </summary>
        public int PrimaryMaxSpellLevel;

        /// <summary>
        /// Other ordinary class spell lists from which spells may be
        /// duplicated.
        /// </summary>
        public BlueprintSpellListReference[] m_OtherSpellLists =
            Array.Empty<BlueprintSpellListReference>();

        /// <summary>
        /// Highest level that may be duplicated from a non-primary list.
        ///
        /// Miracle: 7
        /// Wish: 7
        /// Limited Wish: 5
        /// </summary>
        public int OtherMaxSpellLevel;

        /// <summary>
        /// Optional manual exclusions for spells that should not be
        /// duplicated.
        ///
        /// Excluding a parent ability also excludes its entire variant tree.
        /// Individual leaf variants can be excluded separately.
        /// </summary>
        public BlueprintAbilityReference[] m_ExcludedSpells =
            Array.Empty<BlueprintAbilityReference>();

        /// <summary>
        /// Whether level-zero spells may be included.
        /// </summary>
        public bool IncludeCantrips;

        /// <summary>
        /// Exclude abilities that spend an ability resource belonging to the
        /// original spell rather than the duplicating spell.
        /// </summary>
        public bool ExcludeSpellsWithResourceLogic = true;

        /// <summary>
        /// Recursively replace abilities containing AbilityVariants with
        /// their final castable leaf variants.
        /// </summary>
        public bool FlattenVariants = true;

        /// <summary>
        /// Calculate the save DC from the duplicating spell before replacing
        /// its blueprint with the selected spell.
        /// </summary>
        public bool UseSourceAbilityDC = true;

        /// <summary>
        /// Use the duplicating spell's casting action and full-round status.
        ///
        /// This field is consumed by the corresponding AbilityData Harmony
        /// patches.
        /// </summary>
        public bool UseSourceActionType = true;

        /// <summary>
        /// Material components whose total value is no greater than this
        /// amount are ignored.
        ///
        /// -1: ignore none
        /// 100: Miracle
        /// 1000: Limited Wish
        /// 10000: Wish
        /// </summary>
        public int IgnoreMaterialComponentCostUpTo = -1;

        #endregion

        #region Blueprint caches

        /// <summary>
        /// Final filtered and flattened spells shown by this specific
        /// component.
        /// </summary>
        [NonSerialized]
        private BlueprintAbility[] m_CachedAvailableSpells;

        /// <summary>
        /// Lowest legal duplicated-spell level for each final leaf ability.
        ///
        /// This will also be useful later for displaying or grouping spells
        /// by effective level.
        /// </summary>
        [NonSerialized]
        private Dictionary<BlueprintAbility, int>
            m_CachedEffectiveSpellLevels;

        /// <summary>
        /// Resolved blueprint objects from m_ExcludedSpells.
        /// </summary>
        [NonSerialized]
        private HashSet<BlueprintAbility> m_CachedExcludedSpells;

        /// <summary>
        /// Incremented whenever ClearCache() is called. Runtime conversion
        /// cache entries compare this value to determine whether they must
        /// rebuild.
        /// </summary>
        [NonSerialized]
        private int m_CacheGeneration;

        public bool IncludeAllSpellLevels
        {
            get
            {
                return SelectedSpellLevel < 0;
            }
        }

        public BlueprintSpellList PrimarySpellList
        {
            get
            {
                return m_PrimarySpellList?.Get();
            }
        }

        /// <summary>
        /// Returns the final cached spell list for this component.
        ///
        /// The first call:
        /// 1. Scans all configured spell lists.
        /// 2. Determines each spell's lowest legal level.
        /// 3. Recursively flattens AbilityVariants.
        /// 4. Removes exclusions and resource-backed abilities.
        /// 5. Deduplicates and sorts the results.
        ///
        /// Subsequent calls return the same array without rescanning.
        /// </summary>
        public IReadOnlyList<BlueprintAbility> GetAvailableSpells()
        {
            EnsureSpellCache();
            return m_CachedAvailableSpells;
        }

        /// <summary>
        /// Returns the lowest legal duplicated-spell level assigned to a
        /// final leaf ability, or -1 if the ability is not available through
        /// this component.
        /// </summary>
        public int GetEffectiveSpellLevel(BlueprintAbility spell)
        {
            if (spell == null)
                return -1;

            EnsureSpellCache();

            return m_CachedEffectiveSpellLevels.TryGetValue(
                spell,
                out int level)
                ? level
                : -1;
        }

        /// <summary>
        /// Invalidates every cache owned by this component.
        ///
        /// Call this after changing any of the component's configuration
        /// fields after its spell list has already been requested.
        /// </summary>
        public void ClearCache()
        {
            m_CachedAvailableSpells = null;
            m_CachedEffectiveSpellLevels = null;
            m_CachedExcludedSpells = null;

            unchecked
            {
                m_CacheGeneration++;
            }
        }

        private void EnsureSpellCache()
        {
            if (m_CachedAvailableSpells != null
                && m_CachedEffectiveSpellLevels != null)
            {
                return;
            }

            EnsureExcludedSpellCache();

            // First determine the lowest legal level at which each root
            // spell can be duplicated.
            var rootSpellLevels =
                new Dictionary<BlueprintAbility, int>();

            AddSpellsFromList(
                PrimarySpellList,
                PrimaryMaxSpellLevel,
                rootSpellLevels);

            foreach (BlueprintSpellListReference spellListReference in
                     m_OtherSpellLists
                     ?? Array.Empty<BlueprintSpellListReference>())
            {
                AddSpellsFromList(
                    spellListReference?.Get(),
                    OtherMaxSpellLevel,
                    rootSpellLevels);
            }

            // Then flatten each root into castable leaf variants. A leaf may
            // be reachable through more than one list or parent, so preserve
            // its lowest legal level.
            var leafSpellLevels =
                new Dictionary<BlueprintAbility, int>();

            foreach (KeyValuePair<BlueprintAbility, int> rootEntry in
                     rootSpellLevels)
            {
                var recursionPath =
                    new HashSet<BlueprintAbility>();

                foreach (BlueprintAbility leaf in
                         ExpandLeafVariants(
                             rootEntry.Key,
                             recursionPath))
                {
                    if (!IsAllowedSpell(leaf))
                        continue;

                    if (!leafSpellLevels.TryGetValue(
                            leaf,
                            out int currentLevel)
                        || rootEntry.Value < currentLevel)
                    {
                        leafSpellLevels[leaf] = rootEntry.Value;
                    }
                }
            }

            m_CachedEffectiveSpellLevels = leafSpellLevels;

            IEnumerable<KeyValuePair<BlueprintAbility, int>> entries =
                leafSpellLevels;

            if (!IncludeAllSpellLevels)
            {
                entries = entries.Where(
                    entry =>
                        entry.Value == SelectedSpellLevel);
            }

            m_CachedAvailableSpells =
                entries
                    .OrderBy(entry => entry.Value)
                    .ThenBy(entry => entry.Key.School)
                    .ThenBy(
                        entry => entry.Key.Name,
                        StringComparer.CurrentCultureIgnoreCase)
                    .Select(entry => entry.Key)
                    .ToArray();
        }

        private void EnsureExcludedSpellCache()
        {
            if (m_CachedExcludedSpells != null)
                return;

            var excludedSpells =
                new HashSet<BlueprintAbility>();

            foreach (BlueprintAbilityReference excludedReference in
                     m_ExcludedSpells
                     ?? Array.Empty<BlueprintAbilityReference>())
            {
                BlueprintAbility excluded =
                    excludedReference?.Get();

                if (excluded != null)
                    excludedSpells.Add(excluded);
            }

            m_CachedExcludedSpells = excludedSpells;
        }

        private void AddSpellsFromList(
            BlueprintSpellList spellList,
            int maximumLevel,
            IDictionary<BlueprintAbility, int> minimumLevels)
        {
            if (spellList == null || maximumLevel < 0)
                return;

            int firstLevel =
                IncludeCantrips
                    ? 0
                    : 1;

            int lastLevel =
                Math.Min(maximumLevel, 9);

            for (int level = firstLevel;
                 level <= lastLevel;
                 level++)
            {
                IList<BlueprintAbility> spells =
                    spellList.GetSpells(level);

                if (spells == null)
                    continue;

                foreach (BlueprintAbility spell in spells)
                {
                    if (spell == null)
                        continue;

                    // Excluding a root ability excludes the whole tree.
                    if (IsExcluded(spell))
                        continue;

                    if (!minimumLevels.TryGetValue(
                            spell,
                            out int currentMinimum)
                        || level < currentMinimum)
                    {
                        minimumLevels[spell] = level;
                    }
                }
            }
        }

        /// <summary>
        /// Recursively returns castable leaves from an AbilityVariants tree.
        ///
        /// recursionPath protects against malformed circular variant
        /// references while still allowing the same leaf to appear under
        /// another independent root.
        /// </summary>
        private IEnumerable<BlueprintAbility> ExpandLeafVariants(
            BlueprintAbility ability,
            ISet<BlueprintAbility> recursionPath)
        {
            if (ability == null)
                yield break;

            if (!recursionPath.Add(ability))
                yield break;

            try
            {
                if (!FlattenVariants)
                {
                    yield return ability;
                    yield break;
                }

                AbilityVariants variants =
                    ability.GetComponent<AbilityVariants>();

                BlueprintAbilityReference[] variantReferences =
                    variants?.m_Variants;

                if (variantReferences == null
                    || variantReferences.Length == 0)
                {
                    yield return ability;
                    yield break;
                }

                bool foundValidVariant = false;

                foreach (BlueprintAbilityReference variantReference in
                         variantReferences)
                {
                    BlueprintAbility variant =
                        variantReference?.Get();

                    if (variant == null)
                        continue;

                    foundValidVariant = true;

                    foreach (BlueprintAbility leaf in
                             ExpandLeafVariants(
                                 variant,
                                 recursionPath))
                    {
                        yield return leaf;
                    }
                }

                // Do not silently lose a malformed ability whose
                // AbilityVariants component contains only null references.
                if (!foundValidVariant)
                    yield return ability;
            }
            finally
            {
                recursionPath.Remove(ability);
            }
        }

        #endregion

        #region Spell filters

        /// <summary>
        /// Shared filter applied to final leaf abilities.
        /// </summary>
        public bool IsAllowedSpell(BlueprintAbility spell)
        {
            if (spell == null)
                return false;

            if (IsExcluded(spell))
                return false;

            if (ExcludeSpellsWithResourceLogic
                && spell.GetComponent<AbilityResourceLogic>() != null)
            {
                return false;
            }

            return true;
        }

        private bool IsExcluded(BlueprintAbility spell)
        {
            if (spell == null)
                return false;

            EnsureExcludedSpellCache();

            return m_CachedExcludedSpells.Contains(spell);
        }

        #endregion

        #region Runtime AbilityData cache

        /// <summary>
        /// Runtime conversion caches are keyed weakly by the source
        /// AbilityData. When the source AbilityData is no longer referenced,
        /// its converted abilities can also be collected.
        /// </summary>
        private static readonly ConditionalWeakTable<
            AbilityData,
            RuntimeConversionCacheEntry>
            s_RuntimeConversionCache =
                new ConditionalWeakTable<
                    AbilityData,
                    RuntimeConversionCacheEntry>();

        private sealed class RuntimeConversionCacheEntry
        {
            public AbilityDuplicateSpell Provider;

            public int Generation = -1;

            public BlueprintAbility[] Blueprints =
                Array.Empty<BlueprintAbility>();

            public List<AbilityData> Conversions =
                new List<AbilityData>();

            public int LastDCFrame = -1;

            public int CachedDC;
        }

        /// <summary>
        /// Returns cached runtime AbilityData conversions for this source.
        ///
        /// The conversion objects are rebuilt only when:
        /// - A new source AbilityData is encountered.
        /// - This source was previously associated with another provider.
        /// - ClearCache() changed the cache generation.
        ///
        /// The source spell's DC is refreshed at most once per Unity frame,
        /// so temporary bonuses remain responsive without firing thousands
        /// of repeated RuleCalculateAbilityParams events in one frame.
        /// </summary>
        public IReadOnlyList<AbilityData> GetOrCreateConversions(
            AbilityData source)
        {
            if (source == null)
                return Array.Empty<AbilityData>();

            EnsureSpellCache();

            RuntimeConversionCacheEntry cache =
                s_RuntimeConversionCache.GetValue(
                    source,
                    _ => new RuntimeConversionCacheEntry());

            lock (cache)
            {
                if (cache.Provider != this
                    || cache.Generation != m_CacheGeneration)
                {
                    RebuildRuntimeConversions(
                        source,
                        cache);
                }

                RefreshRuntimeConversionDC(
                    source,
                    cache);

                return cache.Conversions;
            }
        }

        private void RebuildRuntimeConversions(
            AbilityData source,
            RuntimeConversionCacheEntry cache)
        {
            BlueprintAbility[] availableSpells =
                m_CachedAvailableSpells
                ?? Array.Empty<BlueprintAbility>();

            var conversions =
                new List<AbilityData>(
                    availableSpells.Length);

            foreach (BlueprintAbility spell in availableSpells)
            {
                if (spell == null)
                    continue;

                conversions.Add(
                    new AbilityData(source, spell));
            }

            cache.Provider = this;
            cache.Generation = m_CacheGeneration;
            cache.Blueprints = availableSpells;
            cache.Conversions = conversions;
            cache.LastDCFrame = -1;
            cache.CachedDC = 0;
        }

        private void RefreshRuntimeConversionDC(
            AbilityData source,
            RuntimeConversionCacheEntry cache)
        {
            if (!UseSourceAbilityDC)
                return;

            int currentFrame =
                Time.frameCount;

            if (cache.LastDCFrame != currentFrame)
            {
                cache.CachedDC =
                    source.CalculateParams().DC;

                cache.LastDCFrame =
                    currentFrame;
            }

            foreach (AbilityData conversion in
                     cache.Conversions)
            {
                conversion.OverrideDC =
                    cache.CachedDC;

                // The completed DC came from Miracle/Wish/Limited Wish.
                // Do not add school-specific bonuses from the substituted
                // spell a second time.
                conversion.IgnoreDCBonuses = true;
            }
        }

        #endregion

        #region Material components

        /// <summary>
        /// Returns true when the selected spell's own material component
        /// should be suppressed.
        /// </summary>
        public bool ShouldIgnoreMaterialComponent(
            BlueprintAbility spell)
        {
            if (spell == null
                || IgnoreMaterialComponentCostUpTo < 0)
            {
                return false;
            }

            BlueprintAbility.MaterialComponentData material =
                spell.MaterialComponent;

            if (material == null || material.Item == null)
                return false;

            long totalCost =
                (long)material.Item.Cost
                * Math.Max(0, material.Count);

            return totalCost
                   <= IgnoreMaterialComponentCostUpTo;
        }

        #endregion

        #region ConvertedFrom helper

        /// <summary>
        /// Finds the AbilityData in the ConvertedFrom chain whose blueprint
        /// contains AbilityDuplicateSpell.
        /// </summary>
        public static bool TryGetConversionSource(
            AbilityData ability,
            out AbilityData source,
            out AbilityDuplicateSpell component)
        {
            source = null;
            component = null;

            if (ability == null)
                return false;

            AbilityData current =
                ability.ConvertedFrom;

            while (current != null)
            {
                AbilityDuplicateSpell provider =
                    current.Blueprint
                        ?.GetComponent<AbilityDuplicateSpell>();

                if (provider != null)
                {
                    source = current;
                    component = provider;
                    return true;
                }

                current = current.ConvertedFrom;
            }

            return false;
        }

        #endregion

        #region AbilityCustomLogic

        // This component is a runtime conversion provider, not delivery logic.
        public override IEnumerator<AbilityDeliveryTarget> Deliver(
            AbilityExecutionContext context,
            TargetWrapper target)
        {
            yield break;
        }

        public override void Cleanup(
            AbilityExecutionContext context)
        {
        }

        #endregion
    }
}