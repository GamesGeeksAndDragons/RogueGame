using Assets.Deeds;
using Assets.Mazes;
using Assets.Rooms;
using log4net;
using Utils.Random;

namespace Assets.Messaging
{
    internal class LevelBuilder
    {
        private readonly IDieBuilder _randomNumberGenerator;
        private readonly ILog _logger;
        private readonly Dispatcher _dispatcher;
        private readonly DispatchRegistry _dispatchRegistry;
        private readonly ActionRegistry _actionRegistry;

        public LevelBuilder(IDieBuilder randomNumberGenerator, ILog logger, Dispatcher dispatcher, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _logger = logger;
            _dispatcher = dispatcher;
            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;
        }

        internal void Compact(Maze maze, int level)
        {
            foreach (var dispatchee in _dispatchRegistry.Dispatchees)
            {
                var uniqueId = dispatchee.UniqueId;
                if (maze.IsInMaze(uniqueId)) continue;

                _dispatchRegistry.Unregister(uniqueId);
            }

            maze.UniqueId = Maze.DispatcheeName + level;
            _dispatchRegistry.Register(maze);
        }

        internal Maze Build(int level)
        {
            var roomBuilder = new RoomBuilder(_randomNumberGenerator, _logger, _dispatchRegistry, _actionRegistry);
            var mazeBuilder = new MazeBuilder(_randomNumberGenerator, roomBuilder, _logger, _dispatchRegistry, _actionRegistry);

            var maze = mazeBuilder.BuildMazeWithRoomsAndDoors(level);

            Compact(maze, level);

            return maze;
        }
    }
}
