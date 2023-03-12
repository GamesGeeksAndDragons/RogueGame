using Assets.PlayerBuilder;

namespace Assets.PlayerClass;

// https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/data_player.cpp

internal class Rogue : PlayerClassBase
{
    protected new static readonly List<string> GetTitles = new()
    {
        "Vagabond",     "Footpad",     "Cutpurse",      "Robber",
        "Burglar",      "Filcher",     "Sharper",       "Magsman",
        "Common Rogue", "Rogue (1st)", "Rogue (2nd)",   "Rogue (3rd)",
        "Rogue (4th)",  "Rogue (5th)", "Rogue (6th)",   "Rogue (7th)",
        "Rogue (8th)",  "Rogue (9th)", "Master Rogue",  "Expert Rogue",
        "Senior Rogue", "Chief Rogue", "Prime Rogue",   "Low Thief",
        "Thief (1st)",  "Thief (2nd)", "Thief (3rd)",   "Thief (4th)",
        "Thief (5th)",  "Thief (6th)", "Thief (7th)",   "Thief (8th)",
        "Thief (9th)",  "High Thief",  "Master Thief",  "Executioner",
        "Low Assassin", "Assassin",    "High Assassin", "Guildsmaster"
    };

    public override string Class => "Rogue";
    public override int Strength => 2;
    public override int Intelligence => 1;
    public override int Wisdom => -2;
    public override int Dexterity => 3;
    public override int Constitution => 1;
    public override int Charisma => 1;
    public override SpellsEnum Spells => SpellsEnum.Mage;
    public override double ExperiencePenalty => 0.40;
    public override int Disarm => 45;
    public override int Search => 32;
    public override int Stealth => 5;
    public override int Perception => 16;
    public override int Fight => 60;
    public override int Bows => 66;
    public override int Save => 30;
    public override int HitDie => 6;
    public override (int Fight, int Bows, int Device, int Disarm, int Save) LevelIncrement => (2, 2, 4, 3, 3);
}

