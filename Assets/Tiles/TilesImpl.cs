﻿using System;
using System.Collections.Generic;
using Assets.Mazes;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;

namespace Assets.Tiles
{
    internal static class TilesImpl
    {
        internal static IList<(string Name, Coordinate Coordinates)> PopulateBlock(this RoomTiles tiles, int blockRow, int blockCol)
        {
            var rowOffset = blockRow * tiles.TilesPerBlock + 1;
            var colOffset = blockCol * tiles.TilesPerBlock + 1;
            var emptyTiles = new List<(string Name, Coordinate Coordinates)>();

            for (var row = 0; row < tiles.TilesPerBlock; row++)
            {
                for (var column = 0; column < tiles.TilesPerBlock; column++)
                {
                    var coordinates = new Coordinate(row + rowOffset, column + colOffset);

                    emptyTiles.Add((string.Empty, coordinates));
                }
            }

            return emptyTiles;
        }

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

        internal static IDispatchee GetDispatchee(this Tiles tiles, Coordinate coordinates,
            DispatchRegistry registry)
        {
            if (!tiles.IsInside(coordinates) || tiles.IsEmptyTile(coordinates)) return null;

            var uniqueId = tiles[coordinates];
            return registry.GetDispatchee(uniqueId);
        }
    }
}
