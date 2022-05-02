#nullable enable
using Assets.Deeds;
using Assets.Mazes;
using Assets.Messaging;
using Assets.Resources;
using Assets.Rooms;
using Assets.Tiles;
using log4net;
using Utils.Dispatching;
using Utils.Random;

namespace Assets.Level
{
    internal class LevelBuilder
    {
        private readonly IDieBuilder _randomNumberGenerator;
        private readonly ILog _logger;
        private readonly Dispatcher _dispatcher;
        private readonly IDispatchRegistry _dispatchRegistry;
        private readonly IActionRegistry _actionRegistry;
        private readonly IResourceBuilder _resourceBuilder;
        private readonly ILevelDescriptor _descriptor;

        public LevelBuilder(IDieBuilder randomNumberGenerator, ILog logger, Dispatcher dispatcher, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IResourceBuilder resourceBuilder)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _logger = logger;
            _dispatcher = dispatcher;
            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;
            _resourceBuilder = resourceBuilder;
            _descriptor = new LevelDescriptor();
        }

        internal IMaze Build(int level)
        {
            var roomBuilder = new RoomBuilder(_randomNumberGenerator, _logger, _dispatchRegistry, _actionRegistry, _resourceBuilder);
            var mazeBuilder = new MazeBuilder(_randomNumberGenerator, roomBuilder, _logger, _dispatchRegistry, _actionRegistry, _resourceBuilder);

            var levelDetail = _descriptor[level];
            var maze = mazeBuilder.BuildMaze(levelDetail.MinNumRooms, levelDetail.MaxNumRooms);

            return maze;
        }
    }
}
