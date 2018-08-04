using System;
using Assets.Messaging;
using Utils.Coordinates;
using Utils.Enums;

namespace Assets.Actors
{
    internal class Wall : Dispatchee<Wall>
    {
        public WallDirection WallType { get; }

        public Wall(Coordinate coordinates, DispatchRegistry registry, WallDirection type) : base(coordinates, registry)
        {
            WallType = type;
        }

        private Wall(Wall rhs) : base(rhs)
        {
        }

        public override IDispatchee Clone(string parameters=null)
        {
            return new Wall(this);
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
    }
}
