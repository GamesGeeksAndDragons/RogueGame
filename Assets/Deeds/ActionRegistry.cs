﻿using System;
using System.Collections.Generic;
using Assets.Mazes;
using Assets.Tiles;
using Utils.Dispatching;

namespace Assets.Deeds
{
    public interface IActionRegistry
    {
        void RegisterAction(IDispatchee dispatchee, string action);
        void RegisterTiles(ITiles tiles);
        IAction GetAction(string dispatcheeName, string actionName);
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

        public void RegisterAction(IDispatchee dispatchee, string action)
        {
            if(!Deed.IsValid(action)) throw new ArgumentNullException($"Unrecognised action [{action}] when registering for [{dispatchee.Name}]");

            if (_characterActions.ContainsKey(action)) return;

            _characterActions[action] = _actionImpl[action];
        }

        public void RegisterTiles(ITiles tiles)
        {
            foreach (var action in _actionImpl)
            {
                action.Value.Tiles = tiles;
            }
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