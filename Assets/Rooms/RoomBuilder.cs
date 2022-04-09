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
        private readonly DispatchRegistry _dispatchRegistry;
        private readonly ActionRegistry _actionRegistry;
        private readonly Dictionary<string, string[]> _rooms;

        public RoomBuilder(IDieBuilder dieBuilder, ILog logger, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry)
        {
            dieBuilder.ThrowIfNull(nameof(dieBuilder));
            logger.ThrowIfNull(nameof(logger));
            dispatchRegistry.ThrowIfNull(nameof(dispatchRegistry));
            actionRegistry.ThrowIfNull(nameof(actionRegistry));

            _dieBuilder = dieBuilder;
            _logger = logger;
            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;

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

            var tiles = new IDispatchee[maxRows, maxCols];

            for (int rowIndex = 0; rowIndex < maxRows; rowIndex++)
            {
                var row = roomDescription[rowIndex];

                for (int colIndex = 0; colIndex < maxCols; colIndex++)
                {
                    var ch = row[colIndex];
                    var coordinates = new Coordinate(rowIndex, colIndex);
                    var dispatchee = ActorBuilder.Build(ch, coordinates, _dispatchRegistry, _actionRegistry);

                    tiles[rowIndex,colIndex] = dispatchee;
                }
            }

            return new Room(roomName, tiles, _dispatchRegistry, _actionRegistry, _dieBuilder);
        }
    }
}
