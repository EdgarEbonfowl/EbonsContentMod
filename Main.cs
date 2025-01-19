using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using UnityModManagerNet;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.JsonSystem;
using EbonsContentMod.Abilities;
using EbonsContentMod.Feats;
using EbonsContentMod.Archetypes;
using EbonsContentMod.Menu;
using System.Globalization;
using TabletopTweaks.Core.Utilities;
using BlueprintCore.Blueprints.Configurators.Root;
using EbonsContentMod.Races;
using EbonsContentMod.Bloodlines;
using EbonsContentMod.Traits;
using EbonsContentMod.Races.Skinwalkers;
using EbonsContentMod.Utilities;
using Kingmaker.BundlesLoading;
using Kingmaker.Modding;
using Kingmaker.ResourceLinks;
using Kingmaker.SharedTypes;
using System.IO;
using UnityEngine;

namespace EbonsContentMod;

#if DEBUG
[EnableReloading]
#endif
public static class Main
{
    internal const string Toggle1 = "Toggle1";

    internal static Harmony HarmonyInstance;
    internal static UnityModManager.ModEntry.ModLogger log;

    public static bool Load(UnityModManager.ModEntry modEntry)
    {
        log = modEntry.Logger;
#if DEBUG
        modEntry.OnUnload = OnUnload;
#endif
        modEntry.OnGUI = OnGUI;
        Main.ModEntry = modEntry;
        HarmonyInstance = new Harmony(modEntry.Info.Id);
        HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        CreateAssetLinks.LoadAllSettings();
        return true;
    }

    public static void OnGUI(UnityModManager.ModEntry modEntry)
    {

    }

#if DEBUG
    public static bool OnUnload(UnityModManager.ModEntry modEntry)
    {
        HarmonyInstance.UnpatchAll(modEntry.Info.Id);
        return true;
    }

    [HarmonyPatch]
    public static class AssetHandler
    {
        private static Dictionary<string, Shader> shadersByName;
        private static Dictionary<string, Material> materialsByName;

        // Search the Bundles sub-folder in the mod's install folder for the bundle/s.
        [HarmonyPatch(typeof(OwlcatModificationsManager), nameof(OwlcatModificationsManager.TryLoadBundle)), HarmonyPrefix]
        public static bool TryLoadBundle(string bundleName, ref AssetBundle __result)
        {
            if (CreateAssetLinks.Bundles.Contains(bundleName))
            {
                log.Log($"Main.TryLoadBundle: Loading bundle {bundleName}");

                //__result = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Bundles", $"{bundleName}"));
                __result = AssetBundle.LoadFromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"{bundleName}"));

                // Since imported shaders are broken, swap the shaders in the bundle with a donor vanilla one.
                EquipmentEntityLink DonorHead = new EquipmentEntityLink { AssetId = "4eea3ef5f2e01474ba5b03fe28324ad3" };
                log.Log($"Main.TryLoadBundle: Harvesting vanilla donor head ee_head01_m_hm, AssetID {DonorHead.AssetId}");

                if (shadersByName == null)
                {
                    shadersByName = new();
                    shadersByName["Owlcat/Lit"] = DonorHead.Load(false).BodyParts[0].Material.shader;
                }

                log.Log($"Main.TryLoadBundle: Donor material = {DonorHead.Load(false).BodyParts[0].Material.name}, shader = {DonorHead.Load(false).BodyParts[0].Material.shader.name}");

                var materialCollection = __result.LoadAllAssets<OwlcatModificationMaterialsInBundleAsset>();
                log.Log($"Main.TryLoadBundle: Loading bundle MaterialsInBundle list {materialCollection}");

