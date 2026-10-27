
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;
using RimWorld;
using Verse.AI;

namespace Sunshower
{
    // sunshower mental break protection

    [HarmonyPatch(typeof(MentalBreakWorker), "TryStart")]
    public static class MentalBreakProtector
    {
        [HarmonyPrefix]
        public static bool Patch(Pawn __0)
        {
            
            if (__0.health.hediffSet.HasHediff(SunshowerDefOf.Sunshower_BreakProtection))
            {
                if (!__0.health.hediffSet.HasHediff(SunshowerDefOf.Sunshower_BreakProtectionCooldown))
                {
                    Hediff hed = __0.health.AddHediff(SunshowerDefOf.Sunshower_BreakProtectionCooldown);
                    hed.Severity = 1.0f;
                    return false;
                }
            }
            return true;
        }
    }


    // on hit sinking application / consumption / umbrella bs

    // this is like a really shitty practice. good way would be to make a proper damage type and apply it to the weapon.
    [HarmonyPatch(typeof(Verb_MeleeAttackDamage), "ApplyMeleeDamageToTarget")]
    public static class MeleeDamageChecker
    {

        [HarmonyPostfix]
        public static void Patch(Verb_MeleeAttackDamage __instance, LocalTargetInfo target, ref DamageWorker.DamageResult __result)
        {
            
            Pawn attacker = __instance.CasterPawn;


            // nullcheck
            if (attacker == null || !target.HasThing) return;

            // check if the target is a pawn and not an object
            if (target.Thing is Pawn targetPawn)
            {
                // if pawn dead, return
                if (targetPawn.Dead) return;

                float attackerMood = attacker.needs?.mood?.CurLevel ?? 1f;

                ThingWithComps primary = attacker.equipment?.Primary;

                // check if attacker's weapon
                if (primary != null && primary.def == SunshowerDefOf.Sunshower_umbrella)
                {

                    // increase damage by mood (if modd exists)
                    if (targetPawn.needs?.mood != null)
                    {
                        float maxThreshold = targetPawn.GetStatValue(StatDefOf.MentalBreakThreshold);
                        float mood = targetPawn.needs.mood.CurLevel;

                        float multiplier = Utility.getDamageIncrease(maxThreshold * 1.5f, mood);
                        __result.totalDamageDealt *= multiplier;
                    }

                    // sinkin application
                    float sinkingToApply = 0.1f;

                    if (attacker.equipment.AllEquipmentListForReading.Any(thing => thing.def == SunshowerDefOf.Sunshower_Raincoat))
                    {
                        sinkingToApply = sinkingToApply / Math.Max(attackerMood, 0.1f);
                    }

                    Utility.addOrIncreaseSeverity(targetPawn, SunshowerDefOf.Sunshower_Sinking, sinkingToApply);
                    Utility.addOrIncreaseSeverity(attacker, SunshowerDefOf.Sunshower_Sinking, sinkingToApply / 2f);
                }

                // sinking damage
                Utility.applyMoraleDamage(targetPawn);
            }
        }
    }
}
