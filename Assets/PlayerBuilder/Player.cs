using Assets.StartingPlayerStatistics;

namespace Assets.PlayerBuilder;

public interface IPlayer
{
    PlayerRace Race { get; }
    PlayerClass Class { get;}
    PlayerStats Maximum { get; }
    PlayerStats Current { get; }
}

internal class Player : IPlayer
{
    internal Player()
    {
    }

    public PlayerRace Race { get; init; } = null!;
    public PlayerClass Class { get; init; } = null!;
    public PlayerStats Maximum { get; init; } = null!;
    public PlayerStats Current { get; init; } = null!;
}

