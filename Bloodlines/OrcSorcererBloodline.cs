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

namespace EbonsContentMod.Bloodlines
{
    internal class OrcSorcererBloodline
    {
        private static readonly string OrcSorcererBloodlineName = "OrcSorcererBloodline";

        internal const string OrcSorcererBloodlineDisplayName = "OrcSorcererBloodline.Name";
        private static readonly string OrcSorcererBloodlineDescription = "OrcSorcererBloodline.Description";
        public static readonly string OrcSorcererBloodlineGuid = "{0CB461F6-17BD-45A8-9583-451D946357EC}";

        private static readonly string OrcCrossbloodedBloodlineName = "OrcCrossbloodedBloodline";
        public static readonly string OrcCrossbloodedBloodlineGuid = "{34FE03A3-B592-4721-8F85-E3AB64132C07}";

        private static readonly string OrcSeekerBloodlineName = "OrcSeekerBloodline";
        public static readonly string OrcSeekerBloodlineGuid = "{B972CA31-D32C-4927-A9DB-C98A251E14F0}";

        internal const string OrcSorcererBloodlineArcanaDisplayName = "OrcSorcererBloodlineArcana.Name";
        private static readonly string OrcSorcererBloodlineArcanaDescription = "OrcSorcererBloodlineArcana.Description";

        internal const string OrcSorcererBloodlineClassSkillDisplayName = "OrcSorcererBloodlineClassSkill.Name";
        private static readonly string OrcSorcererBloodlineClassSkillDescription = "OrcSorcererBloodlineClassSkill.Description";

        internal const string OrcSorcererBloodlineRequisiteFeatureDisplayName = "OrcSorcererBloodlineRequisiteFeature.Name";
        private static readonly string OrcSorcererBloodlineRequisiteFeatureDescription = "OrcSorcererBloodlineRequisiteFeature.Description";

        internal const string OrcSorcererBloodlineTouchOfRageDisplayName = "OrcSorcererBloodlineTouchOfRage.Name";
        private static readonly string OrcSorcererBloodlineTouchOfRageDescription = "OrcSorcererBloodlineTouchOfRage.Description";

        internal const string OrcSorcererBloodlineFearlessDisplayName = "OrcSorcererBloodlineFearless.Name";
        private static readonly string OrcSorcererBloodlineFearlessDescription = "OrcSorcererBloodlineFearless.Description";

        internal const string OrcSorcererBloodlineStrengthOfTheBeastDisplayName = "OrcSorcererBloodlineStrengthOfTheBeast.Name";
        private static readonly string OrcSorcererBloodlineStrengthOfTheBeastDescription = "OrcSorcererBloodlineStrengthOfTheBeast.Description";

        internal const string OrcSorcererBloodlinePowerOfGiantsDisplayName = "OrcSorcererBloodlinePowerOfGiants.Name";
        private static readonly string OrcSorcererBloodlinePowerOfGiantsDescription = "OrcSorcererBloodlinePowerOfGiants.Description";

        internal const string OrcSorcererBloodlineWarlordRebornDisplayName = "OrcSorcererBloodlineWarlordReborn.Name";
        private static readonly string OrcSorcererBloodlineWarlordRebornDescription = "OrcSorcererBloodlineWarlordReborn.Description";

        private static readonly string OrcSorcererBloodlineSpellsDescription = "OrcSorcererBloodlineSpells.Description";

        internal const string OrcSorcererBloodlineFeatsDisplayName = "OrcSorcererBloodlineFeats.Name";
        private static readonly string OrcSorcererBloodlineFeatsDescription = "OrcSorcererBloodlineFeats.Description";

