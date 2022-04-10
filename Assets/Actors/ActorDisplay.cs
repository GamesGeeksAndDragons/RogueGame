using System;
using Assets.Messaging;
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
        public const string Door1 = "1";
        public const string Door2 = "2";
        public const string Door3 = "3";
        public const string Door4 = "4";
        public const string Door5 = "5";
        public const string Door6 = "6";
        public const string Door7 = "7";
        public const string Door8 = "8";
        public const string Door9 = "9";
        public const string Door10 = "A";
        public const string Door11 = "B";
        public const string Door12 = "C";
        public const string Door13 = "D";
        public const string Door14 = "E";
        public const string Door15 = "F";

        private static void UnknownDispatchee(IDispatchee dispatchee) => throw new ArgumentException($"Unknown dispatchee [{dispatchee.Name}] in ToDisplayChar");


        public static string ToDisplayChar(this IDispatchee dispatchee)
        {
            //System.Diagnostics.Debugger.Break();

            switch (dispatchee.Name)
            {
                case "Null": return Null;
                case "Floor": return Floor;
                case "Rock": return Rock;
                case "Me": return Me;
                case "Monster": return Monster;
                case "Wall": return DisplayWall();
                case "Door": return DisplayDoor();
                default:
                    UnknownDispatchee(dispatchee);
                    return null;
            }

            string DisplayDoor()
            {
                var door = (Door)dispatchee;
                return door.DoorId.ToString("X1");
            }

            string DisplayWall()
            {
                var wall = (Wall)dispatchee;

                switch (wall.WallType)
                {
                    case WallDirection.Horizontal: return WallHorizontal;
                    case WallDirection.Vertical: return WallVertical;
                    case WallDirection.TopLeftCorner: return WallTopLeftCorner;
                    case WallDirection.TopRightCorner: return WallTopRightCorner;
                    case WallDirection.BottomLeftCorner: return WallBottomLeftCorner;
                    case WallDirection.BottomRightCorner: return WallBottomRightCorner;
                    default:
                        UnknownDispatchee(dispatchee);
                        return null;
                }
            }
        }
    }
}
