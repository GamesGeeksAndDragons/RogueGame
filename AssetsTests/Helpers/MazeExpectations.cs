using System;
using Utils;

namespace AssetsTests.Helpers
{
    public interface IMazeExpectations
    {
        string StartingMaze { get; }
        string ExpectedMaze { get; }
    }

    public abstract class MazeExpectations : IMazeExpectations
    {
        public string StartingMaze => Start.Trim(CharHelpers.EndOfLine);
        public string ExpectedMaze => Expected.Trim(CharHelpers.EndOfLine);

        protected abstract string Start { get; }
        protected abstract string Expected { get; }
    }

    static class MazeTestHelpers
    {
        internal static void ThrowUnknownTest(this int testNumber) => throw new ArgumentException($"Unknown test [{testNumber}]");
    }
}