using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Sunshower
{
    [DefOf]
    public static class SunshowerDefOf
    {
        public static ThingDef Sunshower_umbrella;
        public static ThingDef Sunshower_Raincoat;
        public static HediffDef Sunshower_Sinking;

        static SunshowerDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(SunshowerDefOf));
        }
    }
}
