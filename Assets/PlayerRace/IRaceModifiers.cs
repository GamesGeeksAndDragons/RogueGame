using Assets.PlayerClass;

namespace Assets.PlayerRace;

// https://beej.us/moria/mmspoilers/character.html#races

internal interface IRaceModifiers
{
    string Race { get; }
    int Strength { get; }
    int Intelligence { get; }
    int Wisdom { get; }
    int Dexterity { get; }
    int Constitution { get; }
    int Charisma { get; }
    PlayerClassEnum Class { get; }
    double ExperiencePenalty { get; }
    int Disarm { get; }
    int Search { get; }
    int Stealth { get; }
    int Perception { get; } // Lower perception is better
    int Fight { get; }
    int Bows { get; }
    int Device { get; }
    int InfraVision { get; }
    int HitDie { get; }
}
