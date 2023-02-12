#nullable enable
using Assets.Deeds;
using Assets.Level;
using Assets.Personas;
using Utils;
using Utils.Dispatching;
using Utils.Enums;

namespace Assets.Messaging
{
    public interface IDispatcher
    {
        void EnqueueTeleport(IGameLevel level, ICharacter who);
        void EnqueueMove(IGameLevel level, ICharacter who, Compass8Points direction);
        void EnqueueUse(IGameLevel level, ICharacter who, Compass8Points direction);
        void EnqueueStrike(IGameLevel level, string name, int hit, int damage);
        void Dispatch();
    }

    internal class Dispatcher : IDispatcher
    {
        private readonly IDispatchRegistry _dispatchRegistry;
        private readonly IActionRegistry _actionRegistry;
        private readonly Queue<(IGameLevel Level, ICharacter Who, string ActionValue)> _actionQueue = new Queue<(IGameLevel Level, ICharacter Who, string ActionValue)>();

        public Dispatcher(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
        {
            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;
        }

        private void Enqueue(IGameLevel level, ICharacter who, string parameters)
        {
            _actionQueue.Enqueue((level, who, parameters));
        }


        public void EnqueueTeleport(IGameLevel level, ICharacter who)
        {
            var parameters = Deed.Teleport.FormatParameter(who.Coordinates);
            Enqueue(level, who, parameters);
        }

        public void EnqueueMove(IGameLevel level, ICharacter who, Compass8Points direction)
        {
            var parameters = Deed.Move.FormatParameter(direction);
            Enqueue(level, who, parameters);
        }

        public void EnqueueUse(IGameLevel level, ICharacter who, Compass8Points direction)
        {
            EnqueueMove(level, who, direction);
        }

        public void EnqueueStrike(IGameLevel level, string name, int hit, int damage)
        {
            var who = (ICharacter)_dispatchRegistry.GetDispatched(name);

            var parameters = Deed.Hit.FormatParameter(hit)
//                .AppendParameter("Damage", damage)
                ;

            Enqueue(level, who, parameters);
        }

        public void Dispatch()
        {
            while (_actionQueue.Count != 0)
            {
                var (level, who, parameters) = _actionQueue.Dequeue();
                ActionDispatch(level, who, parameters);
            }

            void ActionDispatch(IGameLevel level, ICharacter who, string parameters)
            {
                var parametersList = parameters.ToParameters();

                foreach (var parameter in parametersList)
                {
                    var action = _actionRegistry.GetAction(who.Name, parameter.Name);
                    action.Act(level, who, parameter.Value);
                }
            }
    }
}
}
