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
    internal class Bloodmarked
    {
        internal const string BloodmarkedDisplayName = "Bloodmarked.Name";
        private static readonly string BloodmarkedDescription = "Bloodmarked.Description";

        internal const string BloodmarkedSkilledDisplayName = "Bloodmarked.Skilled.Name";
        private static readonly string BloodmarkedSkilledDescription = "Bloodmarked.Skilled.Description";

        internal const string BloodmarkedChangeShapeDisplayName = "Bloodmarked.ChangeShape.Name";
        private static readonly string BloodmarkedChangeShapeDescription = "Bloodmarked.ChangeShape.Description";

        internal static BlueprintFeature Configure()
        {
            var BiteRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{8D16F61E-D521-4153-89C3-C3163016DBAF}");
            var ClawRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{925F0A7D-259B-4781-B1D0-08EF832CCE59}");
            var GoreRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{FA68944E-650C-4CAD-AEBC-EAC07EC0F93B}");

            var changeshapeicon = BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.ShifterWildShapeWereRatFeature.ToString()).Icon;

            // Create shifting EELinks
            var ShiftedSkin = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "1aeb459da29dca341a78317170eec262" },
                RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Really dark purple
                    RaceRecolorizer.GetColorsFromRGB(40f),
                    RaceRecolorizer.GetColorsFromRGB(10f),
                    RaceRecolorizer.GetColorsFromRGB(70f)
                    )}
            ), "{A67E2B84-2F79-4F37-B952-C423F3E66EE3}", true, true,
            RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Dark Purple
                    RaceRecolorizer.GetColorsFromRGB(70f),
                    RaceRecolorizer.GetColorsFromRGB(45f),
                    RaceRecolorizer.GetColorsFromRGB(90f)
                    )}));

            var ShiftedEyes = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "a47bac4deb099fc4b86a2e01bb425cc5" },
                RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Cold Blue
                    RaceRecolorizer.GetColorsFromRGB(165f),
                    RaceRecolorizer.GetColorsFromRGB(40f),
                    RaceRecolorizer.GetColorsFromRGB(235f)
                    )}), "{CD164F5B-4A3F-47B5-BEA7-BC620D2B5238}");

            //Make buff
            var ShiftedBuff = BuffConfigurator.New("SkinwalkerBloodmarkedChangeShapeBuff", "{D885C2C7-E479-40A8-B387-6D3861B63EE4}")
                .SetDisplayName(BloodmarkedChangeShapeDisplayName)
                .SetDescription(BloodmarkedChangeShapeDescription)
                .SetIcon(changeshapeicon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: 2) // Shifted ability score bonus
                .AddAdditionalLimb(BiteRef) // Bestial feature 1
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Speed, value: 10) // Bestial feature 2
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
            var changeshape = ActivatableAbilityConfigurator.New("SkinwalkerBloodmarkedChangeShape", "{48A564AA-794E-4620-A7ED-76ECC51DEE73}")
                .SetDisplayName(BloodmarkedChangeShapeDisplayName)
                .SetDescription(BloodmarkedChangeShapeDescription)
                .SetIcon(changeshapeicon)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfOwnerDisabled(true)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Standard)
                .SetBuff(ShiftedBuff)
                .AddActionsOnBuffApply(ActionsBuilder.New().RemoveBuffsByDescriptor(SpellDescriptor.Polymorph).Build())
                .Configure();

            // Make skill feature
            var skills = FeatureConfigurator.New("SkinwalkerBloodmarkedSkilled", "{3D072CA1-E0B1-402A-89F5-F45B7913D55A}")
                .SetDisplayName(BloodmarkedSkilledDisplayName)
                .SetDescription(BloodmarkedSkilledDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HumanSkilled.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillAthletics, value: 2) // Skill bonus 1
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillPerception, value: 2) // Skill bonus 2
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            // Make base feature
            var feat = FeatureConfigurator.New("SkinwalkerBloodmarked", "{3BD18948-FE33-4C05-AE3C-DDB13CE0A3F1}")
                .SetDisplayName(BloodmarkedDisplayName)
                .SetDescription(BloodmarkedDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2) // Ability score bonus
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: -2) // Ability score penalty
                .AddFacts(new() { skills, changeshape })
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            return feat;
        }
    }
}
