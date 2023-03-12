#nullable enable
using Assets.Characters;
using Assets.Deeds;
using Assets.Mazes;
using Assets.Messaging;
using Assets.Player;
using Assets.Resources;
using Assets.Rooms;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Random;

namespace Assets.Level;

public interface IGameLevelBuilder
{
    IGameLevel BuildNewLevel(int level);
    IGameLevel BuildExistingLevel(int level, string savedMaze, string[] charactersState);
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

    private IGameLevel BuildGameLevel(int level, IMaze maze)
    {
        return new GameLevel(level, maze, _gameCharacters, _dispatchRegistry, _dispatcher, _dieBuilder);
    }

    private void AddCharactersToLevel(IGameLevel gameLevel, IEnumerable<ICharacter> characters)
    {
        foreach (var character in characters)
        {
            character.Subscribe(gameLevel);
            if (character.Coordinates == Coordinate.NotSet)
            {
                _dispatcher.EnqueueTeleport(gameLevel, character);
            }
        }
    }

    public IGameLevel BuildNewLevel(int level)
    {
        var levelDetail = _descriptor[level];
        var maze = BuildMaze();
        var gameLevel = BuildGameLevel(level, maze);

        var characters = AddRandomCharacters();
        AddCharactersToLevel(gameLevel, characters);

        return gameLevel;

        IMaze BuildMaze()
        {
            var roomBuilder = new RoomBuilder(_dieBuilder, _logger, _dispatchRegistry, _actionRegistry, _resourceBuilder);
            var mazeBuilder = new MazeBuilder(_dieBuilder, roomBuilder, _logger, _dispatchRegistry, _actionRegistry, _resourceBuilder);

            return mazeBuilder.BuildMaze(levelDetail.NumRooms);
        }

        IEnumerable<ICharacter> AddRandomCharacters()
        {
            var numMonsters = _dieBuilder.Between(levelDetail.CharacterDie).Random;
            return _gameCharacters.GenerateRandomCharacters(numMonsters, level, true);
        }
    }

    public IGameLevel BuildExistingLevel(int level, string savedMaze, string[] charactersState)
    {
        var maze = new Maze(_dispatchRegistry, _actionRegistry, _dieBuilder, _resourceBuilder, savedMaze);
        var gameLevel = BuildGameLevel(level, maze);

        var characters = _gameCharacters.Load(charactersState);
        AddCharactersToLevel(gameLevel, characters);

        return gameLevel;
    }

}
