#nullable enable
using System.IO;
using Assets.Deeds;
using Assets.Mazes;
using Assets.Messaging;
using Assets.Resources;
using Assets.Tiles;
using log4net;
using Utils;
using Utils.Dispatching;
using Utils.Display;
using Utils.Random;

namespace Assets.Rooms
{
    public static class KnownRooms
    {
        public const int Rectangle = 2;
        public const int Square = 1;
        public const int LShaped = 3;
        public const int OShaped = 4;
    }

    public class RoomBuilder
    {
        private readonly IDieBuilder _dieBuilder;
        private readonly ILog _logger;
        private readonly IDispatchRegistry _dispatchRegistry;
        private readonly IActionRegistry _actionRegistry;
        private readonly IResourceBuilder _resourceBuilder;
        private readonly Dictionary<int, string[]> _rooms;

        public RoomBuilder(IDieBuilder dieBuilder, ILog logger, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IResourceBuilder resourceBuilder)
        {
            dieBuilder.ThrowIfNull(nameof(dieBuilder));
            logger.ThrowIfNull(nameof(logger));
            dispatchRegistry.ThrowIfNull(nameof(dispatchRegistry));
            actionRegistry.ThrowIfNull(nameof(actionRegistry));

            _dieBuilder = dieBuilder;
            _logger = logger;
            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;
            _resourceBuilder = resourceBuilder;

            _rooms = LoadRooms();

            Dictionary<int, string[]> LoadRooms()
            {
                var filenames = GetRoomFilenamesToLoad(FileAndDirectoryHelpers.LoadFolder);

                var rooms = new Dictionary<int, string[]>();
                foreach (var filename in filenames)
                {
                    var room = File.ReadAllLines(filename);
                    var name = Path.GetFileNameWithoutExtension(filename);
                    var splitName = name.Split('-');
                    var index = splitName[0];
                    var roomIndex = int.Parse(index);

                    rooms[roomIndex] = room;
                }

                return rooms;

                IEnumerable<string> GetRoomFilenamesToLoad(string folder)
                {
                    var directory = FileAndDirectoryHelpers.GetLoadDirectory(folder);
                    return Directory.EnumerateFiles(directory)
                        .Where(fqn => fqn.HasExtension(".room"));
                }
            }
        }

        internal Room BuildRoom(int roomNumber)
        {
            var roomIndex = _dieBuilder.D4.Random;
            var roomDescription = _rooms[roomIndex];
            var maxRows = roomDescription.Length;
            var maxCols = roomDescription.Max(row => row.Length);

            var floorBuilder = _resourceBuilder.FloorBuilder();
            IDispatched FloorForRoom() => floorBuilder(roomNumber, "");

            var tiles = MazeHelpers.BuildDefaultTiles(maxRows, maxCols, FloorForRoom);

            for (int rowIndex = 0; rowIndex < maxRows; rowIndex++)
            {
                var row = roomDescription[rowIndex];

                for (int colIndex = 0; colIndex < maxCols; colIndex++)
                {
                    var actor = row[colIndex].ToString();
                    if (actor.IsFloorActor()) continue;

                    var dispatched = _resourceBuilder.BuildResource(actor);

                    tiles[rowIndex, colIndex] = dispatched.UniqueId;
                }
            }

            var maze = new Maze(_dispatchRegistry, _actionRegistry, _dieBuilder, _resourceBuilder, tiles);

            var room = new Room(_dispatchRegistry, _actionRegistry, _dieBuilder, _resourceBuilder, maze);

            return RotateRoom();

            Room RotateRoom()
            {
                var rotation = _dieBuilder.D4.Random - 1;
                if (rotation == 0) return room;

                return BuildRotatedRoom(room, rotation);
            }
        }

        string[,] BuildRotatedTiles(string[,] tilesToRotate, int maxRows, int maxColumns)
        {
            var (rotatedNumberRows, rotatedNumberColumns) = (maxColumns + 1, maxRows + 1);
            var rotated = new string[rotatedNumberRows, rotatedNumberColumns];

            for (int rowIndex = 0; rowIndex <= maxRows; rowIndex++)
            {
                var row = tilesToRotate.SliceRow(rowIndex).ToArray();

                rotatedNumberColumns--;
                for (int columnIndex = 0; columnIndex < row.Length; columnIndex++)
                {
                    var uniqueId = row[columnIndex];
                    var tile = _dispatchRegistry.GetDispatched(uniqueId);

                    if (tile.IsWall())
                    {
                        var rotatedTile = (Wall)tile;
                        tile = rotatedTile.Rotate();
                    }

                    rotated[columnIndex, rotatedNumberColumns] = tile.UniqueId;
                }
            }

            return rotated;
        }

        internal Room BuildRotatedRoom(Room room, int numTimes = 1)
        {
            numTimes.ThrowIfAbove(3, $"Attempted to rotate a room {numTimes} times.  No need to rotate more than 3 times.");

            var maze = (Maze) room.Maze;
            var rotatedTiles = Rotate(maze.Tiles);

            for (int i = 0; i < numTimes-1; i++)
            {
                rotatedTiles = Rotate(rotatedTiles);
            }

            var newTiles = new Maze(maze.DispatchRegistry, maze.ActionRegistry, maze.DieBuilder, maze.ResourceBuilder, rotatedTiles);

            return new Room(room, newTiles);

            string[,] Rotate(string[,] tiles)
            {
                var (maxRow, maxColumn) = tiles.UpperBounds();
                return BuildRotatedTiles(tiles, maxRow, maxColumn);
            }
        }
    }
}
