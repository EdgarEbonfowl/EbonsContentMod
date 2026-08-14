using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules.Abilities;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using System.Linq;

namespace EbonsContentMod.Components
{
    [AllowedOn(typeof(BlueprintUnitFact), false)]
    [AllowMultipleComponents]
    [TypeId("7D9B98E1-64AD-4B67-AB56-8177A589C255")]
    public class ReplaceCasterLevelFromBuffCaster :
        UnitFactComponentDelegate,
        IInitiatorRulebookHandler<RuleCalculateAbilityParams>,
        IRulebookHandler<RuleCalculateAbilityParams>,
        ISubscriber,
        IInitiatorRulebookSubscriber
    {
        public BlueprintBuffReference m_SourceBuff;

        public BlueprintAbilityReference[] m_Spells;

        public int Divisor = 1;

        public BlueprintBuff SourceBuff =>
            m_SourceBuff?.Get();

        public void OnEventAboutToTrigger(
            RuleCalculateAbilityParams evt)
        {
            if (evt.Spell == null
                || m_Spells == null
                || !m_Spells.Any(spell =>
                    spell?.Get() == evt.Spell))
            {
                return;
            }

            Buff sourceBuff =
                Owner.GetFact(SourceBuff) as Buff;

            if (sourceBuff?.Context == null)
                return;

            int casterLevel =
                sourceBuff.Context.Params.CasterLevel;

            evt.ReplaceCasterLevel =
                casterLevel / Divisor;
        }

        public void OnEventDidTrigger(
            RuleCalculateAbilityParams evt)
        {
        }
    }
}