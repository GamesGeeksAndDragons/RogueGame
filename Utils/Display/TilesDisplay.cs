#nullable enable
using Utils.Dispatching;

namespace Utils.Display
{
    public static class TilesDisplay
    {
        // UTF-8
        public const string Null = "#";
        public const string Floor = " ";
        public const string Rock = "█";
        public const string DebugWeapon = "W";
        public const string WallHorizontal = "═";
        public const string WallVertical = "║";
        public const string WallTopLeftCorner = "╔";
        public const string WallTopRightCorner = "╗";
        public const string WallBottomLeftCorner = "╚";
        public const string WallBottomRightCorner = "╝";
        //https://unicode-table.com/en/sets/superscript-and-subscript-letters/#subscript,-superscript
        public static readonly List<string> Doors = new() { "-", "1", "2", "3", "4", "5", "6", "7", "8", "9", "Α", "Β", "Γ", "Δ", "Ε", "Ζ", "Η", "Θ", "Ι", "Κ", "Λ", "Μ", "Ν", "Ξ", "Ο", "Π", "Ρ", "Σ", "Τ", "Υ", "Φ", "Χ", "Ψ", "Ω" };
        public static readonly List<string> Walls = new() { WallHorizontal, WallVertical, WallTopLeftCorner, WallTopRightCorner, WallBottomLeftCorner, WallBottomRightCorner };

        //https://unicode-table.com/en/sets/superscript-and-subscript-letters/#subscript,-superscript
        public static readonly List<string> RoomNumberOfFloor = new() { Floor, "¹", "²", "³", "⁴", "⁵", "⁶", "⁷", "⁸", "⁹", "α", "β", "γ", "δ", "ε", "ζ", "η", "θ", "ι", "κ", "λ", "μ", "ν", "ξ", "ο", "π", "ρ", "ς", "σ", "τ", "υ", "φ", "χ", "ψ", "ω" };

        //https://unicode-table.com/en/sets/krasivye-bukvy/
        public const string Maze = "ℳ";
        public const string Room = "ℛ";

        // https://unicode-table.com/en/sets/mathematical-signs/
        public const string MagicMissile = "⋇";
        public const string MagicMissile2 = "∗";
        public const string FloorIlluminated = "⋅";

        private static ArgumentException UnknownDispatched(IDispatched dispatched) => new ArgumentException($"Unknown dispatched [{dispatched.Name}] in ToDisplayChar");

        public static bool IsFloorActor(this string actor)
        {
            return RoomNumberOfFloor.Contains(actor.Intern());
        }

        private static string ToNumberString(this int num, List<string> numberArray)
        {
            num.ThrowIfBelow(0, nameof(num));
            num.ThrowIfAbove(RoomNumberOfFloor.Count - 1, nameof(num));

            return numberArray[num].Intern();
        }

        public static string ToRoomNumberString(this int num)
        {
            return num.ToNumberString(RoomNumberOfFloor);
        }

        public static string ToDoorNumberString(this int num)
        {
            return num.ToNumberString(Doors);
        }

        private static int FromNumberString(this string num, List<string> numberArray)
        {
            var index = numberArray.IndexOf(num.Intern());
            if (index == -1) throw new ArgumentException($"Unknown Number [{num}]");

            return index;
        }

        public static int FromRoomNumberString(this string num)
        {
            return num.FromNumberString(RoomNumberOfFloor);
        }

        public static int FromDoorNumberString(this string num)
        {
            return num.FromNumberString(Doors);
        }
    }
}