        internal static void Configure()
        {
            if (!CheckerUtilities.GetModActive("TabletopTweaks-Base")) return;

            var sorcererclass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>(CharacterClassRefs.SorcererClass.ToString());
            var magusclass = BlueprintTools.GetBlueprintReference<BlueprintCharacterClassReference>(CharacterClassRefs.MagusClass.ToString());
            var eldritchscionarchetype = BlueprintTools.GetBlueprintReference<BlueprintArchetypeReference>(ArchetypeRefs.EldritchScionArchetype.ToString());

            var LightSensitivity = BlueprintTools.GetBlueprint<BlueprintFeature>("{BBA86DB1-B3EF-4AB5-A412-A5910F2B8947}");
            var LightSensitivityImmunity = BlueprintTools.GetBlueprint<BlueprintFeature>("{4EA922BA-F88E-4B8E-A378-50C3D0E2DA2E}");

            var spell3 = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(AbilityRefs.BurningHands.ToString());
            var spell5 = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(AbilityRefs.BullsStrength.ToString());
            var spell7 = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(AbilityRefs.Rage.ToString());
            var spell9 = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(AbilityRefs.ControlledFireball.ToString());
            var spell11 = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(AbilityRefs.Cloudkill.ToString());
            var spell13 = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(AbilityRefs.Transformation.ToString());
            var spell15 = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(AbilityRefs.FireStorm.ToString());
            var spell17 = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(AbilityRefs.IronBody.ToString());
            var spell19 = BlueprintTools.GetBlueprintReference<BlueprintAbilityReference>(AbilityRefs.MeteorSwarm.ToString());
            
            var OrcSorcererBloodlineArcana = FeatureConfigurator.New("OrcSorcererBloodlineArcana", "{7EFCA415-649A-4FB4-BC92-1FBB5BA271A1}") // done
                .SetDisplayName(OrcSorcererBloodlineArcanaDisplayName)
                .SetDescription(OrcSorcererBloodlineArcanaDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.ElementalBodyIBase.ToString()).Icon)
                .AddFacts([FeatureRefs.KeenSenses.ToString()])
                .AddComponent<OrcBloodlineArcana>(c =>
                {
                    c.SpellsOnly = true;
                    c.UseContextBonus = false;
                    c.Value = 1; // Not needed I suppose
                })
                .SetIsClassFeature(true)
                .SetRanks(1)
                .Configure();

            var OrcSorcererBloodlineArcanaProgression = ProgressionConfigurator.New("OrcSorcererBloodlineArcanaProgression", "{E33D77CC-95A3-486F-B8B4-5FC303045C8C}") // done
                .SetDisplayName(OrcSorcererBloodlineArcanaDisplayName)
                .SetDescription(OrcSorcererBloodlineArcanaDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.ElementalBodyIBase.ToString()).Icon)
                .SetIsClassFeature(true)
                .SetGiveFeaturesForPreviousLevels(true)
                .SetRanks(1)
                .AddToLevelEntries(1, OrcSorcererBloodlineArcana, LightSensitivity)
                .AddToLevelEntries(9, LightSensitivityImmunity)
                .Configure();

