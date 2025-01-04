using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using EbonsContentMod.Components;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.ActivatableAbilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Utils.Types;
using Kingmaker.Enums;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.Configurators.Classes.Selection;
using BlueprintCore.Blueprints.Configurators.Root;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Conditions.Builder.ContextEx;
using HarmonyLib;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Visual.Animation.Kingmaker.Actions;
using UnityEngine;
using static EbonsContentMod.Utilities.ActivatableAbilityGroupUtilities;
using EbonsContentMod.Utilities;
using Kingmaker.Designers.EventConditionActionSystem.Actions;
using MicroscopicContentExpansion.Utils;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using System.Threading;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;

namespace EbonsContentMod.Abilities
{
    internal class ArcaneDeed
    {
        private const string ArcaneDeedName = "ArcaneDeed";
        internal const string ArcaneDeedDisplayName = "ArcaneDeed.Name";
        private const string ArcaneDeedDescription = "ArcaneDeed.Description";
        public static readonly string ArcaneDeedGuid = "{A2EA114C-D4C6-42B2-B2B4-FF946DEC0081}";

        // Dodging Panache
        private const string DodgingPanacheName = "ArcaneDeedDodgingPanache";
        internal const string DodgingPanacheDisplayName = "ArcaneDeedDodgingPanache.Name";
        private const string DodgingPanacheDescription = "ArcaneDeedDodgingPanache.Description";
        public static readonly string DodgingPanacheGuid = "{5918F432-7CEE-47E5-A202-40DDF3F9E2F1}";

        private const string DodgingPanacheAbilityName = "DodgingPanacheAbility";
        public static readonly string DodgingPanacheAbilityGuid = "{44A3D3E9-6F05-49B2-BF30-7CC67158F494}";

        private const string DodgingPanacheBuffName = "DodgingPanacheBuff";
        public static readonly string DodgingPanacheBuffGuid = "{1BF1F6C2-E9B5-4352-9910-C3BCA0154082}";

        // Bleeding Wound
        internal const string BWoundAbility = "ArcaneDeedBWoundAbility";
        internal const string BWoundAbilityGuid = "{E141795F-1E45-422A-83E8-3CE5252C5D2D}";
        internal const string BWound = "ArcaneDeedBWound";
        internal const string BWoundGuid = "{B4147303-E604-4A95-A6FB-A094ED64BF80}";
        internal const string BWoundDisplayName = "ArcaneDeedBWound.Name";
        internal const string BWoundDescription = "ArcaneDeedBWound.Description";

        internal const string BleedDebuff = "ArcaneDeedBleedDebuff";
        internal const string BleedDebuffGuid = "{CECC85D5-8A9B-4EEC-8020-D3D54E2CEAF8}";
        internal const string BleedBuff = "ArcaneDeedBleedBuff";
        internal const string BleedBuffGuid = "{F776CF5C-953D-41EC-8271-23018E27B66A}";
        internal const string BleedAbility = "ArcaneDeedBleedAbility";
        internal const string BleedAbilityGuid = "{E3B8A8DE-360F-45C6-AFF7-534410320459}";
        internal const string BleedDebuffDisplayName = "ArcaneDeedBleed.Name";
        internal const string BleedDebuffDescription = "ArcaneDeedBleedDebuff.Description";
        internal const string BleedAbilityDescription = "ArcaneDeedBleedAbility.Description";

        internal const string SBleedDebuff = "ArcaneDeedSBleedDebuff";
        internal const string SBleedDebuffGuid = "{2A4B933C-C601-48D7-A547-FB5532CB9D06}";
        internal const string SBleedBuff = "ArcaneDeedSBleedBuff";
        internal const string SBleedBuffGuid = "{F637C824-0EF2-46D3-8EC0-B19FF3CF4732}";
        internal const string SBleedAbility = "ArcaneDeedSBleedAbility";
        internal const string SBleedAbilityGuid = "{BEA44A2A-7BF1-4A50-AB79-27ED5424003D}";
        internal const string SBleedDebuffDisplayName = "ArcaneDeedSBleed.Name";
        internal const string SBleedDebuffDescription = "ArcaneDeedSBleedDebuff.Description";
        internal const string SBleedAbilityDescription = "ArcaneDeedSBleedAbility.Description";

