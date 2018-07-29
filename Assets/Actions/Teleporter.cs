using System;
using System.Collections.Generic;
using System.Text;
using Assets.Actors;
using Assets.Messaging;
using Assets.Rooms;
using Utils.Random;

namespace Assets.Actions
{
    class Teleporter : Action
    {
        private readonly string _actor;
        private readonly string _to;
        private readonly ActorRegistry _registry;
        private readonly Dispatcher _dispatcher;

        public Teleporter(string actor, string to, ActorRegistry registry, Dispatcher dispatcher)
        {
            _actor = actor;
            _to = to;
            _registry = registry;
            _dispatcher = dispatcher;
        }

        public override string Name => typeof(Teleporter).Name;
        public override void Act()
        {
            var room = (Room)_registry.GetActor(_to);

            var coordinate = room.Tiles.RandomEmptyTile();
            var move = new Move(_actor, _to, string.Empty, coordinate.ToString(), _registry);

            _dispatcher.Enqueue(move);
        }
    }
}
