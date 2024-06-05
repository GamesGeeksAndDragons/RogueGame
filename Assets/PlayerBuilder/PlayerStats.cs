namespace Assets.PlayerBuilder;

public class PlayerStats
{
    public string Title { get; set; } = null!;

    public int Strength { get; set; }
    public int Intelligence { get; set; }
    public int Wisdom { get; set; }
    public int Dexterity { get; set; }
    public int Constitution { get; set; }
    public int Charisma { get; set; }

    public double ExperiencePenalty { get; set; }
    public int HitDie { get; set; }
    public int Disarm { get; set; }
    public int Search { get; set; }
    public int Stealth { get; set; }
    public int SearchFrequency { get; set; }
    public int Perception { get; set; }
    public int Fight { get; set; }
    public int Bows { get; set; }
    public int Saving { get; set; }

}