namespace Assets.PlayerBuilder;

public interface IPlayerStats
{
    string Title { get; set; }
    int Strength { get; set; }
    int Intelligence { get; set; }
    int Wisdom { get; set; }
    int Dexterity { get; set; }
    int Constitution { get; set; }
    int Charisma { get; set; }
    int ExperiencePenalty { get; set; }
    int HitDie { get; set; }
    int Disarm { get; set; }
    int Search { get; set; }
    int Stealth { get; set; }
    int SearchFrequency { get; set; }
    int Perception { get; set; }
    int Fight { get; set; }
    int Bows { get; set; }
    int Saving { get; set; }
}

internal class PlayerStats : IPlayerStats
{
    internal PlayerStats()
    {
    }

    internal PlayerStats(IPlayerStats stats)
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