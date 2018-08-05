using Assets.Actors;
using Assets.Rooms;
using log4net;
using Utils.Coordinates;
using Utils.Random;

namespace Assets.Messaging
{
    internal class LevelBuilder
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private readonly ILog _logger;
        private readonly Dispatcher _dispatcher;
        private readonly DispatchRegistry _registry;

        public LevelBuilder(IRandomNumberGenerator randomNumberGenerator, ILog logger, Dispatcher dispatcher, DispatchRegistry registry)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _logger = logger;
            _dispatcher = dispatcher;
            _registry = registry;
        }

        internal void Build(int level)
        {
            var numBlocks = level + 1;

            var roomBuilder = new RandomRoomBuilder(_randomNumberGenerator, _logger, _registry);
            var room = roomBuilder.BuildRoom(numBlocks);
        }
    }
}
