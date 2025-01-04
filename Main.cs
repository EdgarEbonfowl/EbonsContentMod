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
using EbonsContentMod.Equipment;
using EbonsContentMod.Menu;
using System.Globalization;
using TabletopTweaks.Core.Utilities;
using BlueprintCore.Blueprints.Configurators.Root;
using EbonsContentMod.Races;
using EbonsContentMod.Bloodlines;
using EbonsContentMod.Traits;

namespace EbonsContentMod;

#if DEBUG
[EnableReloading]
#endif
public static class Main
{
    internal static Harmony HarmonyInstance;
    internal static UnityModManager.ModEntry.ModLogger log;

    /*private static readonly string RootKey = "mod-menu.ebonscontentmod-settings";
    public static string GetKey(string partialKey)
    {
        return $"{RootKey}.{partialKey}";
    }*/

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

    /*private static void Onclick()
    {
        var logg = new StringBuilder();
        logg.AppendLine("Current settings: ");
        ///log.AppendLine($"-Toggle: {CheckToggle()}");
        logg.AppendLine($"-Default Slider Float: {ModMenu.ModMenu.GetSettingValue<float>(GetKey("float-default"))}");
        logg.AppendLine($"-Slider Float: {ModMenu.ModMenu.GetSettingValue<float>(GetKey("float"))}");
        logg.AppendLine($"-Default Slider Int: {ModMenu.ModMenu.GetSettingValue<int>(GetKey("int-default"))}");
        logg.AppendLine($"-Slider Int: {ModMenu.ModMenu.GetSettingValue<int>(GetKey("int"))}");
        log.Log(logg.ToString());
    }*/
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
                SettingsBuilder.New(RootKey, ModMenuHelpers.CreateString("title", "Ebon's Content Mod"))
                  .AddDefaultButton()
                  .AddButton(
                    Button.New(
                      ModMenuHelpers.CreateString("button-desc", "Restart the game to apply changes!"), ModMenuHelpers.CreateString("button-text", "Do Not Turn Any Chosen Features Off"), Onclick))
                  .AddToggle(
                    Toggle.New(GetKey("BaneOfSpirit"), defaultValue: true, ModMenuHelpers.CreateString("BaneOfSpiritSetting", "Makes Bane of Spirit ability a free action again"))
                      .ShowVisualConnection())
                  .AddToggle(
                    Toggle.New(GetKey("ComeAndGetMe"), defaultValue: true, ModMenuHelpers.CreateString("ComeAndGetMeSetting", "Fixes Come and Get Me! to apply to teammates affected by Inspire Rage"))
                      .ShowVisualConnection())
                  .AddToggle(
                    Toggle.New(GetKey("CriticalRangeRevert"), defaultValue: false, ModMenuHelpers.CreateString("CriticalRangeRevertSetting", "Makes Improved Critical double all sources of threat range extension again"))
                      .ShowVisualConnection())
                  .AddToggle(
                    Toggle.New(GetKey("DiscordantVoice"), defaultValue: true, ModMenuHelpers.CreateString("DiscordantVoiceSetting", "Discordant Voice now applies to any teammate in the aoe of ANY bard song"))
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
                // Insert your mod's patching methods here
                // Example
                /*if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("ComeAndGetMe")))*/
                ComeAndGetMe.Configure();
                /*if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("DiscordantVoice")))*/
                DiscordantVoice.Configure();
                HungryGhostMonk.Configure();
                /*if (ModMenu.ModMenu.GetSettingValue<bool>(GetKey("BaneOfSpirit")))*/
                BaneOfSpirit.Configure();
                ShatterDefenses.Configure();
                FlamboyantArcana.Configure();
                ArcaneDeed.Configure();
                LightSensitivityTrait.Configure();
                OrcSorcererBloodline.Configure();
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

        [HarmonyAfter("DarkCodex", "ExpandedContent", "PrestigePlus", "MysticalMayhem", "CharacterOptionsPlus", "TabletopTweaks-Base", "MicroscopicContentExpansion", "WOTR_MAKING_FRIENDS")]
        [HarmonyPatch(nameof(StartGameLoader.LoadAllJson)), HarmonyPostfix]
        static void HandleOtherMods()
        {
            try
            {
                EldritchScrapper.Configure();
                CollegiateInitiate.Configure();
                Samsaran.Configure();
                Svirfneblin.Configure();
                Duergar.Configure();
            }
            catch (Exception e)
            {
                log.Log(string.Concat("Failed to handle other mods", e));
            }
        }
    }

    public static UnityModManager.ModEntry ModEntry;
}
