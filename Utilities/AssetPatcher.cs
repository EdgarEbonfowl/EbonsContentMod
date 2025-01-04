using System;
using System.Collections.Generic;

using HarmonyLib;

using Kingmaker.BundlesLoading;
using Kingmaker.ResourceLinks;
using Kingmaker.Visual.CharacterSystem;
using TabletopTweaks.Core.Utilities;
using UnityEngine;


namespace EbonsContentMod.Utilities
{
    internal static class Dynamic
    {
        /// <exclude />
        private interface IDynamicAssetLink
        {
            WeakResourceLink Link { get; }
            Func<UnityEngine.Object, UnityEngine.Object> Init { get; }
            UnityEngine.Object CreateObject();
            Type AssetType { get; }
            Type LinkType { get; }
        }

        /// <exclude />
        private abstract class DynamicAssetLink<T, TLink> : IDynamicAssetLink
            where T : UnityEngine.Object
            where TLink : WeakResourceLink<T>, new()
        {
            public virtual Type AssetType => typeof(T);
            public virtual Type LinkType => typeof(TLink);

            public virtual TLink Link { get; }

            WeakResourceLink IDynamicAssetLink.Link => Link;
            public virtual Func<T, T> Init { get; }
            Func<UnityEngine.Object, UnityEngine.Object> IDynamicAssetLink.Init => obj =>
            {
                if (obj is not T t)
                    throw new InvalidCastException();

                return Init(t);
            };

            public DynamicAssetLink(TLink assetLink, Func<T, T> init)
            {
                Init = init;
                Link = assetLink;
            }

            protected abstract T CloneObject(T obj);

            public virtual UnityEngine.Object CreateObject()
            {
                if (Link.LoadObject() is not T obj)
                    throw new Exception($"Failed to instantiate asset from {Link.AssetId}");

                var copy = CloneObject(obj);
                return Init(copy);
            }
        }

        /// <exclude />
        private class DynamicGameObjectLink<TLink> : DynamicAssetLink<GameObject, TLink>
            where TLink : WeakResourceLink<GameObject>, new()
        {
            protected override GameObject CloneObject(GameObject obj)
            {
                var copy = GameObject.Instantiate(obj);

                UnityEngine.Object.DontDestroyOnLoad(copy);

                return copy;
            }

            public DynamicGameObjectLink(TLink link, Func<GameObject, GameObject> init) : base(link, init) { }
        }

        /// <exclude />
        private class DynamicMonobehaviourLink<T, TLink> : DynamicAssetLink<T, TLink>
            where T : MonoBehaviour
            where TLink : WeakResourceLink<T>, new()
        {
            protected override T CloneObject(T obj)
            {
                var copy = GameObject.Instantiate(obj.gameObject);
                copy.SetActive(false);

                UnityEngine.Object.DontDestroyOnLoad(copy);

                var component = copy.GetComponent<T>();

                return component;
            }

            public DynamicMonobehaviourLink(TLink link, Func<T, T> init) : base(link, init) { }
        }

        /// <exclude />
        private static readonly Dictionary<string, IDynamicAssetLink> DynamicAssetLinks = new();

        /// <exclude />
        private static TLink CreateDynamicAssetLinkProxy<TLink>(IDynamicAssetLink proxy, string? assetId = null)
            where TLink : WeakResourceLink, new()
        {
            if (string.IsNullOrEmpty(assetId))
                assetId = null;

            assetId ??= Guid.NewGuid().ToString("N").ToLowerInvariant();

            DynamicAssetLinks.Add(assetId, proxy);

            return new() { AssetId = assetId };
        }

        /// <summary>
        /// Creates a dynamic proxy for a <see cref="WeakResourceLink{GameObject}"/> (eg. PrefabLink)
        /// </summary>
        /// <param name="link">A <typeparamref name="TLink"/> link.</param>
        /// <param name="init">Initialization function to be executed on asset load.</param>
        /// <param name="assetId">Asset ID for the new link. Will be set to a new guid if absent or null.</param>
        public static TLink CreateDynamicProxy<TLink>(this TLink link, Func<GameObject, GameObject> init, string? assetId = null)
            where TLink : WeakResourceLink<GameObject>, new() =>
            CreateDynamicAssetLinkProxy<TLink>(new DynamicGameObjectLink<TLink>(link, init), assetId);

