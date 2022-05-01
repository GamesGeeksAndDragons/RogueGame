#nullable enable
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
            return (direction & compare) == compare;
        }

        public static WallDirection ToWallDirection(this string actor)
        {
            switch (actor)
            {
                case ActorDisplay.WallHorizontal: return WallDirection.Horizontal;
                case ActorDisplay.WallVertical: return WallDirection.Vertical;
                case ActorDisplay.WallTopLeftCorner: return WallDirection.TopLeftCorner;
                case ActorDisplay.WallTopRightCorner: return WallDirection.TopRightCorner;
                case ActorDisplay.WallBottomLeftCorner: return WallDirection.BottomLeftCorner;
                case ActorDisplay.WallBottomRightCorner: return WallDirection.BottomRightCorner;
            }

            throw new ArgumentException($"Unexpected actor [{actor}]");
        }

        public static string FromWallDirection(this WallDirection direction)
        {
            switch (direction)
            {
                case WallDirection.Horizontal: return ActorDisplay.WallHorizontal;
                case WallDirection.Vertical: return ActorDisplay.WallVertical;
                case WallDirection.TopLeftCorner: return ActorDisplay.WallTopLeftCorner;
                case WallDirection.TopRightCorner: return ActorDisplay.WallTopRightCorner;
                case WallDirection.BottomLeftCorner: return ActorDisplay.WallBottomLeftCorner;
                case WallDirection.BottomRightCorner: return ActorDisplay.WallBottomRightCorner;
            }

            throw new ArgumentException($"Unexpected wall direction [{direction}]");
        }
    }
}
