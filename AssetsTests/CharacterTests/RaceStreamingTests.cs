using Assets.PlayerBuilder;
using AssetsTests.Fakes;

namespace AssetsTests.CharacterTests;

public class RaceStreamingTests
{
    [Fact]
    public void CanBuildAPlayer()
    {
        var dieBuilder = new FakeDieBuilder(3,
            3, 3, 3, 3, 2, 3, 3, 2, 3, 3, 2, 3, 3, 2, 3, 3, 2, 3,
            1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 2, 3, 1, 1, 1);


        var builder = new PlayerBuilder(dieBuilder);
        var player = builder.Build("Mage", "Elf", Gender.Female);
        Assert.NotNull(player);
    }
}