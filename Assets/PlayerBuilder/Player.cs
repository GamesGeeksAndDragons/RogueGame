using Assets.StartingPlayerStatistics;

namespace Assets.PlayerBuilder;

public interface IPlayer
{
    PlayerRace Race { get; }
    PlayerClass Class { get;}
    PlayerStats Maximum { get; }
    PlayerStats Current { get; }
    PlayerStats Used { get; }

    PlayerTurn Turn { get; }
    PlayerAbilities Abilities { get; }
    PlayerMagic Magic { get; }
}

public enum Gender
{
    Male,
    Female
};

// https://github.com/jhirschberg70/browser-based-umoria/blob/f9fcf9ce217922be4941c7397007f5635ff2f838/src/player.h#L60
// flags broken into PlayerTurn, PlayerAbilities and PlayerMagic

internal class Player : IPlayer
{
    internal Player(Gender gender, PlayerRace race, PlayerClass pClass, PlayerStats maxStats, int height, int weight)
    {
        Race = race;
        Class = pClass;
        Maximum = maxStats;
        Current = CloneMax();
        Used = CloneMax();
        Gender = gender;
        Height = height;
        Weight = weight;
        Turn.SeeInfra = race.InfraVision;

        PlayerStats CloneMax()
        {
            return new PlayerStats
            {
                Strength = maxStats.Strength,
                Intelligence = maxStats.Intelligence,
                Wisdom = maxStats.Wisdom,
                Dexterity = maxStats.Dexterity,
                Constitution = maxStats.Constitution,
                Charisma = maxStats.Charisma
            };
        }
    }

    public int Height { get; }
    public int Weight { get; }
    public Gender Gender { get; }

    public PlayerRace Race { get; }
    public PlayerClass Class { get; }
    public PlayerStats Maximum { get; }
    public PlayerStats Current { get; }
    public PlayerStats Used { get; }

    public PlayerTurn Turn { get; } = new PlayerTurn();
    public PlayerAbilities Abilities { get; } = new PlayerAbilities();
    public PlayerMagic Magic { get; } = new PlayerMagic();
}
