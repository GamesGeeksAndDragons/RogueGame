using System.Collections.Generic;
using System.Linq;
using Assets.Deeds;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Enums;

namespace Assets.Messaging
{
    internal class Dispatcher
    {
        private readonly DispatchRegistry _registry;
        private readonly Queue<(IDispatchee Dispatchee, string Parameters)> _actionQueue = new Queue<(IDispatchee Dispatchee, string Parameters)>();

        public Dispatcher(DispatchRegistry registry)
        {
            _registry = registry;
        }

        private void Enqueue(IDispatchee who, string parameters)
        {
            _actionQueue.Enqueue((who, parameters));
        }


        public void EnqueueTeleport(IDispatchee dispatchee)
        {
            var parameters = Deed.Teleport.FormatParameter("");
            Enqueue(dispatchee, parameters);
        }

        public void EnqueueMove(IDispatchee dispatchee, Compass8Points direction)
        {
            var parameters = Deed.Move.FormatParameter(direction);
            Enqueue(dispatchee, parameters);
        }

        public void EnqueueUse(IDispatchee dispatchee, Compass8Points direction)
        {
            EnqueueMove(dispatchee, direction);
        }

        public void EnqueueStrike(string name, int hit, int damage)
        {
            var dispatchee = _registry.GetDispatchee(name);

            var parameters = Deed.Hit.FormatParameter(hit)
//                .AppendParameter("Damage", damage)
                ;

            Enqueue(dispatchee, parameters);
        }

        public void Dispatch()
        {
            while (_actionQueue.Count != 0)
            {
                var (who, parameters) = _actionQueue.Dequeue();
                who.Dispatch(parameters);
            }
        }
    }
}