        /// <inheritdoc cref="CreateDynamicProxy{TLink}(TLink, Func{GameObject, GameObject}, string?)"/>
        /// <param name="link">A <typeparamref name="TLink"/> link.</param>
        /// <param name="init">Initialization function to be executed on asset load.</param>
        /// <param name="assetId">Asset ID for the new link. Will be set to a new guid if absent or null.</param>
        public static TLink CreateDynamicProxy<TLink>(this TLink link, Action<GameObject> init, string? assetId = null)
            where TLink : WeakResourceLink<GameObject>, new() =>
            CreateDynamicAssetLinkProxy<TLink>(new DynamicGameObjectLink<TLink>(link, go => { init(go); return go; }), assetId);

        /// <summary>
        /// Creates a dynamic proxy for a MonoBehaviour WeakResourceLink.
        /// This is for assets that are components rather than GameObjects.
        /// <list>
        ///     <item>
        ///         <term>FamiliarLink</term>
        ///         <description><typeparamref name="T"/> = Familiar</description>
        ///     </item>
        ///     <item>
        ///         <term>Link</term>
        ///         <description><typeparamref name="T"/> = TacticalMapObstacle</description>
        ///     </item>
        ///     <item>
        ///         <term>ProjectileLink</term>
        ///         <description><typeparamref name="T"/> = ProjectileView</description>
        ///     </item>
        ///     <item>
        ///         <term>UnitViewLink</term>
        ///         <description><typeparamref name="T"/> = UnitEntityView</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <typeparam name="T">Asset Type.</typeparam>
        /// <typeparam name="TLink">AssetLink Type.</typeparam>
        /// <param name="link">A <typeparamref name="TLink"/>link</param>.
        /// <param name="init">Initialization function to be executed on asset load.</param>
        /// <param name="assetId">Asset ID for the new link. Will be set to a new guid if absent or null.</param>
        /// <returns></returns>
        public static TLink CreateDynamicMonobehaviourProxy<T, TLink>(this TLink link, Func<T, T> init, string? assetId = null)
            where T : MonoBehaviour
            where TLink : WeakResourceLink<T>, new() =>
            CreateDynamicAssetLinkProxy<TLink>(new DynamicMonobehaviourLink<T, TLink>(link, init), assetId);

        /// <inheritdoc cref="CreateDynamicMonobehaviourProxy{T, TLink}(TLink, Func{T, T}, string?)" />
        /// <param name="link">A <typeparamref name="TLink"/>link</param>.
        /// <param name="init">Initialization function to be executed on asset load.</param>
        /// <param name="assetId">Asset ID for the new link. Will be set to a new guid if absent or null.</param>
        /// <returns></returns>
        public static TLink CreateDynamicMonobehaviourProxy<T, TLink>(this TLink link, Action<T> init, string? assetId = null)
            where T : MonoBehaviour
            where TLink : WeakResourceLink<T>, new() =>
            CreateDynamicAssetLinkProxy<TLink>(new DynamicMonobehaviourLink<T, TLink>(link, mb => { init(mb); return mb; }), assetId);


        private class DynamicScriptableObjectLink<T, TLink> : DynamicAssetLink<T, TLink>
            where T : ScriptableObject
            where TLink : WeakResourceLink<T>, new()
        {
            protected override T CloneObject(T obj)
            {
                //var copy = ScriptableObject.Instantiate(obj);

                var copy = Helpers.CreateCopy(obj);

                UnityEngine.Object.DontDestroyOnLoad(copy);

                return copy;
            }

            public DynamicScriptableObjectLink(TLink link, Func<T, T> init) : base(link, init) { }
        }

        public static TLink CreateDynamicScriptableObjectProxy<T, TLink>(this TLink link, Func<T, T> init, string? assetId = null)
            where T : ScriptableObject
            where TLink : WeakResourceLink<T>, new() =>
            CreateDynamicAssetLinkProxy<TLink>(new DynamicScriptableObjectLink<T, TLink>(link, init), assetId);

        public static TLink CreateDynamicScriptableObjectProxy<T, TLink>(this TLink link, Action<T> init, string? assetId = null)
            where T : ScriptableObject
            where TLink : WeakResourceLink<T>, new() =>
            CreateDynamicAssetLinkProxy<TLink>(new DynamicScriptableObjectLink<T, TLink>(link, so => { init(so); return so; }), assetId);

        private class DynamicScriptableObjectLinkAlt<T, TLink> : DynamicAssetLink<T, TLink>
            where T : ScriptableObject
            where TLink : WeakResourceLink<T>, new()
        {
            protected override T CloneObject(T obj)
            {
                var copy = ScriptableObject.Instantiate(obj);

                UnityEngine.Object.DontDestroyOnLoad(copy);

                return copy;
            }

            public DynamicScriptableObjectLinkAlt(TLink link, Func<T, T> init) : base(link, init) { }
        }

