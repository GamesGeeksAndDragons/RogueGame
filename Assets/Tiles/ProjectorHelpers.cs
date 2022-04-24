#nullable enable
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Enums;
using Line = System.Collections.Generic.List<(string Id, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Tiles
{
    internal static class ProjectorHelpers
    {
        internal static bool ShouldProject(this ITiles tiles, Coordinate coordinate)
        {
            if (!tiles.IsInside(coordinate)) return false;

            return tiles.IsRock(coordinate) ||
                   tiles.IsDoor(coordinate) ||
                   tiles.IsFloor(coordinate);
        }

        internal static Line ProjectLine(this ITiles tiles, Coordinate coordinate, IDispatchRegistry registry, Compass4Points direction)
        {
            var line = new List<(string Id, Coordinate Coordinates)>();
            if (tiles.ShouldProject(coordinate)) line.Add((tiles[coordinate], coordinate));

            bool continueProjection;
            do
            {
                coordinate = coordinate.Move(direction);

                continueProjection = tiles.ShouldProject(coordinate);
                if (continueProjection)
                {
                    line.Add((tiles[coordinate], coordinate));
                }
            } while (continueProjection);

            return line;
        }

        internal static Line JoinLines(this Line line1, Line line2)
        {
            var fullPath = new Line(line1);
            fullPath.AddRange(line2);
            return fullPath;
        }
    }
}