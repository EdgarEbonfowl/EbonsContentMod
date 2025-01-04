using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker;
using Kingmaker.RuleSystem.Rules;
using HarmonyLib;
using System.Data;
using Kingmaker.Items;
using Kingmaker.RuleSystem;
using JetBrains.Annotations;
using Kingmaker.Blueprints.Items.Ecnchantments;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.Items.Slots;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.Class.Kineticist;

namespace EbonsContentMod.Rules
{
    internal class CriticalRangeRevert
    {
        [HarmonyPatch(typeof(RuleCalculateWeaponStats))]
        internal class Player_GetCriticalRange_Patch
        {
            [HarmonyPatch(nameof(RuleCalculateWeaponStats.CriticalRange), MethodType.Getter)]
            [HarmonyPostfix]
            public static void Postfix(RuleCalculateWeaponStats __instance, ref int __result)
            {
                __result = (21 - __instance.Weapon.Blueprint.CriticalRollEdge + __instance.CriticalEdgeBonus) * (__instance.DoubleCriticalEdge ? 2 : 1);
            }
        }
    }
}
