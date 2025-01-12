using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonsContentMod.Utilities
{
    internal class CreateAssetLinks
    {
        internal static Dictionary<string, string> Bundles_Dict = new Dictionary<string, string>
        {
            {"6ef34af23d7cd4244af15185bbe57f4d", "GoblinTest_content"},
            {"6e70355518fe22942a8fd60ca740da4a", "GoblinTest_content"},
            {"a89270e53d026e347978e03fc397bd8e", "GoblinTest_content"},
            {"3899c5b49765e4441b4e10c3ffd23842", "GoblinTest_content"},
            {"3da4fa2032b9c3344b0c0f55d062515c", "GoblinTest_content"},
            {"41fb4c393c6a67141b6e7d48911e38a1", "GoblinTest_content"},
            {"87bb557ee94111e4e8c37c7376c2682f", "GoblinTest_content"},
            {"db104eb6b6fdbe445b6843207d003724", "GoblinTest_content"},
            {"35fd267d9f89d384684ead4557431336", "GoblinTest_content"},
            {"1dd6452b08f0cce4c9b03ec751982b4d", "GoblinTest_content"},
            {"1cfe6e9ca33a45044b87e80270517f63", "GoblinTest_content"}
        };

        public static void LoadAllSettings()
        {
            try
            {
                LoadAssetLinks();
            }
            catch (Exception e)
            {
                Main.log.LogException("CreateAssetLinks.LoadAllSettings: Error loading asset links!", e);
            }

        }

        public static Dictionary<string, string> AssetsInBundles = new();
        public static HashSet<string> Bundles = new();

        private static void LoadAssetLinks()
        {
            foreach (var item in Bundles_Dict)
            {
                var guid = item.Key;
                var bundle = item.Value;

                Bundles.Add(bundle);
                AssetsInBundles[guid] = bundle;
            }

            Main.log.Log($"CreateAssetLinks.LoadAssetLinks: Found {AssetsInBundles.Count} asset links in {Bundles.Count} bundles");
        }
    }
}
