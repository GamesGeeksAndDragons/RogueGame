using System.Collections.Generic;
using Assets.Actions;

namespace Assets.Messaging
{
    internal class Dispatcher
    {
        private readonly ActorRegistry _registry;
        private readonly Queue<Action> _actionQueue = new Queue<Action>();

        public Dispatcher(ActorRegistry registry)
        {
            _registry = registry;
        }

        public void Enqueue(Action action)
        {
            _actionQueue.Enqueue(action);
        }

        public void Dispatch()
        {
            while (_actionQueue.Count != 0)
            {
                var action = _actionQueue.Dequeue();
                action.Act();
            }
        }
    }
}
