using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Util
{
    public static class EnumerableExtensions
    {
        public static int Multiply(this IEnumerable<int> values)
        {
            var result = 0;
            var enumerator = values.GetEnumerator();
            if(enumerator.MoveNext())
                result = enumerator.Current;
            while(enumerator.MoveNext())
                result *= enumerator.Current;
            enumerator.Dispose();
            return result;
        }

        public static long Multiply(this IEnumerable<long> values)
        {
            long result = 0;
            var enumerator = values.GetEnumerator();
            if(enumerator.MoveNext())
                result = enumerator.Current;
            while(enumerator.MoveNext())
                result *= enumerator.Current;
            enumerator.Dispose();
            return result;
        }

        public static (T? i1, T? i2) FirstMultisearch<T>(this IEnumerable<T> values, Predicate<T> p1, Predicate<T> p2) where T : class
        {
            var i1 = default(T);
            var i2 = default(T);

            foreach (var item in values)
            {
                if (i1 == default(T) && p1(item))
                {
                    i1 = item;
                    if (i2 != default(T))
                        break;
                }
                if (i2 == default(T) && p2(item))
                {
                    i2 = item;
                    if (i1 != default(T))
                        break;
                }
            }

            return (i1, i2);
        }

        public static IEnumerable<LinkedListNode<T>> AsNodes<T>(this LinkedList<T> list, bool ignoreLast = false)
        {
            for (LinkedListNode<T> node = list.First; node != null; node = node.Next)
            {
                if(ignoreLast && node.Next == null)
                    yield break;
                yield return node;
            }
        }

        public static LinkedList<T> ToLinkedList<T>(this IDictionary<T, T> dict, T start) where T : class
        {
            var ll = new LinkedList<T>();
            ll.AddFirst(start);
            var current = dict[start];
            while (current != null)
            {
                ll.AddLast(current);
                current = dict.TryGetValue(current, out var newCurrent) ? newCurrent : null;
            }
            return ll;
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> values, params T[] additionalValues)
        {
            foreach (var value in values.Concat(additionalValues.AsEnumerable()))
            {
                yield return value;
            }
        }

        public static IEnumerable<(int i, T value)> Indexed<T>(this IEnumerable<T> values)
        {
            int i = 0;
            var iter = values.GetEnumerator();
            while (iter.MoveNext())
            {
                yield return (i++, iter.Current);
            }
            iter.Dispose();
        }

        public static IEnumerable<int> StepTowards(this int start, int end)
        {
            var delta = Math.Abs(start - end);
            var coefficient = 1;
            if (start - end > 0)
            {
                coefficient = -1;
            }
            for (int i = 0; i <= delta; i++)
            {
                yield return start + coefficient * i;
            }
        }

        public static IEnumerable<(T i1, T i2)> Pairwise<T>(this IEnumerable<T> values, bool overlapping = true, bool loop = false)
        {
            using var enumerator = values.GetEnumerator();
            enumerator.MoveNext();
            var firstAssigned = false;
            var first = default(T);
            while (true)
            {
                var i1 = enumerator.Current;
                if (!firstAssigned)
                {
                    first = i1;
                    firstAssigned = true;
                }

                if (!enumerator.MoveNext())
                {
                    if (loop)
                        yield return (i1, first);
                    yield break;
                }
                var i2 = enumerator.Current;
                yield return (i1, i2);
                if (!overlapping && !enumerator.MoveNext())
                    yield break;
            }
        }

        public static T RandomItem<T>(this ICollection<T> items)
        {
            return items.Skip(Random.Shared.Next(items.Count)).First();
        }

        public static string Join(this IEnumerable<char> chars, string separator)
        {
            return string.Join(separator, chars);
        }
        public static string Join(this IEnumerable<string> chars, string separator)
        {
            return string.Join(separator, chars);
        }

        // LCM(2,3) = 6, LCM(2,3,4) = 12
        public static long LCM(this IEnumerable<long> nums)
        {
            return nums.Aggregate((x, y) => Math.Abs(x * y) / GCD(x, y));
        }

        // GCD(24, 18) = 6
        private static long GCD(long x, long y)
        {
            while (y != 0)
            {
                long temp = y;
                y = x % y;
                x = temp;
            }

            return x;
        }

        public static IEnumerable<int> ToInts(this IEnumerable<string> numbers)
        {
            return numbers.Select(s => s.ToInt());
        }

        public static string Dump<T>(this IEnumerable<T> items)
        {
            return string.Join(',', items);
        }

        public static int IndexOf<T>(this T[] arr, T val)
        {
            return Array.IndexOf(arr, val);
        }

        public static IEnumerable<(T, T)> AllPairs<T>(this IReadOnlyList<T> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                for (int j = i+1; j < items.Count; j++)
                {
                    yield return (items[i], items[j]);
                }
            }
        }

        public static T? Pop<T>(this IEnumerator<T> enumerator) where T: class
        {
            if (enumerator.MoveNext())
                return enumerator.Current;
            return null;
        }

        public static char? Pop(this CharEnumerator enumerator)
        {
            if (enumerator.MoveNext())
                return enumerator.Current;
            return null;
        }

        public static IEnumerable<T> Print<T>(this IEnumerable<T> enumerable)
        {
            return enumerable.Select(arg =>
            {
                Console.WriteLine(arg);
                return arg;
            });
        }

        public static TStorage? SelectWithStorage<T, TStorage>(this IEnumerable<T> enumerable, Func<TStorage, T, TStorage> func)
        {
            TStorage storage = default;
            foreach (var arg in enumerable)
            {
                storage = func(storage, arg);
            }

            return storage;
        }
    }
}
