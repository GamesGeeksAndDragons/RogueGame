#nullable enable
using Utils.Dispatching;
using Utils.Enums;

namespace Assets.Actors
{
    public static class ActorDisplay
    {
        public const string Null = "#";
        public const string Floor = " ";
        public const string Rock = "█";
        public const string Me = "@";
        public const string Monster = "M";
        public const string WallHorizontal = "═";
        public const string WallVertical = "║";
        public const string WallTopLeftCorner = "╔";
        public const string WallTopRightCorner = "╗";
        public const string WallBottomLeftCorner = "╚";
        public const string WallBottomRightCorner = "╝";
        public static readonly string[] Doors = new [] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };

        private static ArgumentException UnknownDispatched(IDispatched dispatched) => new ArgumentException($"Unknown dispatched [{dispatched.Name}] in ToDisplayChar");


        public static string ToDisplayChar(this IDispatched dispatched)
        {
            //System.Diagnostics.Debugger.Break();

            switch (dispatched.Name)
            {
                case "Null": return Null;
                case "Floor": return Floor;
                case "Rock": return Rock;
                case "Me": return Me;
                case "Monster": return Monster;
                case "Wall": return DisplayWall();
                case "Door": return DisplayDoor();
                default:
                    throw UnknownDispatched(dispatched);
            }

            string DisplayDoor()
            {
                var door = (Door)dispatched;
                return door.DoorId.ToString("X1");
            }

            string DisplayWall()
            {
                var wall = (Wall)dispatched;

                switch (wall.WallType)
                {
                    case WallDirection.Horizontal: return WallHorizontal;
                    case WallDirection.Vertical: return WallVertical;
                    case WallDirection.TopLeftCorner: return WallTopLeftCorner;
                    case WallDirection.TopRightCorner: return WallTopRightCorner;
                    case WallDirection.BottomLeftCorner: return WallBottomLeftCorner;
                    case WallDirection.BottomRightCorner: return WallBottomRightCorner;
                    default:
                        throw UnknownDispatched(dispatched);
                }
            }
        }
    }
}
