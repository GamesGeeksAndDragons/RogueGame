using System.Collections.Generic;
using System.Linq;
using Assets.Actors;
using Utils;
using Utils.Coordinates;

namespace Assets.Messaging
{
    public class ActorRegistry
    {
        private readonly Dictionary<string, IActor> _namedActors = new Dictionary<string, IActor>();
        private readonly Dictionary<Coordinate, string> _actorCoordindates = new Dictionary<Coordinate, string>();
        private readonly Dictionary<string, uint> _actorCounts = new Dictionary<string, uint>();

        private string GenerateActorName(IActor actor)
        {
            uint count = 0;
            if (_actorCounts.ContainsKey(actor.Name))
            {
                count = _actorCounts[actor.Name];
            }

            _actorCounts[actor.Name] = ++count;

            return actor.Name + count;
        }

        internal string Register(IActor actor)
        {
            actor.ThrowIfNull(nameof(actor));

            var uniqueId = actor.UniqueId.IsNullOrEmpty() ? GenerateActorName(actor) : actor.UniqueId;

            Deregister(actor.Coordinates);

            _namedActors[uniqueId] = actor;

            if (actor.Coordinates != Coordinate.NotSet)
            {
                _actorCoordindates[actor.Coordinates] = uniqueId;
            }

            return uniqueId;
        }

        internal void Deregister(IActor actor)
        {
            actor.ThrowIfNull(nameof(actor));
            actor.UniqueId.ThrowIfEmpty(nameof(actor.UniqueId));

            _namedActors.Remove(actor.UniqueId);
            _actorCoordindates[actor.Coordinates] = null;
        }

        public void Deregister(Coordinate coordinates)
        {
            if (!_actorCoordindates.ContainsKey(coordinates) || _actorCoordindates[coordinates] == null) return;

            var actor = GetActor(coordinates);
            Deregister(actor);
        }

        public IActor GetActor(string id)
        {
            return _namedActors[id];
        }

        public IActor GetActor(Coordinate coordinates)
        {
            var id = _actorCoordindates[coordinates];
            return GetActor(id);
        }

        public IReadOnlyList<IActor> Actors => _namedActors.Values.ToList();
    }
}
