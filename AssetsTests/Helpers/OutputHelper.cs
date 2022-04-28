using System.Linq;
using Assets.Maze;
using Assets.Tiles;
using Utils;
using Xunit.Abstractions;

namespace AssetsTests.Helpers
{
    internal static class OutputHelper
    {
        private static void OutputDivider(this ITestOutputHelper output, int lineLength) =>
            output.WriteLine('='.ToPaddedString(lineLength));

        private static void Output(this string maze, ITestOutputHelper output, int lineLength)
        {
            output.OutputDivider(lineLength);
            output.WriteLine(maze);
        }


        internal static void OutputMazes(this ITestOutputHelper output, params string[] mazes)
        {
            var longestLength = GetLongestLineLength();

            foreach (var maze in mazes)
            {
                maze.Output(output, longestLength);
            }

            output.OutputDivider(longestLength);

            int GetLongestLineLength()
            {
                var longest = int.MaxValue;

                foreach (var maze in mazes)
                {
                    int length = maze.SplitIntoLines().Max(line => line.Length);
                    if (length < longest) longest = length;
                }

                return longest;
            }
        }

        internal static void OutputMazes(this ITestOutputHelper output, params Tiles[] mazes)
        {
            var stringMazes = mazes.Select(maze => maze.ToString()).ToArray();

            output.OutputMazes(stringMazes);
        }
    }
}