        internal const string DBleedDebuff = "ArcaneDeedDBleedDebuff";
        internal const string DBleedDebuffGuid = "{8348338C-B82B-4411-B739-0402687AF908}";
        internal const string DBleedBuff = "ArcaneDeedDBleedBuff";
        internal const string DBleedBuffGuid = "{CD14DFE4-6850-4554-B353-6A77AAAE7BBA}";
        internal const string DBleedAbility = "ArcaneDeedDBleedAbility";
        internal const string DBleedAbilityGuid = "{C7D6A1E6-DE12-44CF-BEB3-004D1F3D93C0}";
        internal const string DBleedDebuffDisplayName = "ArcaneDeedDBleed.Name";
        internal const string DBleedDebuffDescription = "ArcaneDeedDBleedDebuff.Description";
        internal const string DBleedAbilityDescription = "ArcaneDeedDBleedAbility.Description";

        internal const string CBleedDebuff = "ArcaneDeedCBleedDebuff";
        internal const string CBleedDebuffGuid = "{094A018E-4941-4D3A-BC85-2CF206812756}";
        internal const string CBleedBuff = "ArcaneDeedCBleedBuff";
        internal const string CBleedBuffGuid = "{D7B075E8-44D3-4974-B8CB-6E6110C1A1FF}";
        internal const string CBleedAbility = "ArcaneDeedCBleedAbility";
        internal const string CBleedAbilityGuid = "{52F42DAA-F63D-4851-8DFF-34589C51C564}";
        internal const string CBleedDebuffDisplayName = "ArcaneDeedCBleed.Name";
        internal const string CBleedDebuffDescription = "ArcaneDeedCBleedDebuff.Description";
        internal const string CBleedAbilityDescription = "ArcaneDeedCBleedAbility.Description";

        // Targeted Strike
        internal const string TargetedStrike = "ArcaneDeedTargetedStrike";
        internal const string TargetedStrikeGuid = "{6AA0A608-7AD3-4965-9334-7048CEBEF7F5}";
        internal const string TargetedStrikeDisplayName = "ArcaneDeedTargetedStrike.Name";
        internal const string TargetedStrikeDescription = "ArcaneDeedTargetedStrike.Description";
        internal const string TSAbility = "ArcaneDeedTSAbility";
        internal const string TSAbilityGuid = "{B8698F63-81AA-47C4-B0D4-B421D84DD64B}";

        internal const string TSArms = "ArcaneDeedTSArms";
        internal const string TSArmsGuid = "{29B0EF95-D090-4560-99E1-50472E7F7A6F}";
        internal const string TSArmsDisplayName = "ArcaneDeedTSArms.Name";
        internal const string TSArmsDescription = "ArcaneDeedTSArms.Description";

        internal const string TSHead = "ArcaneDeedTSHead";
        internal const string TSHeadGuid = "{FF18CF80-AE5D-4564-A0BE-8F63BF22C66E}";
        internal const string TSHeadDisplayName = "ArcaneDeedTSHead.Name";
        internal const string TSHeadDescription = "ArcaneDeedTSHead.Description";

        internal const string TSLegs = "ArcaneDeedTSLegs";
        internal const string TSLegsGuid = "{5C9B676D-54CE-4F56-B7E4-F487FC4CCC35}";
        internal const string TSLegsDisplayName = "ArcaneDeedTSLegs.Name";
        internal const string TSLegsDescription = "ArcaneDeedTSLegs.Description";

        internal const string TSTorso = "ArcaneDeedTSTorso";
        internal const string TSTorsoGuid = "{196A0BBC-D4F6-4D89-B6E2-15509EC0067C}";
        internal const string TSTorsoDisplayName = "ArcaneDeedTSTorso.Name";
        internal const string TSTorsoDescription = "ArcaneDeedTSTorso.Description";

