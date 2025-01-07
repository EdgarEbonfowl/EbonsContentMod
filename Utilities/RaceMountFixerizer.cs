using BlueprintCore.Blueprints.References;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Visual.Mounts;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using Unity.Burst.CompilerServices;
using UnityEngine;

namespace EbonsContentMod.Utilities
{
    internal class RaceMountFixerizer
    {
        internal const string HumanGuid = "0a5d473ead98b0646b94495af250fdc4";
        internal const string ElfGuid = "25a5878d125338244896ebd3238226c8";
        internal const string DwarfGuid = "c4faf439f0e70bd40b5e36ee80d06be7";
        internal const string HalflingGuid = "b0c3ef2729c498f47970bb50fa1acd30";
        internal const string HalfOrcGuid = "1dc20e195581a804890ddc74218bfd8e";
        internal const string GnomeGuid = "ef35a22c9a27da345a4528f0d5889157";
        internal const string HalfElfGuid = "b3646842ffbd01643ab4dac7479b20b0";
        internal const string AasimarGuid = "b7f02ba92b363064fb873963bec275ee";
        internal const string TieflingGuid = "5c4e42124dc2b4647af6e36cf2590500";
        internal const string OreadGuid = "4d4555326b9b7144f93be1ea61337cd7";
        internal const string DhampirGuid = "64e8b7d5f1ae91d45bbf1e56a3fdff01";
        internal const string KitsuneGuid = "fd188bb7bb0002e49863aec93bfb9d99";

        private static readonly Dictionary<BlueprintRace, BlueprintRace> RaceOriginals = new();

        public static void AddRaceToMountFixes(BlueprintRace newrace, BlueprintRace copiedrace)
        {
            RaceOriginals[newrace] = copiedrace;
        }

        // Mount Points
        [HarmonyPatch]
        static class Patches
        {
            [HarmonyPatch(typeof(MountOffsets), nameof(MountOffsets.GetMountOffsets))]
            [HarmonyPostfix]
            static void GetMountOffsets_Prefix(BlueprintRace race, MountOffsets __instance, ref RaceMountOffsetsConfig.MountOffsetData __result)
            {
                if (__result == null && RaceOriginals.TryGetValue(race, out var originalRace))
                {
                    __result = __instance.GetMountOffsets(originalRace);
                }
            }
        }
    }
}
