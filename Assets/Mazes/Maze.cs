using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Deeds;
using Assets.Messaging;
using Assets.Rooms;
using Assets.Tiles;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Enums;
using Utils.Random;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string Name, string Value)>;

namespace Assets.Mazes
{
    internal class Maze : Dispatchee<Maze>
    {
        internal Maze(DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry, IDieBuilder dieBuilder, int rows, int columns) 
            : base(Coordinate.NotSet, dispatchRegistry, actionRegistry)
        {
            dieBuilder.ThrowIfNull(nameof(dieBuilder));

            _dieBuilder = dieBuilder;

            Tiles = new Tiles.Tiles(rows, columns, DispatchRegistry, ActionRegistry, _dieBuilder);

            ActionRegistry.RegisterMaze(this);
        }

        private Maze(Maze maze) : base(maze.Coordinates, maze.DispatchRegistry, maze.ActionRegistry)
        {
            _dieBuilder = maze._dieBuilder;
            Tiles = maze.Tiles.Clone();
            ActionRegistry.RegisterMaze(this);
        }

        private Maze(DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry, IDieBuilder dieBuilder, Tiles.Tiles tiles) 
            : base(Coordinate.NotSet, dispatchRegistry, actionRegistry)
        {
            _dieBuilder = dieBuilder;
            Tiles = tiles.Clone();
            ActionRegistry.RegisterMaze(this);
        }

        private readonly IDieBuilder _dieBuilder;
        private Tiles.Tiles Tiles { get; }

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

        internal Maze PlaceInMaze(IDispatchee dispatchee, Coordinate coordinates)
        {
            var cloner = (ICloner<Maze>) dispatchee;

            var state = FormatState(coordinates);
            var actor = cloner.Clone(state);

            var newState = coordinates.ToParameter(actor.UniqueId);

            return Clone(newState);
        }

        public bool IsInMaze(string uniqueId) => Tiles.TileExists(uniqueId);

        protected internal override void RegisterActions()
        {
        }

        public override void UpdateState(Maze clone, ExtractedParameters state)
        {
            foreach (var change in state)
            {
                var coordinate = change.Value.ToCoordinates();
                clone.Tiles[coordinate] = change.Name;
            }

            base.UpdateState(clone, state);
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

        public void MoveImpl(ExtractedParameters parameters)
        {
            var dispatchee = parameters.GetDispatchee("Dispatchee", DispatchRegistry);
            var direction = parameters.ToValue<Compass8Points>("Direction");
            var newCoordinates = dispatchee.Coordinates.Move(direction);

            if (!Tiles.IsInside(newCoordinates)) return;
            if (!Tiles[newCoordinates].IsNullOrEmpty()) return;

            PlaceInMaze(dispatchee, newCoordinates);
        }

        public Maze PositionRoomsInMaze(IList<Room> roomsWithDoors)
        {
            var tiles = Tiles.PositionRoomsInTiles(roomsWithDoors);
            return new Maze(DispatchRegistry, ActionRegistry, _dieBuilder, tiles);
        }

        public Maze ConnectDoorsWithCorridors()
        {
            var tiles = Tiles.ConnectDoors(DispatchRegistry, ActionRegistry, _dieBuilder);
            return new Maze(DispatchRegistry, ActionRegistry, _dieBuilder, tiles);
        }

        internal IDispatchee RandomTile(Predicate<Coordinate> condition)
        {
            var randomCoordinates = Tiles.RandomTile(condition);
            return this[randomCoordinates];
        }
    }
}