        internal static void Configure()
        {
            var icon = FeatureRefs.Bravery.Reference.Get().Icon;
            var dodgingpanacheicon = AbilityRefs.MirrorImage.Reference.Get().Icon;

            // Targeted Strike
            var arms_ability = AbilityConfigurator.New(TSArms, TSArmsGuid)
                .SetDisplayName(TSArmsDisplayName)
                .SetDescription(TSArmsDescription)
                .SetIcon(AbilityRefs.StunningFistFatigueAbility.Reference.Get().Icon)
                .SetType(AbilityType.Extraordinary)
                .AllowTargeting(enemies: true)
                .SetIsFullRoundAction()
                .SetRange(AbilityRange.Weapon)
                .SetAnimation(CastAnimationStyle.Immediate)
                .AddAbilityEffectRunAction(ActionsBuilder.New().Add<Disarm>().Build())
                .AddComponent<AbilityCasterDeedWeaponCheck>()
                .AddComponent<AbilityTargetNotImmuneToCritical>()
                .AddComponent<AbilityTargetNotImmuneToPrecision>()
                .AddComponent<AbilityDeliverAttackWithWeaponOnHit>()
                .AddAbilityResourceLogic(requiredResource: AbilityResourceRefs.ArcanePoolResourse.ToString(), isSpendResource: true, amount: 1)
                .Configure();

            var head_ability = AbilityConfigurator.New(TSHead, TSHeadGuid)
                .SetDisplayName(TSHeadDisplayName)
                .SetDescription(TSHeadDescription)
                .SetIcon(FeatureRefs.SlipperyMind.Reference.Get().Icon)
                .SetType(AbilityType.Extraordinary)
                .AllowTargeting(enemies: true)
                .SetIsFullRoundAction()
                .SetRange(AbilityRange.Weapon)
                .SetAnimation(CastAnimationStyle.Immediate)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(BuffRefs.Confusion.Reference.Get(), ContextDuration.Fixed(1)).Build())
                .AddComponent<AbilityCasterDeedWeaponCheck>()
                .AddComponent<AbilityTargetNotImmuneToCritical>()
                .AddComponent<AbilityTargetNotImmuneToPrecision>()
                .AddComponent<AbilityTargetNotImmuneToMindAffecting>()
                .AddComponent<AbilityDeliverAttackWithWeaponOnHit>()
                .AddAbilityResourceLogic(requiredResource: AbilityResourceRefs.ArcanePoolResourse.ToString(), isSpendResource: true, amount: 1)
            .Configure();

            var legs_ability = AbilityConfigurator.New(TSLegs, TSLegsGuid)
                .SetDisplayName(TSLegsDisplayName)
                .SetDescription(TSLegsDescription)
                .SetIcon(FeatureRefs.Trailblazer.Reference.Get().Icon)
                .SetIcon(AbilityRefs.Grease.Reference.Get().Icon)
                .SetType(AbilityType.Extraordinary)
                .AllowTargeting(enemies: true)
                .SetIsFullRoundAction()
                .SetRange(AbilityRange.Weapon)
                .SetAnimation(CastAnimationStyle.Immediate)
                .AddAbilityEffectRunAction(ActionsBuilder.New().KnockdownTarget().Build())
                .AddComponent<AbilityCasterDeedWeaponCheck>()
                .AddComponent<AbilityTargetNotImmuneToCritical>()
                .AddComponent<AbilityTargetNotImmuneToPrecision>()
                .AddComponent<AbilityTargetNotImmuneToProne>()
                .AddComponent<AbilityDeliverAttackWithWeaponOnHit>()
                .AddAbilityResourceLogic(requiredResource: AbilityResourceRefs.ArcanePoolResourse.ToString(), isSpendResource: true, amount: 1)
                .Configure();

