#nullable enable
using Assets.Actors;
using Assets.Deeds;
using Assets.Rooms;
using Assets.Tiles;
using log4net;
using Utils;
using Utils.Dispatching;
using Utils.Random;

namespace Assets.Maze
{
    public class MazeBuilder
    {
        private readonly IDieBuilder _dieBuilder;
        private readonly RoomBuilder _roomBuilder;
        private readonly IMazeDescriptor _descriptor;
        private readonly ILog _logger;
        private readonly IDispatchRegistry _dispatchRegistry;
        private readonly IActionRegistry _actionRegistry;
        private readonly IActorBuilder _actorBuilder;

        internal MazeBuilder(IDieBuilder dieBuilder, RoomBuilder roomBuilder, ILog logger, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IActorBuilder actorBuilder)
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
            _actorBuilder = actorBuilder;
        }

        private IList<Room> BuildRooms(MazeDetail detail)
        {
            var rooms = new List<Room>();

            var dice = _dieBuilder.Between(detail.MinNumRooms, detail.MaxNumRooms);
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
                var connectableRooms = roomsWithDoors.Where(room => !room.Name.IsSame(toConnect.Name)).ToArray();
                if (connectableRooms.Length == 0) throw new ArgumentException("AddDoors: Unable to find a room to add a door to.");
                if (connectableRooms.Length == 1) return connectableRooms.First();

                var randomRoomIndex = _dieBuilder.Between(1, connectableRooms.Length).Random - 1;
                return connectableRooms[randomRoomIndex];
            }
        }

        internal Tiles BuildMaze(int level)
        {
            var mazeDetail = _descriptor[level];

            var rooms = BuildRooms(mazeDetail);
            AddDoors(rooms);

            var maxTileRows = rooms.Sum(room => room.UpperBounds.Row) * rooms.Count;
            var maxTileCols = rooms.Sum(room => room.UpperBounds.Column) * rooms.Count;

            var tiles = TilesHelpers.BuildDefaultTiles(maxTileRows, maxTileCols, _actorBuilder.RockBuilder());
            var maze = new Tiles(_dispatchRegistry, _actionRegistry, _dieBuilder, _actorBuilder, tiles);

            var removed = maze.PositionRoomsInTiles(rooms);
            _dispatchRegistry.Unregister(removed);
            _actionRegistry.RegisterTiles(maze);

            var tunnel = maze.GetTunnelToConnectDoors(_dispatchRegistry, _actionRegistry, _dieBuilder);

            maze.ConnectDoorsWithCorridors(tunnel, _dispatchRegistry, _actorBuilder);

            return maze;
        }
    }
}