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
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using EbonsContentMod.Utilities;

namespace EbonsContentMod.Races.Skinwalkers.SkinwalkerHeritage
{
    internal class Scaleheart
    {
        internal const string ScaleheartDisplayName = "Scaleheart.Name";
        private static readonly string ScaleheartDescription = "Scaleheart.Description";

        internal const string ScaleheartSkilledDisplayName = "Scaleheart.Skilled.Name";
        private static readonly string ScaleheartSkilledDescription = "Scaleheart.Skilled.Description";

        internal const string ScaleheartChangeShapeDisplayName = "Scaleheart.ChangeShape.Name";
        private static readonly string ScaleheartChangeShapeDescription = "Scaleheart.ChangeShape.Description";

        internal static BlueprintFeature Configure()
        {
            var BiteRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{8D16F61E-D521-4153-89C3-C3163016DBAF}");
            var ClawRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{925F0A7D-259B-4781-B1D0-08EF832CCE59}");
            var GoreRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{FA68944E-650C-4CAD-AEBC-EAC07EC0F93B}");

            var changeshapeicon = BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.ShifterWildShapeWereRatFeature.ToString()).Icon;

            // Create shifting EELinks
            var ShiftedEyes = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "a47bac4deb099fc4b86a2e01bb425cc5" },
                RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Cold Blue
                    RaceRecolorizer.GetColorsFromRGB(210f),
                    RaceRecolorizer.GetColorsFromRGB(225f),
                    RaceRecolorizer.GetColorsFromRGB(50f)
                    )}), "{695E744B-AEB7-45F9-AB57-ED006809AEFE}");

            //Make buff
            var ShiftedBuff = BuffConfigurator.New("SkinwalkerScaleheartChangeShapeBuff", "{2919FFE6-377C-4E4D-8A02-467FAD3BAA0B}")
                .SetDisplayName(ScaleheartChangeShapeDisplayName)
                .SetDescription(ScaleheartChangeShapeDescription)
                .SetIcon(changeshapeicon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: 2) // Shifted ability score bonus
                .AddAdditionalLimb(BiteRef) // Bestial feature 1
                .AddMechanicsFeature(Kingmaker.UnitLogic.FactLogic.AddMechanicsFeature.MechanicsFeatureType.Ferocity) // Bestial feature 2
                .AddSpellDescriptorComponent(SpellDescriptor.Polymorph)
                .AddComponent<AddEquipmentEntityBySex>(c =>
                {
                    c.FemaleEquipmentEntity = new EquipmentEntityLink() { AssetId = "7b27b2063f548794e845e0ee8ea7b91b" };
                    c.MaleEquipmentEntity = new EquipmentEntityLink() { AssetId = "6ae0e2be0e8f9f54981033b4a61f11ed" };
                })
                .AddEquipmentEntity("86127616283ae7741ae3e813904865cc")
                .AddEquipmentEntity("7109791d63944254589b908564604c79")
                .AddEquipmentEntity(ShiftedEyes)
                .AddPolymorphBonuses(4)
                .SetFxOnStart("352469f228a3b1f4cb269c7ab0409b8e")
                .SetFxOnRemove("352469f228a3b1f4cb269c7ab0409b8e")
                .Configure();

            // Make Acivatable Abiliy
            var changeshape = ActivatableAbilityConfigurator.New("SkinwalkerScaleheartChangeShape", "{71C6584D-DE7A-4110-8EEC-F433BFE7EBE5}")
                .SetDisplayName(ScaleheartChangeShapeDisplayName)
                .SetDescription(ScaleheartChangeShapeDescription)
                .SetIcon(changeshapeicon)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfOwnerDisabled(true)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Standard)
                .SetBuff(ShiftedBuff)
                .AddActionsOnBuffApply(ActionsBuilder.New().RemoveBuffsByDescriptor(SpellDescriptor.Polymorph).Build())
                .Configure();

            // Make skill feature
            var skills = FeatureConfigurator.New("SkinwalkerScaleheartSkilled", "{17DCF532-F204-4112-A977-3C5830ED7750}")
                .SetDisplayName(ScaleheartSkilledDisplayName)
                .SetDescription(ScaleheartSkilledDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HumanSkilled.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillLoreNature, value: 2) // Skill bonus 1
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillStealth, value: 2) // Skill bonus 2
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            // Make base feature
            var feat = FeatureConfigurator.New("SkinwalkerScaleheart", "{A17B7E42-B809-4157-845E-6DAF6FEF89A3}")
                .SetDisplayName(ScaleheartDisplayName)
                .SetDescription(ScaleheartDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: 2) // Ability score bonus
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: -2) // Ability score penalty
                .AddFacts(new() { skills, changeshape })
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            return feat;
        }
    }
}
