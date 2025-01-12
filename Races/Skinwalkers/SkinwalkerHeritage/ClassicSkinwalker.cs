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
using static EbonsContentMod.Utilities.ActivatableAbilityGroupUtilities;
using Kingmaker.UnitLogic.ActivatableAbilities;

namespace EbonsContentMod.Races.Skinwalkers.SkinwalkerHeritage
{
    internal class ClassicSkinwalker
    {
        internal const string ClassicSkinwalkerDisplayName = "ClassicSkinwalker.Name";
        private static readonly string ClassicSkinwalkerDescription = "ClassicSkinwalker.Description";

        internal const string ClassicSkinwalkerSkilledDisplayName = "ClassicSkinwalker.Skilled.Name";
        private static readonly string ClassicSkinwalkerSkilledDescription = "ClassicSkinwalker.Skilled.Description";

        internal const string ClassicSkinwalkerChangeShapeStrDisplayName = "ClassicSkinwalker.ChangeShapeStr.Name";
        private static readonly string ClassicSkinwalkerChangeShapeStrDescription = "ClassicSkinwalker.ChangeShapeStr.Description";

        internal const string ClassicSkinwalkerChangeShapeConDisplayName = "ClassicSkinwalker.ChangeShapeCon.Name";
        private static readonly string ClassicSkinwalkerChangeShapeConDescription = "ClassicSkinwalker.ChangeShapeCon.Description";

        internal const string ClassicSkinwalkerChangeShapeDexDisplayName = "ClassicSkinwalker.ChangeShapeDex.Name";
        private static readonly string ClassicSkinwalkerChangeShapeDexDescription = "ClassicSkinwalker.ChangeShapeDex.Description";

        internal static BlueprintFeature Configure()
        {
            var BiteRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{8D16F61E-D521-4153-89C3-C3163016DBAF}");
            var ClawRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{925F0A7D-259B-4781-B1D0-08EF832CCE59}");
            var GoreRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{FA68944E-650C-4CAD-AEBC-EAC07EC0F93B}");

            var changeshapeicon = BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.ShifterWildShapeWereRatFeature.ToString()).Icon;

            //Make buff
            var ShiftedBuffstr = BuffConfigurator.New("SkinwalkerClassicSkinwalkerChangeShapeBuffStr", "{D37217B6-A504-456B-8CBB-9103E2DBE7FC}")
                .SetDisplayName(ClassicSkinwalkerChangeShapeStrDisplayName)
                .SetDescription(ClassicSkinwalkerChangeShapeStrDescription)
                .SetIcon(changeshapeicon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: 2) // Shifted ability score bonus
                .AddEmptyHandWeaponOverride(false, false, weapon: ClawRef).AddEmptyHandWeaponOverride(false, false, weapon: ClawRef) // Bestial feature 1
                .AddStatBonus(ModifierDescriptor.NaturalArmor, stat: StatType.AC, value: 1) // Bestial feature 2
                .AddSpellDescriptorComponent(SpellDescriptor.Polymorph)
                .AddComponent<AddEquipmentEntityBySex>(c =>
                {
                    c.FemaleEquipmentEntity = new EquipmentEntityLink() { AssetId = "7b27b2063f548794e845e0ee8ea7b91b" };
                    c.MaleEquipmentEntity = new EquipmentEntityLink() { AssetId = "6ae0e2be0e8f9f54981033b4a61f11ed" };
                })
                .AddEquipmentEntity("1aeb459da29dca341a78317170eec262")
                .AddPolymorphBonuses(4)
                .SetFxOnStart("352469f228a3b1f4cb269c7ab0409b8e")
                .SetFxOnRemove("352469f228a3b1f4cb269c7ab0409b8e")
                .Configure();

            var ShiftedBuffcon = BuffConfigurator.New("SkinwalkerClassicSkinwalkerChangeShapeBuffCon", "{12E6B266-1E30-48BE-B5F6-A235F9F5159C}")
                .SetDisplayName(ClassicSkinwalkerChangeShapeConDisplayName)
                .SetDescription(ClassicSkinwalkerChangeShapeConDescription)
                .SetIcon(changeshapeicon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: 2) // Shifted ability score bonus
                .AddAdditionalLimb(ClawRef).AddAdditionalLimb(ClawRef) // Bestial feature 1
                .AddStatBonus(ModifierDescriptor.NaturalArmor, stat: StatType.AC, value: 1) // Bestial feature 2
                .AddSpellDescriptorComponent(SpellDescriptor.Polymorph)
                .AddComponent<AddEquipmentEntityBySex>(c =>
                {
                    c.FemaleEquipmentEntity = new EquipmentEntityLink() { AssetId = "7b27b2063f548794e845e0ee8ea7b91b" };
                    c.MaleEquipmentEntity = new EquipmentEntityLink() { AssetId = "6ae0e2be0e8f9f54981033b4a61f11ed" };
                })
                .AddEquipmentEntity("1aeb459da29dca341a78317170eec262")
                .AddPolymorphBonuses(4)
                .SetFxOnStart("352469f228a3b1f4cb269c7ab0409b8e")
                .SetFxOnRemove("352469f228a3b1f4cb269c7ab0409b8e")
                .Configure();

