using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using EbonsContentMod.Components;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Mechanics.Components;
using Kingmaker.UnitLogic.Mechanics.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;

namespace EbonsContentMod.Spells
{
    internal class BestowGraceOfTheChampion
    {
        internal static void Configure()
        {
            // Lay on Hands Resource

            var lohResource = AbilityResourceConfigurator.New("BGOTCLOHResource", "{24F8B4F4-7E6A-42E8-A4B1-03C9A4DBB817}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1)
                )
                .SetMax(1)
                .Configure();

            // Smite Resource

            var smiteResource = AbilityResourceConfigurator.New("BGOTCSmiteResource", "{0398829B-A15E-4E84-B05F-3B411D4B9B39}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1)
                )
                .SetMax(1)
                .Configure();

            // Smite Ability

            var smiteAbility = AbilityConfigurator.New("BGOTCSmiteAbility", "{22A28241-F60D-4258-B336-3E16391164FC}")
                .CopyFrom(AbilityRefs.SmiteEvilAbility, c => c is not ContextCalculateSharedValue && c is not ContextRankConfig && c is not AbilityResourceLogic)
                .AddContextCalculateSharedValue(valueType: AbilitySharedValue.DamageBonus, value: new ContextDiceValue()
                {
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
                        ValueRank = AbilityRankType.DamageBonus,
                        ValueShared = AbilitySharedValue.DamageBonus
                    }
                })
                .AddContextCalculateSharedValue(valueType: AbilitySharedValue.StatBonus, value: new ContextDiceValue()
                {
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
                        ValueShared = AbilitySharedValue.StatBonus
                    }
                })
                .AddContextRankConfig(ContextRankConfigs.StatBonus(StatType.Charisma, min: 0))
                //.AddContextRankConfig(ContextRankConfigs.CasterLevel(type: AbilityRankType.DamageBonus).WithDiv2Progression())
                .AddContextRankConfig(ContextRankConfigs.CasterLevel(type: AbilityRankType.DamageBonus))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: smiteResource)
                .Configure();

            // Smite Feature

            /*var smiteFeature = FeatureConfigurator.New("BGOTCSmiteFeature", "{F00BEC3A-F165-436F-82B0-C8DCC34E17D4}")
                .AddHideFeatureInInspect()
                .SetHideInCharacterSheetAndLevelUp()
                .SetHideInUI()
                .AddFacts([smiteAbility])
                .AddAbilityResources(resource: smiteResource, restoreAmount: true)
                .Configure();*/

            // Lay On Hands Ability

            var lohSelfAbility =
                AbilityConfigurator.New(
                    "BGOTCLayOnHandsSelf",
                    "{084D0461-E481-479C-9897-7362701D003E}")
                .CopyFrom(AbilityRefs.LayOnHandsSelf, c => c is not AbilityResourceLogic && c is not ContextRankConfig && c is not AbilityUseOnRest)
                .AddContextRankConfig(
                    ContextRankConfigs
                        .CasterLevel()
                        .WithDiv2Progression())
                .AddAbilityResourceLogic(
                    1,
                    isSpendResource: true,
                    requiredResource: lohResource)
                .Configure();

            var lohOthersAbility =
                AbilityConfigurator.New(
                    "BGOTCLayOnHandsOthers",
                    "{C3F664AA-7495-4411-BDA9-6670D744F4AC}")
                .CopyFrom(AbilityRefs.LayOnHandsOthers, c => c is not AbilityResourceLogic && c is not ContextRankConfig && c is not AbilityUseOnRest)
                .AddContextRankConfig(
                    ContextRankConfigs
                        .CasterLevel()
                        .WithDiv2Progression())
                .AddAbilityResourceLogic(
                    1,
                    isSpendResource: true,
                    requiredResource: lohResource)
                .Configure();

            // Ability Granting Feature

            var grantingFeature =
                FeatureConfigurator.New(
                    "BGOTCGrantedAbilitiesFeature",
                    "{5C069EDD-5ACA-4FCA-A21F-ED84766964A5}")
                .CopyFrom(FeatureRefs.LayOnHandsFeature, c => c is not ReplaceCasterLevelOfAbility && c is not AddFacts)
                .AddFacts(
                [
                    smiteAbility,
                    lohSelfAbility,
                    lohOthersAbility
                ])
                .AddComponent<ReplaceCasterLevelFromBuffCaster>(c =>
                {
                    c.m_SourceBuff =
                        BuffRefs.BestowGraceOfTheChampionBuff.Reference.Get().ToReference<BlueprintBuffReference>();

                    c.m_Spells =
                    [
                        smiteAbility.ToReference<BlueprintAbilityReference>(),
                        lohSelfAbility.ToReference<BlueprintAbilityReference>(),
                        lohOthersAbility.ToReference<BlueprintAbilityReference>()
                    ];

                    c.Divisor = 2;
                })
                /*.AddAbilityResources(
                    resource: lohResource,
                    restoreAmount: true)*/
                .Configure();

            // BGOTC Buff

            BuffConfigurator.For(BuffRefs.BestowGraceOfTheChampionBuff)
                .AddFacts([grantingFeature])
                .AddAbilityResources(resource: smiteResource, restoreAmount: true)
                .AddAbilityResources(resource: lohResource, restoreAmount: true)
                .Configure();

            
        }
    }

    /*[HarmonyPatch(
        typeof(ContextCalculateSharedValue),
        nameof(ContextCalculateSharedValue.Calculate))]
    internal static class BestowGraceSmiteDamageBonusPatch
    {
        private const string SmiteAbilityGuid =
            "22A28241-F60D-4258-B336-3E16391164FC";

        [HarmonyPrefix]
        private static bool Prefix(
            ContextCalculateSharedValue __instance,
            MechanicsContext context,
            ref int __result)
        {
            // Only replace the damage-bonus calculation on our custom Bestow Grace Smite.
            if (__instance.ValueType
                != AbilitySharedValue.DamageBonus)
            {
                return true;
            }

            BlueprintAbility owner =
                __instance.OwnerBlueprint
                    as BlueprintAbility;

            if (owner == null
                || owner.AssetGuid
                    != BlueprintTools
                        .GetBlueprint<BlueprintAbility>(
                            SmiteAbilityGuid)
                        .AssetGuid)
            {
                return true;
            }

            Buff bestowGraceBuff =
                context.MaybeCaster?
                    .GetFact(BuffRefs.BestowGraceOfTheChampionBuff.Reference.Get()) as Buff;

            if (bestowGraceBuff?.Context == null)
            {
                // Something unexpected happened; fall back
                // to the normal shared-value calculation.
                return true;
            }

            int casterLevel =
                bestowGraceBuff.Context.Params.CasterLevel;

            __result =
                casterLevel / 2;

            return false;
        }
    }*/
}
