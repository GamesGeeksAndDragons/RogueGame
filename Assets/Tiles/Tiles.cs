using System.Collections.Generic;
using System.Linq;
using Assets.Actors;
using Assets.Rooms;
using Utils;
using Utils.Coordinates;
using Utils.Enums;

namespace Assets.Tiles
{
    public class Tiles
    {
        private const int TilesPerBlock = 4;
        private readonly Tile[,] _tiles;

        public int RowUpperBound => _tiles.RowUpperBound();
        public int ColumnUpperBound => _tiles.ColumnUpperBound();

        public Tiles(int blockRows, int blockColumns)
        {
            var maxRows = (blockRows + 1) * TilesPerBlock + 2;
            var maxCols = (blockColumns + 1) * TilesPerBlock + 2;

            _tiles = new Tile[maxRows, maxCols];
        }

        internal Tiles(Tiles rhs)
        {
            _tiles = rhs._tiles.CloneActors();
        }

        public Tile this[Coordinate point]
        {
            get => _tiles[point.Row, point.Column];
            set => _tiles[point.Row, point.Column] = value;
        }

        public Tiles Clone()
        {
            return new Tiles(this);
        }

        public override string ToString()
        {
            return _tiles.Print(tile =>
            {
                if (tile == null) return "";

                return tile.Actor == null ? "0" : tile.Actor.ToString();
            });
        }

        private WallDirection? GetWallType(Coordinate coordinate)
        {
            var surroundingTiles = this.ExamineSurroundingTiles(coordinate);

            if (surroundingTiles.IsCorner()) return surroundingTiles.Single().ToWallDirection();
            if (surroundingTiles.IsHorizontal()) return WallDirection.Horizontal;
            if (surroundingTiles.IsVertical()) return WallDirection.Vertical;

            return null;
        }

        public void PopulateWall(Coordinate coordinate)
        {
            var wallType = GetWallType(coordinate);

            if (wallType.HasValue)
            {
                this[coordinate] = new Tile(coordinate, new Wall(wallType.Value));
            }
        }

        public bool IsInside(Coordinate coordinate)
        {
            return _tiles.IsInside(coordinate);
        }

        public Tile[,] PopulateBlock(int blockRow, int blockCol)
        {
            var rowOffset = blockRow * TilesPerBlock + 1;
            var colOffset = blockCol * TilesPerBlock + 1;

            for (var row = 0; row < TilesPerBlock; row++)
            {
                for (var column = 0; column < TilesPerBlock; column++)
                {
                    var coordindates = new Coordinate(row + rowOffset, column + colOffset);

                    this[coordindates].ThrowIfNotNull($"_tiles[{coordindates}]");
                    this[coordindates] = new Tile(coordindates);
                }
            }

            return _tiles;
        }

        public bool IsTile(Coordinate coordinate)
        {
            return this[coordinate] != null;
        }

        public bool HasActor(Coordinate coordinate)
        {
            return this[coordinate].Actor != null;
        }
   }
}
