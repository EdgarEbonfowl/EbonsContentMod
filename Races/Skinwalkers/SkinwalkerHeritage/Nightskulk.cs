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
    internal class Nightskulk
    {
        internal const string NightskulkDisplayName = "Nightskulk.Name";
        private static readonly string NightskulkDescription = "Nightskulk.Description";

        internal const string NightskulkSkilledDisplayName = "Nightskulk.Skilled.Name";
        private static readonly string NightskulkSkilledDescription = "Nightskulk.Skilled.Description";

        internal const string NightskulkChangeShapeDisplayName = "Nightskulk.ChangeShape.Name";
        private static readonly string NightskulkChangeShapeDescription = "Nightskulk.ChangeShape.Description";

        internal static BlueprintFeature Configure()
        {
            var BiteRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{8D16F61E-D521-4153-89C3-C3163016DBAF}");
            var ClawRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{925F0A7D-259B-4781-B1D0-08EF832CCE59}");
            var GoreRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{FA68944E-650C-4CAD-AEBC-EAC07EC0F93B}");

            var changeshapeicon = BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.ShifterWildShapeWereRatFeature.ToString()).Icon;

            // Create shifting EELinks
            var ShiftedSkin = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "1aeb459da29dca341a78317170eec262" },
                RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Dark Gray
                    RaceRecolorizer.GetColorsFromRGB(50f),
                    RaceRecolorizer.GetColorsFromRGB(50f),
                    RaceRecolorizer.GetColorsFromRGB(55f)
                    )}
            ), "{4EF9CB3D-429C-454C-AE24-E81E1CD9691E}", true, true,
            RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Gray
                    RaceRecolorizer.GetColorsFromRGB(105f),
                    RaceRecolorizer.GetColorsFromRGB(105f),
                    RaceRecolorizer.GetColorsFromRGB(110f)
                    )}));

            var ShiftedEyes = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "a47bac4deb099fc4b86a2e01bb425cc5" },
                RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Green with yellow tint
                    RaceRecolorizer.GetColorsFromRGB(110f),
                    RaceRecolorizer.GetColorsFromRGB(190f),
                    RaceRecolorizer.GetColorsFromRGB(0f)
                    )}), "{A9D3DBE8-8A9D-4CBC-B4B7-E028C926824F}");

            //Make buff
            var ShiftedBuff = BuffConfigurator.New("SkinwalkerNightskulkChangeShapeBuff", "{DC68605A-4A3F-4B28-AB85-627D089C6733}")
                .SetDisplayName(NightskulkChangeShapeDisplayName)
                .SetDescription(NightskulkChangeShapeDescription)
                .SetIcon(changeshapeicon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2) // Shifted ability score bonus
                .AddAdditionalLimb(BiteRef) // Bestial feature 1
                // Need to make distraction component
                //.AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Speed, value: 10) // Bestial feature 2
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
            var changeshape = ActivatableAbilityConfigurator.New("SkinwalkerNightskulkChangeShape", "{188714B7-8C1F-43FD-9A0D-6E59B8D74C9C}")
                .SetDisplayName(NightskulkChangeShapeDisplayName)
                .SetDescription(NightskulkChangeShapeDescription)
                .SetIcon(changeshapeicon)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfOwnerDisabled(true)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Standard)
                .SetBuff(ShiftedBuff)
                .AddActionsOnBuffApply(ActionsBuilder.New().RemoveBuffsByDescriptor(SpellDescriptor.Polymorph).Build())
                .Configure();

            // Make skill feature
            var skills = FeatureConfigurator.New("SkinwalkerNightskulkSkilled", "{8DF8936C-BE5E-4A16-B451-D4B3FEFF5E93}")
                .SetDisplayName(NightskulkSkilledDisplayName)
                .SetDescription(NightskulkSkilledDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HumanSkilled.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillLoreNature, value: 2) // Skill bonus 1
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillStealth, value: 2) // Skill bonus 2
                .Configure();

            // Make base feature
            var feat = FeatureConfigurator.New("SkinwalkerNightskulk", "{C5586574-8A17-4AB3-92C1-EC82FB721214}")
                .SetDisplayName(NightskulkDisplayName)
                .SetDescription(NightskulkDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: 2) // Ability score bonus
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: -2) // Ability score penalty
                .AddFacts(new() { skills, changeshape })
                .Configure();

            return feat;
        }
    }
}
