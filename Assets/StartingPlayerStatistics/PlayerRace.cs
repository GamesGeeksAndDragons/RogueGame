using Assets.PlayerClass;

namespace Assets.StartingPlayerStatistics;

// https://beej.us/moria/mmspoilers/character.html#races

public class PlayerRace
{
    public string Name { get; set; } = null!;
    public int Strength { get; set; }
    public int Intelligence { get; set; }
    public int Wisdom { get; set; }
    public int Dexterity { get; set; }
    public int Constitution { get; set; }
    public int Charisma { get; set; }
    public PlayerClassEnum PlayerClasses { get; set; }
    public double ExperiencePenalty { get; set; }
    public int Disarm { get; set; }
    public int Search { get; set; }
    public int Stealth { get; set; }
    public int Perception { get; set; }
    public int Fight { get; set; }
    public int Bows { get; set; }
    public int Device { get; set; }
    public int InfraVision { get; set; }
    public int HitDie => 10;

    public StartingStatistic Age { get; set; } = null!;
    public StartingStatistic MaleHeight { get; set; } = null!;
    public StartingStatistic MaleWeight { get; set; } = null!;
    public StartingStatistic FemaleHeight { get; set; } = null!;
    public StartingStatistic FemaleWeight { get; set; } = null!;
}
