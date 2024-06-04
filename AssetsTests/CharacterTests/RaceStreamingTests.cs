using System.IO;
using System.Text.Json;
using Assets.PlayerRace;

namespace AssetsTests.CharacterTests;

public class RaceStreamingTests
{
    [Fact]
    public void CanDeserializeRaces()
    {
        var races = Races.Get();
    }
}