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
    }
}
