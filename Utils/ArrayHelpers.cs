using System.Collections.Generic;
using System.Linq;
using Utils.Coordinates;

namespace Utils
{
    public static class ArrayHelpers
    {
        public static int ColumnUpperBound<T>(this T[,] array)
        {
            return array.GetUpperBound(1);
        }

        public static int RowUpperBound<T>(this T[,] array)
        {
            return array.GetUpperBound(0);
        }

        public static (int MaxRow, int MaxColumn) UpperBounds<T>(this T[,] array)
        {
            return (array.RowUpperBound(), array.ColumnUpperBound());
        }

        public static T[] ExtractRow<T>(this T[,] matrix, int row)
        {
            var extractedRow = matrix.SliceRow(row).ToArray();
            return extractedRow;
        }

        public static IEnumerable<T> SliceRow<T>(this T[,] matrix, int row)
        {
            var columnUpperBound = matrix.ColumnUpperBound();
            for (var column = 0; column <= columnUpperBound; column++)
            {
                yield return matrix[row, column];
            }
        }

        public static IEnumerable<T> SliceColumn<T>(this T[,] array, int column)
        {
            for (var row = 0; row <= array.RowUpperBound(); row++)
            {
                yield return array[row, column];
            }
        }

        public static bool IsInside<T>(this T[,] array, int row, int column)
        {
            var bounds = array.UpperBounds();
            return row    >= 0 && row    <= bounds.MaxRow && 
                   column >= 0 && column <= bounds.MaxColumn;
        }

        public static bool IsInside<T>(this T[,] array, Coordinate coordinates)
        {
            return array.IsInside(coordinates.Row, coordinates.Column);
        }

        public static string[,] CloneStrings(this string[,] array)
        {
            var maxRow = array.RowUpperBound();
            var maxCol = array.ColumnUpperBound();
            var clone = new string[maxRow + 1, maxCol + 1];

            for (var row = 0; row <= maxRow; row++)
            {
                for (var col = 0; col <= maxCol; col++)
                {
                    var value = array[row, col];
                    if (value == null) continue;

                    clone[row, col] = value;
                }
            }

            return clone;
        }

        public static T[,] Duplicate<T>(this T[,] matrix)
        {
            var maxRow = matrix.RowUpperBound();
            var maxCol = matrix.ColumnUpperBound();
            var duplicate = new T[maxRow + 1, maxCol + 1];

            for (var row = 0; row <= maxRow; row++)
            {
                for (var col = 0; col <= maxCol; col++)
                {
                    var value = matrix[row, col];

                    duplicate[row, col] = value;
                }
            }

            return duplicate;
        }
    }
}
