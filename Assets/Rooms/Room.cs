﻿#nullable enable
using Assets.Actors;
using Assets.Deeds;
using Assets.Maze;
using Assets.Mazes;
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
        (int Row, int Column) UpperBounds { get; }
    }

    internal class Room : IRoom
    {
        internal Room(string name, IMaze maze, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder, IActorBuilder actorBuilder)
        {
            _dieBuilder = dieBuilder;
            _actorBuilder = actorBuilder;
            DispatchRegistry = dispatchRegistry;
            ActionRegistry = actionRegistry;
            OriginalName = name;
            Name = $"{name}{++_counter}";

            Maze = maze;
        }

        internal Room(Room room, IMaze maze) : this(room.OriginalName, maze, room.DispatchRegistry, room.ActionRegistry, room._dieBuilder, room._actorBuilder)
        {
        }

        private readonly IDieBuilder _dieBuilder;
        private readonly IActorBuilder _actorBuilder;
        private static uint _counter;

        internal IDispatchRegistry DispatchRegistry { get; }
        internal IActionRegistry ActionRegistry { get; }

        public string OriginalName { get; }
        public string Name { get; }
        public (int Row, int Column) UpperBounds => Maze.UpperBounds;

        internal readonly IMaze Maze;

        public string this[Coordinate coordinate] => Maze[coordinate];

        public void AddDoor(int doorNumber)
        {
            var toReplaceWithDoor = FindWall();
            if (toReplaceWithDoor == default) return;

            var newDoor = _actorBuilder.Build(doorNumber.ToHexString());

            Maze[toReplaceWithDoor.Coordinates] = newDoor.UniqueId;
            DispatchRegistry.Unregister(toReplaceWithDoor.Wall.UniqueId);

            (Wall Wall, Coordinate Coordinates) FindWall()
            {
                var (maxRow, maxColumn) = Maze.UpperBounds;

                for (int i = 0; i < 10; i++)
                {
                    var (dispatched, coordinates) = Maze.RandomWallTile(WallDirection.NonCorner);

                    if (!IsOutsideWall(coordinates, maxRow, maxColumn)) continue;
                    if (IsNextToDoor(coordinates)) continue;

                    return ((Wall)dispatched, coordinates);
                }

                return default;
            }

            bool IsOutsideWall(Coordinate coordinates, int maxRow, int maxColumns)
            {
                return coordinates.Row == 0 ||
                       coordinates.Row == maxRow - 1 ||
                       coordinates.Column == 0 ||
                       coordinates.Column == maxColumns - 1;
            }

            bool IsNextToDoor(Coordinate coordinates)
            {
                var neighbouringCoordinates = coordinates.Surrounding4Coordinates();

                return neighbouringCoordinates.Any(coordinate =>
                {
                    var id = Maze[coordinates];
                    var dispatched = DispatchRegistry.GetDispatched(id);
                    return dispatched.IsDoor();
                });
            }
        }

        public (Coordinate topLeft, Coordinate topRight, Coordinate bottomLeft, Coordinate bottomRight) GetSize()
        {
            var (maxRow, maxColumn) = Maze.UpperBounds;

            var topLeft = new Coordinate(0, 0);
            var topRight = new Coordinate(0, maxColumn - 1);
            var bottomLeft = new Coordinate(maxRow - 1, 0);
            var bottomRight = new Coordinate(maxRow - 1, maxColumn - 1);

            return (topLeft, topRight, bottomLeft, bottomRight);
        }
    }
}
