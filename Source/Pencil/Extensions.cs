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
    }
}