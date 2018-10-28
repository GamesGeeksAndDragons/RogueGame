using Assets.Mazes;
using log4net;
using Utils.Random;

namespace Assets.Messaging
{
    internal class LevelBuilder
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private readonly IMazeDescriptor _mazeDescriptor;
        private readonly ILog _logger;
        private readonly Dispatcher _dispatcher;
        private readonly DispatchRegistry _registry;

        public LevelBuilder(IRandomNumberGenerator randomNumberGenerator, IMazeDescriptor mazeDescriptor, ILog logger, Dispatcher dispatcher, DispatchRegistry registry)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _mazeDescriptor = mazeDescriptor;
            _logger = logger;
            _dispatcher = dispatcher;
            _registry = registry;
        }

        internal void Compact(Maze maze, int level)
        {
            foreach (var dispatchee in _registry.Dispatchees)
            {
                var uniqueId = dispatchee.UniqueId;
                if (maze.IsInMaze(uniqueId)) continue;

                _registry.Deregister(uniqueId);
            }

            maze.UniqueId = Maze.DispatcheeName + level;
            _registry.Register(maze);
        }

        internal void Build(int level, bool connectTunnels=true)
        {
            var roomBuilder = new RoomBuilder(_randomNumberGenerator, _logger, _registry);
            var mazeBuilder = new MazeBuilder(_randomNumberGenerator, roomBuilder, _mazeDescriptor,  _logger, _registry);

            var maze = connectTunnels ? 
                mazeBuilder.BuildMaze(level) :
                mazeBuilder.BuildMazeWithRoomsAndDoors(level);

            Compact(maze, level);
        }
    }
}
