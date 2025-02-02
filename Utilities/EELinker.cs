using EbonsContentMod.utilities;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Items;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.View;
using Kingmaker.Visual.CharacterSystem;
using Kingmaker.Visual.Mounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.View.Equipment;
using EbonsContentMod;
using Kingmaker.UI.Common;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.Items.Slots;
using static Kingmaker.GameModes.GameModeType;
using Owlcat.Runtime.Core.Utils;
using Kingmaker.UnitLogic.Parts;
using TabletopTweaks.Core.Utilities;
using Kingmaker.Utility;
using UnityEngine;
using static Kingmaker.Visual.CharacterSystem.CharacterStudio;
using Kingmaker.UI.ServiceWindow;
using Kingmaker.Tutorial;
using Kingmaker.UI.UnitSettings;

namespace EbonsContentMod.Utilities
{
    internal class EELinker
    {
        private static readonly Dictionary<BlueprintRace, EquipmentEntityLink[]> EyeLinkEELs = new();

        private static readonly Dictionary<EquipmentEntity, EquipmentEntity[]> EELinkedEyes = new();

        private static readonly Dictionary<BlueprintRace, EquipmentEntityLink[]> SkinLinkEELs = new();

        private static readonly Dictionary<EquipmentEntity, EquipmentEntity[]> EELinkedSkin = new();

        private static readonly Dictionary<BlueprintRace, EquipmentEntityLink[]> HairLinkEELs = new();

        private static readonly Dictionary<EquipmentEntity, EquipmentEntity[]> EELinkedHair = new();

        public static void RegisterEyeLink(BlueprintRace newrace, EquipmentEntityLink[] EELinks)
        {
            EyeLinkEELs[newrace] = EELinks;

            EquipmentEntity[] heads = null;

            foreach (EquipmentEntityLink head in newrace.MaleOptions.Heads)
            {
                heads = heads.AppendToArray(head.Load(false, true));
            }

            foreach (EquipmentEntityLink head in newrace.FemaleOptions.Heads)
            {
                heads = heads.AppendToArray(head.Load(false, true));
            }

            foreach (EquipmentEntityLink eel in EELinks)
            {
                EELinkedEyes[eel.Load(false, true)] = heads;
            }
        }

        public static void RegisterSkinLink(BlueprintRace newrace, EquipmentEntityLink[] EELinks)
        {
            SkinLinkEELs[newrace] = EELinks;

            EquipmentEntity[] heads = null;

            foreach (EquipmentEntityLink head in newrace.MaleOptions.Heads)
            {
                heads = heads.AppendToArray(head.Load(false, true));
            }

            foreach (EquipmentEntityLink head in newrace.FemaleOptions.Heads)
            {
                heads = heads.AppendToArray(head.Load(false, true));
            }

            foreach (EquipmentEntityLink eel in EELinks)
            {
                EELinkedSkin[eel.Load(false, true)] = heads;
            }
        }

        public static void RegisterHairLink(BlueprintRace newrace, EquipmentEntityLink[] EELinks)
        {
            HairLinkEELs[newrace] = EELinks;

            EquipmentEntity[] hairs = null;

            foreach (EquipmentEntityLink hair in newrace.MaleOptions.Hair)
            {
                hairs = hairs.AppendToArray(hair.Load(false, true));
            }

            foreach (EquipmentEntityLink hair in newrace.FemaleOptions.Hair)
            {
                hairs = hairs.AppendToArray(hair.Load(false, true));
            }

            foreach (EquipmentEntityLink eel in EELinks)
            {
                EELinkedHair[eel.Load(false, true)] = hairs;
            }
        }

