#nullable enable
using Assets.Maze;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Enums;
using Line = System.Collections.Generic.List<(string Id, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Mazes
{
    internal static class ProjectorHelpers
    {
        internal static bool ShouldProject(this IMaze maze, Coordinate coordinate)
        {
            if (!maze.IsInside(coordinate)) return false;

            return maze.IsRock(coordinate) ||
                   maze.IsDoor(coordinate) ||
                   maze.IsFloor(coordinate);
        }

        internal static Line ProjectLine(this IMaze maze, Coordinate coordinate, IDispatchRegistry registry, Compass4Points direction)
        {
            var line = new List<(string Id, Coordinate Coordinates)>();
            if (maze.ShouldProject(coordinate)) line.Add((maze[coordinate], coordinate));

            bool continueProjection;
            do
            {
                coordinate = coordinate.Move(direction);

                continueProjection = maze.ShouldProject(coordinate);
                if (continueProjection)
                {
                    line.Add((maze[coordinate], coordinate));
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