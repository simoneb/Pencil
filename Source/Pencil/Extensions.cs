using System;
using System.Collections.Generic;
using System.Linq;

namespace Pencil
{
    public static class Extensions
    {
        public static V Get<T, V>(this T instance, Func<T, V> get) where T : class
        {
            return instance != null ? get(instance) : default(V);
        }

        public static bool Empty<T>(this IEnumerable<T> enumerable)
        {
            return !enumerable.Any();
        }

        public static string Join(this IEnumerable<string> enumerable, string separator)
        {
            return string.Join(separator, enumerable.ToArray());
        }

        public static bool None<T>(this IEnumerable<T> enumerable, Func<T, bool> predicate)
        {
            return !enumerable.Any(predicate);
        }
    }
}