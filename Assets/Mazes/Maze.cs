using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Enums;
using Utils.Random;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string name, string value)>;

namespace Assets.Mazes
{
    internal class Maze : Dispatchee<Maze>
    {
        private readonly IRandomNumberGenerator _randomNumbers;
        public Tiles Tiles;

        internal Maze(MazeBlocks blocks, DispatchRegistry registry, IRandomNumberGenerator randomNumbers) 
            : base(Coordinate.NotSet, registry)
        {
            _randomNumbers = randomNumbers;

            Tiles = new Tiles(blocks.RowUpperBound, blocks.ColumnUpperBound, Registry, _randomNumbers);
        }

        private Maze(Maze rhs) : this(rhs, rhs.Tiles)
        {
        }

        private Maze(Maze rhs, Tiles tiles) : base(rhs)
        {
            Tiles = tiles.Clone();
        }

        public override IDispatchee Clone(string parameters=null)
        {
            return new Maze(this);
        }

        internal Maze PopulateWithTiles(MazeBlocks blocks)
        {
            var tiles = new Tiles(Tiles);

            for (var blockRow = 0; blockRow <= blocks.RowUpperBound; blockRow++)
            {
                for (var blockCol = 0; blockCol <= blocks.ColumnUpperBound; blockCol++)
                {
                    var coordinate = new Coordinate(blockRow, blockCol);
                    if(blocks[coordinate])
                    {
                        tiles.PopulateBlock(blockRow, blockCol);
                    }
                }
            }

            return new Maze(this, tiles);
        }

        internal Maze PopulateWithWalls()
        {
            var tiles = new Tiles(Tiles);

            var (rowMax, colMax) = tiles.UpperBounds;
            for (var row = 0; row <= rowMax; row++)
            {
                for (var col = 0; col <= colMax; col++)
                {
                    var coordinate = new Coordinate(row, col);
                    if (tiles[coordinate] != null)
                    {
                        tiles.PopulateWall(coordinate);
                    }
                }
            }

            return new Maze(this, tiles);
        }

        public override string ToString()
        {
            return Tiles.ToString();
        }

        private void PlaceInMaze(IDispatchee dispatchee, Coordinate coordinates)
        {
            var parameters = $"Coordinates [{coordinates}]";
            var newDispatchee = dispatchee.Clone(parameters);

            var tiles = Tiles.Clone();
            if (tiles.IsInside(dispatchee.Coordinates))
            {
                tiles[dispatchee.Coordinates] = null;
            }

            tiles[newDispatchee.Coordinates] = newDispatchee.UniqueId;

            // ReSharper disable once ObjectCreationAsStatement
            new Maze(this, tiles);
        }

        protected internal override void RegisterActions()
        {
            RegisterAction(Actions.Teleport, TeleportImpl);
            RegisterAction(Actions.Move, MoveImpl);
        }

        public void TeleportImpl(ExtractedParameters parameters)
        {
            var dispatchee = parameters.GetDispatchee("Dispatchee", Registry);
            var coordinates = Tiles.RandomEmptyTile();

            PlaceInMaze(dispatchee, coordinates);
        }

        public void MoveImpl(ExtractedParameters parameters)
        {
            var dispatchee = parameters.GetDispatchee("Dispatchee", Registry);
            var direction = parameters.ToValue<Compass8Points>("Direction");
            var newCoordindates = dispatchee.Coordinates.Move(direction);

            if (!Tiles.IsInside(newCoordindates)) return;
            if (!Tiles[newCoordindates].IsNullOrEmpty()) return;

            PlaceInMaze(dispatchee, newCoordindates);
        }
    }
}
