using System.Collections.Generic;

namespace Option.Extensions
{
    public static class DictionaryExtensions
    {
        public static Option<TValue> TryGetValue<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) =>
            dict.TryGetValue(key, out var value)
                ? (Option<TValue>) value
                : None.Value;
    }
}