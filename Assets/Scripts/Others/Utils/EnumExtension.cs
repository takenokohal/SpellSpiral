using System;
using System.Linq;

namespace Others.Utils
{
    public static class EnumExtension
    {
        public static T Increment<T>(this T t, int value) where T : Enum
        {
            var length = EnumUtil<T>.GetValues().Count();
            var next = Convert.ToInt32(t);
            next += value;
            next += length;
            next %= length;

            var obj = Enum.ToObject(typeof(T), next);
            return (T)obj;
        }
    }
}