            var torso_ability = AbilityConfigurator.New(TSTorso, TSTorsoGuid)
                .SetDisplayName(TSTorsoDisplayName)
                .SetDescription(TSTorsoDescription)
                .SetIcon(AbilityRefs.EnlargePerson.Reference.Get().Icon)
                .SetType(AbilityType.Extraordinary)
                .AllowTargeting(enemies: true)
                .SetIsFullRoundAction()
                .SetRange(AbilityRange.Weapon)
                .SetAnimation(CastAnimationStyle.Immediate)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(BuffRefs.Staggered.Reference.Get(), ContextDuration.Fixed(1)).Build())
                .AddComponent<AbilityCasterDeedWeaponCheck>()
                .AddComponent<AbilityTargetNotImmuneToCritical>()
                .AddComponent<AbilityTargetNotImmuneToPrecision>()
                .AddComponent<AbilityDeliverAttackWithWeaponOnHit>()
                .AddAbilityResourceLogic(requiredResource: AbilityResourceRefs.ArcanePoolResourse.ToString(), isSpendResource: true, amount: 1)
                .Configure();

            var targeted_strike_ability = AbilityConfigurator.New(TSAbility, TSAbilityGuid)
                .SetDisplayName(TargetedStrikeDisplayName)
                .SetDescription(TargetedStrikeDescription)
                .SetIcon(FeatureRefs.TwoHandedFighterBackswing.Reference.Get().Icon)
                .AddAbilityVariants(new() { arms_ability, head_ability, legs_ability, torso_ability })
                .SetIsFullRoundAction()
                .Configure();

            var targeted_strike_feat = FeatureConfigurator.New(TargetedStrike, TargetedStrikeGuid)
                .SetDisplayName(TargetedStrikeDisplayName)
                .SetDescription(TargetedStrikeDescription)
                .SetIcon(FeatureRefs.TwoHandedFighterBackswing.Reference.Get().Icon)
                .AddFacts(new() { targeted_strike_ability })
                .SetIsClassFeature()
                .AddPrerequisiteClassLevel(CharacterClassRefs.MagusClass.ToString(), 7)
                .Configure();

            // Bleeding Wound
            var bleeding_wound_group = (ActivatableAbilityGroup)ECActivatableAbilityGroup.BleedingWoundAbilities;

            var bleed_debuff = BuffConfigurator.New(BleedDebuff, BleedDebuffGuid)
                .CopyFrom(
                    BuffRefs.Bleed1d4Buff,
                    typeof(AddHealTrigger),
                    typeof(SpellDescriptorComponent),
                    typeof(CombatStateTrigger))
                .SetDisplayName(BleedDebuffDisplayName)
                .SetDescription(BleedDebuffDescription)
                .SetIcon(BuffRefs.Bleed1d4Buff.Reference.Get().Icon)
                .AddNotDispelable()
                .AddComponent<AddFactContextActions>(c => { c.NewRound = ActionsBuilder.New().Add<ContextActionDealDamage>(c => { c.m_Type = ContextActionDealDamage.Type.Damage; c.DamageType = DamageTypes.Direct(); c.Value = ContextDice.Value(DiceType.Zero, diceCount: 0, bonus: ContextValues.Property(UnitProperty.StatBonusDexterity, true)); }).Build(); })
                .Configure();

            var strength_bleed_debuff = BuffConfigurator.New(SBleedDebuff, SBleedDebuffGuid)
                .CopyFrom(
                    BuffRefs.Bleed1d4Buff,
                    typeof(AddHealTrigger),
                    typeof(SpellDescriptorComponent),
                    typeof(CombatStateTrigger))
                .SetDisplayName(SBleedDebuffDisplayName)
                .SetDescription(SBleedDebuffDescription)
                .SetIcon(BuffRefs.StrengthDamage.Reference.Get().Icon)
                .AddNotDispelable()
                .AddComponent<AddFactContextActions>(c => { c.NewRound = ActionsBuilder.New().Add<ContextActionDealDamage>(c => { c.m_Type = ContextActionDealDamage.Type.AbilityDamage; c.AbilityType = StatType.Strength; c.Value = ContextDice.Value(DiceType.Zero, diceCount: 0, bonus: ContextValues.Constant(1)); }).Build(); })
                .Configure();

