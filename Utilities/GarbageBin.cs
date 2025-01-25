using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints.Root;
using Kingmaker.Blueprints;
using Kingmaker.Enums;
using Kingmaker.Blueprints.Classes;
using Kingmaker.EntitySystem.Stats;
using System;
using System.Collections.Generic;
using TabletopTweaks.Core.Utilities;
using EbonsContentMod.Utilities;
using UnityEngine;
using Kingmaker.ResourceLinks;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Enums.Damage;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using EbonsContentMod.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;
using static Kingmaker.Visual.Animation.Kingmaker.Actions.UnitAnimationActionCastSpell;
using static TabletopTweaks.Core.MechanicsChanges.MetamagicExtention;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Utils.Types;
using Kingmaker.Craft;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using static EbonsContentMod.UnitParts.UnitPartTouchCharges;
using static Kingmaker.Kingdom.Settlements.SettlementGridTopology;
using Kingmaker.Utility;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using EbonsContentMod.Components;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Visual.CharacterSystem;

namespace EbonsContentMod.Utilities
{
    internal class GarbageBin
    {
        // Not used, leaving it for potential later use
        /*private static EquipmentEntity CloneWithNewColors(BlueprintRace race, EquipmentEntity ee, List<Color> colors)
        {
            var ramps = CreateRampsFromColors(race, colors);
            var guid = RaceFunctions.GetEntityNewGuidFromName(ee.ToString());

            if (guid == "Error")
            {
                var newGuid = BlueprintGuid.NewGuid();
                string newline = "\"" + ee.name + "\"" + " => " + "\"" + newGuid + "\"" + ",";
                File.WriteAllLines(ToolOutputFilePath + "\\" + "RaceRecolorizer" + ee.name + ".txt", [newline]);
                return null;
            }

            EquipmentEntity ret = (EquipmentEntity)ScriptableObject.CreateInstance(typeof(EquipmentEntity));
            ret.BakedTextures = Helpers.CreateCopy<CharacterBakedTextures>(ee.BakedTextures, null);
            ret.BodyParts = Helpers.CreateCopy<List<BodyPart>>(ee.BodyParts, null);
            ret.CantBeHiddenByDollRoom = ee.CantBeHiddenByDollRoom;
            ret.ColorPresets = ee.ColorPresets;
            ret.IsExportEnabled = ee.IsExportEnabled;
            ret.Layer = ee.Layer;
            ret.PrimaryRamps = ramps;
            ret.m_BonesByName = ee.m_BonesByName;
            ret.m_DlcReward = ee.m_DlcReward;
            ret.m_HideBodyParts = ee.m_HideBodyParts;
            ret.m_IsDirty = ee.m_IsDirty;
            ret.m_PrimaryRamps = ramps;
            ret.m_SecondaryRamps = ee.m_SecondaryRamps;
            ret.m_SpecialPrimaryRamps = ee.m_SpecialPrimaryRamps;
            ret.m_SpecialSecondaryRamps = ee.m_SpecialSecondaryRamps;
            ret.OutfitParts = Helpers.CreateCopy<List<EquipmentEntity.OutfitPart>>(ee.OutfitParts, null);
            ret.PrimaryColorsAvailableForPlayer = ee.PrimaryColorsAvailableForPlayer;
            ret.PrimaryColorsProfile = new CharacterColorsProfile();
            ret.PrimaryColorsProfile.Ramps = ramps;
            ret.PrimaryColorsProfile.RampDlcLocks = ee.PrimaryColorsProfile.RampDlcLocks;
            ret.PrimaryColorsProfile.SpecialRamps = ee.PrimaryColorsProfile.SpecialRamps;
            ret.SecondaryColorsProfile = ee.SecondaryColorsProfile;
            ret.SecondaryColorsAvailableForPlayer = ee.SecondaryColorsAvailableForPlayer;
            ret.ShowLowerMaterials = ee.ShowLowerMaterials;
            ret.SkeletonModifiers = ee.SkeletonModifiers;
            ret.name = guid;
            return ret;
        }*/

