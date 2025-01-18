using BlueprintCore.Blueprints.Configurators.Visual;
using Kingmaker.Blueprints.Classes;
using Kingmaker.ResourceLinks;
using Kingmaker.Visual.CharacterSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TabletopTweaks.Core.Utilities;
using UnityEngine;
using BlueprintCore.Utils.Assets;
using EbonsContentMod.Utilities;
using Kingmaker.UI.MVVM._ConsoleView.Settings.Entities;
using static Kingmaker.Armies.TacticalCombat.Grid.TacticalCombatGrid;
using Kingmaker.Blueprints;
using BlueprintCore.Blueprints.Configurators.Classes;
using Kingmaker.Blueprints.CharGen;
using HarmonyLib;
using static HarmonyLib.AccessTools;
using Kingmaker.Visual;
using TabletopTweaks.Core.ModLogic;
using BlueprintCore.Blueprints.References;
using UnityEngine.Experimental.Rendering;
using static UnityModManagerNet.UnityModManager.TextureReplacer;
using UniRx;

namespace EbonsContentMod.Utilities
{
    internal class RaceRecolorizer
    {
        internal static string ToolOutputFilePath = Main.ModEntry.Path + "ToolOutput";

        public static float GetColorsFromRGB(float f)
        {
            var color = f / 255;

            return color;
        }

        public static Texture2D GetArmorRampByIndex(int i)
        {
            var RampEquipmentEEL = new EquipmentEntityLink() { AssetId = "ded027dd3a059ae4aa1e8cd93e450b03" };

            var tex = RampEquipmentEEL.Load(true, false).PrimaryColorsProfile.Ramps[i];

            if (tex != null) return tex;

            return null;
        }
        
        public static T[] Arr<T>(params T[] val)
        {
            return val;
        }

        public static void AppendInPlace<T>(ref T[] arr, params T[] newValue)
        {
            arr = arr.AppendToArray(newValue);
        }

        public static string RemoveWhiteSpace(string str)
        {
            return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }

        public static void AddBlueprint(SimpleBlueprint blueprint, BlueprintGuid assetId)
        {
            var loadedBlueprint = ResourcesLibrary.TryGetBlueprint(assetId);
            if (loadedBlueprint == null)
            {
                ResourcesLibrary.BlueprintsCache.AddCachedBlueprint(assetId, blueprint);
                blueprint.OnEnable();
            }
        }

        private static CustomizationOptions CreateMaleCustomizationOptions(BlueprintRace race, EquipmentEntityLink[] hair, EquipmentEntityLink[] heads, EquipmentEntityLink[] eyebrows, EquipmentEntityLink[] beards, bool NoBeards = false)
        {
            if (beards.Length == 0 && NoBeards == false) beards = race.MaleOptions.Beards;
            else if (NoBeards == true) beards = [];

            CustomizationOptions ret = new CustomizationOptions()
            {
                Beards = Arr(beards),
                Horns = race.MaleOptions.Horns,
                m_Eyebrows = Arr(eyebrows),
                m_Heads = Arr(heads),
                m_Hair = Arr(hair),
                TailSkinColors = race.MaleOptions.TailSkinColors
            };

            return ret;
        }

        private static CustomizationOptions CreateFemaleCustomizationOptions(BlueprintRace race, EquipmentEntityLink[] hair, EquipmentEntityLink[] heads, EquipmentEntityLink[] eyebrows)
        {
            CustomizationOptions ret = new CustomizationOptions();
            ret.Beards = race.FemaleOptions.Beards;
            ret.Horns = race.FemaleOptions.Horns;
            ret.m_Eyebrows = Arr(eyebrows);
            ret.m_Hair = Arr(hair);
            ret.m_Heads = Arr(heads);
            ret.TailSkinColors = race.FemaleOptions.TailSkinColors;

            return ret;
        }

        private static CustomizationOptions CreateMaleKitsuneCustomizationOptions(BlueprintRace race, EquipmentEntityLink[] heads)
        {
            CustomizationOptions ret = new CustomizationOptions()
            {
                Beards = race.MaleOptions.Beards,
                Horns = race.MaleOptions.Horns,
                m_Eyebrows = race.MaleOptions.Eyebrows,
                m_Heads = Arr(heads),
                m_Hair = race.MaleOptions.Hair,
                TailSkinColors = race.MaleOptions.TailSkinColors
            };

            return ret;
        }

        private static CustomizationOptions CreateFemaleKitsuneCustomizationOptions(BlueprintRace race, EquipmentEntityLink[] heads)
        {
            CustomizationOptions ret = new CustomizationOptions();
            ret.Beards = race.FemaleOptions.Beards;
            ret.Horns = race.FemaleOptions.Horns;
            ret.m_Eyebrows = race.FemaleOptions.Eyebrows;
            ret.m_Hair = race.FemaleOptions.Hair;
            ret.m_Heads = Arr(heads);
            ret.TailSkinColors = race.FemaleOptions.TailSkinColors;

            return ret;
        }

        internal static List<Texture2D> CreateRampsFromColors(BlueprintRace race, List<Color> colors, string[] sExtraRamps = null)
        {
            List<Texture2D> ramps = [];

            int i = 0;

            foreach (Color color in colors)
            {
                Texture2D newtexture = new(1, 1, TextureFormat.RGB24, false)
                {
                    filterMode = FilterMode.Bilinear,
                    wrapMode = TextureWrapMode.Clamp,
                    wrapModeU = TextureWrapMode.Clamp,
                    wrapModeV = TextureWrapMode.Clamp,
                    wrapModeW = TextureWrapMode.Clamp,
                };
                newtexture.SetPixel(1, 1, color);
                newtexture.Apply(true, false);
                newtexture.name = race.name + "_Ramp_" + RemoveWhiteSpace(color.ToString()) + i.ToString();
                ramps.Add(newtexture);

                i++;
            }

            return ramps;
        }

