#nullable enable
using Assets.Characters;
using Assets.Deeds;
using Utils;
using Utils.Dispatching;
using Utils.Enums;

namespace Assets.Messaging
{
    public interface IDispatcher
    {
        void EnqueueTeleport(ICharacter who);
        void EnqueueMove(ICharacter who, Compass8Points direction);
        void EnqueueUse(ICharacter who, Compass8Points direction);
        void EnqueueStrike(string name, int hit, int damage);
        void Dispatch();
    }

    internal class Dispatcher : IDispatcher
    {
        private readonly IDispatchRegistry _dispatchRegistry;
        private readonly IActionRegistry _actionRegistry;
        private readonly Queue<(ICharacter Dispatched, string Parameters)> _actionQueue = new Queue<(ICharacter Dispatched, string Parameters)>();

        public Dispatcher(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
        {
            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;
        }

        private void Enqueue(ICharacter who, string parameters)
        {
            _actionQueue.Enqueue((who, parameters));
        }


        public void EnqueueTeleport(ICharacter who)
        {
            var parameters = Deed.Teleport.FormatParameter("");
            Enqueue(who, parameters);
        }

        public void EnqueueMove(ICharacter who, Compass8Points direction)
        {
            var parameters = Deed.Move.FormatParameter(direction);
            Enqueue(who, parameters);
        }

        public void EnqueueUse(ICharacter who, Compass8Points direction)
        {
            EnqueueMove(who, direction);
        }

        public void EnqueueStrike(string name, int hit, int damage)
        {
            var who = (ICharacter)_dispatchRegistry.GetDispatched(name);

            var parameters = Deed.Hit.FormatParameter(hit)
//                .AppendParameter("Damage", damage)
                ;

            Enqueue(who, parameters);
        }

        public void Dispatch()
        {
            while (_actionQueue.Count != 0)
            {
                var (who, parameters) = _actionQueue.Dequeue();
                ActionDispatch(who, parameters);
            }

            void ActionDispatch(ICharacter who, string parameters)
            {
                var parametersList = parameters.ToParameters();

                foreach (var parameter in parametersList)
                {
                    var action = _actionRegistry.GetAction(who.Name, parameter.Name);
                    action.Act(_dispatchRegistry, who, parameter.Value);
                }
            }
    }
}
}
