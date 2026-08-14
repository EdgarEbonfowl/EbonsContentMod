using Kingmaker.Blueprints.Items.Equipment;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BlueprintCore.Blueprints.Configurators.Items.Equipment;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker;
using Kingmaker.ResourceLinks;
using static TabletopTweaks.Core.Utilities.VenderTools;

namespace EbonsContentMod.Utilities
{
    internal class UsableItemsHelperators
    {
        public enum PotionColor : int
        {
            Blue,
            Cyan,
            Green,
            Red,
            Yellow,
            Black,
        }

        public static PotionColor GetPotionColorFromSchool(BlueprintAbility spell)
        {
            var color = spell.School switch
            {
                Kingmaker.Blueprints.Classes.Spells.SpellSchool.Abjuration => PotionColor.Cyan,
                Kingmaker.Blueprints.Classes.Spells.SpellSchool.Conjuration => PotionColor.Red,
                Kingmaker.Blueprints.Classes.Spells.SpellSchool.Divination => PotionColor.Cyan,
                Kingmaker.Blueprints.Classes.Spells.SpellSchool.Enchantment => PotionColor.Yellow,
                Kingmaker.Blueprints.Classes.Spells.SpellSchool.Evocation => PotionColor.Blue,
                Kingmaker.Blueprints.Classes.Spells.SpellSchool.Illusion => PotionColor.Blue,
                Kingmaker.Blueprints.Classes.Spells.SpellSchool.Necromancy => PotionColor.Black,
                Kingmaker.Blueprints.Classes.Spells.SpellSchool.Transmutation => PotionColor.Green,
                _ => PotionColor.Blue
            };

            return color;
        }

        private static Sprite GetPotionIcon(PotionColor color, int spellLevel)
        {
            return color switch
            {
                PotionColor.Blue => spellLevel switch
                {
                    1 => ItemEquipmentUsableRefs.PotionOfOfVanish.Reference.Get().Icon,
                    2 => ItemEquipmentUsableRefs.PotionOfBlur.Reference.Get().Icon,
                    3 => ItemEquipmentUsableRefs.PotionOfInvisibility.Reference.Get().Icon,
                    4 => ItemEquipmentUsableRefs.PotionOfDisplacement.Reference.Get().Icon,
                    5 => ItemEquipmentUsableRefs.PotionOfInvisibilityGreater.Reference.Get().Icon,
                    _ => ItemEquipmentUsableRefs.PotionOfInvisibilityGreater.Reference.Get().Icon
                },
                PotionColor.Cyan => spellLevel switch
                {
                    1 => ItemEquipmentUsableRefs.PotionOfRemoveFear.Reference.Get().Icon,
                    2 => ItemEquipmentUsableRefs.PotionOfGreaterInvisibility.Reference.Get().Icon,
                    3 => ItemEquipmentUsableRefs.PotionOfProtectionFromSpells.Reference.Get().Icon,
                    4 => ItemEquipmentUsableRefs.PotionOfProtectionFromSpells.Reference.Get().Icon,
                    5 => ItemEquipmentUsableRefs.PotionOfHaste.Reference.Get().Icon,
                    _ => ItemEquipmentUsableRefs.PotionOfHaste.Reference.Get().Icon
                },
                PotionColor.Green => spellLevel switch
                {
                    1 => ItemEquipmentUsableRefs.PotionOfEnlargePerson.Reference.Get().Icon,
                    2 => ItemEquipmentUsableRefs.PotionOfFeatherStep.Reference.Get().Icon,
                    3 => ItemEquipmentUsableRefs.PotionOfBarkskin.Reference.Get().Icon,
                    4 => ItemEquipmentUsableRefs.PotionOfBarkskin9.Reference.Get().Icon,
                    5 => ItemEquipmentUsableRefs.PotionOfStoneskin.Reference.Get().Icon,
                    _ => ItemEquipmentUsableRefs.PotionOfStoneskin.Reference.Get().Icon
                },
                PotionColor.Red => spellLevel switch
                {
                    1 => ItemEquipmentUsableRefs.PotionOfCureLightWounds.Reference.Get().Icon,
                    2 => ItemEquipmentUsableRefs.PotionOfCureLightWounds.Reference.Get().Icon,
                    3 => ItemEquipmentUsableRefs.PotionOfRage.Reference.Get().Icon,
                    4 => ItemEquipmentUsableRefs.PotionOfRage.Reference.Get().Icon,
                    5 => ItemEquipmentUsableRefs.PotionOfHeal.Reference.Get().Icon,
                    _ => ItemEquipmentUsableRefs.PotionOfHeal.Reference.Get().Icon
                },
                PotionColor.Yellow => spellLevel switch
                {
                    1 => ItemEquipmentUsableRefs.PotionOfBlessWeapon.Reference.Get().Icon,
                    2 => ItemEquipmentUsableRefs.PotionOfAid.Reference.Get().Icon,
                    3 => ItemEquipmentUsableRefs.PotionOfRemoveParalysis.Reference.Get().Icon,
                    4 => ItemEquipmentUsableRefs.PotionOfRemoveBlindness.Reference.Get().Icon,
                    5 => ItemEquipmentUsableRefs.PotionOfBreathOfLife.Reference.Get().Icon,
                    _ => ItemEquipmentUsableRefs.PotionOfBreathOfLife.Reference.Get().Icon
                },
                PotionColor.Black => spellLevel switch
                {
                    1 => ItemEquipmentUsableRefs.PotionOfInflictLightWounds.Reference.Get().Icon,
                    2 => ItemEquipmentUsableRefs.PotionOfInflictLightWounds.Reference.Get().Icon,
                    3 => ItemEquipmentUsableRefs.PotionOfInflictLightWounds.Reference.Get().Icon,
                    4 => ItemEquipmentUsableRefs.PotionOfInflictCriticalWounds.Reference.Get().Icon,
                    5 => ItemEquipmentUsableRefs.PotionOfInflictCriticalWounds.Reference.Get().Icon,
                    _ => ItemEquipmentUsableRefs.PotionOfInflictCriticalWounds.Reference.Get().Icon
                },
                _ => ItemEquipmentUsableRefs.PotionOfOfVanish.Reference.Get().Icon
            };
        }

