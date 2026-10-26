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
    [HarmonyPatch (typeof(Verb_MeleeAttackDamage), "ApplyMeleeDamageToTarget")]
    public static class MeleeDamageChecker
    {

        [HarmonyPostfix]
        public static void Patch(Verb_MeleeAttackDamage __instance, LocalTargetInfo target, ref DamageWorker.DamageResult __result)
        {
            Pawn attacker = __instance.CasterPawn;

            if (attacker.equipment.Primary.def == SunshowerDefOf.Sunshower_umbrella)
            {
                if(target.Thing is Pawn)
                {
                    if (target.Pawn.Dead) return;
                    float maxThreshold = target.Pawn.GetStatValue(StatDefOf.MentalBreakThreshold);
                    float mood = target.Pawn.needs.mood.CurLevel;
                    
                    float multiplier = Utility.getDamageIncrease(maxThreshold*1.5f, mood);
                    __result.totalDamageDealt *= multiplier;
                    Utility.addOrIncreaseSeverity(target.Pawn, SunshowerDefOf.Sunshower_Sinking);

                    /*
                     the ultra cool game design decision here:
                        sunshower umbrella does both jack shit damage and jack shit sinking.
                        damage is increased on lower morale pawns (up to 2x)
                        to actually stack sinking the pawn gotta apply a shit ton of sinking at once (since it consumes 0.2 on hit, while base sunshower applies 0.1 sinking. )
                        this esentially means that it will deal morale damage, but it wont be able to stack + won't do a lot.

                        to increase sinking stacking the pawn gotta have the sunshower outfit which increases the
                        sinking stack based on the pawn's own emotion.
                     
                     */
                    Utility.applyMoraleDamage(target.Pawn);
                }
            }
        }
    }
}
