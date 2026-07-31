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
using EbonsContentMod.WildTalents;
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
    internal static Settings Settings;
    public static UnityModManager.ModEntry ModEntry;

    internal static Harmony HarmonyInstance;
    internal static UnityModManager.ModEntry.ModLogger log;
    internal static string ModPath;

    public static bool Load(UnityModManager.ModEntry modEntry)
    {
        ModEntry = modEntry;
        ModPath = modEntry.Path;
        log = modEntry.Logger;

        Settings = UnityModManager.ModSettings.Load<Settings>(modEntry);

#if DEBUG
        modEntry.OnUnload = OnUnload;
#endif

        modEntry.OnGUI = OnGUI;
        modEntry.OnSaveGUI = OnSaveGUI;

        HarmonyInstance = new Harmony(modEntry.Info.Id);
        HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());

        CreateAssetLinks.LoadAllSettings();

        return true;
    }

    public static void OnGUI(UnityModManager.ModEntry modEntry)
    {
        GUILayout.Label(
            "Blueprint settings are applied when the game starts. " +
            "Restart the game after changing them. " +
            "Note: If the game was saved with settings enabled, disabling them may break saves");

        GUILayout.Space(10);

        GUILayout.Label("<b>Races</b>");

        Settings.Races = GUILayout.Toggle(
            Settings.Races,
            "Enable added races");

        GUILayout.Space(10);

        GUILayout.Label("<b>Portraits for New Races</b>");

        Settings.Portraits = GUILayout.Toggle(
            Settings.Portraits,
            "Enable portraits for added races");

        GUILayout.Space(10);

        GUILayout.Label("<b>Archetypes</b>");

        Settings.HungryGhostMonk = GUILayout.Toggle(
            Settings.HungryGhostMonk,
            "Enable Hungry Ghost Monk archetype");

        Settings.CollegiateInitiate = GUILayout.Toggle(
            Settings.CollegiateInitiate,
            "Enable Collegiate Initiate archetype");

        Settings.EldritchScrapper = GUILayout.Toggle(
            Settings.EldritchScrapper,
            "Enable Eldritch Scrapper archetype");

        GUILayout.Space(10);

        GUILayout.Label("<b>New Features and Abilities</b>");

        Settings.ArcaneDeed = GUILayout.Toggle(
            Settings.ArcaneDeed,
            "Enable Arcane Deed magus arcana");

        Settings.ArcanistExploits = GUILayout.Toggle(
            Settings.ArcanistExploits,
            "Enable Arcanist Exploits");

        Settings.Bloodlines = GUILayout.Toggle(
            Settings.Bloodlines,
            "Enable added bloodlines and bloodline mutations");

        Settings.FaithMagic = GUILayout.Toggle(
            Settings.FaithMagic,
            "Enable Faith Magic arcane discovery");

        Settings.FlamboyantArcana = GUILayout.Toggle(
            Settings.FlamboyantArcana,
            "Enable Flamboyant Arcana magus arcana");

        GUILayout.Space(10);

        GUILayout.Label("<b>New Wild Talents</b>");

        Settings.AirsLeap = GUILayout.Toggle(
            Settings.AirsLeap,
            "Enable Air's Leap utility wild talent");

        Settings.ClockworkHeart = GUILayout.Toggle(
            Settings.ClockworkHeart,
            "Enable Clockwork Heart utility wild talent");

        Settings.KineticForm = GUILayout.Toggle(
            Settings.KineticForm,
            "Enable Kinetic Form utility wild talent");

        Settings.SparkOfInnovation = GUILayout.Toggle(
            Settings.SparkOfInnovation,
            "Enable Spark Of Innovation utility wild talent");

        Settings.SparkOfLife = GUILayout.Toggle(
            Settings.SparkOfLife,
            "Enable Spark Of Life utility wild talent");

        Settings.WingsOfAir = GUILayout.Toggle(
            Settings.WingsOfAir,
            "Enable Wings Of Air utility wild talent");

        GUILayout.Space(10);

        GUILayout.Label("<b>Mechanics Fixes</b>");

        Settings.ComeAndGetMe = GUILayout.Toggle(
            Settings.ComeAndGetMe,
            "Enable Come and Get Me! fix");

        Settings.DiscordantVoice = GUILayout.Toggle(
            Settings.DiscordantVoice,
            "Enable Discordant Voice fix");

        Settings.MultiProjectileSpellFix = GUILayout.Toggle(
            Settings.MultiProjectileSpellFix,
            "Enable multi-projectile spell fix");
    }

    public static void OnSaveGUI(UnityModManager.ModEntry modEntry)
    {
        Settings.Save(modEntry);
    }

#if DEBUG
    public static bool OnUnload(UnityModManager.ModEntry modEntry)
    {
        HarmonyInstance.UnpatchAll(modEntry.Info.Id);
        return true;
    }

    private static void ConfigureRaces()
    {
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
        Ganzi.Configure();
        AscendingSuccubus.Configure();
        Hobgoblin.Configure();
        AquaticElf.Configure();
        Aphorite.Configure();
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

                if (Settings.ComeAndGetMe)
                {
                    ComeAndGetMe.Configure();
                }

                if (Settings.DiscordantVoice)
                {
                    DiscordantVoice.Configure();
                }

                if (Settings.HungryGhostMonk)
                {
                    HungryGhostMonk.Configure();
                }

                if (Settings.FlamboyantArcana)
                {
                    FlamboyantArcana.Configure();
                }

                if (Settings.ArcaneDeed)
                {
                    ArcaneDeed.Configure();
                }

                LightSensitivityTrait.Configure();

                if (Settings.KineticForm)
                {
                    KineticForm.Configure();
                }

                if (Settings.ClockworkHeart)
                {
                    ClockworkHeart.Configure();
                }

                if (Settings.AirsLeap)
                {
                    AirsLeap.Configure();
                }

                if (Settings.WingsOfAir)
                {
                    WingsOfAir.Configure();
                }

                if (Settings.SparkOfLife)
                {
                    SparkOfLife.Configure();
                }

                if (Settings.SparkOfInnovation)
                {
                    SparkOfInnovation.Configure();
                }               
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
                if (Settings.EldritchScrapper)
                {
                    EldritchScrapper.Configure();
                }

                if (Settings.CollegiateInitiate)
                {
                    CollegiateInitiate.Configure(); // Not compatible with Worldcrawl yet
                }

                if (Settings.Bloodlines)
                {
                    ConfigureBloodlines.Configure();
                }

                if (Settings.Races)
                {
                    ConfigureRaces();
                }

                if (Settings.MultiProjectileSpellFix)
                {
                    MultiProjectileSpellFix.Configure();
                }

                if (Settings.ArcanistExploits)
                {
                    ArcanistExploits.Configure();
                }

                if (Settings.FaithMagic)
                {
                    FaithMagic.Configure();
                }

                GarbageBin.Configure();
            }
            catch (Exception e)
            {
                log.Log(string.Concat("Failed to handle other mods", e));
            }
        }
    }
}
