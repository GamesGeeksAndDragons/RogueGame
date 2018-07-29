using Assets.Messaging;

namespace Assets.ActionEnqueue
{
    internal class Teleport : Enqueuer<Teleport>, IEnqueuer
    {
        private readonly string _actor;
        private readonly string _room;
        private readonly Dispatcher _dispatcher;

        public Teleport(string actor, string room, Dispatcher dispatcher)
        {
            _actor = actor;
            _room = room;
            _dispatcher = dispatcher;
        }

        public override void Enqueue()
        {
            var action = ActionName;
            var parameters = $"Room : {_room}, Actor : {_actor}";

            _dispatcher.Enqueue(action, parameters);
        }
    }
}
