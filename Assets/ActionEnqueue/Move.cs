using Assets.Messaging;

namespace Assets.ActionEnqueue
{
    class Move : Enqueuer<Move>, IEnqueuer
    {
        private readonly string _actor;
        private readonly string _direction;
        private readonly Dispatcher _dispatcher;

        public Move(string actor, string direction, Dispatcher dispatcher)
        {
            _actor = actor;
            _direction = direction;
            _dispatcher = dispatcher;
        }

        public override void Enqueue()
        {
            var action = ActionName;
            var parameters = $"Actor : {_actor} Direction : {_direction}";

            _dispatcher.Enqueue(action, parameters);
        }
    }
}
