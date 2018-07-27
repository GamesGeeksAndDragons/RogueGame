using Assets.Actors;
using Assets.Messaging;
using Assets.Rooms;
using Utils;
using Utils.Coordinates;
using Utils.Enums;

namespace Assets.Tiles
{
    public class Tiles
    {
        private readonly ActorRegistry _registry;
        private const int TilesPerBlock = 4;
        private readonly string[,] _tiles;

        public int RowUpperBound => _tiles.RowUpperBound();
        public int ColumnUpperBound => _tiles.ColumnUpperBound();

        public Tiles(int blockRows, int blockColumns, ActorRegistry registry)
        {
            _registry = registry;

            var maxRows = (blockRows + 1) * TilesPerBlock + 2;
            var maxCols = (blockColumns + 1) * TilesPerBlock + 2;

            _tiles = new string[maxRows, maxCols];
        }

        internal Tiles(Tiles rhs)
        {
            _registry = rhs._registry;
            _tiles = rhs._tiles.CloneStrings();
        }

        public string this[Coordinate point]
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
            return _tiles.Print(uniqueId =>
            {
                if (uniqueId == null) return "";

                var actor = _registry.GetActor(uniqueId);

                return actor.ToString();
            });
        }

        private WallDirection? GetWallType(Coordinate coordinate)
        {
            var surroundingTiles = this.ExamineSurroundingTiles(coordinate);
            if (surroundingTiles == Compass8Points.Undefined) return null;

            if (surroundingTiles.IsCorner()) return surroundingTiles.ToWallDirection();
            if (surroundingTiles.IsHorizontal()) return WallDirection.Horizontal;
            if (surroundingTiles.IsVertical()) return WallDirection.Vertical;

            return null;
        }

        public void PopulateWall(Coordinate coordinate)
        {
            var wallType = GetWallType(coordinate);

            if (wallType.HasValue)
            {
                AddTile(coordinate, new Wall(coordinate, wallType.Value));
            }
        }

        private void AddTile(Coordinate coordindates, Actor actor)
        {
            this[coordindates].ThrowIfNotNull($"_tiles[{coordindates}]");
            _registry.Register(actor);
            this[coordindates] = actor.UniqueId;
        }

        public bool IsInside(Coordinate coordinate)
        {
            return _tiles.IsInside(coordinate);
        }

        public string[,] PopulateBlock(int blockRow, int blockCol)
        {
            var rowOffset = blockRow * TilesPerBlock + 1;
            var colOffset = blockCol * TilesPerBlock + 1;

            for (var row = 0; row < TilesPerBlock; row++)
            {
                for (var column = 0; column < TilesPerBlock; column++)
                {
                    var coordindates = new Coordinate(row + rowOffset, column + colOffset);

                    AddTile(coordindates, new Tile(coordindates));
                }
            }

            return _tiles;
        }

        private string ActorType(Coordinate coordinate)
        {
            var id = this[coordinate];
            if (id.IsNullOrEmpty()) return string.Empty;

            var actor = _registry.GetActor(id);
            if (actor == null) return string.Empty;

            return actor.Name;
        }

        public bool IsTile(Coordinate coordinate)
        {
            return ActorType(coordinate) == "TILE";
        }

        public bool IsWall(Coordinate coordinate)
        {
            return ActorType(coordinate) == "WALL";
        }
   }
}
