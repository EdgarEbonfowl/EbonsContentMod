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

namespace EbonsContentMod.Races
{
    internal class Undine
    {
        public static List<Color> RaceHeadColors =
        [
            new Color( // Light Water
                RaceRecolorizer.GetColorsFromRGB(110f),
                RaceRecolorizer.GetColorsFromRGB(143f),
                RaceRecolorizer.GetColorsFromRGB(202f)
                ),
            new Color( // Light Seafoam
                RaceRecolorizer.GetColorsFromRGB(134f),
                RaceRecolorizer.GetColorsFromRGB(162f),
                RaceRecolorizer.GetColorsFromRGB(164f)
                ),
            new Color( // Deep Water //
                RaceRecolorizer.GetColorsFromRGB(71f),
                RaceRecolorizer.GetColorsFromRGB(114f),
                RaceRecolorizer.GetColorsFromRGB(154f)
                )
        ];

        public static List<Color> RaceEyeColors =
        [
            new Color( // White
                RaceRecolorizer.GetColorsFromRGB(182f),
                RaceRecolorizer.GetColorsFromRGB(182f),
                RaceRecolorizer.GetColorsFromRGB(182f)
                ),
            new Color( // Light Purple
                RaceRecolorizer.GetColorsFromRGB(190f),
                RaceRecolorizer.GetColorsFromRGB(150f),
                RaceRecolorizer.GetColorsFromRGB(205f)
                ),
            new Color( // Light Teal
                RaceRecolorizer.GetColorsFromRGB(143f),
                RaceRecolorizer.GetColorsFromRGB(220f),
                RaceRecolorizer.GetColorsFromRGB(203f)
                ),
            new Color( // Light Blue
                RaceRecolorizer.GetColorsFromRGB(144f),
                RaceRecolorizer.GetColorsFromRGB(160f),
                RaceRecolorizer.GetColorsFromRGB(236f)
                )
        ];

        public static List<Color> RaceHairColors =
        [
            new Color( // Dark blue
                RaceRecolorizer.GetColorsFromRGB(50f),
                RaceRecolorizer.GetColorsFromRGB(81f),
                RaceRecolorizer.GetColorsFromRGB(200f)
                ),
            new Color( // Very blue
                RaceRecolorizer.GetColorsFromRGB(68f),
                RaceRecolorizer.GetColorsFromRGB(106f),
                RaceRecolorizer.GetColorsFromRGB(200f)
                ),
            new Color( // Dark Purple
                RaceRecolorizer.GetColorsFromRGB(39f),
                RaceRecolorizer.GetColorsFromRGB(16f),
                RaceRecolorizer.GetColorsFromRGB(86f)
                ),
            new Color( // Teal Seafoam
                RaceRecolorizer.GetColorsFromRGB(116f),
                RaceRecolorizer.GetColorsFromRGB(196f),
                RaceRecolorizer.GetColorsFromRGB(213f)
                ),
            new Color( // Very Dark Blue
                RaceRecolorizer.GetColorsFromRGB(6f),
                RaceRecolorizer.GetColorsFromRGB(28f),
                RaceRecolorizer.GetColorsFromRGB(109f)
                ),
            new Color( // Medium Purple
                RaceRecolorizer.GetColorsFromRGB(109f),
                RaceRecolorizer.GetColorsFromRGB(80f),
                RaceRecolorizer.GetColorsFromRGB(166f)
                ),
            new Color( // Sea Green
                RaceRecolorizer.GetColorsFromRGB(38f),
                RaceRecolorizer.GetColorsFromRGB(155f),
                RaceRecolorizer.GetColorsFromRGB(143f)
                ),
            new Color( // Almost Black Blue
                RaceRecolorizer.GetColorsFromRGB(6f),
                RaceRecolorizer.GetColorsFromRGB(16f),
                RaceRecolorizer.GetColorsFromRGB(59f)
                ),
            new Color( // Light Blue
                RaceRecolorizer.GetColorsFromRGB(134f),
                RaceRecolorizer.GetColorsFromRGB(153f),
                RaceRecolorizer.GetColorsFromRGB(236f)
                ),
            new Color( // Black
                RaceRecolorizer.GetColorsFromRGB(30f),
                RaceRecolorizer.GetColorsFromRGB(30f),
                RaceRecolorizer.GetColorsFromRGB(35f)
                )
        ];

