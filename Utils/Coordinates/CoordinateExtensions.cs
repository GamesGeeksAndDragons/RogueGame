using System;
using Utils.Enums;

namespace Utils.Coordinates
{
    public static class CoordinateExtensions
    {
        public static Coordinate Left(this Coordinate coordinates, int left = 1) => new Coordinate(coordinates.Row, coordinates.Column - left);
        public static Coordinate Right(this Coordinate coordinates, int right = 1) => new Coordinate(coordinates.Row, coordinates.Column + right);
        public static Coordinate Up(this Coordinate coordinates, int up = 1) => new Coordinate(coordinates.Row - up, coordinates.Column);
        public static Coordinate Down(this Coordinate coordinates, int down=1) => new Coordinate(coordinates.Row + down, coordinates.Column);

        public static bool IsInside(this Coordinate point, int maxRow, int maxColumn)
        {
            return point.Row >= 0 && point.Column >= 0 &&
                   point.Row <= maxRow && point.Column <= maxColumn;
        }

        public static Coordinate Move(this Coordinate coordinates, Compass4Points direction)
        {
            switch (direction)
            {
                case Compass4Points.North: return coordinates.Up();
                case Compass4Points.South: return coordinates.Down();
                case Compass4Points.East: return coordinates.Right(); 
                case Compass4Points.West: return coordinates.Left();
                default:
                    var message = $"Unrecognised direction [{direction}]";
                    throw new ArgumentException(message);
            }
        }

        public static Coordinate Move(this Coordinate coordinates, Compass8Points direction)
        {
            switch (direction)
            {
                case Compass8Points.North: return coordinates.Up();
                case Compass8Points.NorthEast: return coordinates.Up().Right();
                case Compass8Points.NorthWest: return coordinates.Up().Left();
                case Compass8Points.South: return coordinates.Down();
                case Compass8Points.SouthEast: return coordinates.Down().Right();
                case Compass8Points.SouthWest: return coordinates.Down().Left();
                case Compass8Points.East: return coordinates.Right();
                case Compass8Points.West: return coordinates.Left();
                default:
                    var message = $"Unrecognised direction [{direction}]";
                    throw new ArgumentException(message);
            }
        }

        public static Coordinate ToCoordinates(this string coordinates)
        {
            var parts = coordinates.Split('(', ',', ')');
            var row = int.Parse(parts[1]);
            var column = int.Parse(parts[2]);

            return new Coordinate(row, column);
        }
    }
}