#nullable enable
using AssetsTests.Fakes;
using AssetsTests.Helpers;

namespace AssetsTests.ActionTests;

// ReSharper disable once InconsistentNaming
public class GivenThereAreCharactersWhoHaveNotBeenPositionedInTheMaze_WhenTeleported : MazeTestHelper
{
    public GivenThereAreCharactersWhoHaveNotBeenPositionedInTheMaze_WhenTeleported(ITestOutputHelper output) : base(output)
    {
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void ThenTheyShouldBeTeleportedToAnEmptyTile(int testNum)
    {
        var expectations = TestArrange(testNum);
        TestAct();
        TestAssert(expectations);
    }

    protected IMazeExpectations TestArrange(int testNum)
    {
        DieBuilder = new FakeDieBuilder(1, 1, 1, 1);
        var expectations = TeleportTestExpectations.GetExpectations(testNum);

        base.TestArrange(expectations);

        return expectations;
    }

    protected override void TestAct()
    {
        Dispatcher.Dispatch();
    }
}