            var dexterity_bleed_debuff = BuffConfigurator.New(DBleedDebuff, DBleedDebuffGuid)
                .CopyFrom(
                    BuffRefs.Bleed1d4Buff,
                    typeof(AddHealTrigger),
                    typeof(SpellDescriptorComponent),
                    typeof(CombatStateTrigger))
                .SetDisplayName(DBleedDebuffDisplayName)
                .SetDescription(DBleedDebuffDescription)
                .SetIcon(BuffRefs.DexterityDamage.Reference.Get().Icon)
                .AddNotDispelable()
                .AddComponent<AddFactContextActions>(c => { c.NewRound = ActionsBuilder.New().Add<ContextActionDealDamage>(c => { c.m_Type = ContextActionDealDamage.Type.AbilityDamage; c.AbilityType = StatType.Dexterity; c.Value = ContextDice.Value(DiceType.Zero, diceCount: 0, bonus: ContextValues.Constant(1)); }).Build(); })
                .Configure();

            var constitution_bleed_debuff = BuffConfigurator.New(CBleedDebuff, CBleedDebuffGuid)
                .CopyFrom(
                    BuffRefs.Bleed1d4Buff,
                    typeof(AddHealTrigger),
                    typeof(SpellDescriptorComponent),
                    typeof(CombatStateTrigger))
                .SetDisplayName(CBleedDebuffDisplayName)
                .SetDescription(CBleedDebuffDescription)
                .SetIcon(BuffRefs.ConstitutionDamage.Reference.Get().Icon)
                .AddNotDispelable()
                .AddComponent<AddFactContextActions>(c => { c.NewRound = ActionsBuilder.New().Add<ContextActionDealDamage>(c => { c.m_Type = ContextActionDealDamage.Type.AbilityDamage; c.AbilityType = StatType.Constitution; c.Value = ContextDice.Value(DiceType.Zero, diceCount: 0, bonus: ContextValues.Constant(1)); }).Build(); })
                .Configure();

            var bleed_buff = BuffConfigurator.New(BleedBuff, BleedBuffGuid)
                .AddNotDispelable()
                .SetFlags(BlueprintBuff.Flags.HiddenInUi)
                .AddInitiatorAttackWithWeaponTrigger(duelistWeapon: true, onlyHit: true, action: ActionsBuilder.New().Conditional(ConditionsBuilder.New().HasBuff(bleed_debuff).Build(), ifFalse: ActionsBuilder.New().ApplyBuffPermanent(bleed_debuff).Add<ContextSpendArcanePool>(c => { c.Value = 1; }).Build()))
                .Configure();

            var strength_bleed_buff = BuffConfigurator.New(SBleedBuff, SBleedBuffGuid)
                .AddNotDispelable()
                .SetFlags(BlueprintBuff.Flags.HiddenInUi)
                .AddInitiatorAttackWithWeaponTrigger(duelistWeapon: true, onlyHit: true, action: ActionsBuilder.New().Conditional(ConditionsBuilder.New().HasBuff(strength_bleed_debuff).Build(), ifFalse: ActionsBuilder.New().ApplyBuffPermanent(strength_bleed_debuff).Add<ContextSpendArcanePool>(c => { c.Value = 2; }).Build()))
                .Configure();

            var dexterity_bleed_buff = BuffConfigurator.New(DBleedBuff, DBleedBuffGuid)
                .AddNotDispelable()
                .SetFlags(BlueprintBuff.Flags.HiddenInUi)
                .AddInitiatorAttackWithWeaponTrigger(duelistWeapon: true, onlyHit: true, action: ActionsBuilder.New().Conditional(ConditionsBuilder.New().HasBuff(dexterity_bleed_debuff).Build(), ifFalse: ActionsBuilder.New().ApplyBuffPermanent(dexterity_bleed_debuff).Add<ContextSpendArcanePool>(c => { c.Value = 2; }).Build()))
                .Configure();

            var constitution_bleed_buff = BuffConfigurator.New(CBleedBuff, CBleedBuffGuid)
                .AddNotDispelable()
                .SetFlags(BlueprintBuff.Flags.HiddenInUi)
                .AddInitiatorAttackWithWeaponTrigger(duelistWeapon: true, onlyHit: true, action: ActionsBuilder.New().Conditional(ConditionsBuilder.New().HasBuff(constitution_bleed_debuff).Build(), ifFalse: ActionsBuilder.New().ApplyBuffPermanent(constitution_bleed_debuff).Add<ContextSpendArcanePool>(c => { c.Value = 2; }).Build()))
                .Configure();

