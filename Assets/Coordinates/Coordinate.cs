namespace Assets.Coordinates
{
    public struct Coordinate
    {
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Coordinate))
                return false;

            return Equals((Coordinate)obj);
        }

        public bool Equals(Coordinate other)
        {
            return X == other.X && Y == other.Y;
        }

        public static bool operator==(Coordinate point1, Coordinate point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(Coordinate point1, Coordinate point2)
        {
            return !point1.Equals(point2);
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }
    }
}