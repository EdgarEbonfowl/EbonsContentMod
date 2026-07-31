using BlueprintCore.Actions.Builder;
using BlueprintCore.Actions.Builder.ContextEx;
using BlueprintCore.Conditions.Builder.ContextEx;
using BlueprintCore.Blueprints.Configurators.UnitLogic.ActivatableAbilities;
using BlueprintCore.Blueprints.CustomConfigurators;
using BlueprintCore.Blueprints.CustomConfigurators.UnitLogic.Buffs;
using BlueprintCore.Conditions.Builder;
using BlueprintCore.Blueprints.References;
using BlueprintCore.Utils.Types;
using BlueprintCore.Utils;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.UnitLogic.ActivatableAbilities.Restrictions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueprintCore.Blueprints.CustomConfigurators.Classes;
using Kingmaker.Blueprints;
using Kingmaker.UnitLogic.ActivatableAbilities;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using HarmonyLib;
using EbonsContentMod.UnitParts;
using Kingmaker.UnitLogic.Commands;
using Kingmaker.UnitLogic.Commands.Base;
using Kingmaker.Utility;
using Owlcat.Runtime.UniRx;
using Kingmaker.UnitLogic;

namespace EbonsContentMod.Abilities
{
    internal class ComeAndGetMe
    {
        internal static void Configure()
		{
            ActivatableAbilityConfigurator.For(ActivatableAbilityRefs.ComeAndGetMeToggleAbility)
				.RemoveComponents(c => c is RestrictionHasFact)
				.SetDoNotTurnOffOnRest()
				.Configure();

			BuffConfigurator.For(BuffRefs.ComeAndGetMeSwitchBuff)
				.AddFactContextActions(
					activated:
						ActionsBuilder.New()
							.Conditional(
								ConditionsBuilder.New().HasFact(BuffRefs.InspiredRageEffectBuffMythic.ToString()).HasFact(BuffRefs.InspiredRageEffectBuff.ToString()).UseOr(),
								ifTrue: ActionsBuilder.New().ApplyBuffPermanent(BuffRefs.ComeAndGetMeEffectBuff.ToString(), isNotDispelable: true)))
				.Configure();

            BuffConfigurator
                .For(BuffRefs.InspiredRageEffectBuff)
                .AddComponent<
                    Components.RestoreComeAndGetMeToggle>()
                .Configure();

            BuffConfigurator
                .For(BuffRefs.InspiredRageEffectBuffMythic)
                .AddComponent<
                    Components.RestoreComeAndGetMeToggle>()
                .Configure();
        }
    }

    [HarmonyPatch(typeof(ActivatableAbility))]
    internal static class ComeAndGetMeToggleStatePatch
    {
        private static bool IsComeAndGetMeToggle(
            ActivatableAbility ability)
        {
            return ability?.Blueprint ==
                ActivatableAbilityRefs
                    .ComeAndGetMeToggleAbility
                    .Reference
                    .Get();
        }

        // Save the state only after the player has issued the command to activate or deactivate this specific toggle.

        // This does not run when the fact or its applied buff is automatically removed because the unit leaves Inspired Rage.

