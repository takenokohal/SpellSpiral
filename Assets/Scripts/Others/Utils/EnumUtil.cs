using System;
using System.Collections.Generic;
using System.Linq;

namespace Others.Utils
{
    public static class EnumUtil<T> where T : Enum
    {
        public static IEnumerable<T> GetValues()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static T GetRandomValue()
        {
            return GetValues().GetRandomValue();
        }

        public static IEnumerable<string> GetStrings() =>
            GetValues().Select(value => value.ToString());
    }
}