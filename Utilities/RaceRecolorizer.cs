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

        internal static string GetEntityNewGuidFromName(string name)
        {
            return name switch
            {
                "DuergarRace_Dwarf_VisualPreset" => "859a529889e44e76aa1ae9690ffea623",
                "DuergarRace_Dwarf_VisualPreset_fat" => "2cf6c3c707454e4c8a55479cf19652e0",
                "DuergarRace_Dwarf_VisualPreset_thin" => "722f5cf11f2d481d91f7f5d8fafd3e0f",
                "DuergarRace_KEE_Body_Dwarf" => "ba7a6e06f71c499a8fccbdfc6cbce9fd",
                "SamsaranRace_Human_Standard_VisualPreset" => "d9a1b11be43b4041b4e63ca1280a6548",
                "SamsaranRace_Human_Standard_VisualPreset_fat" => "b9e0c796a15f474bb5cf29caf69ad9d8",
                "SamsaranRace_Human_Standard_VisualPreset_thin" => "3ec30a47a93645359eef3e43de61084e",
                "SamsaranRace_KEE_Body_Human" => "32949778a8eb4f0a8b7567e910470890",
                "SvirfneblinRace_Gnome_VisualPreset" => "a9f379f4f2534d529c681837b43ff8e2",
                "SvirfneblinRace_Gnome_VisualPreset_fat" => "d87ecac0636444e5a49632fc13b85102",
                "SvirfneblinRace_Gnome_VisualPreset_thin" => "7832a4260c5b48d8b1839c4af5696afc",
                "SvirfneblinRace_KEE_Body_Gnome" => "61c53ed860aa4b07b80c1db129095927",
                "SamsaranRace_Male_e7c86166041c1e04a92276abdab68afa" => "0487d7b04c624360afe55e452086dfbb",
                "SamsaranRace_Female_bb6988a21733fad4296ad22537248fea" => "30fe0775de6b4f73885c6028c0c44de9",
                "DuergarRace_Female_082755264a7d4ab43888645cfeef2e43" => "64880c96e4904230b4e41da89d1a0f62",
                "DuergarRace_Male_f097c18b5fcd269468e19243a62786c0" => "9e7cad5c027240d79c41a05128b6b344",
                "SvirfneblinRace_Female_e969fe61ab898284ebeb387accb994d9" => "565a4f6e52864da88d8eeb9231490176",
                "SvirfneblinRace_Male_ac15d06e7975ca74b970783daaef9b60" => "f2cd137f9ef74c89bf028bebb6ac32b4",
                "SylphRace_Female_bb6988a21733fad4296ad22537248fea" => "e5316e4cd55d4d3b8d358bf6ea0bf278",
                "SylphRace_Human_Standard_VisualPreset" => "cbc9a0a83379408b87b69ef045a7b092",
                "SylphRace_Human_Standard_VisualPreset_fat" => "e56d4226939d492daaf31cab39267f03",
                "SylphRace_Human_Standard_VisualPreset_thin" => "363de6841b174367aec11d043e1c2238",
                "SylphRace_KEE_Body_Human" => "204219385222416a98417282d5a6acd7",
                "SylphRace_Male_e7c86166041c1e04a92276abdab68afa" => "bceabb71f99b49cb857dfc7705d0fb31",
                "UndineRace_Female_bb6988a21733fad4296ad22537248fea" => "9e2c719042d2483f9467a6d3c8822bac",
                "UndineRace_Human_Standard_VisualPreset" => "d0afa5a0293c4252848af3a91e0d7203",
                "UndineRace_Human_Standard_VisualPreset_fat" => "78ce3c701f394a35b40c8470fb176ebb",
                "UndineRace_Human_Standard_VisualPreset_thin" => "863b483aae0f49b18de686c22b4a94e0",
                "UndineRace_KEE_Body_Human" => "313116c25e06427491195720e6299bdd",
                "UndineRace_Male_e7c86166041c1e04a92276abdab68afa" => "2bfda561ee0b4d138c5da2fb193cded3",
                "StrixRace_Female_bb6988a21733fad4296ad22537248fea" => "afe9814302394aec80f3f0528e1d7d5d",
                "StrixRace_Human_Standard_VisualPreset" => "651beb06155e4922944696df6d0995ef",
                "StrixRace_Human_Standard_VisualPreset_fat" => "de77a73ba80b4c35a88de75d9e9cf394",
                "StrixRace_Human_Standard_VisualPreset_thin" => "3ca47021901e4d2a9d9f1ad1edfd0277",
                "StrixRace_KEE_Body_Human" => "2f5284ce1c7f4017884cb2cd57960e1b",
                "StrixRace_Male_e7c86166041c1e04a92276abdab68afa" => "2cc186fcc24248a9a05afed8a989bc04",
                "EbonsDrowRace_Elf_VisualPreset" => "958f125c8f8243ad9b6ed007d5df5396",
                "EbonsDrowRace_Elf_VisualPreset_fat" => "4116ae3131d6409587e3e9c06fa73a7f",
                "EbonsDrowRace_Elf_VisualPreset_thin" => "54ea57422e4d4af09157e86146b45b76",
                "EbonsDrowRace_Female_86acde9747d561649919dd1117cd858c" => "47c54f1fa0b94819b7a906189aa5e532",
                "EbonsDrowRace_KEE_Body_Elf" => "32ae70a145da4ce09e8bcfdce47adbe1",
                "EbonsDrowRace_Male_14f3db044cb98ad4ab0ed4edfba5b6d6" => "867200cd31664343b8fe6bbb169e5321",
                "SuliRace_Female_bb6988a21733fad4296ad22537248fea" => "80c531285ebc4b429a6a96b8a129d8a0",
                "SuliRace_Human_Standard_VisualPreset" => "bb8632bc9946407baf8bce74533ecfde",
                "SuliRace_Human_Standard_VisualPreset_fat" => "6525edd9eb0f49c1ab8e1b3339075728",
                "SuliRace_Human_Standard_VisualPreset_thin" => "719c81aab4a34296b0641ec0c055c87b",
                "SuliRace_KEE_Body_Human" => "ff1c513fe90d4298862ecd377e40785c",
                "SuliRace_Male_e7c86166041c1e04a92276abdab68afa" => "1204fcdf42f34391b1a7e6c8e4c0c98f",
                _ => "Error"
            };
        }

        // Not used, leaving it for potential later use
        private static EquipmentEntity CloneWithNewColors(BlueprintRace race, EquipmentEntity ee, List<Color> colors)
        {
            var ramps = CreateRampsFromColors(race, colors);
            var guid = GetEntityNewGuidFromName(ee.ToString());

            if (guid == "Error")
            {
                var newGuid = BlueprintGuid.NewGuid();
                string newline = "\"" + ee.name + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                File.WriteAllLines(ToolOutputFilePath + "\\" + "RaceRecolorizer" + ee.name + ".txt", [newline]);
                return null;
            }

            EquipmentEntity ret = (EquipmentEntity)ScriptableObject.CreateInstance(typeof(EquipmentEntity));
            ret.BakedTextures = Helpers.CreateCopy<CharacterBakedTextures>(ee.BakedTextures, null);
            ret.BodyParts = Helpers.CreateCopy<List<BodyPart>>(ee.BodyParts, null);
            ret.CantBeHiddenByDollRoom = ee.CantBeHiddenByDollRoom;
            ret.ColorPresets = ee.ColorPresets;
            ret.IsExportEnabled = ee.IsExportEnabled;
            ret.Layer = ee.Layer;
            ret.PrimaryRamps = ramps;
            ret.m_BonesByName = ee.m_BonesByName;
            ret.m_DlcReward = ee.m_DlcReward;
            ret.m_HideBodyParts = ee.m_HideBodyParts;
            ret.m_IsDirty = ee.m_IsDirty;
            ret.m_PrimaryRamps = ramps;
            ret.m_SecondaryRamps = ee.m_SecondaryRamps;
            ret.m_SpecialPrimaryRamps = ee.m_SpecialPrimaryRamps;
            ret.m_SpecialSecondaryRamps = ee.m_SpecialSecondaryRamps;
            ret.OutfitParts = Helpers.CreateCopy<List<EquipmentEntity.OutfitPart>>(ee.OutfitParts, null);
            ret.PrimaryColorsAvailableForPlayer = ee.PrimaryColorsAvailableForPlayer;
            ret.PrimaryColorsProfile = new CharacterColorsProfile();
            ret.PrimaryColorsProfile.Ramps = ramps;
            ret.PrimaryColorsProfile.RampDlcLocks = ee.PrimaryColorsProfile.RampDlcLocks;
            ret.PrimaryColorsProfile.SpecialRamps = ee.PrimaryColorsProfile.SpecialRamps;
            ret.SecondaryColorsProfile = ee.SecondaryColorsProfile;
            ret.SecondaryColorsAvailableForPlayer = ee.SecondaryColorsAvailableForPlayer;
            ret.ShowLowerMaterials = ee.ShowLowerMaterials;
            ret.SkeletonModifiers = ee.SkeletonModifiers;
            ret.name = guid;
            return ret;
        }

        // Add handling for tails and horns
        private static CustomizationOptions CreateMaleCustomizationOptions(BlueprintRace race, EquipmentEntityLink[] hair, EquipmentEntityLink[] heads, EquipmentEntityLink[] eyebrows, EquipmentEntityLink[] beards)
        {
            if (beards.Length == 0) beards = race.MaleOptions.Beards;

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

        internal static EquipmentEntityLink PatchEntityLinkColor(BlueprintRace race, EquipmentEntityLink asset, List<Texture2D> Ramps, CharacterColorsProfile NewSkinColorsProfile = null, List<Texture2D> SecondaryRamps = null, BlueprintRace eyerace = null, bool isHair = false)
        {
            Dictionary<string, Texture2D> EyeTextures = null;

            if (eyerace != null) EyeTextures = GetRaceEyes(eyerace);

            var newlink = asset.CreateDynamicScriptableObjectProxy<EquipmentEntity, EquipmentEntityLink>(ee =>
            {
                var PrimaryRampDLCLocks = ee.PrimaryColorsProfile.RampDlcLocks;
                var PrimarySpecialRamps = ee.PrimaryColorsProfile.SpecialRamps;
                var PrimaryName = ee.PrimaryColorsProfile.name;
                var SecondaryRampDLCLocks = ee.SecondaryColorsProfile.RampDlcLocks;
                var SecondarySpecialRamps = ee.SecondaryColorsProfile.SpecialRamps;
                var SecondaryName = ee.SecondaryColorsProfile.name;

                ee.name = race.name + "_" + ee.name;
                if (NewSkinColorsProfile != null)
                {
                    ee.PrimaryColorsProfile = NewSkinColorsProfile;
                    ee.PrimaryColorsProfile.Ramps = Ramps;
                    ee.PrimaryColorsProfile.RampDlcLocks = PrimaryRampDLCLocks;
                    ee.PrimaryColorsProfile.SpecialRamps = PrimarySpecialRamps;
                    ee.PrimaryColorsProfile.name = race.name + "_" + PrimaryName;
                }
                else ee.PrimaryColorsProfile.Ramps = Ramps;
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
                if (eyerace != null)
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
            });

            return newlink;
        }

        internal static EquipmentEntityLink PatchHairLinkColor(BlueprintRace race, EquipmentEntityLink asset, List<Texture2D> Ramps, CharacterColorsProfile NewHairColorsProfile = null, List<Texture2D> SecondaryRamps = null, BlueprintRace eyerace = null, bool isHair = false)
        {
            var newlink = asset.CreateDynamicScriptableObjectProxy<EquipmentEntity, EquipmentEntityLink>(ee =>
            {
                var PrimaryRampDLCLocks = ee.PrimaryColorsProfile.RampDlcLocks;
                var PrimarySpecialRamps = ee.PrimaryColorsProfile.SpecialRamps;

                ee.name = race.name + "_" + ee.name;
                ee.PrimaryColorsProfile = NewHairColorsProfile;
                ee.PrimaryColorsProfile.Ramps = Ramps;
                ee.PrimaryColorsProfile.RampDlcLocks = PrimaryRampDLCLocks;
                ee.PrimaryColorsProfile.SpecialRamps = PrimarySpecialRamps;
                ee.m_PrimaryRamps = Ramps;
                ee.PrimaryRamps = Ramps;
            });

            return newlink;
        }

        internal static BlueprintRaceVisualPresetReference[] BuildNewPresets(BlueprintRace race, List<Texture2D> Ramps, CharacterColorsProfile NewSkinColorsProfile)
        {
            BlueprintRaceVisualPreset[] presets = new BlueprintRaceVisualPreset[race.m_Presets.Length];

            // Clone the skin with new ramps and consistent profile
            var skin = AltHelpers.CreateCopyAlt(race.m_Presets[0].Get().Skin, skin =>
            {
                skin.name = race.name + "_" + race.m_Presets[0].Get().Skin.name;

                // Get all the GUIDs
                var SkinGuid = GetEntityNewGuidFromName(skin.name);
                
                if (SkinGuid == "Error")
                {
                    var newGuid = BlueprintGuid.NewGuid();
                    string newline = "\"" + skin.name + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                    File.WriteAllLines(ToolOutputFilePath + "\\" + "RaceRecolorizer" + skin.name + ".txt", [newline]);
                }

                var MaleEntityLinkGuid = GetEntityNewGuidFromName(race.name + "_Male_" + skin.m_MaleArray[0].AssetId);

                if (MaleEntityLinkGuid == "Error")
                {
                    var newGuid = BlueprintGuid.NewGuid();
                    string newline = "\"" + race.name + "_Male_" + skin.m_MaleArray[0].AssetId + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                    File.WriteAllLines(ToolOutputFilePath + "\\" + "RaceRecolorizer" + race.name + "_Male_" + skin.m_MaleArray[0].AssetId + ".txt", [newline]);
                    MaleEntityLinkGuid = null;
                }

                var FemaleEntityLinkGuid = GetEntityNewGuidFromName(race.name + "_Female_" + skin.m_FemaleArray[0].AssetId);

                if (FemaleEntityLinkGuid == "Error")
                {
                    var newGuid = BlueprintGuid.NewGuid();
                    string newline = "\"" + race.name + "_Female_" + skin.m_FemaleArray[0].AssetId + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                    File.WriteAllLines(ToolOutputFilePath + "\\" + "RaceRecolorizer" + race.name + "_Female_" + skin.m_FemaleArray[0].AssetId + ".txt", [newline]);
                    FemaleEntityLinkGuid = null;
                }

                // Copy the EELinks, give them the same color profile as the heads, and set them in place
                var NewMaleLink = skin.m_MaleArray[0].CreateDynamicScriptableObjectProxyAlt<EquipmentEntity, EquipmentEntityLink>(ee =>
                {
                    ee.PrimaryColorsProfile = NewSkinColorsProfile;

                }, MaleEntityLinkGuid);

                skin.m_MaleArray = Arr(new EquipmentEntityLink { AssetId = MaleEntityLinkGuid });

                var NewFemaleLink = skin.m_FemaleArray[0].CreateDynamicScriptableObjectProxyAlt<EquipmentEntity, EquipmentEntityLink>(ee =>
                {
                    ee.PrimaryColorsProfile = NewSkinColorsProfile;

                }, FemaleEntityLinkGuid);

                skin.m_FemaleArray = Arr(new EquipmentEntityLink { AssetId = FemaleEntityLinkGuid });

                // Add the skin blueprint
                skin.AssetGuid = BlueprintGuid.Parse(SkinGuid);
                AddBlueprint(skin, skin.AssetGuid);
            });

            // Now that we got the skin, build the presets
            for (int i = 0; i < presets.Length; i++)
            {                
                // TTT's version of CreateCopy (actually ObjectDeepCopier which it calls) causes
                // a stack overflow if used here. Use the older version instead.
                presets[i] = AltHelpers.CreateCopyAlt(race.m_Presets[i].Get(), p => 
                {
                    p.name = race.name + "_" + p.name;

                    var guid = GetEntityNewGuidFromName(p.name);

                    if (guid == "Error")
                    {
                        var newGuid = BlueprintGuid.NewGuid();
                        string newline = "\"" + p.name + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                        File.WriteAllLines(ToolOutputFilePath + "\\" + "RaceRecolorizer" + p.name + ".txt", [newline]);
                        //return null;
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

        // Add handling for tails and horns
        public static BlueprintRace RecolorRace(BlueprintRace race, List<Color> headcolors, List<Color> haircolors, string[] sExtraHeadRamps = null, string[] sExtraHairRamps = null, List<Color> eyecolors = null, List<Color> horncolors = null, BlueprintRace eyerace = null, bool BaldRace = false, bool OnlyMalesBald = false, bool NoEyebrows = false, bool OnlyAddHeadColors = false, bool OnlyAddHairColors = false, bool OnlyAddEyeColors = false, BlueprintRace HeadSwapRace = null, EquipmentEntityLink[] CustomMaleHeads = null, EquipmentEntityLink[] CustomFemaleHeads = null, EquipmentEntityLink[] CustomMaleHairs = null, EquipmentEntityLink[] CustomFemaleHairs = null)
        {
            var HeadRamps = CreateRampsFromColors(race, headcolors);
            if (OnlyAddHeadColors)
            {
                var NewHeadRamps = HeadRamps;
                HeadRamps = race.MaleOptions.Heads[1].Load().m_PrimaryRamps;
                HeadRamps.AddRange(NewHeadRamps);
            }

            var HairRamps = CreateRampsFromColors(race, haircolors);
            if (OnlyAddHairColors)
            {
                var NewHairRamps = HairRamps;
                HairRamps = race.MaleOptions.Hair[1].Load().m_PrimaryRamps;
                HairRamps.AddRange(NewHairRamps);
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

            var maleheads = race.MaleOptions.Heads;
            var femaleheads = race.FemaleOptions.Heads;

            if (HeadSwapRace != null) maleheads = HeadSwapRace.MaleOptions.Heads;
            if (HeadSwapRace != null) femaleheads = HeadSwapRace.FemaleOptions.Heads;

            if (CustomMaleHeads != null) maleheads = CustomMaleHeads;
            if (CustomFemaleHeads != null) femaleheads = CustomFemaleHeads;

            CharacterColorsProfile NewSkinColorsProfile = new();
            CharacterColorsProfile NewHairColorsProfile = new();

            // Define new head arrays.
            EquipmentEntityLink[] MaleHeadArray = [];
            EquipmentEntityLink[] FemaleHeadArray = [];

            foreach (EquipmentEntityLink head in maleheads)
            {
                var NewHead = PatchEntityLinkColor(race, head, HeadRamps, NewSkinColorsProfile, EyeRamps, eyerace);

                if (NewHead != null)
                {
                    MaleHeadArray = MaleHeadArray.AppendToArray(NewHead);
                }
            }

            foreach (EquipmentEntityLink head in femaleheads)
            {
                var NewHead = PatchEntityLinkColor(race, head, HeadRamps, NewSkinColorsProfile, EyeRamps, eyerace);

                if (NewHead != null)
                {
                    FemaleHeadArray = FemaleHeadArray.AppendToArray(NewHead);
                }
            }

            var malehairs = race.MaleOptions.Hair;
            var femalehairs = race.FemaleOptions.Hair;

            if (CustomMaleHairs != null) malehairs = CustomMaleHairs;
            if (CustomFemaleHairs != null) femalehairs = CustomFemaleHairs;

            var BaldHair = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString()).MaleOptions.Hair[9];

            if (BaldRace) malehairs = Arr(BaldHair);
            if (BaldRace && !OnlyMalesBald) femalehairs = Arr(BaldHair);

            EquipmentEntityLink[] MaleHairArray = [];
            EquipmentEntityLink[] FemaleHairArray = [];

            foreach (EquipmentEntityLink hair in malehairs)
            {
                var NewHair = PatchHairLinkColor(race, hair, HairRamps, NewHairColorsProfile);

                if (NewHair != null)
                {
                    MaleHairArray = MaleHairArray.AppendToArray(NewHair);
                }
            }

            foreach (EquipmentEntityLink hair in femalehairs)
            {
                var NewHair = PatchHairLinkColor(race, hair, HairRamps, NewHairColorsProfile);

                if (NewHair != null)
                {
                    FemaleHairArray = FemaleHairArray.AppendToArray(NewHair);
                }
            }

            var malebrows = race.MaleOptions.Eyebrows;
            var femalebrows = race.FemaleOptions.Eyebrows;

            EquipmentEntityLink[] MaleBrowArray = [];
            EquipmentEntityLink[] FemaleBrowArray = [];

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

            EquipmentEntityLink[] MaleBeardArray = [];

            if (malebeards.Length > 0)
            {
                foreach (EquipmentEntityLink beard in malebeards)
                {
                    var NewBeard = PatchHairLinkColor(race, beard, HairRamps, NewHairColorsProfile);

                    if (NewBeard != null)
                    {
                        MaleBeardArray = MaleBeardArray.AppendToArray(NewBeard);
                    }
                }
            }
            
            var MaleOptions = CreateMaleCustomizationOptions(race, MaleHairArray, MaleHeadArray, MaleBrowArray, MaleBeardArray);
            var FemaleOptions = CreateFemaleCustomizationOptions(race, FemaleHairArray, FemaleHeadArray, FemaleBrowArray);

            var NewPresets = BuildNewPresets(race, HeadRamps, NewSkinColorsProfile);

            race = RaceConfigurator.For(race)
                .SetMaleOptions(MaleOptions)
                .SetFemaleOptions(FemaleOptions)
                .SetPresets(NewPresets[0], NewPresets[1], NewPresets[2]) // Hate doing it this way, but I can't remember the better way!
                .Configure();

            return race;
        }
    }
}
