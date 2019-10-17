using System;
using System.Collections.Generic;
using System.Linq;

namespace Option.Extensions
{
    public static class EnumerableExtensions
    {
        public static Option<T> FirstOrNone<T>(this IEnumerable<T> enumerable) =>
            enumerable.Select(x => (Option<T>) x)
                .DefaultIfEmpty(None.Value)
                .First();

        public static Option<T> FirstOrNone<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            bool TryPredicate(T x)
            {
                try
                {
                    return predicate(x);
                }
                catch
                {
                    return false;
                }
            }

            return enumerable.Where(TryPredicate).FirstOrNone();
        }

        public static IEnumerable<TResult> SelectSome<T, TResult>(this IEnumerable<T> enumerable, Func<T, Option<TResult>> func) =>
            enumerable.Select(func)
                .OfType<Some<TResult>>()
                .Select(x => x.Content);
    }
}