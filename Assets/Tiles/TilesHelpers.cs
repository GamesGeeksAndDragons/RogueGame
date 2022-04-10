using System;
using System.Collections.Generic;
using Assets.Actors;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Enums;
using Utils.Exceptions;
using TileChanges = System.Collections.Generic.List<(string Name, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Tiles
{
    internal static class TilesHelpers
    {
        internal static IList<string> GetTilesOfType<TTileType>(this string[,] tiles, Func<string, IDispatchee> getDispatchee)
        {
            var tilesType = new List<string>();
            var tileType = typeof(TTileType).Name;

            var (rowMax, colMax) = tiles.UpperBounds();
            for (var row = 0; row <= rowMax; row++)
            {
                for (var col = 0; col <= colMax; col++)
                {
                    var name = tiles[row, col];
                    if (name.IsNullOrEmpty()) continue;

                    var tile = getDispatchee(name);
                    if (tile.Name == tileType) tilesType.Add(tile.UniqueId);
                }
            }

            return tilesType;
        }

        internal static IDispatchee GetDispatchee(this ITiles tiles, Coordinate coordinates, IDispatchRegistry registry)
        {
            if (!tiles.IsInside(coordinates)) return null;

            var uniqueId = tiles[coordinates];
            return registry.GetDispatchee(uniqueId);
        }

        internal static Compass4Points GetSearchDirectionsThroughRock(this Tiles tiles, Coordinate searchFrom, DispatchRegistry registry, Compass4Points searchDirection)
        {
            var tile = tiles.GetDispatchee(searchFrom.Move(searchDirection), registry);
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

        internal static bool IsTileType<T>(this ITiles tiles, Coordinate coordinates, IDispatchRegistry registry) 
            where T : Dispatchee<T>
        {
            var name = tiles[coordinates];
            if (name.IsNullOrEmpty()) return false;

            var dispatchee = registry.GetDispatchee(name);
            return dispatchee.IsTypeof<T>();
        }

        internal static bool IsDoor(this ITiles tiles, Coordinate coordinates, IDispatchRegistry registry)
        {
            return tiles.IsTileType<Door>(coordinates, registry);
        }

        internal static bool IsRock(this ITiles tiles, Coordinate coordinates, IDispatchRegistry registry)
        {
            return tiles.IsTileType<Rock>(coordinates, registry);
        }

        internal static bool IsFloor(this ITiles tiles, Coordinate coordinates, IDispatchRegistry registry)
        {
            return tiles.IsTileType<Floor>(coordinates, registry);
        }

        public static IDispatchee RandomFloorTile(this ITiles tiles, bool isOccupied = true)
        {
            return tiles.RandomTile(dispatchee =>
            {
                if (!dispatchee.IsFloor()) return false;

                var floor = (Floor) dispatchee;
                return IsOccupied() == isOccupied;

                bool IsOccupied()
                {
                    return floor.Contains != null;
                }
            });
        }
    }
}
