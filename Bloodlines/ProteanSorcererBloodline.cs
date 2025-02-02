using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Mechanics.Components;
using EbonsContentMod.Components;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.NewComponents;
using static Kingmaker.Armies.TacticalCombat.Grid.TacticalCombatGrid;
using Kingmaker.UnitLogic.Buffs.Components;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using Kingmaker.Designers.Mechanics.Buffs;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using EbonsContentMod.Utilities;
using TabletopTweaks.Core.Config;
using EbonsContentMod.utilities;
using HarmonyLib;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic;
using BlueprintCore.Conditions.Builder;
//using BlueprintCore.Conditions.Builder.BasicEx;
using BlueprintCore.Utils.Assets;
using Kingmaker.ElementsSystem;
using Kingmaker.Localization;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.Utility;
using UnityEngine;
using Kingmaker.RuleSystem;
using BlueprintCore.Conditions.Builder.ContextEx;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UI.UnitSettings.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components.TargetCheckers;

namespace EbonsContentMod.Bloodlines
{
    internal class ProteanSorcererBloodline
    {
        private static readonly string ProteanSorcererBloodlineName = "ProteanSorcererBloodline";

        internal const string ProteanSorcererBloodlineDisplayName = "ProteanSorcererBloodline.Name";
        private static readonly string ProteanSorcererBloodlineDescription = "ProteanSorcererBloodline.Description";
        public static readonly string ProteanSorcererBloodlineGuid = "{E3811E14-0C23-4AED-9A66-7C1F83F34221}";

        private static readonly string ProteanCrossbloodedBloodlineName = "ProteanCrossbloodedBloodline";
        private static readonly string ProteanSeekerBloodlineName = "ProteanSeekerBloodline";

        internal const string ProteanSorcererBloodlineClassSkillDisplayName = "ProteanSorcererBloodlineClassSkill.Name";
        private static readonly string ProteanSorcererBloodlineClassSkillDescription = "ProteanSorcererBloodlineClassSkill.Description";

        private static readonly string ProteanSorcererBloodlineSpellsDescription = "ProteanSorcererBloodlineSpells.Description";

        internal const string ProteanSorcererBloodlineFeatsDisplayName = "ProteanSorcererBloodlineFeats.Name";
        private static readonly string ProteanSorcererBloodlineFeatsDescription = "ProteanSorcererBloodlineFeats.Description";

        internal const string ProteanSorcererBloodlineRequisiteFeatureDisplayName = "ProteanSorcererBloodlineRequisiteFeature.Name";
        private static readonly string ProteanSorcererBloodlineRequisiteFeatureDescription = "ProteanSorcererBloodlineRequisiteFeature.Description";

        internal const string ProteanSorcererBloodlineArcanaDisplayName = "ProteanSorcererBloodlineArcana.Name";
        private static readonly string ProteanSorcererBloodlineArcanaDescription = "ProteanSorcererBloodlineArcana.Description";

        internal const string ProteanSorcererBloodlineProtoplasmDisplayName = "ProteanSorcererBloodlineProtoplasm.Name";
        private static readonly string ProteanSorcererBloodlineProtoplasmDescription = "ProteanSorcererBloodlineProtoplasm.Description";
        internal const string ProteanSorcererBloodlineProtoplasmImmobileDisplayName = "ProteanSorcererBloodlineProtoplasmImmobile.Name";
        internal const string ProteanSorcererBloodlineProtoplasmEntangledDisplayName = "ProteanSorcererBloodlineProtoplasmEntagled.Name";

        internal const string ProteanSorcererBloodlineResistancesDisplayName = "ProteanSorcererBloodlineResistances.Name";
        private static readonly string ProteanSorcererBloodlineResistancesDescription = "ProteanSorcererBloodlineResistances.Description";

        internal const string ProteanSorcererBloodlineRealityWrinkleDisplayName = "ProteanSorcererBloodlineRealityWrinkle.Name";
        private static readonly string ProteanSorcererBloodlineRealityWrinkleDescription = "ProteanSorcererBloodlineRealityWrinkle.Description";

        internal const string ProteanSorcererBloodlineBlackTentaclesDisplayName = "ProteanSorcererBloodlineBlackTentacles.Name";
        private static readonly string ProteanSorcererBloodlineBlackTentaclesDescription = "ProteanSorcererBloodlineBlackTentacles.Description";

        internal const string ProteanSorcererBloodlineSpatialTearDisplayName = "ProteanSorcererBloodlineSpatialTear.Name";
        private static readonly string ProteanSorcererBloodlineSpatialTearDescription = "ProteanSorcererBloodlineSpatialTear.Description";
        internal const string ProteanSorcererBloodlineSpatialTearSingleDisplayName = "ProteanSorcererBloodlineSpatialTearSingle.Name";
        internal const string ProteanSorcererBloodlineSpatialTearMassDisplayName = "ProteanSorcererBloodlineSpatialTearMass.Name";

        internal const string ProteanSorcererBloodlineAvatarOfChaosDisplayName = "ProteanSorcererBloodlineAvatarOfChaos.Name";
        private static readonly string ProteanSorcererBloodlineAvatarOfChaosDescription = "ProteanSorcererBloodlineAvatarOfChaos.Description";

        internal static void Configure()
        {
            if (!CheckerUtilities.GetModActive("TabletopTweaks-Base")) return;

            var sorcererclass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>(CharacterClassRefs.SorcererClass.ToString());
            var magusclass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>(CharacterClassRefs.MagusClass.ToString());
            var eldritchscionarchetype = BlueprintTools.GetBlueprintReference<BlueprintArchetypeReference>(ArchetypeRefs.EldritchScionArchetype.ToString());

            // **Class skill**
            var ProteanSorcererBloodlineClassSkill = FeatureConfigurator.New("ProteanSorcererBloodlineClassSkill", "{306B4BE8-83DE-4FFC-AE27-1D470CA4181C}") // done
                .SetDisplayName(ProteanSorcererBloodlineClassSkillDisplayName)
                .SetDescription(ProteanSorcererBloodlineClassSkillDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.BloodlineFeyClassSkill.ToString()).Icon)
                .AddComponent<AddClassSkill>(c => {
                    c.Skill = StatType.SkillLoreNature;
                })
                .SetIsClassFeature(true)
                .SetRanks(1)
                .Configure();

