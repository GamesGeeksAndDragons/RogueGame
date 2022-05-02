using Assets.Deeds;
using Assets.Mazes;
using Assets.Messaging;
using log4net;
using Utils.Dispatching;
using Utils.Random;

namespace Assets.Level
{
    internal class Level
    {
        private readonly IDieBuilder _randomNumberGenerator;
        private readonly IDispatcher _dispatcher;
        private readonly IActionRegistry _actionRegistry;
        private readonly IMaze _maze;

        public Level(IDieBuilder randomNumberGenerator, IDispatcher dispatcher, IActionRegistry actionRegistry, IMaze maze)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _dispatcher = dispatcher;
            _actionRegistry = actionRegistry;
            _maze = maze;
        }
    }
}
