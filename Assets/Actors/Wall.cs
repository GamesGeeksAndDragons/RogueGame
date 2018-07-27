using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;
using Utils.Coordinates;
using Utils.Enums;

namespace Assets.Actors
{
    public class Wall : Actor
    {
        public WallDirection WallType { get; }

        public Wall(Coordinate coordinates, WallDirection type) : base(coordinates)
        {
            WallType = type;
        }

        public override string Name => "WALL";
        public override string UniqueId { get; internal set; }

        public override Actor Clone()
        {
            return new Wall(Coordinates, WallType);
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
