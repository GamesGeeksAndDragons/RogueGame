#nullable enable
using Utils;

namespace Assets.Deeds
{
    internal static class Deed
    {
        public const string Hit = "Hit";
        public const string Move = "Move";
        public const string Strike = "Strike";
        public const string Teleport = "Teleport";
        public const string Use = "Use";

        private static readonly string[] Deeds = new string[]
        {
            Hit, Move, Strike, Teleport, Use
        };

        public static bool IsValid(string deed)
        {
            return Deeds.Any(action => action.IsSame(deed));
        }
    }
}