            // **Bonus Spells**
            var spell3 = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(AbilityRefs.RayOfEnfeeblement.ToString());
            var spell5 = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(AbilityRefs.Blur.ToString());
            var spell7 = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(AbilityRefs.Displacement.ToString());
            var spell9 = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(AbilityRefs.ConfusionSpell.ToString());
            var spell11 = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(AbilityRefs.BalefulPolymorph.ToString());
            var spell13 = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(AbilityRefs.Disintegrate.ToString());
            var spell15 = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(AbilityRefs.PolymorphGreaterBase.ToString());
            var spell17 = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(AbilityRefs.FrightfulAspect.ToString());
            var spell19 = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(AbilityRefs.Shapechange.ToString());

            // **Spell Adder Features**
            var ProteanSorcererBloodlineSpells3 = FeatureConfigurator.New("ProteanSorcererBloodlineSpells3", "{C8E83BB8-5128-41FC-91FA-90683912125E}")
                .SetDisplayName(spell3.Get().m_DisplayName)
                .SetDescription(ProteanSorcererBloodlineSpellsDescription)
                .SetIcon(spell3.Get().Icon)
                .SetIsClassFeature(true)
                .AddComponent<AddKnownSpell>(c =>
                {
                    c.m_CharacterClass = sorcererclass;
                    c.m_Spell = spell3;
                    c.SpellLevel = 1;
                })
                .Configure();

            var ProteanSorcererBloodlineSpells5 = FeatureConfigurator.New("ProteanSorcererBloodlineSpells5", "{34AC02F3-98C4-4A12-960C-6900F1A8E278}")
                .SetDisplayName(spell5.Get().m_DisplayName)
                .SetDescription(ProteanSorcererBloodlineSpellsDescription)
                .SetIcon(spell5.Get().Icon)
                .SetIsClassFeature(true)
                .AddComponent<AddKnownSpell>(c =>
                {
                    c.m_CharacterClass = sorcererclass;
                    c.m_Spell = spell5;
                    c.SpellLevel = 2;
                })
                .Configure();

            var ProteanSorcererBloodlineSpells7 = FeatureConfigurator.New("ProteanSorcererBloodlineSpells7", "{EF28E209-A5C4-4B59-ACFA-35C6355EB60D}")
                .SetDisplayName(spell7.Get().m_DisplayName)
                .SetDescription(ProteanSorcererBloodlineSpellsDescription)
                .SetIcon(spell7.Get().Icon)
                .SetIsClassFeature(true)
                .AddComponent<AddKnownSpell>(c =>
                {
                    c.m_CharacterClass = sorcererclass;
                    c.m_Spell = spell7;
                    c.SpellLevel = 3;
                })
                .Configure();

            var ProteanSorcererBloodlineSpells9 = FeatureConfigurator.New("ProteanSorcererBloodlineSpells9", "{628A23C7-0940-477E-8B77-AB724A2F7AAE}")
                .SetDisplayName(spell9.Get().m_DisplayName)
                .SetDescription(ProteanSorcererBloodlineSpellsDescription)
                .SetIcon(spell9.Get().Icon)
                .SetIsClassFeature(true)
                .AddComponent<AddKnownSpell>(c =>
                {
                    c.m_CharacterClass = sorcererclass;
                    c.m_Spell = spell9;
                    c.SpellLevel = 4;
                })
                .Configure();

            var ProteanSorcererBloodlineSpells11 = FeatureConfigurator.New("ProteanSorcererBloodlineSpells11", "{17C10AA7-4745-4056-8FC4-450D4FFE96FB}")
                .SetDisplayName(spell11.Get().m_DisplayName)
                .SetDescription(ProteanSorcererBloodlineSpellsDescription)
                .SetIcon(spell11.Get().Icon)
                .SetIsClassFeature(true)
                .AddComponent<AddKnownSpell>(c =>
                {
                    c.m_CharacterClass = sorcererclass;
                    c.m_Spell = spell11;
                    c.SpellLevel = 5;
                })
                .Configure();

            var ProteanSorcererBloodlineSpells13 = FeatureConfigurator.New("ProteanSorcererBloodlineSpells13", "{C94F084B-CAF0-40E0-ACA0-D03D6C2EE926}")
                .SetDisplayName(spell13.Get().m_DisplayName)
                .SetDescription(ProteanSorcererBloodlineSpellsDescription)
                .SetIcon(spell13.Get().Icon)
                .SetIsClassFeature(true)
                .AddComponent<AddKnownSpell>(c =>
                {
                    c.m_CharacterClass = sorcererclass;
                    c.m_Spell = spell13;
                    c.SpellLevel = 6;
                })
                .Configure();

            var ProteanSorcererBloodlineSpells15 = FeatureConfigurator.New("ProteanSorcererBloodlineSpells15", "{204DB620-4A96-4797-A820-91212B5FA106}")
                .SetDisplayName(spell15.Get().m_DisplayName)
                .SetDescription(ProteanSorcererBloodlineSpellsDescription)
                .SetIcon(spell15.Get().Icon)
                .SetIsClassFeature(true)
                .AddComponent<AddKnownSpell>(c =>
                {
                    c.m_CharacterClass = sorcererclass;
                    c.m_Spell = spell15;
                    c.SpellLevel = 7;
                })
                .Configure();

            var ProteanSorcererBloodlineSpells17 = FeatureConfigurator.New("ProteanSorcererBloodlineSpells17", "{77ACD129-FD52-40D4-830E-6FBC92CE475B}")
                .SetDisplayName(spell17.Get().m_DisplayName)
                .SetDescription(ProteanSorcererBloodlineSpellsDescription)
                .SetIcon(spell17.Get().Icon)
                .SetIsClassFeature(true)
                .AddComponent<AddKnownSpell>(c =>
                {
                    c.m_CharacterClass = sorcererclass;
                    c.m_Spell = spell17;
                    c.SpellLevel = 8;
                })
                .Configure();

            var ProteanSorcererBloodlineSpells19 = FeatureConfigurator.New("ProteanSorcererBloodlineSpells19", "{378C7249-23B9-49C2-9CF5-7F2A3FDF5092}")
                .SetDisplayName(spell19.Get().m_DisplayName)
                .SetDescription(ProteanSorcererBloodlineSpellsDescription)
                .SetIcon(spell19.Get().Icon)
                .SetIsClassFeature(true)
                .AddComponent<AddKnownSpell>(c =>
                {
                    c.m_CharacterClass = sorcererclass;
                    c.m_Spell = spell19;
                    c.SpellLevel = 9;
                })
                .Configure();

