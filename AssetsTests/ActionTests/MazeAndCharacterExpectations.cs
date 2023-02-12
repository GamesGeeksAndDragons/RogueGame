using AssetsTests.Helpers;
using Utils.Coordinates;

namespace AssetsTests.ActionTests;

public interface ICharacterExpectations
{
    string Me { get; }
}

abstract class MazeAndCharacterExpectations : MazeExpectations, ICharacterExpectations
{
    protected string ActorCoordinates(string actor, int row, int column) => $"{actor} " + new Coordinate(row, column).FormatParameter();

    public abstract string Me { get; }
}