        /// <summary>
        /// Creates ramps from a list of colors
        /// </summary>
        /// <remarks>
        /// <para>
        /// colors = List&lt;Color&gt; containing the colors you want to create ramps from.
        /// </para>
        /// </remarks>
        /// <returns>
        /// New List&lt;Texture2D&gt; containing the ramps created from the colors passed to the function.
        /// </returns>
        public static List<Texture2D> CreateRampsFromColorsSimple(List<Color> colors)
        {
            List<Texture2D> ramps = [];

            int i = 0;

            foreach (Color color in colors)
            {
                Texture2D newtexture = new(1, 1, TextureFormat.RGB24, false)
                {
                    filterMode = FilterMode.Bilinear,
                    wrapMode = TextureWrapMode.Clamp,
                    wrapModeU = TextureWrapMode.Clamp,
                    wrapModeV = TextureWrapMode.Clamp,
                    wrapModeW = TextureWrapMode.Clamp,
                };
                newtexture.SetPixel(1, 1, color);
                newtexture.Apply(true, false);
                ramps.Add(newtexture);

                i++;
            }

            return ramps;
        }

        public static EquipmentEntity RemoveBodyparts(EquipmentEntity asset, List<BodyPartType> parts)
        {
            foreach (BodyPartType part in parts)
            {
                for (int i = 0; i < asset.BodyParts.ToArray().Length; i++)
                {
                    if (asset.BodyParts[i].m_Type == ((long)part)) asset.BodyParts.RemoveAt(i);
                }
            }

            return asset;
        }

        internal static EquipmentEntityLink PatchEntityLinkColor(BlueprintRace race, EquipmentEntityLink asset, List<Texture2D> Ramps, CharacterColorsProfile NewSkinColorsProfile = null, List<Texture2D> SecondaryRamps = null, BlueprintRace eyerace = null, EquipmentEntityLink eyeEE = null, CharacterColorsProfile NewEyeColorsProfile = null, List<BodyPartType> RemoveHeadParts = null)
        {
            var name = race.name + "_" + asset.Load(true, false).name + asset.AssetId;
            var guid = RaceFunctions.GetEntityNewGuidFromName(name);

            if (guid == "Error")
            {
                var newGuid = BlueprintGuid.NewGuid();
                string newline = "\"" + name + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                File.WriteAllLines(ToolOutputFilePath + "\\" + "RaceRecolorizer" + name + ".txt", [newline]);
                return null;
            }

            Dictionary<string, Texture2D> EyeTextures = null;
            Dictionary<string, Texture2D> EyeEETextures = null;

            if (eyerace != null) EyeTextures = GetRaceEyes(eyerace);
            if (eyeEE != null) EyeEETextures = GetEEEyes(eyeEE);

            var newlink = asset.CreateDynamicScriptableObjectProxy<EquipmentEntity, EquipmentEntityLink>(ee =>
            {
                var PrimaryRampDLCLocks = ee.PrimaryColorsProfile.RampDlcLocks;
                var PrimarySpecialRamps = ee.PrimaryColorsProfile.SpecialRamps;
                var PrimaryName = ee.PrimaryColorsProfile.name;
                var SecondaryRampDLCLocks = ee.SecondaryColorsProfile.RampDlcLocks;
                var SecondarySpecialRamps = ee.SecondaryColorsProfile.SpecialRamps;
                var SecondaryName = ee.SecondaryColorsProfile.name;

                if (RemoveHeadParts != null) ee = RemoveBodyparts(ee, RemoveHeadParts);

                if (NewSkinColorsProfile != null)
                {
                    ee.PrimaryColorsProfile = NewSkinColorsProfile;
                    ee.PrimaryColorsProfile.Ramps = Ramps;
                    ee.PrimaryColorsProfile.RampDlcLocks = PrimaryRampDLCLocks;
                    ee.PrimaryColorsProfile.SpecialRamps = PrimarySpecialRamps;
                    ee.PrimaryColorsProfile.name = race.name + "_" + PrimaryName;
                }
                else ee.PrimaryColorsProfile.Ramps = Ramps;
                if(NewEyeColorsProfile != null)
                {
                    ee.SecondaryColorsProfile = NewEyeColorsProfile;
                }
                if (SecondaryRamps != null)
                {
                    ee.SecondaryColorsProfile.Ramps = SecondaryRamps;
                }
                ee.m_PrimaryRamps = Ramps;
                ee.PrimaryRamps = Ramps;
                if (SecondaryRamps != null)
                {
                    ee.m_SecondaryRamps = SecondaryRamps;
                    ee.SecondaryRamps = SecondaryRamps;
                }
                if (eyerace != null && eyeEE == null)
                {
                    foreach (BodyPart part in ee.BodyParts)
                    {
                        if (part.Type == BodyPartType.Eyes && EyeTextures != null)
                        {
                            part.Textures = new(part.Textures);
                            part.Textures[0].ActiveTexture = EyeTextures.GetValueSafe("Active");
                            part.Textures[0].DiffuseTexture = EyeTextures.GetValueSafe("Diffuse");
                            part.Textures[0].MaskTexture = EyeTextures.GetValueSafe("Mask");
                            part.Textures[0].NormalTexture = EyeTextures.GetValueSafe("Normal");
                            part.Textures[0].RampShadowTexture = EyeTextures.GetValueSafe("Shadow");
                        }
                    }
                }
                else if (eyeEE != null)
                {
                    foreach (BodyPart part in ee.BodyParts)
                    {
                        if (part.Type == BodyPartType.Eyes && EyeEETextures != null)
                        {
                            part.Textures = new(part.Textures);
                            part.Textures[0].ActiveTexture = EyeEETextures.GetValueSafe("Active");
                            part.Textures[0].DiffuseTexture = EyeEETextures.GetValueSafe("Diffuse");
                            part.Textures[0].MaskTexture = EyeEETextures.GetValueSafe("Mask");
                            part.Textures[0].NormalTexture = EyeEETextures.GetValueSafe("Normal");
                            part.Textures[0].RampShadowTexture = EyeEETextures.GetValueSafe("Shadow");
                        }
                    }
                }
            }, guid);

            return newlink;
        }

