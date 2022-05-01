#nullable enable
using Assets.Actors;
using Assets.Deeds;
using Assets.Maze;
using Assets.Mazes;
using Assets.Rooms;
using log4net;
using Utils.Dispatching;
using Utils.Random;

namespace Assets.Messaging
{
    internal class LevelBuilder
    {
        private readonly IDieBuilder _randomNumberGenerator;
        private readonly ILog _logger;
        private readonly Dispatcher _dispatcher;
        private readonly IDispatchRegistry _dispatchRegistry;
        private readonly IActionRegistry _actionRegistry;
        private readonly IActorBuilder _actorBuilder;

        public LevelBuilder(IDieBuilder randomNumberGenerator, ILog logger, Dispatcher dispatcher, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IActorBuilder actorBuilder)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _logger = logger;
            _dispatcher = dispatcher;
            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;
            _actorBuilder = actorBuilder;
        }

        internal Mazes.Maze Build(int level)
        {
            var roomBuilder = new RoomBuilder(_randomNumberGenerator, _logger, _dispatchRegistry, _actionRegistry, _actorBuilder);
            var mazeBuilder = new MazeBuilder(_randomNumberGenerator, roomBuilder, _logger, _dispatchRegistry, _actionRegistry, _actorBuilder);

            var maze = mazeBuilder.BuildMaze(level);

            return maze;
        }
    }
}