        private static int GetPotionCost(int spellLevel)
        {
            return spellLevel switch
            {
                0 => 25,
                1 => 50,
                2 => 300,
                3 => 750,
                4 => 1400,
                5 => 2250,
                6 => 3300,
                _ => 25
            };
        }

        private static PrefabLink GetPotionPrefab(PotionColor color)
        {
            var AssetID = color switch
            {
                PotionColor.Blue => "7b2a2ed1f3284224c804038a713c391f",
                PotionColor.Cyan => "e805c0e867b583b4f8c24b2b045b5be3",
                PotionColor.Green => "51097fd1d322c0d41b33dac27da51bf4",
                PotionColor.Red => "8de60d0edae1a1a47ba9fee1e1d97e32",
                PotionColor.Yellow => "9b57d6e56c83fc14d9580c6f766fbe20",
                PotionColor.Black => "9a25623afd4d41a4e8e04b04f970c11f",
                _ => "7b2a2ed1f3284224c804038a713c391f"
            };
            return new PrefabLink()
            {
                AssetId = AssetID
            };
        }

        private static void AddPotionToCraftRoot(BlueprintItemEquipmentUsable potion)
        {
            if (potion.Type != UsableItemType.Potion) { return; }

            Game.Instance.BlueprintRoot.CraftRoot.m_PotionsItems.Add(potion.ToReference<BlueprintItemEquipmentUsableReference>());
        }

        /*private static void AddPotionToBFTCraftRoot(BlueprintItemEquipmentUsable potion)
        {
            if (potion.Type != UsableItemType.Potion) { return; }

            BFTCraftRoot.m_PotionsItems.Add(potion.ToReference<BlueprintItemEquipmentUsableReference>());
        }*/

