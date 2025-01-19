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
using Kingmaker.Craft;
using Kingmaker.ResourceLinks;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using EbonsContentMod.Components;
using Kingmaker.Blueprints.Classes.Spells;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.UnitLogic.Mechanics.Components;
using BlueprintCore.Utils.Types;
using System.Linq;
using BlueprintCore.Actions.Builder;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Commands.Base;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.UnitLogic.FactLogic;


namespace EbonsContentMod.Races
{
    internal class Rougarou
    {
        public static List<Color> RaceHeadColors = [];

        public static List<Color> RaceHairColors = [];

        public static EquipmentEntityLink[] MaleHeads =
        [
            new EquipmentEntityLink() {AssetId = "ab2f104d7da82b04096926588b1d9931"},
            new EquipmentEntityLink() {AssetId = "f425aeb797e5d2e47a9d197be278583f"}, 
            new EquipmentEntityLink() {AssetId = "47009e9da206fea4d98ccffdd824de26"}, 
            new EquipmentEntityLink() {AssetId = "f899a03df5f1be5448a40693a7ce3572"},
            new EquipmentEntityLink() {AssetId = "09629795c40f6874aa82e8545582595e"},
            new EquipmentEntityLink() {AssetId = "f7ccdeb39f9d45145b8a73dbd902aa6e"}
        ];

        // Good amount of clipping in all these, but oh well
        public static EquipmentEntityLink[] FemaleHeads =
        [
            new EquipmentEntityLink() {AssetId = "fe61066c32dce2746a8382eed3f800a9"}, 
            new EquipmentEntityLink() {AssetId = "b5557b26e1cb015439856936706e0f91"}, 
            new EquipmentEntityLink() {AssetId = "3e06d6c73dbfc784faf21d5263e84881"},
            new EquipmentEntityLink() {AssetId = "0412d70836fe0b947884dcaf8f345f22"},
            new EquipmentEntityLink() {AssetId = "8661c51a25896ee40a724bcfa82fa05f"},
            new EquipmentEntityLink() {AssetId = "187c2d326f6b1184eb985010ae35cc22"}
        ];

        public static List<Texture2D> CustomSkinRamps =
        [
            RaceRecolorizer.GetArmorRampByIndex(1),
            RaceRecolorizer.GetArmorRampByIndex(2),
            RaceRecolorizer.GetArmorRampByIndex(3),
            RaceRecolorizer.GetArmorRampByIndex(4),
            RaceRecolorizer.GetArmorRampByIndex(5),
            RaceRecolorizer.GetArmorRampByIndex(74),
            RaceRecolorizer.GetArmorRampByIndex(75),
            RaceRecolorizer.GetArmorRampByIndex(76),
            RaceRecolorizer.GetArmorRampByIndex(77),
            RaceRecolorizer.GetArmorRampByIndex(0),
            RaceRecolorizer.GetArmorRampByIndex(6),
            RaceRecolorizer.GetArmorRampByIndex(7),
            RaceRecolorizer.GetArmorRampByIndex(8)
            
        ];

        public static List<Texture2D> CustomEyeRamps =
        [
            RaceRecolorizer.GetArmorRampByIndex(45),
            RaceRecolorizer.GetArmorRampByIndex(46),
            RaceRecolorizer.GetArmorRampByIndex(73),
            RaceRecolorizer.GetArmorRampByIndex(74),
            RaceRecolorizer.GetArmorRampByIndex(75),
            RaceRecolorizer.GetArmorRampByIndex(52),
            RaceRecolorizer.GetArmorRampByIndex(53),
            RaceRecolorizer.GetArmorRampByIndex(54),
            RaceRecolorizer.GetArmorRampByIndex(38),
            RaceRecolorizer.GetArmorRampByIndex(39),
            RaceRecolorizer.GetArmorRampByIndex(40),
            RaceRecolorizer.GetArmorRampByIndex(41),
            RaceRecolorizer.GetArmorRampByIndex(31),
            RaceRecolorizer.GetArmorRampByIndex(32),
            RaceRecolorizer.GetArmorRampByIndex(33),
            RaceRecolorizer.GetArmorRampByIndex(34),
            RaceRecolorizer.GetArmorRampByIndex(35),
            RaceRecolorizer.GetArmorRampByIndex(36),
            RaceRecolorizer.GetArmorRampByIndex(59),
            RaceRecolorizer.GetArmorRampByIndex(60),
            RaceRecolorizer.GetArmorRampByIndex(61)
        ];

        public static BlueprintRace CopyRace = BlueprintTools.GetBlueprint<BlueprintRace>(RaceRefs.KitsuneRace.ToString());

        private static readonly string RougarouName = "RougarouRace";

        internal const string RougarouDisplayName = "Rougarou.Name";
        private static readonly string RougarouDescription = "Rougarou.Description";
        public static readonly string RaceGuid = "{3DDDDE72-0F8E-4CD0-A470-003C74F25FEB}";

        internal const string NaturalAttackDisplayName = "Rougarou.NaturalAttack.Name";
        private static readonly string NaturalAttackDescription = "Rougarou.NaturalAttack.Description";

        internal const string ChangeShapeDisplayName = "Rougarou.ChangeShape.Name";
        private static readonly string ChangeShapeDescription = "Rougarou.ChangeShape.Description";

