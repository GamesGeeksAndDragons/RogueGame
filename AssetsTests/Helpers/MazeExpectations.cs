using Utils;

namespace AssetsTests.Helpers
{
    public interface IMazeExpectations
    {
        int Level { get; }
        string StartingMaze { get; }
        string ExpectedMaze { get; }
    }

    public abstract class MazeExpectations : IMazeExpectations
    {
        protected MazeExpectations()
        {
            Level = 1;
        }

        public int Level { get; protected set; }

        private readonly string _startingMaze;
        public string StartingMaze
        {
            get => _startingMaze;
            init => _startingMaze = value.Trim(CharHelpers.EndOfLine);
        }

        private readonly string _expectedMaze;
        public string ExpectedMaze
        {
            get => _expectedMaze;
            init => _expectedMaze = value.Trim(CharHelpers.EndOfLine);
        }
    }

    static class MazeTestHelpers
    {
        internal static void ThrowUnknownTest(int testNumber) => throw new ArgumentException($"Unknown test [{testNumber}]");
    }
}