        [HarmonyPatch]
        static class Patches
        {
            [HarmonyPatch(typeof(DollState), nameof(DollState.ApplyRamps))]
            [HarmonyPostfix]
            static void ApplyRamps_Postfix(Kingmaker.Visual.CharacterSystem.Character character, DollState __instance)
            {
                var race = __instance.Race;
                
                if (__instance.EyesColorRampIndex >= 0 && EyeLinkEELs.TryGetValue(race, out var EyeLinks))
                {
                    foreach (EquipmentEntityLink link in EyeLinks)
                    {
                        character.SetPrimaryRampIndex(link.Load(), __instance.EyesColorRampIndex, false);
                    }
                }

                if (__instance.SkinRampIndex >= 0 && SkinLinkEELs.TryGetValue(race, out var SkinLinks))
                {
                    foreach (EquipmentEntityLink link in SkinLinks)
                    {
                        character.SetPrimaryRampIndex(link.Load(), __instance.SkinRampIndex, false);
                    }
                }

                if (__instance.HairRampIndex >= 0 && HairLinkEELs.TryGetValue(race, out var HairLinks))
                {
                    foreach (EquipmentEntityLink link in HairLinks)
                    {
                        character.SetPrimaryRampIndex(link.Load(), __instance.HairRampIndex, false);
                    }
                }
            }

            [HarmonyPatch(typeof(Character), nameof(Character.AddEquipmentEntity), typeof(EquipmentEntity), typeof(bool), typeof(int), typeof(int))]
            [HarmonyPostfix]
            public static void AddEquipmentEntityLinker_Postfix(Character __instance, EquipmentEntity ee)
            {
                //Main.log.Log("AddEquipmentEntityLinker_Postfix: running, target ee = " + ee.name);

                var SavedRampIndices = __instance.m_RampIndices;

                if (EELinkedEyes.TryGetValue(ee, out var ees))
                {
                    foreach (Character.SelectedRampIndices rampindices in SavedRampIndices)
                    {
                        var ee2 = rampindices.EquipmentEntity;
                        
                        if (ees.Contains(ee2))
                        {
                            __instance.SetPrimaryRampIndex(ee, rampindices.SecondaryIndex);
                            break;
                        }
                    }
                }

                //Main.log.Log("AddEquipmentEntityLinker_Postfix: searching " + EELinkedSkin.Keys.Count.ToString() + " items beginning with \"" + EELinkedSkin.Keys.First().name + "\" for a skin-linked ee match.");

                if (EELinkedSkin.TryGetValue(ee, out var ees2))
                {
                    //Main.log.Log("AddEquipmentEntityLinker_Postfix: found skin-linked ee - " + ee.name);

                    //Main.log.Log("AddEquipmentEntityLinker_Postfix: searching " + __instance.m_RampIndices.Count.ToString() + " ramp indices for a reference ee.");

                    foreach (Character.SelectedRampIndices rampindices in SavedRampIndices)
                    {
                        var ee2 = rampindices.EquipmentEntity;

                        if (ees2.Contains(ee2))
                        {
                            //Main.log.Log("AddEquipmentEntityLinker_Postfix: found reference ee - " + ee2.name);

                            //Main.log.Log("AddEquipmentEntityLinker_Postfix: setting new primary rampindex for ee - " + rampindices.PrimaryIndex.ToString());

                            __instance.SetRampIndices(ee, rampindices.PrimaryIndex, rampindices.PrimaryIndex);
                            break;
                        }
                    }
                }

                if (EELinkedHair.TryGetValue(ee, out var ees3))
                {
                    foreach (Character.SelectedRampIndices rampindices in SavedRampIndices)
                    {
                        var ee2 = rampindices.EquipmentEntity;

                        if (ees3.Contains(ee2))
                        {
                            __instance.SetPrimaryRampIndex(ee, rampindices.PrimaryIndex);
                            break;
                        }
                    }
                }
            }

