using Assets.StartingPlayerStatistics;

namespace AssetsTests.CharacterTests;

public class RaceStreamingTests
{
    [Fact]
    public void CanDeserializeRaces()
    {
        var races = PlayerRaces.Get();
    }
}