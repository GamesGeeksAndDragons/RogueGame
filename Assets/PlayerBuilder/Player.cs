using Assets.StartingPlayerStatistics;

namespace Assets.PlayerBuilder;

public interface IPlayer
{
    PlayerRace Race { get; }
    PlayerClass Class { get;}
    PlayerStats Maximum { get; }
    PlayerStats Current { get; }
}

public enum Gender
{
    Male,
    Female
};

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
}
