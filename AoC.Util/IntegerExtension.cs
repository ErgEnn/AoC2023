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
    }
}
