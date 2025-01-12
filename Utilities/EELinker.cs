using HarmonyLib;
using Kingmaker.Blueprints.Classes;
using Kingmaker.ResourceLinks;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.Visual.Mounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonsContentMod.Utilities
{
    internal class EELinker
    {
        private static readonly Dictionary<BlueprintRace, EquipmentEntityLink[]> EyeLinkEELs = new();

        private static readonly Dictionary<BlueprintRace, EquipmentEntityLink[]> SkinLinkEELs = new();

        private static readonly Dictionary<BlueprintRace, EquipmentEntityLink[]> HairLinkEELs = new();

        public static void RegisterEyeLink(BlueprintRace newrace, EquipmentEntityLink[] EELinks)
        {
            EyeLinkEELs[newrace] = EELinks;
        }

        public static void RegisterSkinLink(BlueprintRace newrace, EquipmentEntityLink[] EELinks)
        {
            SkinLinkEELs[newrace] = EELinks;
        }

        public static void RegisterHairLink(BlueprintRace newrace, EquipmentEntityLink[] EELinks)
        {
            HairLinkEELs[newrace] = EELinks;
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
        }
    }
}
