using System;
using System.Collections.Generic;
using System.Text;
using Utils.Coordinates;

namespace Assets.Mazes
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
                    var coordindates = new Coordinate(row + rowOffset, column + colOffset);

                    emptyTiles.Add((string.Empty, coordindates));
                }
            }

            return emptyTiles;
        }

    }
}
