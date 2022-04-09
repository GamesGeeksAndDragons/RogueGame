using System;

namespace Utils.Enums
{
    [Flags]
    public enum Compass4Points
    {
        Undefined = 0x00, North = 0x01, South = 0x02, East = 0x04, West = 0x08
    }

    [Flags]
    public enum Compass8Points
    {
        Undefined = Compass4Points.Undefined,
        North= Compass4Points.North,
        South = Compass4Points.South,
        East = Compass4Points.East,
        West = Compass4Points.West,
        NorthEast = 0x10,
        NorthWest = 0x20,
        SouthEast = 0x40,
        SouthWest = 0x80,
    }

}