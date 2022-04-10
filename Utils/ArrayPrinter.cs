using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.Coordinates;

namespace Utils
{
    public static class ArrayPrinter
    {
        private static List<string> GetAxis<T>(T[,] array, Func<T[,], int> upperBound)
        {
            var max = upperBound(array);

            var axis = new List<string>();
            for (var i = 0; i <= max; i++)
            {
                axis.Add(i.ToString());
            }

            return axis;
        }

        private static List<string> GetColumnAxis<T>(T[,] array)
        {
            return GetAxis(array, ArrayHelpers.ColumnUpperBound);
        }

        private static List<string> GetRowAxis<T>(T[,] array)
        {
            return GetAxis(array, ArrayHelpers.RowUpperBound);
        }

        private static string[,] GetIntermediateArray<T>(this T[,] array, Func<T, string> printer)
        {
            var rowMax = array.RowUpperBound();
            var colMax = array.ColumnUpperBound();

            var intermediate = new string[rowMax + 1, colMax + 1];

            for (int row = 0; row <= rowMax; row++)
            {
                for (int col = 0; col <= colMax; col++)
                {
                    var item = array[row, col];
                    intermediate[row, col] = printer(item);
                }
            }

            return intermediate;
        }

        private static int GetMaxLength(string item, int max)
        {
            if (!item.IsNullOrEmpty())
            {
                max = Math.Max(item.Length, max);
            }

            return max;
        }

        private static int GetMaxSizeForAxis(IEnumerable<string> axis)
        {
            int max = 0;
            foreach (var item in axis)
            {
                max = GetMaxLength(item, max);
            }

            return max;
        }

        private static int GetMaxSizeForItem(string[,] intermediate)
        {
            var rowMax = intermediate.RowUpperBound();
            var colMax = intermediate.ColumnUpperBound();

            var max = 0;
            for (var row = 0; row <= rowMax; row++)
            {
                for (var col = 0; col <= colMax; col++)
                {
                    max = GetMaxLength(intermediate[row, col], max);
                }
            }

            return max;
        }

        private static string PrintItem(string item, int maxLength, bool shouldPad)
        {
            var length = Math.Max(0, maxLength - item.Length);

            shouldPad = shouldPad && length > 0;
            if (shouldPad)
            {
                item += ' '.ToPaddedString(length);
            }

            return item;
        }

        private static string PrintRow(string[,] intermediate, List<string> yAxis, int row, int axisLength, int itemLength)
        {
            var sb = new StringBuilder();

            var prefix = PrintItem(yAxis[row], axisLength, true) + "|";
            sb.Append(prefix);

            var colMax = intermediate.ColumnUpperBound();
            for (int col = 0; col <= colMax; col++)
            {
                var item = intermediate[row, col];
                var line = PrintItem(item, itemLength, true);
                sb.Append(line);
            }

            var suffix = "|" + PrintItem(yAxis[row], axisLength, true);
            sb.Append(suffix);

            return sb.ToString();
        }

        private static string Print(string[,] intermediate, List<string> xAxis, List<string> yAxis)
        {
            var axisLength = GetMaxSizeForAxis(yAxis);
            var itemLength = GetMaxSizeForItem(intermediate);

            var axis = PrintColumnHeader(xAxis, axisLength, itemLength);
            var sb = new StringBuilder();
            sb.AppendLine(axis);
            sb.Append('-'.ToPaddedString(axis.Length));

            var maxY = intermediate.RowUpperBound();

            for (int y = 0; y <= maxY; y++)
            {
                sb.AppendLine();

                var row = PrintRow(intermediate, yAxis, y, axisLength, itemLength);
                sb.Append(row);
            }

            sb.AppendLine();
            sb.AppendLine('-'.ToPaddedString(axis.Length));
            sb.Append(axis);

            return sb.ToString();
        }

        private static string PrintColumnHeader(List<string> axis, int axisLength, int maxItemLength)
        {
            var sb = new StringBuilder();
            var prefix = ' '.ToPaddedString(axisLength) + "|";
            sb.Append(prefix);

            foreach (var item in axis)
            {
                var printed = item;

                printed = printed.Length > maxItemLength ? 
                    printed.Right(maxItemLength) : 
                    printed.AddPadding(' ', maxItemLength - item.Length);

                sb.Append(printed);
            }

            var suffix = "|" + ' '.ToPaddedString(axisLength);
            sb.Append(suffix);

            return sb.ToString();
        }

        public static string Print<T>(this T[,] array, Func<T, string> printer = null)
        {
            if (printer == null)
            {
                printer = t => t.ToString();
            }

            var intermediate = GetIntermediateArray(array, printer);
            var xAxis = GetColumnAxis(array);
            var yAxis = GetRowAxis(array);

            return Print(intermediate, xAxis, yAxis);
        }

        public static string PrintCoordinates<T>(this T[,] array)
        {
            var intermediate = BuildCoordinateArray(array);

            return Print(intermediate);
        }

        private static Coordinate[,] BuildCoordinateArray<T>(T[,] array)
        {
            int maxColumns = array.ColumnUpperBound() + 1;
            int maxRows = array.RowUpperBound() + 1;

            var coordinateArray = new Coordinate[maxRows, maxColumns];
            for (var row = 0; row < maxRows; row++)
            {
                for (var col = 0; col < maxColumns; col++)
                {
                    coordinateArray[row, col] = new Coordinate(row, col);
                }
            }

            return coordinateArray;
        }
    }
}
