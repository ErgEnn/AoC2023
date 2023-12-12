
using System.Text.RegularExpressions;

namespace AoC.Util
{
    public static class StringExtensions
    {

        public static string[] Lines(this string s)
        {
            return s.Split(Environment.NewLine);
        }

        private static readonly Dictionary<string, Regex> _regexCache = new();

        private static void InitializeRegex(FormattableString pattern)
        {
            if (!_regexCache.ContainsKey(pattern.Format))
            {
                var regexPattern = pattern.Format;
                for (int i = 0; i < pattern.ArgumentCount; i++)
                {
                    regexPattern = regexPattern.Replace("{" + i + "}", "(.+)");
                }
                _regexCache.Add(pattern.Format, new Regex(regexPattern));
            }
        }

        public static (T1 s1, T2 s2) Deconstruct<T1, T2>(this string s, string separator = " ", string innerSeparator = ",")
        {
            return s.Split(separator) switch
            {
                [var s1, var s2] => (Convert<T1>(s1, innerSeparator), Convert<T2>(s2, innerSeparator)),
            };
        }

        public static (T1 s1, T2 s2) Deconstruct<T1, T2>(this string s, FormattableString pattern)
        {
            InitializeRegex(pattern);

            var matches = _regexCache[pattern.Format].Match(s);

            return (
                Convert<T1>(matches.Groups[1].Value, pattern.GetArguments()[0]),
                Convert<T2>(matches.Groups[2].Value, pattern.GetArguments()[1]));
        }

        public static (T1 s1, T2 s2, T3 s3) Deconstruct<T1, T2, T3>(this string s, char separator = ' ')
        {
            return s.Split(separator) switch
            {
                [var s1, var s2, var s3] => (Convert<T1>(s1), Convert<T2>(s2), Convert<T3>(s3)),
            };
        }

        public static (T1 s1, T2 s2, T3 s3) Deconstruct<T1, T2, T3>(this string s, FormattableString pattern)
        {
            InitializeRegex(pattern);

            var matches = _regexCache[pattern.Format].Match(s);

            return (
                Convert<T1>(matches.Groups[1].Value, pattern.GetArguments()[0]),
                Convert<T2>(matches.Groups[2].Value, pattern.GetArguments()[1]),
                Convert<T3>(matches.Groups[3].Value, pattern.GetArguments()[2]));
        }

        public static (T1 s1, T2 s2, T3 s3, T4 s4) Deconstruct<T1, T2, T3, T4>(this string s, char separator = ' ')
        {
            return s.Split(separator) switch
            {
                [var s1, var s2, var s3, var s4] => (Convert<T1>(s1), Convert<T2>(s2), Convert<T3>(s3), Convert<T4>(s4)),
            };
        }

        
        public static (T1 s1, T2 s2, T3 s3, T4 s4) Deconstruct<T1, T2, T3, T4>(this string s, FormattableString pattern)
        {
            InitializeRegex(pattern);

            var matches = _regexCache[pattern.Format].Match(s);

            return (
                Convert<T1>(matches.Groups[1].Value), Convert<T2>(matches.Groups[2].Value),
                Convert<T3>(matches.Groups[3].Value), Convert<T4>(matches.Groups[4].Value));
        }
        public static (T1 s1, T2 s2, T3 s3, T4 s4, T5 s5) Deconstruct<T1, T2, T3, T4, T5>(this string s, char separator = ' ')
        {
            return s.Split(separator) switch
            {
                [var s1, var s2, var s3, var s4, var s5] => (Convert<T1>(s1), Convert<T2>(s2), Convert<T3>(s3), Convert<T4>(s4), Convert<T5>(s5)),
            };
        }
        public static (T1 s1, T2 s2, T3 s3, T4 s4, T5 s5, T6 s6) Deconstruct<T1, T2, T3, T4, T5, T6>(this string s, char separator = ' ')
        {
            return s.Split(separator) switch
            {
                [var s1, var s2, var s3, var s4, var s5, var s6] => (Convert<T1>(s1), Convert<T2>(s2), Convert<T3>(s3), Convert<T4>(s4), Convert<T5>(s5), Convert<T6>(s6)),
            };
        }


        public static (T1 s1, T2 s2, T3 s3,T4 s4,T5 s5,T6 s6) Deconstruct<T1, T2, T3, T4, T5, T6>(this string s, FormattableString pattern)
        {
            InitializeRegex(pattern);

            var matches = _regexCache[pattern.Format].Match(s);

            return (
                Convert<T1>(matches.Groups[1].Value, pattern.GetArguments()[0]),
                Convert<T2>(matches.Groups[2].Value, pattern.GetArguments()[1]),
                Convert<T3>(matches.Groups[3].Value, pattern.GetArguments()[2]),
                Convert<T4>(matches.Groups[4].Value, pattern.GetArguments()[2]),
                Convert<T5>(matches.Groups[5].Value, pattern.GetArguments()[2]),
                Convert<T6>(matches.Groups[6].Value, pattern.GetArguments()[2]));
        }

        private static T Convert<T>(this string s, object? arg = null)
        {
            if(typeof(T) == typeof(string[]) && arg is string separator)
                return (T)System.Convert.ChangeType(s.Split(separator), typeof(T));
            if(typeof(T) == typeof(int[]) && arg is string separator2)
                return (T)System.Convert.ChangeType(s.Split(separator2).ToInts().ToArray(), typeof(T));
            return (T) System.Convert.ChangeType(s, typeof(T));
        }

        public static int ToInt(this string s)
        {
            return int.Parse(s);
        }

        public static long ToLong(this string s)
        {
            return long.Parse(s);
        }

        public static IEnumerable<(string, string)> PairsOfLines(this IEnumerable<string> lines)
        {
            var enumerator = lines.GetEnumerator();
            while (enumerator.MoveNext())
            {
                yield return (enumerator.Current, enumerator.MoveNext() ? enumerator.Current : null);
                enumerator.MoveNext();
            }
            enumerator.Dispose();
        }

        public static IEnumerable<(string? prev, string current, string? next)> AdjacentLines(
            this IEnumerable<string> lines)
        {
            var enumerator = lines.GetEnumerator();
            string? prev = null;
            string? next = null;
            enumerator.MoveNext();
            var current = enumerator.Current;
            enumerator.MoveNext();
            next = enumerator.Current;
            yield return (prev, current, next);
            while (next != null)
            {
                prev = current;
                current = next;
                if (enumerator.MoveNext())
                    next = enumerator.Current;
                else
                    next = null;
                yield return (prev, current, next);
            }
        }

        public static bool IsNullOrWhitespace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        public static ReadOnlySpan<char> ClampingSpan(this string s, int start, int length)
        {
            if (start + length > s.Length)
            {
                length = s.Length - start;
            }
            return s.AsSpan(start.ClampMin(0), length);
        }

        public static ReadOnlySpan<char> ClampingSpan(this string s, Range range)
        {
            var clampedLength = (range.End.Value).ClampMax(s.Length);
            return s.AsSpan(range.Start.Value.ClampMin(0), clampedLength);
        }

        public static int ToInt(this ReadOnlySpan<char> chrs)
        {
            return int.Parse(chrs);
        }

        public static char? SafeCharAt(this string s, int i)
        {
            return i.IsBetween(0, s.Length - 1) ? s[i] : null;
        }

    }
    
}