        public static EquipmentEntityLink[] MaleHeads =
        [
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).MaleOptions.Heads[0],
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).MaleOptions.Heads[3]
        ];

        public static EquipmentEntityLink[] FemaleHeads =
        [
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).FemaleOptions.Heads[0],
            BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.TieflingRace.ToString()).FemaleOptions.Heads[3]
        ];

        public static EquipmentEntityLink[] FemaleHairs =
        [
            new EquipmentEntityLink() {AssetId = "afa22656ed5030c4ba273583ba2b3a16"}, // Long Wild Tiefling
            new EquipmentEntityLink() {AssetId = "3897a5ce68b630548bb85db0a68a465a"}  // Tight French Braid
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.HumanRace.ToString());

        private static readonly string UndineName = "UndineRace";

        internal const string UndineDisplayName = "Undine.Name";
        private static readonly string UndineDescription = "Undine.Description";
        public static readonly string RaceGuid = "{E7D00F27-6230-42D2-896F-D5215F5876D8}";

        internal const string WaterAffinityDisplayName = "Undine.WaterAffinity.Name";
        private static readonly string WaterAffinityDescription = "Undine.WaterAffinity.Description";

        internal const string EnergyResistanceDisplayName = "Undine.EnergyResistance.Name";
        private static readonly string EnergyResistanceDescription = "Undine.EnergyResistance.Description";

        internal const string ElementalHeritageDisplayName = "Undine.ElementalHeritage.Name";
        private static readonly string ElementalHeritageDescription = "Undine.ElementalHeritage.Description";

        internal const string UndineGeneralDisplayName = "Undine.UndineGeneral.Name";
        private static readonly string UndineGeneralDescription = "Undine.UndineGeneral.Description";

        internal const string MistsoulDisplayName = "Undine.Mistsoul.Name";
        private static readonly string MistsoulDescription = "Undine.Mistsoul.Description";

        internal const string RimesoulDisplayName = "Undine.Rimesoul.Name";
        private static readonly string RimesoulDescription = "Undine.Rimesoul.Description";

        internal const string ChillTouchDisplayName = "Undine.ChillTouch.Name";
        private const string ChillTouchDescription = "Undine.ChillTouch.Description";
        private const string ChillTouchBaseDisplayName = "Undine.ChillTouch.Touch.Name";

        internal const string HydraulicPushDisplayName = "Undine.HydraulicPush.Name";
        private static readonly string HydraulicPushDescription = "Undine.HydraulicPush.Description";

        internal static void Configure()
        {
            var NewFemaleHairArray = FemaleHairs.AppendToArray(CopyRace.FemaleOptions.Hair);

            // Spell-Like Abilities
            var SpellLikeResource = AbilityResourceConfigurator.New("UndineSpellLikeResource", "{45AB6FBC-C144-4F73-8D43-A3CA4B4B4E54}")
                .SetMaxAmount(
                    ResourceAmountBuilder.New(1))
                .Configure();

            var ChillTouchBase = AbilityConfigurator.NewSpell("UndineChillTouchBase", "{6B4C7DC4-2197-4EE6-8AD3-EC2A7623FF6A}", SpellSchool.Necromancy, canSpecialize: false)
                .SetDisplayName(ChillTouchBaseDisplayName)
                .SetDescription(ChillTouchDescription)
                .SetIcon(AbilityRefs.ArmyMagusChillTouch.Reference.Get().Icon)
                .SetLocalizedSavingThrow(Common.SavingThrowFortPartialOrWillNegates)
                .SetRange(AbilityRange.Touch)
                .AllowTargeting(enemies: true)
                .SetSpellResistance()
                .SetEffectOnEnemy(AbilityEffectOnUnit.Harmful)
                .SetAnimation(CastAnimationStyle.Touch)
                .SetActionType(CommandType.Standard)
                .AddComponent(new TouchCharges(ContextValues.Rank()))
                .AddAbilityDeliverTouch(touchWeapon: ItemWeaponRefs.TouchItem.ToString())
                .AddAbilityEffectRunAction(
                  actions:
                    ActionsBuilder.New()
                      .Conditional(
                        ConditionsBuilder.New().HasFact(FeatureRefs.UndeadType.ToString()),
                        ifTrue: ActionsBuilder.New()
                          .SavingThrow(
                            SavingThrowType.Will,
                            onResult: ActionsBuilder.New()
                              .ConditionalSaved(
                                failed: ActionsBuilder.New()
                                  .ApplyBuff(
                                    BuffRefs.FrightenedUndead.ToString(),
                                    ContextDuration.VariableDice(DiceType.D4, diceCount: 1, bonus: ContextValues.Rank())))),
                        ifFalse: ActionsBuilder.New()
                          .DealDamage(
                            DamageTypes.Energy(DamageEnergyType.NegativeEnergy),
                            ContextDice.Value(DiceType.D6, diceCount: ContextValues.Constant(1)))
                          .SavingThrow(
                            SavingThrowType.Fortitude,
                            onResult: ActionsBuilder.New()
                              .ConditionalSaved(
                                failed: ActionsBuilder.New()
                                  .DealDamageToAbility(StatType.Strength, ContextDice.Value(DiceType.One, diceCount: 1))))))
                .AddContextRankConfig(ContextRankConfigs.CharacterLevel())
                .SetType(AbilityType.SpellLike)
                .Configure();

            var ChillTouch = AbilityConfigurator.NewSpell("UndineChillTouch", "{B398AE19-AEE6-42F4-B5DE-ED5D1994C483}", SpellSchool.Necromancy, canSpecialize: true)
                .CopyFrom(ChillTouchBase)
                .SetDisplayName(ChillTouchDisplayName)
                .SetShouldTurnToTarget(true)
                .AddAbilityEffectStickyTouch(touchDeliveryAbility: ChillTouchBase)
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .SetType(AbilityType.SpellLike)
                .Configure();

            var blur = AbilityConfigurator.New("UndineBlur", "{368FA273-FBB7-4DBC-832C-3AF7CF15D247}")
                .CopyFrom(AbilityRefs.Blur, c => c is not (SpellListComponent or CraftInfoComponent))
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .SetType(AbilityType.SpellLike)
                .AddPretendSpellLevel(spellLevel: 2)
                .Configure();

            var HydraulicPush = AbilityConfigurator.NewSpell("UndineHydraulicPush", "{E8D66656-9B93-4DE8-9372-419F14EEAF67}", SpellSchool.Evocation, false)
                .SetDisplayName(HydraulicPushDisplayName)
                .SetDescription(HydraulicPushDescription)
                .SetIcon(AbilityRefs.WaterBlastAbility.Reference.Get().Icon)
                .AddComponent<AbilityDeliverProjectile>(c => {
                    c.m_Projectiles = new BlueprintProjectileReference[] {
                        BlueprintTools.GetBlueprintReference<BlueprintProjectileReference>("06e268d6a2b5a3a438c2dd52d68bfef6")
                    };
                    c.Type = AbilityProjectileType.Simple;
                    c.m_Length = new Feet() { m_Value = 0 };
                    c.m_LineWidth = new Feet() { m_Value = 5 };
                    c.AttackRollBonusStat = StatType.Unknown;
                })
                .AddComponent<AbilityEffectRunAction>(c => {
                    c.SavingThrowType = SavingThrowType.Unknown;
                    c.Actions = Helpers.CreateActionList(
                        new ContextActionCombatManeuver()
                        {
                            Type = CombatManeuver.BullRush,
                            ReplaceStat = true,
                            NewStat = StatType.Unknown,
                            UseCasterLevelAsBaseAttack = true,
                            UseBestMentalStat = true,
                            OnSuccess = Helpers.CreateActionList()
                        }
                        );
                })
                .SetType(AbilityType.SpellLike)
                .SetRange(AbilityRange.Close)
                .SetCanTargetPoint(false)
                .SetCanTargetEnemies(true)
                .SetCanTargetFriends(true)
                .SetCanTargetSelf(false)
                .SetEffectOnAlly(AbilityEffectOnUnit.None)
                .SetEffectOnEnemy(AbilityEffectOnUnit.Harmful)
                .SetAnimation(CastAnimationStyle.Omni)
                .SetActionType(CommandType.Standard)
                .AddAbilityResourceLogic(1, isSpendResource: true, requiredResource: SpellLikeResource)
                .Configure();

            // Elemental Heritage
            var Rimesoul = FeatureConfigurator.New("UndineRimesoul", "{A08E5760-98E5-474D-9B57-70EFEB881D01}")
                .SetDisplayName(RimesoulDisplayName)
                .SetDescription(RimesoulDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Charisma, value: -2)
                .AddFacts([ChillTouch])
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: ChillTouch)
                .Configure();

            var Mistsoul = FeatureConfigurator.New("UndineMistsoul", "{EAA925F4-C8CF-4CA0-ABE6-9632B4B78069}")
                .SetDisplayName(MistsoulDisplayName)
                .SetDescription(MistsoulDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Constitution, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: -2)
                .AddFacts([blur])
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: blur)
                .Configure();

            var UndineGeneral = FeatureConfigurator.New("UndineGeneral", "{FE92658B-828A-4BCD-8D94-72C2C5A6EFF3}")
                .SetDisplayName(UndineGeneralDisplayName)
                .SetDescription(UndineGeneralDescription)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Dexterity, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: -2)
                .AddFacts([HydraulicPush])
                .AddAbilityResources(1, SpellLikeResource, true)
                .AddReplaceCasterLevelOfAbility(spell: HydraulicPush)
                .Configure();

            var UndineElementalHeritageSelection = FeatureSelectionConfigurator.New("UndineElementalHeritageSelection", "{8219CCA3-3A7D-4785-B273-F23032413991}")
                .SetDisplayName(ElementalHeritageDisplayName)
                .SetDescription(ElementalHeritageDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeatureSelection>(FeatureSelectionRefs.HalfOrcHeritageSelection.ToString()).Icon)
                .AddToAllFeatures(UndineGeneral, Mistsoul, Rimesoul)
                .SetGroup(FeatureGroup.Racial)
                .Configure();

            // Energy Resistance
            var EnergyResistance = FeatureConfigurator.New("UndineEnergyResistance", "{A9CF4775-23A8-4CD2-80C0-1CEF37A27B32}")
                .SetDisplayName(EnergyResistanceDisplayName)
                .SetDescription(EnergyResistanceDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.BloodlineDraconicWhiteResistancesAbilityLevel1.ToString()).Icon)
                .AddDamageResistanceEnergy(type: DamageEnergyType.Cold, value: 5)
                .Configure();

            // Water Affinity
            var WaterAffinity = FeatureConfigurator.New("UndineWaterAffinity", "{088DB5A3-49BE-4AA4-B7C8-236516AFC23C}")
                .SetDisplayName(WaterAffinityDisplayName)
                .SetDescription(WaterAffinityDescription)
                .SetIcon(BlueprintTools.GetBlueprint<BlueprintFeature>(FeatureRefs.BloodlineElementalWaterArcana.ToString()).Icon)
                .AddIncreaseSpellDescriptorCasterLevel(1, SpellDescriptor.Cold, modifierDescriptor: ModifierDescriptor.Racial)
                .AddIncreaseSpellDescriptorDC(1, SpellDescriptor.Cold, modifierDescriptor: ModifierDescriptor.Racial)
                .Configure();

            var race =
            RaceConfigurator.New(UndineName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(UndineDisplayName)
                .SetDescription(UndineDescription)
                .SetSelectableRaceStat(false)
                .SetFeatures(UndineElementalHeritageSelection, EnergyResistance, WaterAffinity, FeatureRefs.KeenSenses.ToString())
                .SetRaceId(Race.Human)
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, eyecolors: RaceEyeColors, eyerace: BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.OreadRace.ToString()), CustomMaleHeads: MaleHeads, CustomFemaleHeads: FemaleHeads, CustomFemaleHairs: NewFemaleHairArray);

            // Add race to mount fixes
            RaceMountFixerizer.AddRaceToMountFixes(recoloredrace, CopyRace);

            // Add race to race list
            var raceRef = recoloredrace.ToReference<BlueprintRaceReference>();
            ref var races = ref BlueprintRoot.Instance.Progression.m_CharacterRaces;

            var length = races.Length;
            Array.Resize(ref races, length + 1);
            races[length] = raceRef;
        }
    }
}
