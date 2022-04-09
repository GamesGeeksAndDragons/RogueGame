using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Actors;
using Assets.Messaging;
using Utils.Coordinates;
using Utils.Enums;
using Line = System.Collections.Generic.List<(string Id, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Tiles
{
    internal static class ProjectorHelpers
    {
        internal static bool ShouldProject(this Tiles tiles, Coordinate coordinate, DispatchRegistry registry)
        {
            if (!tiles.IsInside(coordinate)) return false;

            return tiles.IsRock(coordinate, registry) ||
                   tiles.IsDoor(coordinate, registry) ||
                   tiles.IsFloor(coordinate, registry);
        }

        internal static Line ProjectLine(this Tiles tiles, Coordinate coordinate, DispatchRegistry registry, Compass4Points direction)
        {
            var line = new List<(string Id, Coordinate Coordinates)>();
            if (tiles.ShouldProject(coordinate, registry)) line.Add((tiles[coordinate], coordinate));

            bool continueProjection;
            do
            {
                coordinate = coordinate.Move(direction);

                continueProjection = tiles.ShouldProject(coordinate, registry);
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