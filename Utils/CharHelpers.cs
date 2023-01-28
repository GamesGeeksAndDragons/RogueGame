#nullable enable
namespace Utils
{
    public static class CharHelpers
    {
        public static string ToPaddedString(this char ch, int length)
        {
            length.ThrowIfBelow(0, nameof(length)).ThrowIfEqual();

            return new string(ch, length);
        }

        public const char CarriageReturn = '\r';
        public const char NewLine = '\n';
        public static readonly char[] EndOfLine = {CarriageReturn, NewLine};
    }
}
