using System.Collections.Generic;
using Utils.Coordinates;
using Utils.Enums;

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

        private void Enqueue(string action, string parameters)
        {
            _actionQueue.Enqueue((action, parameters));
        }

        public void EnqueueTeleport(IDispatchee dispatchee)
        {
            Enqueue(Actions.Teleport, $"Dispatchee [{dispatchee.UniqueId}]");
        }

        public void EnqueueMove(IDispatchee dispatchee, Compass8Points direction)
        {
            Enqueue(Actions.Move, $"Dispatchee [{dispatchee.UniqueId}] Direction [{direction.ToString()}]");
        }

        public void EnqueueUse(IDispatchee dispatchee, Compass8Points direction)
        {
            Enqueue(Actions.Use, $"Dispatchee [{dispatchee.UniqueId}] Direction [{direction.ToString()}]");
        }

        public void EnqueueStrike(Coordinate coordinates, int hit, int damage)
        {
            Enqueue(Actions.Strike, $"Coordinates [{coordinates}] Hit [{hit}] Damage [{damage}]");
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
