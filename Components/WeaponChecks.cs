using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Components.Base;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EbonsContentMod.Components
{
    
    public class AbilityCasterDeedWeaponCheck : BlueprintComponent, IAbilityCasterRestriction
    {
        public static bool IsDeedWeapon(BlueprintItemWeapon weapon, UnitDescriptor wielder)
        {
            // Identical check for Duelist weapons
            if (weapon.IsMelee && (weapon.Category.HasSubCategory(WeaponSubCategory.Light) || weapon.Category.HasSubCategory(WeaponSubCategory.OneHandedPiercing) || (wielder.State.Features.DuelingMastery && weapon.Category == WeaponCategory.DuelingSword) || wielder.Ensure<UnitPartDamageGrace>().HasEntry(weapon.Category)))
            {
                return true;
            }
            return false;
        }

        public string GetAbilityCasterRestrictionUIText()
        {
            return "Requires swashbuckler weapon";
        }

        public bool IsCasterRestrictionPassed(UnitEntityData caster)
        {
            return (IsDeedWeapon(caster.Body.PrimaryHand.Weapon.Blueprint, caster.Descriptor) || IsDeedWeapon(caster.Body.SecondaryHand.Weapon.Blueprint, caster.Descriptor));
        }
    }
}
