using System.Collections.Generic;
using Assets.Deeds;
using Utils;
using Utils.Dispatching;
using Utils.Enums;

namespace Assets.Messaging
{
    public interface IDispatcher
    {
        void EnqueueTeleport(IDispatchee dispatchee);
        void EnqueueMove(IDispatchee dispatchee, Compass8Points direction);
        void EnqueueUse(IDispatchee dispatchee, Compass8Points direction);
        void EnqueueStrike(string name, int hit, int damage);
        void Dispatch();
    }

    internal class Dispatcher : IDispatcher
    {
        private readonly IDispatchRegistry _registry;
        private readonly Queue<(IDispatchee Dispatchee, string Parameters)> _actionQueue = new Queue<(IDispatchee Dispatchee, string Parameters)>();

        public Dispatcher(IDispatchRegistry registry)
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
