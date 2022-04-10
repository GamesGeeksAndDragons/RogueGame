using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Deeds;
using Assets.Messaging;
using Assets.Rooms;
using log4net;
using Utils;
using Utils.Random;

namespace Assets.Mazes
{
    public class MazeBuilder
    {
        private readonly IDieBuilder _dieBuilder;
        private readonly RoomBuilder _roomBuilder;
        private readonly IMazeDescriptor _descriptor;
        private readonly ILog _logger;
        private readonly DispatchRegistry _dispatchRegistry;
        private readonly ActionRegistry _actionRegistry;

        internal MazeBuilder(IDieBuilder dieBuilder, RoomBuilder roomBuilder, ILog logger, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry)
        {
            dieBuilder.ThrowIfNull(nameof(dieBuilder));
            roomBuilder.ThrowIfNull(nameof(roomBuilder));
            logger.ThrowIfNull(nameof(logger));
            dispatchRegistry.ThrowIfNull(nameof(dispatchRegistry));

            _dieBuilder = dieBuilder;
            _roomBuilder = roomBuilder;
            _descriptor = new MazeDescriptor();
            _logger = logger;
            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;
        }

        private IList<Room> BuildRooms(MazeDetail detail)
        {
            var rooms = new List<Room>();

            var dice = _dieBuilder.Between(detail.MinNumRooms, detail.MaxNumRooms);
            var numRooms = dice.Random;

            for (var i = 0; i < numRooms; i++)
            {
                var room = _roomBuilder.BuildRoom(KnownRooms.Square);
                rooms.Add(room);
            }

            return rooms;
        }

        private IList<Room> AddDoors(IList<Room> rooms)
        {
            if (rooms.Count == 1) return rooms;

            var roomsWithDoors = new List<Room>(rooms);

            for (var index = 0; index < roomsWithDoors.Count; index++)
            {
                var room = roomsWithDoors[index];

                var doorNumber = index + 1;
                var roomWithDoor = room.AddDoor(doorNumber);
                roomsWithDoors[index] = roomWithDoor;

                var connectedRoom = GetRoomToConnectTo(roomWithDoor);
                var connectedRoomIndex = roomsWithDoors.IndexOf(connectedRoom);
                connectedRoom = connectedRoom.AddDoor(doorNumber);
                roomsWithDoors[connectedRoomIndex] = connectedRoom;
            }

            return roomsWithDoors;

            Room GetRoomToConnectTo(Room toConnect)
            {
                var connectableRooms = roomsWithDoors.Where(room => !room.Name.IsSame(toConnect.Name)).ToArray();
                if (connectableRooms.Length == 0) throw new ArgumentException("AddDoors: Unable to find a room to add a door to.");
                if (connectableRooms.Length == 1) return connectableRooms.First();

                var randomRoomIndex = _dieBuilder.Between(1, connectableRooms.Length).Random - 1;
                return connectableRooms[randomRoomIndex];
            }
        }

        internal Maze BuildMazeWithRoomsAndDoors(int level)
        {
            var mazeDetail = _descriptor[level];

            var rooms = BuildRooms(mazeDetail);
            var roomsWithDoors = AddDoors(rooms);

            var maxTileRows = rooms.Sum(room => room.NumberRows) * rooms.Count;
            var maxTileCols = rooms.Sum(room => room.NumberColumns) * rooms.Count;

            var mazeOfRocks = new Maze(_dispatchRegistry, _actionRegistry, _dieBuilder, maxTileRows, maxTileCols);
            mazeOfRocks.PositionRoomsInMaze(roomsWithDoors);

            return mazeOfRocks;
        }

        internal Maze BuildMaze(int level)
        {
            var mazeWithDisconnectedRooms = BuildMazeWithRoomsAndDoors(level);
            var mazeWithConnectedRooms = mazeWithDisconnectedRooms.ConnectDoorsWithCorridors();

            return mazeWithConnectedRooms;
        }
    }
}