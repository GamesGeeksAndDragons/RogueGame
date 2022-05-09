#nullable enable
using Assets.Characters;
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
    internal interface ILevelBuilder
    {
        (IMaze, Me, IReadOnlyList<ICharacter>) BuildNewGame(int level);
    }

    internal class LevelBuilder : ILevelBuilder
    {
        private readonly IDieBuilder _dieBuilder;
        private readonly ILog _logger;
        private readonly IDispatcher _dispatcher;
        private readonly IDispatchRegistry _dispatchRegistry;
        private readonly IActionRegistry _actionRegistry;
        private readonly IResourceBuilder _resourceBuilder;
        private readonly ILevelDescriptor _descriptor;

        public LevelBuilder(IDieBuilder dieBuilder, ILog logger, IDispatcher dispatcher, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, IResourceBuilder resourceBuilder)
        {
            _dieBuilder = dieBuilder;
            _logger = logger;
            _dispatcher = dispatcher;
            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;
            _resourceBuilder = resourceBuilder;
            _descriptor = new LevelDescriptor();
        }

        public (IMaze, Me, IReadOnlyList<ICharacter>) BuildNewGame(int level)
        {
            var roomBuilder = new RoomBuilder(_dieBuilder, _logger, _dispatchRegistry, _actionRegistry, _resourceBuilder);
            var mazeBuilder = new MazeBuilder(_dieBuilder, roomBuilder, _logger, _dispatchRegistry, _actionRegistry, _resourceBuilder);

            var levelDetail = _descriptor[level];
            var maze = mazeBuilder.BuildMaze(levelDetail.NumRooms);

            var characterBuilder = new LevelCharacters(_resourceBuilder, _dieBuilder, levelDetail);
            var me = characterBuilder.BuildMe("");
            var characters = characterBuilder.BuildCharacters();

            TeleportMonsters();
            _dispatcher.EnqueueTeleport(me);

            _dispatcher.Dispatch();

            return (maze, me, characters);

            void TeleportMonsters()
            {
                foreach (var character in characters)
                {
                    _dispatcher.EnqueueTeleport(character);
                }
            }
        }
    }
}
