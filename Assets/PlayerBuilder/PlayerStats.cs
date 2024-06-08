namespace Assets.PlayerBuilder;

internal class PlayerStats
{
    internal PlayerStats()
    {
    }

    internal PlayerStats(PlayerStats stats)
    {
        Title = stats.Title;
        Strength = stats.Strength;
        Intelligence = stats.Intelligence;
        Wisdom = stats.Wisdom;
        Dexterity = stats.Dexterity;
        Constitution = stats.Constitution;
        Charisma = stats.Charisma;

        ExperiencePenalty = stats.ExperiencePenalty;
        HitDie = stats.HitDie;
        Disarm = stats.Disarm;
        Search = stats.Search;
        Stealth = stats.Stealth;
        SearchFrequency = stats.SearchFrequency;
        Perception = stats.Perception;
        Fight = stats.Fight;
        Bows = stats.Bows;
        Saving = stats.Saving;
    }

    public string Title { get; set; } = null!;

    public int Strength { get; set; }
    public int Intelligence { get; set; }
    public int Wisdom { get; set; }
    public int Dexterity { get; set; }
    public int Constitution { get; set; }
    public int Charisma { get; set; }

    public int ExperiencePenalty { get; set; }
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