            var ShiftedBuffdex = BuffConfigurator.New("SkinwalkerClassicSkinwalkerChangeShapeBuffDex", "{280B493C-F155-483F-B1CC-F5FAADDF2EC7}")
                .SetDisplayName(ClassicSkinwalkerChangeShapeDexDisplayName)
                .SetDescription(ClassicSkinwalkerChangeShapeDexDescription)
                .SetIcon(changeshapeicon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2) // Shifted ability score bonus
                .AddAdditionalLimb(ClawRef).AddAdditionalLimb(ClawRef) // Bestial feature 1
                .AddStatBonus(ModifierDescriptor.NaturalArmor, stat: StatType.AC, value: 1) // Bestial feature 2
                .AddSpellDescriptorComponent(SpellDescriptor.Polymorph)
                .AddComponent<AddEquipmentEntityBySex>(c =>
                {
                    c.FemaleEquipmentEntity = new EquipmentEntityLink() { AssetId = "7b27b2063f548794e845e0ee8ea7b91b" };
                    c.MaleEquipmentEntity = new EquipmentEntityLink() { AssetId = "6ae0e2be0e8f9f54981033b4a61f11ed" };
                })
                .AddEquipmentEntity("1aeb459da29dca341a78317170eec262")
                .AddPolymorphBonuses(4)
                .SetFxOnStart("352469f228a3b1f4cb269c7ab0409b8e")
                .SetFxOnRemove("352469f228a3b1f4cb269c7ab0409b8e")
                .Configure();

            // Make Acivatable Abiliy
            var changeshapestr = ActivatableAbilityConfigurator.New("SkinwalkerClassicSkinwalkerChangeShapeStr", "{77DBC6FA-6815-4450-94FC-3A01A8AC17AB}")
                .SetDisplayName(ClassicSkinwalkerChangeShapeStrDisplayName)
                .SetDescription(ClassicSkinwalkerChangeShapeStrDescription)
                .SetIcon(changeshapeicon)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfOwnerDisabled(true)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Standard)
                .SetBuff(ShiftedBuffstr)
                .AddActionsOnBuffApply(ActionsBuilder.New().RemoveBuffsByDescriptor(SpellDescriptor.Polymorph).Build())
                .SetGroup((ActivatableAbilityGroup)ECActivatableAbilityGroup.SkinwalkerChangeShapeAbilities)
                .Configure();

            var changeshapecon = ActivatableAbilityConfigurator.New("SkinwalkerClassicSkinwalkerChangeShapeCon", "{9B5C2FD6-9F28-4078-BED8-E7C6BF156615}")
                .SetDisplayName(ClassicSkinwalkerChangeShapeConDisplayName)
                .SetDescription(ClassicSkinwalkerChangeShapeConDescription)
                .SetIcon(changeshapeicon)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfOwnerDisabled(true)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Standard)
                .SetBuff(ShiftedBuffcon)
                .AddActionsOnBuffApply(ActionsBuilder.New().RemoveBuffsByDescriptor(SpellDescriptor.Polymorph).Build())
                .SetGroup((ActivatableAbilityGroup)ECActivatableAbilityGroup.SkinwalkerChangeShapeAbilities)
                .Configure();

            var changeshapedex = ActivatableAbilityConfigurator.New("SkinwalkerClassicSkinwalkerChangeShapeDex", "{F617567B-F16D-4D3F-AE46-BDCE464A378F}")
                .SetDisplayName(ClassicSkinwalkerChangeShapeDexDisplayName)
                .SetDescription(ClassicSkinwalkerChangeShapeDexDescription)
                .SetIcon(changeshapeicon)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfOwnerDisabled(true)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Standard)
                .SetBuff(ShiftedBuffdex)
                .AddActionsOnBuffApply(ActionsBuilder.New().RemoveBuffsByDescriptor(SpellDescriptor.Polymorph).Build())
                .SetGroup((ActivatableAbilityGroup)ECActivatableAbilityGroup.SkinwalkerChangeShapeAbilities)
                .Configure();

            // Make skill feature
            var skills = FeatureConfigurator.New("SkinwalkerClassicSkinwalkerSkilled", "{A27D9334-AA32-4E0A-A8FC-062564D5B702}")
                .SetDisplayName(ClassicSkinwalkerSkilledDisplayName)
                .SetDescription(ClassicSkinwalkerSkilledDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HumanSkilled.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillLoreNature, value: 2) // Skill bonus
                .Configure();

            // Make base feature
            var feat = FeatureConfigurator.New("SkinwalkerClassicSkinwalker", "{AD92E945-5512-4B87-B462-DDCB2F3BBFA2}")
                .SetDisplayName(ClassicSkinwalkerDisplayName)
                .SetDescription(ClassicSkinwalkerDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: 2) // Ability score bonus
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: -2) // Ability score penalty
                .AddFacts(new() { skills, changeshapestr, changeshapecon, changeshapedex })
                .Configure();

            return feat;
        }
    }
}
