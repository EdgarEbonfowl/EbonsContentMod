using BlueprintCore.Blueprints.References;
using Kingmaker.Blueprints;
using Kingmaker.Designers;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Damage;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.UnitLogic;
using Kingmaker.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.UI.GridLayoutGroup;
using TabletopTweaks.Core.Utilities;
using Kingmaker.Blueprints.Items.Weapons;

namespace EbonsContentMod.Components
{
    internal class KuruBiteTempHP : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleAttackWithWeapon>, IRulebookHandler<RuleAttackWithWeapon>, ISubscriber, IInitiatorRulebookSubscriber
    {
        void IRulebookHandler<RuleAttackWithWeapon>.OnEventAboutToTrigger(RuleAttackWithWeapon evt)
        {
        }

        void IRulebookHandler<RuleAttackWithWeapon>.OnEventDidTrigger(RuleAttackWithWeapon evt)
        {
            var buff = BlueprintTools.GetBlueprint<BlueprintBuff>("{DA56164F-52F6-448E-BF3B-972911432762}"); // Kuru Bite Temp HP Buff
            var bite = BlueprintTools.GetBlueprint<BlueprintItemWeapon>("{EBD45E84-C7F5-4B3A-AAE1-B3524FC68B20}"); // Kuru bite weapon blueprint

            if (evt.AttackRoll.IsHit && 
                !evt.Target.HasFact(BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("734a29b693e9ec346ba2951b27987e33")) && // Not undead or construct
                !evt.Target.HasFact(BlueprintTools.GetBlueprintReference<BlueprintUnitFactReference>("fd389783027d63343b4a5634bd81645f")) &&
                evt.Weapon.Blueprint == bite)
            {
                GameHelper.ApplyBuff(Owner, buff, 600.Rounds());
            }
        }
    }
}
