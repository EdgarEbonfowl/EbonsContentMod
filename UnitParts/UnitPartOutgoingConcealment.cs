using JetBrains.Annotations;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Parts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UnitLogic.Parts.UnitPartConcealment;
using HarmonyLib;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.Utility;
using Kingmaker.UnitLogic.FactLogic;
using TabletopTweaks.Core.Utilities;
using Kingmaker.Blueprints.Root;
using Kingmaker;

namespace EbonsContentMod.UnitParts
{
    // Holic75_PT
    // This part was ported, but allows to ignore specific (not just "fog") concealment descriptors.
    public class UnitPartOutgoingConcealment : OldStyleUnitPart
    {

        public void AddConcealment(UnitPartConcealment.ConcealmentEntry entry)
        {
            if (this.m_Concealments == null)
            {
                this.m_Concealments = new List<UnitPartConcealment.ConcealmentEntry>();
            }
            this.m_Concealments.Add(entry);
        }

        public void RemoveConcealement(UnitPartConcealment.ConcealmentEntry entry)
        {
            if (this.m_Concealments == null)
            {
                return;
            }
            foreach (UnitPartConcealment.ConcealmentEntry concealmentEntry in this.m_Concealments)
            {
                if (concealmentEntry.Descriptor == entry.Descriptor && concealmentEntry.Concealment == entry.Concealment)
                {
                    WeaponRangeType? rangeType = concealmentEntry.RangeType;
                    WeaponRangeType? rangeType2 = entry.RangeType;
                    if ((rangeType.GetValueOrDefault() == rangeType2.GetValueOrDefault() & rangeType != null == (rangeType2 != null)) && concealmentEntry.DistanceGreater == entry.DistanceGreater)
                    {
                        this.m_Concealments.Remove(concealmentEntry);
                        if (this.m_Concealments.Count <= 0)
                        {
                            this.m_Concealments = null;
                        }
                        break;
                    }
                }
            }
        }

        public static Concealment Calculate([NotNull] UnitEntityData initiator, [NotNull] UnitEntityData target, bool attack = false)
        {
            UnitPartConcealment unitPartConcealment = initiator.Get<UnitPartConcealment>();
            UnitPartOutgoingConcealment unitPartOutgoingConcealment = initiator.Get<UnitPartOutgoingConcealment>();

            if (unitPartOutgoingConcealment?.m_Concealments == null)
            {
                return Concealment.None;                                // No concealment if the list is null
            }

            bool has_working_true_seeing = initiator.Descriptor.State.HasConditionAndItWorksOnTarget(UnitCondition.TrueSeeing, target);


            Concealment a = Concealment.None;

            var ignore_concealment_descriptor_part = initiator.Get<UnitPartIgnoreConcealmentDescriptor>();


            foreach (UnitPartConcealment.ConcealmentEntry concealment in unitPartOutgoingConcealment.m_Concealments)
            {

                bool is_ignore_concealment_descriptor_active = (ignore_concealment_descriptor_part != null) && (ignore_concealment_descriptor_part.Active());
                bool is_concealment_descriptor_ignored = ignore_concealment_descriptor_part.IsIgnoringConcealmentDescriptor(concealment.Descriptor);

                if (is_ignore_concealment_descriptor_active && is_concealment_descriptor_ignored)
                {
                    continue;
                }
                if ((!is_concealment_descriptor_ignored) && concealment.Descriptor != ConcealmentDescriptor.InitiatorIsBlind && has_working_true_seeing)
                {
                    continue;
                }
                if (!concealment.OnlyForAttacks || attack)
                {
                    float num2 = initiator.DistanceTo(target);
                    float num3 = initiator.View.Corpulence + target.View.Corpulence;
                    if ((double)num2 <= (double)concealment.DistanceGreater.Meters + (double)num3)
                    {
                        continue;
                    }
                    if (concealment.RangeType != null)
                    {
                        RuleAttackRoll ruleAttackRoll = Rulebook.CurrentContext.LastEvent<RuleAttackRoll>();
                        ItemEntityWeapon itemEntityWeapon = (ruleAttackRoll != null) ? ruleAttackRoll.Weapon : initiator.GetFirstWeapon();
                        if (itemEntityWeapon == null || !concealment.RangeType.Value.IsSuitableWeapon(itemEntityWeapon))
                        {
                            continue;
                        }
                    }
                    a = UnitPartOutgoingConcealment.Max(a, concealment.Concealment);
                }
            }

            return a;
        }


        public static Concealment Max(Concealment a, Concealment b)
        {
            if (a <= b)
            {
                return b;
            }
            return a;
        }

        [CanBeNull]
        private List<UnitPartConcealment.ConcealmentEntry> m_Concealments;

