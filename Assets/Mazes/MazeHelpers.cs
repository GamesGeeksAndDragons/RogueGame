#nullable enable
using Assets.Messaging;
using Assets.Resources;
using Assets.Tiles;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Display;
using Utils.Random;
using TilesType = System.Collections.Generic.List<(string UniqueId, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Mazes;

internal static class MazeHelpers
{
    internal static TilesType GetTiles<TTileType>(this string[,] tiles)
    {
        var tilesOfType = new TilesType();
        var type = typeof(TTileType);
        var tileType = type.Name;
        if (type.IsInterface && tileType[0] == 'I') tileType = tileType.Right(tileType.Length - 1);

        var (rowMax, colMax) = tiles.UpperBounds();
        for (var row = 0; row <= rowMax; row++)
        {
            for (var col = 0; col <= colMax; col++)
            {
                var uniqueId = tiles[row, col];
                if (uniqueId.IsNullOrEmpty()) continue;

                var isType = uniqueId.StartsWith(tileType);
                if (!isType) continue;

                var tile = (UniqueId: uniqueId, Coordinates: new Coordinate(row, col));
                tilesOfType.Add(tile);
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

    public static (IFloor Tile, Coordinate Coordinates) RandomFloorTile(this IMaze maze, IDieBuilder dieBuilder, IList<string> checkedTiles, bool isTunnelTile)
    {
        var floorTiles = maze.GetTiles<IFloor>(tile =>
            tile.Tile.IsTunnel == isTunnelTile &&
            !checkedTiles.Contains(tile.Tile.UniqueId));

        var randomIndex = dieBuilder.Between(1, floorTiles.Count).Random - 1;
        var randomTile = floorTiles[randomIndex];
        checkedTiles.Add(randomTile.Tile.UniqueId);
        return randomTile;
    }

    public static void DefaultTiles(this string[,] tiles, Func<IDispatched> resourceBuilder)
    {
        var (maxRows, maxColumns) = tiles.UpperBounds();

        for (var row = 0; row <= maxRows; row++)
        {
            for (var column = 0; column <= maxColumns; column++)
            {
                var tile = tiles[row, column];
                if (!tile.IsNullOrEmpty()) continue;

                var rock = resourceBuilder();
                tiles[row, column] = rock.UniqueId;
            }
        }
    }

    public static void ConnectDoorsWithCorridors(this IMaze maze, TilesType changes, IDispatchRegistry dispatchRegistry, IResourceBuilder builder)
    {
        var floorBuilder = builder.FloorBuilder();
        IDispatched FloorForTunnel() => floorBuilder(TilesDisplay.Tunnel, "");

        var tunnel = BuildTunnelTiles(changes);

        var replaced = maze.Replace(tunnel);

        dispatchRegistry.Unregister(replaced);

        TilesType BuildTunnelTiles(TilesType projectedLine)
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
