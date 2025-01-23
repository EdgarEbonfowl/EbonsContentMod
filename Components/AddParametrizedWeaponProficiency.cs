using System;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Items.Slots;
using Kingmaker.UnitLogic;
using UnityEngine;
using UnityEngine.Serialization;

namespace EbonsContentMod.Components
{
    // Token: 0x02002347 RID: 9031
    [AllowedOn(typeof(BlueprintParametrizedFeature), false)]
    [TypeId("b24cb50ee67d46c4db9961c8a6947b02")]
    public class AddParametrizedWeaponProficiency : UnitFactComponentDelegate
    {
        public override void OnActivate()
        {
            if (base.Param == null || base.Param.Value.WeaponCategory == null)
            {
                return;
            }
            WeaponCategory value = base.Param.Value.WeaponCategory.Value;
            base.Owner.Proficiencies.Add(value);
        }
    }
}
