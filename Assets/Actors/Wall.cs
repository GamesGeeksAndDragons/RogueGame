using System;
using System.Collections.Generic;
using System.Text;
using Utils.Enums;

namespace Assets.Actors
{
    public class Wall : Actor
    {
        public WallDirection WallType { get; }

        public Wall(WallDirection type)
        {
            WallType = type;
        }

        public override string Name => "WALL";

        public override Actor Clone()
        {
            return new Wall(WallType);
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