                try
                {
                    if (materialCollection != null)
                    {
                        log.Log($"Main.TryLoadBundle: MaterialsInBundle length = {materialCollection.Length}");
                        foreach (var entry in materialCollection)
                        {
                            for (int i = 0; i < entry.Materials.Length; i++)
                            {
                                var material = entry.Materials[i];
                                log.Log($"Main.TryLoadBundle: Fixing material {i + 1}, {material.name}");

                                if (material == null)
                                {
                                    log.Log("Main.TryLoadBundle: Null material, probably stale asset, skipping");
                                    continue;
                                }

                                if (material.shader != null && shadersByName.TryGetValue(material.shader.name, out var replacement))
                                {
                                    log.Log("Main.TryLoadBundle: Attempting to replace bundle shader with donor shader");
                                    material.shader = replacement;
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    log.Log($"Caught an exception trying to replace bundle material's shader!\n{e}");
                }

                return false;
            }
            return true;
        }

        // Map each asset to a bundle.
        [HarmonyPatch(typeof(OwlcatModificationsManager), nameof(OwlcatModificationsManager.GetBundleNameForAsset)), HarmonyPrefix]
        public static bool GetBundleNameForAsset(string guid, ref string __result)
        {
            if (CreateAssetLinks.AssetsInBundles.TryGetValue(guid, out var bundle))
            {
                log.Log($"Main.GetBundleNameForAsset: Redirecting asset with GUID {guid} to AssetBundle {bundle}");
                __result = bundle;
                return false;
            }
            return true;
        }

        [HarmonyPatch(typeof(OwlcatModificationsManager), nameof(OwlcatModificationsManager.GetDependenciesForBundle)), HarmonyPrefix]
        public static bool GetDependenciesForBundle(string bundleName, ref DependencyData __result)
        {
            if (CreateAssetLinks.Bundles.Contains(bundleName))
            {
                __result = null;
                return false;
            }

            return true;
        }
    }

    [HarmonyPatch(typeof(AssetBundle))]
    public static class AssetPatcher
    {
        public static Dictionary<string, Action<UnityEngine.Object>> LoadActions = new();

        [HarmonyPatch(nameof(AssetBundle.LoadAsset), typeof(string), typeof(Type)), HarmonyPostfix]
        public static void LoadAsset(string name, ref UnityEngine.Object __result)
        {
            if (LoadActions.TryGetValue(name, out var action))
            {
                log.Log($"Main.LoadAsset: Patching asset {name} on load");
                action(__result);
            }
        }
    }

#endif
    [HarmonyPatch(typeof(BlueprintsCache))]
    public static class BlueprintsCaches_Patch
    {
        private static bool Initialized = false;

        [HarmonyPriority(Priority.First)]
        [HarmonyPatch(nameof(BlueprintsCache.Init)), HarmonyPostfix]
        public static void Init_Postfix()
        {
            
            /*ModMenu.ModMenu.AddSettings(
                SettingsBuilder.New("EbonsContentModSettings", ModMenuHelpers.CreateString("title", "Ebon's Content Mod"))
                  .AddToggle(
                    Toggle.New("BaneOfSpirit", defaultValue: true, ModMenuHelpers.CreateString("BaneOfSpiritSetting", "Makes Bane of Spirit ability a free action again"))
                      .ShowVisualConnection())
                  .AddToggle(
                    Toggle.New("ComeAndGetMe", defaultValue: true, ModMenuHelpers.CreateString("ComeAndGetMeSetting", "Fixes Come and Get Me! to apply to teammates affected by Inspire Rage"))
                      .ShowVisualConnection())
                  .AddToggle(
                    Toggle.New("CriticalRangeRevert", defaultValue: false, ModMenuHelpers.CreateString("CriticalRangeRevertSetting", "Makes Improved Critical double all sources of threat range extension again"))
                      .ShowVisualConnection())
                  .AddToggle(
                    Toggle.New("DiscordantVoice", defaultValue: true, ModMenuHelpers.CreateString("DiscordantVoiceSetting", "Discordant Voice now applies to any teammate in the aoe of ANY bard song"))
                      .ShowVisualConnection())

            );*/

            try
            {
                if (Initialized)
                {
                    log.Log("Already initialized blueprints cache.");
                    return;
                }
                Initialized = true;

                log.Log("Patching blueprints.");

                ComeAndGetMe.Configure();
                DiscordantVoice.Configure();
                HungryGhostMonk.Configure();
                FlamboyantArcana.Configure();
                ArcaneDeed.Configure();
                LightSensitivityTrait.Configure();
            }
            catch (Exception e)
            {
                log.Log(string.Concat("Failed to initialize.", e));
            }
        }
    }

    [HarmonyPatch(typeof(StartGameLoader))]
    static class StartGameLoader_Patch
    {
        private static bool Initialized = false;

        [HarmonyPatch(nameof(StartGameLoader.LoadPackTOC)), HarmonyPostfix]
        static void LoadPackTOC()
        {
            try
            {
                if (Initialized)
                {
                    log.Log("Already configured delayed blueprints.");
                    return;
                }
                Initialized = true;

                RootConfigurator.ConfigureDelayedBlueprints();
            }
            catch (Exception e)
            {
                log.Log(string.Concat("Failed to configure delayed blueprints.", e));
            }
        }

        [HarmonyAfter("DarkCodex", "ExpandedContent", "PrestigePlus", "MysticalMayhem", "CharacterOptionsPlus", "TabletopTweaks-Base", "MicroscopicContentExpansion", "WOTR_MAKING_FRIENDS", "DP_WOTR_PlayableRaceExp")]
        [HarmonyPatch(nameof(StartGameLoader.LoadAllJson)), HarmonyPostfix]
        static void HandleOtherMods()
        {
            try
            {
                EldritchScrapper.Configure();
                CollegiateInitiate.Configure(); // Not compatible with Worldcrawl yet
                OrcSorcererBloodline.Configure();
                Samsaran.Configure();
                Svirfneblin.Configure();
                Duergar.Configure();
                Sylph.Configure();
                Undine.Configure();
                Strix.Configure();
                Drow.Configure();
                Orc.Configure();
                Suli.Configure();
                Android.Configure();
                Ifrit.Configure();
                Fetchling.Configure();
                Changeling.Configure();
                Skinwalker.Configure();
                Goblin.Configure();
                Kuru.Configure();
                Vishkanya.Configure();
                Shabti.Configure();
                Rougarou.Configure();
                Nagaji.Configure();
                Mongrel.Configure();
                MultiProjectileSpellFix.Configure();
                ArcanistExploits.Configure();
                FaithMagic.Configure();
                GarbageBin.Configure();
            }
            catch (Exception e)
            {
                log.Log(string.Concat("Failed to handle other mods", e));
            }
        }
    }

    public static UnityModManager.ModEntry ModEntry;
}
