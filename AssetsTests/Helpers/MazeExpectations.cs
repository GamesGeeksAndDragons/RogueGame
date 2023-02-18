using Utils;
using Utils.Coordinates;

namespace AssetsTests.Helpers
{
    public interface IMazeExpectations
    {
        int Level { get; }
        string StartingMaze { get; }
        string ExpectedMaze { get; }

        string[] CharactersState { get; }
    }

    public abstract class MazeExpectations : IMazeExpectations
    {
        private readonly List<string> _characterState = new List<string>();

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

        public string[] CharactersState => _characterState.ToArray();

        protected string ActorCoordinates(string actor, int row, int column) => $"{actor} " + new Coordinate(row, column).FormatParameter();

        protected void AddCharacter(string state) => _characterState.Add(state);
    }

    static class MazeTestHelpers
    {
        internal static void ThrowUnknownTest(int testNumber) => throw new ArgumentException($"Unknown test [{testNumber}]");
    }
}