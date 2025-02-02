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
using Kingmaker.Visual.CharacterSystem;

namespace EbonsContentMod.Races.Skinwalkers.SkinwalkerHeritage
{
    internal class Coldborn
    {
        internal const string ColdbornDisplayName = "Coldborn.Name";
        private static readonly string ColdbornDescription = "Coldborn.Description";

        internal const string ColdbornSkilledDisplayName = "Coldborn.Skilled.Name";
        private static readonly string ColdbornSkilledDescription = "Coldborn.Skilled.Description";

        internal const string ColdbornChangeShapeDisplayName = "Coldborn.ChangeShape.Name";
        private static readonly string ColdbornChangeShapeDescription = "Coldborn.ChangeShape.Description";

        internal static BlueprintFeature Configure()
        {
            var BiteRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{8D16F61E-D521-4153-89C3-C3163016DBAF}");
            var ClawRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{925F0A7D-259B-4781-B1D0-08EF832CCE59}");
            var GoreRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{FA68944E-650C-4CAD-AEBC-EAC07EC0F93B}");

            var changeshapeicon = BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.ShifterWildShapeWereRatFeature.ToString()).Icon;

            // Create shifting EELinks
            var ShiftedSkin = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "1aeb459da29dca341a78317170eec262" },
                RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Medium brown
                    RaceRecolorizer.GetColorsFromRGB(70f),
                    RaceRecolorizer.GetColorsFromRGB(40f),
                    RaceRecolorizer.GetColorsFromRGB(0f)
                    )}
            ), "{D01BC535-0D66-437C-918D-C5E0331DE176}", true, true,
            RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Brown
                    RaceRecolorizer.GetColorsFromRGB(100f),
                    RaceRecolorizer.GetColorsFromRGB(60f),
                    RaceRecolorizer.GetColorsFromRGB(0f)
                    )}));

            var ShiftedEyes = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "a47bac4deb099fc4b86a2e01bb425cc5" },
                RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Cold Blue
                    RaceRecolorizer.GetColorsFromRGB(0f),
                    RaceRecolorizer.GetColorsFromRGB(140f),
                    RaceRecolorizer.GetColorsFromRGB(255f)
                    )}), "{F44608D1-57E2-49D9-9DB4-E9D951617F0F}");

            var FemaleClawsEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "0fe6ca359f6292540a5430327647dc01" }, RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Medium brown
                    RaceRecolorizer.GetColorsFromRGB(70f),
                    RaceRecolorizer.GetColorsFromRGB(40f),
                    RaceRecolorizer.GetColorsFromRGB(0f)
                    )}
            ), "{9F31B749-02C1-4789-953E-BC56316B668D}", true, true,
            RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Brown
                    RaceRecolorizer.GetColorsFromRGB(100f),
                    RaceRecolorizer.GetColorsFromRGB(60f),
                    RaceRecolorizer.GetColorsFromRGB(0f)
                    )}),
                BodyPartsToRemove: [BodyPartType.Torso, BodyPartType.UpperArms, BodyPartType.Forearms, BodyPartType.UpperLegs, BodyPartType.LowerLegs, BodyPartType.Spaulders, BodyPartType.Skirt, BodyPartType.Cuffs, BodyPartType.Belt, BodyPartType.BeltBag, BodyPartType.Feet], SetLayer: 209,
                HandCopyEE: ShiftedSkin);

            var MaleClawsEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "3c638ac1505198b4fa587f04ca655718" }, RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Medium brown
                    RaceRecolorizer.GetColorsFromRGB(70f),
                    RaceRecolorizer.GetColorsFromRGB(40f),
                    RaceRecolorizer.GetColorsFromRGB(0f)
                    )}
            ), "{0BD0A7AB-E00C-48D2-9913-1D7E4ABEAACA}", true, true,
            RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Brown
                    RaceRecolorizer.GetColorsFromRGB(100f),
                    RaceRecolorizer.GetColorsFromRGB(60f),
                    RaceRecolorizer.GetColorsFromRGB(0f)
                    )}),
                BodyPartsToRemove: [BodyPartType.Torso, BodyPartType.UpperArms, BodyPartType.Forearms, BodyPartType.UpperLegs, BodyPartType.LowerLegs, BodyPartType.Spaulders, BodyPartType.Skirt, BodyPartType.Cuffs, BodyPartType.Belt, BodyPartType.BeltBag, BodyPartType.Feet], SetLayer: 209,
                HandCopyEE: ShiftedSkin);

            //Make buff
            var ShiftedBuff = BuffConfigurator.New("SkinwalkerColdbornChangeShapeBuff", "{04116FBD-5A62-4A1A-8E2D-66C1DE20028F}")
                .SetDisplayName(ColdbornChangeShapeDisplayName)
                .SetDescription(ColdbornChangeShapeDescription)
                .SetIcon(changeshapeicon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: 2) // Shifted ability score bonus
                .AddAdditionalLimb(BiteRef) // Bestial feature 1
                .AddEmptyHandWeaponOverride(false, false, weapon: ClawRef).AddEmptyHandWeaponOverride(false, false, weapon: ClawRef) // Bestial feature 2
                .AddSpellDescriptorComponent(SpellDescriptor.Polymorph)
                .AddComponent<AddEquipmentEntityBySex>(c =>
                {
                    c.FemaleEquipmentEntity = new EquipmentEntityLink() { AssetId = "7b27b2063f548794e845e0ee8ea7b91b" };
                    c.MaleEquipmentEntity = new EquipmentEntityLink() { AssetId = "6ae0e2be0e8f9f54981033b4a61f11ed" };
                })
                .AddComponent<AddEquipmentEntityBySex>(c =>
                {
                    c.FemaleEquipmentEntity = FemaleClawsEE;
                    c.MaleEquipmentEntity = MaleClawsEE;
                })
                .AddEquipmentEntity(ShiftedSkin)
                .AddEquipmentEntity(ShiftedEyes)
                .AddPolymorphBonuses(4)
                .SetFxOnStart("352469f228a3b1f4cb269c7ab0409b8e")
                .SetFxOnRemove("352469f228a3b1f4cb269c7ab0409b8e")
                .Configure();

            // Make Acivatable Abiliy
            var changeshape = ActivatableAbilityConfigurator.New("SkinwalkerColdbornChangeShape", "{1517AEF9-8748-4335-A870-78BEBE2D218D}")
                .SetDisplayName(ColdbornChangeShapeDisplayName)
                .SetDescription(ColdbornChangeShapeDescription)
                .SetIcon(changeshapeicon)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfOwnerDisabled(true)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Standard)
                .SetBuff(ShiftedBuff)
                .AddActionsOnBuffApply(ActionsBuilder.New().RemoveBuffsByDescriptor(SpellDescriptor.Polymorph).Build())
                .Configure();

            // Make skill feature
            var skills = FeatureConfigurator.New("SkinwalkerColdbornSkilled", "{37A2DE2D-A19F-4CCC-9DA9-09AF0D9FECF6}")
                .SetDisplayName(ColdbornSkilledDisplayName)
                .SetDescription(ColdbornSkilledDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HumanSkilled.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillAthletics, value: 2) // Skill bonus 1
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillLoreNature, value: 2) // Skill bonus 2
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            // Make base feature
            var feat = FeatureConfigurator.New("SkinwalkerColdborn", "{D02C6545-B0B3-4BE0-91D3-052810387230}")
                .SetDisplayName(ColdbornDisplayName)
                .SetDescription(ColdbornDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: 2) // Ability score bonus
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: -2) // Ability score penalty
                .AddFacts(new() { skills, changeshape })
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            return feat;
        }
    }
}
