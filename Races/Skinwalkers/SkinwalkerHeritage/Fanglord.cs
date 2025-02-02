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
    internal class Fanglord
    {
        internal const string FanglordDisplayName = "Fanglord.Name";
        private static readonly string FanglordDescription = "Fanglord.Description";

        internal const string FanglordSkilledDisplayName = "Fanglord.Skilled.Name";
        private static readonly string FanglordSkilledDescription = "Fanglord.Skilled.Description";

        internal const string FanglordChangeShapeDisplayName = "Fanglord.ChangeShape.Name";
        private static readonly string FanglordChangeShapeDescription = "Fanglord.ChangeShape.Description";

        internal static BlueprintFeature Configure()
        {
            var BiteRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{8D16F61E-D521-4153-89C3-C3163016DBAF}");
            var ClawRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{925F0A7D-259B-4781-B1D0-08EF832CCE59}");
            var GoreRef = BlueprintTools.GetBlueprintReference<BlueprintItemWeaponReference>("{FA68944E-650C-4CAD-AEBC-EAC07EC0F93B}");

            var changeshapeicon = BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.ShifterWildShapeWereRatFeature.ToString()).Icon;

            // Create shifting EELinks
            var ShiftedSkin = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "1aeb459da29dca341a78317170eec262" },
                RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // White with orange tint
                    RaceRecolorizer.GetColorsFromRGB(100f),
                    RaceRecolorizer.GetColorsFromRGB(50f),
                    RaceRecolorizer.GetColorsFromRGB(0f)
                    )}
            ), "{7410D095-1D3E-432E-952E-79E1AA5BC851}", true, true,
            RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Orange
                    RaceRecolorizer.GetColorsFromRGB(170f),
                    RaceRecolorizer.GetColorsFromRGB(80f),
                    RaceRecolorizer.GetColorsFromRGB(0f)
                    )}));

            var ShiftedEyes = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "a47bac4deb099fc4b86a2e01bb425cc5" },
                RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // White
                    RaceRecolorizer.GetColorsFromRGB(160f),
                    RaceRecolorizer.GetColorsFromRGB(160f),
                    RaceRecolorizer.GetColorsFromRGB(160f)
                    )}), "{B963D2D7-1681-4838-97C9-F3D08E016E28}");

            //Make buff
            var ShiftedBuff = BuffConfigurator.New("SkinwalkerFanglordChangeShapeBuff", "{D17C5A3B-12FA-4DF3-8F57-E91F529D5624}")
                .SetDisplayName(FanglordChangeShapeDisplayName)
                .SetDescription(FanglordChangeShapeDescription)
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
            var changeshape = ActivatableAbilityConfigurator.New("SkinwalkerFanglordChangeShape", "{74C0D044-6BE2-4B79-9CB5-757C69212C9E}")
                .SetDisplayName(FanglordChangeShapeDisplayName)
                .SetDescription(FanglordChangeShapeDescription)
                .SetIcon(changeshapeicon)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfOwnerDisabled(true)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Standard)
                .SetBuff(ShiftedBuff)
                .AddActionsOnBuffApply(ActionsBuilder.New().RemoveBuffsByDescriptor(SpellDescriptor.Polymorph).Build())
                .Configure();

            // Make skill feature
            var skills = FeatureConfigurator.New("SkinwalkerFanglordSkilled", "{3075BED6-3F34-4FFF-8EB9-02FC6896E96F}")
                .SetDisplayName(FanglordSkilledDisplayName)
                .SetDescription(FanglordSkilledDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HumanSkilled.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillAthletics, value: 2) // Skill bonus 1
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillPerception, value: 2) // Skill bonus 2
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            // Make base feature
            var feat = FeatureConfigurator.New("SkinwalkerFanglord", "{590872B8-5298-446F-AEFF-9E3F52F887B3}")
                .SetDisplayName(FanglordDisplayName)
                .SetDescription(FanglordDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2) // Ability score bonus
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: -2) // Ability score penalty
                .AddFacts(new() { skills, changeshape })
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            return feat;
        }
    }
}
