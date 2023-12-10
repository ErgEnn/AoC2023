namespace AoC.Util
{
    public static class TupleExtensions
    {
        public static bool IsIn(this (int, int) point, int min, int max)
        {
            return point.Item1 >= min && point.Item1 <= max && point.Item2 >= min && point.Item2 <= max;
        }

        public static (int i1, int i2) Add(this (int, int) tuple1, (int, int) tuple2)
        {
            return (tuple1.Item1 + tuple2.Item1, tuple1.Item2 + tuple2.Item2);
        }

        public static (int i1, int i2, int i3) Add(this (int, int, int) tuple1, (int, int, int) tuple2)
        {
            return (tuple1.Item1 + tuple2.Item1, tuple1.Item2 + tuple2.Item2, tuple1.Item3 + tuple2.Item3);
        }

        public static (int i1, int i2, int i3) Subtract(this (int, int, int) tuple1, (int, int, int) tuple2)
        {
            return (tuple1.Item1 - tuple2.Item1, tuple1.Item2 - tuple2.Item2, tuple1.Item3 - tuple2.Item3);
        }

        public static bool AnyGreaterThan(this (int, int, int) tuple1, (int, int, int) tuple2)
        {
            return tuple1.Item1 > tuple2.Item1 ||  tuple1.Item2 > tuple2.Item2 || tuple1.Item3 > tuple2.Item3;
        }

        public static bool AnyLessThan(this (int, int, int) tuple1, (int, int, int) tuple2)
        {
            return tuple1.Item1 < tuple2.Item1 ||  tuple1.Item2 < tuple2.Item2 || tuple1.Item3 < tuple2.Item3;
        }
    }
}
