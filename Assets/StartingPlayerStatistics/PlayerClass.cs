using Assets.PlayerBuilder;

namespace Assets.StartingPlayerStatistics;

// https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/character.h#L60
// https://github.com/jhirschberg70/browser-based-umoria/blob/f9fcf9ce217922be4941c7397007f5635ff2f838/src/data_player.cpp#L262

public class PlayerClass : PlayerStats
{
    public string Name { get; set; } = null!;
    public double ExperiencePenalty { get; set; }
    public int HitDie { get; set; }
    public int Disarm { get; set; }
    public int Search { get; set; }
    public int SearchFrequency { get; set; }
    public int Stealth { get; set; }
    public int Perception { get; set; }
    public int Fight { get; set; }
    public int Bows { get; set; }
    public int Saving { get; set; }

    public SpellsEnum Spells { get; set; }

    public LevelIncrement LevelIncrement { get; set; } = null!;

    public string[] Titles { get; set; } = null!;
}