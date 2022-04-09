using System;
using System.Collections.Generic;
using Assets.Mazes;
using Utils.Dispatching;

namespace Assets.Deeds
{
    public class ActionRegistry
    {
        private readonly Dictionary<string, IAction> _actionImpl;
        private readonly Dictionary<string, IAction> _characterActions = new Dictionary<string, IAction>();

        internal ActionRegistry()
        {
            _actionImpl = new Dictionary<string, IAction>
            {
                {Deed.Hit, new StrikeAction()},
                {Deed.Strike, new StrikeAction()},
                {Deed.Move, new MoveAction()},
                {Deed.Teleport, new TeleportAction()},
                {Deed.Use, new UseAction()}
            };
        }

        public void RegisterAction(IDispatchee dispatchee, string action)
        {
            if(!Deed.IsValid(action)) throw new ArgumentNullException($"Unrecognised action [{action}] when registering for [{dispatchee.Name}]");

            if (_characterActions.ContainsKey(action)) return;

            _characterActions[action] = _actionImpl[action];
        }

        internal void RegisterMaze(Maze maze)
        {
            var teleport = (TeleportAction) _actionImpl[Deed.Teleport];
            teleport.Maze = maze;
        }

        public IAction GetAction(string dispatcheeName, string actionName)
        {
            if (_actionImpl.TryGetValue(actionName, out var action))
            {
                return action;
            }

            throw new ArgumentOutOfRangeException($"Couldn't find an action for [{actionName}]", actionName);
        }
    }
}