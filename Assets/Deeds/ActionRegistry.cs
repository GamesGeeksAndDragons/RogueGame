#nullable enable
using Assets.Mazes;
using Utils.Dispatching;

namespace Assets.Deeds
{
    public interface IActionRegistry
    {
        void RegisterAction(IDispatched dispatched, string action);
        IAction GetAction(string dispatchedName, string actionName);
    }

    internal class ActionRegistry : IActionRegistry
    {
        private readonly Dictionary<string, Action> _actionImpl;
        private readonly Dictionary<string, Action> _characterActions = new Dictionary<string, Action>();

        internal ActionRegistry()
        {
            _actionImpl = new Dictionary<string, Action>
            {
                {Deed.Hit, new StrikeAction()},
                {Deed.Strike, new StrikeAction()},
                {Deed.Move, new MoveAction()},
                {Deed.Teleport, new TeleportAction()},
                {Deed.Use, new UseAction()}
            };
        }

        public void RegisterAction(IDispatched dispatched, string action)
        {
            if(!Deed.IsValid(action)) throw new ArgumentNullException($"Unrecognised action [{action}] when registering for [{dispatched.Name}]");

            if (_characterActions.ContainsKey(action)) return;

            _characterActions[action] = _actionImpl[action];
        }

        public IAction GetAction(string dispatchedName, string actionName)
        {
            if (_actionImpl.TryGetValue(actionName, out var action))
            {
                return action;
            }

            throw new ArgumentOutOfRangeException($"Couldn't find an action for [{actionName}]", actionName);
        }
    }
}