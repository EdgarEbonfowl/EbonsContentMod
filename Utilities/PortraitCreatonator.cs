using BlueprintCore.Blueprints.Configurators;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Root;
using Kingmaker.Enums;
using Kingmaker.ResourceLinks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonsContentMod.Utilities
{
    internal class PortraitCreatonator
    {
        public static string CreatePortraitDirectory(string raceFolder)
        {
            return Path.Combine(
                Main.ModPath,
                "Portraits",
                raceFolder);
        }
        
        public static void RegisterRacePortrait(string name, string guid, BlueprintRace race, Gender sex, string folderName, PortraitCategory category = PortraitCategory.Wrath)
        {
            if (!Main.Settings.Portraits)
            {
                return;
            }

            var portraitDirectory = CreatePortraitDirectory(folderName);
            
            var portrait = PortraitConfigurator.New(name, guid)
                .SetData(new PortraitData(portraitDirectory)
                {
                    PortraitCategory = category
                })
                .AddPortraitDollSettings(sex, race: race)
                .Configure();

            RaceRecolorizer.AppendInPlace(ref BlueprintRoot.Instance.CharGen.m_Portraits, portrait.ToReference<BlueprintPortraitReference>());
        }
    }
}
