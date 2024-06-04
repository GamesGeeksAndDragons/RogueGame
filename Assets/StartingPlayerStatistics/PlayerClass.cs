using Assets.PlayerBuilder;
using Assets.PlayerClass;

namespace Assets.StartingPlayerStatistics;

// https://beej.us/moria/mmspoilers/character.html#races

//public class PlayerClass
//{
//    public string Name { get; set; } = null!;
//    public int Strength { get; set; }
//    public int Intelligence { get; set; }
//    public int Wisdom { get; set; }
//    public int Dexterity { get; set; }
//    public int Constitution { get; set; }
//    public int Charisma { get; set; }
//    public double ExperiencePenalty { get; set; }
//    public int HitDie { get; set; }
//    public int Disarm { get; set; }
//    public int Search { get; set; }
//    public int Stealth { get; set; }
//    public int Perception { get; set; }
//    public int Fight { get; set; }
//    public int Bows { get; set; }
//    public int Save { get; set; }

//    public SpellsEnum Spells { get; set; }

//    public LevelIncrement LevelIncrement { get; set; } = null!;

//    public string[] Titles { get; set; } = null!;
//}

public record PlayerClass
{
    public string Name { get; init; } = null!;
    public int Strength { get; init; }
    public int Intelligence { get; init; }
    public int Wisdom { get; init; }
    public int Dexterity { get; init; }
    public int Constitution { get; init; }
    public int Charisma { get; init; }
    public double ExperiencePenalty { get; init; }
    public int HitDie { get; init; }
    public int Disarm { get; init; }
    public int Search { get; init; }
    public int Stealth { get; init; }
    public int Perception { get; init; }
    public int Fight { get; init; }
    public int Bows { get; init; }
    public int Save { get; init; }

    public SpellsEnum Spells { get; init; }

    public LevelIncrement LevelIncrement { get; init; } = null!;

    public string[] Titles { get; init; } = null!;
}