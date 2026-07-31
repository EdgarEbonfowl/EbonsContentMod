using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.View;
using Kingmaker.View.Equipment;
using Kingmaker.Visual.CharacterSystem;
using Owlcat.Runtime.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EbonsContentMod.Utilities
{
    internal class EELinker
    {
        /*
         * These retain the registered links by race. They are used by
         * DollState.ApplyRamps and AddItemEquipment.
         */
        private static readonly Dictionary<BlueprintRace, EquipmentEntityLink[]>
            EyeLinkEELs = new();

        private static readonly Dictionary<BlueprintRace, EquipmentEntityLink[]>
            SkinLinkEELs = new();

        private static readonly Dictionary<BlueprintRace, EquipmentEntityLink[]>
            HairLinkEELs = new();

        /*
         * Canonical mappings. AssetId strings are stable even when the game
         * loads a new EquipmentEntity object for the same resource.
         *
         * "SourcesByLinkedAssetId":
         *     Linked circuitry/body-paint asset -> source head/hair assets.
         *
         * "LinksBySourceAssetId":
         *     Source head/hair asset -> linked circuitry/body-paint assets.
         */
        private static readonly Dictionary<string, string[]>
            EyeSourcesByLinkedAssetId =
                new(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<string, string[]>
            SkinSourcesByLinkedAssetId =
                new(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<string, string[]>
            HairSourcesByLinkedAssetId =
                new(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<string, string[]>
            EyeLinksBySourceAssetId =
                new(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<string, string[]>
            SkinLinksBySourceAssetId =
                new(StringComparer.OrdinalIgnoreCase);

        private static readonly Dictionary<string, string[]>
            HairLinksBySourceAssetId =
                new(StringComparer.OrdinalIgnoreCase);

        /*
         * EquipmentEntity itself does not expose its originating AssetId.
         * Therefore, register the relationship whenever both the link and its
         * loaded entity are available.
         */
        private static readonly Dictionary<EquipmentEntity, string>
            AssetIdByInstance = new();

        /*
         * A rebuilt EquipmentEntity may be a new object. The signature allows
         * us to associate that new object with a registered AssetId.
         *
         * A signature can theoretically match multiple AssetIds. In that case
         * the resolver refuses to guess.
         */
        private static readonly Dictionary<string, HashSet<string>>
            AssetIdsBySignature =
                new(StringComparer.Ordinal);

        private static bool ApplyingLinkedRamp;

        // ================================================================
        // Registration
        // ================================================================

        public static void RegisterEyeLink(
            BlueprintRace newrace,
            EquipmentEntityLink[] EELinks)
        {
            if (newrace == null || EELinks == null)
            {
                return;
            }

            EyeLinkEELs[newrace] = EELinks;

            EquipmentEntityLink[] sourceLinks =
                GetCombinedLinks(
                    newrace.MaleOptions?.Heads,
                    newrace.FemaleOptions?.Heads);

            RegisterLinkSet(
                sourceLinks,
                EELinks,
                EyeSourcesByLinkedAssetId,
                EyeLinksBySourceAssetId);
        }

        public static void RegisterSkinLink(
            BlueprintRace newrace,
            EquipmentEntityLink[] EELinks)
        {
            if (newrace == null || EELinks == null)
            {
                return;
            }

            SkinLinkEELs[newrace] = EELinks;

            EquipmentEntityLink[] sourceLinks =
                GetCombinedLinks(
                    newrace.MaleOptions?.Heads,
                    newrace.FemaleOptions?.Heads);

            RegisterLinkSet(
                sourceLinks,
                EELinks,
                SkinSourcesByLinkedAssetId,
                SkinLinksBySourceAssetId);
        }

        public static void RegisterHairLink(
            BlueprintRace newrace,
            EquipmentEntityLink[] EELinks)
        {
            if (newrace == null || EELinks == null)
            {
                return;
            }

            HairLinkEELs[newrace] = EELinks;

            EquipmentEntityLink[] sourceLinks =
                GetCombinedLinks(
                    newrace.MaleOptions?.Hair,
                    newrace.FemaleOptions?.Hair);

            RegisterLinkSet(
                sourceLinks,
                EELinks,
                HairSourcesByLinkedAssetId,
                HairLinksBySourceAssetId);
        }

        private static EquipmentEntityLink[] GetCombinedLinks(
            IEnumerable<EquipmentEntityLink> first,
            IEnumerable<EquipmentEntityLink> second)
        {
            return (first ?? Enumerable.Empty<EquipmentEntityLink>())
                .Concat(second ?? Enumerable.Empty<EquipmentEntityLink>())
                .Where(link =>
                    link != null &&
                    !string.IsNullOrEmpty(NormalizeAssetId(link.AssetId.ToString())))
                .GroupBy(
                    link => NormalizeAssetId(link.AssetId.ToString()),
                    StringComparer.OrdinalIgnoreCase)
                .Select(group => group.First())
                .ToArray();
        }

        private static void RegisterLinkSet(
            EquipmentEntityLink[] sourceLinks,
            EquipmentEntityLink[] linkedLinks,
            Dictionary<string, string[]> sourcesByLinkedAssetId,
            Dictionary<string, string[]> linksBySourceAssetId)
        {
            sourceLinks ??= Array.Empty<EquipmentEntityLink>();
            linkedLinks ??= Array.Empty<EquipmentEntityLink>();

            /*
             * Register each link/object relationship while both are known.
             */
            foreach (EquipmentEntityLink link in sourceLinks)
            {
                LoadAndRegister(link);
            }

            foreach (EquipmentEntityLink link in linkedLinks)
            {
                LoadAndRegister(link);
            }

            string[] sourceAssetIds = sourceLinks
                .Where(link =>
                    link != null &&
                    !string.IsNullOrEmpty(NormalizeAssetId(link.AssetId.ToString())))
                .Select(link => NormalizeAssetId(link.AssetId.ToString()))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            string[] linkedAssetIds = linkedLinks
                .Where(link =>
                    link != null &&
                    !string.IsNullOrEmpty(NormalizeAssetId(link.AssetId.ToString())))
                .Select(link => NormalizeAssetId(link.AssetId.ToString()))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            foreach (string linkedAssetId in linkedAssetIds)
            {
                MergeMapping(
                    sourcesByLinkedAssetId,
                    linkedAssetId,
                    sourceAssetIds);
            }

            foreach (string sourceAssetId in sourceAssetIds)
            {
                MergeMapping(
                    linksBySourceAssetId,
                    sourceAssetId,
                    linkedAssetIds);
            }
        }

        private static void MergeMapping(
            Dictionary<string, string[]> dictionary,
            string key,
            IEnumerable<string> values)
        {
            if (dictionary == null || string.IsNullOrEmpty(key))
            {
                return;
            }

            string[] newValues = (values ?? Enumerable.Empty<string>())
                .Where(value => !string.IsNullOrEmpty(value))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();

            if (dictionary.TryGetValue(key, out string[] existing))
            {
                dictionary[key] = existing
                    .Concat(newValues)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToArray();
            }
            else
            {
                dictionary[key] = newValues;
            }
        }

        // ================================================================
        // Runtime AssetId resolution
        // ================================================================

        private static EquipmentEntity LoadAndRegister(
            EquipmentEntityLink link)
        {
            if (link == null)
            {
                return null;
            }

            string assetId =
                NormalizeAssetId(link.AssetId.ToString());

            if (string.IsNullOrEmpty(assetId))
            {
                return null;
            }

            EquipmentEntity equipmentEntity =
                link.Load(false, true);

            if (equipmentEntity != null)
            {
                RegisterLoadedInstance(
                    equipmentEntity,
                    assetId);
            }

            return equipmentEntity;
        }

        private static void RegisterLoadedInstance(
            EquipmentEntity equipmentEntity,
            string assetId)
        {
            if (equipmentEntity == null ||
                string.IsNullOrEmpty(assetId))
            {
                return;
            }

            AssetIdByInstance[equipmentEntity] = assetId;

            Main.log.Log(
                $"EELinker REGISTER INSTANCE: " +
                $"name={equipmentEntity.name}, " +
                $"hash={equipmentEntity.GetHashCode()}, " +
                $"assetId={assetId}");

            string signature =
                GetEquipmentEntitySignature(equipmentEntity);

            if (string.IsNullOrEmpty(signature))
            {
                return;
            }

            if (!AssetIdsBySignature.TryGetValue(
                    signature,
                    out HashSet<string> assetIds))
            {
                assetIds = new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);

                AssetIdsBySignature[signature] = assetIds;
            }

            assetIds.Add(assetId);

            Main.log.Log(
                $"EELinker REGISTER SIGNATURE: " +
                $"name={equipmentEntity.name}, " +
                $"assetId={assetId}, " +
                $"candidateCount={assetIds.Count}, " +
                $"signature={signature}");
        }

        private static string GetEquipmentEntitySignature(
            EquipmentEntity equipmentEntity)
        {
            if (equipmentEntity == null)
            {
                return null;
            }

            string normalizedName =
                equipmentEntity.name?.Replace("(Clone)", string.Empty)
                                    .Trim()
                ?? string.Empty;

            return string.Join(
                "|",
                normalizedName,
                equipmentEntity.OtherVariantGuid ?? string.Empty,
                equipmentEntity.IsLowresVariant.ToString(),
                equipmentEntity.Layer.ToString());
        }

        private static bool TryGetAssetId(
            EquipmentEntity equipmentEntity,
            out string assetId)
        {
            assetId = null;

            if (equipmentEntity == null)
            {
                Main.log.Log("EELinker TryGetAssetId: equipmentEntity was null.");
                return false;
            }

            if (AssetIdByInstance.TryGetValue(
                    equipmentEntity,
                    out assetId))
            {
                Main.log.Log(
                    $"EELinker TryGetAssetId DIRECT: " +
                    $"name={equipmentEntity.name}, " +
                    $"hash={equipmentEntity.GetHashCode()}, " +
                    $"assetId={assetId}");

                return true;
            }

            string signature =
                GetEquipmentEntitySignature(equipmentEntity);

            if (string.IsNullOrEmpty(signature))
            {
                Main.log.Log(
                    $"EELinker TryGetAssetId FAILED: " +
                    $"name={equipmentEntity.name}, " +
                    $"hash={equipmentEntity.GetHashCode()}, " +
                    $"signature was empty.");

                return false;
            }

            if (!AssetIdsBySignature.TryGetValue(
                    signature,
                    out HashSet<string> candidates))
            {
                Main.log.Log(
                    $"EELinker TryGetAssetId FAILED: " +
                    $"name={equipmentEntity.name}, " +
                    $"hash={equipmentEntity.GetHashCode()}, " +
                    $"no registered signature match. " +
                    $"signature={signature}");

                return false;
            }

            if (candidates.Count != 1)
            {
                Main.log.Log(
                    $"EELinker TryGetAssetId AMBIGUOUS: " +
                    $"name={equipmentEntity.name}, " +
                    $"hash={equipmentEntity.GetHashCode()}, " +
                    $"candidateCount={candidates.Count}, " +
                    $"candidates={string.Join(",", candidates)}, " +
                    $"signature={signature}");

                return false;
            }

            assetId = candidates.First();

            AssetIdByInstance[equipmentEntity] = assetId;

            Main.log.Log(
                $"EELinker TryGetAssetId SIGNATURE: " +
                $"name={equipmentEntity.name}, " +
                $"hash={equipmentEntity.GetHashCode()}, " +
                $"assetId={assetId}");

            return true;
        }

        // ================================================================
        // Character ramp lookup helpers
        // ================================================================

        private static Character.SelectedRampIndices FindRampIndices(
            Character character,
            IEnumerable<string> assetIds)
        {
            if (character == null || assetIds == null)
            {
                return null;
            }

            var desiredAssetIds = new HashSet<string>(
                assetIds.Where(id => !string.IsNullOrEmpty(id)),
                StringComparer.OrdinalIgnoreCase);

            if (desiredAssetIds.Count == 0)
            {
                return null;
            }

            foreach (Character.SelectedRampIndices rampIndices
                     in character.m_RampIndices)
            {
                EquipmentEntity runtimeEntity =
                    rampIndices?.EquipmentEntity;

                if (runtimeEntity == null)
                {
                    continue;
                }

                if (TryGetAssetId(
                        runtimeEntity,
                        out string runtimeAssetId) &&
                    desiredAssetIds.Contains(runtimeAssetId))
                {
                    return rampIndices;
                }
            }

            return null;
        }

        private static EquipmentEntity FindRuntimeEntity(
            Character character,
            string assetId)
        {
            if (character == null || string.IsNullOrEmpty(assetId))
            {
                return null;
            }

            foreach (Character.SelectedRampIndices rampIndices
                     in character.m_RampIndices)
            {
                EquipmentEntity runtimeEntity =
                    rampIndices?.EquipmentEntity;

                if (runtimeEntity == null)
                {
                    continue;
                }

                if (!TryGetAssetId(
                        runtimeEntity,
                        out string runtimeAssetId))
                {
                    continue;
                }

                if (string.Equals(
                        runtimeAssetId,
                        assetId,
                        StringComparison.OrdinalIgnoreCase))
                {
                    Main.log.Log(
                        $"EELinker FindRuntimeEntity FOUND: " +
                        $"requested={assetId}, " +
                        $"name={runtimeEntity.name}, " +
                        $"hash={runtimeEntity.GetHashCode()}");

                    return runtimeEntity;
                }
            }

            Main.log.Log(
                $"EELinker FindRuntimeEntity FAILED: " +
                $"requested={assetId}, " +
                $"rampEntryCount={character.m_RampIndices.Count}");

            return null;
        }

        private static bool TryGetSourceRamp(
            Character character,
            string[] sourceAssetIds,
            bool useSecondaryRamp,
            out int rampIndex)
        {
            rampIndex = -1;

            Character.SelectedRampIndices source =
                FindRampIndices(character, sourceAssetIds);

            if (source == null)
            {
                return false;
            }

            int candidate = useSecondaryRamp
                ? source.SecondaryIndex
                : source.PrimaryIndex;

            if (candidate < 0)
            {
                return false;
            }

            rampIndex = candidate;
            return true;
        }

        // ================================================================
        // Helpers
        // ================================================================

        private static string NormalizeAssetId(string assetId)
        {
            if (string.IsNullOrWhiteSpace(assetId))
            {
                return null;
            }

            return assetId
                .Trim()
                .Trim('{', '}')
                .Replace("-", string.Empty)
                .ToLowerInvariant();
        }

        // ================================================================
        // Harmony patches
        // ================================================================

        [HarmonyPatch]
        private static class Patches
        {
            /*
             * This catches both sides of the rebuild:
             *
             * 1. A source head/hair receives its selected ramp and propagates
             *    that ramp to linked entities.
             *
             * 2. A linked entity later receives its default ramp and is
             *    corrected from the current source entity.
             */
            [HarmonyPatch(
                typeof(Character),
                "SetRampIndices",
                typeof(EquipmentEntity),
                typeof(int),
                typeof(int),
                typeof(int),
                typeof(int))]
            [HarmonyPostfix]
            private static void SetRampIndices_Postfix(
                Character __instance,
                EquipmentEntity ee,
                int primaryRampIndex,
                int secondaryRampIndex)
            {
                if (!Main.Settings.Races)
                {
                    return;
                }

                if (ApplyingLinkedRamp)
                {
                    return;
                }

                if (__instance == null)
                {
                    Main.log.Log(
                        "EELinker SetRampIndices: character was null.");

                    return;
                }

                if (ee == null)
                {
                    Main.log.Log(
                        "EELinker SetRampIndices: EquipmentEntity was null.");

                    return;
                }

                Main.log.Log(
                    $"EELinker SetRampIndices ENTER: " +
                    $"name={ee.name}, " +
                    $"hash={ee.GetHashCode()}, " +
                    $"primary={primaryRampIndex}, " +
                    $"secondary={secondaryRampIndex}");

                if (!TryGetAssetId(ee, out string currentAssetId))
                {
                    Main.log.Log(
                        $"EELinker SetRampIndices ABORT: " +
                        $"could not resolve AssetId for {ee.name}, " +
                        $"hash={ee.GetHashCode()}");

                    return;
                }

                Main.log.Log(
                    $"EELinker SetRampIndices RESOLVED: " +
                    $"name={ee.name}, " +
                    $"assetId={currentAssetId}, " +
                    $"isEyeSource={EyeLinksBySourceAssetId.ContainsKey(currentAssetId)}, " +
                    $"isEyeLinked={EyeSourcesByLinkedAssetId.ContainsKey(currentAssetId)}");

                try
                {
                    ApplyingLinkedRamp = true;

                    /*
                     * TARGET-SIDE CORRECTION
                     *
                     * The entity whose ramp just changed is itself a linked
                     * eye/skin/hair entity.
                     */

                    if (EyeSourcesByLinkedAssetId.TryGetValue(
                            currentAssetId,
                            out string[] eyeSourceAssetIds) &&
                        TryGetSourceRamp(
                            __instance,
                            eyeSourceAssetIds,
                            useSecondaryRamp: true,
                            out int eyeRamp))
                    {
                        Main.log.Log(
                            $"EELinker TARGET eye correction: " +
                            $"linked={ee.name}, " +
                            $"linkedAssetId={currentAssetId}, " +
                            $"eyeRamp={eyeRamp}");

                        __instance.SetPrimaryRampIndex(
                            ee,
                            eyeRamp,
                            false);

                        return;
                    }

                    if (SkinSourcesByLinkedAssetId.TryGetValue(
                            currentAssetId,
                            out string[] skinSourceAssetIds) &&
                        TryGetSourceRamp(
                            __instance,
                            skinSourceAssetIds,
                            useSecondaryRamp: false,
                            out int skinRamp))
                    {
                        /*
                         * Preserve your original behavior of assigning the
                         * source skin ramp to both ramp channels.
                         */
                        __instance.SetRampIndices(
                            ee,
                            skinRamp,
                            skinRamp);

                        return;
                    }

                    if (HairSourcesByLinkedAssetId.TryGetValue(
                            currentAssetId,
                            out string[] hairSourceAssetIds) &&
                        TryGetSourceRamp(
                            __instance,
                            hairSourceAssetIds,
                            useSecondaryRamp: false,
                            out int hairRamp))
                    {
                        __instance.SetPrimaryRampIndex(
                            ee,
                            hairRamp,
                            false);

                        return;
                    }

                    /*
                     * SOURCE-SIDE PROPAGATION
                     *
                     * The entity whose ramp just changed is a source head or
                     * source hair entity.
                     */

                    if (secondaryRampIndex >= 0 &&
                        EyeLinksBySourceAssetId.TryGetValue(
                            currentAssetId,
                            out string[] eyeLinkedAssetIds))
                    {
                        Main.log.Log(
                            $"EELinker SOURCE eye propagation: " +
                            $"source={ee.name}, " +
                            $"sourceAssetId={currentAssetId}, " +
                            $"eyeRamp={secondaryRampIndex}, " +
                            $"linkedCount={eyeLinkedAssetIds.Length}");

                        foreach (string linkedAssetId
                                 in eyeLinkedAssetIds)
                        {
                            EquipmentEntity runtimeLinkedEntity =
                                FindRuntimeEntity(
                                    __instance,
                                    linkedAssetId);

                            Main.log.Log(
                                $"EELinker SOURCE searching for linked AssetId: " +
                                $"{linkedAssetId}");

                            if (runtimeLinkedEntity == null)
                            {
                                continue;
                            }

                            __instance.SetPrimaryRampIndex(
                                runtimeLinkedEntity,
                                secondaryRampIndex,
                                false);
                        }
                    }

                    if (primaryRampIndex >= 0 &&
                        SkinLinksBySourceAssetId.TryGetValue(
                            currentAssetId,
                            out string[] skinLinkedAssetIds))
                    {
                        foreach (string linkedAssetId
                                 in skinLinkedAssetIds)
                        {
                            EquipmentEntity runtimeLinkedEntity =
                                FindRuntimeEntity(
                                    __instance,
                                    linkedAssetId);

                            if (runtimeLinkedEntity == null)
                            {
                                continue;
                            }

                            __instance.SetRampIndices(
                                runtimeLinkedEntity,
                                primaryRampIndex,
                                primaryRampIndex);
                        }
                    }

                    if (primaryRampIndex >= 0 &&
                        HairLinksBySourceAssetId.TryGetValue(
                            currentAssetId,
                            out string[] hairLinkedAssetIds))
                    {
                        foreach (string linkedAssetId
                                 in hairLinkedAssetIds)
                        {
                            EquipmentEntity runtimeLinkedEntity =
                                FindRuntimeEntity(
                                    __instance,
                                    linkedAssetId);

                            if (runtimeLinkedEntity == null)
                            {
                                continue;
                            }

                            __instance.SetPrimaryRampIndex(
                                runtimeLinkedEntity,
                                primaryRampIndex,
                                false);
                        }
                    }
                }
                finally
                {
                    ApplyingLinkedRamp = false;
                }
            }

            /*
             * Character creation already provides authoritative eye, skin,
             * and hair ramp values through DollState.
             */
            [HarmonyPatch(
                typeof(DollState),
                nameof(DollState.ApplyRamps))]
            [HarmonyPostfix]
            private static void ApplyRamps_Postfix(
                Character character,
                DollState __instance)
            {
                if (!Main.Settings.Races)
                {
                    return;
                }

                if (character == null || __instance == null)
                {
                    return;
                }

                BlueprintRace race = __instance.Race;

                if (__instance.EyesColorRampIndex >= 0 &&
                    EyeLinkEELs.TryGetValue(
                        race,
                        out EquipmentEntityLink[] eyeLinks))
                {
                    foreach (EquipmentEntityLink link in eyeLinks)
                    {
                        EquipmentEntity runtimeEntity =
                            LoadAndRegister(link);

                        if (runtimeEntity != null)
                        {
                            character.SetPrimaryRampIndex(
                                runtimeEntity,
                                __instance.EyesColorRampIndex,
                                false);
                        }
                    }
                }

                if (__instance.SkinRampIndex >= 0 &&
                    SkinLinkEELs.TryGetValue(
                        race,
                        out EquipmentEntityLink[] skinLinks))
                {
                    foreach (EquipmentEntityLink link in skinLinks)
                    {
                        EquipmentEntity runtimeEntity =
                            LoadAndRegister(link);

                        if (runtimeEntity != null)
                        {
                            character.SetPrimaryRampIndex(
                                runtimeEntity,
                                __instance.SkinRampIndex,
                                false);
                        }
                    }
                }

                if (__instance.HairRampIndex >= 0 &&
                    HairLinkEELs.TryGetValue(
                        race,
                        out EquipmentEntityLink[] hairLinks))
                {
                    foreach (EquipmentEntityLink link in hairLinks)
                    {
                        EquipmentEntity runtimeEntity =
                            LoadAndRegister(link);

                        if (runtimeEntity != null)
                        {
                            character.SetPrimaryRampIndex(
                                runtimeEntity,
                                __instance.HairRampIndex,
                                false);
                        }
                    }
                }
            }

            /*
             * If a linked entity is added after its source already has its
             * final ramp, apply that existing source ramp immediately.
             */
            [HarmonyPatch(
                typeof(Character),
                nameof(Character.AddEquipmentEntity),
                typeof(EquipmentEntity),
                typeof(bool),
                typeof(int),
                typeof(int))]
            [HarmonyPostfix]
            private static void AddEquipmentEntity_Postfix(
                Character __instance,
                EquipmentEntity ee)
            {
                if (!Main.Settings.Races)
                {
                    return;
                }

                if (ApplyingLinkedRamp ||
                    __instance == null ||
                    ee == null ||
                    !TryGetAssetId(ee, out string linkedAssetId))
                {
                    return;
                }

                try
                {
                    ApplyingLinkedRamp = true;

                    if (EyeSourcesByLinkedAssetId.TryGetValue(
                            linkedAssetId,
                            out string[] eyeSourceAssetIds) &&
                        TryGetSourceRamp(
                            __instance,
                            eyeSourceAssetIds,
                            useSecondaryRamp: true,
                            out int eyeRamp))
                    {
                        __instance.SetPrimaryRampIndex(
                            ee,
                            eyeRamp,
                            false);

                        return;
                    }

                    if (SkinSourcesByLinkedAssetId.TryGetValue(
                            linkedAssetId,
                            out string[] skinSourceAssetIds) &&
                        TryGetSourceRamp(
                            __instance,
                            skinSourceAssetIds,
                            useSecondaryRamp: false,
                            out int skinRamp))
                    {
                        __instance.SetRampIndices(
                            ee,
                            skinRamp,
                            skinRamp);

                        return;
                    }

                    if (HairSourcesByLinkedAssetId.TryGetValue(
                            linkedAssetId,
                            out string[] hairSourceAssetIds) &&
                        TryGetSourceRamp(
                            __instance,
                            hairSourceAssetIds,
                            useSecondaryRamp: false,
                            out int hairRamp))
                    {
                        __instance.SetPrimaryRampIndex(
                            ee,
                            hairRamp,
                            false);
                    }
                }
                finally
                {
                    ApplyingLinkedRamp = false;
                }
            }

            /*
             * This is retained from your original implementation for item
             * equipment that directly supplies one of the registered linked
             * EquipmentEntityLinks.
             */
            [HarmonyPatch(
                typeof(UnitEntityView),
                nameof(UnitEntityView.AddItemEquipment),
                typeof(ItemEntity),
                typeof(UnitEntityData),
                typeof(Character))]
            [HarmonyPostfix]
            private static void AddItemEquipment_Postfix(
                ItemEntity item,
                UnitEntityData unit,
                ref Character avatar)
            {
                if (!Main.Settings.Races)
                {
                    return;
                }

                if (item == null ||
                    unit == null ||
                    unit.IsPlayerFaction != true)
                {
                    return;
                }

                KingmakerEquipmentEntity itemEquipment =
                    item.EquipmentEntity;

                if (itemEquipment == null)
                {
                    return;
                }

                BlueprintRace race =
                    unit.Descriptor?.Progression?.Race;

                if (race == null ||
                    (!EyeLinkEELs.ContainsKey(race) &&
                     !SkinLinkEELs.ContainsKey(race) &&
                     !HairLinkEELs.ContainsKey(race)))
                {
                    return;
                }

                Character character = avatar.Or(null);

                if (character == null)
                {
                    return;
                }

                Kingmaker.Blueprints.Gender gender = unit.Gender;
                Kingmaker.Blueprints.Race actualRace =
                    UnitEntityView.GetActualRace(unit);

                int eyeRamp = -1;
                int skinRamp = -1;
                int hairRamp = -1;

                EquipmentEntityLink[] raceHeadLinks =
                    gender == Kingmaker.Blueprints.Gender.Male
                        ? race.MaleOptions.Heads
                        : race.FemaleOptions.Heads;

                EquipmentEntityLink[] raceHairLinks =
                    gender == Kingmaker.Blueprints.Gender.Male
                        ? race.MaleOptions.Hair
                        : race.FemaleOptions.Hair;

                string[] raceHeadAssetIds =
                    (raceHeadLinks ??
                     Array.Empty<EquipmentEntityLink>())
                    .Where(link =>
                        link != null &&
                        !string.IsNullOrEmpty(NormalizeAssetId(link.AssetId.ToString())))
                    .Select(link => NormalizeAssetId(link.AssetId.ToString()))
                    .ToArray();

                string[] raceHairAssetIds =
                    (raceHairLinks ??
                     Array.Empty<EquipmentEntityLink>())
                    .Where(link =>
                        link != null &&
                        !string.IsNullOrEmpty(NormalizeAssetId(link.AssetId.ToString())))
                    .Select(link => NormalizeAssetId(link.AssetId.ToString()))
                    .ToArray();

                Character.SelectedRampIndices headIndices =
                    FindRampIndices(
                        character,
                        raceHeadAssetIds);

                if (headIndices != null)
                {
                    skinRamp = headIndices.PrimaryIndex;
                    eyeRamp = headIndices.SecondaryIndex;
                }

                Character.SelectedRampIndices hairIndices =
                    FindRampIndices(
                        character,
                        raceHairAssetIds);

                if (hairIndices != null)
                {
                    hairRamp = hairIndices.PrimaryIndex;
                }

                HashSet<string> registeredEyeIds =
                    GetRegisteredAssetIds(
                        EyeLinkEELs,
                        race);

                HashSet<string> registeredSkinIds =
                    GetRegisteredAssetIds(
                        SkinLinkEELs,
                        race);

                HashSet<string> registeredHairIds =
                    GetRegisteredAssetIds(
                        HairLinkEELs,
                        race);

                foreach (EquipmentEntityLink equipmentEntityLink
                         in itemEquipment.GetLinks(gender, actualRace))
                {
                    if (equipmentEntityLink == null ||
                        string.IsNullOrEmpty(
                             NormalizeAssetId(
                                equipmentEntityLink.AssetId.ToString())))
                    {
                        continue;
                    }

                    LoadAndRegister(equipmentEntityLink);

                    string assetId =
                        NormalizeAssetId(
                            equipmentEntityLink.AssetId.ToString());

                    if (eyeRamp >= 0 &&
                        registeredEyeIds.Contains(assetId))
                    {
                        character.RemoveEquipmentEntity(
                            equipmentEntityLink);

                        character.AddEquipmentEntity(
                            equipmentEntityLink,
                            false,
                            eyeRamp);
                    }

                    if (skinRamp >= 0 &&
                        registeredSkinIds.Contains(assetId))
                    {
                        character.RemoveEquipmentEntity(
                            equipmentEntityLink);

                        character.AddEquipmentEntity(
                            equipmentEntityLink,
                            false,
                            skinRamp);
                    }

                    if (hairRamp >= 0 &&
                        registeredHairIds.Contains(assetId))
                    {
                        character.RemoveEquipmentEntity(
                            equipmentEntityLink);

                        character.AddEquipmentEntity(
                            equipmentEntityLink,
                            false,
                            hairRamp);
                    }
                }
            }
        }

        private static HashSet<string> GetRegisteredAssetIds(
            Dictionary<BlueprintRace, EquipmentEntityLink[]> dictionary,
            BlueprintRace race)
        {
            if (race == null ||
                !dictionary.TryGetValue(
                    race,
                    out EquipmentEntityLink[] links))
            {
                return new HashSet<string>(
                    StringComparer.OrdinalIgnoreCase);
            }

            return new HashSet<string>(
                (links ?? Array.Empty<EquipmentEntityLink>())
                .Where(link =>
                    link != null &&
                    !string.IsNullOrEmpty(NormalizeAssetId(link.AssetId.ToString())))
                .Select(link => NormalizeAssetId(link.AssetId.ToString())),
                StringComparer.OrdinalIgnoreCase);
        }
    }
}