using BlueprintCore.Blueprints.Configurators.Classes;
using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using BlueprintCore.Utils.Types;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints.Classes.Prerequisites;
using HarmonyLib;

namespace EbonsContentMod.Utilities
{
    internal class RaceOddsAndEndsFixerizer
    {
        public static void FixRace(BlueprintRace race)
        {
            // Destiny Beyond Birth

            FeatureConfigurator.For(FeatureRefs.DestinyBeyondBirthMythicFeat)
                .OnConfigure(bp =>
                {
                    var prerequisite = bp.GetComponent<PrerequisiteFeaturesFromList>();

                    if (prerequisite != null)
                    {
                        prerequisite.m_Features = prerequisite.m_Features.AppendToArray(
                            race.ToReference<BlueprintFeatureReference>()
                        );
                    }
                })
                .Configure();

            var Destiny = BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(FeatureRefs.DestinyBeyondBirthMythicFeat.ToString());


            var statBonuses = race.GetComponents<AddStatBonus>();

            var raceConfig = RaceConfigurator.For(race);

            foreach (AddStatBonus statBonus in statBonuses)
            {
                if (statBonus.Value < 0)
                {
                    raceConfig
                        .AddComponent<AddStatBonusIfHasFact>(c =>
                        {
                            var valueToAdd = 0 - statBonus.Value;

                            c.m_CheckedFacts = [
                                Destiny
                                ];
                            c.Descriptor = ModifierDescriptor.Racial;
                            c.Stat = statBonus.Stat;
                            c.Value = ContextValues.Constant(valueToAdd);
                        });
                }
            }

            raceConfig
                .AddRecalculateOnFactsChange([Destiny])
                .Configure();
        }

        public static void FixFeat(BlueprintFeature feat)
        {
            var statBonuses = feat.GetComponents<AddStatBonus>();

            var featConfig = FeatureConfigurator.For(feat);

            foreach (AddStatBonus statBonus in statBonuses)
            {
                if (statBonus.Value < 0)
                {
                    featConfig
                        .AddComponent<AddStatBonusIfHasFact>(c =>
                        {
                            var valueToAdd = 0 - statBonus.Value;

                            c.m_CheckedFacts = [
                                BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>(FeatureRefs.DestinyBeyondBirthMythicFeat.ToString())
                                ];
                            c.Descriptor = ModifierDescriptor.Racial;
                            c.Stat = statBonus.Stat;
                            c.Value = ContextValues.Constant(valueToAdd);
                        });
                }
            }

            featConfig.Configure();
        }

    }
}
