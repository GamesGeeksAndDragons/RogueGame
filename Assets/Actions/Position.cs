using Assets.Messaging;
using Assets.Rooms;
using Utils.Coordinates;

namespace Assets.Actions
{
    public class Position : Action
    {
        private readonly string _actor;
        private readonly string _room;
        private readonly string _to;
        private readonly ActorRegistry _registry;

        public Position(string actor, string room, string to, ActorRegistry registry)
        {
            _actor = actor;
            _room = room;
            _to = to;
            _registry = registry;
        }

        public override string Name => typeof(Position).Name;

        public override void Act()
        {
            var coordinates = _to.ToCoordinates();

            var actor = _registry.GetActor(_actor);
            var newActor = actor.Move(coordinates);

            _registry.Deregister(actor);
            _registry.Register(newActor);

            var room = (Room) _registry.GetActor(_room);
            var roomWithActor = room.Place(actor, coordinates);

            _registry.Deregister(room);
            _registry.Register(roomWithActor);
        }
    }
}