            // **Bonus Feats**
            var ProteanSorcererBloodlineFeats = FeatureSelectionConfigurator.New("ProteanSorcererBloodlineFeats", "{48E0F026-A9EA-42DA-A8DC-7FD456412FA4}")
                .SetDisplayName(ProteanSorcererBloodlineFeatsDisplayName)
                .SetDescription(ProteanSorcererBloodlineFeatsDescription)
                .SetRanks(1)
                .SetIsClassFeature(true)
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .AddToAllFeatures(
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.AgileManeuvers.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.DefensiveCombatTraining.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.GreatFortitude.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.GreatFortitudeImproved.ToString()),
                    ParametrizedFeatureRefs.SpellFocus.Reference.Get().ToReference<BlueprintParametrizedFeatureReference>().ToString(),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.Toughness.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.EmpowerSpellFeat.ToString())
                    )
                .Configure();

            // **Requisite Features**
            var ProteanSorcererBloodlineRequisiteFeature = FeatureConfigurator.New("ProteanSorcererBloodlineRequisiteFeature", "{DBEB0B3C-CAA2-4908-8A90-DC0251A1A932}") // done
                .SetDisplayName(ProteanSorcererBloodlineRequisiteFeatureDisplayName)
                .SetDescription(ProteanSorcererBloodlineRequisiteFeatureDescription)
                .SetHideInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .SetIsClassFeature(true)
                .SetRanks(1)
                .Configure();

            var BloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("e2cfd3ce-df7c-4008-8b25-aa82d6db3c77"); // BloodlineRequisiteFeature from TTT

            // **Bloodline Arcana**
            // Run through a postfix patch for RuleDispelMagic.CasterLevelCheckValue
            var ProteanSorcererBloodlineArcana = FeatureConfigurator.New("ProteanSorcererBloodlineArcana", "{9254E145-C713-4E08-A5E3-66D96BC0E2F2}")
                .SetDisplayName(ProteanSorcererBloodlineArcanaDisplayName)
                .SetDescription(ProteanSorcererBloodlineArcanaDescription)
                .SetIcon(AbilityRefs.ElementalBodyIBase.Reference.Get().Icon)
                .Configure();

            // **Bloodline Powers**

            // Protoplasm

            var ProteanSorcererBloodlineProtoplasmResource = AbilityResourceConfigurator.New("ProteanSorcererBloodlineProtoplasmResource", "{D3992C98-94A6-4A90-B9CC-2A6127D8CD9A}")
                .SetMaxAmount(ResourceAmountBuilder.New(3).IncreaseByStat(StatType.Charisma))
                .Configure();

            var EntangleBuff = BuffConfigurator.New("ProteanSorcererBloodlineProtoplasmEntangleBuff", "{E86C0595-B37B-4DA9-9A0C-F5E06080BA61}")
                .CopyFrom(BuffRefs.EntangleBuff)
                .SetDisplayName(ProteanSorcererBloodlineProtoplasmEntangledDisplayName)
                .SetDescription(ProteanSorcererBloodlineProtoplasmDescription)
                .SetIcon(AbilityRefs.TouchOfSlime.Reference.Get().Icon)
                .AddFactContextActions(newRound: ActionsBuilder.New().DealDamage(new DamageTypeDescription() { Type = DamageType.Energy, Energy = Kingmaker.Enums.Damage.DamageEnergyType.Acid }, new ContextDiceValue() { DiceType = DiceType.Zero, BonusValue = ContextValues.Constant(1) }).Build())
                .Configure();

            var ImmobileBuff = BuffConfigurator.New("ProteanSorcererBloodlineProtoplasmImmobileBuff", "{CB385701-E36F-46BD-A47F-CB5BDFBBAB40}")
                .CopyFrom(BuffRefs.EntangleBuff)
                .SetDisplayName(ProteanSorcererBloodlineProtoplasmImmobileDisplayName)
                .SetDescription(ProteanSorcererBloodlineProtoplasmDescription)
                .SetIcon(AbilityRefs.TouchOfSlime.Reference.Get().Icon)
                .AddFactContextActions(newRound: ActionsBuilder.New().DealDamage(new DamageTypeDescription() { Type = DamageType.Energy, Energy = Kingmaker.Enums.Damage.DamageEnergyType.Acid }, new ContextDiceValue() { DiceType = DiceType.Zero, BonusValue = ContextValues.Constant(1) }).Build())
                .AddCondition(UnitCondition.CantMove)
                .Configure();

            var actions = ActionsBuilder.New().DealDamage(new DamageTypeDescription() { Type = DamageType.Energy, Energy = Kingmaker.Enums.Damage.DamageEnergyType.Acid }, ContextDice.Value(DiceType.Zero, bonus: 1)).ConditionalSaved(
                failed: ActionsBuilder.New().ApplyBuff(ImmobileBuff, ContextDuration.FixedDice(DiceType.D3, 1)),
                succeed: ActionsBuilder.New().ApplyBuff(EntangleBuff, ContextDuration.FixedDice(DiceType.D3, 1))).Build();

            var ProteanSorcererBloodlineProtoplasmAbility = AbilityConfigurator.New("ProteanSorcererBloodlineProtoplasmAbility", "{E2E9F0E4-1EDE-4A1D-9D8C-5CAF2E301E88}")
                .CopyFrom(AbilityRefs.AcidSplash, c => c is not AbilityEffectRunAction or SpellListComponent or CantripComponent or ActionPanelLogic or ContextRankConfig)
                .SetDisplayName(ProteanSorcererBloodlineProtoplasmDisplayName)
                .SetDescription(ProteanSorcererBloodlineProtoplasmDescription)
                .SetIcon(AbilityRefs.TouchOfSlime.Reference.Get().Icon)
                //.AddComponent(AbilityRefs.AcidSplash.Reference.Get().GetComponent<AbilityDeliverProjectile>())
                .AddAbilityEffectRunAction(actions, savingThrowType: SavingThrowType.Reflex)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: ProteanSorcererBloodlineProtoplasmResource)
                .SetActionType(UnitCommand.CommandType.Standard)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 1)
                .SetRange(AbilityRange.Close)
                .Configure();

            var ProteanSorcererBloodlineProtoplasm = FeatureConfigurator.New("ProteanSorcererBloodlineProtoplasmFeature", "{DCABCD91-11D1-45EA-8FF6-8756728F0E51}")
                .SetDisplayName(ProteanSorcererBloodlineProtoplasmDisplayName)
                .SetDescription(ProteanSorcererBloodlineProtoplasmDescription)
                .SetIcon(AbilityRefs.TouchOfSlime.Reference.Get().Icon)
                .AddAbilityResources(resource: ProteanSorcererBloodlineProtoplasmResource, restoreAmount: true)
                .AddFacts([ProteanSorcererBloodlineProtoplasmAbility])
                .SetIsClassFeature(true)
                .SetRanks(1)
                .Configure();

            // Protean Resistances

            var ProteanSorcererBloodlineResistances1 = FeatureConfigurator.New("ProteanSorcererBloodlineResistances1", "{7F9FD77C-F8D7-4E13-AD01-ECB2FA4D2063}") // done
                .SetDisplayName(ProteanSorcererBloodlineResistancesDisplayName)
                .SetDescription(ProteanSorcererBloodlineResistancesDescription)
                .SetIcon(FeatureRefs.BloodlineDraconicBlackResistancesAbilityLevel1.Reference.Get().Icon)
                .AddSavingThrowBonusAgainstDescriptor(2, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable, spellDescriptor: SpellDescriptor.Polymorph)
                .AddSavingThrowBonusAgainstDescriptor(2, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable, spellDescriptor: SpellDescriptor.Petrified)
                .AddSavingThrowBonusAgainstSchool(modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable, school: SpellSchool.Transmutation, value: 2)
                .AddDamageResistanceEnergy(type: Kingmaker.Enums.Damage.DamageEnergyType.Acid, value: 5)
                .SetIsClassFeature(true)
                .SetRanks(1)
                .Configure();

            var ProteanSorcererBloodlineResistances2 = FeatureConfigurator.New("ProteanSorcererBloodlineResistances2", "{6882CD7C-ECDC-42DE-8AC0-CDD5075B9224}") // done
                .SetDisplayName(ProteanSorcererBloodlineResistancesDisplayName)
                .SetDescription(ProteanSorcererBloodlineResistancesDescription)
                .SetIcon(FeatureRefs.BloodlineDraconicBlackResistancesAbilityLevel1.Reference.Get().Icon)
                .AddSavingThrowBonusAgainstDescriptor(4, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable, spellDescriptor: SpellDescriptor.Polymorph)
                .AddSavingThrowBonusAgainstDescriptor(4, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable, spellDescriptor: SpellDescriptor.Petrified)
                .AddSavingThrowBonusAgainstSchool(modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable, school: SpellSchool.Transmutation, value: 4)
                .AddDamageResistanceEnergy(type: Kingmaker.Enums.Damage.DamageEnergyType.Acid, value: 10)
                .AddRemoveFeatureOnApply(ProteanSorcererBloodlineResistances1)
                .SetIsClassFeature(true)
                .SetRanks(1)
                .Configure();

            // Reality Wrinkle - AoE Apply concealment buff

            var ProteanSorcererBloodlineRealityWrinkleResource = AbilityResourceConfigurator.New("ProteanSorcererBloodlineRealityWrinkleResource", "{52BA9733-137C-40D8-905C-DF85C78DE1B9}")
                .SetMaxAmount(ResourceAmountBuilder.New(1).IncreaseByLevelStartPlusDivStep([CharacterClassRefs.SorcererClass.ToString()], levelsPerStep: 1, bonusPerStep: 1, startingLevel: 1))
                .Configure();

            var ProteanSorcererBloodlineRealityWrinkleEffectBuff = BuffConfigurator.New("ProteanSorcererBloodlineRealityWrinkleEffectBuff", "{2979D90F-4144-4E8E-AD09-A02DC65710C4}")
                .AddConcealment(concealment: Kingmaker.Enums.Concealment.Partial, descriptor: Kingmaker.Enums.ConcealmentDescriptor.Blur, checkDistance: false, checkWeaponRangeType: false, onlyForAttacks: false)
                .AddFormationACBonus(1)
                .SetFxOnStart("ea8ddc3e798aa25458e2c8a15e484c68")
                .Configure();

            var ProteanSorcererBloodlineRealityWrinkleArea = AbilityAreaEffectConfigurator.New("ProteanSorcererBloodlineRealityWrinkleArea", "{FEF6A3C9-709E-4059-8298-68DDE18D5A74}") // Add Fx
                .AddAbilityAreaEffectBuff(ProteanSorcererBloodlineRealityWrinkleEffectBuff, false, ConditionsBuilder.New().IsAlly().IsCaster(true).Build())
                .SetAffectEnemies(false)
                .SetAggroEnemies(true)
                .SetSize(10.Feet())
                .SetShape(AreaEffectShape.Cylinder)
                .Configure();

            var ProteanSorcererBloodlineRealityWrinkleBuff = BuffConfigurator.New("ProteanSorcererBloodlineRealityWrinkleBuff", "{7768CF24-7315-4055-AA76-F29625EC013C}") // Add Fx on start
                .SetDisplayName(ProteanSorcererBloodlineRealityWrinkleDisplayName)
                .SetDescription(ProteanSorcererBloodlineRealityWrinkleDescription)
                .SetIcon(FeatureRefs.StygianSlayerShadowyMistForm.Reference.Get().Icon) // Consider changing
                .AddConcealment(concealment: Kingmaker.Enums.Concealment.Partial, descriptor: Kingmaker.Enums.ConcealmentDescriptor.Blur, checkDistance: false, checkWeaponRangeType: false, onlyForAttacks: false)
                .AddFormationACBonus(1)
                .AddAreaEffect(ProteanSorcererBloodlineRealityWrinkleArea)
                .SetFxOnStart("ea8ddc3e798aa25458e2c8a15e484c68")
                .Configure();

            var ProteanSorcererBloodlineRealityWrinkleAbility = ActivatableAbilityConfigurator.New("ProteanSorcererBloodlineRealityWrinkleAbility", "{9D3753A6-00BE-487C-91BB-AC4C6329A565}")
                .SetDisplayName(ProteanSorcererBloodlineRealityWrinkleDisplayName)
                .SetDescription(ProteanSorcererBloodlineRealityWrinkleDescription)
                .SetIcon(FeatureRefs.StygianSlayerShadowyMistForm.Reference.Get().Icon) // Consider changing
                .AddActivatableAbilityResourceLogic(requiredResource: ProteanSorcererBloodlineRealityWrinkleResource, spendType: Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilityResourceLogic.ResourceSpendType.NewRound)
                .AddActionPanelLogic(priority: 3)
                .SetBuff(ProteanSorcererBloodlineRealityWrinkleBuff)
                .Configure();

            var ProteanSorcererBloodlineRealityWrinkle = FeatureConfigurator.New("ProteanSorcererBloodlineRealityWrinkleFeature", "{EDBDFD50-9135-41DD-B893-885EEDC89D60}")
                .SetDisplayName(ProteanSorcererBloodlineRealityWrinkleDisplayName)
                .SetDescription(ProteanSorcererBloodlineRealityWrinkleDescription)
                .SetIcon(FeatureRefs.StygianSlayerShadowyMistForm.Reference.Get().Icon) // Consider changing
                .AddAbilityResources(resource: ProteanSorcererBloodlineRealityWrinkleResource, restoreAmount: true)
                .AddFacts([ProteanSorcererBloodlineRealityWrinkleAbility])
                .SetIsClassFeature(true)
                .SetRanks(1)
                .Configure();

            // Spatial Tear - Dimension Door and Black Tentacles

            // Make black tentacles
            string sourceFx = "aa448b28b377b1c49b136d88fa346600";
            string newFx = "{C0513064-1925-414E-85D8-107C32566F12}";

            var ProteanSorcererBloodlineSpatialTearResource = AbilityResourceConfigurator.New("ProteanSorcererBloodlineSpatialTearResource", "{B832F588-B63F-4BEF-8F4E-F81936B14828}")
                .SetMaxAmount(ResourceAmountBuilder.New(1).IncreaseByLevelStartPlusDivStep([CharacterClassRefs.SorcererClass.ToString()], levelsPerStep: 20, bonusPerStep: 1))
                .Configure();

            AssetTool.RegisterDynamicPrefabLink(newFx, sourceFx, BlackTentaclesFx);

            var BlackTentaclesBuff = BuffConfigurator.New("ProteanBloodlineBlackTentaclesBuff", "{87EF5C46-1AC0-4F7C-8349-CF8A428D70DE}")
                .CopyFrom(BuffRefs.OmoxGrappleBuff)
                .SetDisplayName(ProteanSorcererBloodlineBlackTentaclesDisplayName)
                .SetDescription(ProteanSorcererBloodlineBlackTentaclesDescription)
                .AddContextRankConfig(ContextRankConfigs.CasterLevel())
                .AddCondition(UnitCondition.Entangled)
                .AddCondition(UnitCondition.CantMove)
                .AddActionsOnBuffApply(actions: ActionsBuilder.New()
                                                    .DealDamage(damageType: DamageTypes.Untyped(), value: ContextDice.Value(DiceType.D6, diceCount: ContextValues.Constant(1), bonus: 4))
                                                    .Build())
                .AddNewRoundTrigger(newRoundActions: ActionsBuilder.New()
                                                    .DealDamage(damageType: DamageTypes.Untyped(), value: ContextDice.Value(DiceType.D6, diceCount: ContextValues.Constant(1), bonus: 4))
                                                    .BreakFree(success: ActionsBuilder.New().RemoveSelf(), useCMB: true)
                                                    .Build())
                .SetFlags(new BlueprintBuff.Flags[] { BlueprintBuff.Flags.HiddenInUi })
                .Configure();

            ActionList grappleAction = ActionsBuilder.New()
                .ApplyBuffPermanent(BlackTentaclesBuff)
                .Build();

            grappleAction.Actions = new GameAction[] {
                new BlackTentaclesGrappleAction {
                    Bonus = 5,
                    Success = ActionsBuilder.New()
                                    .Conditional(ConditionsBuilder.New()
                                                .HasBuff(BlackTentaclesBuff),
                                                ifFalse: ActionsBuilder.New().ApplyBuffPermanent(buff: BlackTentaclesBuff).Build()).Build(),
                    Failure = ActionsBuilder.New().RemoveBuff(buff: BlackTentaclesBuff).Build(),
                    Value = ContextValues.Rank()
                    }
            };

            BlueprintAbilityAreaEffect areaEffect = AbilityAreaEffectConfigurator.New("ProteanBloodlineBlackTentaclesArea", "{E7FBCA90-F956-4A0A-BE8E-53D245A91227}")
              .SetAffectEnemies()
              .SetAggroEnemies()
              .SetSize(20.Feet())
              .AddContextRankConfig(ContextRankConfigs.CasterLevel())
              .AddAbilityAreaEffectRunAction(
                                unitEnter: grappleAction,
                                round: grappleAction,
                                unitExit: ActionsBuilder.New()
                                            .RemoveBuff(buff: BlackTentaclesBuff)
                                            .RemoveBuff(BuffRefs.EntangleBuffDifficultTerrain.Cast<BlueprintBuffReference>().Reference)
                                            .Build())
              .SetShape(AreaEffectShape.Cylinder)
              .SetFx(newFx)
              .Configure();

            var BlackTentaclesAbility = AbilityConfigurator.New("ProteanBloodlineBlackTentaclesAbility", "{C143D56F-8D09-4C33-8960-272F8DEC2251}")
                .CopyFrom(AbilityRefs.SickeningEntanglement, c => c is not (AbilityEffectRunAction or AbilityAoERadius))
                .SetDisplayName(ProteanSorcererBloodlineBlackTentaclesDisplayName)
                .SetDescription(ProteanSorcererBloodlineBlackTentaclesDescription)
                .AddSpellComponent(SpellSchool.Conjuration)
                .SetIcon(AbilityRefs.SickeningEntanglement.Reference.Get().Icon)
                .AddAbilityEffectRunAction(actions: ActionsBuilder.New()
                                                    .SpawnAreaEffect(areaEffect: areaEffect,
                                                                     durationValue: ContextDuration.Variable(ContextValues.Rank())))
                .AddAbilityAoERadius(radius: 20.Feet())
                .SetType(AbilityType.SpellLike)
                .SetRange(AbilityRange.Personal)
                .AddPretendSpellLevel(spellLevel: 4)
                .Configure();

            /*var LeaveBlackTentaclesSingle = ActionFlow.DoSingle<ContextActionCastTargetedAndPersonalSpells>(aa =>
            {
                aa.m_Spell = AbilityRefs.DimensionDoorCasterOnly.Reference.Get().ToReference<BlueprintAbilityReference>();
                aa.m_SecondSpell = BlackTentaclesAbility.ToReference<BlueprintAbilityReference>();
                aa.MarkAsChild = true;
                aa.OverrideSpellLevel = true;
                aa.SpellLevel = 4;
                //aa.m_SpendAction = true;
                //aa.m_StandardAction = true;
            });
            var LeaveBlackTentaclesMass = ActionFlow.DoSingle<ContextActionCastTargetedAndPersonalSpells>(aa =>
            {
                aa.m_Spell = AbilityRefs.DimensionDoor.Reference.Get().ToReference<BlueprintAbilityReference>();
                aa.m_SecondSpell = BlackTentaclesAbility.ToReference<BlueprintAbilityReference>();
                aa.MarkAsChild = true;
                aa.OverrideSpellLevel = true;
                aa.SpellLevel = 4;
                //aa.m_SpendAction = true;
                //aa.m_StandardAction = true;
            });
            var LeaveBlackTentaclesSingle2 = ActionsBuilder.New().CastSpell(BlackTentaclesAbility, spendAction: false).CastSpell(AbilityRefs.DimensionDoorCasterOnly.ToString());
            var LeaveBlackTentaclesMass2 = ActionsBuilder.New().CastSpell(BlackTentaclesAbility, spendAction: false).CastSpell(AbilityRefs.DimensionDoor.ToString());*/

            var SpatialTearAbilitySingle = AbilityConfigurator.New("ProteanSorcererBloodlineSpatialTearAbilitySingle", "{3F873838-3E0D-42F2-9C43-5EF809E2E898}")
                .CopyFrom(AbilityRefs.DimensionDoorCasterOnly, typeof(SpellComponent), typeof(LineOfSightIgnorance))
                .RemoveComponents(c => c is AbilityCustomDimensionDoor)
                .SetDisplayName(ProteanSorcererBloodlineSpatialTearSingleDisplayName)
                .SetDescription(AbilityRefs.DimensionDoorCasterOnly.Reference.Get().m_Description)
                .SetIcon(AbilityRefs.DimensionDoorCasterOnly.Reference.Get().Icon)
                .AddComponent<AbilityCustomDimensionDoorLeaveBehindAOE>(c =>
                {
                    c.Radius.m_Value = 0.0f;
                    c.PortalFromPrefab = new Kingmaker.ResourceLinks.PrefabLink() { AssetId = "382a57043c80f914681c0b0f04fe63ec" };
                    c.PortalToPrefab = new Kingmaker.ResourceLinks.PrefabLink() { AssetId = "dde3273425ddf5f4b902116fdf27edb9" };
                    c.PortalBone = "Offset";
                    c.CasterDisappearFx = new Kingmaker.ResourceLinks.PrefabLink() { AssetId = "9535b8cda5cb3164eb9e53a621e53a60" };
                    c.CasterAppearFx = new Kingmaker.ResourceLinks.PrefabLink() { AssetId = "7e864434489d2ac4da4d4eb22793e5d3" };
                    c.SideDisappearFx = new Kingmaker.ResourceLinks.PrefabLink() { AssetId = "9535b8cda5cb3164eb9e53a621e53a60" };
                    c.SideAppearFx = new Kingmaker.ResourceLinks.PrefabLink() { AssetId = "7e864434489d2ac4da4d4eb22793e5d3" };
                    c.m_AreaEffect = areaEffect.ToReference<BlueprintAbilityAreaEffectReference>();
                    c.m_CasterDisappearProjectile = ProjectileRefs.DimensionDoor00_CasterDisappear.Reference.Get().ToReference<BlueprintProjectileReference>();
                    c.m_CasterAppearProjectile = ProjectileRefs.DimensionDoor00_CasterAppear.Reference.Get().ToReference<BlueprintProjectileReference>();
                    c.m_SideDisappearProjectile = ProjectileRefs.DimensionDoor00_SideDisappear.Reference.Get().ToReference<BlueprintProjectileReference>();
                    c.m_SideAppearProjectile = ProjectileRefs.DimensionDoor00_SideAppear.Reference.Get().ToReference<BlueprintProjectileReference>();
                })
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: ProteanSorcererBloodlineSpatialTearResource)
                //.AddAbilityEffectRunAction(LeaveBlackTentaclesSingle2)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 4)
                .Configure();

            var SpatialTearAbilityMass = AbilityConfigurator.New("ProteanSorcererBloodlineSpatialTearAbilityMass", "{3F1D7BB2-9BB7-4C45-AA6A-B781FFBBB1AD}")
                .CopyFrom(AbilityRefs.DimensionDoor, typeof(SpellComponent), typeof(LineOfSightIgnorance))
                .RemoveComponents(c => c is AbilityCustomDimensionDoor)
                .SetDisplayName(ProteanSorcererBloodlineSpatialTearMassDisplayName)
                .SetDescription(AbilityRefs.DimensionDoor.Reference.Get().m_Description)
                .SetIcon(AbilityRefs.DimensionDoor.Reference.Get().Icon)
                .AddComponent<AbilityCustomDimensionDoorLeaveBehindAOE>(c =>
                {
                    c.Radius.m_Value = 10.0f;
                    c.PortalFromPrefab = new Kingmaker.ResourceLinks.PrefabLink() { AssetId = "382a57043c80f914681c0b0f04fe63ec" };
                    c.PortalToPrefab = new Kingmaker.ResourceLinks.PrefabLink() { AssetId = "dde3273425ddf5f4b902116fdf27edb9" };
                    c.PortalBone = "Offset";
                    c.CasterDisappearFx = new Kingmaker.ResourceLinks.PrefabLink() { AssetId = "9535b8cda5cb3164eb9e53a621e53a60" };
                    c.CasterAppearFx = new Kingmaker.ResourceLinks.PrefabLink() { AssetId = "7e864434489d2ac4da4d4eb22793e5d3" };
                    c.SideDisappearFx = new Kingmaker.ResourceLinks.PrefabLink() { AssetId = "9535b8cda5cb3164eb9e53a621e53a60" };
                    c.SideAppearFx = new Kingmaker.ResourceLinks.PrefabLink() { AssetId = "7e864434489d2ac4da4d4eb22793e5d3" };
                    c.m_AreaEffect = areaEffect.ToReference<BlueprintAbilityAreaEffectReference>();
                    c.m_CasterDisappearProjectile = ProjectileRefs.DimensionDoor00_CasterDisappear.Reference.Get().ToReference<BlueprintProjectileReference>();
                    c.m_CasterAppearProjectile = ProjectileRefs.DimensionDoor00_CasterAppear.Reference.Get().ToReference<BlueprintProjectileReference>();
                    c.m_SideDisappearProjectile = ProjectileRefs.DimensionDoor00_SideDisappear.Reference.Get().ToReference<BlueprintProjectileReference>();
                    c.m_SideAppearProjectile = ProjectileRefs.DimensionDoor00_SideAppear.Reference.Get().ToReference<BlueprintProjectileReference>();
                    c.m_CameraShouldFollow = false;
                    c.UseAnimations = false;
                    c.TakeOffAnimation = null;
                    c.TakeoffTime = 1.0f;
                    c.DissapearTime = 1.0f;
                    c.LandingAnimation = null;
                    c.LandingTime = 1.0f;
                    c.AppearTime = 1.0f;
                })
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: ProteanSorcererBloodlineSpatialTearResource)
                //.AddAbilityEffectRunAction(LeaveBlackTentaclesMass2)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 4)
                .Configure();

            var SpatialTearAbility = AbilityConfigurator.New("ProteanSorcererBloodlineSpatialTearAbility", "{CC7E57F5-394C-4B77-9592-D5C438708D0A}")
                .CopyFrom(AbilityRefs.DimensionDoorBase)
                .SetDisplayName(ProteanSorcererBloodlineSpatialTearDisplayName)
                .SetDescription(ProteanSorcererBloodlineSpatialTearDescription)
                .SetIcon(AbilityRefs.DimensionDoorBase.Reference.Get().Icon)
                .AddAbilityVariants([SpatialTearAbilitySingle.ToReference<BlueprintAbilityReference>(), SpatialTearAbilityMass.ToReference<BlueprintAbilityReference>()])
                .AddLineOfSightIgnorance()
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 4)
                .Configure();
            
            var SpatialTear = FeatureConfigurator.New("ProteanSorcererBloodlineSpatialTearFeature", "{1AB24BFC-0B76-4F1B-B682-143825DB3C8E}")
                .SetDisplayName(ProteanSorcererBloodlineSpatialTearDisplayName)
                .SetDescription(ProteanSorcererBloodlineSpatialTearDescription)
                .SetIcon(AbilityRefs.DimensionDoor.Reference.Get().Icon)
                .AddAbilityResources(resource: ProteanSorcererBloodlineSpatialTearResource, restoreAmount: true)
                .SetIsClassFeature(true)
                .SetRanks(1)
                .AddFacts([SpatialTearAbility])
                .Configure();

            // Avatar of Chaos

            var AvatarOfChaos = FeatureConfigurator.New("ProteanSorcererBloodlineAvatarOfChaos", "{ED20FBB1-B2B4-40DC-8E68-6B7B5F56D294}")
                .SetDisplayName(ProteanSorcererBloodlineAvatarOfChaosDisplayName)
                .SetDescription(ProteanSorcererBloodlineAvatarOfChaosDescription)
                .SetIcon(AbilityRefs.EmbodimentOfOrder.Reference.Get().Icon) // Uncertainty Principle, Embodiment of Order, Protection of Nature, Scintilating Pattern
                .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Acid, ignoreFeature: "{ED20FBB1-B2B4-40DC-8E68-6B7B5F56D294}")
                .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Petrified, ignoreFeature: "{ED20FBB1-B2B4-40DC-8E68-6B7B5F56D294}")
                .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Polymorph, ignoreFeature: "{ED20FBB1-B2B4-40DC-8E68-6B7B5F56D294}")
                .AddComponent<SpellPenetrationBonusAgainstAlignment>(c =>
                {
                    c.Alignment = Kingmaker.Enums.AlignmentComponent.Lawful;
                    c.Value = 2;
                })
                .AddIncreaseSpellDescriptorDC(bonusDC: 2, spellsOnly: true, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable, descriptor: SpellDescriptor.Chaos)
                .SetIsClassFeature(true)
                .SetRanks(1)
                .Configure();

            // **Bloodline Progressions**

            var SorcererProteanBloodline = ProgressionConfigurator.New(ProteanSorcererBloodlineName, ProteanSorcererBloodlineGuid)
                .SetDisplayName(ProteanSorcererBloodlineDisplayName)
                .SetDescription(ProteanSorcererBloodlineDescription)
                .SetIcon(AbilityRefs.Shapechange.Reference.Get().Icon) // Change
                .AddToClasses([
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = sorcererclass
                    },
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = magusclass
                    } ])
                .AddToArchetypes([
                    new BlueprintProgression.ArchetypeWithLevel {
                        m_Archetype = eldritchscionarchetype
                    }
                ])
                .SetGiveFeaturesForPreviousLevels(true)
                .SetGroups(FeatureGroup.BloodLine)
                .SetRanks(1)
                .SetIsClassFeature(true)
                .AddToLevelEntries(1, ProteanSorcererBloodlineProtoplasm, ProteanSorcererBloodlineArcana, ProteanSorcererBloodlineClassSkill, ProteanSorcererBloodlineRequisiteFeature, BloodlineRequisiteFeature)
                .AddToLevelEntries(3, ProteanSorcererBloodlineSpells3, ProteanSorcererBloodlineResistances1)
                .AddToLevelEntries(5, ProteanSorcererBloodlineSpells5)
                .AddToLevelEntries(7, ProteanSorcererBloodlineSpells7)
                .AddToLevelEntries(9, ProteanSorcererBloodlineSpells9, ProteanSorcererBloodlineResistances2, ProteanSorcererBloodlineRealityWrinkle)
                .AddToLevelEntries(11, ProteanSorcererBloodlineSpells11)
                .AddToLevelEntries(13, ProteanSorcererBloodlineSpells13)
                .AddToLevelEntries(15, ProteanSorcererBloodlineSpells15, SpatialTear)
                .AddToLevelEntries(17, ProteanSorcererBloodlineSpells17)
                .AddToLevelEntries(19, ProteanSorcererBloodlineSpells19)
                .AddToLevelEntries(20, AvatarOfChaos)
                .AddPrerequisiteNoFeature(BloodlineRequisiteFeature)
                .AddPrerequisiteNoFeature(ProteanSorcererBloodlineRequisiteFeature)
                .Configure();

            var CrossbloodedProteanBloodline = ProgressionConfigurator.New(ProteanCrossbloodedBloodlineName, "{BE45B911-258B-4F56-9480-E535755E868C}")
                .SetDisplayName(ProteanSorcererBloodlineDisplayName)
                .SetDescription(ProteanSorcererBloodlineDescription)
                .SetIcon(AbilityRefs.Shapechange.Reference.Get().Icon) // Change
                .AddToClasses([
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = sorcererclass
                    },
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = magusclass
                    } ])
                .AddToArchetypes([
                    new BlueprintProgression.ArchetypeWithLevel {
                        m_Archetype = eldritchscionarchetype
                    }
                ])
                .SetGiveFeaturesForPreviousLevels(true)
                .SetGroups(FeatureGroup.BloodragerBloodline)
                .SetRanks(1)
                .SetIsClassFeature(true)
                .AddToLevelEntries(1, ProteanSorcererBloodlineArcana, ProteanSorcererBloodlineClassSkill, ProteanSorcererBloodlineRequisiteFeature, BloodlineRequisiteFeature)
                .AddToLevelEntries(3, ProteanSorcererBloodlineSpells3)
                .AddToLevelEntries(5, ProteanSorcererBloodlineSpells5)
                .AddToLevelEntries(7, ProteanSorcererBloodlineSpells7)
                .AddToLevelEntries(9, ProteanSorcererBloodlineSpells9)
                .AddToLevelEntries(11, ProteanSorcererBloodlineSpells11)
                .AddToLevelEntries(13, ProteanSorcererBloodlineSpells13)
                .AddToLevelEntries(15, ProteanSorcererBloodlineSpells15)
                .AddToLevelEntries(17, ProteanSorcererBloodlineSpells17)
                .AddToLevelEntries(19, ProteanSorcererBloodlineSpells19)
                .Configure();

            var SeekerProteanBloodline = ProgressionConfigurator.New(ProteanSeekerBloodlineName, "{E3C4EF71-8CA9-4ABA-8307-A8B2E8AC5700}")
                .SetDisplayName(ProteanSorcererBloodlineDisplayName)
                .SetDescription(ProteanSorcererBloodlineDescription)
                .SetIcon(AbilityRefs.Shapechange.Reference.Get().Icon) // Change
                .AddToClasses([
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = sorcererclass
                    },
                    new BlueprintProgression.ClassWithLevel {
                        m_Class = magusclass
                    } ])
                .AddToArchetypes([
                    new BlueprintProgression.ArchetypeWithLevel {
                        m_Archetype = eldritchscionarchetype
                    }
                ])
                .SetGiveFeaturesForPreviousLevels(true)
                .SetGroups(FeatureGroup.BloodLine)
                .SetRanks(1)
                .SetIsClassFeature(true)
                .AddToLevelEntries(1, ProteanSorcererBloodlineProtoplasm, ProteanSorcererBloodlineArcana, ProteanSorcererBloodlineClassSkill, ProteanSorcererBloodlineRequisiteFeature, BloodlineRequisiteFeature)
                .AddToLevelEntries(3, ProteanSorcererBloodlineSpells3)
                .AddToLevelEntries(5, ProteanSorcererBloodlineSpells5)
                .AddToLevelEntries(7, ProteanSorcererBloodlineSpells7)
                .AddToLevelEntries(9, ProteanSorcererBloodlineSpells9, ProteanSorcererBloodlineRealityWrinkle)
                .AddToLevelEntries(11, ProteanSorcererBloodlineSpells11)
                .AddToLevelEntries(13, ProteanSorcererBloodlineSpells13)
                .AddToLevelEntries(15, ProteanSorcererBloodlineSpells15)
                .AddToLevelEntries(17, ProteanSorcererBloodlineSpells17)
                .AddToLevelEntries(19, ProteanSorcererBloodlineSpells19)
                .AddToLevelEntries(20, AvatarOfChaos)
                .AddPrerequisiteNoFeature(BloodlineRequisiteFeature)
                .AddPrerequisiteNoFeature(ProteanSorcererBloodlineRequisiteFeature)
                .Configure();

            try
            {
                BloodlineTools.RegisterSorcererFeatSelection(ProteanSorcererBloodlineFeats, SorcererProteanBloodline);
                BloodlineTools.RegisterSorcererBloodline(SorcererProteanBloodline);
                BloodlineTools.RegisterCrossbloodedBloodline(CrossbloodedProteanBloodline);
                BloodlineTools.RegisterSeekerBloodline(SeekerProteanBloodline);
            }
            finally
            {
                // finished
            }
        }

        private static void BlackTentaclesFx(GameObject gameObject)
        {
            UnityEngine.Object.DestroyImmediate(gameObject.transform.Find("AnimationRoot/WaveAll_StinkingSmoke00").gameObject);
            UnityEngine.Object.DestroyImmediate(gameObject.transform.Find("AnimationRoot/WaveAll_StinkingSmoke00_RotatableCopy").gameObject);
            UnityEngine.Object.DestroyImmediate(gameObject.transform.Find("AnimationRoot/WaveAll_GrassLiana00_RotatableCopy").gameObject);
            UnityEngine.Object.DestroyImmediate(gameObject.transform.Find("AnimationRoot/WaveAll_GrassLianaSingle00_RotatableCopy").gameObject);
            UnityEngine.Object.DestroyImmediate(gameObject.transform.Find("AnimationRoot/WaveAll_GrassLiana00").gameObject);
            UnityEngine.Object.DestroyImmediate(gameObject.transform.Find("AnimationRoot/WaveAll_GrassLianaSingle00").gameObject);
            UnityEngine.Object.DestroyImmediate(gameObject.transform.Find("AnimationRoot/WaveAll_FireFliesGreen").gameObject);
            UnityEngine.Object.DestroyImmediate(gameObject.transform.Find("AnimationRoot/WaveAll_FireFliesViolet").gameObject);
            gameObject.transform.localScale = new(0.25f, 1.0f, 0.25f);
        }

        [HarmonyPatch(typeof(RuleDispelMagic))]
        internal class Player_DispelCasterLevelCheckValue_Patch
        {
            [HarmonyPatch(nameof(RuleDispelMagic.CasterLevelCheckValue))]
            [HarmonyPostfix]
            public static void Postfix(RuleDispelMagic __instance, ref int __result)
            {
                var Fact = BlueprintTools.GetBlueprint<BlueprintFeature>("{9254E145-C713-4E08-A5E3-66D96BC0E2F2}");
                if (Fact == null)
                {
                    Main.log.Log("DispelCasterLevelCheckValue: Checked fact is null");
                    return;
                }
                var caster = __instance.Buff.Context.MaybeCaster;
                var spellSchool = __instance.Buff.Context.SpellSchool;       

                if (caster != null && spellSchool != SpellSchool.None && caster.HasFact(Fact) && (spellSchool == SpellSchool.Transmutation || spellSchool == SpellSchool.Conjuration))
                    __result -= 4;
            }
        }
    }
}
