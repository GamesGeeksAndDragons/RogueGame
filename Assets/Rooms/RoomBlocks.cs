using System.Linq;
using Utils;
using Utils.Coordinates;

namespace Assets.Rooms
{
    internal class RoomBlocks
    {
        public bool[,] Blocks { get; internal set; }
        public int RowCount => Blocks.RowUpperBound();
        public int ColumnCount => Blocks.ColumnUpperBound();

        internal RoomBlocks(bool[,] blocks)
        {
            Blocks = blocks;
        }

        public RoomBlocks(int numBlocks)
        {
            Blocks = new bool[numBlocks, numBlocks];
        }

        public bool IsInsideBounds(Coordinate point)
        {
            return point.Row >= 0 && point.Column >= 0 && 
                   point.Row <= RowCount && point.Column <= ColumnCount;
        }

        public bool IsTouchingAnyBlock(Coordinate point)
        {
            if (this[point]) return true;

            var north = point.Up();
            if (IsInsideBounds(north) && this[north]) return true;

            var south = point.Down();
            if (IsInsideBounds(south) && this[south]) return true;

            var east = point.Right();
            if (IsInsideBounds(east) && this[east]) return true;

            var west = point.Left();
            if (IsInsideBounds(west) && this[west]) return true;

            return false;
        }

        public bool CanMoveTo(Coordinate point)
        {
            return IsInsideBounds(point) && ! this[point];
        }

        public bool IsCornered(Coordinate point)
        {
            var canMove = CanMoveTo(point.Up());
            if (canMove) return false;

            canMove = CanMoveTo(point.Down());
            if (canMove) return false;

            canMove = CanMoveTo(point.Right());
            if (canMove) return false;

            canMove = CanMoveTo(point.Left());

            return !canMove;
        }

        public bool this[Coordinate point]
        {
            get => Blocks[point.Row, point.Column];
            set => Blocks[point.Row, point.Column] = value;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public virtual string ToString(bool coordinates)
        {
            return coordinates ? 
                Blocks.PrintCoordinates() :
                Blocks.Print(b => b ? "*" : ".");
        }

        private bool HasRowAnyBlocks(int row)
        {
            var slice = Blocks.SliceRow(row).ToList();
            return slice.Any(hasBlock => hasBlock);
        }

        private bool HasColumnAnyBlocks(int column)
        {
            var slice = Blocks.SliceColumn(column).ToList();
            return slice.Any(hasBlock => hasBlock);
        }

        private int MinRow()
        {
            for (var row = 0; row < RowCount; row++)
            {
                if (HasRowAnyBlocks(row))
                {
                    return row;
                }
            }

            return 0;
        }

        private int MaxRow()
        {
            for (var row = RowCount; row >= 0; row--)
            {
                if (HasRowAnyBlocks(row))
                {
                    return row;
                }
            }

            return RowCount;
        }

        private int MinColumn()
        {
            for (var y = 0; y < ColumnCount; y++)
            {
                if (HasColumnAnyBlocks(y))
                {
                    return y;
                }
            }

            return 0;
        }

        private int MaxColumn()
        {
            for (var y = ColumnCount; y >= 0; y--)
            {
                if (HasColumnAnyBlocks(y))
                {
                    return y;
                }
            }

            return ColumnCount;
        }

        public void ReduceLayout()
        {
            var minRow = MinRow();
            var maxRow = MaxRow();
            var minColumn = MinColumn();
            var maxColumn = MaxColumn();

            var newRow = maxRow - minRow + 1;
            var newCol = maxColumn - minColumn + 1;
            var newBlocks = new bool[newRow, newCol];

            for (var x = minRow; x <= maxRow; x++)
            {
                for (var y = minColumn; y <= maxColumn; y++)
                {
                    if (Blocks[x, y])
                    {
                        newBlocks[x - minRow, y - minColumn] = Blocks[x, y];
                    }
                }
            }

            Blocks = newBlocks;
        }
    }
}
