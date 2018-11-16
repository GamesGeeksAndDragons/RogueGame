using System;
using Utils.Coordinates;

namespace Utils
{
    public static class ParameterExceptionHelpers
    {
        public static void ThrowIfSameInstance<T>(this T t1, T t2, string name1, string name2) where T : class
        {
            if (ReferenceEquals(t1, t2))
            {
                throw new ArgumentNullException($"Unexpected same instances [{name1},{name2}] for type [{typeof(T).Name}]");
            }
        }

        public static void ThrowIfNull<T>(this T t, string name) where T : class
        {
            if (t == null)
            {
                throw new ArgumentNullException(name, $"Unexpected null for [{name}] for type [{typeof(T).Name}]");
            }
        }

        public static void ThrowIfNotNull<T>(this T t, string name) where T : class
        {
            if (t != null)
            {
                throw new ArgumentNullException(name, $"Expected [{name}] to be null for type [{typeof(T).Name}] and it was [{t}]");
            }
        }

        public static void ThrowIfEqual(this int lhs, int rhs, string name)
        {
            if (lhs == rhs)
            {
                throw new ArgumentException(name, $"[{name}:{lhs}] should not have a value of [{rhs}]");
            }
        }

        public static void ThrowIfNotEqual(this int lhs, int rhs, string name)
        {
            if (lhs != rhs)
            {
                throw new ArgumentException(name, $"[{name}:{lhs}] should have the same, but it was different [{rhs}]");
            }
        }

        public static void ThrowIfAbove(this int lhs, int rhs, string name, bool alsoCheckEquality=false)
        {
            if (lhs > rhs)
            {
                throw new ArgumentException(name, $"[{name}:{lhs}] should not be above [{rhs}]");
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
                throw new ArgumentException(name, $"[{name}:{lhs}] should not be below [{rhs}]");
            }

            if (alsoCheckEquality)
            {
                rhs.ThrowIfEqual(lhs, name);
            }
        }

        public static void ThrowIfEmpty(this string lhs, string name)
        {
            if(lhs.IsNullOrEmpty())
            {
                throw new ArgumentException(name, $"[{name}] was empty when it should not have been");
            }
        }

        public static void ThrowIfNotEmpty(this string lhs, string name)
        {
            if (! lhs.IsNullOrEmpty())
            {
                throw new ArgumentException(name, $"[{name}] was expected to be empty when it was [{lhs}]");
            }
        }

        public static void ThrowIfOutsideBounds(this string [,] array, Coordinate coordinates, string name)
        {
            if (!array.IsInside(coordinates))
            {
                var maxBounary = new Coordinate(array.UpperBounds());
                throw new ArgumentException(name, $"[{name}] expected the indexer [{coordinates}] to be inside [{maxBounary}] and they were outside");
            }
        }
    }
}
