using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.References;
using EbonsContentMod.UnitParts;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Components;
using Kingmaker.Utility;
using Owlcat.Runtime.UniRx;
using Kingmaker.UnitLogic;

namespace EbonsContentMod.Components
{
    internal class RestoreComeAndGetMeToggle :
        UnitBuffComponentDelegate
    {
        public override void OnActivate()
        {
            base.OnActivate();

            var owner = Owner;
            if (owner == null)
            {
                return;
            }

            var state =
                owner.Get<UnitPartComeAndGetMeToggleState>();

            Main.log.Log(
                $"Come and Get Me: Inspired Rage activated for " +
                $"{owner.CharacterName}; " +
                $"hasSavedState={state?.HasSavedState ?? false}, " +
                $"saved={state?.ShouldBeActive ?? false}");

            if (state == null ||
                !state.HasSavedState ||
                !state.ShouldBeActive)
            {
                return;
            }

            /*
             * Give AddFacts/AddFactsFromCaster time to restore or expose
             * the activatable ability before looking for it.
             */
            DelayedInvoker.InvokeInTime(
                () => Restore(owner),
                0.01f);
        }

        private static void Restore(
            Kingmaker.UnitLogic.UnitDescriptor owner)
        {
            var toggleBlueprint =
                ActivatableAbilityRefs
                    .ComeAndGetMeToggleAbility
                    .Reference
                    .Get();

            ActivatableAbility toggle =
                owner.ActivatableAbilities.Enumerable.FirstOrDefault(
                    a => a.Blueprint == toggleBlueprint);

            if (toggle == null)
            {
                Main.log.Log(
                    $"Come and Get Me: restoration failed for " +
                    $"{owner.CharacterName}; toggle fact not found.");

                return;
            }

            Main.log.Log(
                $"Come and Get Me: restoring for " +
                $"{owner.CharacterName}; " +
                $"current={toggle.IsOn}, " +
                $"started={toggle.IsStarted}, " +
                $"available={toggle.IsAvailable}");

            if (!toggle.IsOn)
            {
                toggle.IsOn = true;
            }

            /*
             * Normally setting IsOn starts or queues the ability. Calling
             * TryStart here handles cases where it remained on but stopped
             * when the previous Inspired Rage instance disappeared.
             */
            if (toggle.IsOn &&
                !toggle.IsStarted &&
                toggle.IsAvailable)
            {
                toggle.TryStart();
            }

            Main.log.Log(
                $"Come and Get Me: restore completed for " +
                $"{owner.CharacterName}; " +
                $"isOn={toggle.IsOn}, " +
                $"started={toggle.IsStarted}, " +
                $"buff={toggle.AppliedBuff?.Blueprint?.name ?? "null"}");
        }
    }
}
