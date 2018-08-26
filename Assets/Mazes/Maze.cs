using System.Collections.Generic;
using System.Linq;
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
        private IRandomNumberGenerator _randomNumbers;
        private readonly MazeTiles _tiles;

        internal Maze(DispatchRegistry registry, IRandomNumberGenerator randomNumbers, int rows, int columns) 
            : base(Coordinate.NotSet, registry)
        {
            randomNumbers.ThrowIfNull(nameof(randomNumbers));

            _randomNumbers = randomNumbers;

            _tiles = new MazeTiles(rows, columns, Registry, _randomNumbers);
        }

        private Maze(Maze maze) : base(maze.Coordinates, maze.Registry)
        {
            _randomNumbers = maze._randomNumbers;
            _tiles = (MazeTiles)maze._tiles.Clone();
        }

        public override Maze Create()
        {
            return new Maze(this);
        }

        public override string ToString()
        {
            return _tiles.ToString();
        }

        private Maze PlaceInMaze(IDispatchee dispatchee, Coordinate coordinates)
        {
            var cloner = (ICloner<Maze>) dispatchee;

            var state = FormatState(coordinates);
            var actor = cloner.Clone(state);

            var newState = coordinates.ToTileState(actor.UniqueId);

            return Clone(newState);
        }

        public bool IsInsideMaze(Coordinate coordinates) => _tiles.IsInside(coordinates);
        public string TileName(Coordinate coordinates) => _tiles[coordinates];
        public bool IsTyleType<TTyleType>(Coordinate coordinates) => _tiles.IsTyleType<TTyleType>(coordinates);
        public Coordinate RandomRockTile() => _tiles.RandomRockTile();
        public (int MaxRows, int MaxColumns) MazeUpperBounds => _tiles.UpperBounds;
        public IDispatchee GetDispatchee(string uniqueId) => Registry.GetDispatchee(uniqueId);
        public IList<string> GetTiles<TTyleType>() => _tiles.GetTilesOfType<TTyleType>();
        public int RandomNumber(int max) { return _randomNumbers.Dice(max); }
        public int RandomBetween(int max) { return _randomNumbers.Between(1, max); }
        public bool IsInMaze(string uniqueId) => _tiles.TileExists(uniqueId);

        protected internal override void RegisterActions()
        {
            RegisterAction(Actions.Teleport, TeleportImpl);
            RegisterAction(Actions.Move, MoveImpl);
        }

        public override void UpdateState(Maze clone, ExtractedParameters state)
        {
            if (state.HasValue(nameof(Tiles)))
            {
                var tiles = state.ToString(nameof(Tiles));
                var tilesChanged = tiles.ToTiles();

                foreach (var tileChange in tilesChanged)
                {
                    clone._tiles[tileChange.Coordinates] = tileChange.Name;
                }
            }

            base.UpdateState(clone, state);
        }

        public void TeleportImpl(ExtractedParameters parameters)
        {
            var dispatchee = parameters.GetDispatchee("Dispatchee", Registry);
            var coordinates = _tiles.RandomEmptyTile();

            PlaceInMaze(dispatchee, coordinates);
        }

        public void MoveImpl(ExtractedParameters parameters)
        {
            var dispatchee = parameters.GetDispatchee("Dispatchee", Registry);
            var direction = parameters.ToValue<Compass8Points>("Direction");
            var newCoordindates = dispatchee.Coordinates.Move(direction);

            if (!_tiles.IsInside(newCoordindates)) return;
            if (!_tiles[newCoordindates].IsNullOrEmpty()) return;

            PlaceInMaze(dispatchee, newCoordindates);
        }
    }
}
