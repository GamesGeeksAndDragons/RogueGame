using Assets.PlayerBuilder;

namespace Assets.PlayerClass;

// https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/data_player.cpp

internal class Warrior : PlayerClassBase
{
    protected new static readonly List<string> GetTitles = new()
    {
        "Rookie",       "Private",      "Soldier",      "Mercenary",
        "Veteran(1st)", "Veteran(2nd)", "Veteran(3rd)", "Warrior(1st)",
        "Warrior(2nd)", "Warrior(3rd)", "Warrior(4th)", "Swordsman-1",
        "Swordsman-2",  "Swordsman-3",  "Hero",         "Swashbuckler",
        "Myrmidon",     "Champion-1",   "Champion-2",   "Champion-3",
        "Superhero",    "Knight",       "Superior Knt", "Gallant Knt",
        "Knt Errant",   "Guardian Knt", "Baron",        "Duke",
        "Lord (1st)",   "Lord (2nd)",   "Lord (3rd)",   "Lord (4th)",
        "Lord (5th)",   "Lord (6th)",   "Lord (7th)",   "Lord (8th)",
        "Lord (9th)",   "Lord Gallant", "Lord Keeper",  "Lord Noble"
    };

    internal Warrior()
    {
    }

    public override string Class => "Warrior";
    public override int Strength => 5;
    public override int Intelligence => -2;
    public override int Wisdom => -2;
    public override int Dexterity => 2;
    public override int Constitution => 2;
    public override int Charisma => -1;
    public override SpellsEnum Spells => SpellsEnum.None;
    public override double ExperiencePenalty => 0.00;
    public override int Disarm => 25;
    public override int Search => 14;
    public override int Stealth => 1;
    public override int Perception => 38;
    public override int Fight => 70;
    public override int Bows => 55;
    public override int Save => 18;
    public override int HitDie => 9;
    public override (int Fight, int Bows, int Device, int Disarm, int Save) LevelIncrement => (4, 4, 2, 2, 3);
}