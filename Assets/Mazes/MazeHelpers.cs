#nullable enable
using Assets.Messaging;
using Assets.Resources;
using Assets.Tiles;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Display;
using Utils.Enums;
using TileChanges = System.Collections.Generic.List<(string UniqueId, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Mazes
{
    internal static class MazeHelpers
    {
        internal static TileChanges GetTiles<TTileType>(this string[,] tiles, Func<string, IDispatched> getDispatched)
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

                    var dispatched = getDispatched(uniqueId);
                    if(dispatched.Name != tileType) continue;

                    var tile = (UniqueId: uniqueId, Coordinates: new Coordinate(row, col));
                    tilesOfType.Add( tile );
                }
            }

            return tilesOfType;
        }

        internal static bool IsTileType<T>(this IMaze maze, Coordinate coordinates) 
            where T : Dispatched<T>
        {
            var dispatched = maze.GetDispatched(coordinates);
            return dispatched.IsTypeof<T>();
        }

        internal static bool IsDoor(this IMaze maze, Coordinate coordinates)
        {
            return maze.IsTileType<Door>(coordinates);
        }

        internal static bool IsRock(this IMaze maze, Coordinate coordinates)
        {
            return maze.IsTileType<Rock>(coordinates);
        }

        internal static bool IsFloor(this IMaze maze, Coordinate coordinates)
        {
            return maze.IsTileType<Floor>(coordinates);
        }

        public static (IDispatched Dispatched, Coordinate Coordinates) RandomRockTile(this IMaze maze, IList<string> checkedTiles)
        {
            return maze.RandomTile(dispatched => dispatched.IsRock(), checkedTiles);
        }

        public static (IDispatched Dispatched, Coordinate Coordinates) RandomFloorTile(this IMaze maze, IList<string> checkedTiles, bool isTunnelTile, bool isOccupied)
        {
            return maze.RandomTile(dispatched =>
            {
                if (!dispatched.IsFloor()) return false;

                var floor = (Floor) dispatched;
                
                if (isTunnelTile != floor.IsTunnel) return false;

                return IsOccupied() == isOccupied;

                bool IsOccupied()
                {
                    return ! floor.Contained.IsNull();
                }
            }, checkedTiles);
        }

        public static void DefaultTiles(this string[,] tiles, Func<IDispatched> resourceBuilder)
        {
            var (maxRows, maxColumns) = tiles.UpperBounds();

            for (var row = 0; row <= maxRows; row++)
            {
                for (var column = 0; column <= maxColumns; column++)
                {
                    var tile = tiles[row, column];
                    if (! tile.IsNullOrEmpty()) continue;

                    var rock = resourceBuilder();
                    tiles[row, column] = rock.UniqueId;
                }
            }
        }

        public static string[,] BuildDefaultTiles(int maxRows, int maxColumns, Func<IDispatched> actorBuilder)
        {
            var tiles = new string[maxRows,maxColumns];
            tiles.DefaultTiles(actorBuilder);
            return tiles;
        }


        public static void ConnectDoorsWithCorridors(this IMaze maze, TileChanges changes, IDispatchRegistry dispatchRegistry, IResourceBuilder builder)
        {
            var floorBuilder = builder.FloorBuilder();
            IDispatched FloorForTunnel() => floorBuilder(0, "");

            var tunnel = BuildTunnelTiles(changes);

            var replaced = maze.Replace(tunnel);

            dispatchRegistry.Unregister(replaced);
            
            TileChanges BuildTunnelTiles(TileChanges projectedLine)
            {
                var extracted = projectedLine.Where(tile => IsTileType<Rock>(maze, tile.Coordinates)).ToList();
                return extracted
                    .Select(Tunnel)
                    .ToList();
            }

            (string UniqueId, Coordinate Coordinates) Tunnel((string UniqueId, Coordinate Coordinates) tile)
            {
                var actor = FloorForTunnel();

                return (actor.UniqueId, tile.Coordinates);
            }
        }
    }
}
