using System.Collections.Generic;
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

        public static IEnumerable<T> SliceRow<T>(this T[,] array, int row)
        {
            for (var column = 0; column <= array.ColumnUpperBound(); column++)
            {
                yield return array[row, column];
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
            return row    >= 0 && row    <= array.RowUpperBound() && 
                   column >= 0 && column <= array.ColumnUpperBound();
        }

        public static bool IsInside<T>(this T[,] array, Coordinate coordinates)
        {
            return array.IsInside(coordinates.Row, coordinates.Column);
        }
    }
}
