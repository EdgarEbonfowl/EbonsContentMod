using System;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.ActivatableAbilities;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;
using EbonsContentMod.Components;

namespace EbonsContentMod.Components
{
    // Token: 0x020022E8 RID: 8936
    [ComponentName("Count hit dice as class levels for prerequisites")]
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    public class CharacterLevelsForPrerequisites : UnitFactComponentDelegate
    {
        [SerializeField]
        public BlueprintCharacterClassReference m_FakeClass;

        [SerializeField]
        public BlueprintFeatureSelectionReference m_ForSelection;

        public double Modifier = 1.0;

        public int Summand;

        public BlueprintCharacterClass FakeClass => m_FakeClass?.Get();

        public BlueprintFeatureSelection ForSelection => m_ForSelection?.Get();
    }

    [HarmonyPatch(typeof(PrerequisiteClassLevel), "GetClassLevel")]
    internal static class PrerequisiteClassLevel_GetClassLevel_Patch
    {
        private static void Postfix(
            PrerequisiteClassLevel __instance,
            UnitDescriptor unit,
            ref int __result)
        {
            if (unit == null)
                return;

            foreach (CharacterLevelsForPrerequisites component in
                     unit.Progression.Features
                         .SelectFactComponents<CharacterLevelsForPrerequisites>())
            {
                if (component.FakeClass != __instance.CharacterClass)
                    continue;

                if (component.ForSelection != null
                    && !component.ForSelection.AllFeatures.Any(feature =>
                        feature != null
                        && feature.AssetGuid
                            == __instance.OwnerBlueprint.AssetGuid))
                {
                    continue;
                }

                int hitDiceAsClassLevels =
                    (int)(
                        component.Modifier
                        * unit.Progression.CharacterLevel
                        + component.Summand);

                __result = Math.Max(__result, hitDiceAsClassLevels);
            }
        }
    }
}
