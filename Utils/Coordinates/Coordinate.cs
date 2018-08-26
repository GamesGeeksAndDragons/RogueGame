namespace Utils.Coordinates
{
    public struct Coordinate
    {
        public Coordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public Coordinate((int row, int column) bounds)
        {
            Row = bounds.row;
            Column = bounds.column;
        }

        public int Row { get; }
        public int Column { get; }

        public static readonly Coordinate NotSet = new Coordinate(-1, -1);

        public override int GetHashCode()
        {
            return Row ^ Column;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Coordinate))
                return false;

            return Equals((Coordinate)obj);
        }

        public bool Equals(Coordinate other)
        {
            return Row == other.Row && Column == other.Column;
        }

        public static bool operator==(Coordinate point1, Coordinate point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(Coordinate point1, Coordinate point2)
        {
            return !point1.Equals(point2);
        }

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