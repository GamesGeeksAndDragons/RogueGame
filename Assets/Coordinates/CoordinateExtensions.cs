namespace Assets.Coordinates
{
    public static class CoordinateExtensions
    {
        public static Coordinate GoWest(this Coordinate coordinate) => new Coordinate(coordinate.X - 1, coordinate.Y);
        public static Coordinate GoEast(this Coordinate coordinate) => new Coordinate(coordinate.X + 1, coordinate.Y);
        public static Coordinate GoNorth(this Coordinate coordinate) => new Coordinate(coordinate.X, coordinate.Y - 1);
        public static Coordinate GoSouth(this Coordinate coordinate) => new Coordinate(coordinate.X, coordinate.Y + 1);
    }
}