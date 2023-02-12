#nullable enable
using Assets.Deeds;
using Assets.Mazes;
using Assets.Messaging;
using Assets.Personas;
using Assets.Resources;
using Assets.Rooms;
using log4net;
using Utils;
using Utils.Dispatching;
using Utils.Random;

namespace Assets.Level
{
    public interface IGameLevelBuilder
    {
        IGameLevel BuildNewGame();
        IGameLevel BuildExistingLevel(int level, string savedMaze);

        void AddCharacter(IGameLevel gameLevel, string state);
    }

    internal class GameLevelBuilder : IGameLevelBuilder
    {
        private readonly IGameCharacters _gameCharacters;
        private readonly IDieBuilder _dieBuilder;
        private readonly ILog _logger;
        private readonly IDispatcher _dispatcher;
        private readonly IDispatchRegistry _dispatchRegistry;
        private readonly IActionRegistry _actionRegistry;
        private readonly IResourceBuilder _resourceBuilder;
        private readonly ILevelDescriptor _descriptor;

        public GameLevelBuilder(IGameCharacters gameCharacters, IDieBuilder dieBuilder, ILog logger, IDispatcher dispatcher, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IResourceBuilder resourceBuilder)
        {
            gameCharacters.ThrowIfNull(nameof(gameCharacters));
            dieBuilder.ThrowIfNull(nameof(dieBuilder));
            logger.ThrowIfNull(nameof(dieBuilder));
            dispatcher.ThrowIfNull(nameof(dispatcher));
            dispatchRegistry.ThrowIfNull(nameof(dispatchRegistry));
            actionRegistry.ThrowIfNull(nameof(actionRegistry));
            resourceBuilder.ThrowIfNull(nameof(resourceBuilder));

            _gameCharacters = gameCharacters;
            _dieBuilder = dieBuilder;
            _logger = logger;
            _dispatcher = dispatcher;
            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;
            _resourceBuilder = resourceBuilder;
            _descriptor = new LevelDescriptor();
        }

        private IGameLevel BuildLevel(IMaze maze, params string[] charactersState)
        {
            _gameCharacters.Load(charactersState);

            var level = new GameLevel(maze, _gameCharacters, _dispatchRegistry, _dispatcher, _dieBuilder);

            return level;
        }

        internal IGameLevel BuildNewGame(int level)
        {
            var roomBuilder = new RoomBuilder(_dieBuilder, _logger, _dispatchRegistry, _actionRegistry, _resourceBuilder);
            var mazeBuilder = new MazeBuilder(_dieBuilder, roomBuilder, _logger, _dispatchRegistry, _actionRegistry, _resourceBuilder);

            var levelDetail = _descriptor[level];
            var maze = mazeBuilder.BuildMaze(levelDetail.NumRooms);

            return BuildLevel(maze);
        }

        public IGameLevel BuildNewGame()
        {
            return BuildNewGame(1);
        }
        
        public IGameLevel BuildExistingLevel(int level, string savedMaze)
        {
            var maze = new Maze(_dispatchRegistry, _actionRegistry, _dieBuilder, _resourceBuilder, savedMaze);
            return BuildLevel(maze);
        }

        public void AddRandomCharacters(IGameLevel gameLevel, string characterDie)
        {
            var count = _dieBuilder.Between(characterDie).Random;
            var characters = _gameCharacters.GenerateRandomCharacters(count);
            
            foreach (var character in characters)
            {
                character.Subscribe(gameLevel);
            }

            TeleportCharacters();

            throw new NotImplementedException("have to add random characters to the maze");

            void TeleportCharacters()
            {
                foreach (var character in characters)
                {
                    _dispatcher.EnqueueTeleport(gameLevel, character);
                }
                //_dispatcher.EnqueueTeleport(level, me);
            }
        }

        public void AddCharacter(IGameLevel gameLevel, string state)
        {
            var characters = _gameCharacters.Load(state);
            foreach (var character in characters)
            {
                character.Subscribe(gameLevel);
            }
        }
    }
}
