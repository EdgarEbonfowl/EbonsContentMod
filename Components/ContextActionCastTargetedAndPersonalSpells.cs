using System;
using Kingmaker;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.EntitySystem.Entities;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.UnitLogic.Mechanics.Actions;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.Utility;
using UnityEngine;
using UnityEngine.Serialization;
using Kingmaker.UnitLogic;

namespace EbonsContentMod.Components
{
    internal class ContextActionCastTargetedAndPersonalSpells : ContextAction
    {
        public BlueprintAbility Spell
        {
            get
            {
                BlueprintAbilityReference spell = this.m_Spell;
                if (spell == null)
                {
                    return null;
                }
                return spell.Get();
            }
        }

        public BlueprintAbility SecondSpell
        {
            get
            {
                BlueprintAbilityReference spell = this.m_SecondSpell;
                if (spell == null)
                {
                    return null;
                }
                return spell.Get();
            }
        }

        public override string GetCaption()
        {
            return string.Format("Cast spell {0}", this.Spell) + (this.OverrideDC ? string.Format(" DC: {0}", this.DC) : "") + (this.OverrideSpellLevel ? string.Format(" SL: {0}", this.SpellLevel) : "");
        }

        public override void RunAction()
        {
            if (base.Context.MaybeCaster == null)
            {
                PFLog.Default.Error(this, "Caster is missing", Array.Empty<object>());
                return;
            }
            if (base.Target.Unit == null)
            {
                PFLog.Default.Error(this, "Target Unit is missing", Array.Empty<object>());
                return;
            }
            AbilityData spellData;
            AbilityData secondSpellData;
            if (!this.CastByTarget)
            {
                spellData = new AbilityData(this.Spell, base.Context.MaybeCaster.Descriptor);
                secondSpellData = new AbilityData(this.SecondSpell, base.Context.MaybeCaster.Descriptor);
            }
            else
            {
                spellData = new AbilityData(this.Spell, base.Target.Unit.Descriptor);
                secondSpellData = new AbilityData(this.SecondSpell, base.Target.Unit.Descriptor);
            }
            if (this.MarkAsChild)
            {
                spellData.IsChildSpell = true;
                secondSpellData.IsChildSpell = true;
            }
            if (this.OverrideSpellbook)
            {
                AbilityData spellData2 = spellData;
                AbilityData secondSpellData2 = secondSpellData;
                AbilityExecutionContext abilityContext = base.AbilityContext;
                Spellbook overrideSpellbook;
                if ((overrideSpellbook = ((abilityContext != null) ? abilityContext.Ability.OverrideSpellbook : null)) == null)
                {
                    AbilityExecutionContext abilityContext2 = base.AbilityContext;
                    overrideSpellbook = ((abilityContext2 != null) ? abilityContext2.Ability.Spellbook : null);
                }
                spellData2.OverrideSpellbook = overrideSpellbook;
                secondSpellData2.OverrideSpellbook = overrideSpellbook;
            }
            if (this.OverrideDC)
            {
                spellData.OverrideDC = new int?(this.DC.Calculate(base.Context));
                secondSpellData.OverrideDC = new int?(this.DC.Calculate(base.Context));
            }
            if (this.OverrideSpellLevel)
            {
                spellData.OverrideSpellLevel = new int?(this.SpellLevel.Calculate(base.Context));
                secondSpellData.OverrideSpellLevel = new int?(this.SpellLevel.Calculate(base.Context));
            }
            if (!spellData.CanTarget(base.Target) || (this.CastByTarget && !spellData.IsAvailableForCast))
            {
                PFLog.Default.Error(this, string.Format("{0}: {1} is not valid target for {2}", this.name, base.Target, this.Spell), Array.Empty<object>());
                if (this.LogIfCanNotTarget)
                {
                    EventBus.RaiseEvent<IUILogCustomAbilityImmuneHandler>(delegate (IUILogCustomAbilityImmuneHandler h)
                    {
                        h.HandleImmuneToAbility(spellData, this.Target.Unit);
                    }, true);
                }
                return;
            }
            AbilityExecutionContext abilityContext3 = base.AbilityContext;
            bool isDuplicateSpellApplied = abilityContext3 != null && abilityContext3.IsDuplicateSpellApplied;
            Rulebook.Trigger<RuleCastSpell>(new RuleCastSpell(spellData, base.Target)
            {
                IsDuplicateSpellApplied = isDuplicateSpellApplied
            });
            Rulebook.Trigger<RuleCastSpell>(new RuleCastSpell(secondSpellData, base.Context.MaybeCaster)
            {
                IsDuplicateSpellApplied = isDuplicateSpellApplied
            });
            TargetWrapper targetWrapper = this.CastByTarget ? base.Target : base.Context.MaybeCaster;
            if (this.m_FullRoundAction && targetWrapper != null)
            {
                UnitEntityData unit = targetWrapper.Unit;
                if (unit != null)
                {
                    unit.SpendAction(UnitCommand.CommandType.Standard, true, 0f);
                }
            }
            if (this.m_StandardAction && targetWrapper != null)
            {
                UnitEntityData unit2 = targetWrapper.Unit;
                if (unit2 != null)
                {
                    unit2.SpendAction(UnitCommand.CommandType.Standard, false, 0f);
                }
            }
            if (this.m_MoveAction && targetWrapper != null)
            {
                UnitEntityData unit3 = targetWrapper.Unit;
                if (unit3 != null)
                {
                    unit3.SpendAction(UnitCommand.CommandType.Move, false, 0f);
                }
            }
            if (this.m_SwiftAction && targetWrapper != null)
            {
                UnitEntityData unit4 = targetWrapper.Unit;
                if (unit4 == null)
                {
                    return;
                }
                unit4.SpendAction(UnitCommand.CommandType.Swift, false, 0f);
            }
        }

        [SerializeField]
        [FormerlySerializedAs("Spell")]
        public BlueprintAbilityReference m_Spell;

        public BlueprintAbilityReference m_SecondSpell;

        public bool OverrideSpellbook;

        public bool OverrideDC;

        [ShowIf("OverrideDC")]
        public ContextValue DC;

        public bool OverrideSpellLevel;

        [ShowIf("OverrideSpellLevel")]
        public ContextValue SpellLevel;

        public bool CastByTarget;

        public bool LogIfCanNotTarget;

        public bool MarkAsChild;

        [SerializeField]
        public bool m_SpendAction;

        [SerializeField]
        [ShowIf("m_SpendAction")]
        public bool m_FullRoundAction;

        [SerializeField]
        [ShowIf("m_SpendAction")]
        public bool m_StandardAction;

        [SerializeField]
        [ShowIf("m_SpendAction")]
        public bool m_MoveAction;

        [SerializeField]
        [ShowIf("m_SpendAction")]
        public bool m_SwiftAction;
    }
}
