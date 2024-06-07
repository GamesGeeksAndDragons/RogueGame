﻿using Assets.StartingPlayerStatistics;

namespace Assets.PlayerBuilder;

public interface IPlayer
{
    IPlayerRace Race { get; }
    IPlayerClass Class { get;}
    IPlayerStats Maximum { get; }
    IPlayerStats Current { get; }
    IPlayerStats TurnStats { get; }
    PlayerAbilities Abilities { get; }
    PlayerSpells Magic { get; }
}

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
    internal Player(Gender gender, IPlayerRace race, IPlayerClass pClass, IPlayerStats startingStats, PlayerHitPoints hitPoints, int height, int weight)
    {
        Race = race;
        Class = pClass;
        Current = startingStats;
        Maximum = new PlayerStats(startingStats);
        TurnStats = new PlayerStats(startingStats);
        HitPoints = hitPoints;

        Gender = gender;
        Height = height;
        Weight = weight;

        Abilities.SeeInfra = race.InfraVision;
    }

    public int Height { get; }
    public int Weight { get; }
    public Gender Gender { get; }

    public IPlayerRace Race { get; }
    public IPlayerClass Class { get; }
    public IPlayerStats Maximum { get; }
    public IPlayerStats Current { get; }
    public IPlayerStats TurnStats { get; private set; }

    public PlayerHitPoints HitPoints { get; }

    public PlayerAbilities Abilities { get; } = new PlayerAbilities();
    public PlayerSpells Magic { get; } = new PlayerSpells();

    public void BeginTurn()
    {
    }
}
