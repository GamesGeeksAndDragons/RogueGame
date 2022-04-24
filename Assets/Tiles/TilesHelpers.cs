#nullable enable
using Assets.Actors;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Enums;
using Utils.Exceptions;
using TileChanges = System.Collections.Generic.List<(string UniqueId, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Tiles
{
    internal static class TilesHelpers
    {
        internal static TileChanges GetTilesOfType<TTileType>(this string[,] tiles, Func<string, IDispatched> getDispatchee)
        {
            var tilesOfType = new TileChanges();
            var tileType = typeof(TTileType).Name;

            var (rowMax, colMax) = tiles.UpperBounds();
            for (var row = 0; row <= rowMax; row++)
            {
                for (var col = 0; col <= colMax; col++)
                {
                    var uniqueId = tiles[row, col];
                    if (uniqueId.IsNullOrEmpty()) continue;

                    var dispatched = getDispatchee(uniqueId);
                    if(dispatched.Name != tileType) continue;

                    var tile = (UniqueId: uniqueId, Coordinates: new Coordinate(row, col));
                    tilesOfType.Add( tile );
                }
            }

            return tilesOfType;
        }

        internal static IDispatched GetDispatched(this ITiles tiles, Coordinate coordinates, IDispatchRegistry registry)
        {
            if (!tiles.IsInside(coordinates)) 
                throw new ArgumentException($"Request for coordinates [{coordinates}] not found inside tiles with bounds [{tiles.UpperBounds}]");

            var uniqueId = tiles[coordinates];
            return registry.GetDispatched(uniqueId);
        }

        internal static Compass4Points GetSearchDirectionsThroughRock(this Tiles tiles, Coordinate searchFrom, DispatchRegistry registry, Compass4Points searchDirection)
        {
            var tile = tiles.GetDispatched(searchFrom.Move(searchDirection), registry);
            return tile.IsRock() ? searchDirection : Compass4Points.Undefined;
        }

        internal static Compass4Points GetSearchDirectionsThroughRock(this Tiles tiles, Coordinate searchFrom, DispatchRegistry registry, (Compass4Points first, Compass4Points second) searchDirections)
        {
            var searchedDirection = tiles.GetSearchDirectionsThroughRock(searchFrom, registry, searchDirections.first);
            if (searchedDirection != Compass4Points.Undefined) return searchedDirection;

            searchedDirection = tiles.GetSearchDirectionsThroughRock(searchFrom, registry, searchDirections.second);
            if (searchedDirection != Compass4Points.Undefined) return searchedDirection;

            throw new UnexpectedTileException($"Attempting to find the direction through rock for coordinates [{searchFrom}]");
        }

        internal static bool IsTileType<T>(this ITiles tiles, Coordinate coordinates) 
            where T : Dispatched<T>
        {
            var dispatchee = tiles.GetDispatched(coordinates);
            return dispatchee.IsTypeof<T>();
        }

        internal static bool IsDoor(this ITiles tiles, Coordinate coordinates)
        {
            return tiles.IsTileType<Door>(coordinates);
        }

        internal static bool IsRock(this ITiles tiles, Coordinate coordinates)
        {
            return tiles.IsTileType<Rock>(coordinates);
        }

        internal static bool IsFloor(this ITiles tiles, Coordinate coordinates)
        {
            return tiles.IsTileType<Floor>(coordinates);
        }

        public static (IDispatched Dispatched, Coordinate Coordinates) RandomRockTile(this ITiles tiles)
        {
            return tiles.RandomTile(dispatched => dispatched.IsRock());
        }

        public static (IDispatched Dispatched, Coordinate Coordinates) RandomWallTile(this ITiles tiles, WallDirection onlyThese = WallDirection.All)
        {
            return tiles.RandomTile(dispatched =>
            {
                if (!dispatched.IsWall()) return false;

                var wall = (Wall)dispatched;
                return onlyThese.HasDirection(wall.WallType);
            });
        }

        public static (IDispatched Dispatched, Coordinate Coordinates) RandomFloorTile(this ITiles tiles, bool isOccupied = true)
        {
            return tiles.RandomTile(dispatched =>
            {
                if (!dispatched.IsFloor()) return false;

                var floor = (Floor) dispatched;
                return IsOccupied() == isOccupied;

                bool IsOccupied()
                {
                    return ! floor.Contained.IsNull();
                }
            });
        }

        public static void ConnectDoorsWithCorridors(this ITiles tiles, TileChanges changes, IDispatchRegistry dispatchRegistry, IActorBuilder builder)
        {
            var tunnel = BuildTunnelTiles(changes);

            var replaced = tiles.Replace(tunnel);

            dispatchRegistry.Unregister(replaced);

            TileChanges BuildTunnelTiles(TileChanges projectedLine)
            {
                var extracted = projectedLine.Where(tile => IsTileType<Rock>(tiles, tile.Coordinates)).ToList();
                return extracted
                    .Select(Tunnel)
                    .ToList();
            }

            (string UniqueId, Coordinate Coordinates) Tunnel((string UniqueId, Coordinate Coordinates) tile)
            {
                var actor = builder.Build<Floor>();
                return (actor.UniqueId, tile.Coordinates);
            }
        }
    }
}