            var OrcSorcererBloodlineClassSkill = FeatureConfigurator.New("OrcSorcererBloodlineClassSkill", "{D2EC1525-3BB8-4DB3-8194-146E9EF9BF9C}") // done
                .SetDisplayName(OrcSorcererBloodlineClassSkillDisplayName)
                .SetDescription(OrcSorcererBloodlineClassSkillDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.SkillFocusLoreNature.ToString()).Icon)
                .AddComponent<AddClassSkill>(c => {
                    c.Skill = StatType.SkillLoreNature;
                })
                .SetIsClassFeature(true)
                .SetRanks(1)
                .Configure();

            var OrcSorcererBloodlineTouchOfRageResource = AbilityResourceConfigurator.New("OrcSorcererBloodlineTouchOfRageResource", "{68E17C17-BB36-4DA8-B162-675E3E76BA4A}") // done
                .SetMaxAmount( ResourceAmountBuilder.New(3).IncreaseByStat(StatType.Charisma))
                .Configure();

            var OrcSorcererBloodlineTouchOfRageAbilityBuff = BuffConfigurator.New("OrcSorcererBloodlineTouchOfRageAbilityBuff", "{8A935E8B-0204-4C3D-AD35-A609CD4AF862}") // done
                .CopyFrom(BuffRefs.NobilityDomainBaseBuff, c => c is not (AbilityEffectRunAction or AbilityResourceLogic or AddStatBonus or BuffAllSkillsBonus or BuffAllSavesBonus))
                .SetDisplayName(OrcSorcererBloodlineTouchOfRageDisplayName)
                .SetDescription(OrcSorcererBloodlineTouchOfRageDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.KiQuiveringPalm.ToString()).Icon)
                .AddContextStatBonus(StatType.SaveWill, ContextValues.Rank(), descriptor: Kingmaker.Enums.ModifierDescriptor.Morale)
                .AddComponent<AddContextWeaponDamageBonus>(c =>
                {
                    c.Value = ContextValues.Rank();
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.Morale;
                })
                .AddComponent<AddContextRankAttackBonus>(c =>
                {
                    c.Value = ContextValues.Rank();
                    c.Descriptor = Kingmaker.Enums.ModifierDescriptor.Morale;
                })
                .AddContextRankConfig(ContextRankConfigs.ClassLevel([CharacterClassRefs.SorcererClass.ToString()]).WithDiv2Progression())
                .SetRanks(1)
                .Configure();

            var OrcSorcererBloodlineTouchOfRageAbility = AbilityConfigurator.New("OrcSorcererBloodlineTouchOfRageAbilityTouch", "{91A12E9F-DCD7-443B-9FB4-043C5F257D7D}") // done
                .CopyFrom(AbilityRefs.WarDomainBaseAbility, c => c is not (AbilityEffectRunAction or AbilityResourceLogic))
                .SetDisplayName(OrcSorcererBloodlineTouchOfRageDisplayName)
                .SetDescription(OrcSorcererBloodlineTouchOfRageDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.KiQuiveringPalm.ToString()).Icon)
                .AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(OrcSorcererBloodlineTouchOfRageAbilityBuff, durationValue: ContextDuration.Fixed(1)).Build())
                //.AddAbilityEffectRunAction(ActionsBuilder.New().ApplyBuff(OrcSorcererBloodlineTouchOfRageAbilityBuff, durationValue: ContextDuration.Variable(ContextValues.Rank())).Build())
                //.AddContextRankConfig(ContextRankConfigs.ClassLevel([CharacterClassRefs.SorcererClass.ToString()]).WithDiv2Progression())
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: OrcSorcererBloodlineTouchOfRageResource)
                .SetActionType(UnitCommand.CommandType.Standard)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 1)
                .Configure();


            /*var OrcSorcererBloodlineTouchOfRageAbility = AbilityConfigurator.New("OrcSorcererBloodlineTouchOfRageAbility", "{D5355EEA-EA74-4309-B974-28DC6F583D3A}") // done
                .CopyFrom(AbilityRefs.WarDomainBaseAbilityCast, c => c is not AbilityEffectStickyTouch)
                .SetDisplayName(OrcSorcererBloodlineTouchOfRageDisplayName)
                .SetDescription(OrcSorcererBloodlineTouchOfRageDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.KiQuiveringPalm.ToString()).Icon)
                .AddAbilityEffectStickyTouch(touchDeliveryAbility: OrcSorcererBloodlineTouchOfRageAbilityTouch)
                //.AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: OrcSorcererBloodlineTouchOfRageResource)
                .SetActionType(Kingmaker.UnitLogic.Commands.Base.UnitCommand.CommandType.Standard)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 1)
                .Configure();*/

            var OrcSorcererBloodlineTouchOfRage = FeatureConfigurator.New("OrcSorcererBloodlineTouchOfRage", "{33A4F8A3-BEF8-48A3-BB3C-799C4CA5FB8D}") // done
                .SetDisplayName(OrcSorcererBloodlineTouchOfRageDisplayName)
                .SetDescription(OrcSorcererBloodlineTouchOfRageDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.KiQuiveringPalm.ToString()).Icon)
                .AddFacts([OrcSorcererBloodlineTouchOfRageAbility])
                .AddAbilityResources(resource: OrcSorcererBloodlineTouchOfRageResource, restoreAmount: true)
                .SetIsClassFeature(true)
                .SetRanks(1)
                .Configure();

            var OrcSorcererBloodlineRequisiteFeature = FeatureConfigurator.New("OrcSorcererBloodlineRequisiteFeature", "{09AE4A9F-250C-42AD-9F31-A15B3FCED759}") // done
                .SetDisplayName(OrcSorcererBloodlineRequisiteFeatureDisplayName)
                .SetDescription(OrcSorcererBloodlineRequisiteFeatureDescription)
                .SetHideInUI(true)
                .SetHideInCharacterSheetAndLevelUp(true)
                .SetIsClassFeature(true)
                .SetRanks(1)
                .Configure();

            var BloodlineRequisiteFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("e2cfd3ce-df7c-4008-8b25-aa82d6db3c77"); // BloodlineRequisiteFeature from TTT

            var OrcSorcererBloodlineFearless1 = FeatureConfigurator.New("OrcSorcererBloodlineFearless1", "{CE8CF3B1-4C0C-4B14-B102-F05896931352}") // done
                .SetDisplayName(OrcSorcererBloodlineFearlessDisplayName)
                .SetDescription(OrcSorcererBloodlineFearlessDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.BlessingOfCourageAndLife.ToString()).Icon)
                .AddSavingThrowBonusAgainstDescriptor(4, modifierDescriptor: Kingmaker.Enums.ModifierDescriptor.UntypedStackable, spellDescriptor: SpellDescriptor.Fear)
                .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.NaturalArmor, stat: StatType.AC, value: 1)
                .SetIsClassFeature(true)
                .SetRanks(1)
                .Configure();

            var OrcSorcererBloodlineFearless2 = FeatureConfigurator.New("OrcSorcererBloodlineFearless2", "{2A13439B-D5F5-4B3B-B561-FD33D08C865A}") // done
                .SetDisplayName(OrcSorcererBloodlineFearlessDisplayName)
                .SetDescription(OrcSorcererBloodlineFearlessDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.BlessingOfCourageAndLife.ToString()).Icon)
                .AddBuffDescriptorImmunity(descriptor: SpellDescriptor.Fear)
                .AddStatBonus(Kingmaker.Enums.ModifierDescriptor.NaturalArmor, stat: StatType.AC, value: 2)
                .AddRemoveFeatureOnApply(OrcSorcererBloodlineFearless1)
                .SetIsClassFeature(true)
                .SetRanks(1)
                .Configure();

            var OrcSorcererBloodlineStrengthOfTheBeast1 = FeatureConfigurator.New("OrcSorcererBloodlineStrengthOfTheBeast1", "{8DBFBA5B-45A4-45DA-A153-9D314C4D1484}") // done
                .CopyFrom(FeatureRefs.BloodlineAbyssalStrengthAbilityLevel1, c => c)
                //.SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.FocusedRageFeature.ToString()).Icon) //
                .SetDisplayName(OrcSorcererBloodlineStrengthOfTheBeastDisplayName)
                .SetDescription(OrcSorcererBloodlineStrengthOfTheBeastDescription)
                .Configure();

            var OrcSorcererBloodlineStrengthOfTheBeast2 = FeatureConfigurator.New("OrcSorcererBloodlineStrengthOfTheBeast2", "{C996FDC6-0D4A-4E86-A65C-6299AF6DD5D9}") // done
                .CopyFrom(FeatureRefs.BloodlineAbyssalStrengthAbilityLevel2, c => c)
                //.SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.FocusedRageFeature.ToString()).Icon) //
                .SetDisplayName(OrcSorcererBloodlineStrengthOfTheBeastDisplayName)
                .SetDescription(OrcSorcererBloodlineStrengthOfTheBeastDescription)
                .AddRemoveFeatureOnApply(OrcSorcererBloodlineStrengthOfTheBeast1)
                .Configure();

            var OrcSorcererBloodlineStrengthOfTheBeast3 = FeatureConfigurator.New("OrcSorcererBloodlineStrengthOfTheBeast3", "{E14879E5-2FEA-48C9-A45C-4366D39188A4}") // done
                .CopyFrom(FeatureRefs.BloodlineAbyssalStrengthAbilityLevel3, c => c)
                //.SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.FocusedRageFeature.ToString()).Icon) //
                .SetDisplayName(OrcSorcererBloodlineStrengthOfTheBeastDisplayName)
                .SetDescription(OrcSorcererBloodlineStrengthOfTheBeastDescription)
                .AddRemoveFeatureOnApply(OrcSorcererBloodlineStrengthOfTheBeast2)
                .Configure();

            var OrcSorcererBloodlineStrengthOfTheBeastProgression = ProgressionConfigurator.New("OrcSorcererBloodlineStrengthOfTheBeastProgression", "{9AE9C5AF-E89F-4729-B7D1-BB6A8194FB2F}") // done
                .SetDisplayName(OrcSorcererBloodlineStrengthOfTheBeastDisplayName)
                .SetDescription(OrcSorcererBloodlineStrengthOfTheBeastDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.BloodlineAbyssalStrengthAbilityLevel1.ToString()).Icon)
                .SetIsClassFeature(true)
                .SetGiveFeaturesForPreviousLevels(true)
                .SetRanks(1)
                .AddToLevelEntries(9, OrcSorcererBloodlineStrengthOfTheBeast1)
                .AddToLevelEntries(13, OrcSorcererBloodlineStrengthOfTheBeast2)
                .AddToLevelEntries(17, OrcSorcererBloodlineStrengthOfTheBeast3)
                .Configure();

            var OrcSorcererBloodlinePowerOfGiantsResource = AbilityResourceConfigurator.New("OrcSorcererBloodlinePowerOfGiantsResource", "{FD10FF89-8D29-4101-B22D-DF44C170AB92}") // done
                .SetMaxAmount(
                    ResourceAmountBuilder.New(0).IncreaseByLevelStartPlusDivStep(classes: [CharacterClassRefs.SorcererClass.ToString()], otherClassLevelsMultiplier: 1, levelsPerStep: 1, bonusPerStep: 1, startingLevel: 0)) // I think this should work for all character levels
                .Configure();

            var OrcSorcererBloodlinePowerOfGiantsBuff = BuffConfigurator.New("OrcSorcererBloodlinePowerOfGiantsBuff", "{FAC897BF-FAF0-404D-BA45-5F150542E96C}") // done
                .CopyFrom(BuffRefs.EnlargePersonBuff, c => c is not AddGenericStatBonus)
                .SetDisplayName(OrcSorcererBloodlinePowerOfGiantsDisplayName)
                .SetDescription(OrcSorcererBloodlinePowerOfGiantsDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.FocusedRageFeature.ToString()).Icon)
                .AddGenericStatBonus(Kingmaker.Enums.ModifierDescriptor.Size, StatType.Strength, 6)
                .AddGenericStatBonus(Kingmaker.Enums.ModifierDescriptor.Size, StatType.Dexterity, -2)
                .AddGenericStatBonus(Kingmaker.Enums.ModifierDescriptor.Size, StatType.Constitution, 4)
                .AddGenericStatBonus(Kingmaker.Enums.ModifierDescriptor.NaturalArmor, StatType.AC, 4)
                .Configure();

            var OrcSorcererBloodlinePowerOfGiantsActivatableAbility = ActivatableAbilityConfigurator.New("OrcSorcererBloodlinePowerOfGiantsActivatableAbility", "{1DB167C2-4D3F-4D4E-9BBC-ED41A5392BAD}") // done
                .SetDisplayName(OrcSorcererBloodlinePowerOfGiantsDisplayName)
                .SetDescription(OrcSorcererBloodlinePowerOfGiantsDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.FocusedRageFeature.ToString()).Icon)
                .AddActivatableAbilityResourceLogic(requiredResource: OrcSorcererBloodlinePowerOfGiantsResource, spendType: Kingmaker.UnitLogic.ActivatableAbilities.ActivatableAbilityResourceLogic.ResourceSpendType.OncePerMinute)
                .SetBuff(OrcSorcererBloodlinePowerOfGiantsBuff)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfOwnerDisabled(true)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Standard)
                .Configure();

            var OrcSorcererBloodlinePowerOfGiants = FeatureConfigurator.New("OrcSorcererBloodlinePowerOfGiants", "{84591E6C-74EE-40CE-BA08-FE49F72D0221}") // done
                .SetDisplayName(OrcSorcererBloodlinePowerOfGiantsDisplayName)
                .SetDescription(OrcSorcererBloodlinePowerOfGiantsDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.FocusedRageFeature.ToString()).Icon)
                .AddFacts([OrcSorcererBloodlinePowerOfGiantsActivatableAbility])
                .AddAbilityResources(resource: OrcSorcererBloodlinePowerOfGiantsResource, restoreAmount: true)
                .Configure();

            var OrcSorcererBloodlineWarlordRebornTransformationResource = AbilityResourceConfigurator.New("OrcSorcererBloodlineWarlordRebornTransformationResource", "{290F6125-ABB3-4DCC-9F86-B628E0607831}") // done
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var OrcSorcererBloodlineWarlordRebornTransformation = AbilityConfigurator.New("OrcSorcererBloodlineWarlordRebornTransformation", "{1C1DDA18-95E9-4E1F-B2ED-30D09C47E8FE}") // done
                .CopyFrom(AbilityRefs.Transformation, c => c is not (SpellListComponent or CraftInfoComponent or ContextRankConfig))
                .SetDisplayName(OrcSorcererBloodlineWarlordRebornDisplayName)
                .SetDescription(OrcSorcererBloodlineWarlordRebornDescription)
                .AddAbilityResourceLogic(isSpendResource: true, requiredResource: OrcSorcererBloodlineWarlordRebornTransformationResource)
                //.AddComponent(Helpers.CreateCopy(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.Transformation.ToString()).Components.Where(c => c is ContextRankConfig).First()))
                .AddContextRankConfig(new ContextRankConfig()
                {
                    m_Type = Kingmaker.Enums.AbilityRankType.Default,
                    m_BaseValueType = ContextRankBaseValueType.CasterLevel,
                    m_Progression = ContextRankProgression.AsIs
                })
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 6)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintAbility>(AbilityRefs.Transformation.ToString()).Icon)
                .Configure();

            var OrcSorcererBloodlineWarlordReborn = FeatureConfigurator.New("OrcSorcererBloodlineWarlordReborn", "{C5391C64-C975-42A0-BEC9-CE952A1623BF}") // done
                .SetDisplayName(OrcSorcererBloodlineWarlordRebornDisplayName)
                .SetDescription(OrcSorcererBloodlineWarlordRebornDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.IncredibleHeftFeature.ToString()).Icon)
                .AddFacts([OrcSorcererBloodlineWarlordRebornTransformation])
                .AddAbilityResources(resource: OrcSorcererBloodlineWarlordRebornTransformationResource, restoreAmount: true)
                .AddDamageResistancePhysical(isStackable: true, value: ContextValues.Constant(5))
                .AddEnergyDamageImmunity(Kingmaker.Enums.Damage.DamageEnergyType.Fire, false)
                .SetIsClassFeature(true)
                .Configure();

            var OrcSorcererBloodlineSpells3 = FeatureConfigurator.New("OrcSorcererBloodlineSpells3", "{2DFD5453-A2E4-4521-8A3F-FD67F872B659}")
                .SetDisplayName(spell3.Get().m_DisplayName)
                .SetDescription(OrcSorcererBloodlineSpellsDescription)
                .SetIcon(spell3.Get().Icon)
                .SetIsClassFeature(true)
                .AddComponent<AddKnownSpell>(c =>
                {
                    c.m_CharacterClass = sorcererclass;
                    c.m_Spell = spell3;
                    c.SpellLevel = 1;
                })
                .Configure();

            var OrcSorcererBloodlineSpells5 = FeatureConfigurator.New("OrcSorcererBloodlineSpells5", "{64163882-D75A-4294-A779-FDBA2A5B3062}")
                .SetDisplayName(spell5.Get().m_DisplayName)
                .SetDescription(OrcSorcererBloodlineSpellsDescription)
                .SetIcon(spell5.Get().Icon)
                .SetIsClassFeature(true)
                .AddComponent<AddKnownSpell>(c =>
                {
                    c.m_CharacterClass = sorcererclass;
                    c.m_Spell = spell5;
                    c.SpellLevel = 2;
                })
                .Configure();

            var OrcSorcererBloodlineSpells7 = FeatureConfigurator.New("OrcSorcererBloodlineSpells7", "{3CC1AF27-E4B5-45A3-B60A-30DCE27E27B8}")
                .SetDisplayName(spell7.Get().m_DisplayName)
                .SetDescription(OrcSorcererBloodlineSpellsDescription)
                .SetIcon(spell7.Get().Icon)
                .SetIsClassFeature(true)
                .AddComponent<AddKnownSpell>(c =>
                {
                    c.m_CharacterClass = sorcererclass;
                    c.m_Spell = spell7;
                    c.SpellLevel = 3;
                })
                .Configure();

            var OrcSorcererBloodlineSpells9 = FeatureConfigurator.New("OrcSorcererBloodlineSpells9", "{02230C4F-DD9A-4A76-9BDB-FA0FB8D2BC3C}")
                .SetDisplayName(spell9.Get().m_DisplayName)
                .SetDescription(OrcSorcererBloodlineSpellsDescription)
                .SetIcon(spell9.Get().Icon)
                .SetIsClassFeature(true)
                .AddComponent<AddKnownSpell>(c =>
                {
                    c.m_CharacterClass = sorcererclass;
                    c.m_Spell = spell9;
                    c.SpellLevel = 4;
                })
                .Configure();

            var OrcSorcererBloodlineSpells11 = FeatureConfigurator.New("OrcSorcererBloodlineSpells11", "{9EE877EB-4E97-4E56-9C7D-0ABF47667722}")
                .SetDisplayName(spell11.Get().m_DisplayName)
                .SetDescription(OrcSorcererBloodlineSpellsDescription)
                .SetIcon(spell11.Get().Icon)
                .SetIsClassFeature(true)
                .AddComponent<AddKnownSpell>(c =>
                {
                    c.m_CharacterClass = sorcererclass;
                    c.m_Spell = spell11;
                    c.SpellLevel = 5;
                })
                .Configure();

            var OrcSorcererBloodlineSpells13 = FeatureConfigurator.New("OrcSorcererBloodlineSpells13", "{374F1AB9-F526-4667-83B5-E45AF8946608}")
                .SetDisplayName(spell13.Get().m_DisplayName)
                .SetDescription(OrcSorcererBloodlineSpellsDescription)
                .SetIcon(spell13.Get().Icon)
                .SetIsClassFeature(true)
                .AddComponent<AddKnownSpell>(c =>
                {
                    c.m_CharacterClass = sorcererclass;
                    c.m_Spell = spell13;
                    c.SpellLevel = 6;
                })
                .Configure();

            var OrcSorcererBloodlineSpells15 = FeatureConfigurator.New("OrcSorcererBloodlineSpells15", "{8587DF35-D014-43A6-A95B-830BC1F79FDF}")
                .SetDisplayName(spell15.Get().m_DisplayName)
                .SetDescription(OrcSorcererBloodlineSpellsDescription)
                .SetIcon(spell15.Get().Icon)
                .SetIsClassFeature(true)
                .AddComponent<AddKnownSpell>(c =>
                {
                    c.m_CharacterClass = sorcererclass;
                    c.m_Spell = spell15;
                    c.SpellLevel = 7;
                })
                .Configure();

            var OrcSorcererBloodlineSpells17 = FeatureConfigurator.New("OrcSorcererBloodlineSpells17", "{42890B1F-9C7D-4F30-BC3A-5DAC4D70D4D0}")
                .SetDisplayName(spell17.Get().m_DisplayName)
                .SetDescription(OrcSorcererBloodlineSpellsDescription)
                .SetIcon(spell17.Get().Icon)
                .SetIsClassFeature(true)
                .AddComponent<AddKnownSpell>(c =>
                {
                    c.m_CharacterClass = sorcererclass;
                    c.m_Spell = spell17;
                    c.SpellLevel = 8;
                })
                .Configure();

            var OrcSorcererBloodlineSpells19 = FeatureConfigurator.New("OrcSorcererBloodlineSpells19", "{9E1A6C64-AD6F-49DA-98E1-D17E764B9D85}")
                .SetDisplayName(spell19.Get().m_DisplayName)
                .SetDescription(OrcSorcererBloodlineSpellsDescription)
                .SetIcon(spell19.Get().Icon)
                .SetIsClassFeature(true)
                .AddComponent<AddKnownSpell>(c =>
                {
                    c.m_CharacterClass = sorcererclass;
                    c.m_Spell = spell19;
                    c.SpellLevel = 9;
                })
                .Configure();

            var OrcSorcererBloodlineFeats = FeatureSelectionConfigurator.New("OrcSorcererBloodlineFeats", "{BBE7DA35-41A2-4E23-820B-713215CCDAEC}")
                .SetDisplayName(OrcSorcererBloodlineFeatsDisplayName)
                .SetDescription(OrcSorcererBloodlineFeatsDescription)
                .SetRanks(1)
                .SetIsClassFeature(true)
                .SetHideInUI(true)
                .SetHideNotAvailibleInUI(true)
                .AddToAllFeatures(
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.Diehard.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.Endurance.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.GreatFortitude.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.IntimidatingProwess.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.PowerAttackFeature.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.Toughness.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintFeatureReference>(FeatureRefs.EmpowerSpellFeat.ToString())
                    )
                .Configure();

            var SorcererOrcBloodline = ProgressionConfigurator.New(OrcSorcererBloodlineName, OrcSorcererBloodlineGuid)
                .SetDisplayName(OrcSorcererBloodlineDisplayName)
                .SetDescription(OrcSorcererBloodlineDescription)
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
                .AddToLevelEntries(1, OrcSorcererBloodlineTouchOfRage, OrcSorcererBloodlineArcanaProgression, OrcSorcererBloodlineClassSkill, OrcSorcererBloodlineRequisiteFeature, BloodlineRequisiteFeature)
                .AddToLevelEntries(3, OrcSorcererBloodlineSpells3, OrcSorcererBloodlineFearless1)
                .AddToLevelEntries(5, OrcSorcererBloodlineSpells5)
                .AddToLevelEntries(7, OrcSorcererBloodlineSpells7)
                .AddToLevelEntries(9, OrcSorcererBloodlineSpells9, OrcSorcererBloodlineFearless2, OrcSorcererBloodlineStrengthOfTheBeastProgression)
                .AddToLevelEntries(11, OrcSorcererBloodlineSpells11)
                .AddToLevelEntries(13, OrcSorcererBloodlineSpells13)
                .AddToLevelEntries(15, OrcSorcererBloodlineSpells15, OrcSorcererBloodlinePowerOfGiants)
                .AddToLevelEntries(17, OrcSorcererBloodlineSpells17)
                .AddToLevelEntries(19, OrcSorcererBloodlineSpells19)
                .AddToLevelEntries(20, OrcSorcererBloodlineWarlordReborn)
                .AddPrerequisiteNoFeature(BloodlineRequisiteFeature)
                .AddPrerequisiteNoFeature(OrcSorcererBloodlineRequisiteFeature)
                .Configure();

            var CrossbloodedOrcBloodline = ProgressionConfigurator.New(OrcCrossbloodedBloodlineName, OrcCrossbloodedBloodlineGuid)
                .SetDisplayName(OrcSorcererBloodlineDisplayName)
                .SetDescription(OrcSorcererBloodlineDescription)
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
                .AddToLevelEntries(1, OrcSorcererBloodlineArcanaProgression, OrcSorcererBloodlineClassSkill, OrcSorcererBloodlineRequisiteFeature, BloodlineRequisiteFeature)
                .AddToLevelEntries(3, OrcSorcererBloodlineSpells3)
                .AddToLevelEntries(5, OrcSorcererBloodlineSpells5)
                .AddToLevelEntries(7, OrcSorcererBloodlineSpells7)
                .AddToLevelEntries(9, OrcSorcererBloodlineSpells9)
                .AddToLevelEntries(11, OrcSorcererBloodlineSpells11)
                .AddToLevelEntries(13, OrcSorcererBloodlineSpells13)
                .AddToLevelEntries(15, OrcSorcererBloodlineSpells15)
                .AddToLevelEntries(17, OrcSorcererBloodlineSpells17)
                .AddToLevelEntries(19, OrcSorcererBloodlineSpells19)
                .Configure();

            var SeekerOrcBloodline = ProgressionConfigurator.New(OrcSeekerBloodlineName, OrcSeekerBloodlineGuid)
                .SetDisplayName(OrcSorcererBloodlineDisplayName)
                .SetDescription(OrcSorcererBloodlineDescription)
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
                .AddToLevelEntries(1, OrcSorcererBloodlineTouchOfRage, OrcSorcererBloodlineArcanaProgression, OrcSorcererBloodlineClassSkill, OrcSorcererBloodlineRequisiteFeature, BloodlineRequisiteFeature)
                .AddToLevelEntries(3, OrcSorcererBloodlineSpells3)
                .AddToLevelEntries(5, OrcSorcererBloodlineSpells5)
                .AddToLevelEntries(7, OrcSorcererBloodlineSpells7)
                .AddToLevelEntries(9, OrcSorcererBloodlineSpells9, OrcSorcererBloodlineStrengthOfTheBeastProgression)
                .AddToLevelEntries(11, OrcSorcererBloodlineSpells11)
                .AddToLevelEntries(13, OrcSorcererBloodlineSpells13)
                .AddToLevelEntries(15, OrcSorcererBloodlineSpells15)
                .AddToLevelEntries(17, OrcSorcererBloodlineSpells17)
                .AddToLevelEntries(19, OrcSorcererBloodlineSpells19)
                .AddToLevelEntries(20, OrcSorcererBloodlineWarlordReborn)
                .AddPrerequisiteNoFeature(BloodlineRequisiteFeature)
                .AddPrerequisiteNoFeature(OrcSorcererBloodlineRequisiteFeature)
                .Configure();

            BloodlineTools.RegisterSorcererFeatSelection(OrcSorcererBloodlineFeats, SorcererOrcBloodline);
            BloodlineTools.RegisterSorcererBloodline(SorcererOrcBloodline);
            BloodlineTools.RegisterCrossbloodedBloodline(CrossbloodedOrcBloodline);
            BloodlineTools.RegisterSeekerBloodline(SeekerOrcBloodline);
        }
    }
}