        internal static void Configure()
        {
            var ChangeShapeBuff = BuffConfigurator.New("EbonsRougarouChangeShapeBuff", "{A322AB4B-F691-4AE4-8382-A52C17A980CC}")
                //.CopyFrom(BuffRefs.BeastShapeIBuff)
                .SetDisplayName(ChangeShapeDisplayName)
                .SetDescription(ChangeShapeDescription)
                .SetIcon(AbilityRefs.AspectOfTheWolf.Reference.Get().Icon)
                .AddPolymorph(
                    strengthBonus: 2, naturalArmor: 2, prefab: "0dc0f602a83a2034ba5842f73c0012c1", keepSlots: false, allowDamageTransfer: false, silentCaster: true, size: Size.Medium, specialDollType: SpecialDollType.None,
                    facts: [FeatureRefs.TrippingBite.ToString()], mainHand: ItemWeaponRefs.Bite1d6.ToString(),
                    enterTransition: Helpers.CreateCopy(BlueprintTools.GetBlueprint<BlueprintBuff>(BuffRefs.BeastShapeIBuff.ToString()).GetComponent<Polymorph>().m_EnterTransition),
                    exitTransition: Helpers.CreateCopy(BlueprintTools.GetBlueprint<BlueprintBuff>(BuffRefs.BeastShapeIBuff.ToString()).GetComponent<Polymorph>().m_ExitTransition),
                    transitionExternal: Helpers.CreateCopy(BlueprintTools.GetBlueprint<BlueprintBuff>(BuffRefs.BeastShapeIBuff.ToString()).GetComponent<Polymorph>().m_TransitionExternal))
                .AddPolymorphBonuses(4)
                .AddSpellDescriptorComponent(SpellDescriptor.Polymorph)
                .AddBuffMovementSpeed(value: 20)
                .AddReplaceAsksList("a05bf641a3fb6e3498707ac9814f583b")
                .AddReplaceSourceBone("Locator_HeadCenterFX_00")
                .AddReplaceCastSource(Kingmaker.Blueprints.Root.Fx.CastSource.Head)
                .AddComponent<SuppressBuffs>(c => { c.m_Buffs = [BlueprintTools.GetBlueprintReference<BlueprintBuffReference>(BuffRefs.EnlargePersonBuff.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintBuffReference>(BuffRefs.EnlargePersonPrimal8Buff.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintBuffReference>(BuffRefs.EnlargePersonPrimal16Buff.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintBuffReference>(BuffRefs.ReducePersonBuff.ToString()),
                    BlueprintTools.GetBlueprintReference<BlueprintBuffReference>(BuffRefs.ShifterClawVisualBuff.ToString())]; })
                .Configure();

            var ChangeShape = ActivatableAbilityConfigurator.New("RougarouChangeShape", "{E0522152-5396-4ACB-B370-7BB49F4B593B}")
                .SetDisplayName(ChangeShapeDisplayName)
                .SetDescription(ChangeShapeDescription)
                .SetIcon(AbilityRefs.AspectOfTheWolf.Reference.Get().Icon)
                .SetDeactivateImmediately(true)
                .SetDeactivateIfOwnerDisabled(true)
                .SetActivateWithUnitCommand(UnitCommand.CommandType.Standard)
                .SetBuff(ChangeShapeBuff)
                .AddActionsOnBuffApply(ActionsBuilder.New().RemoveBuffsByDescriptor(SpellDescriptor.Polymorph).Build())
                .Configure();

            var ChangeShapeFeature = FeatureConfigurator.New("RougarouChangeShapeFeature", "{94482F9B-A18B-4356-821B-C84A6CC4DCA2}")
                .SetDisplayName(ChangeShapeDisplayName)
                .SetDescription(ChangeShapeDescription)
                .SetIcon(AbilityRefs.AspectOfTheWolf.Reference.Get().Icon)
                .AddFacts([ChangeShape])
                .Configure();

            var NaturalAttack = FeatureConfigurator.New("RougarouNaturalAttack", "{19BAD70E-C12F-472F-9887-217B10F9680B}")
                .SetDisplayName(NaturalAttackDisplayName)
                .SetDescription(NaturalAttackDescription)
                .SetIcon(AbilityRefs.AcidMaw.Reference.Get().Icon)
                .AddAdditionalLimb(weapon: ItemWeaponRefs.Bite1d4.ToString())
                .Configure();

            var race =
            RaceConfigurator.New(RougarouName, RaceGuid)
                .CopyFrom(CopyRace)
                .SetDisplayName(RougarouDisplayName)
                .SetDescription(RougarouDescription)
                .SetSelectableRaceStat(false)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Strength, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Wisdom, value: 2)
                .AddStatBonus(ModifierDescriptor.Racial, stat: StatType.Intelligence, value: -2)
                .SetFeatures(ChangeShapeFeature, NaturalAttack, FeatureRefs.KeenSenses.ToString())
                .SetRaceId(Race.Human)
                .ClearSpecialDollTypes()
                .Configure();

            // Recolor Race
            var recoloredrace = RaceRecolorizer.RecolorRace(race, RaceHeadColors, RaceHairColors, KitsuneBaldRace: true, CustomMaleHeads: MaleHeads, CustomFemaleHeads: FemaleHeads, CustomHeadRamps: CustomSkinRamps, CustomEyeRamps: CustomEyeRamps);

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
