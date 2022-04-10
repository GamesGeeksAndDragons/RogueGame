using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Actors;
using Assets.Deeds;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Enums;
using Utils.Random;
using TileChanges = System.Collections.Generic.List<(string Name, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Tiles
{
    class DoorConnectorImpl
    {
        public ITiles Tiles { get; }
        private readonly IDispatchRegistry _dispatchRegistry;
        private readonly IActionRegistry _actionRegistry;
        private readonly IDieBuilder _dieBuilder;

        internal DoorConnectorImpl(ITiles tiles, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder)
        {
            Tiles = new Tiles(tiles);
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

        private (IList<(string Id, Coordinate Coordinates)> first, IList<(string Id, Coordinate Coordinates)> second) GetOverlapping(IList<(string Id, Coordinate Coordinates)> first, IList<(string Id, Coordinate Coordinates)> second)
        {
            var firstOverlap = new List<(string Id, Coordinate Coordinates)>();
            var secondOverlap = new List<(string Id, Coordinate Coordinates)>();

            foreach (var firstTilePosition in first)
            {
                var secondTilePosition = second.FirstOrDefault(tilePosition => CoordinateExtensions.IsOverlapping(tilePosition.Coordinates, firstTilePosition.Coordinates));
                if (secondTilePosition != default(ValueTuple<string, Coordinate>))
                {
                    firstOverlap.Add(firstTilePosition);
                    secondOverlap.Add(secondTilePosition);
                }
            }

            return (firstOverlap, secondOverlap);
        }

        private void TunnelThroughTiles(IList<(string Id, Coordinate Coordinates)> projection)
        {
            var section = ExtractLine(projection);
            Tiles.Replace(section);
        }

        private (string Id, Coordinate Coordinates) Tunnel((string Id, Coordinate Coordinates) tile)
        {
            var actor = ActorBuilder.Build<Floor>(tile.Coordinates, _dispatchRegistry, _actionRegistry, "");
            return (actor.UniqueId, actor.Coordinates);
        }

        private TileChanges ExtractLine(IList<(string Id, Coordinate Coordinates)> projectedLine)
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

        private IList<(string Id, Coordinate Coordinates)> ProjectLine(Coordinate coordinate, Compass4Points direction)
        {
            var line = new List<(string Id, Coordinate Coordinates)>();
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