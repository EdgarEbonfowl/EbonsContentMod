using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using BlueprintCore.Blueprints.CustomConfigurators.Classes.Selection;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Abilities;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils;
using BlueprintCore.Utils.Types;
using Kingmaker.UnitLogic.Mechanics.Components;
using EbonsContentMod.Components;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Craft;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Abilities.Components;
using Kingmaker.UnitLogic.FactLogic;
using Kingmaker.UnitLogic.Mechanics;
using Kingmaker.UnitLogic.Commands.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TabletopTweaks.Core.Utilities;
using TabletopTweaks.Core.NewComponents;
using static Kingmaker.Armies.TacticalCombat.Grid.TacticalCombatGrid;
using Kingmaker.UnitLogic.Buffs.Components;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using Kingmaker.Designers.Mechanics.Buffs;
using TabletopTweaks.Core.NewComponents.Prerequisites;
using EbonsContentMod.Utilities;

namespace EbonsContentMod.Bloodlines
{
    internal class NaniteSorcererBloodline
    {
        private static readonly string NaniteSorcererBloodlineName = "NaniteSorcererBloodline";

        internal const string NaniteSorcererBloodlineDisplayName = "NaniteSorcererBloodline.Name";
        private static readonly string NaniteSorcererBloodlineDescription = "NaniteSorcererBloodline.Description";
        public static readonly string NaniteSorcererBloodlineGuid = "{46BF24BD-CACB-499B-B118-48FACDCC0556}";

        internal static void Configure()
        {
            if (!CheckerUtilities.GetModActive("TabletopTweaks-Base")) return;

            var sorcererclass = CharacterClassRefs.SorcererClass.Reference.Get();
            var magusclass = CharacterClassRefs.MagusClass.Reference.Get();
            var eldritchscionarchetype = ArchetypeRefs.EldritchScionArchetype.Reference.Get();
        }
    }
}
