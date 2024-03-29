﻿#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Dispatching;
using Utils.Enums;

namespace Assets.Tiles;

internal class Wall : Dispatched<Wall>
{
    public WallDirection WallType { get; private set; }

    internal Wall(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
        : base(dispatchRegistry, actionRegistry, actor, state)
    {
        WallType = actor.ToWallDirection();
    }

    public override void UpdateState(Parameters state)
    {
        if (state.HasValue(nameof(WallType)))
        {
            WallType = state.ToValue<WallDirection>(nameof(WallType));
        }

        base.UpdateState(state);
    }

    public override Parameters CurrentState
    {
        get
        {
            var state = base.CurrentState;

            state.AppendParameter(nameof(WallType), WallType);

            return state;
        }
    }

    public Wall Rotate()
    {
        var newDirection = GetRotatedDirection();
        var newActor = newDirection.FromWallDirection();
        return new Wall(DispatchRegistry, ActionRegistry, newActor, "");

        WallDirection GetRotatedDirection()
        {
            switch (WallType)
            {
                case WallDirection.Horizontal: return WallDirection.Vertical;
                case WallDirection.Vertical: return WallDirection.Horizontal;
                case WallDirection.TopLeftCorner: return WallDirection.TopRightCorner;
                case WallDirection.TopRightCorner: return WallDirection.BottomRightCorner;
                case WallDirection.BottomLeftCorner: return WallDirection.TopLeftCorner;
                case WallDirection.BottomRightCorner: return WallDirection.BottomLeftCorner;
                default:
                    throw new ArgumentException($"Unexpected WallType [{WallType}]");
            }
        }
    }
}
