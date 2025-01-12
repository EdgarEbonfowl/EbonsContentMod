using Kingmaker.Blueprints.Area;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Controllers;
using Kingmaker.Designers;
using Kingmaker.Enums;
using Kingmaker.PubSubSystem;
using Kingmaker.RuleSystem.Rules;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic;
using Kingmaker;
using Owlcat.Runtime.Visual.Effects.WeatherSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.UI.GridLayoutGroup;
using TabletopTweaks.Core.Utilities;

namespace EbonsContentMod.Components
{
    internal class Nocturnal : UnitFactComponentDelegate, IInitiatorRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, IRulebookHandler<RuleCalculateAttackBonusWithoutTarget>, ITimeOfDayChangedHandler, IWeatherChangeHandler, IAreaHandler, ISubscriber, IInitiatorRulebookSubscriber
    {
        internal BlueprintBuff NocturnalBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("{C1A8AB56-409D-4DC6-8D64-D191D38CCC09}");

        public void OnEventAboutToTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
            // This should capture area changes?
            if (GetIsDark()) GameHelper.ApplyBuff(Owner, NocturnalBuff);
            else GameHelper.RemoveBuff(Owner, NocturnalBuff);
        }

        public void OnEventDidTrigger(RuleCalculateAttackBonusWithoutTarget evt)
        {
        }

        public void OnTimeOfDayChanged()
        {
            // This should capture time of day changes
            if (GetIsDark()) GameHelper.ApplyBuff(Owner, NocturnalBuff);
            else GameHelper.RemoveBuff(Owner, NocturnalBuff);
        }

        public void OnWeatherChange()
        {
            // This should capture weather changes
            if (GetIsDark()) GameHelper.ApplyBuff(Owner, NocturnalBuff);
            else GameHelper.RemoveBuff(Owner, NocturnalBuff);
        }

        public void OnAreaBeginUnloading()
        {
        }

        public void OnAreaDidLoad()
        {
            // This should capture area changes if the above bit doesn't
            if (GetIsDark()) GameHelper.ApplyBuff(Owner, NocturnalBuff);
            else GameHelper.RemoveBuff(Owner, NocturnalBuff);
        }

        public bool GetIsDark()
        {
            var RainModerateBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("f37b708de9eeb2c4ab248d79bb5b5aa7");
            var SnowModerateBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("845332298344c6447972dc9b131add08");
            var RainStormBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("7c260a8970e273d439f2a2e19b7196af");
            var RainHeavyBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("5c315bec0240479d9fafcc65b9efb574");
            var SnowHeavyBuff = BlueprintTools.GetBlueprint<BlueprintBuff>("4a15ab872f11463da1c1265d5b4324ad");

            // Indoors or outdoors
            if (AreaService.Instance.CurrentAreaPart.IsIndoor) return true;

            // Time of day
            if (Game.Instance.TimeOfDay == Kingmaker.AreaLogic.TimeOfDay.Evening || Game.Instance.TimeOfDay == Kingmaker.AreaLogic.TimeOfDay.Night) return true;

            // Global map
            if (Game.Instance.CurrentlyLoadedArea.IsGlobalMap) return false;

            // Area
            if (AreaService.Instance.CurrentAreaSetting == AreaSetting.Abyss ||
                AreaService.Instance.CurrentAreaSetting == AreaSetting.Forest ||
                AreaService.Instance.CurrentAreaSetting == AreaSetting.Underground ||
                AreaService.Instance.CurrentAreaSetting == AreaSetting.None)
                return true;

            // Weather
            if (WeatherController.Instance.m_WeatherInclemencyController.ActualInclemency == InclemencyType.Moderate ||
                WeatherController.Instance.m_WeatherInclemencyController.ActualInclemency == InclemencyType.Heavy ||
                WeatherController.Instance.m_WeatherInclemencyController.ActualInclemency == InclemencyType.Storm)
                return true;

            // Maybe not needed
            if (Owner.Buffs.GetBuff(RainModerateBuff) != null ||
                Owner.Buffs.GetBuff(SnowModerateBuff) != null ||
                Owner.Buffs.GetBuff(RainStormBuff) != null ||
                Owner.Buffs.GetBuff(RainHeavyBuff) != null ||
                Owner.Buffs.GetBuff(SnowHeavyBuff) != null)
                return true;

            return false;
        }
    }
}
