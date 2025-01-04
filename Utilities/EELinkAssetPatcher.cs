using System;
using System.Collections.Generic;
using HarmonyLib;
using Kingmaker.BundlesLoading;
using Kingmaker.ResourceLinks;
using Kingmaker.Visual.CharacterSystem;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using System.IO;
using System.Reflection;
using BlueprintCore.Utils.Assets;
using System.Linq;

namespace EbonsContentMod.Utilities
{
    internal class EELinkAssetPatcher
    {
        private static readonly string AssemblyName = Assembly.GetAssembly(typeof(AssetTool)).GetName().Name.ToLower();
        private static readonly string BundleName =
          Path.Combine(Path.GetDirectoryName(Assembly.GetAssembly(typeof(AssetTool)).Location), $"{AssemblyName}_assets");

        private static readonly HashSet<string> AllAssetIds = new();

        private static readonly Dictionary<string, (EquipmentEntityLink source, Action<EquipmentEntity> init)> DynamicPrefabLinks =
          new();
        
        private static EquipmentEntity Clone(EquipmentEntity ee)
        {
            EquipmentEntity ret = (EquipmentEntity)ScriptableObject.CreateInstance(typeof(EquipmentEntity));
            ret.BakedTextures = Helpers.CreateCopy<CharacterBakedTextures>(ee.BakedTextures, null);
            ret.BodyParts = Helpers.CreateCopy<List<BodyPart>>(ee.BodyParts, null);
            ret.CantBeHiddenByDollRoom = ee.CantBeHiddenByDollRoom;
            ret.ColorPresets = ee.ColorPresets;
            ret.IsExportEnabled = ee.IsExportEnabled;
            ret.Layer = ee.Layer;
            ret.m_BonesByName = ee.m_BonesByName;
            ret.m_DlcReward = ee.m_DlcReward;
            ret.m_HideBodyParts = ee.m_HideBodyParts;
            ret.m_IsDirty = ee.m_IsDirty;
            ret.m_PrimaryRamps = new List<Texture2D>();
            ret.m_SecondaryRamps = ee.m_SecondaryRamps;
            ret.m_SpecialPrimaryRamps = ee.m_SpecialPrimaryRamps;
            ret.m_SpecialSecondaryRamps = ee.m_SpecialSecondaryRamps;
            ret.OutfitParts = Helpers.CreateCopy<List<EquipmentEntity.OutfitPart>>(ee.OutfitParts, null);
            ret.PrimaryColorsAvailableForPlayer = ee.PrimaryColorsAvailableForPlayer;
            ret.PrimaryColorsProfile = new CharacterColorsProfile();
            ret.PrimaryColorsProfile.Ramps = ee.PrimaryColorsProfile.Ramps;
            ret.PrimaryColorsProfile.RampDlcLocks = ee.PrimaryColorsProfile.RampDlcLocks;
            ret.PrimaryColorsProfile.SpecialRamps = ee.PrimaryColorsProfile.SpecialRamps;
            ret.SecondaryColorsProfile = ee.SecondaryColorsProfile;
            ret.SecondaryColorsAvailableForPlayer = ee.SecondaryColorsAvailableForPlayer;
            ret.ShowLowerMaterials = ee.ShowLowerMaterials;
            ret.SkeletonModifiers = ee.SkeletonModifiers;
            return ret;
        }
        
        /// <summary>
        /// Registers <paramref name="assetId"/> as an asset. When the asset is requested for load,
        /// <paramref name="source"/> is loaded and copied, <paramref name="init"/> is called on the copy, and the copy is
        /// returned.
        /// </summary>
        /// 
        /// <remarks>
        /// Use this to create a new asset based on a base game asset dynamically. You configure the <c>GameObject</c> in
        /// <paramref name="init"/> and it will be used whenever an asset with <paramref name="assetId"/> is requested.
        /// 
        /// <example>
        /// <code>
        /// var assetId = "0a2f44a9-0650-452f-8ff6-24fd4e61c7ef"; // New GUID identifying your prefab
        /// var sourceAssetId = "fd21d914e9f6f5e4faa77365549ad0a7"; // A 20-ft ground ice effect from the base game
        /// 
        /// // When assetId is requested: load sourceAssetId, copy it, then set its scale to 50%
        /// RegisterDynamicPrefabLink(assetId, sourceAssetId, prefab => prefab.transform.localScale = new(0.5f, 0.5f));
        /// 
        /// // Creates an AOE using the new asset
        /// AbilityAreaEffectConfigurator.New(AreaEffectName, AreaEffectGuid).SetFx(assetId).Configure();
        /// </code>
        /// </example>
        /// </remarks>
        /// <param name="assetId">Unique identifier used to load the asset</param>
        /// <param name="source">The base game prefab copied to create your prefab.</param>
        /// <param name="init">Function called passing in a copy of the source prefab. Use this to configure your prefab.</param>
        /// <exception cref="ArgumentException">If <paramref name="assetId"/> is already in use</exception>
        public static void RegisterDynamicEquipmentEntityLink(string assetId, EquipmentEntityLink source, Action<EquipmentEntity> init)
        {
            if (DynamicPrefabLinks.ContainsKey(assetId) || AllAssetIds.Contains(assetId))
                throw new ArgumentException($"{assetId} is already in use!");

            DynamicPrefabLinks[assetId] = (source, init);
            AllAssetIds.Add(assetId);
        }

