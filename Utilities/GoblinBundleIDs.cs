using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonsContentMod.Utilities
{
    internal class GoblinBundleIDs
    {
        internal static Dictionary<string, string> Bundle_IDs_Dict = new Dictionary<string, string>
        {
            {"CP_GoblinTest_Eyes", "6ef34af23d7cd4244af15185bbe57f4d"},
            {"CP_GoblinTest_Skin", "6e70355518fe22942a8fd60ca740da4a"},
            {"EE_Body01_Desat_M_GB", "a89270e53d026e347978e03fc397bd8e"},
            {"EE_Head01_70_M_GB", "3899c5b49765e4441b4e10c3ffd23842"},
            {"EE_Head01_75_M_GB", "3da4fa2032b9c3344b0c0f55d062515c"},
            {"EE_Head01_80_M_GB", "41fb4c393c6a67141b6e7d48911e38a1"}, // This one
            {"EE_Head01_85_M_GB", "87bb557ee94111e4e8c37c7376c2682f"},
            {"EE_Head01_90_M_GB", "db104eb6b6fdbe445b6843207d003724"},
            {"EE_Head01_95_M_GB", "35fd267d9f89d384684ead4557431336"}
        };

        internal static string Get_Bundle_Asset_ID(string key)
        {
            if (Bundle_IDs_Dict.TryGetValue(key, out var id))
            {
                return id;
            }
            else
            {
                return "";
            }
        }
    }
}
