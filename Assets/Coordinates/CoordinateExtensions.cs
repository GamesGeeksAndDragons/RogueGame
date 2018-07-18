namespace Assets.Coordinates
{
    public static class CoordinateExtensions
    {
        public static Coordinate West(this Coordinate coordinate) => new Coordinate(coordinate.X - 1, coordinate.Y);
        public static Coordinate East(this Coordinate coordinate) => new Coordinate(coordinate.X + 1, coordinate.Y);
        public static Coordinate North(this Coordinate coordinate) => new Coordinate(coordinate.X, coordinate.Y - 1);
        public static Coordinate South(this Coordinate coordinate) => new Coordinate(coordinate.X, coordinate.Y + 1);

        public static bool IsConnectedTo(this Coordinate coordinate, Coordinate point)
        {
            return coordinate == point ||
                   coordinate.North() == point ||
                   coordinate.South() == point ||
                   coordinate.East() == point ||
                   coordinate.West() == point;
        }
    }
}