            var bleed_ability = ActivatableAbilityConfigurator.New(BleedAbility, BleedAbilityGuid)
                .SetDisplayName(BleedDebuffDisplayName)
                .SetDescription(BleedAbilityDescription)
                .SetIcon(BuffRefs.Bleed1d4Buff.Reference.Get().Icon)
                .SetBuff(bleed_buff)
                .SetDeactivateImmediately()
                .AddComponent<AddAbilityResourceDepletedTrigger>(c => { c.m_Resource = BlueprintTool.GetRef<BlueprintAbilityResourceReference>(AbilityResourceRefs.ArcanePoolResourse.ToString()); c.Action = ActionsBuilder.New().RemoveSelf().Build(); c.Cost = 1; })
                .SetHiddenInUI()
                .SetGroup(bleeding_wound_group)
                .Configure();

            var strength_bleed_ability = ActivatableAbilityConfigurator.New(SBleedAbility, SBleedAbilityGuid)
                .SetDisplayName(SBleedDebuffDisplayName)
                .SetDescription(SBleedAbilityDescription)
                .SetIcon(BuffRefs.StrengthDamage.Reference.Get().Icon)
                .SetBuff(strength_bleed_buff)
                .SetDeactivateImmediately()
                .AddComponent<AddAbilityResourceDepletedTrigger>(c => { c.m_Resource = BlueprintTool.GetRef<BlueprintAbilityResourceReference>(AbilityResourceRefs.ArcanePoolResourse.ToString()); c.Action = ActionsBuilder.New().RemoveSelf().Build(); c.Cost = 2; })
                .SetHiddenInUI()
                .SetGroup(bleeding_wound_group)
                .Configure();

            var dexterity_bleed_ability = ActivatableAbilityConfigurator.New(DBleedAbility, DBleedAbilityGuid)
                .SetDisplayName(DBleedDebuffDisplayName)
                .SetDescription(DBleedAbilityDescription)
                .SetIcon(BuffRefs.DexterityDamage.Reference.Get().Icon)
                .SetBuff(dexterity_bleed_buff)
                .SetDeactivateImmediately()
                .AddComponent<AddAbilityResourceDepletedTrigger>(c => { c.m_Resource = BlueprintTool.GetRef<BlueprintAbilityResourceReference>(AbilityResourceRefs.ArcanePoolResourse.ToString()); c.Action = ActionsBuilder.New().RemoveSelf().Build(); c.Cost = 2; })
                .SetHiddenInUI()
                .SetGroup(bleeding_wound_group)
                .Configure();

            var constitution_bleed_ability = ActivatableAbilityConfigurator.New(CBleedAbility, CBleedAbilityGuid)
                .SetDisplayName(CBleedDebuffDisplayName)
                .SetDescription(CBleedAbilityDescription)
                .SetIcon(BuffRefs.ConstitutionDamage.Reference.Get().Icon)
                .SetBuff(constitution_bleed_buff)
                .SetDeactivateImmediately()
                .AddComponent<AddAbilityResourceDepletedTrigger>(c => {c.m_Resource = BlueprintTool.GetRef<BlueprintAbilityResourceReference>(AbilityResourceRefs.ArcanePoolResourse.ToString()); c.Action = ActionsBuilder.New().RemoveSelf().Build(); c.Cost = 2;})
                .SetHiddenInUI()
                .SetGroup(bleeding_wound_group)
                .Configure();

            var bleeding_wound_ability = ActivatableAbilityConfigurator.New(BWoundAbility, BWoundAbilityGuid)
                .SetDisplayName(BWoundDisplayName)
                .SetDescription(BWoundDescription)
                .SetIcon(ActivatableAbilityRefs.DuelistCripplingCriticalBleedAbility.Reference.Get().Icon)
                .AddActivatableAbilityVariants(variants: new() { bleed_ability, strength_bleed_ability, dexterity_bleed_ability, constitution_bleed_ability })
                .AddActivationDisable()
                .Configure();

