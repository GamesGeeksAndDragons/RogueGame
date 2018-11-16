using System.Collections.Generic;
using Assets.Messaging;
using Assets.Tiles;
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
        private readonly Tiles.Tiles _tiles;

        internal Maze(DispatchRegistry registry, IRandomNumberGenerator randomNumbers, int rows, int columns) 
            : base(Coordinate.NotSet, registry)
        {
            randomNumbers.ThrowIfNull(nameof(randomNumbers));

            _randomNumbers = randomNumbers;

            _tiles = new Tiles.Tiles(rows, columns, Registry, _randomNumbers);
        }

        private Maze(Maze maze) : base(maze.Coordinates, maze.Registry)
        {
            _randomNumbers = maze._randomNumbers;
            _tiles = maze._tiles.Clone();
        }

        private Maze(DispatchRegistry registry, IRandomNumberGenerator randomNumbers, Tiles.Tiles tiles) : base(Coordinate.NotSet, registry)
        {
            _randomNumbers = randomNumbers;
            _tiles = tiles.Clone();
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

        public bool IsInMaze(string uniqueId) => _tiles.TileExists(uniqueId);

        protected internal override void RegisterActions()
        {
            RegisterAction(Actions.Teleport, TeleportImpl);
            RegisterAction(Actions.Move, MoveImpl);
        }

        public override void UpdateState(Maze clone, ExtractedParameters state)
        {
            if (state.HasValue(nameof(Tiles.Tiles)))
            {
                var tiles = state.ToString(nameof(Tiles.Tiles));
                var tilesChanged = tiles.ToTiles();

                foreach (var tileChange in tilesChanged)
                {
                    clone._tiles[tileChange.Coordinates] = tileChange.Name;
                }
            }

            base.UpdateState(clone, state);
        }

        internal Maze GrowMaze()
        {
            var bounds = _tiles.UpperBounds;
            var maze = new Maze(Registry, _randomNumbers, bounds.Row*2, bounds.Column * 2);

            for (var row = 0; row <= bounds.Row; row++)
            {
                for (var column = 0; column <= bounds.Column; column++)
                {
                    var coordinate = new Coordinate(row, column);
                    maze._tiles[coordinate] = _tiles[coordinate];
                }
            }

            return maze;
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
            var newCoordinates = dispatchee.Coordinates.Move(direction);

            if (!_tiles.IsInside(newCoordinates)) return;
            if (!_tiles[newCoordinates].IsNullOrEmpty()) return;

            PlaceInMaze(dispatchee, newCoordinates);
        }

        public Maze PositionRoomsInMaze(IList<Room> roomsWithDoors)
        {
            var tiles = _tiles.PositionRoomsInTiles(Registry, roomsWithDoors);
            return new Maze(Registry, _randomNumbers, tiles);
        }

        public Maze ConnectDoorsWithCorridors()
        {
            return this;
        }
    }
}
