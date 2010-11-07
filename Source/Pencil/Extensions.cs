using System;

namespace Pencil
{
    public static class Extensions
    {
        public static V Get<T, V>(this T instance, Func<T, V> get) where T : class
        {
            return instance != null ? get(instance) : default(V);
        }
    }
}