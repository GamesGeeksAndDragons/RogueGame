using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Actors;
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Enums;
using Utils.Random;

namespace Assets.Rooms
{
    interface IRoom
    {
        string Name { get; }
        int NumberColumns { get; }
        int NumberRows { get; }
        Room Rotate(int times);
        IEnumerable<Door> Doors { get; }
        void AddDoor(int doorNumber);
    }

    internal class Room : IRoom
    {
        internal Room(string name, IDispatchee[,] tiles, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder, IActorBuilder actorBuilder)
        {
            _dieBuilder = dieBuilder;
            _actorBuilder = actorBuilder;
            DispatchRegistry = dispatchRegistry;
            ActionRegistry = actionRegistry;
            Name = $"{name}{++_counter}";

            _tiles = tiles;

            SetRowAndColumnNumbers();
        }

        protected Room(Room room, IDispatchee[,] newTiles, Door newDoor)
        {
            _dieBuilder = room._dieBuilder;
            DispatchRegistry = room.DispatchRegistry;
            ActionRegistry = room.ActionRegistry;
            LoadFolder = room.LoadFolder;
            Name = room.Name;

            _doors.AddRange( room.Doors.Select(door => new Door(door)) );
            if(newDoor != null) _doors.Add(newDoor);

            _tiles = newTiles;

            SetRowAndColumnNumbers();
        }

        private void SetRowAndColumnNumbers()
        {
            NumberColumns = _tiles.ColumnUpperBound() + 1;
            NumberRows = _tiles.RowUpperBound() + 1;
        }

        public Room Create(IDispatchee[,] newTiles)
        {
            return new Room(this, newTiles, null);
        }

        private readonly IDieBuilder _dieBuilder;
        private readonly IActorBuilder _actorBuilder;
        private readonly List<Door> _doors = new List<Door>();
        private static uint _counter;

        internal IDispatchRegistry DispatchRegistry { get; }
        internal IActionRegistry ActionRegistry { get; }

        public string LoadFolder { get; internal set; }
        public string Name { get; private set; }
        public int NumberColumns { get; private set; }
        public int NumberRows { get; private set; }
        private readonly IDispatchee[,] _tiles;

        public IEnumerable<Door> Doors => _doors;

        public IDispatchee this[Coordinate coordinate] => _tiles[coordinate.Row, coordinate.Column];

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (var rowIndex = 0; rowIndex < NumberRows; rowIndex++)
            {
                var row = _tiles.ExtractRow(rowIndex);
                foreach (var dispatchee in row)
                {
                    var display = dispatchee.ToString();
                    sb.Append(display);
                }
                sb.AppendLine();
            }

            return sb.ToString(0, sb.Length - Environment.NewLine.Length);
        }

        private Room Rotate(int rotatedNumberRows, int rotatedNumberColumns)
        {
            var rotatedRoom = new IDispatchee[rotatedNumberRows, rotatedNumberColumns];

            for (int rowIndex = 0; rowIndex < NumberRows; rowIndex++)
            {
                var row = _tiles.SliceRow(rowIndex).ToArray();

                rotatedNumberColumns--;
                for (int columnIndex = 0; columnIndex < row.Length; columnIndex++)
                {
                    var tile = row[columnIndex];
                    if (tile.IsWall())
                    {
                        var rotatedTile = (Wall)tile;
                        tile = rotatedTile.Rotate();
                    }

                    rotatedRoom[columnIndex, rotatedNumberColumns] = tile;
                }
            }

            return Create(rotatedRoom);
        }

        public Room Rotate(int times)
        {
            times.ThrowIfAbove(3, $"Attempted to rotate a room {times} times.  No need to rotate more than 3 times.");

            var rotated = this;
            for (int i = 0; i < times; i++)
            {
                var rotatedNumberColumns = rotated.NumberRows;
                var rotatedNumberRows = rotated.NumberColumns;
                rotated = rotated.Rotate(rotatedNumberRows, rotatedNumberColumns);
            }

            return rotated;
        }

        public void AddDoor(int doorNumber)
        {
            var walls = WallsWhichCanHaveDoors().ToList();

            var wallToReplaceWithDoor = SelectWall();
            if (wallToReplaceWithDoor == default) return;

            var newDoor = _actorBuilder.Build<Door>(doorNumber.ToString());

            _tiles[wallToReplaceWithDoor.Coordinates.Row, wallToReplaceWithDoor.Coordinates.Column] = newDoor;

            bool IsOutsideWall(Coordinate coordinates)
            {
                return coordinates.Row == 0 ||
                       coordinates.Row == NumberRows - 1 ||
                       coordinates.Column == 0 ||
                       coordinates.Column == NumberColumns - 1;
            }

            bool IsNextToDoor(Coordinate coordinates)
            {
                var neighbouringCoordinates = coordinates.Surrounding4Coordinates();

                return Doors.Any(door => neighbouringCoordinates.Any(neighbour => neighbour == coordinates));
            }

            IEnumerable<(Coordinate Coordinates, Wall Wall)> WallsWhichCanHaveDoors()
            {
                var wallsWhichCanHaveDoors = new List<(Coordinate Coordinates, Wall Wall)>();

                var (maxRow, maxColumns) = _tiles.UpperBounds();
                for (int row = 0; row < maxRow; row++)
                {
                    for (int column = 0; column < maxColumns; column++)
                    {
                        var coordinates = new Coordinate(row, column);
                        var dispatchee = this[coordinates];

                        if (!dispatchee.IsWall()) continue;

                        var wall = (Wall)dispatchee;
                        if (wall.IsCorner) continue;

                        if (!IsOutsideWall(coordinates)) continue;
                        if (IsNextToDoor(coordinates)) continue;

                        wallsWhichCanHaveDoors.Add((coordinates, wall));
                    }
                }

                return wallsWhichCanHaveDoors;
            }

            (Coordinate Coordinates, Wall Wall) SelectWall()
            {
                if (walls.Count == 0) return default;

                var index = walls.Count == 1 ? 0 : _dieBuilder.Dice(walls.Count - 1).Random;
                var selectedWall = walls[index];

                return selectedWall;
            }

        }

        public (Coordinate topLeft, Coordinate topRight, Coordinate bottomLeft, Coordinate bottomRight) GetSize()
        {
            var topLeft = new Coordinate(0, 0);
            var topRight = new Coordinate(0, NumberColumns - 1);
            var bottomLeft = new Coordinate(NumberRows - 1, 0);
            var bottomRight = new Coordinate(NumberRows - 1, NumberColumns - 1);

            return (topLeft, topRight, bottomLeft, bottomRight);
        }
    }
}
