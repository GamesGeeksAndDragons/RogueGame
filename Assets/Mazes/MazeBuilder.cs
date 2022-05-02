#nullable enable
using Assets.Deeds;
using Assets.Resources;
using Assets.Rooms;
using Assets.Tiles;
using log4net;
using Utils;
using Utils.Dispatching;
using Utils.Random;

namespace Assets.Mazes
{
    public class MazeBuilder
    {
        private readonly IDieBuilder _dieBuilder;
        private readonly RoomBuilder _roomBuilder;
        private readonly ILog _logger;
        private readonly IDispatchRegistry _dispatchRegistry;
        private readonly IActionRegistry _actionRegistry;
        private readonly IResourceBuilder _resourceBuilder;

        internal MazeBuilder(IDieBuilder dieBuilder, RoomBuilder roomBuilder, ILog logger, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IResourceBuilder resourceBuilder)
        {
            dieBuilder.ThrowIfNull(nameof(dieBuilder));
            roomBuilder.ThrowIfNull(nameof(roomBuilder));
            logger.ThrowIfNull(nameof(logger));
            dispatchRegistry.ThrowIfNull(nameof(dispatchRegistry));

            _dieBuilder = dieBuilder;
            _roomBuilder = roomBuilder;
            _logger = logger;
            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;
            _resourceBuilder = resourceBuilder;
        }

        private IList<Room> BuildRooms(string numRoomsBetween)
        {
            var rooms = new List<Room>();

            var dice = _dieBuilder.Between(numRoomsBetween);
            var numRooms = dice.Random;

            for (var i = 0; i < numRooms; i++)
            {
                var room = _roomBuilder.BuildRoom(KnownRooms.Square, i+1);
                rooms.Add(room);
            }

            return rooms;
        }

        private void AddDoors(IList<Room> rooms)
        {
            if (rooms.Count == 1) return;

            var roomsWithDoors = new List<Room>(rooms);

            for (var index = 0; index < roomsWithDoors.Count; index++)
            {
                var room = roomsWithDoors[index];

                var doorNumber = index + 1;
                room.AddDoor(doorNumber);

                var connectedRoom = GetRoomToConnectTo(room);
                connectedRoom.AddDoor(doorNumber);
            }

            Room GetRoomToConnectTo(Room toConnect)
            {
                var connectableRooms = roomsWithDoors.Where(room => !room.UniqueId.IsSame(toConnect.UniqueId)).ToArray();
                if (connectableRooms.Length == 0) throw new ArgumentException("AddDoors: Unable to find a room to add a door to.");
                if (connectableRooms.Length == 1) return connectableRooms.First();

                var randomRoomIndex = _dieBuilder.Between(1, connectableRooms.Length).Random - 1;
                return connectableRooms[randomRoomIndex];
            }
        }

        internal IMaze BuildMaze(string numRooms)
        {
            var rooms = RoomsWithDoors();

            var maxTileRows = rooms.Sum(room => room.UpperBounds.Row) * rooms.Count;
            var maxTileCols = rooms.Sum(room => room.UpperBounds.Column) * rooms.Count;

            var tiles = MazeHelpers.BuildDefaultTiles(maxTileRows, maxTileCols, _resourceBuilder.DefaultRockBuilder());
            var maze = new Maze(_dispatchRegistry, _actionRegistry, _dieBuilder, _resourceBuilder, tiles);

            var removed = maze.PositionRoomsInMaze(rooms);

            _dispatchRegistry.Unregister(removed);
            _dispatchRegistry.Register(maze);

            foreach (var room in rooms)
            {
                _dispatchRegistry.Unregister(room);
            }

            var tunnel = maze.GetTunnelToConnectDoors(_dispatchRegistry, _actionRegistry, _dieBuilder);

            maze.ConnectDoorsWithCorridors(tunnel, _dispatchRegistry, _resourceBuilder);

            return maze;

            IList<Room> RoomsWithDoors()
            {
                var roomsWithDoors = BuildRooms(numRooms);
                AddDoors(roomsWithDoors);
                return roomsWithDoors;
            }
        }
    }
}