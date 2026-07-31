using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;

namespace EbonsContentMod.Menu;

public class Settings : UnityModManager.ModSettings
{
    // Races
    public bool Races = true;

    // Portraits
    public bool Portraits = true;

    // Archetypes
    public bool HungryGhostMonk = true;
    public bool CollegiateInitiate = true;
    public bool EldritchScrapper = true;

    // Abilities
    public bool FlamboyantArcana = true;
    public bool ArcaneDeed = true;
    public bool Bloodlines = true;
    public bool ArcanistExploits = true;
    public bool FaithMagic = true;

    // Fixes
    public bool ComeAndGetMe = true;
    public bool DiscordantVoice = true;
    public bool MultiProjectileSpellFix = true;

    // Wild Talents
    public bool AirsLeap = true;
    public bool ClockworkHeart = true;
    public bool KineticForm = true;
    public bool SparkOfInnovation = true;
    public bool SparkOfLife = true;
    public bool WingsOfAir = true;

    public override void Save(UnityModManager.ModEntry modEntry)
    {
        Save(this, modEntry);
    }
}