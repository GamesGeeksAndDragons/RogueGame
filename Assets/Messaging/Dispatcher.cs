using System.Collections.Generic;

namespace Assets.Messaging
{
    internal class Dispatcher
    {
        private readonly DispatchRegistry _registry;
        private readonly Queue<(string action, string parameters)> _actionQueue = new Queue<(string action, string parameters)>();

        public Dispatcher(DispatchRegistry registry)
        {
            _registry = registry;
        }

        public void Enqueue(string action, string parameters)
        {
            _actionQueue.Enqueue((action, parameters));
        }

        public void Dispatch()
        {
            while (_actionQueue.Count != 0)
            {
                var (action, parameters) = _actionQueue.Dequeue();
                Dispatch(action, parameters);
            }
        }

        private void Dispatch(string action, string parameters)
        {
            foreach (var dispatchee in _registry.Dispatchees)
            {
                dispatchee.Dispatch(action, parameters);
            }
        }
    }
}
