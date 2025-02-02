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
    internal class Witchwolf
    {
        internal const string WitchwolfDisplayName = "Witchwolf.Name";
        private static readonly string WitchwolfDescription = "Witchwolf.Description";

        internal const string WitchwolfSkilledDisplayName = "Witchwolf.Skilled.Name";
        private static readonly string WitchwolfSkilledDescription = "Witchwolf.Skilled.Description";

        internal const string WitchwolfChangeShapeDisplayName = "Witchwolf.ChangeShape.Name";
        private static readonly string WitchwolfChangeShapeDescription = "Witchwolf.ChangeShape.Description";

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
                    RaceRecolorizer.GetColorsFromRGB(60f),
                    RaceRecolorizer.GetColorsFromRGB(60f),
                    RaceRecolorizer.GetColorsFromRGB(65f)
                    )}
            ), "{6A6A3C82-5D5E-4787-8571-0C9C13C9D477}", true, true,
            RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Light Blue Gray
                    RaceRecolorizer.GetColorsFromRGB(155f),
                    RaceRecolorizer.GetColorsFromRGB(160f),
                    RaceRecolorizer.GetColorsFromRGB(180f)
                    )}));

            var ShiftedEyes = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "a47bac4deb099fc4b86a2e01bb425cc5" },
                RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>()
                {new Color( // Light Blue
                    RaceRecolorizer.GetColorsFromRGB(135f),
                    RaceRecolorizer.GetColorsFromRGB(215f),
                    RaceRecolorizer.GetColorsFromRGB(255f)
                    )}), "{FDB858AF-58C3-4903-B991-A62C63A3EB50}");

            //Make buff
            var ShiftedBuff = BuffConfigurator.New("SkinwalkerWitchwolfChangeShapeBuff", "{5E4C4682-0A68-438F-B133-C0833BC64111}")
                .SetDisplayName(WitchwolfChangeShapeDisplayName)
                .SetDescription(WitchwolfChangeShapeDescription)
                .SetIcon(changeshapeicon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: 2) // Shifted ability score bonus
                .AddAdditionalLimb(BiteRef) // Bestial feature 1
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SaveFortitude, value: 2) // Bestial feature 2
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SaveReflex, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SaveWill, value: 2)
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
            var changeshape = ActivatableAbilityConfigurator.New("SkinwalkerWitchwolfChangeShape", "{8144A9CB-2C44-414B-A7E1-AF0087D0601A}")
                .SetDisplayName(WitchwolfChangeShapeDisplayName)
                .SetDescription(WitchwolfChangeShapeDescription)
                .SetIcon(changeshapeicon)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfOwnerDisabled(true)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Standard)
                .SetBuff(ShiftedBuff)
                .AddActionsOnBuffApply(ActionsBuilder.New().RemoveBuffsByDescriptor(SpellDescriptor.Polymorph).Build())
                .Configure();

            // Make skill feature
            var skills = FeatureConfigurator.New("SkinwalkerWitchwolfSkilled", "{AE3BBF49-9740-4178-AC67-4D3FFA46A13C}")
                .SetDisplayName(WitchwolfSkilledDisplayName)
                .SetDescription(WitchwolfSkilledDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.HumanSkilled.ToString()).Icon)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillPerception, value: 2) // Skill bonus 1
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.SkillLoreNature, value: 2) // Skill bonus 2
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            // Make base feature
            var feat = FeatureConfigurator.New("SkinwalkerWitchwolf", "{C5F5F09E-5143-42FA-93AB-F63897512945}")
                .SetDisplayName(WitchwolfDisplayName)
                .SetDescription(WitchwolfDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: 2) // Ability score bonus
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: -2) // Ability score penalty
                .AddFacts(new() { skills, changeshape })
                .SetGroups(FeatureGroup.Racial)
                .Configure();

            return feat;
        }
    }
}
