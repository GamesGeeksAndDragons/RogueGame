using Assets.StartingPlayerStatistics;

namespace Assets.PlayerBuilder;

public interface IPlayer
{
    PlayerRace Race { get; }
    PlayerClass Class { get;}
    PlayerStats Maximum { get; }
    PlayerStats Current { get; }
    PlayerStats TurnStats { get; }

    PlayerTurn Turn { get; }
    PlayerAbilities Abilities { get; }
    PlayerSpells Magic { get; }
}

public enum Gender
{
    Male,
    Female
};

// https://github.com/jhirschberg70/browser-based-umoria/blob/f9fcf9ce217922be4941c7397007f5635ff2f838/src/player.h#L60
// flags broken into PlayerTurn, PlayerAbilities and PlayerSpells

/*
 * In Moria, it looks like they have several copies of the stats
 *  Maximum: a copy of the largest stats achieved so they can be used to restore to if a drain etc happens
 *  Current: the current values, when stats increase both Current and Maximum are updated. When decreased only Current
 *  Used: the values going to be used for a turn
 *  Modified: the values at the end of a turn, also includes the stats increased by wearing something
 * There are also copies used for display purposes
 *
 * I'm going to try to rationalise these copies so will only use Maximum and Current and TurnStats
 * TurnStats will take Current and apply all bonuses/adjustments
 */

internal class Player : IPlayer
{
    public const int MaxLevel = 40;
    internal Player(Gender gender, PlayerRace race, PlayerClass pClass, PlayerStats startingStats, PlayerHitPoints hitPoints, int height, int weight)
    {
        Race = race;
        Class = pClass;
        Current = startingStats;
        Maximum = CloneCurrent();
        TurnStats = CloneCurrent();
        HitPoints = hitPoints;

        Gender = gender;
        Height = height;
        Weight = weight;
        Turn.SeeInfra = race.InfraVision;
    }

    PlayerStats CloneCurrent()
    {
        return new PlayerStats
        {
            Strength = Current.Strength,
            Intelligence = Current.Intelligence,
            Wisdom = Current.Wisdom,
            Dexterity = Current.Dexterity,
            Constitution = Current.Constitution,
            Charisma = Current.Charisma
        };
    }

    public int Height { get; }
    public int Weight { get; }
    public Gender Gender { get; }

    public PlayerRace Race { get; }
    public PlayerClass Class { get; }
    public PlayerStats Maximum { get; }
    public PlayerStats Current { get; }
    public PlayerStats TurnStats { get; private set; }

    public PlayerHitPoints HitPoints { get; }

    public PlayerTurn Turn { get; } = new PlayerTurn();
    public PlayerAbilities Abilities { get; } = new PlayerAbilities();
    public PlayerSpells Magic { get; } = new PlayerSpells();

    public void BeginTurn()
    {
        TurnStats = CloneCurrent();
    }
}
