#nullable enable
using Assets.Level;
using Assets.Mazes;
using Assets.Messaging;
using Assets.Personas;
using Utils;

namespace Assets.Game
{
    internal class Game
    {
        public Guid Id { get; }
        public string Folder { get; set; }

        internal IDispatcher Dispatcher;
        internal ILevelBuilder LevelBuilder;
        internal int Level;

        internal IMaze? Maze;
        internal Me? Me;
        internal IReadOnlyList<ICharacter>? Characters;


        public Game(GameConfig gameConfig, IDispatcher dispatcher, ILevelBuilder levelBuilder)
        {
            Id = gameConfig.Id;
            Folder = gameConfig.Folder;

            Dispatcher = dispatcher;
            LevelBuilder = levelBuilder;
        }

        public void NewGame()
        {
            FileAndDirectoryHelpers.CreateFolder(Folder);

            var level = LevelBuilder.BuildNewGame();
            Level++;
        }
    }
}