        internal static EquipmentEntityLink PatchHairLinkColor(BlueprintRace race, EquipmentEntityLink asset, List<Texture2D> Ramps, CharacterColorsProfile NewHairColorsProfile = null)
        {
            var name = race.name + "_" + asset.Load(true, false).name + asset.AssetId;
            var guid = RaceFunctions.GetEntityNewGuidFromName(name);

            if (guid == "Error")
            {
                var newGuid = BlueprintGuid.NewGuid();
                string newline = "\"" + name + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                File.WriteAllLines(ToolOutputFilePath + "\\" + "RaceRecolorizer" + name + ".txt", [newline]);
                return null;
            }

            var newlink = asset.CreateDynamicScriptableObjectProxy<EquipmentEntity, EquipmentEntityLink>(ee =>
            {
                var PrimaryRampDLCLocks = ee.PrimaryColorsProfile.RampDlcLocks;
                var PrimarySpecialRamps = ee.PrimaryColorsProfile.SpecialRamps;

                ee.PrimaryColorsProfile = NewHairColorsProfile;
                ee.PrimaryColorsProfile.Ramps = Ramps;
                ee.PrimaryColorsProfile.RampDlcLocks = PrimaryRampDLCLocks;
                ee.PrimaryColorsProfile.SpecialRamps = PrimarySpecialRamps;
                ee.m_PrimaryRamps = Ramps;
                ee.PrimaryRamps = Ramps;
            }, guid);

            return newlink;
        }

        internal static string PatchEEProfile(BlueprintRace race, EquipmentEntityLink asset, List<Texture2D> Ramps)
        {
            var name = race.name + "_" + asset.AssetId;
            var guid = RaceFunctions.GetEntityNewGuidFromName(name);

            if (guid == "Error")
            {
                var newGuid = BlueprintGuid.NewGuid();
                string newline = "\"" + name + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                File.WriteAllLines(ToolOutputFilePath + "\\" + "RaceRecolorizer" + name + ".txt", [newline]);
                return null;
            }

            var newlink = asset.CreateDynamicScriptableObjectProxy<EquipmentEntity, EquipmentEntityLink>(ee =>
            {
                //ee.PrimaryColorsProfile = new CharacterColorsProfile();
                ee.PrimaryColorsProfile = CreateInstance<CharacterColorsProfile>();
                ee.m_PrimaryRamps = Ramps;
                ee.PrimaryRamps = Ramps;

            }, guid);

            return guid;
        }

        /// <summary>
        /// Creates a recolored/retextured version of an EquipmentEntity.
        /// </summary>
        /// <remarks>
        /// <para>
        /// asset = EquipmentEntityLink you want to recolor.
        /// </para>
        /// <para>
        /// Ramps = List&lt;Texture2D&gt; containing a set of ramps for the primary ramp array of your new EquipmentEntity.
        /// </para>
        /// <para>
        /// guid = string for a unique GUID to give the copied and recolored asset.
        /// </para>
        /// <para>
        /// RemovePrimaryProfile = bool, if true, replaces the primary CharacterColorsProfile with an empty one.
        /// </para>
        /// <para>
        /// RemoveSecondaryProfile = bool, if true, replaces the secondary CharacterColorsProfile with an empty one.
        /// </para>
        /// <para>
        /// SecondaryRamps = List&lt;Texture2D&gt; containing a set of ramps for the secondary ramp array of your new EquipmentEntity.
        /// </para>
        /// <para>
        /// eyeEE = EquipmentEntityLink of a EquipmentEntity that you want to take the eye textures from and apply to your new EquipmentEntity's eye bodypart.
        /// </para>
        /// <para>
        /// BodyPartsToRemove = List&lt;BodyPartType&gt;, will remove all the BodyPartTypes in the list from the linked EquipmentEntity
        /// </para>
        /// </remarks>
        /// <returns>
        /// New EquipmentEntityLink for the recolored EquipmentEntity
        /// </returns>
        public static EquipmentEntityLink RecolorEELink(EquipmentEntityLink asset, List<Texture2D> Ramps, string guid, bool RemovePrimaryProfile = false, bool RemoveSecondaryProfile = false, List<Texture2D> SecondaryRamps = null, EquipmentEntityLink eyeEE = null, List<BodyPartType> BodyPartsToRemove = null)
        {
            var newlink = asset.CreateDynamicScriptableObjectProxy<EquipmentEntity, EquipmentEntityLink>(ee =>
            {
                Dictionary<string, Texture2D> EyeEETextures = null;

                if (eyeEE != null) EyeEETextures = GetEEEyes(eyeEE);

                if (BodyPartsToRemove != null) ee = RemoveBodyparts(ee, BodyPartsToRemove);

                if (RemovePrimaryProfile == true)
                {
                    //ee.PrimaryColorsProfile = new CharacterColorsProfile();
                    ee.PrimaryColorsProfile = CreateInstance<CharacterColorsProfile>();
                }
                if (RemoveSecondaryProfile == true)
                {
                    //ee.SecondaryColorsProfile = new CharacterColorsProfile();
                    ee.SecondaryColorsProfile = CreateInstance<CharacterColorsProfile>();
                }
                ee.m_PrimaryRamps = Ramps;
                ee.PrimaryRamps = Ramps;
                if (SecondaryRamps != null)
                {
                    ee.SecondaryRamps = SecondaryRamps;
                    ee.m_SecondaryRamps = SecondaryRamps;
                }
                if (eyeEE != null)
                {
                    foreach (BodyPart part in ee.BodyParts)
                    {
                        if (part.Type == BodyPartType.Eyes && EyeEETextures != null)
                        {
                            part.Textures = new(part.Textures);
                            part.Textures[0].ActiveTexture = EyeEETextures.GetValueSafe("Active");
                            part.Textures[0].DiffuseTexture = EyeEETextures.GetValueSafe("Diffuse");
                            part.Textures[0].MaskTexture = EyeEETextures.GetValueSafe("Mask");
                            part.Textures[0].NormalTexture = EyeEETextures.GetValueSafe("Normal");
                            part.Textures[0].RampShadowTexture = EyeEETextures.GetValueSafe("Shadow");
                        }
                    }
                }

            }, guid);

            return newlink;
        }

