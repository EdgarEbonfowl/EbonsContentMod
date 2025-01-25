using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.Items;
using Kingmaker.View;
using Kingmaker.View.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Visual.CharacterSystem;
using Kingmaker.Blueprints.Classes;
using EbonsContentMod;
using Kingmaker.UI.Common;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.Items.Slots;
using Kingmaker.UnitLogic;
using Kingmaker.ResourceLinks;
using static Kingmaker.GameModes.GameModeType;
using Owlcat.Runtime.Core.Utils;
using Kingmaker.UnitLogic.Parts;
using TabletopTweaks.Core.Utilities;
using Kingmaker.Utility;
using UnityEngine;
using static Kingmaker.Visual.CharacterSystem.CharacterStudio;
using Kingmaker.UI.ServiceWindow;
using Kingmaker.Tutorial;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UI.UnitSettings;
//using static Kingmaker.Visual.CharacterSystem.CharacterStudio;

namespace EbonsContentMod.utilities
{
    internal class RaceEquipmentHiderizer
    {
        public static readonly Dictionary<BlueprintRace, ItemsFilter.ItemType[]> HiddenItems = new();

        public static void AddRaceHiddenItems(BlueprintRace newrace, ItemsFilter.ItemType[] hiddenitems)
        {
            HiddenItems[newrace] = hiddenitems;
        }
    }

    // Helmet equipment hider
    [HarmonyPatch(typeof(DollState))]
    internal class Player_GetShowHelm_Patch
    {
        [HarmonyPatch(nameof(DollState.ShowHelm), MethodType.Getter)]
        [HarmonyPostfix]
        public static void Postfix(DollState __instance, ref bool __result)
        {
            var race = __instance.Race;
            if (race == null) 
            {
                //Main.log.Log("GetShowHelmetPatch: Unit is null");
                return;
            }
            foreach (var bprace in RaceEquipmentHiderizer.HiddenItems.Keys)
            {
                if (race == bprace)
                {
                    if (RaceEquipmentHiderizer.HiddenItems[bprace].Contains(ItemsFilter.ItemType.Head)) __result = false;
                }
            }
        }
    }

    [HarmonyPatch(typeof(UnitUISettings))]
    internal class Player_GetShowHelmUnitUISettings_Patch
    {
        [HarmonyPatch(nameof(UnitUISettings.ShowHelm), MethodType.Getter)]
        [HarmonyPostfix]
        public static void Postfix(UnitUISettings __instance, ref bool __result)
        {
            var race = __instance.Owner.Progression.Race;
            if (race == null)
            {
                //Main.log.Log("GetShowHelmetUnitUISettingsPatch: Unit is null");
                return;
            }
            foreach (var bprace in RaceEquipmentHiderizer.HiddenItems.Keys)
            {
                if (race == bprace)
                {
                    if (RaceEquipmentHiderizer.HiddenItems[bprace].Contains(ItemsFilter.ItemType.Head)) __result = false;
                }
            }
        }
    }

    // Class Equipment hider
    [HarmonyPatch(typeof(DollState))]
    internal class Player_GetShowCloth_Patch
    {
        [HarmonyPatch(nameof(DollState.ShowCloth), MethodType.Getter)]
        [HarmonyPostfix]
        public static void Postfix(DollState __instance, ref bool __result)
        {
            var race = __instance.Race;
            if (race == null)
            {
                //Main.log.Log("GetShowClothPatch: Unit is null");
                return;
            }
            foreach (var bprace in RaceEquipmentHiderizer.HiddenItems.Keys)
            {
                if (race == bprace)
                {
                    if (RaceEquipmentHiderizer.HiddenItems[bprace].Contains(ItemsFilter.ItemType.Armor) || RaceEquipmentHiderizer.HiddenItems[bprace].Contains(ItemsFilter.ItemType.Shirt)) __result = false;
                }
            }
        }
    }

    [HarmonyPatch(typeof(UnitUISettings))]
    internal class Player_GetShowClassEquipmentUnitUISettings_Patch
    {
        [HarmonyPatch(nameof(UnitUISettings.ShowClassEquipment), MethodType.Getter)]
        [HarmonyPostfix]
        public static void Postfix(UnitUISettings __instance, ref bool __result)
        {
            var race = __instance.Owner.Progression.Race;
            if (race == null)
            {
                //Main.log.Log("GetShowClassEquipmentetUnitUISettingsPatch: Unit is null");
                return;
            }
            foreach (var bprace in RaceEquipmentHiderizer.HiddenItems.Keys)
            {
                if (race == bprace)
                {
                    if (RaceEquipmentHiderizer.HiddenItems[bprace].Contains(ItemsFilter.ItemType.Armor) || RaceEquipmentHiderizer.HiddenItems[bprace].Contains(ItemsFilter.ItemType.Shirt)) __result = false;
                }
            }
        }
    }

    // Cloak equipment hider
    [HarmonyPatch(typeof(DollState))]
    internal class Player_GetShowCloak_Patch
    {
        [HarmonyPatch(nameof(DollState.ShowCloak), MethodType.Getter)]
        [HarmonyPostfix]
        public static void Postfix(DollState __instance, ref bool __result)
        {
            var race = __instance.Race;
            if (race == null)
            {
                //Main.log.Log("GetShowCloakPatch: Unit is null");
                return;
            }
            foreach (var bprace in RaceEquipmentHiderizer.HiddenItems.Keys)
            {
                if (race == bprace)
                {
                    if (RaceEquipmentHiderizer.HiddenItems[bprace].Contains(ItemsFilter.ItemType.Shoulders)) __result = false;
                }
            }
        }
    }

