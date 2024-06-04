using Assets.PlayerBuilder;
using Assets.StartingPlayerStatistics;

namespace Assets.PlayerClass;

internal interface IPlayerClass : IClassModifiers
{
    string Title { get; }
    int Level { get; }
}

internal abstract class PlayerClassBase : IPlayerClass
{
    protected static Dictionary<int, string> Titles = new();

    protected static List<string> GetTitles()
    {
        return new List<string>();
    }

    protected PlayerClassBase()
    {
        var level = Level = 0;
        var titles = GetTitles();
        titles.ForEach(title => Titles.Add(level++, title));
    }

    public string Title => Titles[Level];
    public int Level { get; protected set; }
    public abstract string Class { get; }
    public abstract int Strength { get; }
    public abstract int Intelligence { get; }
    public abstract int Wisdom { get; }
    public abstract int Dexterity { get; }
    public abstract int Constitution { get; }
    public abstract int Charisma { get; }
    public abstract SpellsEnum Spells { get; }
    public abstract double ExperiencePenalty { get; }
    public abstract int Disarm { get; }
    public abstract int Search { get; }
    public abstract int Stealth { get; }
    public abstract int Perception { get; }
    public abstract int Fight { get; }
    public abstract int Bows { get; }
    public abstract int Save { get; }
    public abstract int HitDie { get; }
    public abstract (int Fight, int Bows, int Device, int Disarm, int Save) LevelIncrement { get; }
}