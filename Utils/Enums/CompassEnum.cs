using System;

namespace Utils.Enums
{
    [Flags]
    public enum Compass4Points
    {
        North = 0x01, South = 0x02, East = 0x04, West = 0x08
    }


    [Flags]
    public enum Compass8Points
    {
        North= Compass4Points.North,
        South = Compass4Points.South,
        East = Compass4Points.East,
        West = Compass4Points.West,
        NorthEast = Compass4Points.North | Compass4Points.East,
        NorthWest = Compass4Points.North | Compass4Points.West,
        SouthEast = Compass4Points.South | Compass4Points.East,
        SouthWest = Compass4Points.South | Compass4Points.West,
    }


}