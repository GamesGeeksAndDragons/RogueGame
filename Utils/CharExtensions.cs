﻿namespace Utils
{
    public static class CharExtensions
    {
        public static string ToPaddedString(this char ch, int length)
        {
            length.ThrowIfBelow(0, nameof(length)).ThrowIfEqual();

            return new string(ch, length);
        }
    }
}
