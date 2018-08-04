using Assets.Messaging;

namespace Assets.ActionEnqueue
{
    class Move : Enqueuer<Move>, IEnqueuer
    {
        private readonly string _dispatchee;
        private readonly string _direction;
        private readonly Dispatcher _dispatcher;

        public Move(string dispatchee, string direction, Dispatcher dispatcher)
        {
            _dispatchee = dispatchee;
            _direction = direction;
            _dispatcher = dispatcher;
        }

        public override void Enqueue()
        {
            var action = ActionName;
            var parameters = $"Dispatchee : {_dispatchee} Direction : {_direction}";

            _dispatcher.Enqueue(action, parameters);
        }
    }
}