        // How I was trying to copy components in spells to resolve incorrect owner blueprints before just implementing the patch from PretigePlus

        /* 
         var SpellCraftInfoComponent = spellability.GetComponent<CraftInfoComponent>();
            var SpellAbilityDeliverProjectile = spellability.GetComponent<AbilityDeliverProjectile>();
            var SpellAbilityDeliverDelay = spellability.GetComponent<AbilityDeliverDelay>();
            var SpellAbilityDeliverTouch = spellability.GetComponent<AbilityDeliverTouch>();
            var SpellContextRankConfig = spellability.GetComponent<ContextRankConfig>();
        
        if (SpellCraftInfoComponent != null)
                {
                    var fixedSpell = AbilityConfigurator.For(NewSpell)
                        .AddComponent<CraftInfoComponent>(c =>
                        {
                            c.AOEType = SpellCraftInfoComponent.AOEType;
                            c.SpellType = SpellCraftInfoComponent.SpellType;
                            c.SavingThrow = SpellCraftInfoComponent.SavingThrow;
                            c.m_Flags = SpellCraftInfoComponent.m_Flags;
                            c.m_PrototypeLink = SpellCraftInfoComponent.m_PrototypeLink;
                        })
                        .Configure();
                }

                if (SpellAbilityDeliverProjectile != null)
                {
                    var fixedSpell = AbilityConfigurator.For(NewSpell)
                        .AddComponent<AbilityDeliverProjectile>(c =>
                        {
                            c.AttackRollBonusStat = SpellAbilityDeliverProjectile.AttackRollBonusStat;
                            c.DelayBetweenProjectiles = SpellAbilityDeliverProjectile.DelayBetweenProjectiles;
                            c.IsHandOfTheApprentice = SpellAbilityDeliverProjectile.IsHandOfTheApprentice;
                            c.MaxProjectilesCountRank = SpellAbilityDeliverProjectile.MaxProjectilesCountRank;
                            c.m_ControlledProjectileHolderBuff = SpellAbilityDeliverProjectile.m_ControlledProjectileHolderBuff;
                            c.m_Flags = SpellAbilityDeliverProjectile.m_Flags;
                            c.m_HasIsAllyEffectRunConditions = SpellAbilityDeliverProjectile.m_HasIsAllyEffectRunConditions;
                            c.m_Length = SpellAbilityDeliverProjectile.m_Length;
                            c.m_LineWidth = SpellAbilityDeliverProjectile.m_LineWidth;
                            c.m_Projectiles = SpellAbilityDeliverProjectile.m_Projectiles;
                            c.m_PrototypeLink = SpellAbilityDeliverProjectile.m_PrototypeLink;
                            c.m_Weapon = SpellAbilityDeliverProjectile.m_Weapon;
                            c.NeedAttackRoll = SpellAbilityDeliverProjectile.ReplaceAttackRollBonusStat;
                            c.Type = SpellAbilityDeliverProjectile.Type;
                            c.UseCastPositionInsteadCasterPosition = SpellAbilityDeliverProjectile.UseCastPositionInsteadCasterPosition;
                            c.UseMaxProjectilesCount = SpellAbilityDeliverProjectile.UseMaxProjectilesCount;
                        })
                        .Configure();
                }

                if (SpellAbilityDeliverDelay != null)
                {
                    var fixedSpell = AbilityConfigurator.For(NewSpell)
                        .AddComponent<AbilityDeliverDelay>(c =>
                        {
                            c.DelaySeconds = SpellAbilityDeliverDelay.DelaySeconds;
                            c.m_Flags = SpellAbilityDeliverDelay.m_Flags;
                            c.m_HasIsAllyEffectRunConditions = SpellAbilityDeliverDelay.m_HasIsAllyEffectRunConditions;
                            c.m_PrototypeLink = SpellAbilityDeliverDelay.m_PrototypeLink;
                            
                        })
                        .Configure();
                }

                if (SpellAbilityDeliverTouch != null)
                {
                    var fixedSpell = AbilityConfigurator.For(NewSpell)
                        .AddComponent<AbilityDeliverTouch>(c =>
                        {
                            c.m_Flags = SpellAbilityDeliverTouch.m_Flags;
                            c.m_HasIsAllyEffectRunConditions = SpellAbilityDeliverTouch.m_HasIsAllyEffectRunConditions;
                            c.m_PrototypeLink = SpellAbilityDeliverTouch.m_PrototypeLink;
                            c.m_TouchWeapon = SpellAbilityDeliverTouch.m_TouchWeapon;
                        })
                        .Configure();
                }

                if (SpellContextRankConfig != null)
                {
                    SpellContextRankConfig.OwnerBlueprint = NewSpell;
                    
                    var fixedSpell = AbilityConfigurator.For(NewSpell)
                        .AddComponent(SpellContextRankConfig)
                        .Configure();
                }
        
        var VariantCraftInfoComponent = variantability.GetComponent<CraftInfoComponent>();
                    var VariantAbilityDeliverProjectile = variantability.GetComponent<AbilityDeliverProjectile>();
                    var VariantAbilityDeliverDelay = spellability.GetComponent<AbilityDeliverDelay>();
                    var VariantAbilityDeliverTouch = spellability.GetComponent<AbilityDeliverTouch>();
                    var VariantContextRankConfig = spellability.GetComponent<ContextRankConfig>();

        if (VariantCraftInfoComponent != null)
                    {
                        var fixedSpell = AbilityConfigurator.For(NewSpellVariant)
                            .AddComponent<CraftInfoComponent>(c =>
                            {
                                c.AOEType = VariantCraftInfoComponent.AOEType;
                                c.SpellType = VariantCraftInfoComponent.SpellType;
                                c.SavingThrow = VariantCraftInfoComponent.SavingThrow;
                                c.m_Flags = VariantCraftInfoComponent.m_Flags;
                                c.m_PrototypeLink = VariantCraftInfoComponent.m_PrototypeLink;
                            })
                            .Configure();
                    }

                    if (VariantAbilityDeliverProjectile != null)
                    {
                        var fixedSpell = AbilityConfigurator.For(NewSpellVariant)
                        .AddComponent<AbilityDeliverProjectile>(c =>
                        {
                            c.AttackRollBonusStat = VariantAbilityDeliverProjectile.AttackRollBonusStat;
                            c.DelayBetweenProjectiles = VariantAbilityDeliverProjectile.DelayBetweenProjectiles;
                            c.IsHandOfTheApprentice = VariantAbilityDeliverProjectile.IsHandOfTheApprentice;
                            c.MaxProjectilesCountRank = VariantAbilityDeliverProjectile.MaxProjectilesCountRank;
                            c.m_ControlledProjectileHolderBuff = VariantAbilityDeliverProjectile.m_ControlledProjectileHolderBuff;
                            c.m_Flags = VariantAbilityDeliverProjectile.m_Flags;
                            c.m_HasIsAllyEffectRunConditions = VariantAbilityDeliverProjectile.m_HasIsAllyEffectRunConditions;
                            c.m_Length = VariantAbilityDeliverProjectile.m_Length;
                            c.m_LineWidth = VariantAbilityDeliverProjectile.m_LineWidth;
                            c.m_Projectiles = VariantAbilityDeliverProjectile.m_Projectiles;
                            c.m_PrototypeLink = VariantAbilityDeliverProjectile.m_PrototypeLink;
                            c.m_Weapon = VariantAbilityDeliverProjectile.m_Weapon;
                            c.NeedAttackRoll = VariantAbilityDeliverProjectile.ReplaceAttackRollBonusStat;
                            c.Type = VariantAbilityDeliverProjectile.Type;
                            c.UseCastPositionInsteadCasterPosition = VariantAbilityDeliverProjectile.UseCastPositionInsteadCasterPosition;
                            c.UseMaxProjectilesCount = VariantAbilityDeliverProjectile.UseMaxProjectilesCount;
                        })
                        .Configure();
                    }

                    if (VariantAbilityDeliverDelay != null)
                    {
                        var fixedSpell = AbilityConfigurator.For(NewSpellVariant)
                            .AddComponent<AbilityDeliverDelay>(c =>
                            {
                                c.DelaySeconds = VariantAbilityDeliverDelay.DelaySeconds;
                                c.m_Flags = VariantAbilityDeliverDelay.m_Flags;
                                c.m_HasIsAllyEffectRunConditions = VariantAbilityDeliverDelay.m_HasIsAllyEffectRunConditions;
                                c.m_PrototypeLink = VariantAbilityDeliverDelay.m_PrototypeLink;

                            })
                            .Configure();
                    }

                    if (VariantAbilityDeliverTouch != null)
                    {
                        var fixedSpell = AbilityConfigurator.For(NewSpellVariant)
                            .AddComponent<AbilityDeliverTouch>(c =>
                            {
                                c.m_Flags = VariantAbilityDeliverTouch.m_Flags;
                                c.m_HasIsAllyEffectRunConditions = VariantAbilityDeliverTouch.m_HasIsAllyEffectRunConditions;
                                c.m_PrototypeLink = VariantAbilityDeliverTouch.m_PrototypeLink;
                                c.m_TouchWeapon = VariantAbilityDeliverTouch.m_TouchWeapon;
                            })
                            .Configure();
                    }

                    if (VariantContextRankConfig != null)
                    {
                        VariantContextRankConfig.OwnerBlueprint = NewSpellVariant;

                        var fixedSpell = AbilityConfigurator.For(NewSpellVariant)
                            .AddComponent(VariantContextRankConfig)
                            .Configure();
                    }
         
         */

