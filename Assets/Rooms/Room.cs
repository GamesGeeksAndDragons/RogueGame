﻿#nullable enable
using Assets.Deeds;
using Assets.Mazes;
using Assets.Messaging;
using Assets.Resources;
using Assets.Tiles;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Display;
using Utils.Enums;
using Utils.Random;

namespace Assets.Rooms
{
    interface IRoom
    {
        string Name { get; }
        (int Row, int Column) UpperBounds { get; }
    }

    internal class Room : Dispatched<Room>, IRoom
    {
        internal Room(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IDieBuilder dieBuilder, IResourceBuilder resourceBuilder, IMaze maze)
        : base(dispatchRegistry, actionRegistry, TilesDisplay.Room)
        {
            _dieBuilder = dieBuilder;
            _resourceBuilder = resourceBuilder;
            _doorBuilder = _resourceBuilder.DoorBuilder();

            Maze = maze;
        }

        internal Room(Room room, IMaze maze) 
            : this(room.DispatchRegistry, room.ActionRegistry, room._dieBuilder, room._resourceBuilder, maze)
        {
        }

        private readonly IDieBuilder _dieBuilder;
        private readonly IResourceBuilder _resourceBuilder;

        public (int Row, int Column) UpperBounds => Maze.UpperBounds;

        internal readonly IMaze Maze;
        private readonly Func<int, string, IDispatched> _doorBuilder;

        public string this[Coordinate coordinate] => Maze[coordinate];

        public void AddDoor(int doorNumber)
        {
            var toReplaceWithDoor = FindWall();
            if (toReplaceWithDoor == default) return;

            var newDoor = _doorBuilder(doorNumber, "");

            Maze[toReplaceWithDoor.Coordinates] = newDoor.UniqueId;
            DispatchRegistry.Unregister(toReplaceWithDoor.Wall.UniqueId);

            (Wall Wall, Coordinate Coordinates) FindWall()
            {
                var (maxRow, maxColumn) = Maze.UpperBounds;

                const int maxRetries = 50;
                for (int i = 0; i < maxRetries; i++)
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

                return neighbouringCoordinates.Any(neighbouring=>
                {
                    if(! Maze.IsInside(neighbouring)) return false;

                    var neighbouringId = Maze[neighbouring];
                    var dispatched = DispatchRegistry.GetDispatched(neighbouringId);
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