        public static TLink CreateDynamicScriptableObjectProxyAlt<T, TLink>(this TLink link, Func<T, T> init, string? assetId = null)
            where T : ScriptableObject
            where TLink : WeakResourceLink<T>, new() =>
            CreateDynamicAssetLinkProxy<TLink>(new DynamicScriptableObjectLinkAlt<T, TLink>(link, init), assetId);

        public static TLink CreateDynamicScriptableObjectProxyAlt<T, TLink>(this TLink link, Action<T> init, string? assetId = null)
            where T : ScriptableObject
            where TLink : WeakResourceLink<T>, new() =>
            CreateDynamicAssetLinkProxy<TLink>(new DynamicScriptableObjectLinkAlt<T, TLink>(link, so => { init(so); return so; }), assetId);


        private static EquipmentEntity CloneEE(EquipmentEntity ee)
        {
            EquipmentEntity ret = (EquipmentEntity)ScriptableObject.CreateInstance(typeof(EquipmentEntity));
            ret.BakedTextures = Helpers.CreateCopy<CharacterBakedTextures>(ee.BakedTextures, null);
            ret.BodyParts = Helpers.CreateCopy<List<BodyPart>>(ee.BodyParts, null);
            ret.CantBeHiddenByDollRoom = ee.CantBeHiddenByDollRoom;
            ret.ColorPresets = ee.ColorPresets;
            ret.IsExportEnabled = ee.IsExportEnabled;
            ret.Layer = ee.Layer;
            ret.PrimaryRamps = ee.PrimaryRamps;
            ret.m_BonesByName = ee.m_BonesByName;
            ret.m_DlcReward = ee.m_DlcReward;
            ret.m_HideBodyParts = ee.m_HideBodyParts;
            ret.m_IsDirty = ee.m_IsDirty;
            ret.m_PrimaryRamps = ee.PrimaryRamps;
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
            ret.name = ee.name;
            return ret;
        }

        private class DynamicEquipmentEntityLink<T, TLink> : DynamicAssetLink<T, TLink>
            where T : EquipmentEntity
            where TLink : WeakResourceLink<T>, new()
        {
            protected override T CloneObject(T obj)
            {
                //var copy = ScriptableObject.Instantiate(obj);

                var copy = CloneEE(obj);

                UnityEngine.Object.DontDestroyOnLoad(copy);

                return (T)copy;
            }

            public DynamicEquipmentEntityLink(TLink link, Func<T, T> init) : base(link, init) { }
        }

        public static TLink CreateDynamicEquipmentEntityProxy<T, TLink>(this TLink link, Func<T, T> init, string? assetId = null)
            where T : EquipmentEntity
            where TLink : WeakResourceLink<T>, new() =>
            CreateDynamicAssetLinkProxy<TLink>(new DynamicEquipmentEntityLink<T, TLink>(link, init), assetId);

        public static TLink CreateDynamicEquipmentEntityProxy<T, TLink>(this TLink link, Action<T> init, string? assetId = null)
            where T : EquipmentEntity
            where TLink : WeakResourceLink<T>, new() =>
            CreateDynamicAssetLinkProxy<TLink>(new DynamicEquipmentEntityLink<T, TLink>(link, so => { init(so); return so; }), assetId);


        [HarmonyPatch]
        static class Patches
        {
            [HarmonyPatch(typeof(AssetBundle), nameof(AssetBundle.LoadAsset), typeof(string), typeof(Type))]
            [HarmonyPrefix]
            static bool LoadAsset_Prefix(string name, ref UnityEngine.Object __result)
            {
                try
                {
                    if (DynamicAssetLinks.ContainsKey(name))
                    {
                        var assetProxy = DynamicAssetLinks[name];

                        var copy = assetProxy.CreateObject();

                        if (copy is MonoBehaviour mb)
                            __result = mb.gameObject;
                        else
                            __result = copy;

                        return false;
                    }
                }
                catch (Exception e)
                {
                }

                return true;
            }

            [HarmonyPatch(typeof(BundlesLoadService), nameof(BundlesLoadService.GetBundleNameForAsset))]
            [HarmonyPrefix]
            static bool GetBundleNameForAsset_Prefix(string assetId, ref string __result, BundlesLoadService __instance)
            {
                try
                {
                    if (DynamicAssetLinks.ContainsKey(assetId))
                    {
                        var assetProxy = DynamicAssetLinks[assetId];

                        __result = __instance.GetBundleNameForAsset(assetProxy.Link.AssetId);
                        return false;
                    }
                }
                catch (Exception e)
                {
                }
                return true;
            }
        }
    }
}