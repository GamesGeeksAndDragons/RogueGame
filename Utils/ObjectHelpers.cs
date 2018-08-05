using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public static class ObjectHelpers
    {
        public static bool IsSameInstance<T>(this T first, T second)
        {
            return ReferenceEquals(first, second);
        }
    }
}
