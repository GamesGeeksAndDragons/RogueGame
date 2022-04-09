using System;
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Enums;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string Name, string Value)>;

namespace Assets.Actors
{
    internal class Wall : Dispatchee<Wall>
    {
        public WallDirection WallType { get; private set; }

        internal Wall(Coordinate coordinates, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry, string state) 
            : base(coordinates, dispatchRegistry, actionRegistry)
        {
            WallType = state.ToEnum<WallDirection>();
        }

        internal Wall(Wall wall) 
            : base(wall.Coordinates, wall.DispatchRegistry, wall.ActionRegistry)
        {
            WallType = wall.WallType;
        }

        public override Wall Create()
        {
            return ActorBuilder.Build(this);
        }

        public override void UpdateState(Wall wall, ExtractedParameters state)
        {
            if (state.HasValue(nameof(WallType)))
            {
                wall.WallType = state.ToValue<WallDirection>(nameof(WallType));
            }

            base.UpdateState(wall, state);
        }
        
        public static string FormatState(WallDirection? wallType = null, int? armourClass = null, Coordinate? coordinates = null, string uniqueId = null)
        {
            var state = string.Empty;

            if (wallType.HasValue) state += nameof(WallType).FormatParameter(wallType.Value);

            return state + Dispatchee<Wall>.FormatState(coordinates, uniqueId);
        }

        public override string ToString()
        {
            switch (WallType)
            {
                case WallDirection.Horizontal: return ActorDisplay.WallHorizontal.ToString();
                case WallDirection.Vertical: return ActorDisplay.WallVertical.ToString();
                case WallDirection.TopLeftCorner: return ActorDisplay.WallTopLeftCorner.ToString();
                case WallDirection.TopRightCorner: return ActorDisplay.WallTopRightCorner.ToString();
                case WallDirection.BottomLeftCorner: return ActorDisplay.WallBottomLeftCorner.ToString();
                case WallDirection.BottomRightCorner: return ActorDisplay.WallBottomRightCorner.ToString();
            }

            throw new ArgumentException($"Unexpected WallType [{WallType}]");
        }

        public static WallDirection GetDirection(char actor)
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

        public bool IsCorner
        {
            get
            {
                switch (WallType)
                {
                    case WallDirection.Horizontal:
                    case WallDirection.Vertical:
                        return false;
                    case WallDirection.TopLeftCorner:
                    case WallDirection.TopRightCorner:
                    case WallDirection.BottomLeftCorner:
                    case WallDirection.BottomRightCorner:
                        return true;
                    default:
                        throw new ArgumentException($"Unexpected WallType [{WallType}]");
                }
            }
        }

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
            return new Wall(Coordinates, DispatchRegistry, ActionRegistry, newDirection);
        }
    }
}
