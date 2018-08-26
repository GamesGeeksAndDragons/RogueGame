using System;
using Assets.Messaging;
using Utils.Coordinates;
using Utils.Enums;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string name, string value)>;

namespace Assets.Actors
{
    internal class Wall : Dispatchee<Wall>
    {
        public WallDirection WallType { get; private set; }

        public Wall(Coordinate coordinates, DispatchRegistry registry, WallDirection type) : base(coordinates, registry)
        {
            WallType = type;
        }

        private Wall(Wall wall) : base(wall.Coordinates, wall.Registry)
        {
            WallType = wall.WallType;
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
                case WallDirection.Horizontal: return "═";
                case WallDirection.Vertical: return "║";
                case WallDirection.TopLeftCorner: return "╔";
                case WallDirection.TopRightCorner: return "╗";
                case WallDirection.BottomLeftCorner: return "╚";
                case WallDirection.BottomRightCorner: return "╝";
            }

            throw new ArgumentException($"Unexpected WallType [{WallType}]");
        }

        public override Wall Create()
        {
            return new Wall(this);
        }
    }
}
