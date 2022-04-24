#nullable enable
using Assets.Deeds;
using Utils;
using Utils.Dispatching;
using Utils.Enums;

namespace Assets.Messaging
{
    public interface IDispatcher
    {
        void EnqueueTeleport(IDispatched dispatched);
        void EnqueueMove(IDispatched dispatched, Compass8Points direction);
        void EnqueueUse(IDispatched dispatched, Compass8Points direction);
        void EnqueueStrike(string name, int hit, int damage);
        void Dispatch();
    }

    internal class Dispatcher : IDispatcher
    {
        private readonly IDispatchRegistry _registry;
        private readonly Queue<(IDispatched Dispatchee, string Parameters)> _actionQueue = new Queue<(IDispatched Dispatchee, string Parameters)>();

        public Dispatcher(IDispatchRegistry registry)
        {
            _registry = registry;
        }

        private void Enqueue(IDispatched who, string parameters)
        {
            _actionQueue.Enqueue((who, parameters));
        }


        public void EnqueueTeleport(IDispatched dispatched)
        {
            var parameters = Deed.Teleport.FormatParameter("");
            Enqueue(dispatched, parameters);
        }

        public void EnqueueMove(IDispatched dispatched, Compass8Points direction)
        {
            var parameters = Deed.Move.FormatParameter(direction);
            Enqueue(dispatched, parameters);
        }

        public void EnqueueUse(IDispatched dispatched, Compass8Points direction)
        {
            EnqueueMove(dispatched, direction);
        }

        public void EnqueueStrike(string name, int hit, int damage)
        {
            var dispatchee = _registry.GetDispatched(name);

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
