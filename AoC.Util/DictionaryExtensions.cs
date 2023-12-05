using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Util
{
    public static class DictionaryExtensions
    {
        public static void CreateOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if(dict.ContainsKey(key))
                dict[key] = value;
            else
                dict.Add(key, value);
        }

        public static void Increment<TKey>(this IDictionary<TKey, int> dict, TKey key, int step = 1)
        {
            if(dict.ContainsKey(key))
                dict[key] = dict[key]+step;
            else
                dict.Add(key, step);
        }
        public static void CreateOrAppend<TKey, TValue>(this IDictionary<TKey, List<TValue>> dict, TKey key, TValue value)
        {
            if(dict.ContainsKey(key))
                dict[key].Add(value);
            else
                dict.Add(key, new List<TValue>(){value});
        }

        public static IEnumerable<TVal> ForKeys<TKey, TVal>(this IDictionary<TKey, TVal> dict, params TKey[] keys)
        {
            foreach (var key in keys)
            {
                yield return dict[key];
            }
        }

        public static void AddToSetUnderKey<TKey, TVal>(this IDictionary<TKey, HashSet<TVal>> dict, TKey key, TVal value)
        {
            if (dict.TryGetValue(key, out var set))
            {
                set.Add(value);
            }
            else
            {
                dict.Add(key,new HashSet<TVal>(){value});
            }
        }
    }
}