        internal static EquipmentEntityLink SwitchEEEyeTexture(EquipmentEntityLink asset, string guid, List<Texture2D> Ramps = null, bool RemovePrimaryProfile = false, bool RemoveSecondaryProfile = false, List<Texture2D> SecondaryRamps = null, EquipmentEntityLink eyeEE = null)
        {
            var newlink = asset.CreateDynamicScriptableObjectProxy<EquipmentEntity, EquipmentEntityLink>(ee =>
            {
                Dictionary<string, Texture2D> EyeEETextures = null;

                if (eyeEE != null) EyeEETextures = GetEEEyes(eyeEE);

                if (RemovePrimaryProfile == true)
                {
                    //ee.PrimaryColorsProfile = new CharacterColorsProfile();
                    ee.PrimaryColorsProfile = CreateInstance<CharacterColorsProfile>();
                }
                if (RemoveSecondaryProfile == true)
                {
                    //ee.SecondaryColorsProfile = new CharacterColorsProfile();
                    ee.SecondaryColorsProfile = CreateInstance<CharacterColorsProfile>();
                }
                if (Ramps != null)
                {
                    ee.m_PrimaryRamps = Ramps;
                    ee.PrimaryRamps = Ramps;
                }
                if (SecondaryRamps != null)
                {
                    ee.SecondaryRamps = SecondaryRamps;
                    ee.m_SecondaryRamps = SecondaryRamps;
                }
                if (eyeEE != null)
                {
                    foreach (BodyPart part in ee.BodyParts)
                    {
                        if (part.Type == BodyPartType.Eyes && EyeEETextures != null)
                        {
                            part.Textures = new(part.Textures);
                            part.Textures[0].ActiveTexture = EyeEETextures.GetValueSafe("Active");
                            part.Textures[0].DiffuseTexture = EyeEETextures.GetValueSafe("Diffuse");
                            part.Textures[0].MaskTexture = EyeEETextures.GetValueSafe("Mask");
                            part.Textures[0].NormalTexture = EyeEETextures.GetValueSafe("Normal");
                            part.Textures[0].RampShadowTexture = EyeEETextures.GetValueSafe("Shadow");
                        }
                    }
                }

            }, guid);

            return newlink;
        }

