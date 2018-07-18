using System;

namespace Assets.Actions
{
    public enum Direction
    {
        North, South, East, West, NorthEast, NorthWest, SouthEast, SouthWest
    }

    [Flags]
    public enum BlockDirection
    {
        Up = 0x01, Down = 0x02, Right = 0x04, Left = 0x08
    }

}