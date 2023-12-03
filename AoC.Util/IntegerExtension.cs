using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Util
{
    public static class IntegerExtension
    {
        public static bool IsBetween(this int val, int min, int max)
        {
            return val >= min && val <= max;
        }

        public static string ToFormattedString(this int[] values)
        {
            return string.Join(",", values);
        }

        public static int ClampMin(this int i, int min)
        {
            return Math.Max(i, min);
        }

        public static int ClampMax(this int i, int max)
        {
            return Math.Min(i, max);
        }
    }
}
