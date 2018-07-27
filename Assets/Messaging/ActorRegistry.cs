using System;
using System.Collections.Generic;
using System.Text;
using Assets.Actors;
using Utils;
using Utils.Coordinates;

namespace Assets.Messaging
{
    public class ActorRegistry
    {
        private readonly Dictionary<string, Actor> _registry = new Dictionary<string, Actor>();
        private readonly Dictionary<string, uint> _actorCounts = new Dictionary<string, uint>();

        private string GenerateActorName(Actor actor)
        {
            uint count = 0;
            if (_actorCounts.ContainsKey(actor.Name))
            {
                count = _actorCounts[actor.Name];
            }

            _actorCounts[actor.Name] = ++count;

            return actor.Name + count;
        }

        public void Register(Actor actor)
        {
            actor.ThrowIfNull(nameof(actor));
            actor.UniqueId.ThrowIfNotEmpty(nameof(actor.UniqueId));

            actor.UniqueId = GenerateActorName(actor);

            _registry[actor.UniqueId] = actor;
        }

        public Actor GetActor(string id)
        {
            return _registry[id];
        }
    }
}
