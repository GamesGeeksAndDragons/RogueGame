using Assets.Actors;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Enums;
using Utils.Random;

namespace Assets.Rooms
{
    public class Tiles
    {
        private readonly ActorRegistry _registry;
        private readonly IRandomNumberGenerator _randomNumbers;
        private const int TilesPerBlock = 4;
        private readonly string[,] _tiles;

        public (int row, int column) UpperBounds => _tiles.UpperBounds();

        public Tiles(int blockRows, int blockColumns, ActorRegistry registry, IRandomNumberGenerator randomNumbers)
        {
            _registry = registry;
            _randomNumbers = randomNumbers;

            var maxRows = (blockRows + 1) * TilesPerBlock + 2;
            var maxCols = (blockColumns + 1) * TilesPerBlock + 2;

            _tiles = new string[maxRows, maxCols];
            DefaultTilesToRock(maxRows, maxCols);
        }

        private void DefaultTilesToRock(int maxRows, int maxColumns)
        {
            for (var row = 0; row < maxRows; row++)
            {
                for (var column = 0; column < maxColumns; column++)
                {
                    var rockCoordinates = new Coordinate(row, column);
                    _tiles.ThrowIfOutsideBounds(rockCoordinates, nameof(_tiles));

                    var rock = new Rock(rockCoordinates, _registry);
                    this[rock.Coordinates] = rock.UniqueId;
                }
            }
        }

        internal Tiles(Tiles rhs)
        {
            _registry = rhs._registry;
            _randomNumbers = rhs._randomNumbers;
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

        private WallDirection? GetWallDirection(Coordinate coordinate)
        {
            var surroundingSpace = this.DiscoverSurroundingSpace(coordinate);
            if (surroundingSpace == Compass8Points.Undefined) return null;

            if (surroundingSpace.IsCorner()) return surroundingSpace.ToWallDirection();
            if (surroundingSpace.IsHorizontal()) return WallDirection.Horizontal;
            if (surroundingSpace.IsVertical()) return WallDirection.Vertical;

            return null;
        }

        public void PopulateWall(Coordinate coordinate)
        {
            var wallDirection = GetWallDirection(coordinate);

            if (wallDirection.HasValue)
            {
                AddWall(coordinate, wallDirection.Value);
            }
        }

        private void EmptyTile(Coordinate coordinates)
        {
            _tiles.ThrowIfOutsideBounds(coordinates, nameof(_tiles));

            _registry.Deregister(coordinates);
            this[coordinates] = null;
        }

        private void AddWall(Coordinate coordindates, WallDirection direction)
        {
            this[coordindates].ThrowIfNull($"_tiles[{coordindates}]");

            var wall = new Wall(coordindates, _registry, direction);
            this[coordindates] = wall.UniqueId;
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

                    EmptyTile(coordindates);
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

        public Coordinate RandomEmptyTile()
        {
            var (maxRows, maxColumns) = UpperBounds;

            Coordinate coordinates;

            do
            {
                var row = _randomNumbers.Dice(maxRows);
                var col = _randomNumbers.Dice(maxColumns);

                coordinates = new Coordinate(row, col);
            } while (_tiles.IsInside(coordinates) && this[coordinates] != null);

            return coordinates;
        }
   }
}
