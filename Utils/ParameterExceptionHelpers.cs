using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public static class ParameterExceptionHelpers
    {
        public static void ThrowIfNull<T>(this T t, string name) where T : class
        {
            if (t == null)
            {
                throw new ArgumentNullException(name, $"Unexpected null for [{name}]");
            }
        }

        public static void ThrowIfEqual(this int lhs, int rhs, string name)
        {
            if (lhs == rhs)
            {
                throw new ArgumentException(name, $"[{name}] should not have a value of [{lhs}]");
            }
        }

        public static void ThrowIfAbove(this int lhs, int rhs, string name, bool alsoCheckEquality=false)
        {
            if (lhs > rhs)
            {
                throw new ArgumentNullException(name, $"Unexpected null for [{name}]");
            }

            if (alsoCheckEquality)
            {
                rhs.ThrowIfEqual(lhs, name);
            }
        }

        public static void ThrowIfBelow(this int lhs, int rhs, string name, bool alsoCheckEquality = false)
        {
            if (lhs < rhs)
            {
                throw new ArgumentNullException(name, $"Unexpected null for [{name}]");
            }

            if (alsoCheckEquality)
            {
                rhs.ThrowIfEqual(lhs, name);
            }
        }
    }
}
