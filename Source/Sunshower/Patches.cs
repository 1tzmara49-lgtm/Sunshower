
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;
using RimWorld;

namespace Sunshower
{
    [HarmonyPatch(typeof(Verb_MeleeAttackDamage), "ApplyMeleeDamageToTarget")]
    public static class MeleeDamageChecker
    {

        [HarmonyPostfix]
        public static void Patch(Verb_MeleeAttackDamage __instance, LocalTargetInfo target, ref DamageWorker.DamageResult __result)
        {
            Pawn attacker = __instance.CasterPawn;

            if (attacker == null || !target.HasThing) return;

            if (target.Thing is Pawn targetPawn)
            {
                if (targetPawn.Dead) return;

                float attackerMood = attacker.needs?.mood?.CurLevel ?? 1f;

                ThingWithComps primary = attacker.equipment?.Primary;

                if (primary != null && primary.def == SunshowerDefOf.Sunshower_umbrella)
                {
                    if (targetPawn.needs?.mood != null)
                    {
                        float maxThreshold = targetPawn.GetStatValue(StatDefOf.MentalBreakThreshold);
                        float mood = targetPawn.needs.mood.CurLevel;

                        float multiplier = Utility.getDamageIncrease(maxThreshold * 1.5f, mood);
                        __result.totalDamageDealt *= multiplier;
                    }

                    float sinkingToApply = 0.1f;

                    if (attacker.equipment.AllEquipmentListForReading.Any(thing => thing.def == SunshowerDefOf.Sunshower_Raincoat))
                    {
                        sinkingToApply = sinkingToApply / Math.Max(attackerMood, 0.1f);
                    }

                    Utility.addOrIncreaseSeverity(targetPawn, SunshowerDefOf.Sunshower_Sinking, sinkingToApply);
                    Utility.addOrIncreaseSeverity(attacker, SunshowerDefOf.Sunshower_Sinking, sinkingToApply / 2f);
                }

                Utility.applyMoraleDamage(targetPawn);
            }
        }
    }
}
