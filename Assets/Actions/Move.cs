using Assets.Actors;
using Assets.Messaging;
using Assets.Rooms;
using Utils.Coordinates;
using Utils.Enums;

namespace Assets.Actions
{
    public class MoveAction : Action
    {
        private readonly string _actor;
        private readonly string _room;
        private readonly string _from;
        private readonly string _to;
        private readonly ActorRegistry _registry;

        public MoveAction(string actor, string room, string @from, string to, ActorRegistry registry)
        {
            _actor = actor;
            _room = room;
            _from = @from;
            _to = to;
            _registry = registry;
        }

        public override string Name => "MOVE";

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