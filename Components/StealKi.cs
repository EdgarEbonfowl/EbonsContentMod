using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.RuleSystem;
using Kingmaker.UnitLogic;
using Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using Kingmaker.Designers;
using Kingmaker.Blueprints.Classes.Spells;

namespace EbonsContentMod.Components
{
    internal class StealKiDiseaseSave : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            int monklevel = Owner.Progression.GetClassLevel(CharacterClassRefs.MonkClass.Reference);

            if ((evt.AttackRoll.IsCriticalConfirmed ||
                evt.Target.HPLeft <= 0) &&
                monklevel >= 11 &&
                Owner.Descriptor.Resources.GetResource(AbilityResourceRefs.KiPowerResource.Reference).Amount > 0 && // Must have at least one ki
                Owner.HasFact(BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("8564EBBA-71A1-47EC-A0B1-B1CB7ED819A7"))) // Has Life From A Stone
            {
                Game.Instance.Rulebook.TriggerEvent<RuleDispelMagic>(new RuleDispelMagic(Owner, Owner, null, RuleDispelMagic.CheckType.DC, Kingmaker.EntitySystem.Stats.StatType.Wisdom));
            }

            else if ((evt.AttackRoll.IsCriticalConfirmed ||
                evt.Target.HPLeft <= 0) &&
                monklevel >= 11 &&
                Owner.Descriptor.Resources.GetResource(AbilityResourceRefs.KiPowerResource.Reference).Amount > 0 && // Must have at least one ki
                !evt.Target.HasFact(BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("734a29b693e9ec346ba2951b27987e33")) && // Not Undead
                !evt.Target.HasFact(BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("fd389783027d63343b4a5634bd81645f"))) // Not Construct
            {
                Game.Instance.Rulebook.TriggerEvent<RuleHealDamage>(new RuleHealDamage(Owner, Owner, DiceFormula.Zero, monklevel));
            }
        }
    }
}