            // This may not be needed...
            [HarmonyPatch(typeof(UnitEntityView), nameof(UnitEntityView.AddItemEquipment), typeof(ItemEntity), typeof(UnitEntityData), typeof(Character))]
            [HarmonyPostfix]
            public static void AddItemEquipmentLinker_Postfix(UnitEntityView __instance, ItemEntity item, UnitEntityData unit, ref Character avatar)
            {
                if (item == null)
                {
                    //Main.log.Log("RaceEquipmentHiderizer: Item is null");
                    return;
                }
                //Main.log.Log("RaceEquipmentHiderizer: Item is real");
                if (unit.IsPlayerFaction != true)
                {
                    //Main.log.Log("RaceEquipmentHiderizer: Unit is not on Player's team");
                    return;
                }

                KingmakerEquipmentEntity kingmakerEquipmentEntity = (item != null) ? item.EquipmentEntity : null;
                if (kingmakerEquipmentEntity == null)
                {
                    return;
                }
                Kingmaker.Blueprints.Gender gender = unit.Gender;
                Kingmaker.Blueprints.Race indexRace = UnitEntityView.GetActualRace(unit);
                BlueprintRace race = unit.Descriptor.Progression.Race;

                // race not in any dictionary
                if (!EyeLinkEELs.Keys.Contains(race) && !SkinLinkEELs.Keys.Contains(race) && !HairLinkEELs.Keys.Contains(race)) return;

                Character character = avatar.Or(null);
                if (character == null) return;

                // Get saved ramp indices
                int eyeramp = 0;
                int headramp = 0;
                int hairramp = 0;

                if (gender == Kingmaker.Blueprints.Gender.Male)
                {
                    foreach (Character.SelectedRampIndices rampindices in character.m_RampIndices)
                    {
                        foreach (EquipmentEntityLink eel in race.MaleOptions.Heads)
                        {
                            if (rampindices.EquipmentEntity == eel.Load(true, false))
                            {
                                headramp = rampindices.PrimaryIndex;
                                eyeramp = rampindices.SecondaryIndex;
                            }
                        }

                        foreach (EquipmentEntityLink eel in race.MaleOptions.Hair)
                        {
                            if (rampindices.EquipmentEntity == eel.Load(true, false))
                            {
                                hairramp = rampindices.PrimaryIndex;
                            }
                        }
                    }
                }
                else
                {
                    foreach (Character.SelectedRampIndices rampindices in character.m_RampIndices)
                    {
                        foreach (EquipmentEntityLink eel in race.FemaleOptions.Heads)
                        {
                            if (rampindices.EquipmentEntity == eel.Load(true, false))
                            {
                                headramp = rampindices.PrimaryIndex;
                                eyeramp = rampindices.SecondaryIndex;
                            }
                        }

                        foreach (EquipmentEntityLink eel in race.FemaleOptions.Hair)
                        {
                            if (rampindices.EquipmentEntity == eel.Load(true, false))
                            {
                                hairramp = rampindices.PrimaryIndex;
                            }
                        }
                    }
                }

                foreach (EquipmentEntityLink equipmentEntityLink in kingmakerEquipmentEntity.GetLinks(gender, indexRace))
                {
                    if (EyeLinkEELs.Keys.Contains(race) && EyeLinkEELs[race].Contains(equipmentEntityLink))
                    {
                        character.RemoveEquipmentEntity(equipmentEntityLink);
                        character.AddEquipmentEntity(equipmentEntityLink, false, eyeramp);
                    }
                    if (SkinLinkEELs.Keys.Contains(race) && SkinLinkEELs[race].Contains(equipmentEntityLink))
                    {
                        character.RemoveEquipmentEntity(equipmentEntityLink);
                        character.AddEquipmentEntity(equipmentEntityLink, false, headramp);
                    }
                    if (HairLinkEELs.Keys.Contains(race) && HairLinkEELs[race].Contains(equipmentEntityLink))
                    {
                        character.RemoveEquipmentEntity(equipmentEntityLink);
                        character.AddEquipmentEntity(equipmentEntityLink, false, hairramp);
                    }
                }
            }
        }
    }
}
