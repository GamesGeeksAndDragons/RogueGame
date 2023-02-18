#nullable enable
using AssetsTests.Helpers;
using Utils.Coordinates;
using Utils.Display;

namespace AssetsTests.ActionTests;

public interface ICharacterExpectations : IMazeExpectations
{
    string Me { get; }
}

abstract class MazeAndCharacterExpectations : MazeExpectations, ICharacterExpectations
{
    protected string ActorCoordinates(string actor, int row, int column) => $"{actor} " + new Coordinate(row, column).FormatParameter();

    public virtual string Me => CharacterDisplay.Me;
}