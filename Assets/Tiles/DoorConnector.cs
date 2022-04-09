using System.Collections.Generic;
using System.Linq;
using Assets.Actors;
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Enums;
using Utils.Exceptions;
using Utils.Random;
using TilePosition = System.ValueTuple<string, Utils.Coordinates.Coordinate>;
using Line = System.Collections.Generic.List<(string Id, Utils.Coordinates.Coordinate Coordinates)>;
using ILine = System.Collections.Generic.IList<(string Id, Utils.Coordinates.Coordinate Coordinates)>;
using SearchDirections = System.ValueTuple<Utils.Enums.Compass4Points, Utils.Enums.Compass4Points>;

namespace Assets.Tiles
{
    public static class DoorConnector
    {
        public static Tiles ConnectDoors(this Tiles tilesWithUnconnectedDoors, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry, IDieBuilder dieBuilder)
        {
            IList<Door> GetDoors()
            {
                var doorNames = tilesWithUnconnectedDoors.GetTilesOfType<Door>();
                return doorNames.Select(dispatchRegistry.GetDispatchee)
                    .Cast<Door>()
                    .ToList();
            }

            var doors = GetDoors();

            var doorConnector = new DoorConnectorImpl(tilesWithUnconnectedDoors, dispatchRegistry, actionRegistry, dieBuilder);

            while (doors.Count > 0)
            {
                var firstDoor = doors.First();
                var doorsToConnect = doors.Where(door => door.DoorId == firstDoor.DoorId).ToList();
                doorsToConnect.Count.ThrowIfNotEqual(2, "doorsToConnect.Count");

                var first = doorsToConnect[0];
                var second = doorsToConnect[1];
                var firstDirection = GetDirectionToProject(first);
                var secondDirection = GetDirectionToProject(second);

                doorConnector.ConnectDoors(first, firstDirection, second, secondDirection);
                doorsToConnect.ForEach(door => doors.Remove(door));
            }

            return doorConnector.Tiles;

            Compass4Points GetDirectionToProject(Door door)
            {
                foreach (var point in CompassHelpers.ValidCompass4Points)
                {
                    var projectionDirection = SearchDirection(point);
                    if (projectionDirection != Compass4Points.Undefined) return projectionDirection;
                }

                throw new UnexpectedTileException($"Attempting to find the direction to project for door id:[{door.DoorId}] :coordinates [{door.Coordinates}]");

                Compass4Points SearchDirection(Compass4Points direction)
                {
                    var tile = tilesWithUnconnectedDoors.GetDispatchee(door.Coordinates.Move(direction), dispatchRegistry);
                    return tile.IsRock() ? direction : Compass4Points.Undefined;
                }
            }
        }
    }

    class DoorConnectorImpl
    {
        public Tiles Tiles { get; private set; }
        private readonly DispatchRegistry _dispatchRegistry;
        private readonly ActionRegistry _actionRegistry;
        private readonly IDieBuilder _dieBuilder;

        internal DoorConnectorImpl(Tiles tiles, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry, IDieBuilder dieBuilder)
        {
            Tiles = tiles;
            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;
            _dieBuilder = dieBuilder;
        }

        internal void ConnectDoors(Door firstDoor, Compass4Points firstDirection, Door secondDoor, Compass4Points secondDirection)
        {
            var doorProjector = new DoorProjector(Tiles, _dispatchRegistry, _dieBuilder);
            var path = doorProjector.FindProjection(firstDoor, firstDirection, secondDoor, secondDirection);
            TunnelThroughTiles(path);
        }

        private (ILine first, ILine second) GetOverlapping(ILine first, ILine second)
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

        private void TunnelThroughTiles(ILine projection)
        {
            var section = ExtractLine(projection);
            var stateChange = section.ToStateChange();
            Tiles = Tiles.Clone(stateChange);
        }

        private (string Id, Coordinate Coordinates) Tunnel((string Id, Coordinate Coordinates) tile)
        {
            var actor = ActorBuilder.Build<Floor>(tile.Coordinates, _dispatchRegistry, _actionRegistry, "");
            return (actor.UniqueId, actor.Coordinates);
        }

        private ILine ExtractLine(ILine projectedLine)
        {
            var extracted = projectedLine.Where(tile => Tiles.IsTileType<Rock>(tile.Coordinates, _dispatchRegistry)).ToList();
            return extracted
                .Select(Tunnel)
                .ToList();
        }

        private bool ShouldProject(Coordinate coordinate)
        {
            if (!Tiles.IsInside(coordinate)) return false;

            return Tiles.IsTileType<Rock>(coordinate, _dispatchRegistry) ||
                   Tiles.IsTileType<Door>(coordinate, _dispatchRegistry) ||
                   Tiles.IsTileType<Floor>(coordinate, _dispatchRegistry);
        }

        private ILine ProjectLine(Coordinate coordinate, Compass4Points direction)
        {
            var line = new Line();
            if(ShouldProject(coordinate)) line.Add((Tiles[coordinate], coordinate));

            bool continueProjection;
            do
            {
                coordinate = coordinate.Move(direction);

                continueProjection = ShouldProject(coordinate);
                if (continueProjection)
                {
                    line.Add((Tiles[coordinate], coordinate));
                }
            } while (continueProjection);

            return line;
        }
    }
}
