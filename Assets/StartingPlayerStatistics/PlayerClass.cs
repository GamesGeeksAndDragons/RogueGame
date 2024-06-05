using Assets.PlayerBuilder;

namespace Assets.StartingPlayerStatistics;

// https://beej.us/moria/mmspoilers/character.html#races

public class PlayerClass : PlayerStats
{
    public string Name { get; set; } = null!;
    public double ExperiencePenalty { get; set; }
    public int HitDie { get; set; }
    public int Disarm { get; set; }
    public int Search { get; set; }
    public int Stealth { get; set; }
    public int Perception { get; set; }
    public int Fight { get; set; }
    public int Bows { get; set; }
    public int Save { get; set; }

    public SpellsEnum Spells { get; set; }

    public LevelIncrement LevelIncrement { get; set; } = null!;

    public string[] Titles { get; set; } = null!;
}