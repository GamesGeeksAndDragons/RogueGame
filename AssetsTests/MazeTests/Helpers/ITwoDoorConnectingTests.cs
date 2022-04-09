using Utils;

namespace AssetsTests.MazeTests.Helpers
{
    internal interface ITwoDoorConnectingTests
    {
        string StartingMaze { get; }
        string ExpectedMaze { get; }
    }

    internal abstract class TwoDoorConnectingTests : ITwoDoorConnectingTests
    {
        public string StartingMaze => Start.Trim(CharHelpers.EndOfLine);
        public string ExpectedMaze => Expected.Trim(CharHelpers.EndOfLine);

        protected abstract string Start { get; }
        protected abstract string Expected { get; }
    }
}