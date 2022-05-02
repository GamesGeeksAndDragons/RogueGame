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
        public const string Rectangle = nameof(Rectangle);
        public const string Square = nameof(Square);
        public const string LShaped = nameof(LShaped);
        public const string OShaped = nameof(OShaped);
    }

    public class RoomBuilder
    {
        private readonly IDieBuilder _dieBuilder;
        private readonly ILog _logger;
        private readonly IDispatchRegistry _dispatchRegistry;
        private readonly IActionRegistry _actionRegistry;
        private readonly IResourceBuilder _resourceBuilder;
        private readonly Dictionary<string, string[]> _rooms;

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

            Dictionary<string, string[]> LoadRooms()
            {
                var filenames = GetRoomFilenamesToLoad(FileAndDirectoryHelpers.LoadFolder);

                var rooms = new Dictionary<string, string[]>();
                foreach (var filename in filenames)
                {
                    var room = File.ReadAllLines(filename);
                    var name = Path.GetFileNameWithoutExtension(filename);

                    rooms[name] = room;
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

        internal Room BuildRoom(string roomName, int roomNumber)
        {
            var roomDescription = _rooms[roomName];
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

                    var dispatched = _resourceBuilder.BuildTile(actor);

                    tiles[rowIndex, colIndex] = dispatched.UniqueId;
                }
            }

            var maze = new Mazes.Maze(_dispatchRegistry, _actionRegistry, _dieBuilder, _resourceBuilder, tiles);

            return new Room(roomName, maze, _dispatchRegistry, _actionRegistry, _dieBuilder, _resourceBuilder);
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

            var maze = (Mazes.Maze) room.Maze;
            var rotatedTiles = Rotate(maze.Tiles);

            for (int i = 0; i < numTimes-1; i++)
            {
                rotatedTiles = Rotate(rotatedTiles);
            }

            var newTiles = new Mazes.Maze(maze.DispatchRegistry, maze.ActionRegistry, maze.DieBuilder, maze.ResourceBuilder, rotatedTiles);

            return new Room(room, newTiles);

            string[,] Rotate(string[,] tiles)
            {
                var (maxRow, maxColumn) = tiles.UpperBounds();
                return BuildRotatedTiles(tiles, maxRow, maxColumn);
            }
        }
    }
}