        [HarmonyPatch(typeof(ActivatableAbility), "SetIsOn")]
        internal static class ComeAndGetMeSetIsOnPatch
        {
            [HarmonyPostfix]
            private static void Postfix(
                ActivatableAbility __instance,
                bool value)
            {
                if (!Main.Settings.ComeAndGetMe) return;

                if (__instance?.Blueprint !=
                    ActivatableAbilityRefs
                        .ComeAndGetMeToggleAbility
                        .Reference
                        .Get())
                {
                    return;
                }

                var owner = __instance.Owner;
                if (owner == null)
                {
                    return;
                }

                bool actualState = __instance.IsOn;

                bool hasInspiredRage =
                    owner.HasFact(
                        BuffRefs.InspiredRageEffectBuff.Reference.Get()) ||
                    owner.HasFact(
                        BuffRefs.InspiredRageEffectBuffMythic.Reference.Get());

                Main.log.Log(
                    $"Come and Get Me: SetIsOn for {owner.CharacterName}; " +
                    $"requested={value}, actual={actualState}, " +
                    $"started={__instance.IsStarted}, " +
                    $"inspiredRage={hasInspiredRage}");

                /*
                 * Turning on is always an intentional preference worth remembering.
                 *
                 * Save false only while Inspired Rage is still present. When the
                 * character runs out of the AOE, Inspired Rage disappears and the
                 * game automatically switches this ability off. That automatic
                 * shutdown must not overwrite the saved preference.
                 */
                if (actualState || hasInspiredRage)
                {
                    owner.Ensure<UnitPartComeAndGetMeToggleState>()
                        .SaveState(actualState);

                    Main.log.Log(
                        $"Come and Get Me: saved preference={actualState} " +
                        $"for {owner.CharacterName}");
                }
                else
                {
                    Main.log.Log(
                        $"Come and Get Me: ignored automatic shutdown " +
                        $"for {owner.CharacterName}");
                }
            }
        }

        // Restore the last player-selected state when AddFacts creates a new ActivatableAbility instance.

        [HarmonyPatch("OnActivate")]
        [HarmonyPostfix]
        private static void OnActivate_Postfix(
            ActivatableAbility __instance)
        {
            if (!Main.Settings.ComeAndGetMe) return;

            if (!IsComeAndGetMeToggle(__instance))
            {
                return;
            }

            var owner = __instance.Owner;
            if (owner == null)
            {
                return;
            }

            var state = owner.Get<UnitPartComeAndGetMeToggleState>();

            Main.log.Log(
                $"Come and Get Me: toggle added for {owner.CharacterName}; " +
                $"current={__instance.IsOn}, " +
                $"hasSavedState={state?.HasSavedState ?? false}, " +
                $"saved={state?.ShouldBeActive ?? false}");

            if (state == null || !state.HasSavedState)
            {
                return;
            }

            bool desiredState = state.ShouldBeActive;

            /*
             * OnActivate fires while AddFacts is still creating/initializing
             * the ActivatableAbility. Restore after that process finishes.
             */
            DelayedInvoker.InvokeInTime(
                () =>
            {
                if (__instance.Owner == null)
                {
                    Main.log.Log(
                        "Come and Get Me: delayed restore aborted; owner is null.");
                    return;
                }

                /*
                    * Make sure this exact fact instance still belongs to the unit.
                    * It could theoretically have been removed again before this runs.
                    */
                if (!__instance.Active)
                {
                    Main.log.Log(
                        $"Come and Get Me: delayed restore aborted for " +
                        $"{owner.CharacterName}; fact is no longer active.");
                    return;
                }

                Main.log.Log(
                    $"Come and Get Me: delayed restore beginning for " +
                    $"{owner.CharacterName}; current={__instance.IsOn}, " +
                    $"desired={desiredState}, started={__instance.IsStarted}");

                if (__instance.IsOn != desiredState)
                {
                    __instance.IsOn = desiredState;
                }

                Main.log.Log(
                    $"Come and Get Me: delayed restore finished for " +
                    $"{owner.CharacterName}; current={__instance.IsOn}, " +
                    $"started={__instance.IsStarted}, " +
                    $"buff={__instance.AppliedBuff?.Blueprint?.name ?? "null"}");
            },
            0.01f);

            var hasNormalInspiredRage =
                owner.Buffs.HasFact(
                    BuffRefs.InspiredRageEffectBuff.Reference.Get());

            var hasMythicInspiredRage =
                owner.Buffs.HasFact(
                    BuffRefs.InspiredRageEffectBuffMythic.Reference.Get());

            var hasComeAndGetMeEffect =
                owner.Buffs.HasFact(
                    BuffRefs.ComeAndGetMeEffectBuff.Reference.Get());

            Main.log.Log(
                $"Come and Get Me: post-restore facts for {owner.CharacterName}; " +
                $"normalRage={hasNormalInspiredRage}, " +
                $"mythicRage={hasMythicInspiredRage}, " +
                $"effect={hasComeAndGetMeEffect}");
        }
    }
}
