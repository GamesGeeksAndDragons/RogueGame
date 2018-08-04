using Assets.Messaging;

namespace Assets.ActionEnqueue
{
    internal class Teleport : Enqueuer<Teleport>, IEnqueuer
    {
        private readonly string _dispatchee;
        private readonly Dispatcher _dispatcher;

        public Teleport(string dispatchee, Dispatcher dispatcher)
        {
            _dispatchee = dispatchee;
            _dispatcher = dispatcher;
        }

        public override void Enqueue()
        {
            var action = ActionName;
            var parameters = $"Dispatchee : {_dispatchee}";

            _dispatcher.Enqueue(action, parameters);
        }
    }
}
