﻿using System.Collections.Generic;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Enums;
using Utils.Random;

namespace Assets.Mazes
{
    internal class RoomTiles : Tiles.Tiles
    {
        internal readonly int TilesPerBlock;

        public RoomTiles(int blockRows, int blockColumns, int tilesPerBlock, DispatchRegistry registry, IRandomNumberGenerator randomNumbers)
        : base((blockRows + 1) * tilesPerBlock + 2, (blockColumns + 1) * tilesPerBlock + 2, registry, randomNumbers)
        {
            randomNumbers.ThrowIfNull(nameof(randomNumbers));
            registry.ThrowIfNull(nameof(registry));
            blockRows.ThrowIfBelow(0, nameof(blockRows));
            blockColumns.ThrowIfBelow(0, nameof(blockColumns));
            tilesPerBlock.ThrowIfBelow(1, nameof(tilesPerBlock));

            TilesPerBlock = tilesPerBlock;
       }

        private RoomTiles(RoomTiles tiles) : base(tiles)
        {
            TilesPerBlock = tiles.TilesPerBlock;
        }

        private WallDirection? GetWallDirection(Coordinate coordinate)
        {
            var surroundingSpace = this.DiscoverSurroundingSpace(coordinate);
            if (surroundingSpace == Compass8Points.Undefined) return null;

            if (surroundingSpace.IsCorner()) return surroundingSpace.ToWallDirection();
            if (surroundingSpace.IsHorizontal()) return WallDirection.Horizontal;
            if (surroundingSpace.IsVertical()) return WallDirection.Vertical;

            return null;
        }

        public void PopulateWall(Coordinate coordinate, IList<(string Name, Coordinate coordinates)> wallChanges)
        {
            var wallDirection = GetWallDirection(coordinate);

            if (wallDirection.HasValue)
            {
                var wall = CreateWall(coordinate, wallDirection.Value);
                wallChanges.Add((wall.UniqueId, coordinate));
            }
        }

        public override Tiles.Tiles Create()
        {
            return new RoomTiles(this);
        }
    }
}