        public static string RemoveWhitespace(string str)
        {
            return string.Join("", str.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
        }

        // Creates potions when given a spell ability blueprint, spell level, and new GUID
        // Adds potions to crafting and to leveled vendor lists automaically
        // Cannot dynamically set caster level when crafting per tabletop :(
        // To deal with this shortcoming, I have added an optional caster level argument which allows for creating lowest, medium and high versions where appropriate
        // Now with mythic potions! Can be crafted, but do not show up in vendor inventories.
        // Now can account for Brown Fur Transmuter being able to change the range of transmutation spells and therefore create potions from personal range transmutations
        public static BlueprintItemEquipmentUsable CreatePotionFromSpell(BlueprintAbility spell, int iSpellLevel, string sGuid, int iCasterLevel = -1, bool mythic = false, bool BFT = false)
        {
            var spellname = RemoveWhitespace(spell.name);
            string name = spellname + "Potion";
            if (iCasterLevel != -1) name = name + iCasterLevel.ToString();
            if (BFT) name = name + "BFT";
            var color = GetPotionColorFromSchool(spell);
            var prefab = GetPotionPrefab(color);
            var icon = GetPotionIcon(color, iSpellLevel);
            if (iCasterLevel == -1) iCasterLevel = iSpellLevel * 2 - 1;
            var cost = GetPotionCost(iSpellLevel);
            int DC = 10 + iSpellLevel + iSpellLevel / 2;

            var Potion = ItemEquipmentUsableConfigurator.New(name, sGuid)
                .CopyFrom(ItemEquipmentUsableRefs.PotionOfCureLightWounds)
                .SetBeltItemPrefab(prefab)
                .SetIcon(icon)
                .SetAbility(spell)
                .SetCasterLevel(iCasterLevel)
                .SetSpellLevel(iSpellLevel)
                .SetDC(DC)
                .SetCost(cost)
                .Configure();

            AddPotionToCraftRoot(Potion);

            //if (BFT) AddPotionToBFTCraftRoot(Potion);

            if (!mythic && !BFT) AddPotionToLeveledVenders(Potion);

            return Potion;
        }

        private static int GetScrollCost(int spellLevel)
        {
            return spellLevel switch
            {
                0 => 13,
                1 => 25,
                2 => 150,
                3 => 375,
                4 => 700,
                5 => 1125,
                6 => 1650,
                7 => 2275,
                8 => 3000,
                9 => 3825,
                10 => 5000,
                _ => 0
            };
        }

        private static void AddScrollToCraftRoot(BlueprintItemEquipmentUsable scroll)
        {
            if (scroll.Type != UsableItemType.Scroll) { return; }

            Game.Instance.BlueprintRoot.CraftRoot.m_ScrollsItems.Add(scroll.ToReference<BlueprintItemEquipmentUsableReference>());
        }

        // Creates scrolls when given a spell ability blueprint, spell level, icon, and new GUID
        // Adds scrolls to crafting and to leveled vendor lists automaically
        // Cannot dynamically set caster level when crafting per tabletop :(
        // To deal with this shortcoming, I have added an optional caster level argument which allows for creating lowest, medium and high versions where appropriate
        // Now with mythic scrolls! Can be crafted, but do not show up in vendor inventories.
        public static BlueprintItemEquipmentUsable CreateScrollFromSpell(BlueprintAbility spell, int iSpellLevel, string sGuid, Sprite icon, int iCasterLevel = -1, bool mythic = false)
        {
            var scrollItemPrefab = new PrefabLink()
            {
                AssetId = "d711efe72d029364a9ad378d5f0955c0"
            };

            var spellname = RemoveWhitespace(spell.name);
            string name = spellname + "Scroll";
            if (iCasterLevel != -1) name = name + iCasterLevel.ToString();
            else iCasterLevel = iSpellLevel * 2 - 1;
            var cost = GetScrollCost(iSpellLevel);
            int DC = 10 + iSpellLevel + iSpellLevel / 2;

            var Scroll = ItemEquipmentUsableConfigurator.New(name, sGuid)
                .CopyFrom(ItemEquipmentUsableRefs.ScrollOfCureLightWounds)
                .SetBeltItemPrefab(scrollItemPrefab)
                .SetIcon(icon)
                .SetAbility(spell)
                .SetCasterLevel(iCasterLevel)
                .SetSpellLevel(iSpellLevel)
                .SetDC(DC)
                .SetCost(cost)
                .AddCopyScroll(spell)
                .Configure();

            AddScrollToCraftRoot(Scroll);

            if (!mythic) AddScrollToLeveledVenders(Scroll);

            return Scroll;
        }
    }
}
