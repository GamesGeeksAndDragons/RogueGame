namespace Utils.Coordinates
{
    public static class CoordinateExtensions
    {
        public static Coordinate Left(this Coordinate coordinate, int left = 1) => new Coordinate(coordinate.Row, coordinate.Column - left);
        public static Coordinate Right(this Coordinate coordinate, int right = 1) => new Coordinate(coordinate.Row, coordinate.Column + right);
        public static Coordinate Up(this Coordinate coordinate, int up = 1) => new Coordinate(coordinate.Row - up, coordinate.Column);
        public static Coordinate Down(this Coordinate coordinate, int down=1) => new Coordinate(coordinate.Row + down, coordinate.Column);
    }
}