    [HarmonyPatch(typeof(UnitUISettings))]
    internal class Player_GetShowCloakUnitUISettings_Patch
    {
        [HarmonyPatch(nameof(UnitUISettings.ShowCloak), MethodType.Getter)]
        [HarmonyPostfix]
        public static void Postfix(UnitUISettings __instance, ref bool __result)
        {
            var race = __instance.Owner.Progression.Race;
            if (race == null)
            {
                //Main.log.Log("GetShowCloakUnitUISettingsPatch: Unit is null");
                return;
            }
            foreach (var bprace in RaceEquipmentHiderizer.HiddenItems.Keys)
            {
                if (race == bprace)
                {
                    if (RaceEquipmentHiderizer.HiddenItems[bprace].Contains(ItemsFilter.ItemType.Shoulders)) __result = false;
                }
            }
        }
    }

    // Handle equipping other items that don't have natively supported hiding and auto-setting some of the hiding UI settings
    [HarmonyPatch(typeof(UnitEntityView), nameof(UnitEntityView.AddItemEquipment), typeof(ItemEntity), typeof(UnitEntityData), typeof(Character))]
    [HarmonyPriority(Priority.VeryLow)]
    public static class UnitEntityView_RaceEquipmentHiderizer_Patch
    {
        public static void Postfix(UnitEntityView __instance, ItemEntity item, UnitEntityData unit, ref Character avatar)
        {
            // First, let's turn hiding on
            foreach (var bprace in RaceEquipmentHiderizer.HiddenItems.Keys)
            {
                if (unit != null && unit.HasFact(bprace))
                {
                    //Main.log.Log("AddItemEquipmentPatch: Found " + bprace.Name);
                    if (RaceEquipmentHiderizer.HiddenItems[bprace].Contains(ItemsFilter.ItemType.Head))
                    {
                        avatar.m_ShowHelmet = false;
                        unit.Descriptor.UISettings.ShowHelm = false;
                    }
                    if (RaceEquipmentHiderizer.HiddenItems[bprace].Contains(ItemsFilter.ItemType.Armor))
                    {
                        avatar.m_ShowCloth = false;
                        unit.Descriptor.UISettings.ShowClassEquipment = false;
                    }
                    if (RaceEquipmentHiderizer.HiddenItems[bprace].Contains(ItemsFilter.ItemType.Shirt))
                    {
                        avatar.m_ShowCloth = false;
                        unit.Descriptor.UISettings.ShowClassEquipment = false;
                    }
                    if (RaceEquipmentHiderizer.HiddenItems[bprace].Contains(ItemsFilter.ItemType.Shoulders))
                    {
                        unit.Descriptor.UISettings.ShowCloak = false;
                    }
                }
            }

            // Next delete any equipped EE identified as hidden
            try
            {
                //Main.log.Log("RaceEquipmentHiderizer: AddItemEquipment patch running");

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
                //Main.log.Log("RaceEquipmentHiderizer: Unit is player faction");

                if (item.Blueprint != null)
                {
                    foreach (var race in RaceEquipmentHiderizer.HiddenItems.Keys)
                    {
                        //Main.log.Log("RaceEquipmentHiderizer: Checking Race: " + race.Name);
                        if (unit.HasFact(race))
                        {
                            
                            
                            //Main.log.Log("RaceEquipmentHiderizer: Found Race");
                            RaceEquipmentHiderizer.HiddenItems.TryGetValue(race, out var types);
                            //Main.log.Log("RaceEquipmentHiderizer: Checking for hidden types");
                            foreach (ItemsFilter.ItemType type in types)
                            {
                                if (type == item.Blueprint.ItemType)
                                {
                                    //Main.log.Log("RaceEquipmentHiderizer: Found hidden item type");
                                    KingmakerEquipmentEntity kingmakerEquipmentEntity = (item != null) ? item.EquipmentEntity : null;
                                    if (kingmakerEquipmentEntity == null)
                                    {
                                        return;
                                    }
                                    Kingmaker.Blueprints.Gender gender = unit.Gender;
                                    Kingmaker.Blueprints.Race actualRace = UnitEntityView.GetActualRace(unit);
                                    foreach (EquipmentEntityLink equipmentEntityLink in kingmakerEquipmentEntity.GetLinks(gender, actualRace))
                                    {
                                        Character character = avatar.Or(null);
                                        if (character != null)
                                        {
                                            //Main.log.Log("RaceEquipmentHiderizer: Removing equipment entity");
                                            character.RemoveEquipmentEntity(equipmentEntityLink);
                                        }
                                    }
                                    return;
                                }
                            }
                            //Main.log.Log("RaceEquipmentHiderizer: Did not find hidden types, now removing EEs directly");
                        }
                    }
                }
                else
                {
                    //Main.log.Log("RaceEquipmentHiderizer: Item blueprint is null.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Main.log.Error(ex.ToString());
                return;
            }
        }
    }
}


