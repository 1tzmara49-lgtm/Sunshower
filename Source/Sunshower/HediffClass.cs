using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Sunshower
{
    
    public class SunshowerHediffClass : HediffWithComps
    {
        private int ticker = 0;
        public override void Tick()
        {
            base.Tick();
            ticker++;
            if (ticker == 250)
            {
                ticker = 0;
                Pawn pawn = this.pawn;

                Hediff sinkingHediff = pawn?.health?.hediffSet.GetFirstHediffOfDef(SunshowerDefOf.Sunshower_Sinking);

                if (sinkingHediff == null) return;

                this.Severity = sinkingHediff.Severity;
                Log.Message("Ticking. Current stage");
                Log.Message(this.Severity);
                Log.Message(sinkingHediff.Severity);
                
            }
        }
    }
}