        private static readonly HashSet<string> AssetIds = new();
        private static void LoadAssets(string assetFile)
        {
            if (!File.Exists(assetFile))
            {
                return;
            }

            var bundle = BundlesLoadService.Instance.RequestBundle(assetFile);
            foreach (var assetId in bundle.GetAllAssetNames())
            {
                AllAssetIds.Add(assetId);
                AssetIds.Add(assetId);
            }
        }

        [HarmonyPatch(typeof(AssetBundle))]
        static class AssetBundle_Patch
        {
            [HarmonyPatch(nameof(AssetBundle.LoadAsset), typeof(string), typeof(Type)), HarmonyPrefix]
            static bool LoadAsset(string name, ref UnityEngine.Object __result)
            {
                try
                {
                    if (DynamicPrefabLinks.ContainsKey(name))
                    {
                        (var source, var init) = DynamicPrefabLinks[name];
                        __result = Create(source.Load(), init);
                        return false;
                    }
                }
                catch (Exception e)
                {
                }
                return true;
            }

            private static EquipmentEntity Create(EquipmentEntity sourceObject, Action<EquipmentEntity> init)
            {
                var copyObj = Clone(sourceObject); // make this a deep copy!
                UnityEngine.Object.DontDestroyOnLoad(copyObj);
                init(copyObj);
                return copyObj;
            }
        }

        [HarmonyPatch(typeof(BundlesLoadService))]
        static class BundlesLoadService_Patch
        {
            [HarmonyPatch(nameof(BundlesLoadService.GetBundleNameForAsset)), HarmonyPrefix]
            static bool Prefix(string assetId, ref string __result, BundlesLoadService __instance)
            {
                try
                {
                    if (AssetIds.Contains(assetId))
                    {
                        __result = BundleName;
                        return false;
                    }

                    if (DynamicPrefabLinks.ContainsKey(assetId))
                    {
                        (var source, _) = DynamicPrefabLinks[assetId];
                        __result = __instance.GetBundleNameForAsset(source.AssetId);
                        return false;
                    }
                }
                catch (Exception e)
                {
                }
                return true;
            }
        }

        [HarmonyPatch(typeof(BlueprintsCache))]
        static class BlueprintsCaches_Patch
        {
            private static bool Initialized = false;

            [HarmonyPriority(Priority.First)]
            [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPrefix]
            static void Prefix()
            {
                try
                {
                    if (Initialized)
                    {
                        return;
                    }
                    Initialized = true;

                    LoadAssets(BundleName);
                }
                catch (Exception e)
                {
                }
            }
        }
    }

    /// <summary>
    /// Wrapper for flexible reference of WeakResourceLink types.
    /// </summary>
    public class AssetLink<TLink> where TLink : WeakResourceLink, new()
    {
        private readonly TLink Link;

        private AssetLink(string assetId)
        {
            Link = new() { AssetId = assetId };
        }

        private AssetLink(TLink link)
        {
            Link = link;
        }

        public TLink Get()
        {
            return Link;
        }

        public static implicit operator AssetLink<TLink>(TLink link)
        {
            return new(link);
        }

        public static implicit operator AssetLink<TLink>(string assetId)
        {
            return new(assetId);
        }
    }

    /// <summary>
    /// Wrapper for flexible reference of asset types.
    /// </summary>
    public class Asset<T> where T : UnityEngine.Object
    {
        private readonly T? Value;
        private readonly string AssetId;

        private Asset(string assetId)
        {
            AssetId = assetId;
            Value = ResourcesLibrary.TryGetResource<T>(assetId);
        }

        private Asset(T asset)
        {
            AssetId = asset.name;
            Value = asset;
        }

        public T Get()
        {
            if (Value is null)
            {
                throw new InvalidOperationException($"Unable to fetch asset w/ ID {AssetId}");
            }
            return Value;
        }

        public static implicit operator Asset<T>(T asset)
        {
            return asset is null ? null : new(asset);
        }

        public static implicit operator Asset<T>(string assetId)
        {
            return new(assetId);
        }
    }
}
