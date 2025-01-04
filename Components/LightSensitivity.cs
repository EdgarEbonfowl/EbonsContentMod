using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Parts;
using Kingmaker.Utility;
using Kingmaker.UnitLogic;
using Kingmaker;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using TabletopTweaks.Core.Utilities;
using Kingmaker.Designers;
using Owlcat.Runtime.Visual.Effects.WeatherSystem;
using Kingmaker.Controllers;
using Kingmaker.Blueprints.Classes;

namespace EbonsContentMod.Components
{
    internal class LightSensitivity : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, ITimeOfDayChangedHandler, IWeatherChangeHandler, IAreaHandler, ISubscriber, IInitiatorRulebookSubscriber
    {
        internal BlueprintBuff SensitivityBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("{FCA0ADBC-EB20-40D9-A854-0F7AAC45B659}");
        internal BlueprintFeature SensitivityImmunityFeature = BlueprintTools.GetBlueprint<BlueprintFeature>("{4EA922BA-F88E-4B8E-A378-50C3D0E2DA2E}");

        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            // This should capture area changes?
            if (GetIsBlinding()) GameHelper.ApplyBuff(Owner, SensitivityBuff);
            else GameHelper.RemoveBuff(Owner, SensitivityBuff);
        }

        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
        }

        public void OnTimeOfDayChanged()
        {
            // This should capture time of day changes
            if (GetIsBlinding()) GameHelper.ApplyBuff(Owner, SensitivityBuff);
            else GameHelper.RemoveBuff(Owner, SensitivityBuff);
        }

        public void OnWeatherChange()
        {
            // This should capture weather changes
            if (GetIsBlinding()) GameHelper.ApplyBuff(Owner, SensitivityBuff);
            else GameHelper.RemoveBuff(Owner, SensitivityBuff);
        }

        public void OnAreaBeginUnloading()
        {
        }

        public void OnAreaDidLoad()
        {
            // This should capture area changes if the above bit doesn't
            if (GetIsBlinding()) GameHelper.ApplyBuff(Owner, SensitivityBuff);
            else GameHelper.RemoveBuff(Owner, SensitivityBuff);
        }

        public bool GetIsBlinding()
        {
            var RainModerateBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("f37b708de9eeb2c4ab248d79bb5b5aa7");
            var SnowModerateBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("845332298344c6447972dc9b131add08");
            var RainStormBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("7c260a8970e273d439f2a2e19b7196af");
            var RainHeavyBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("5c315bec0240479d9fafcc65b9efb574");
            var SnowHeavyBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("4a15ab872f11463da1c1265d5b4324ad");

            // Immune to Light Sensitivity now
            if (Owner.HasFact(SensitivityImmunityFeature)) return false;
            
            // Indoors or outdoors
            if (AreaService.Instance.CurrentAreaPart.IsIndoor) return false;

            // Time of day
            if (Game.Instance.TimeOfDay == Kingmaker.AreaLogic.TimeOfDay.Evening || Game.Instance.TimeOfDay == Kingmaker.AreaLogic.TimeOfDay.Night) return false;

            // Global map
            if (Game.Instance.CurrentlyLoadedArea.IsGlobalMap) return false;

            // Area
            if (AreaService.Instance.CurrentAreaSetting == AreaSetting.Abyss ||
                AreaService.Instance.CurrentAreaSetting == AreaSetting.Forest ||
                AreaService.Instance.CurrentAreaSetting == AreaSetting.Underground ||
                AreaService.Instance.CurrentAreaSetting == AreaSetting.None)
                return false;

            // Weather
            if (WeatherController.Instance.m_WeatherInclemencyController.ActualInclemency == InclemencyType.Moderate ||
                WeatherController.Instance.m_WeatherInclemencyController.ActualInclemency == InclemencyType.Heavy ||
                WeatherController.Instance.m_WeatherInclemencyController.ActualInclemency == InclemencyType.Storm)
                return false;

            // Maybe not needed
            if (Owner.Buffs.GetBuff(RainModerateBuff) != null ||
                Owner.Buffs.GetBuff(SnowModerateBuff) != null ||
                Owner.Buffs.GetBuff(RainStormBuff) != null ||
                Owner.Buffs.GetBuff(RainHeavyBuff) != null ||
                Owner.Buffs.GetBuff(SnowHeavyBuff) != null)
                return false;

            return true;
        }
    }
}
