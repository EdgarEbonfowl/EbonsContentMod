using JetBrains.Annotations;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.Blueprints.Root.Strings;
using Kingmaker.Blueprints;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.UnitLogic.Class.LevelUp;
using Kingmaker.UnitLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Serialization;
using UnityEngine;

namespace EbonsContentMod.Components
{
    [AllowMultipleComponents]
    [TypeId("fb876f3084f01754db3a4acbbb328a89")]
    public class PrerequisiteSex : Prerequisite
    {
        public override bool CheckInternal(FeatureSelectionState selectionState, UnitDescriptor unit, LevelUpState state)
        {
            if (this.Not)
            {
                if (unit.Gender != gender)
                return true;
            }
            else
            {
                if (unit.Gender == gender)
                    return true;
            }
            return false;
        }

        public override string GetUITextInternal(UnitDescriptor unit)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(string.Format("{0} {1}: {2}", this.gender, UIStrings.Instance.Tooltips.Level, unit.Gender));
            if (unit != null)
            {
                stringBuilder.Append("\n");
                stringBuilder.Append(string.Format(UIStrings.Instance.Tooltips.CurrentValue, unit.Gender));
            }
            return stringBuilder.ToString();
        }

        [SerializeField]
        public Gender gender;

        // Token: 0x0400755A RID: 30042
        public bool Not = false;
    }
}
