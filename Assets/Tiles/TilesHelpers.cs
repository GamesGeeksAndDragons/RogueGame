using System;
using System.Collections.Generic;
using Assets.Actors;
using Assets.Mazes;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Enums;
using Utils.Exceptions;

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

        internal static IDispatchee GetDispatchee(this Tiles tiles, Coordinate coordinates, DispatchRegistry registry)
        {
            if (!tiles.IsInside(coordinates) || tiles.IsEmptyTile(coordinates)) return null;

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

        internal static bool IsTileType<T>(this Tiles tiles, Coordinate coordinates, DispatchRegistry registry) 
            where T : Dispatchee<T>
        {
            var name = tiles[coordinates];
            if (name.IsNullOrEmpty()) return false;

            var dispatchee = registry.GetDispatchee(name);
            return dispatchee.IsTypeof<T>();
        }

        internal static bool IsDoor(this Tiles tiles, Coordinate coordinates, DispatchRegistry registry)
        {
            return tiles.IsTileType<Door>(coordinates, registry);
        }

        internal static bool IsRock(this Tiles tiles, Coordinate coordinates, DispatchRegistry registry)
        {
            return tiles.IsTileType<Rock>(coordinates, registry);
        }

        internal static bool IsFloor(this Tiles tiles, Coordinate coordinates, DispatchRegistry registry)
        {
            return tiles.IsTileType<Floor>(coordinates, registry);
        }
    }
}
