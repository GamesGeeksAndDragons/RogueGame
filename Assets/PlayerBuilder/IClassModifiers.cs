namespace Assets.PlayerBuilder;

public interface IClassModifiers
{
    string Class { get; }
    int Strength { get; }
    int Intelligence { get; }
    int Wisdom { get; }
    int Dexterity { get; }
    int Constitution { get; }
    int Charisma { get; }
    SpellsEnum Spells { get; }
    double ExperiencePenalty { get; }
    int Disarm { get; }
    int Search { get; }
    int Stealth { get; }
    int Perception { get; } // Lower perception is better
    int Fight { get; }
    int Bows { get; }
    int Save { get; }
    int HitDie { get; }
    (int Fight, int Bows, int Device, int Disarm, int Save) LevelIncrement { get; }
}