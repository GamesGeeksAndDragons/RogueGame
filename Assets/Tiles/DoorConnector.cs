using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Actors;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Enums;
using Utils.Exceptions;
using Line = System.Collections.Generic.List<(string Id, Utils.Coordinates.Coordinate Coordinates)>;
using ILine = System.Collections.Generic.IList<(string Id, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Tiles
{
    internal static class DoorConnector
    {
        internal static Tiles ConnectDoors(this Tiles tilesWithUnconnectedDoors, DispatchRegistry registry)
        {
            IList<Door> GetDoors()
            {
                var doorNames = tilesWithUnconnectedDoors.GetTilesOfType<Door>();
                return doorNames.Select(registry.GetDispatchee)
                    .Cast<Door>()
                    .ToList();
            }

            var doors = GetDoors();

            var tilesWithConnectedDoors = tilesWithUnconnectedDoors.Clone();

            while (doors.Count > 0)
            {
                var firstDoor = doors.First();
                var doorsToConnect = doors.Where(door => door.DoorId == firstDoor.DoorId).ToList();
                doorsToConnect.Count.ThrowIfNotEqual(2, "doorsToConnect.Count");

                var first = doorsToConnect[0];
                var second = doorsToConnect[1];

                tilesWithConnectedDoors = ConnectDoors(tilesWithConnectedDoors, first, second, registry);
                doorsToConnect.ForEach(door => doors.Remove(door));
            }

            return tilesWithConnectedDoors;
        }

        private static Tiles ConnectDoors(Tiles tilesWithUnconnectedDoors, Door first, Door second, DispatchRegistry registry)
        {
            var directions = GetDirectionsToProject(tilesWithUnconnectedDoors, first, registry);
            var line = ProjectLine(tilesWithUnconnectedDoors, first.Coordinates, directions.first);
            var segment = ProjectLine(tilesWithUnconnectedDoors, first.Coordinates, directions.second);
            foreach (var tile in segment)
            {
                line.Add(tile);
            }
            line.Add((first.UniqueId, first.Coordinates));
            line = line.OrderBy(tile => tile.Coordinates.Row + tile.Coordinates.Column).ToList();

            var doorInProjection = line.Any(tile => tile.Coordinates == second.Coordinates);
            if (doorInProjection)
            {
                var section = LineSection(line, first.Coordinates, second.Coordinates, directions.first);
                var stateChange = section.ToTilesState();
                var tilesWithNewlyConnectedDoors = tilesWithUnconnectedDoors.Clone(stateChange);
                return tilesWithNewlyConnectedDoors;
            }

            return tilesWithUnconnectedDoors;
        }

        private static (string Id, Coordinate Coordinates) Tunnel((string Id, Coordinate Coordinates) tile)
        {
            return (null, tile.Coordinates);
        }

        private static ILine LineSection(ILine projectedLine, Coordinate first, Coordinate second, Compass4Points direction)
        {
            if(direction== Compass4Points.Undefined) throw new ArgumentException("Direction in LineSection is undefined");

            bool IsBetween(Coordinate tileCoordinates)
            {
                if (direction == Compass4Points.North || direction == Compass4Points.South)
                {
                    return tileCoordinates.Row >= first.Row && tileCoordinates.Row <= second.Row;
                }

                return tileCoordinates.Column >= first.Column && tileCoordinates.Column <= second.Column;
            }

            return projectedLine
                .Where(tile => IsBetween(tile.Coordinates))
                .Select(Tunnel)
                .ToList();
        }

        private static ILine ProjectLine(Tiles tiles, Coordinate coordinate, Compass4Points direction)
        {
            bool continueProjection;
            var line = new Line();

            do
            {
                coordinate = coordinate.Move(direction);

                continueProjection = tiles.IsInside(coordinate) && !tiles.IsTileType<Wall>(coordinate);

                if (continueProjection)
                {
                    line.Add((tiles[coordinate], coordinate));
                }
            } while (continueProjection);

            return line;
        }

        private static bool IsWall(IDispatchee dispatchee)
        {
            if (dispatchee == null) return false;
            return dispatchee.Name == Wall.DispatcheeName;
        }

        private static (Compass4Points first, Compass4Points second) GetDirectionsToProject(Tiles tilesWithUnconnectedDoors, Door door, DispatchRegistry registry)
        {
            var start = door.Coordinates;
            var east = tilesWithUnconnectedDoors.GetDispatchee(start.Move(Compass4Points.East), registry);
            var west = tilesWithUnconnectedDoors.GetDispatchee(start.Move(Compass4Points.West), registry);

            if (IsWall(east) && IsWall(west))
                return (Compass4Points.North, Compass4Points.South);

            var north = tilesWithUnconnectedDoors.GetDispatchee(start.Move(Compass4Points.North), registry);
            var south = tilesWithUnconnectedDoors.GetDispatchee(start.Move(Compass4Points.South), registry);

            if (IsWall(north) && IsWall(south))
                return (Compass4Points.East, Compass4Points.West);

            throw new UnexpectedTileException($"Attempting to find the direction to project for door:coordinates [{door}:{start}]");
        }
    }
}
