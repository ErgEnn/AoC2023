﻿namespace AoC.Util
{
    public static class IntegerExtension
    {
        public static bool IsBetween(this int val, int minInclusive, int maxInclusive)
        {
            return val >= minInclusive && val <= maxInclusive;
        }

        public static bool IsBetween(this long val, long min, long max)
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

        public static Range ToAbs(this Range rng)
        {
            if (rng.Start.Value > rng.End.Value)
                return rng.End..rng.Start;
            return rng;
        }
    }
}
