using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class EnumHelpers
    {
        public static IEnumerable<T> Values<T>() where T : struct
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static T ToEnum<T>(this int value)
        {
            return (T)Enum.ToObject(typeof(T), value);
        }
    }
}
