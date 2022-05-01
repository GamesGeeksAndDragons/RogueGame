#nullable enable
using Utils.Dispatching;

namespace Utils
{
    public static class ActorDisplay
    {
        // UTF-8
        public const string Null = "#";
        public const string Floor = " ";
        public const string Rock = "█";
        public const string Me = "@";
        public const string DebugWeapon = "W";
        public const string DebugMonster = "M";
        public const string WallHorizontal = "═";
        public const string WallVertical = "║";
        public const string WallTopLeftCorner = "╔";
        public const string WallTopRightCorner = "╗";
        public const string WallBottomLeftCorner = "╚";
        public const string WallBottomRightCorner = "╝";
        public static readonly List<string> Doors = new() { "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };
        public static readonly List<string> Walls = new() { WallHorizontal, WallVertical, WallTopLeftCorner, WallTopRightCorner, WallBottomLeftCorner, WallBottomRightCorner };

        //https://unicode-table.com/en/sets/superscript-and-subscript-letters/#subscript,-superscript
        public static readonly List<string> RoomNumberOfFloor = new() { Floor, "¹", "²", "³", "⁴", "⁵", "⁶", "⁷", "⁸", "⁹", "ᵃ", "ᵇ", "ᶜ", "ᵈ", "ᵉ", "ᶠ", "ᶢ", "ᵸ", "ᵢ", "ᶨ", "ᵏ" };

        // https://unicode-table.com/en/sets/mathematical-signs/
        public const string Tiles = "*";
        public const string MagicMissile = "⋇";
        public const string MagicMissile2 = "∗";
        public const string FloorIlluminated = "⋅";

        private static ArgumentException UnknownDispatched(IDispatched dispatched) => new ArgumentException($"Unknown dispatched [{dispatched.Name}] in ToDisplayChar");

        public static bool IsFloorActor(this string actor)
        {
            return RoomNumberOfFloor.Contains(actor);
        }

        public static string ToRoomNumberString(this int num)
        {
            num.ThrowIfBelow(0, nameof(num));
            num.ThrowIfAbove(RoomNumberOfFloor.Count-1, nameof(num));

            return RoomNumberOfFloor[num];
        }

        public static int FromRoomNumberString(this string num)
        {
            var index = RoomNumberOfFloor.IndexOf(num);
            if (index == -1) throw new ArgumentException($"Unknown RoomNumber [{num}]");

            return index;
        }
    }
}
