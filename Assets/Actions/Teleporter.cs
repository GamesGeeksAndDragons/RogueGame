using System;
using System.Collections.Generic;
using System.Text;
using Assets.Actors;
using Assets.Messaging;
using Assets.Rooms;

namespace Assets.Actions
{
    class Teleporter : Action
    {
        private readonly string _actor;
        private readonly string _to;
        private readonly ActorRegistry _registry;

        public Teleporter(string actor, string to, ActorRegistry registry)
        {
            _actor = actor;
            _to = to;
            _registry = registry;
        }

        public override string Name => "TELEPORTER";
        public override void Act()
        {
            var actor = _registry.GetActor(_actor);
            var to = (Room)_registry.GetActor(_to);
        }
    }
}
