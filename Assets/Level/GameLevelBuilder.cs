#nullable enable
using Assets.Deeds;
using Assets.Mazes;
using Assets.Messaging;
using Assets.Personas;
using Assets.Resources;
using Assets.Rooms;
using log4net;
using Utils.Dispatching;
using Utils.Random;

namespace Assets.Level
{
    public interface ILevelBuilder
    {
        IGameLevel BuildNewGame();
        IGameLevel BuildExistingLevel(int level, string savedMaze);
    }

    internal class GameLevelBuilder : ILevelBuilder
    {
        private readonly IDieBuilder _dieBuilder;
        private readonly ILog _logger;
        private readonly IDispatcher _dispatcher;
        private readonly IDispatchRegistry _dispatchRegistry;
        private readonly IActionRegistry _actionRegistry;
        private readonly IResourceBuilder _resourceBuilder;
        private readonly IPersonasBuilder _personasBuilder;
        private readonly ILevelDescriptor _descriptor;

        public GameLevelBuilder(IDieBuilder dieBuilder, ILog logger, IDispatcher dispatcher, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IResourceBuilder resourceBuilder, IPersonasBuilder personasBuilder)
        {
            _dieBuilder = dieBuilder;
            _logger = logger;
            _dispatcher = dispatcher;
            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;
            _resourceBuilder = resourceBuilder;
            _personasBuilder = personasBuilder;
            _descriptor = new LevelDescriptor();
        }

        private IGameLevel BuildLevel(IMaze maze, string characterDie)
        {
            var me = _personasBuilder.BuildMe();
            var personaCount = _dieBuilder.Between(characterDie).Random;
            var personas = _personasBuilder.BuildCharacters(personaCount);

            var level = new GameLevel(maze, me, personas, _dispatchRegistry, _dispatcher, _dieBuilder);

            TeleportCharacters();

            return level;

            void TeleportCharacters()
            {
                foreach (var character in personas)
                {
                    _dispatcher.EnqueueTeleport(level, character);
                }
                _dispatcher.EnqueueTeleport(level, me);
            }
        }

        internal IGameLevel BuildNewGame(int level)
        {
            var roomBuilder = new RoomBuilder(_dieBuilder, _logger, _dispatchRegistry, _actionRegistry, _resourceBuilder);
            var mazeBuilder = new MazeBuilder(_dieBuilder, roomBuilder, _logger, _dispatchRegistry, _actionRegistry, _resourceBuilder);

            var levelDetail = _descriptor[level];
            var maze = mazeBuilder.BuildMaze(levelDetail.NumRooms);

            return BuildLevel(maze, levelDetail.CharacterDie);
        }

        public IGameLevel BuildNewGame()
        {
            return BuildNewGame(1);
        }
        
        public IGameLevel BuildExistingLevel(int level, string savedMaze)
        {
            var maze = new Maze(_dispatchRegistry, _actionRegistry, _dieBuilder, _resourceBuilder, savedMaze);
            var levelDetail = _descriptor[level];

            return BuildLevel(maze, levelDetail.CharacterDie);
        }
    }
}
