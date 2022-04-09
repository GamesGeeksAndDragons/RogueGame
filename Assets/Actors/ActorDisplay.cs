using System;
using Assets.Messaging;
using Utils.Dispatching;

namespace Assets.Actors
{
    public static class ActorDisplay
    {
        public const char Null = '#';
        public const char Floor = ' ';
        public const char Rock = '█';
        public const char WallHorizontal = '═';
        public const char WallVertical = '║';
        public const char WallTopLeftCorner = '╔';
        public const char WallTopRightCorner = '╗';
        public const char WallBottomLeftCorner = '╚';
        public const char WallBottomRightCorner = '╝';
        public const char Door1 = '1';
        public const char Door2 = '2';
        public const char Door3 = '3';
        public const char Door4 = '4';
        public const char Door5 = '5';
        public const char Door6 = '6';
        public const char Door7 = '7';
        public const char Door8 = '8';
        public const char Door9 = '9';

        public static char ToChar(this IDispatchee dispatchee)
        {
            System.Diagnostics.Debugger.Break();

            switch (dispatchee.Name)
            {
                case "Null": return Null;
                case "Floor": return Floor;
                case "Rock": return Rock;
                case "WallHorizontal": return WallHorizontal;
                case "WallVertical": return WallVertical;
                case "WallTopLeftCorner": return WallTopLeftCorner;
                case "WallTopRightCorner": return WallTopRightCorner;
                case "WallBottomLeftCorner": return WallBottomLeftCorner;
                case "WallBottomRightCorner": return WallBottomRightCorner;
                default:
                    throw new ArgumentException($"Unknown dispatchee [{dispatchee.Name}] in ToChar");
            }
        }
    }
}
