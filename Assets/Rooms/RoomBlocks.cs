using System;
using System.Collections.Generic;
using System.Text;
using Assets.Coordinates;

namespace Assets.Rooms
{
    internal class RoomBlocks
    {
        private readonly bool[,] _blocks;
        public int UpperBound { get; }

        public RoomBlocks(int numBlocks)
        {
            UpperBound = numBlocks - 1;
            _blocks = new bool[numBlocks, numBlocks];
        }

        public bool IsInsideBounds(Coordinate point)
        {
            return point.X >= 0 && point.Y >= 0 && point.X <= UpperBound && point.Y <= UpperBound;
        }


        public bool IsTouchingAnyBlock(Coordinate point)
        {
            if (this[point]) return true;

            var north = point.GoNorth();
            if (IsInsideBounds(north) && this[north]) return true;

            var south = point.GoSouth();
            if (IsInsideBounds(south) && this[south]) return true;

            var east = point.GoEast();
            if (IsInsideBounds(east) && this[east]) return true;

            var west = point.GoWest();
            if (IsInsideBounds(west) && this[west]) return true;

            return false;
        }

        public bool CanMoveTo(Coordinate point)
        {
            return IsInsideBounds(point) && ! this[point];
        }

        public bool IsCornered(Coordinate point)
        {
            var canMove = CanMoveTo(point.GoNorth());
            if (canMove) return false;

            canMove = CanMoveTo(point.GoSouth());
            if (canMove) return false;

            canMove = CanMoveTo(point.GoEast());
            if (canMove) return false;

            canMove = CanMoveTo(point.GoWest());

            return !canMove;
        }

        public bool this[Coordinate point]
        {
            get => _blocks[point.X, point.Y];
            set => _blocks[point.X, point.Y] = value;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int y = 0; y <= UpperBound; y++)
            {
                for (int x = 0; x <= UpperBound; x++)
                {
                    sb.Append(this[new Coordinate(x,y)] ? "*" : ".");
                }

                sb.AppendLine("|");
            }

            return sb.ToString();
        }
    }
}
