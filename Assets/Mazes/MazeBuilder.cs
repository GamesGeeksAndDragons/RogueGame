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
            var roomsToConnect = new List<Room>(rooms);

            var roomToAddDoorTo = roomsToConnect.FirstOrDefault(room => ! room.HasDoors);

            var doorNumber = 0;
            while (roomToAddDoorTo != null)
            {
                var roomWithDoor = roomToAddDoorTo.AddDoor(++doorNumber);

                roomsToConnect.Remove(roomToAddDoorTo);
                roomsToConnect.Add(roomWithDoor);

                if (roomsToConnect.Count != 0)
                {
                    var randomRoom = _randomNumberGenerator.Between(0, roomsToConnect.Count - 1);
                    var roomToConnect = roomsToConnect[randomRoom];

                    var connectedRoom = roomToConnect.AddDoor(doorNumber);

                    roomsToConnect.Remove(roomToConnect);
                    roomsToConnect.Add(connectedRoom);
                }

                roomToAddDoorTo = roomsToConnect.FirstOrDefault(room => !room.HasDoors);
            }

            return roomsToConnect;
        }

        internal Maze BuildMaze(int level)
        {
            var mazeDetail = _descriptor[level];

            var rooms = BuildRooms(mazeDetail);
            var roomsWithDoors = AddDoors(rooms);

            var maxTileRows = rooms.Sum(room => room.Tiles.UpperBounds.Row) * 2;
            var maxTileCols = rooms.Sum(room => room.Tiles.UpperBounds.Column) * 2;

            var mazeOfRocks = new Maze(_registry, _randomNumberGenerator, maxTileRows, maxTileCols);
            var mazeWithDisconnectedRooms = mazeOfRocks.PositionRoomsInMaze(roomsWithDoors);
            var mazeWithConnectedRooms = mazeWithDisconnectedRooms.ConnectDoorsWithCorridors();

            return mazeWithConnectedRooms;
        }
    }
}