            var bleeding_wound_feat = FeatureConfigurator.New(BWound, BWoundGuid)
                .SetDisplayName(BWoundDisplayName)
                .SetDescription(BWoundDescription)
                .SetIcon(ActivatableAbilityRefs.DuelistCripplingCriticalBleedAbility.Reference.Get().Icon)
                .AddFacts(new() { bleeding_wound_ability, bleed_ability, strength_bleed_ability, dexterity_bleed_ability, constitution_bleed_ability })
                .SetIsClassFeature()
                .AddPrerequisiteClassLevel(CharacterClassRefs.MagusClass.ToString(), 11)
                .Configure();

            // Dodging Panache
            var dodging_buff = BuffConfigurator.New(DodgingPanacheBuffName, DodgingPanacheBuffGuid)
                .SetDisplayName(DodgingPanacheDisplayName)
                .SetDescription(DodgingPanacheDescription)
                .SetIcon(AbilityRefs.MirrorImage.Reference.Get().Icon)
                .AddNotDispelable()
                .AddComponent<ArcaneDeedDodgingPanache>()
                .AddComponent<AddAbilityResourceDepletedTrigger>(c => { c.m_Resource = BlueprintTool.GetRef<BlueprintAbilityResourceReference>(AbilityResourceRefs.ArcanePoolResourse.ToString()); c.Action = ActionsBuilder.New().RemoveSelf().Build(); c.Cost = 1; })
                .Configure();

            var dodging_ability = ActivatableAbilityConfigurator.New(DodgingPanacheAbilityName, DodgingPanacheAbilityGuid)
                .SetDisplayName(DodgingPanacheDisplayName)
                .SetDescription(DodgingPanacheDescription)
                .SetIcon(AbilityRefs.MirrorImage.Reference.Get().Icon)
                .AddActivatableAbilityResourceLogic(requiredResource: AbilityResourceRefs.ArcanePoolResourse.ToString(), spendType: ActivatableAbilityResourceLogic.ResourceSpendType.Never)
                .SetBuff(dodging_buff)
                .SetDeactivateImmediately()
                .Configure();

            var dodging_feat = FeatureConfigurator.New(DodgingPanacheName, DodgingPanacheGuid)
                .SetDisplayName(DodgingPanacheDisplayName)
                .SetDescription(DodgingPanacheDescription)
                .SetIcon(dodgingpanacheicon)
                .AddFacts(new() { dodging_ability })
                .SetIsClassFeature()
                .AddPrerequisiteClassLevel(CharacterClassRefs.MagusClass.ToString(), 1)
                .Configure();

            // Main Arcane Deed Feature Selection
            var selection = FeatureSelectionConfigurator.New(ArcaneDeedName, ArcaneDeedGuid, FeatureGroup.MagusArcana, FeatureGroup.EldritchScionArcana)
                .SetDisplayName(ArcaneDeedDisplayName)
                .SetDescription(ArcaneDeedDescription)
                .SetIcon(icon)
                .SetReapplyOnLevelUp(true)
                .SetIsClassFeature(true)
                .AddComponent<FeatureTagsComponent>(c =>
                {
                    c.FeatureTags = FeatureTag.ClassSpecific;
                })
                .AddPrerequisiteClassLevel(CharacterClassRefs.MagusClass.ToString(), 1)
                .AddPrerequisiteFeature("4FEF6080-4942-44EC-91B3-DA915E324675")
                .AddToAllFeatures(dodging_feat, bleeding_wound_feat, targeted_strike_feat)
                .Configure();

            // Add to selections
            FeatureSelectionConfigurator.For(FeatureSelectionRefs.MagusArcanaSelection)
                .AddToAllFeatures(selection)
                .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.EldritchMagusArcanaSelection)
                .AddToAllFeatures(selection)
                .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.HexcrafterMagusHexArcanaSelection)
                .AddToAllFeatures(selection)
                .Configure();

            FeatureSelectionConfigurator.For(FeatureSelectionRefs.ExtraArcanaSelection)
                .AddToAllFeatures(selection)
                .Configure();
        }
    }
}
