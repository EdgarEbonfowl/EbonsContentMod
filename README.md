# Ebon's Content Mod
This mod was created to add new content from tabletop to Wrath of the Righteous that wasn't represented in the other major content mods. This requires [TabletopTweaks-Core](https://github.com/Vek17/TabletopTweaks-Core "TabletopTweaks-Core"), [WrathPatches](https://github.com/microsoftenator2022/WrathPatches "WrathPatches") (I'm not entirely sure why, but Micro says it does, so it does), and is partially dependent upon [TabletopTweaks-Base](https://github.com/Vek17/TabletopTweaks-Base "TabletopTweaks-Base"), though it will work without it.

If you want to read through my rationale for any implementation decisions I had to make along the way, please see my [Modder Notes](https://github.com/EdgarEbonfowl/EbonsContentMod/blob/main/ModderNotes.md "Modder Notes").

## Added Races
This mod adds the following races to the game and they should work like the default races. Note, I have not completed a playthrough with these races, so I cannot say for sure that you will not encounter problems. You may experience some strange behavior, especially in dialog, where the race type either is not recognized or, more likely, is recognized as some other race (an NPC calling your drow character an elf, for instnace). That being said, if you encounter bugs, please report them!

**If you don't see your favorite race here:** the way I do races involves cloning, recoloring, and then cobbling together base game assets (think Mr. Potatohead plus a paint brush), so only races that can be made from vanilla assets are included here. That means no tengu, no catfolk, etc. If you want me to make a race that would look acceptable with vanilla assets let me know OR if you have some 3D modeling skills and want to submit custom assets, I can make your race. Currently, only the goblin is using anything custom. Playable Mongrels and Ganzi will be coming in a future update.

**Note on colors:** the method I use to add custom color textures to the game causes my color ramps for skin color, eye color, hair color, etc. to not show their color swatch in the character creator, showing up as empty circles instead. I promise, they still work, just click through them and you will see.

- [Sylph](https://www.d20pfsrd.com/races/other-races/featured-races/arg-sylph/ "Sylph")
- [Undine](https://www.d20pfsrd.com/races/other-races/featured-races/arg-undine/ "Undine")
- [Duergar](https://www.d20pfsrd.com/races/other-races/uncommon-races/arg-duergar/ "Duergar")
- [Svirfneblin](https://www.d20pfsrd.com/races/other-races/uncommon-races/arg-svirfneblin/ "Svirfneblin")
- [Samsaran](https://www.d20pfsrd.com/races/other-races/uncommon-races/arg-samsaran/ "Samsaran")
- [Strix](https://www.d20pfsrd.com/races/other-races/uncommon-races/arg-strix/ "Strix")
- [Ifrit](https://www.d20pfsrd.com/races/other-races/featured-races/arg-ifrit/ "Ifrit")
- [Suli](https://www.d20pfsrd.com/races/other-races/uncommon-races/arg-suli/ "Suli")
- [Fetchling](https://www.d20pfsrd.com/races/other-races/featured-races/arg-fetchling/ "Fetchling")
- [Drow](https://www.d20pfsrd.com/races/other-races/featured-races/arg-drow/ "Drow")
- [Changeling](https://www.d20pfsrd.com/races/other-races/uncommon-races/arg-changeling/ "Changeling")
  - Should be female only. For now, male changelings are playable.
- [Kuru](https://www.d20pfsrd.com/races/other-races/more-races/race-points-unknown/kuru-rp/ "Kuru")
- [Vishkanya](https://www.d20pfsrd.com/races/other-races/uncommon-races/arg-vishkanyas/ "Vishkanya")
- [Shabti](https://www.d20pfsrd.com/races/other-races/more-races/advanced-races-11-20-rp/shabti/ "Shabti")
- [Android](https://www.d20pfsrd.com/races/other-races/more-races/advanced-races-11-20-rp/android-16-rp/ "Android")
- [Skinwalker](https://www.d20pfsrd.com/races/other-races/more-races/standard-races-1-10-rp/skinwalkers-10-rp/ "Skinwalker")
- [Goblin](https://www.d20pfsrd.com/races/other-races/featured-races/arg-goblin/ "Goblin")
  - Female head is in the works. For now, play male goblin.
  - Helmets/goggles will not look correct and should be hidden for now. Working on a permanent hiding solution for goblins.
- [Orc](https://www.d20pfsrd.com/races/other-races/featured-races/arg-orc/ "Orc")
- [Rougarou](https://www.d20pfsrd.com/races/other-races/more-races/race-points-unknown/rougarou-player-characters/ "Rougarou")
- [Nagaji](https://www.d20pfsrd.com/races/other-races/uncommon-races/arg-nagaji/ "Nagaji")
- [Mongrel](https://www.d20pfsrd.com/bestiary/monster-listings/monstrous-humanoids/mongrelman/ "Mongrel")

Here is what all the new races look like:

![ECM1](https://github.com/user-attachments/assets/6caac997-715a-4b6a-802a-4da565a8e65e)
![ECM2](https://github.com/user-attachments/assets/3b50acfa-ee31-42ff-87dd-c377becbceb1)
![ECM3](https://github.com/user-attachments/assets/ccd5e7d3-6ac9-485d-a086-cf29f1609de9)
![ECM4](https://github.com/user-attachments/assets/8582439b-3204-46e5-a1c4-1689980e4a8a)
![ECM5](https://github.com/user-attachments/assets/29d1714c-7684-49e2-a188-3e47fc057c9a)
![ECM6](https://github.com/user-attachments/assets/86209339-c40e-404a-aa2f-50fca795ca74)
![ECM7](https://github.com/user-attachments/assets/16249231-2e62-4e01-bacd-f7b1fe65bd57)
![ECM8](https://github.com/user-attachments/assets/7a2253b8-8d51-4aa3-be85-b09581c36857)
![ECM11](https://github.com/user-attachments/assets/ea765d50-3259-478b-a636-0c6bdd461e3e)
![ECM12](https://github.com/user-attachments/assets/51f6066e-aba0-4463-9d54-23406ea16fec)

Android circuitry colors are linked to the eye color - how fun!
  
![ECM9](https://github.com/user-attachments/assets/9012c1d3-8344-4542-9f15-da893f0a9d1e)

Different Change Shape forms for the skinwalker:
  
![ECM10](https://github.com/user-attachments/assets/7e685556-bd92-49a8-a8ae-87f8d01a4812)

## Archetypes
- **Arcanist**
  - [Collegiate Initiate](https://www.d20pfsrd.com/classes/hybrid-classes/arcanist/archetypes/paizo-arcanist-archetypes/collegiate-initiate-arcanist/ "Collegiate Initiate")
- **Monk**
  - [Hungry Ghost Monk](https://www.d20pfsrd.com/classes/core-classes/monk/archetypes/paizo-monk-archetypes/hungry-ghost-monk/ "Hungry Ghost Monk")
- **Sorcerer**
  - [Eldritch Scrapper](https://www.d20pfsrd.com/classes/core-classes/sorcerer/archetypes/paizo-sorcerer-archetypes/eldritch-scrapper/ "Eldritch Scrapper")
 
## Bloodlines
- [Orc Sorcerer Bloodline](https://www.d20pfsrd.com/classes/core-classes/sorcerer/bloodlines/bloodlines-from-paizo/orc-bloodline/ "Orc Sorcerer Bloodline")
  - Requires TabletopTweaks-Base

## Magus Arcana
- [Arcane Deed](https://www.d20pfsrd.com/classes/base-classes/magus/magus-arcana/paizo-magus-arcana/arcane-deed-ex/ "Arcane Deed")
- [Flamboyant Arcana](https://www.d20pfsrd.com/classes/base-classes/magus/magus-arcana/paizo-magus-arcana/flamboyant-arcana-ex "Flamboyant Arcana")

## Arcanist Exploits
- [Arcane Discoveries](https://www.d20pfsrd.com/classes/core-classes/wizard/arcane-discoveries/ "Arcane Discoveries") are added to the exploit selection list per tabletop. You only can pick one, so choose wisely. You must have TabletopTweaks-Base installed for this feature to take effect. The discoveries that are based on spell school are really only useful for the School Savant archetype which is added by [HomebrewArchetypes](https://www.nexusmods.com/pathfinderwrathoftherighteous/mods/279 "HomebrewArchetypes"). Integrated arcane discoveries:
  - [Alchemical Affinity](https://www.d20pfsrd.com/classes/core-classes/wizard/arcane-discoveries/arcane-discoveries-paizo/alchemical-affinity/ "Alchemical Affinity")
  - [Faith Magic](https://www.d20pfsrd.com/classes/core-classes/wizard/arcane-discoveries/arcane-discoveries-paizo/faith-magic/ "Faith Magic")
  - [Idealize](https://www.d20pfsrd.com/classes/core-classes/wizard/arcane-discoveries/arcane-discoveries-paizo/idealize-su/ "Idealize")
  - [Knowledge Is Power](https://www.d20pfsrd.com/classes/core-classes/wizard/arcane-discoveries/arcane-discoveries-paizo/knowledge-is-power-ex/ "Knowledge Is Power")
  - [Opposition Research](https://www.d20pfsrd.com/classes/core-classes/wizard/arcane-discoveries/arcane-discoveries-paizo/opposition-research/ "Opposition Research")
  - [Yuelral's Blessing](https://aonprd.com/WizardArcaneDiscoveries.aspx "Yuelral's Blessing")
  - If you have [MysticalMayhem](https://gitgud.io/Kreaddy/mysticalmayhem "MysticalMayhem") the following arcane discoveries are also added:
    - School Expertise - this is homebrew, but not anything crazy so I added it
    - [Staff-Like Wand](https://www.d20pfsrd.com/classes/core-classes/wizard/arcane-discoveries/arcane-discoveries-paizo/staff-like-wand/ "Staff-Like Wand")
   
## Ability Fixes
- **Come And Get Me!** barbarian power now works correctly with the skald's raging song. The vanilla version only affected the skald, not teammates. It will be disabled any time a teammate steps out of the song's aura then steps back in, so be aware of that.
- **Discordant Voice** feat now applies to any ally within the area of effect of any bard song rather than just the beneficial ones.

## Mechanics Fixes/Changes
- Multi-projectile spells (Hellfire Ray, Scorching Ray, etc.) that should be able to assign projectiles to different targets per tabletop rules will now switch to a new target if the current target dies, rather than just dumping all the extra projectiles into the first target's corpse. All tabletop targeting rules are respected - for instance, no two targets can be more than 30 feet apart for Hellfire Ray. There was some early inconsistency here, but I think I have corrected any flaws in the implementation and my current testing seems to indicate that this consistently works as intended - please submit an issue report if you find otherwise.

## Installation
1. Download and install [Unity Mod Manager](https://github.com/newman55/unity-mod-manager).
2. Drop the zip file from the relaease into the "Drop Zip Files Here" panel in the Unity Mod Manager UI in the "Mods" tab.

**Note:** This require the Doorstop installation method for Unity Mod Manager, most people do it this way, but if yours is installed using the Assembly method you can do one of these (you can see which method you used at the bottom of the "Install" tab):
1. Uninstall UMM and reinstall using the Doorstop method
2. Use WrathPatches to upgrade the game's Harmony version
3. Copy `Harmony` from `Wrath_Data\Managed\UnityModManager` to `Wrath_Data\Managed`

## Other Mods
* This version will not play nice with Worldcrawl. If you use that mod, I have released a Worldcrawl-compatible version over on [Nexus Mods](https://www.nexusmods.com/pathfinderwrathoftherighteous/mods/758?tab=files "Nexus Mods"). The Worldcrawl version does not have the Collegiate Initiate archetype, but is otherwise unchanged. I will work on a solution that preserves the archetype, but for now, use that version if you use Worldcrawl.
* The cloned parts created for the new races will not show up as editable options in Visual Adjustments 2. Everything else still will work fine. Maybe one day there will be a meeting of the minds to sort this issue out, but for now, know that is a limitation.

## Acknowledgements
* WittleWolfie for [BlueprintCore](https://github.com/WittleWolfie/WW-Blueprint-Core/tree/main), an API that really speeds up the process.
* DarthParametric for contributing the custom assets for the goblin model, generally giving me a lot of help, and having a cool screen name.
* The modding community on [Discord](https://discord.com/invite/owlcat), especially Micro who has helped me a ton even if his code is too complex to be human-readable.
