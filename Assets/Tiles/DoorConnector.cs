using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Actors;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Enums;
using Utils.Exceptions;
using TilePosition = System.ValueTuple<string, Utils.Coordinates.Coordinate>;
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

            var tilesWithConnectedDoors = tilesWithUnconnectedDoors;

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

        private static Tiles ConnectDoors(Tiles tilesWithUnconnectedDoors, Door firstDoor, Door secondDoor, DispatchRegistry registry)
        {
            bool IsDoor(Coordinate coordinates)
            {
                return tilesWithUnconnectedDoors.IsTileType<Door>(coordinates);
            }

            bool IsDoorToConnect(string doorId1, string doorId2, Coordinate coordinates)
            {
                var id = tilesWithUnconnectedDoors[coordinates];
                return id.IsSame(doorId1) || id.IsSame(doorId2);
            }

            ILine RemoveOtherDoorsInProjection(ILine projectedLine)
            {
                return projectedLine.Where(projection =>
                        ! IsDoor(projection.Coordinates) ||
                        IsDoorToConnect(firstDoor.UniqueId, secondDoor.UniqueId, projection.Coordinates))
                    .ToList();
            }

            var directions = GetDirectionsToProject(tilesWithUnconnectedDoors, firstDoor, registry);

            var firstProjection = ProjectLineFromDoor(tilesWithUnconnectedDoors, firstDoor, directions);
            firstProjection = RemoveOtherDoorsInProjection(firstProjection);

            var secondDoorInProjection = firstProjection.Any(tile => tile.Coordinates == secondDoor.Coordinates);
            if (secondDoorInProjection)
            {
                return TunnelThroughTiles(tilesWithUnconnectedDoors, firstDoor.Coordinates, secondDoor.Coordinates, firstProjection);
            }

            var secondProjection = ProjectLineFromDoor(tilesWithUnconnectedDoors, secondDoor, directions);
            secondProjection = RemoveOtherDoorsInProjection(secondProjection);

            var connectingLine = ConnectProjections(tilesWithUnconnectedDoors, firstDoor, secondDoor, firstProjection, secondProjection, directions);

            return ConnectDoorsViaConnectingLine(tilesWithUnconnectedDoors, firstProjection, secondProjection, connectingLine);
        }

        private static Tiles ConnectDoorsViaConnectingLine(Tiles tilesWithUnconnectedDoors, ILine firstProjection, ILine secondProjection, ILine connectingLine)
        {
            var startOfConnectingLine = connectingLine.First();
            var firstDoor = firstProjection.Single(tilePosition => tilesWithUnconnectedDoors.IsTileType<Door>(tilePosition.Coordinates));
            var fromFirstToConnecting = ExtractLineBetween(firstProjection, firstDoor.Coordinates, startOfConnectingLine.Coordinates);

            var endOfConnectingLine = connectingLine.Last();
            var secondDoor = secondProjection.Single(tilePosition => tilesWithUnconnectedDoors.IsTileType<Door>(tilePosition.Coordinates));
            var fromConnectingToSecond = ExtractLineBetween(secondProjection, secondDoor.Coordinates, endOfConnectingLine.Coordinates);

            var finalRoute = new Line();
            finalRoute.AddRange(fromFirstToConnecting);
            finalRoute.AddRange(connectingLine.Select(Tunnel));
            finalRoute.AddRange(fromConnectingToSecond);

            var stateChange = finalRoute.ToTilesState();
            var tilesWithNewlyConnectedDoors = tilesWithUnconnectedDoors.Clone(stateChange);
            return tilesWithNewlyConnectedDoors;
        }

        private static (ILine first, ILine second) GetOverlapping(ILine first, ILine second)
        {
            var firstOverlap = new Line();
            var secondOverlap = new Line();

            foreach (var firstTilePosition in first)
            {
                var secondTilePosition = second.FirstOrDefault(tilePosition => tilePosition.Coordinates.IsOverlapping(firstTilePosition.Coordinates));
                if (secondTilePosition != default(TilePosition))
                {
                    firstOverlap.Add(firstTilePosition);
                    secondOverlap.Add(secondTilePosition);
                }
            }

            return (firstOverlap, secondOverlap);
        }

        private static ILine ConnectProjections(Tiles tiles, Door firstDoor, Door secondDoor,
            ILine firstProjection, ILine secondProjection, (Compass4Points first, Compass4Points second) directions)
        {
            var updatedFirst = RemoveDoorsAndBuffer(firstDoor, firstProjection, directions);
            var updatedSecond = RemoveDoorsAndBuffer(secondDoor, secondProjection, directions);

            (updatedFirst, updatedSecond) = GetOverlapping(updatedFirst, updatedSecond);

            //var updatedDirections = SwapAxis(directions);

            var connectingLines = ProjectConnectingLines(tiles, updatedFirst, updatedSecond);
            var middleLine = connectingLines.Count / 2;
            if (middleLine == 0) return connectingLines.First();
            return connectingLines.Skip(middleLine).First();
        }

        private static List<ILine> ProjectConnectingLines(Tiles tiles, ILine firstProjection, ILine secondProjection)
        {
            var connectingLines = new List<ILine>();

            foreach (var firstTilePosition in firstProjection)
            {
                var secondTilePosition = secondProjection.FirstOrDefault(tilePosition => firstTilePosition.Coordinates.IsOverlapping(tilePosition.Coordinates));
                if (secondTilePosition != default(TilePosition))
                {
                    var connectingLine = ProjectLineBetween(tiles, firstTilePosition.Coordinates, secondTilePosition.Coordinates);
                    if (connectingLine.Count > 0)
                    {
                        connectingLines.Add(connectingLine);
                    }
                }
            }

            return connectingLines;
        }

        private static ILine ProjectLineBetween(Tiles tiles, Coordinate firstCoordinate, Coordinate secondCoordinate)
        {
            var direction = firstCoordinate.GetStraightDirectionOfTravel(secondCoordinate);
            direction.ThrowIfEqual(Compass4Points.Undefined, nameof(direction));

            var next = firstCoordinate;
            var isBlocked = !ShouldTunnel(tiles, next);
            var projected = new Line();
            while (!isBlocked && next != secondCoordinate)
            {
                var tileId = tiles[next];
                projected.Add( (tileId, next) );
                next = next.Move(direction);
                isBlocked = ! ShouldTunnel(tiles, next);
            }

            if (isBlocked)
            {
                projected.Clear();
            }
            else
            {
                var tileId = tiles[next];
                projected.Add((tileId, next));
            }

            return projected;
        }

        private static (Compass4Points first, Compass4Points second) SwapAxis((Compass4Points first, Compass4Points second) directions)
        {
            if (directions.first == Compass4Points.North && directions.second == Compass4Points.South)
            {
                return (Compass4Points.West, Compass4Points.East);
            }

            if (directions.first == Compass4Points.East && directions.second == Compass4Points.West)
            {
                return (Compass4Points.North, Compass4Points.South);
            }

            throw new ArgumentException(nameof(directions), $"Unexpected direction ([{directions.first}],[{directions.second}])");
        }

        private static ILine RemoveDoorsAndBuffer(Door door, ILine intersection,
            (Compass4Points first, Compass4Points second) directions)
        {
            var tilePositionsToIgnore = new Line
            {
                (null, door.Coordinates),
                (null, door.Coordinates.Move(directions.first)),
                (null, door.Coordinates.Move(directions.second)),
            };

            bool ShouldIgnore((string Id, Coordinate Coordinates) intersect)
            {
                return tilePositionsToIgnore.Any(ignorable => ignorable.Coordinates == intersect.Coordinates);
            }

            return intersection.Where(position => ! ShouldIgnore(position)).ToList();
        }

        private static Tiles TunnelThroughTiles(Tiles tilesWithUnconnectedDoors, Coordinate first, Coordinate second, ILine projection)
        {
            var section = ExtractLineBetween(projection, first, second);
            var stateChange = section.ToTilesState();
            var tilesWithNewlyConnectedDoors = tilesWithUnconnectedDoors.Clone(stateChange);
            return tilesWithNewlyConnectedDoors;
        }

        private static ILine ProjectLineFromDoor(Tiles tilesWithUnconnectedDoors, Door door, (Compass4Points first, Compass4Points second) directions)
        {
            var projection1 = ProjectLine(tilesWithUnconnectedDoors, door.Coordinates, directions.first);
            var projection2 = ProjectLine(tilesWithUnconnectedDoors, door.Coordinates, directions.second);
            var combined = CombineProjectionsAndDoor(door, projection1, projection2);
            return combined;
        }

        private static ILine CombineProjectionsAndDoor(Door door, ILine projection1, ILine projection2)
        {
            foreach (var tile in projection2)
            {
                projection1.Add(tile);
            }

            projection1.Add((door.UniqueId, door.Coordinates));
            projection1 = projection1.OrderBy(tile => tile.Coordinates.Row + tile.Coordinates.Column).ToList();
            return projection1;
        }

        private static (string Id, Coordinate Coordinates) Tunnel((string Id, Coordinate Coordinates) tile)
        {
            return (null, tile.Coordinates);
        }

        private static ILine ExtractLineBetween(ILine projectedLine, Coordinate first, Coordinate second)
        {
            var direction = first.GetStraightDirectionOfTravel(second);
            direction.ThrowIfEqual(Compass4Points.Undefined, nameof(direction));

            bool IsBetween(Coordinate tileCoordinates)
            {
                if (direction == Compass4Points.North || direction == Compass4Points.South)
                {
                    var minRow = Math.Min(first.Row, second.Row);
                    var maxRow = Math.Max(first.Row, second.Row);

                    return tileCoordinates.Row >= minRow && tileCoordinates.Row <= maxRow;
                }

                var minColumn = Math.Min(first.Column, second.Column);
                var maxColumn = Math.Max(first.Column, second.Column);
                return tileCoordinates.Column >= minColumn && tileCoordinates.Column <= maxColumn;
            }

            var extracted = projectedLine.Where(tile => IsBetween(tile.Coordinates)).ToList();
            return extracted
                .Select(Tunnel)
                .ToList();
        }

        private static bool ShouldTunnel(this Tiles tiles, Coordinate coordinate)
        {
            if (!tiles.IsInside(coordinate)) return false;

            return
                tiles.IsTileType<Rock>(coordinate) ||
                tiles.IsTileType<Door>(coordinate) ||
                tiles[coordinate].IsNullOrEmpty();
        }

        private static ILine ProjectLine(Tiles tiles, Coordinate coordinate, Compass4Points direction)
        {
            bool continueProjection;
            var line = new Line();

            do
            {
                coordinate = coordinate.Move(direction);

                continueProjection = ShouldTunnel(tiles, coordinate);
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
