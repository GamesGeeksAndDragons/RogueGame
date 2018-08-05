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

        internal void Build(int level)
        {
            var mazeBuilder = new RandomMazeBuilder(_randomNumberGenerator, _mazeDescriptor,  _logger, _registry);
            mazeBuilder.BuildMaze(level);
        }
    }
}
