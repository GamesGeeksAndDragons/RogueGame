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

        dieBuilder.AddFakeDie(6,
            1,2,3,4,5,6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4, 5, 6, 1, 2, 3, 4);

        var builder = new PlayerBuilder(dieBuilder);
        var player = builder.Build("Rogue", "Elf", Gender.Female);
        Assert.NotNull(player);
    }
}