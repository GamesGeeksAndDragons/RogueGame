using System.Collections.Generic;

namespace Assets.Messaging
{
    internal class Dispatcher
    {
        private readonly ActorRegistry _registry;
        private readonly Queue<(string action, string parameters)> _actionQueue = new Queue<(string action, string parameters)>();

        public Dispatcher(ActorRegistry registry)
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
            foreach (var actor in _registry.Actors)
            {
                actor.Dispatch(action, parameters);
            }
        }
    }
}
