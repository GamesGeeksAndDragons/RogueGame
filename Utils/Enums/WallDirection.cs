#nullable enable
using Utils.Display;

namespace Utils.Enums
{
    [Flags]
    public enum WallDirection
    {
        Horizontal = 0x01,
        Vertical = 0x02,
        TopLeftCorner = 0x04,
        TopRightCorner = 0x08,
        BottomLeftCorner = 0x10,
        BottomRightCorner = 0x20,

        Corner = TopLeftCorner | TopRightCorner | BottomLeftCorner | BottomRightCorner,
        NonCorner = Horizontal | Vertical,
        All = NonCorner | Corner,
    }

    public static class WallDirectionHelpers
    {
        public static bool HasDirection(this WallDirection direction, WallDirection compare)
        {
            var bitwise = direction & compare;
            return bitwise == compare;
        }

        public static WallDirection ToWallDirection(this string actor)
        {
            switch (actor)
            {
                case TilesDisplay.WallHorizontal: return WallDirection.Horizontal;
                case TilesDisplay.WallVertical: return WallDirection.Vertical;
                case TilesDisplay.WallTopLeftCorner: return WallDirection.TopLeftCorner;
                case TilesDisplay.WallTopRightCorner: return WallDirection.TopRightCorner;
                case TilesDisplay.WallBottomLeftCorner: return WallDirection.BottomLeftCorner;
                case TilesDisplay.WallBottomRightCorner: return WallDirection.BottomRightCorner;
            }

            throw new ArgumentException($"Unexpected actor [{actor}]");
        }

        public static string FromWallDirection(this WallDirection direction)
        {
            switch (direction)
            {
                case WallDirection.Horizontal: return TilesDisplay.WallHorizontal;
                case WallDirection.Vertical: return TilesDisplay.WallVertical;
                case WallDirection.TopLeftCorner: return TilesDisplay.WallTopLeftCorner;
                case WallDirection.TopRightCorner: return TilesDisplay.WallTopRightCorner;
                case WallDirection.BottomLeftCorner: return TilesDisplay.WallBottomLeftCorner;
                case WallDirection.BottomRightCorner: return TilesDisplay.WallBottomRightCorner;
            }

            throw new ArgumentException($"Unexpected wall direction [{direction}]");
        }
    }
}