        internal static BlueprintRaceVisualPresetReference[] BuildNewPresets(BlueprintRace race, List<Texture2D> Ramps, CharacterColorsProfile NewSkinColorsProfile, EquipmentEntityLink CustomBody, EquipmentEntityLink CustomBodyNoRecolor)
        {
            BlueprintRaceVisualPreset[] presets = new BlueprintRaceVisualPreset[race.m_Presets.Length];

            // Clone the skin with new ramps and consistent profile
            var skin = AltHelpers.CreateCopyAlt(race.m_Presets[0].Get().Skin, skin =>
            {
                skin.name = race.name + "_" + race.m_Presets[0].Get().Skin.name;

                // Get all the GUIDs
                var SkinGuid = RaceFunctions.GetEntityNewGuidFromName(skin.name);
                
                if (SkinGuid == "Error")
                {
                    var newGuid = BlueprintGuid.NewGuid();
                    string newline = "\"" + skin.name + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                    File.WriteAllLines(ToolOutputFilePath + "\\" + "RaceRecolorizer" + skin.name + ".txt", [newline]);
                }

                var MaleEntityLinkGuid = RaceFunctions.GetEntityNewGuidFromName(race.name + "_Male_" + skin.m_MaleArray[0].AssetId);

                if (MaleEntityLinkGuid == "Error")
                {
                    var newGuid = BlueprintGuid.NewGuid();
                    string newline = "\"" + race.name + "_Male_" + skin.m_MaleArray[0].AssetId + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                    File.WriteAllLines(ToolOutputFilePath + "\\" + "RaceRecolorizer" + race.name + "_Male_" + skin.m_MaleArray[0].AssetId + ".txt", [newline]);
                    MaleEntityLinkGuid = null;
                }

                var FemaleEntityLinkGuid = RaceFunctions.GetEntityNewGuidFromName(race.name + "_Female_" + skin.m_FemaleArray[0].AssetId);

                if (FemaleEntityLinkGuid == "Error")
                {
                    var newGuid = BlueprintGuid.NewGuid();
                    string newline = "\"" + race.name + "_Female_" + skin.m_FemaleArray[0].AssetId + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                    File.WriteAllLines(ToolOutputFilePath + "\\" + "RaceRecolorizer" + race.name + "_Female_" + skin.m_FemaleArray[0].AssetId + ".txt", [newline]);
                    FemaleEntityLinkGuid = null;
                }

                var MaleBody = skin.m_MaleArray[0];
                var FemaleBody = skin.m_FemaleArray[0];

                if (CustomBody != null) // Will need to break this up into male and female if I use it to handle more than the goblin I think
                {
                    MaleBody = CustomBody;
                    FemaleBody = CustomBody;
                }

                // Copy the EELinks, give them the same color profile as the heads, and set them in place
                var NewMaleLink = MaleBody.CreateDynamicScriptableObjectProxyAlt<EquipmentEntity, EquipmentEntityLink>(ee =>
                {
                    ee.PrimaryColorsProfile = NewSkinColorsProfile;

                }, MaleEntityLinkGuid);

                if (CustomBodyNoRecolor == null) skin.m_MaleArray = Arr(new EquipmentEntityLink { AssetId = MaleEntityLinkGuid });
                else skin.m_MaleArray = Arr(CustomBodyNoRecolor);

                var NewFemaleLink = FemaleBody.CreateDynamicScriptableObjectProxyAlt<EquipmentEntity, EquipmentEntityLink>(ee =>
                {
                    ee.PrimaryColorsProfile = NewSkinColorsProfile;

                }, FemaleEntityLinkGuid);

                if (CustomBodyNoRecolor == null) skin.m_FemaleArray = Arr(new EquipmentEntityLink { AssetId = FemaleEntityLinkGuid });
                else skin.m_FemaleArray = Arr(CustomBodyNoRecolor);

                // Add the skin blueprint
                skin.AssetGuid = BlueprintGuid.Parse(SkinGuid);
                AddBlueprint(skin, skin.AssetGuid);
            });

            // Now that we got the skin, build the presets
            for (int i = 0; i < presets.Length; i++)
            {                
                presets[i] = AltHelpers.CreateCopyAlt(race.m_Presets[i].Get(), p => 
                {
                    p.name = race.name + "_" + p.name;

                    var guid = RaceFunctions.GetEntityNewGuidFromName(p.name);

                    if (guid == "Error")
                    {
                        var newGuid = BlueprintGuid.NewGuid();
                        string newline = "\"" + p.name + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                        File.WriteAllLines(ToolOutputFilePath + "\\" + "RaceRecolorizer" + p.name + ".txt", [newline]);
                    }

                    p.m_Skin = skin.ToReference<KingmakerEquipmentEntityReference>();

                    p.AssetGuid = BlueprintGuid.Parse(guid);
                    AddBlueprint(p, p.AssetGuid);
                });
            }

            var RetPresets = presets.Select(p => p.ToReference<BlueprintRaceVisualPresetReference>()).ToArray();

            return RetPresets;
        }

        internal static Dictionary<string, Texture2D> GetRaceEyes(BlueprintRace race)
        {

            var BodyParts = race.MaleOptions.Heads.First().Load(true, false).BodyParts;

            foreach (BodyPart part in BodyParts)
            {
                if (part.Type != BodyPartType.Eyes) continue;

                Dictionary<string, Texture2D> EyeTextures = new()
                {
                    {"Active", part.Textures.First().ActiveTexture},
                    {"Diffuse", part.Textures.First().DiffuseTexture},
                    {"Mask", part.Textures.First().MaskTexture},
                    {"Normal", part.Textures.First().NormalTexture},
                    {"Shadow", part.Textures.First().RampShadowTexture}
                };

                return EyeTextures;
            }

            return null;
        }

        internal static Dictionary<string, Texture2D> GetEEEyes(EquipmentEntityLink asset)
        {

            var BodyParts = asset.Load(true, false).BodyParts;

            foreach (BodyPart part in BodyParts)
            {
                if (part.Type != BodyPartType.Eyes) continue;

                Dictionary<string, Texture2D> EyeTextures = new()
                {
                    {"Active", part.Textures.First().ActiveTexture},
                    {"Diffuse", part.Textures.First().DiffuseTexture},
                    {"Mask", part.Textures.First().MaskTexture},
                    {"Normal", part.Textures.First().NormalTexture},
                    {"Shadow", part.Textures.First().RampShadowTexture}
                };

                return EyeTextures;
            }

            return null;
        }

