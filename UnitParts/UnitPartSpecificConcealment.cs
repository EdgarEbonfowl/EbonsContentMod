using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.Enums.Damage;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.Utility;
using Kingmaker.Designers.Mechanics.Collections;
using Kingmaker.UnitLogic.Buffs;
using JetBrains.Annotations;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Root;
using Kingmaker.Controllers;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.ElementsSystem;
using Kingmaker.EntitySystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.Settings;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Mechanics;
using Newtonsoft.Json;
using Owlcat.Runtime.Core.Utils;
using TurnBased.Controllers;
using EbonsContentMod.Components;
using TabletopTweaks.Core.Config;
using Kingmaker.Enums;
using Kingmaker.Items;
using Kingmaker.UnitLogic.Parts;
using Owlcat.Runtime.Core.Logging;



namespace EbonsContentMod.UnitParts
{
    // Holic75_PT
    public class UnitPartSpecificConcealment : AdditiveUnitPart
    {

        public override CountableFlag UnitPartFlag
        {
            get
            {
                return SpecificConcealment;
            }
        }

        public void AddEntry(EntityFact fact, SpecificConcealmentBase component)
        {
            if (this.SpecificConcealmentEntries.HasItem((UnitPartSpecificConcealment.SpecificConcealmentEntry i) => i.Fact == fact && i.Component == component))
            {
                string messageFormat = "UnitPartSpecificConcealment.AddEntry: item already exists (fact: {0}, component: {1})";
                object[] array = new object[2];
                array[0] = fact;
                int num = 1;
                SpecificConcealmentBase SpecificConcealmentBaseEffect = component.Or(null);
                array[num] = ((SpecificConcealmentBaseEffect != null) ? SpecificConcealmentBaseEffect.name : null);
                string error_message = String.Format(messageFormat, array[0], array[1]);
                return;
            }
            this.SpecificConcealmentEntries.Add(new UnitPartSpecificConcealment.SpecificConcealmentEntry(fact, component));
        }

        public void RemoveEntry(EntityFact fact, SpecificConcealmentBase component)
        {
            this.SpecificConcealmentEntries.RemoveAll((UnitPartSpecificConcealment.SpecificConcealmentEntry i) => i.Fact == fact && i.Component == component);
            if (this.SpecificConcealmentEntries.Count < 1)
            {
                this.RemoveUnitPartFlag();
            }
        }

        public UnitPartConcealment.ConcealmentEntry[] GetConcealments(UnitEntityData attacker)
        {
            var Concealments = new List<UnitPartConcealment.ConcealmentEntry>();

            if (!(this.SpecificConcealmentEntries.Count < 1))
            {
                foreach (var entry in this.SpecificConcealmentEntries)
                {
                    bool Works_On = false;

                    entry.Fact.CallComponents<SpecificConcealmentBase>(s => Works_On = s.WorksAgainst(attacker));

                    if (Works_On)
                    {
                        entry.Fact.CallComponents<SpecificConcealmentBase>(s => Concealments.Add(s.CreateConcealmentEntry()));
                    }
                }
            }

            return Concealments.ToArray();
        }


        public CountableFlag SpecificConcealment = new CountableFlag();

        public readonly List<UnitPartSpecificConcealment.SpecificConcealmentEntry> SpecificConcealmentEntries = new List<UnitPartSpecificConcealment.SpecificConcealmentEntry>();

        public struct SpecificConcealmentEntry
        {
            public SpecificConcealmentEntry(EntityFact fact, SpecificConcealmentBase component)
            {
                this.Fact = fact;
                this.Component = component;
            }

            public readonly EntityFact Fact;

            public readonly SpecificConcealmentBase Component;
        }


    }
}