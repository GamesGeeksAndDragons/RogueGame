using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Actors;
using Assets.Deeds;
using Assets.Messaging;
using log4net;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
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
        private readonly IActorBuilder _actorBuilder;
        private readonly Dictionary<string, string[]> _rooms;

        public RoomBuilder(IDieBuilder dieBuilder, ILog logger, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IActorBuilder actorBuilder)
        {
            dieBuilder.ThrowIfNull(nameof(dieBuilder));
            logger.ThrowIfNull(nameof(logger));
            dispatchRegistry.ThrowIfNull(nameof(dispatchRegistry));
            actionRegistry.ThrowIfNull(nameof(actionRegistry));

            _dieBuilder = dieBuilder;
            _logger = logger;
            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;
            _actorBuilder = actorBuilder;

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
                        .Where(fqn => Path.HasExtension(".room"));
                }
            }
        }

        internal Room BuildRoom(string roomName)
        {
            var roomDescription = _rooms[roomName];
            var maxRows = roomDescription.Length;
            var maxCols = roomDescription.Max(row => row.Length);

            var tiles = new Tiles.Tiles(maxRows, maxCols, _dispatchRegistry, _actionRegistry, _dieBuilder, _actorBuilder);

            for (int rowIndex = 0; rowIndex < maxRows; rowIndex++)
            {
                var row = roomDescription[rowIndex];

                for (int colIndex = 0; colIndex < maxCols; colIndex++)
                {
                    var actor = row[colIndex].ToString();
                    var dispatchee = _actorBuilder.Build(actor);

                    var coordinates = new Coordinate(rowIndex, colIndex);
                    tiles[coordinates] = dispatchee.UniqueId;
                }
            }

            return new Room(roomName, tiles, _dispatchRegistry, _actionRegistry, _dieBuilder, _actorBuilder);
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
                    var tile = _dispatchRegistry.GetDispatchee(uniqueId);

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

            var originalTiles = (Tiles.Tiles) room.Tiles;
            var tiles = originalTiles.TilesRegistry;
            var (maxRow, maxColumn) = tiles.UpperBounds();

            var rotatedTiles = BuildRotatedTiles(tiles, maxRow, maxColumn);
            for (int i = 0; i < numTimes-1; i++)
            {
                (maxRow, maxColumn) = rotatedTiles.UpperBounds();
                rotatedTiles = BuildRotatedTiles(rotatedTiles, maxRow, maxColumn);
            }

            var newTiles = new Tiles.Tiles(originalTiles, rotatedTiles);

            return new Room(room, newTiles);
        }
    }
}
