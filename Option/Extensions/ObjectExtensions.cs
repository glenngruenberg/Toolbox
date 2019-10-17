using System;

namespace Option.Extensions
{
    public static class ObjectExtensions
    {
        public static Option<T> Given<T>(this T obj, bool condition) =>
            condition
                ? (Option<T>) obj
                : None.Value;

        public static Option<T> Given<T>(this T obj, Func<T, bool> predicate) => obj.Given(predicate(obj));
    }
}