        [HarmonyPatch(typeof(UnitPartConcealment))]
        [HarmonyPatch("Calculate", MethodType.Normal)]
        static class UnitPartConcealment__Calculate__Patch
        {
            public static bool Prefix(UnitEntityData initiator, UnitEntityData target, bool attack, ref Concealment __result)
            {
                UnitPartConcealment unitPartConcealment1 = initiator.Get<UnitPartConcealment>();
                UnitPartConcealment unitPartConcealment2 = target.Get<UnitPartConcealment>();
                bool has_true_seeing = initiator.Descriptor.State.HasConditionAndItWorksOnTarget(UnitCondition.TrueSeeing, target);

                if ((unitPartConcealment1 != null) && unitPartConcealment1.IgnoreAll)
                {
                    __result = Concealment.None;
                    return false;
                }

                List<ValueTuple<Feet, UnitConditionExceptions>> m_BlindsightRanges = Traverse.Create(unitPartConcealment1).Field("m_BlindsightRanges").GetValue<List<ValueTuple<Feet, UnitConditionExceptions>>>();

                if (m_BlindsightRanges != null)
                {
                    Feet feet = 0.Feet();

                    foreach (ValueTuple<Feet, UnitConditionExceptions> valueTuple in m_BlindsightRanges)
                    {
                        if ((valueTuple.Item2 == null || !valueTuple.Item2.IsExceptional(target)) && feet < valueTuple.Item1)
                        {
                            feet = valueTuple.Item1;
                        }
                    }
                    float num = initiator.View.Corpulence + target.View.Corpulence;
                    if ((double)initiator.DistanceTo(target) - (double)num <= (double)feet.Meters)
                    {
                        __result = Concealment.None;
                        return false;
                    }
                }
                Concealment a = Concealment.None;
                bool has_working_true_seeing = initiator.Descriptor.State.HasConditionAndItWorksOnTarget(UnitCondition.TrueSeeing, target);
                if (target.Descriptor.State.HasCondition(UnitCondition.Invisible) && !initiator.Descriptor.State.HasConditionAndItWorksOnTarget(UnitCondition.SeeInvisibility, target) && !has_working_true_seeing)
                {
                    a = Concealment.Total;
                }

                var ignore_concealment_descriptor_part = initiator.Get<UnitPartIgnoreConcealmentDescriptor>();

                List<UnitPartConcealment.ConcealmentEntry> m_Concealments = Traverse.Create(unitPartConcealment2).Field("m_Concealments").GetValue<List<UnitPartConcealment.ConcealmentEntry>>();

                var all_concealments = m_Concealments?.ToArray() ?? new UnitPartConcealment.ConcealmentEntry[0];
                var specific_concealment_part = target.Get<UnitPartSpecificConcealment>();

                if (specific_concealment_part != null)
                {
                    all_concealments = all_concealments.AppendToArray(specific_concealment_part.GetConcealments(initiator));
                }

                if (a < Concealment.Total && !all_concealments.Empty())
                {
                    foreach (UnitPartConcealment.ConcealmentEntry concealment in all_concealments)
                    {
                        bool is_ignore_concealment_descriptor_active = (ignore_concealment_descriptor_part != null) && (ignore_concealment_descriptor_part.Active());
                        bool is_concealment_descriptor_ignored = ignore_concealment_descriptor_part.IsIgnoringConcealmentDescriptor(concealment.Descriptor);

                        if (is_ignore_concealment_descriptor_active && is_concealment_descriptor_ignored)
                        {
                            continue;
                        }
                        if ((!is_concealment_descriptor_ignored) && concealment.Descriptor != ConcealmentDescriptor.InitiatorIsBlind && has_working_true_seeing)
                        {
                            continue;
                        }
                        if (!concealment.OnlyForAttacks || attack)
                        {
                            float num2 = initiator.DistanceTo(target);
                            float num3 = initiator.View.Corpulence + target.View.Corpulence;
                            if ((double)num2 <= (double)concealment.DistanceGreater.Meters + (double)num3)
                            {
                                continue;
                            }
                            if (concealment.RangeType != null)
                            {
                                RuleAttackRoll ruleAttackRoll = Rulebook.CurrentContext.LastEvent<RuleAttackRoll>();
                                ItemEntityWeapon itemEntityWeapon = (ruleAttackRoll != null) ? ruleAttackRoll.Weapon : initiator.GetFirstWeapon();
                                if (itemEntityWeapon == null || !concealment.RangeType.Value.IsSuitableWeapon(itemEntityWeapon))
                                {
                                    continue;
                                }
                            }
                            a = a > concealment.Concealment ? a : concealment.Concealment;
                        }
                    }
                }
                if (unitPartConcealment2 != null && unitPartConcealment2.Disable)
                {
                    a = Concealment.None;
                }
                if (initiator.Descriptor.State.HasCondition(UnitCondition.Blindness))
                {
                    a = Concealment.Total;
                }
                if (initiator.Descriptor.State.HasCondition(UnitCondition.PartialConcealmentOnAttacks))
                {
                    a = Concealment.Partial;
                }
                if (a == Concealment.None && (ignore_concealment_descriptor_part == null || !ignore_concealment_descriptor_part.Active()) && Game.Instance.Player.Weather.ActualWeather >= BlueprintRoot.Instance.WeatherSettings.ConcealmentBeginsOn)
                {
                    RuleAttackRoll ruleAttackRoll2 = Rulebook.CurrentContext.LastEvent<RuleAttackRoll>();
                    ItemEntityWeapon itemEntityWeapon2 = (ruleAttackRoll2 != null) ? ruleAttackRoll2.Weapon : initiator.GetFirstWeapon();
                    if (itemEntityWeapon2 != null && WeaponRangeType.Ranged.IsSuitableWeapon(itemEntityWeapon2))
                    {
                        a = Concealment.Partial;
                    }
                }

                a = UnitPartOutgoingConcealment.Max(UnitPartOutgoingConcealment.Calculate(initiator, target, attack), a);

                if (unitPartConcealment1 != null && unitPartConcealment1.IgnorePartial && a == Concealment.Partial)
                {
                    a = Concealment.None;
                }
                if (unitPartConcealment1 != null && unitPartConcealment1.TreatTotalAsPartial && a == Concealment.Total)
                {
                    a = Concealment.Partial;
                }
                __result = a;
                return false;


            }
        }


    }
}