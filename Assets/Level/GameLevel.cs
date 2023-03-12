#nullable enable
using Assets.Characters;
using Assets.Mazes;
using Assets.Messaging;
using Assets.Player;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Random;

namespace Assets.Level;

public interface IGameLevel : ICharacterPosition, IObserver<PositionObservation>
{
    int Level { get; }
    IDispatchRegistry DispatchRegistry { get; }
    IDispatcher Dispatcher { get; }
    IDieBuilder DieBuilder { get; }
    IMaze Maze { get; }
    IGameCharacters GameCharacters { get; }
}

internal class GameLevel : IGameLevel
{
    public IDispatchRegistry DispatchRegistry { get; }
    public IDispatcher Dispatcher { get; }
    public IDieBuilder DieBuilder { get; }
    public int Level { get; }
    public IMaze Maze { get; }
    public IGameCharacters GameCharacters { get; }

    public GameLevel(int level, IMaze maze, IGameCharacters gameCharacters, IDispatchRegistry dispatchRegistry, IDispatcher dispatcher, IDieBuilder dieBuilder)
    {
        Level = level;
        Maze = maze;
        GameCharacters = gameCharacters;
        DispatchRegistry = dispatchRegistry;
        Dispatcher = dispatcher;
        DieBuilder = dieBuilder;
    }

    public void OnCompleted()
    {
    }

    public void OnError(Exception error)
    {
    }

    public void OnNext(PositionObservation observation)
    {
        var character = observation.Character;
        var (before, after) = observation.Change;

        if (before == after && after == Coordinate.NotSet)
        {
            GameCharacters.Add(character);
        }
        else if (after == Coordinate.NotSet)
        {
            GameCharacters.Remove(character);
        }
        else
        {
            GameCharacters.Position(character, before);
        }
    }

    public ICharacter? this[Coordinate position] => GameCharacters[position];

    public ICharacter Me => GameCharacters.Me;
}