        /// <summary>
        /// Creates a recolored version of a given race.
        /// </summary>
        /// <remarks>
        /// <para>
        /// race = BlueprintRace of the race that you want to recolor.
        /// </para>
        /// <para>
        /// headcolors = List&lt;Color&gt; of the colors you want to use to create the head/skin ramps. Can use any color, but will show up as empty swatches in the character creator.
        /// </para>
        /// <para>
        /// haircolors = List&lt;Color&gt; of the colors you want to use to create the hair/eyebrow/beard ramps. Can use any color, but will show up as empty swatches in the character creator.
        /// </para>
        /// <para>
        /// eyecolors = List&lt;Color&gt; of the colors you want to use to create the eye ramps. Can use any color, but will show up as empty swatches in the character creator.
        /// </para>
        /// <para>
        /// horncolors = List&lt;Color&gt; of the colors you want to use to create the horn ramps. Can use any color, but will show up as empty swatches in the character creator (not implemented yet).
        /// </para>
        /// <para>
        /// eyerace = BlueprintRace of a race that you want to take the eye textures from and apply to your new race's eyes.
        /// </para>
        /// <para>
        /// eyeEE = EquipmentEntityLink of a EquipmentEntity that you want to take the eye textures from and apply to your new race's eyes.
        /// </para>
        /// <para>
        /// BaldRace = bool, if true, all members of your race will be bald.
        /// </para>
        /// <para>
        /// OnlyMalesBald = bool, if true, and BaldRace is true, only male members of your race will be bald.
        /// </para>
        /// <para>
        /// KitsuneBaldRace = bool, if true, will not assign anything but heads to male or female options. Use when copying the kitsune race.
        /// </para>
        /// <para>
        /// NoEyebrows = bool, if true, your race will not have eyebrows.
        /// </para>
        /// <para>
        /// NoBeards = bool, if true, male members of your race will not have beards.
        /// </para>
        /// <para>
        /// OnlyAddHeadColors = bool, if true, the head colors you pass in will be added to the ramp array of the copied race rather than replacing it.
        /// </para>
        /// <para>
        /// OnlyAddHairColors = bool, if true, the hair colors you pass in will be added to the ramp array of the copied race rather than replacing it.
        /// </para>
        /// <para>
        /// OnlyAddEyeColors = bool, if true, the eye colors you pass in will be added to the ramp array of the copied race rather than replacing it.
        /// </para>
        /// <para>
        /// HeadSwapRace = BlueprintRace for a race which you want to use the heads from instead of the heads from the race you are copying.
        /// </para>
        /// <para>
        /// CustomMaleHeads = EquipmentEntityLink[] containing the specific male heads you want to be included with your race.
        /// </para>
        /// <para>
        /// CustomFemaleHeads = EquipmentEntityLink[] containing the specific female heads you want to be included with your race.
        /// </para>
        /// <para>
        /// CustomMaleHairs = EquipmentEntityLink[] containing the specific male hairs you want to be included with your race.
        /// </para>
        /// <para>
        /// CustomFemaleHairs = EquipmentEntityLink[] containing the specific female hairs you want to be included with your race.
        /// </para>
        /// <para>
        /// CustomBody = EquipmentEntityLink for the specific body you want to be used for your race.
        /// </para>
        /// <para>
        /// CustomHeadRamps = List&lt;Texture2D&gt; containing a set of ramps for the head/skin ramp array of your race, replaces 'headcolors' argument.
        /// </para>
        /// <para>
        /// CustomEyeRamps = List&lt;Texture2D&gt; containing a set of ramps for the eye ramp array of your race, replaces 'eyecolors' argument.
        /// </para>
        /// <para>
        /// CustomHairRamps = List&lt;Texture2D&gt; containing a set of ramps for the hair/eyebrow/beard ramp array of your race, replaces 'haircolors' argument.
        /// </para>
        /// <para>
        /// CustomBodyNoRecolor = EquipmentEntityLink for the specific body you want to be used for your race, will not be recolored.
        /// </para>
        /// <para>
        /// CustomMaleHeadsNoRecolor = EquipmentEntityLink[] containing the specific male heads you want to be included with your race, will not be recolored.
        /// </para>
        /// <para>
        /// CustomFemaleHeadsNoRecolor = EquipmentEntityLink[] containing the specific female heads you want to be included with your race, will not be recolored.
        /// </para>
        /// <para>
        /// EyeLinkedEEs = EquipmentEntityLink[] containing links to equipment entities which will change colors along with the eyes. Finicky, works best for tattoos/warpaint.
        /// </para>
        /// <para>
        /// StartMalesWithBeard = bool, if true, the "no beard" option will be added to the end of the beard array, making males appear appear with a beard by default.
        /// </para>
        /// <para>
        /// RemoveHeadParts = List&lt;BodyPartType&gt;, will remove all the BodyPartTypes indicated from the head EquipmentEntities 
        /// </para>
        /// </remarks>
        /// <returns>
        /// New recolored race.
        /// </returns>
        public static BlueprintRace RecolorRace(BlueprintRace race, List<Color> headcolors, List<Color> haircolors, /*string[] sExtraHeadRamps = null, string[] sExtraHairRamps = null,*/ List<Color> eyecolors = null, List<Color> horncolors = null, BlueprintRace eyerace = null, EquipmentEntityLink eyeEE = null, bool BaldRace = false, bool OnlyMalesBald = false, bool KitsuneBaldRace = false, bool NoEyebrows = false, bool NoBeards = false, bool OnlyAddHeadColors = false, bool OnlyAddHairColors = false, bool OnlyAddEyeColors = false, BlueprintRace HeadSwapRace = null, EquipmentEntityLink[] CustomMaleHeads = null, EquipmentEntityLink[] CustomFemaleHeads = null, EquipmentEntityLink[] CustomMaleHairs = null, EquipmentEntityLink[] CustomFemaleHairs = null, EquipmentEntityLink CustomBody = null, List<Texture2D> CustomHeadRamps = null, List<Texture2D> CustomEyeRamps = null, List<Texture2D> CustomHairRamps = null, EquipmentEntityLink CustomBodyNoRecolor = null, EquipmentEntityLink[] CustomMaleHeadsNoRecolor = null,  EquipmentEntityLink[] CustomFemaleHeadsNoRecolor = null, EquipmentEntityLink[] EyeLinkedEEs = null, bool StartMalesWithBeard = false, List<BodyPartType> RemoveHeadParts = null/*, bool OnlyFemale = false*/)
        {
            // TO DO: add handling for tails and horns
            
            var HeadRamps = CreateRampsFromColors(race, headcolors);
            if (CustomHeadRamps != null) HeadRamps = CustomHeadRamps;
            if (OnlyAddHeadColors)
            {
                var NewHeadRamps = HeadRamps;
                HeadRamps = race.MaleOptions.Heads[1].Load().m_PrimaryRamps;
                HeadRamps.AddRange(NewHeadRamps);
            }

            var HairRamps = CreateRampsFromColors(race, haircolors);
            if (CustomHairRamps != null) HairRamps = CustomHairRamps;
            if (OnlyAddHairColors)
            {
                var NewHairRamps = HairRamps;
                HairRamps = race.MaleOptions.Hair[1].Load().m_PrimaryRamps;
                if (NewHairRamps.Count > 0) HairRamps.AddRange(NewHairRamps);
            }

            List<Texture2D> EyeRamps = null;
            if (eyecolors != null)
            { 
                EyeRamps = CreateRampsFromColors(race, eyecolors);
                if (OnlyAddEyeColors)
                {
                    var NewEyeRamps = EyeRamps;
                    EyeRamps = race.MaleOptions.Heads[1].Load().m_SecondaryRamps;
                    EyeRamps.AddRange(NewEyeRamps);
                }
            }
            if (CustomEyeRamps != null)
            { 
                EyeRamps = CustomEyeRamps;
                if (OnlyAddEyeColors)
                {
                    var NewEyeRamps = CreateRampsFromColors(race, eyecolors);
                    EyeRamps.AddRange(NewEyeRamps);
                }
            }

            var maleheads = race.MaleOptions.Heads;
            var femaleheads = race.FemaleOptions.Heads;

            if (HeadSwapRace != null) maleheads = HeadSwapRace.MaleOptions.Heads;
            if (HeadSwapRace != null) femaleheads = HeadSwapRace.FemaleOptions.Heads;

            if (CustomMaleHeads != null) maleheads = CustomMaleHeads;
            if (CustomFemaleHeads != null) femaleheads = CustomFemaleHeads;

            //CharacterColorsProfile NewSkinColorsProfile = new();
            //CharacterColorsProfile NewHairColorsProfile = new();
            //CharacterColorsProfile NewEyeColorsProfile = new();
            CharacterColorsProfile NewSkinColorsProfile = CreateInstance<CharacterColorsProfile>();
            CharacterColorsProfile NewHairColorsProfile = CreateInstance<CharacterColorsProfile>();
            CharacterColorsProfile NewEyeColorsProfile = CreateInstance<CharacterColorsProfile>();
            if (eyecolors == null) NewEyeColorsProfile = null;

            // Define new head arrays.
            EquipmentEntityLink[] MaleHeadArray = [];
            EquipmentEntityLink[] FemaleHeadArray = [];

            if (CustomMaleHeadsNoRecolor == null)
            {
                foreach (EquipmentEntityLink head in maleheads)
                {
                    var NewHead = PatchEntityLinkColor(race, head, HeadRamps, NewSkinColorsProfile, EyeRamps, eyerace, eyeEE, NewEyeColorsProfile, RemoveHeadParts);

                    if (NewHead != null)
                    {
                        MaleHeadArray = MaleHeadArray.AppendToArray(NewHead);
                    }
                }

                foreach (EquipmentEntityLink head in femaleheads)
                {
                    var NewHead = PatchEntityLinkColor(race, head, HeadRamps, NewSkinColorsProfile, EyeRamps, eyerace, eyeEE, NewEyeColorsProfile, RemoveHeadParts);

                    if (NewHead != null)
                    {
                        FemaleHeadArray = FemaleHeadArray.AppendToArray(NewHead);
                    }
                }
            }
            else
            {
                MaleHeadArray = CustomMaleHeadsNoRecolor;
                FemaleHeadArray = CustomFemaleHeadsNoRecolor;
            }

            var malehairs = race.MaleOptions.Hair;
            var femalehairs = race.FemaleOptions.Hair;

            if (CustomMaleHairs != null) malehairs = CustomMaleHairs;
            if (CustomFemaleHairs != null) femalehairs = CustomFemaleHairs;

            //var BaldHair = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString()).MaleOptions.Hair[9];
            // Just recolor the bald hair one time
            var BaldHair = new EquipmentEntityLink() { AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502" };
            if (KitsuneBaldRace == false)  BaldHair = PatchHairLinkColor(race, new EquipmentEntityLink() { AssetId = "b85db19d7adf6aa48b5dd2bb7bfe1502" }, HairRamps, NewHairColorsProfile);

            // Could probably just set the arrays empty now to avoid loading the bald EEs extra times
            //if (BaldRace) malehairs = Arr(BaldHair);
            //if (BaldRace && !OnlyMalesBald) femalehairs = Arr(BaldHair);

            if (BaldRace) malehairs = [];
            if (BaldRace && !OnlyMalesBald) femalehairs = [];

            EquipmentEntityLink[] MaleHairArray = [];
            EquipmentEntityLink[] FemaleHairArray = [];
            EquipmentEntityLink[] MaleBrowArray = [];
            EquipmentEntityLink[] FemaleBrowArray = [];
            EquipmentEntityLink[] MaleBeardArray = [];

            if (KitsuneBaldRace == false)
            {
                foreach (EquipmentEntityLink hair in malehairs)
                {
                    // Skip the bald hair so we don't repeat GUID lookups
                    if (hair.Load(true, false).name == "EE_EMPTY_HairStyleColors") continue;
                    if (hair.AssetId == "b85db19d7adf6aa48b5dd2bb7bfe1502") continue;

                    var NewHair = PatchHairLinkColor(race, hair, HairRamps, NewHairColorsProfile);

                    if (NewHair != null)
                    {
                        MaleHairArray = MaleHairArray.AppendToArray(NewHair);
                    }
                }

                foreach (EquipmentEntityLink hair in femalehairs)
                {
                    // Skip the bald hair so we don't repeat GUID lookups
                    if (hair.Load(true, false).name == "EE_EMPTY_HairStyleColors") continue;
                    if (hair.AssetId == "b85db19d7adf6aa48b5dd2bb7bfe1502") continue;

                    var NewHair = PatchHairLinkColor(race, hair, HairRamps, NewHairColorsProfile);

                    if (NewHair != null)
                    {
                        FemaleHairArray = FemaleHairArray.AppendToArray(NewHair);
                    }
                }

                // Add the recolored bald hair to the end of the array
                MaleHairArray = MaleHairArray.AppendToArray(BaldHair);
                FemaleHairArray = FemaleHairArray.AppendToArray(BaldHair);

                var malebrows = race.MaleOptions.Eyebrows;
                var femalebrows = race.FemaleOptions.Eyebrows;

                foreach (EquipmentEntityLink brow in malebrows)
                {
                    var NewBrow = PatchHairLinkColor(race, brow, HairRamps, NewHairColorsProfile);

                    if (NewBrow != null && NoEyebrows == false)
                    {
                        MaleBrowArray = MaleBrowArray.AppendToArray(NewBrow);
                    }
                }

                foreach (EquipmentEntityLink brow in femalebrows)
                {
                    var NewBrow = PatchHairLinkColor(race, brow, HairRamps, NewHairColorsProfile);

                    if (NewBrow != null && NoEyebrows == false)
                    {
                        FemaleBrowArray = FemaleBrowArray.AppendToArray(NewBrow);
                    }
                }

                var malebeards = race.MaleOptions.Beards;

                if (StartMalesWithBeard == false) MaleBeardArray = MaleBeardArray.AppendToArray(BaldHair);

                if (malebeards.Length > 0)
                {
                    foreach (EquipmentEntityLink beard in malebeards)
                    {
                        // Skip the bald hair so we don't repeat GUID lookups
                        if (beard.Load(true, false).name == "EE_EMPTY_HairStyleColors") continue;
                        if (beard.AssetId == "b85db19d7adf6aa48b5dd2bb7bfe1502") continue;

                        var NewBeard = PatchHairLinkColor(race, beard, HairRamps, NewHairColorsProfile);

                        if (NewBeard != null && NoBeards == false)
                        {
                            MaleBeardArray = MaleBeardArray.AppendToArray(NewBeard);
                        }
                    }
                }

                if (StartMalesWithBeard == true) MaleBeardArray = MaleBeardArray.AppendToArray(BaldHair);
            }

            //var EyeRampsForEEs = race.MaleOptions.Heads[0].Load().m_SecondaryRamps;
            List<Texture2D> EyeRampsForEEs = [];
            if (EyeLinkedEEs != null && MaleHeadArray[0].Load().m_SecondaryRamps != null)  EyeRampsForEEs = MaleHeadArray[0].Load().m_SecondaryRamps;
            if (EyeRamps != null) EyeRampsForEEs = EyeRamps;

            string[] NewEyeLinkedEEStrings = [];
            EquipmentEntityLink[] NewEyeLinkedEELinks = [];

            if (EyeLinkedEEs != null)
            {
                foreach (EquipmentEntityLink EELink in EyeLinkedEEs)
                {
                    var NewEELink = PatchEEProfile(race, EELink, EyeRampsForEEs);

                    if (NewEELink != null)
                    {
                        NewEyeLinkedEEStrings = NewEyeLinkedEEStrings.AppendToArray(NewEELink);
                        NewEyeLinkedEELinks = NewEyeLinkedEELinks.AppendToArray(new EquipmentEntityLink() { AssetId = NewEELink });
                    }
                }
            }

            if (NewEyeLinkedEELinks.Length > 0) EELinker.RegisterEyeLink(race, NewEyeLinkedEELinks);

            var MaleOptions = CreateMaleCustomizationOptions(race, MaleHairArray, MaleHeadArray, MaleBrowArray, MaleBeardArray, NoBeards);
            if (KitsuneBaldRace == true) MaleOptions = CreateMaleKitsuneCustomizationOptions(race, MaleHeadArray);
            var FemaleOptions = CreateFemaleCustomizationOptions(race, FemaleHairArray, FemaleHeadArray, FemaleBrowArray);
            if (KitsuneBaldRace == true) FemaleOptions = CreateFemaleKitsuneCustomizationOptions(race, FemaleHeadArray);

            var NewPresets = BuildNewPresets(race, HeadRamps, NewSkinColorsProfile, CustomBody, CustomBodyNoRecolor);

            RaceConfigurator ReturnRace = RaceConfigurator.For(race)
                .SetMaleOptions(MaleOptions)
                .SetFemaleOptions(FemaleOptions)
                .SetPresets(NewPresets[0], NewPresets[1], NewPresets[2]); // Hate doing it this way, but I can't remember the better way!

            if (NewEyeLinkedEEStrings.Length > 0) foreach (string EEL in NewEyeLinkedEEStrings) ReturnRace.AddEquipmentEntity(new EquipmentEntityLink() { AssetId = EEL});

                var ReturnRaceReference = ReturnRace.Configure().ToReference<BlueprintRaceReference>();

            return BlueprintTools.GetBlueprint<BlueprintRace>(ReturnRaceReference.ToString());
        }
    }
}
