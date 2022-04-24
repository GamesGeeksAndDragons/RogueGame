#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Enums;
using Parameters = System.Collections.Generic.List<(string Name, string Value)>;

namespace Assets.Actors
{
    internal class Wall : Dispatched<Wall>
    {
        public WallDirection WallType { get; private set; }

        internal Wall(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string state) 
            : base(dispatchRegistry, actionRegistry)
        {
            WallType = state.ToEnum<WallDirection>();
        }

        internal Wall(Wall wall) 
            : base(wall.DispatchRegistry, wall.ActionRegistry)
        {
            WallType = wall.WallType;
        }

        public override void UpdateState(Parameters state)
        {
            if (state.HasValue(nameof(WallType)))
            {
                WallType = state.ToValue<WallDirection>(nameof(WallType));
            }

            base.UpdateState(state);
        }

        public override Parameters CurrentState()
        {
            var state = base.CurrentState();

            state.AppendParameter(nameof(WallType), WallType);

            return state;
        }

        public static WallDirection GetDirection(string actor)
        {
            switch (actor)
            {
                case ActorDisplay.WallHorizontal: return WallDirection.Horizontal;
                case ActorDisplay.WallVertical: return WallDirection.Vertical;
                case ActorDisplay.WallTopLeftCorner: return WallDirection.TopLeftCorner;
                case ActorDisplay.WallTopRightCorner: return WallDirection.TopRightCorner;
                case ActorDisplay.WallBottomLeftCorner: return WallDirection.BottomLeftCorner;
                case ActorDisplay.WallBottomRightCorner: return WallDirection.BottomRightCorner;
            }

            throw new ArgumentException($"Unexpected actor [{actor}]");
        }

        public bool IsCorner => WallType.HasDirection(WallDirection.Corner);

        public Wall Rotate()
        {
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

            var newDirection = GetRotatedDirection().ToString();
            return new Wall(DispatchRegistry, ActionRegistry, newDirection);
        }
    }
}
