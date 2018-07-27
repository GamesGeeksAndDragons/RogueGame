using Assets.Actions;
using Assets.Actors;
using Assets.Rooms;
using log4net;
using Utils.Coordinates;
using Utils.Random;

namespace Assets.Messaging
{
    public class LevelBuilder
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private readonly ILog _logger;
        private readonly ActorRegistry _registry;

        public LevelBuilder(IRandomNumberGenerator randomNumberGenerator, ILog logger, ActorRegistry registry)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _logger = logger;
            _registry = registry;
        }

        internal Dispatcher Build(int level)
        {
            var numBlocks = level + 1;

            var roomBuilder = new RandomRoomBuilder(_randomNumberGenerator, _logger, _registry);
            var room = roomBuilder.BuildRoom(numBlocks);
            _registry.Register(room);

            var me = new Me(Coordinate.NotSet);
            _registry.Register(me);

            var dispatcher = new Dispatcher(_registry);

            var teleporter = new Teleporter(me.UniqueId, room.UniqueId, _registry);

            dispatcher.Dispatch(teleporter);

            return dispatcher;
        }
    }
}
