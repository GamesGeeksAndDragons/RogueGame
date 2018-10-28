using System.Collections.Generic;
using System.Linq;
using Assets.Messaging;
using log4net;
using Utils;
using Utils.Random;

namespace Assets.Mazes
{
    public class MazeBuilder
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private readonly RoomBuilder _roomBuilder;
        private readonly IMazeDescriptor _descriptor;
        private readonly ILog _logger;
        private readonly DispatchRegistry _registry;

        public MazeBuilder(IRandomNumberGenerator randomNumberGenerator, RoomBuilder roomBuilder, IMazeDescriptor descriptor, ILog logger, DispatchRegistry registry)
        {
            randomNumberGenerator.ThrowIfNull(nameof(randomNumberGenerator));
            roomBuilder.ThrowIfNull(nameof(roomBuilder));
            descriptor.ThrowIfNull(nameof(descriptor));
            logger.ThrowIfNull(nameof(logger));
            registry.ThrowIfNull(nameof(registry));

            _randomNumberGenerator = randomNumberGenerator;
            _roomBuilder = roomBuilder;
            _descriptor = descriptor;
            _logger = logger;
            _registry = registry;
        }

        private IList<Room> BuildRooms(MazeDetail detail)
        {
            var rooms = new List<Room>();

            var numRooms = _randomNumberGenerator.Between(detail.MinNumRooms, detail.MaxNumRooms);

            for (var i = 0; i < numRooms; i++)
            {
                var room = _roomBuilder.BuildRoom(detail.BlocksPerRoom, detail.TilesPerBlock);
                rooms.Add(room);
            }

            return rooms;
        }

        private IList<Room> AddDoors(IList<Room> rooms)
        {
            var roomsWithoutDoors = new List<Room>(rooms);
            var roomsWithDoors = new List<Room>(rooms.Count);

            Room RoomWithoutDoor(Room first)
            {
                return roomsWithoutDoors.FirstOrDefault(room =>
                {
                    var doors = first == null ?
                        room.Doors :
                        room.Doors.Where(doorName => doorName != first?.Name).ToList();
                    return doors.Count == 0;
                });
            }

            (Room first, Room second) RoomsWithoutAnyDoors()
            {
                var room1 = RoomWithoutDoor(null);
                if (room1 != null) roomsWithoutDoors.Remove(room1);

                var room2 = RoomWithoutDoor(room1);
                if (room2 != null) roomsWithoutDoors.Remove(room2);

                return (room1, room2);
            }

            var doorNumber = 0;
            void AddDoors((Room first, Room second) roomsWithNoDoors)
            {
                var room1 = roomsWithNoDoors.first?.AddDoor(++doorNumber);
                if (room1 != null) roomsWithDoors.Add(room1);

                var room2 = roomsWithNoDoors.second?.AddDoor(doorNumber);
                if (room2 != null) roomsWithDoors.Add(room2);
            }

            while (roomsWithoutDoors.Count > 1)
            {
                var roomsWithNoDoors = RoomsWithoutAnyDoors();
                AddDoors(roomsWithNoDoors);
            }

            return roomsWithDoors;
        }

        internal Maze BuildMazeWithRoomsAndDoors(int level)
        {
            var mazeDetail = _descriptor[level];

            var rooms = BuildRooms(mazeDetail);
            var roomsWithDoors = AddDoors(rooms);

            var maxTileRows = rooms.Sum(room => room.Tiles.UpperBounds.Row) * 2;
            var maxTileCols = rooms.Sum(room => room.Tiles.UpperBounds.Column) * 2;

            var mazeOfRocks = new Maze(_registry, _randomNumberGenerator, maxTileRows, maxTileCols);
            var mazeWithDisconnectedRooms = mazeOfRocks.PositionRoomsInMaze(roomsWithDoors);

            return mazeWithDisconnectedRooms;
        }

        internal Maze BuildMaze(int level)
        {
            var mazeWithDisconnectedRooms = BuildMazeWithRoomsAndDoors(level);
            var mazeWithConnectedRooms = mazeWithDisconnectedRooms.ConnectDoorsWithCorridors();

            return mazeWithConnectedRooms;
        }
    }
}