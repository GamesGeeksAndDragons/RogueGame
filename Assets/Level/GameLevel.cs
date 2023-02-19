#nullable enable
using Assets.Mazes;
using Assets.Messaging;
using Assets.Personas;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Random;

namespace Assets.Level
{
    public interface IGameLevel : ICharacterPosition, IObserver<PositionObservation>
    {
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
        public IMaze Maze { get; }
        public IGameCharacters GameCharacters { get; }

        public GameLevel(IMaze maze, IGameCharacters gameCharacters, IDispatchRegistry dispatchRegistry, IDispatcher dispatcher, IDieBuilder dieBuilder)
        {
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
            var (before, after) = observation.Change;
            if (before == after && after == Coordinate.NotSet) return;

            var character = GameCharacters[observation.UniqueId];

            if (after == Coordinate.NotSet)
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
}