        internal const string ObscuringMistDisplayName = "Undine.ObscuringMist.Name";
        private static readonly string ObscuringMistDescription = "Undine.ObscuringMist.Description";

        internal static void Configure()
        {
            var ObscuringMistBuff = BuffConfigurator.New("UndineObscuringMistBuff", "{001754CA-D3D5-499E-8EB2-64F995704F7D}")
                .SetDisplayName(ObscuringMistDisplayName)
                .SetDescription(ObscuringMistDescription)
                .SetIcon(FeatureRefs.CloudInfusion.Reference.Get().Icon)
                .AddConcealment(concealment: Concealment.Partial, descriptor: ConcealmentDescriptor.Fog)
                .AddConcealment(checkDistance: true, distanceGreater: 5.Feet(), concealment: Concealment.Total, descriptor: ConcealmentDescriptor.Fog)
                /*.AddComponent<AddOutgoingConcealment>(c =>
                {
                    c.Descriptor = ConcealmentDescriptor.Fog;
                    c.Concealment = Concealment.Partial;
                    c.CheckDistance = false;
                    c.CheckWeaponRangeType = false;
                    c.DistanceGreater = 0.Feet();
                    c.OnlyForAttacks = false;
                    c.RangeType = WeaponRangeType.Melee;
                })
                .AddComponent<AddOutgoingConcealment>(c =>
                {
                    c.Descriptor = ConcealmentDescriptor.Fog;
                    c.Concealment = Concealment.Total;
                    c.CheckDistance = true;
                    c.CheckWeaponRangeType = false;
                    c.DistanceGreater = 5.Feet();
                    c.OnlyForAttacks = false;
                    c.RangeType = WeaponRangeType.Melee;
                })*/
                .Configure();

            var ObscuringMistArea = AbilityAreaEffectConfigurator.New("UdineObscuringMistArea", "{967C4904-8D7E-42E6-9726-EB15D16BD8E6}")
                .CopyFrom(AbilityAreaEffectRefs.MindFogArea)
                .SetFx(BlueprintTools.GetBlueprint<BlueprintAbilityAreaEffect>(AbilityAreaEffectRefs.StinkingCloudArea.ToString()).Fx)
                .SetSize(20.Feet())
                .SetSpellResistance(false)
                .AddAbilityAreaEffectBuff(buff: ObscuringMistBuff)
                .Configure();

            var ObscuringMist = AbilityConfigurator.NewSpell("UndineObscuringMist", "{20576BE2-2C0A-4A1A-A13D-AB5DF4FD3C80}", SpellSchool.Conjuration, false)
                .CopyFrom(AbilityRefs.StinkingCloud, c => c is not (SpellListComponent or CraftInfoComponent or AbilityEffectRunAction or SpellListComponent or ContextRankConfig))
                .SetDisplayName(ObscuringMistDisplayName)
                .SetDescription(ObscuringMistDescription)
                .SetIcon(FeatureRefs.CloudInfusion.Reference.Get().Icon)
                .SetSpellResistance(false)
                .AddAbilityEffectRunAction(ActionsBuilder.New().SpawnAreaEffect(ObscuringMistArea, new ContextDurationValue()
                {
                    Rate = DurationRate.Rounds,
                    DiceType = DiceType.Zero,
                    DiceCountValue = new ContextValue()
                    {
                        ValueType = ContextValueType.Simple,
                        Value = 0,
                        ValueRank = AbilityRankType.Default,
                        ValueShared = AbilitySharedValue.Damage,
                        Property = UnitProperty.None
                    },
                    BonusValue = new ContextValue()
                    {
                        ValueType = ContextValueType.Rank,
                        Value = 0,
                        ValueRank = AbilityRankType.Default,
                        ValueShared = AbilitySharedValue.Damage,
                        Property = UnitProperty.None
                    },
                    m_IsExtendable = true,
                }, false).Build())
                .AddComponent<ContextRankConfig>(c =>
                {
                    c.m_Type = AbilityRankType.Default;
                    c.m_BaseValueType = ContextRankBaseValueType.CharacterLevel;
                    c.m_Stat = StatType.Unknown;
                    c.m_SpecificModifier = ModifierDescriptor.None;
                    c.m_Progression = ContextRankProgression.AsIs;
                    c.m_StartLevel = 0;
                    c.m_StepLevel = 0;
                    c.m_UseMax = true;
                    c.m_Max = 20;
                })
                //.AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var RightAmorphousLimbsEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "86127616283ae7741ae3e813904865cc" }, RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>() { new Color( // Pink-Purple
                RaceRecolorizer.GetColorsFromRGB(140f), // 140
                RaceRecolorizer.GetColorsFromRGB(0f),
                RaceRecolorizer.GetColorsFromRGB(90f) // 75
                )}), "{3F3D11E6-F9AD-4F36-BB3B-77B42AFA8DAE}", true, true,
                BodyPartsToRemove: new List<BodyPartType>() { BodyPartType.Eyes, BodyPartType.Head, BodyPartType.Ears, BodyPartType.Torso, BodyPartType.NeckTorso, BodyPartType.UpperLegs, BodyPartType.LowerLegs, BodyPartType.Feet });

            var LeftAmorphousLimbsEE = RaceRecolorizer.RecolorEELink(new EquipmentEntityLink() { AssetId = "7109791d63944254589b908564604c79" }, RaceRecolorizer.CreateRampsFromColorsSimple(new List<Color>() { new Color( // Pink-Purple
                RaceRecolorizer.GetColorsFromRGB(140f), // 140
                RaceRecolorizer.GetColorsFromRGB(0f),
                RaceRecolorizer.GetColorsFromRGB(90f) // 75
                )}), "{902C55AD-0678-4941-B881-02CDF179683D}", true, true,
                BodyPartsToRemove: new List<BodyPartType>() { BodyPartType.Eyes, BodyPartType.Head, BodyPartType.Ears, BodyPartType.Torso, BodyPartType.NeckTorso, BodyPartType.UpperLegs, BodyPartType.LowerLegs, BodyPartType.Feet });
        }
    }
}
