#nullable enable
using System.IO;
using System.Runtime.CompilerServices;
using Utils.Coordinates;

namespace Utils
{
    public static class ParameterExceptionHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfSameInstance<T>(this T t1, T t2, string name1, string name2) where T : class
        {
            if (ReferenceEquals(t1, t2))
            {
                throw new ArgumentNullException($"Unexpected same instances [{name1},{name2}] for type [{typeof(T).Name}]");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull<T>(this T t, string name) where T : class
        {
            if (t == null)
            {
                throw new ArgumentNullException(name, $"Unexpected null for [{name}] for type [{typeof(T).Name}]");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNotNull<T>(this T t, string name) where T : class
        {
            if (t != null)
            {
                throw new ArgumentNullException(name, $"Expected [{name}] to be null for type [{typeof(T).Name}] and it was [{t}]");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T Lhs, T Rhs, string Name) ThrowIfEqual<T>(this T lhs, T rhs, string name)
            where T : struct 
        {
            return (lhs, rhs, name).ThrowIfEqual<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (T Lhs, T Rhs, string Name) ThrowIfEqual<T>(this (T Lhs, T Rhs, string Name) test)
            where T : struct
        {
            if (test.Lhs.Equals(test.Rhs))
            {
                throw new ArgumentException(test.Name, $"[{test.Name}:{test.Lhs}] should not equal [{test.Rhs}]");
            }

            return test;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Lhs, int Rhs, string Name) ThrowIfNotEqual(this int lhs, int rhs, string name)
        {
            return (lhs, rhs, name).ThrowIfNotEqual();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Lhs, int Rhs, string Name) ThrowIfNotEqual(this (int Lhs, int Rhs, string Name) test)
        {
            if (test.Lhs != test.Rhs)
            {
                throw new ArgumentException(test.Name, $"[{test.Name}:{test.Lhs}] should not differ, but it was [{test.Rhs}]");
            }

            return test;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Lhs, int Rhs, string Name) ThrowIfAbove(this int lhs, int rhs, string name)
        {
            return (lhs, rhs, name).ThrowIfAbove();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Lhs, int Rhs, string Name) ThrowIfAbove(this (int Lhs, int Rhs, string Name) test)
        {
            if (test.Lhs > test.Rhs)
            {
                throw new ArgumentException(test.Name, $"[{test.Name}:{test.Lhs}] should not be above [{test.Rhs}]");
            }

            return test;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Lhs, int Rhs, string Name) ThrowIfBelow(this int lhs, int rhs, string name)
        {
            return (lhs, rhs, name).ThrowIfBelow();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (int Lhs, int Rhs, string Name) ThrowIfBelow(this (int Lhs, int Rhs, string Name) test)
        {
            if (test.Lhs < test.Rhs)
            {
                throw new ArgumentException(test.Name, $"[{test.Name}:{test.Lhs}] should not be below [{test.Rhs}]");
            }

            return test;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (string Lhs, string Name) ThrowIfEmpty(this string lhs, string name)
        {
            return (lhs, name).ThrowIfEmpty();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (string Lhs, string Name) ThrowIfEmpty(this (string Lhs, string Name) test)
        {
            if (test.Lhs.IsNullOrEmpty())
            {
                throw new ArgumentException(test.Name, $"[{test.Name}] was empty when it should not have been");
            }

            return test;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (string Lhs, string Name) ThrowIfNotEmpty(this string lhs, string name)
        {
            return (lhs, name).ThrowIfNotEmpty();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (string Lhs, string Name) ThrowIfNotEmpty(this (string Lhs, string Name) test)
        {
            if (!test.Lhs.IsNullOrEmpty())
            {
                throw new ArgumentException(test.Name, $"[{test.Name}] was expected to be empty when it was [{test.Lhs}]");
            }

            return test;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfFileNotExist(this string filename)
        {
            var exists = File.Exists(filename);
            if (!exists)
            {
                throw new FileNotFoundException("Not exist", filename);
            }
        }
    }
}
