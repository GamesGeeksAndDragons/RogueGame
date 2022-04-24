#nullable enable
namespace Utils.Coordinates
{
    public readonly record struct Coordinate(int Row, int Column)
    {
        public Coordinate((int row, int column) bounds) : this(bounds.row, bounds.column)
        {
        }

        public static readonly Coordinate NotSet = new Coordinate(-1, -1);

        public static Coordinate operator +(Coordinate point1, Coordinate point2)
        {
            return new Coordinate(point1.Row + point2.Row, point1.Column + point2.Column);
        }

        public static Coordinate operator -(Coordinate point1, Coordinate point2)
        {
            return new Coordinate(point1.Row - point2.Row, point1.Column - point2.Column);
        }

        public override string ToString()
        {
            return $"({Row},{Column})";
        }
    }
}

