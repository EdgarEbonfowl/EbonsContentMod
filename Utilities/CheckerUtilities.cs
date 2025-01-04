using Kingmaker.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using UnityModManagerNet;

namespace EbonsContentMod.Utilities
{
    public static class CheckerUtilities
    {
        /// <summary>
        /// Determines if a given mod is loaded.
        /// </summary>
        /// <remarks>
        /// <para>
        /// ModID = String identifier of the mod to check.
        /// </para>
        /// </remarks>
        /// <returns>
        /// True if the mod is active.
        /// </returns>
        public static bool GetModActive(string ModID)
        {
            return UnityModManager.FindMod(ModID) is { } mod && mod.Active;
        }

        /// <summary>
        /// Determines if a given blueprint has been created.
        /// </summary>
        /// <remarks>
        /// <para>
        /// guid = GUID string for the checked blueprint.
        /// </para>
        /// </remarks>
        /// <returns>
        /// True if the blueprint has been created.
        /// </returns>
        public static bool GetBlueprintCreated(string guid)
        {
            var Check = BlueprintTools.GetBlueprint<BlueprintScriptableObject>(guid);
            if (Check == null) return false;
            else return true;
        }
    }
}
