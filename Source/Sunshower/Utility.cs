using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Verse;

namespace Sunshower
{
    public class Utility
    {
        public static void addOrIncreaseSeverity(Pawn pawn, HediffDef hediffDef, float increase)
        {
            var hediffset = pawn?.health.hediffSet;

            if (hediffset == null)
            {
                return;
            }

            if (hediffset.HasHediff(hediffDef))
            {
                Hediff hediff = hediffset.GetFirstHediffOfDef(hediffDef);
                hediff.Severity += increase;

            }
            else
            {
                pawn.health.AddHediff(hediffDef);
            }
        }

        public static float getDamageIncrease(float moodThreshold, float currentMood)
        {
            if (currentMood == 0)
            {
                return 2.0f;
            }

            float percentage = currentMood / moodThreshold;
            
            if (percentage >= 1.0f)
            {
                return 0.5f;
            }

            float multiplier = 2.0f - (percentage * 1.5f);

            return multiplier;



        }


        public static void applyMoraleDamage(Pawn pawn)
        {
            var hediffset = pawn?.health?.hediffSet;

            if (hediffset == null)
            {
                return;
            }

            HediffDef def = SunshowerDefOf.Sunshower_Sinking;

            if (hediffset.HasHediff(def))
            {
                Hediff hediff = hediffset.GetFirstHediffOfDef(def);

                if (hediff == null)
                {
                    return;
                }

                float consumedSeverity = 0.2f;

                var mood = pawn.needs?.mood;

                if (mood != null)
                {
                    mood.CurLevel -= hediff.Severity / 2;
                }
                else
                {
                    DamageInfo dinfo = new DamageInfo(DamageDefOf.Blunt, armorPenetration:100f, amount:hediff.Severity/2);
                    pawn.TakeDamage(dinfo);
                }

                hediff.Severity -= consumedSeverity;
            }
        }

    }
}
