#nullable enable
using Assets.Deeds;
using Assets.Resources;
using Assets.Rooms;
using Utils;
using Utils.Dispatching;
using Utils.Display;
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

        private IList<IRoom> BuildRooms(string numRoomsBetween)
        {
            var rooms = new List<IRoom>();

            var dice = _dieBuilder.Between(numRoomsBetween);
            var numRooms = dice.Random;

            for (var i = 0; i < numRooms; i++)
            {
                var room = _roomBuilder.BuildRoom(i+1);
                rooms.Add(room);
            }

            return rooms;
        }

        private void AddDoors(IList<IRoom> rooms)
        {
            if (rooms.Count == 1) return;

            var roomsWithDoors = new List<IRoom>(rooms);

            for (var index = 0; index < roomsWithDoors.Count; index++)
            {
                var room = roomsWithDoors[index];

                var doorNumber = index + 1;
                room.AddDoor(doorNumber);

                var connectedRoom = GetRoomToConnectTo(room);
                connectedRoom.AddDoor(doorNumber);
            }

            IRoom GetRoomToConnectTo(IRoom toConnect)
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

            var tiles = BuildDefaultTiles();
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

            string BuildDefaultTiles()
            {
                var maxRows = rooms.Sum(room => room.UpperBounds.Row) * rooms.Count;
                var maxColumns = rooms.Sum(room => room.UpperBounds.Column) * rooms.Count;

                var row = new string(TilesDisplay.Rock[0], maxColumns);
                var sb = new StringBuilder();
                for (var i = 0; i < maxRows; i++)
                {
                    sb.AppendLine(row);
                }

                return sb.ToString();
            }

            IList<IRoom> RoomsWithDoors()
            {
                var roomsWithDoors = BuildRooms(numRooms);
                AddDoors(roomsWithDoors);
                return roomsWithDoors;
            }
        }
    }
}