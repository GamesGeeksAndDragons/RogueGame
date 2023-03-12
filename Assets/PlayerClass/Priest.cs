using Assets.PlayerBuilder;

namespace Assets.PlayerClass;

// https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/data_player.cpp

internal class Priest : PlayerClassBase
{
    protected new static readonly List<string> GetTitles = new()
    {
        "Believer",     "Acolyte(1st)", "Acolyte(2nd)", "Acolyte(3rd)",
        "Adept (1st)",  "Adept (2nd)",  "Adept (3rd)",  "Priest (1st)",
        "Priest (2nd)", "Priest (3rd)", "Priest (4th)", "Priest (5th)",
        "Priest (6th)", "Priest (7th)", "Priest (8th)", "Priest (9th)",
        "Curate (1st)", "Curate (2nd)", "Curate (3rd)", "Curate (4th)",
        "Curate (5th)", "Curate (6th)", "Curate (7th)", "Curate (8th)",
        "Curate (9th)", "Canon (1st)",  "Canon (2nd)",  "Canon (3rd)",
        "Canon (4th)",  "Canon (5th)",  "Low Lama",     "Lama-1",
        "Lama-2",       "Lama-3",       "High Lama",    "Great Lama",
        "Patriarch",    "High Priest",  "Great Priest", "Noble Priest"
    };

    public override string Class => "Priest";
    public override int Strength => -3;
    public override int Intelligence => -3;
    public override int Wisdom => 3;
    public override int Dexterity => -1;
    public override int Constitution => 0;
    public override int Charisma => 2;
    public override SpellsEnum Spells => SpellsEnum.Priest;
    public override double ExperiencePenalty => 0.20;
    public override int Disarm => 25;
    public override int Search => 16;
    public override int Stealth => 2;
    public override int Perception => 32;
    public override int Fight => 48;
    public override int Bows => 35;
    public override int Save => 30;
    public override int HitDie => 2;
    public override (int Fight, int Bows, int Device, int Disarm, int Save) LevelIncrement => (2, 2, 4, 3, 3);
}