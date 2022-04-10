using System;
using System.Collections.Generic;
using Assets.Deeds;
using Assets.Messaging;
using Assets.Rooms;
using Assets.Tiles;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Random;
using TileChanges = System.Collections.Generic.List<(string Name, Utils.Coordinates.Coordinate Coordinates)>;

namespace Assets.Mazes
{
    public interface IMaze
    {
        IDispatchee this[Coordinate coordinate] { get; }
        string Name { get; }
        string UniqueId { get; }
        IDispatchee RandomTile(Predicate<IDispatchee> condition);
        void Update(TileChanges state);
        bool IsInMaze(string uniqueId);
    }

    internal class Maze : Dispatchee<Maze>, IMaze
    {
        internal Maze(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder, int rows, int columns) 
            : base(Coordinate.NotSet, dispatchRegistry, actionRegistry)
        {
            dieBuilder.ThrowIfNull(nameof(dieBuilder));

            _dieBuilder = dieBuilder;

            Tiles = new Tiles.Tiles(rows, columns, DispatchRegistry, ActionRegistry, _dieBuilder);

            ActionRegistry.RegisterTiles(Tiles);
        }

        private Maze(Maze maze) : base(maze.Coordinates, maze.DispatchRegistry, maze.ActionRegistry)
        {
            _dieBuilder = maze._dieBuilder;
            Tiles = maze.Tiles;
            ActionRegistry.RegisterTiles(Tiles);
        }

        private Maze(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder, ITiles tiles) 
            : base(Coordinate.NotSet, dispatchRegistry, actionRegistry)
        {
            _dieBuilder = dieBuilder;
            Tiles = tiles;
            ActionRegistry.RegisterTiles(Tiles);
        }

        private readonly IDieBuilder _dieBuilder;
        internal ITiles Tiles { get; private set; }

        public IDispatchee this[Coordinate coordinate]
        {
            get
            {
                var name = Tiles[coordinate];
                return DispatchRegistry.GetDispatchee(name);
            }
        }

        public override Maze Create()
        {
            return new Maze(this);
        }

        public override string ToString()
        {
            return Tiles.ToString();
        }

        public bool IsInMaze(string uniqueId) => Tiles.TileExists(uniqueId);

        protected internal override void RegisterActions()
        {
        }

        public void Update(TileChanges state)
        {
            Tiles.Replace(state);
        }

        internal Maze GrowMaze()
        {
            var bounds = Tiles.UpperBounds;
            var maze = new Maze(DispatchRegistry, ActionRegistry, _dieBuilder, bounds.Row*2, bounds.Column * 2);

            for (var row = 0; row <= bounds.Row; row++)
            {
                for (var column = 0; column <= bounds.Column; column++)
                {
                    var coordinate = new Coordinate(row, column);
                    maze.Tiles[coordinate] = Tiles[coordinate];
                }
            }

            return maze;
        }

        //public void MoveImpl(Parameters parameters)
        //{
        //    var dispatchee = parameters.GetDispatchee("Dispatchee", DispatchRegistry);
        //    var direction = parameters.ToValue<Compass8Points>("Direction");
        //    var newCoordinates = dispatchee.Coordinates.Move(direction);

        //    if (!Tiles.IsInside(newCoordinates)) return;
        //    if (!Tiles[newCoordinates].IsNullOrEmpty()) return;

        //    //PlaceInMaze(dispatchee, newCoordinates);
        //}

        public void PositionRoomsInMaze(IList<Room> roomsWithDoors)
        {
            Tiles = Tiles.PositionRoomsInTiles(roomsWithDoors);
            ActionRegistry.RegisterTiles(Tiles);
        }

        public Maze ConnectDoorsWithCorridors()
        {
            var tiles = Tiles.ConnectDoors(DispatchRegistry, ActionRegistry, _dieBuilder);
            return new Maze(DispatchRegistry, ActionRegistry, _dieBuilder, tiles);
        }

        public IDispatchee RandomTile(Predicate<IDispatchee> condition)
        {
            return Tiles.RandomTile(condition);
        }
    }
}
