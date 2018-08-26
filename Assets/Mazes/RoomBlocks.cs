using System.Linq;
using Utils;
using Utils.Coordinates;

namespace Assets.Mazes
{
    internal class RoomBlocks
    {
        private bool[,] Blocks { get; }
        public int RowUpperBound => Blocks.RowUpperBound();
        public int ColumnUpperBound => Blocks.ColumnUpperBound();

        internal RoomBlocks(bool[,] blocks)
        {
            Blocks = blocks;
        }

        public RoomBlocks(int numBlocks)
        {
            Blocks = new bool[numBlocks, numBlocks];
        }

        public bool IsTouchingAnyBlock(Coordinate point)
        {
            if (this[point]) return true;

            var north = point.Up();
            if (north.IsInside(RowUpperBound, ColumnUpperBound) && this[north]) return true;

            var south = point.Down();
            if (south.IsInside(RowUpperBound, ColumnUpperBound) && this[south]) return true;

            var east = point.Right();
            if (east.IsInside(RowUpperBound, ColumnUpperBound) && this[east]) return true;

            var west = point.Left();
            if (west.IsInside(RowUpperBound, ColumnUpperBound) && this[west]) return true;

            return false;
        }

        public bool CanMoveTo(Coordinate point)
        {
            return point.IsInside(RowUpperBound, ColumnUpperBound) && ! this[point];
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

        public bool IsInside(Coordinate coordinate)
        {
            return Blocks.IsInside(coordinate);
        }

        private bool HasColumnAnyBlocks(int column)
        {
            var slice = Blocks.SliceColumn(column).ToList();
            return slice.Any(hasBlock => hasBlock);
        }

        private int MinRow()
        {
            for (var row = 0; row < RowUpperBound; row++)
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
            for (var row = RowUpperBound; row >= 0; row--)
            {
                if (HasRowAnyBlocks(row))
                {
                    return row;
                }
            }

            return RowUpperBound;
        }

        private int MinColumn()
        {
            for (var column = 0; column <= ColumnUpperBound; column++)
            {
                if (HasColumnAnyBlocks(column))
                {
                    return column;
                }
            }

            return 0;
        }

        private int MaxColumn()
        {
            for (var column = ColumnUpperBound; column >= 0; column--)
            {
                if (HasColumnAnyBlocks(column))
                {
                    return column;
                }
            }

            return ColumnUpperBound;
        }

        public RoomBlocks ReduceLayout()
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

            return new RoomBlocks(newBlocks);
        }
    }
}
