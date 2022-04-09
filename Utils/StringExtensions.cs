using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils
{
    public static class StringExtensions
    {
        public static bool IsAlphabetic(this string str)
        {
            return str.All(char.IsLetter);
        }

        public static bool IsAlphaNumeric(this string str)
        {
            return str.All(char.IsLetterOrDigit);
        }

        public static bool IsAlphaNumericWithSpaces(this string str)
        {
            return str.All(IsAlphaNumericWithSpacesPredicate);
        }

        private static bool IsAlphaNumericWithSpacesPredicate(char ch)
        {
            return char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch);
        }

        public static bool IsNumeric(this string str)
        {
            return str.All(char.IsDigit);
        }

        public static bool IsOnlyWhitespace(this string str)
        {
            return str.All(char.IsWhiteSpace);
        }

        public static bool HasWhitespace(this string str)
        {
            return str.Any(char.IsWhiteSpace);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsNullOrEmptyOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        public static string Right(this string str, int rightCount)
        {
            if (str.IsNullOrEmptyOrWhiteSpace()) return string.Empty;

            return str.Length <= rightCount ? str : str.Substring(str.Length - rightCount);
        }

        public static string Left(this string str, int leftCount)
        {
            if (str.IsNullOrEmptyOrWhiteSpace()) return string.Empty;

            return str.Length <= leftCount ? str : str.Substring(0, leftCount);
        }

        public static string AddPadding(this string str, char pad, int length)
        {
            return length > 0 ? str + new string(pad, length) : str;
        }

        public static bool IsSame(this string str, string with, StringComparison comparison= StringComparison.CurrentCultureIgnoreCase)
        {
            return string.Compare(str, with, comparison)==0;
        }

        public static string[] SplitIntoLines(this string str)
        {
            return str.Split(CharHelpers.EndOfLine)
                .Where(line => ! line.IsNullOrEmpty())
                .ToArray();
        }
    }

}
