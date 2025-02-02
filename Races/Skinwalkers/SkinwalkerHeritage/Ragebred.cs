using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using EbonsContentMod.Utilities;
using Kingmaker.Blueprints.Classes;
using Kingmaker.ResourceLinks;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.EntitySystem.Stats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Actions.Builder;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.Commands.Base;
using TabletopTweaks.Core.Utilities;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Blueprints;
using BlueprintCore.Actions.Builder.ContextEx;
using EbonsContentMod.Components;

namespace EbonsContentMod.Races.Skinwalkers.SkinwalkerHeritage
{
    internal class Ragebred
    {
        internal const string RagebredDisplayName = "Ragebred.Name";
        private static readonly string RagebredDescription = "Ragebred.Description";

        internal const string RagebredSkilledDisplayName = "Ragebred.Skilled.Name";
        private static readonly string RagebredSkilledDescription = "Ragebred.Skilled.Description";

        internal const string RagebredChangeShapeDisplayName = "Ragebred.ChangeShape.Name";
        private static readonly string RagebredChangeShapeDescription = "Ragebred.ChangeShape.Description";

        internal static BlueprintFeature Configure()
        {
            var BiteRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{8D16F61E-D521-4153-89C3-C3163016DBAF}");
            var ClawRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{925F0A7D-259B-4781-B1D0-08EF832CCE59}");
            var GoreRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{FA68944E-650C-4CAD-AEBC-EAC07EC0F93B}");

            var changeshapeicon = BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.ShifterWildShapeWereRatFeature.ToString()).Icon;

            // Create shifting EELinks
            var ShiftedSkin = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "1aeb459da29dca341a78317170eec262" },
                RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Very dark red
                    RaceRecolorizer.GetColorsFromRGB(75f),
                    RaceRecolorizer.GetColorsFromRGB(25f),
                    RaceRecolorizer.GetColorsFromRGB(25f)
                    )}
            ), "{D5CA8AFD-1A30-4ADE-92B9-EF5602C251FD}", true, true,
            RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Much Redder
                    RaceRecolorizer.GetColorsFromRGB(120f),
                    RaceRecolorizer.GetColorsFromRGB(50f),
                    RaceRecolorizer.GetColorsFromRGB(35f)
                    )}));

            var ShiftedEyes = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "a47bac4deb099fc4b86a2e01bb425cc5" },
                RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Red
                    RaceRecolorizer.GetColorsFromRGB(255f),
                    RaceRecolorizer.GetColorsFromRGB(0f),
                    RaceRecolorizer.GetColorsFromRGB(0f)
                    )}), "{BD5997AF-D8A4-4C8F-8A63-7778A0E9E457}");

            //Make buff
            var ShiftedBuff = BuffConfigurator.New("SkinwalkerRagebredChangeShapeBuff", "{2558B647-6569-4B52-8145-C9EE5E444D3D}")
                .SetDisplayName(RagebredChangeShapeDisplayName)
                .SetDescription(RagebredChangeShapeDescription)
                .SetIcon(changeshapeicon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Speed, value: 10)
                .AddAdditionalLimb(GoreRef)
                .AddSpellDescriptorComponent(SpellDescriptor.Polymorph)
                .AddComponent<AddEquipmentEntityBySex>(c =>
                {
                    c.FemaleEquipmentEntity = new EquipmentEntityLink() { AssetId = "7b27b2063f548794e845e0ee8ea7b91b" };
                    c.MaleEquipmentEntity = new EquipmentEntityLink() { AssetId = "6ae0e2be0e8f9f54981033b4a61f11ed" };
                })
                .AddEquipmentEntity(ShiftedSkin)
                .AddEquipmentEntity(ShiftedEyes)
                .AddPolymorphBonuses(4)
                .SetFxOnStart("352469f228a3b1f4cb269c7ab0409b8e")
                .SetFxOnRemove("352469f228a3b1f4cb269c7ab0409b8e")
                .Configure();

            // Make Acivatable Abiliy
            var changeshape = ActivatableAbilityConfigurator.New("SkinwalkerRagebredChangeShape", "{61947AFD-F181-4906-9BF8-3ECA040CEF16}")
                .SetDisplayName(RagebredChangeShapeDisplayName)
                .SetDescription(RagebredChangeShapeDescription)
                .SetIcon(changeshapeicon)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfOwnerDisabled(true)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Standard)
                .SetBuff(ShiftedBuff)
                .AddActionsOnBuffApply(ActionsBuilder.New().RemoveBuffsByDescriptor(SpellDescriptor.Polymorph).Build())
                .Configure();

            // Make skill feature
            var skills = FeatureConfigurator.New("SkinwalkerRagebredSkilled", "{6D867572-9924-422C-9C40-CECDF226BCF7}")
                .SetDisplayName(RagebredSkilledDisplayName)
                .SetDescription(RagebredSkilledDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HumanSkilled.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillLoreNature, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillPerception, value: 2)
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            // Make base feature
            var feat = FeatureConfigurator.New("SkinwalkerRagebred", "{97420EF4-D06C-471E-B083-0E646CABA92E}")
                .SetDisplayName(RagebredDisplayName)
                .SetDescription(RagebredDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: -2)
                .AddFacts(new() { skills, changeshape })
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            return feat;
        }
    }
}
