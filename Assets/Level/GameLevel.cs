#nullable enable
using Assets.Mazes;
using Assets.Messaging;
using Assets.Personas;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Random;

namespace Assets.Level
{
    public interface IGameLevel
    {
        public IDispatchRegistry DispatchRegistry { get; }
        public IDispatcher Dispatcher { get; }
        public IDieBuilder DieBuilder { get; }
        public IMaze Maze { get; }
        public IReadOnlyList<ICharacter> Characters { get; }
        public ICharacter Me { get; }
        public ICharacter? this[Coordinate position] { get; }
    }

    internal class GameLevel : IGameLevel, IObserver<PositionObservation>, IDisposable
    {
        public IDispatchRegistry DispatchRegistry { get; }
        public IDispatcher Dispatcher { get; }
        public IDieBuilder DieBuilder { get; }
        public IMaze Maze { get; }
        public ICharacter Me { get; }

        internal readonly List<ICharacter> GameCharacters;
        internal readonly Dictionary<Coordinate, string> Positions;
        public IReadOnlyList<ICharacter> Characters => GameCharacters;

        public ICharacter? this[Coordinate position]
        {
            get
            {
                if (!Positions.ContainsKey(position)) return null;

                var uniqueId = Positions[position];
                return (ICharacter)DispatchRegistry.GetDispatched(uniqueId);
            }
        }

        public GameLevel(IMaze maze, ICharacter me, IReadOnlyList<ICharacter> characters, IDispatchRegistry dispatchRegistry, IDispatcher dispatcher, IDieBuilder dieBuilder)
        {
            Maze = maze;
            Me = me;
            GameCharacters = new List<ICharacter>(characters);
            Positions = new Dictionary<Coordinate, string>();
            DispatchRegistry = dispatchRegistry;
            Dispatcher = dispatcher;
            DieBuilder = dieBuilder;

            SubscribeCharacters();
            me.Subscribe(this);
        }

        public void OnCompleted()
        {
        }

        internal void SubscribeCharacters()
        {
            foreach (var character in GameCharacters)
            {
                character.Subscribe(this);
            }
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(PositionObservation observation)
        {
            var (before, after) = observation.Change;
            if (before == after && after == Coordinate.NotSet) return;

            if (Positions.ContainsKey(before))
            {
                Positions.Remove(before);
            }

            if (after != Coordinate.NotSet)
            {
                Positions.Add(after, observation.UniqueId);
            }
        }

        public void Dispose()
        {
            if (GameCharacters.Count == 0) return;

            foreach (var character in GameCharacters)
            {
                character.Dispose();
            }
        }
    }
}
