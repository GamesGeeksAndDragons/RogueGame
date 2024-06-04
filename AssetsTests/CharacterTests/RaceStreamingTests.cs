using Assets.StartingPlayerStatistics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AssetsTests.CharacterTests;

public class RaceStreamingTests
{
    [Fact]
    public void CanDeserializeRaces()
    {
        var races = PlayerRaces.Get();

        Assert.Fail("Must have a valid test here");
    }

    [Fact]
    public void CanDeserializeClasses()
    {
        var classes = PlayerClasses.Get();
    }
}