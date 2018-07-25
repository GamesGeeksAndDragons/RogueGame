using System;
using Utils.Enums;

namespace Utils.Coordinates
{
    public static class CoordinateExtensions
    {
        public static Coordinate Left(this Coordinate coordinate, int left = 1) => new Coordinate(coordinate.Row, coordinate.Column - left);
        public static Coordinate Right(this Coordinate coordinate, int right = 1) => new Coordinate(coordinate.Row, coordinate.Column + right);
        public static Coordinate Up(this Coordinate coordinate, int up = 1) => new Coordinate(coordinate.Row - up, coordinate.Column);
        public static Coordinate Down(this Coordinate coordinate, int down=1) => new Coordinate(coordinate.Row + down, coordinate.Column);

        public static bool IsInside(this Coordinate point, int maxRow, int maxColumn)
        {
            return point.Row >= 0 && point.Column >= 0 &&
                   point.Row <= maxRow && point.Column <= maxColumn;
        }

        public static Coordinate Move(this Coordinate coordinate, Compass4Points direction)
        {
            switch (direction)
            {
                case Compass4Points.North: return coordinate.Up();
                case Compass4Points.South: return coordinate.Down();
                case Compass4Points.East: return coordinate.Right(); 
                case Compass4Points.West: return coordinate.Left();
                default:
                    var message = $"Unrecognised direction [{direction}]";
                    throw new ArgumentException(message);
            }

        }
    }
}