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
        public static void addOrIncreaseSeverity(Pawn pawn, HediffDef hediffDef)
        {
            var hediffset = pawn?.health.hediffSet;

            if (hediffset == null)
            {
                return;
            }

            if (hediffset.HasHediff(hediffDef))
            {
                Hediff hediff = hediffset.GetFirstHediffOfDef(hediffDef);
                hediff.Severity += 0.1f;

            }
            else
            {
                pawn.health.AddHediff(hediffDef);
            }
        }

        public static float getDamageIncrease(float moodThreshold, float currentMood)
        {
            Log.Message("Threshold: "+moodThreshold);
            Log.Message("Mood: " + currentMood);
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
            var hediffset = pawn?.health.hediffSet;

            if (hediffset == null)
            {
                return;
            }

            HediffDef def = SunshowerDefOf.Sunshower_Sinking;

            if (hediffset.HasHediff(def))
            {
                Hediff hediff = hediffset.GetFirstHediffOfDef(def);

                float consumedSeverity = 0.2f;

                pawn.needs.mood.CurLevel -= hediff.Severity / 2;
                hediff.Severity -= consumedSeverity;
            }
        }
    }
}
