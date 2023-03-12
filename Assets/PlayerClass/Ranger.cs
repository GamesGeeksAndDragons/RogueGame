using Assets.PlayerBuilder;

namespace Assets.PlayerClass;

// https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/data_player.cpp

internal class Ranger : PlayerClassBase
{
    protected new static readonly List<string> GetTitles = new()
    {
        "Runner (1st)",  "Runner (2nd)",  "Runner (3rd)",  "Strider (1st)",
        "Strider (2nd)", "Strider (3rd)", "Scout (1st)",   "Scout (2nd)",
        "Scout (3rd)",   "Scout (4th)",   "Scout (5th)",   "Courser (1st)",
        "Courser (2nd)", "Courser (3rd)", "Courser (4th)", "Courser (5th)",
        "Tracker (1st)", "Tracker (2nd)", "Tracker (3rd)", "Tracker (4th)",
        "Tracker (5th)", "Tracker (6th)", "Tracker (7th)", "Tracker (8th)",
        "Tracker (9th)", "Guide (1st)",   "Guide (2nd)",   "Guide (3rd)",
        "Guide (4th)",   "Guide (5th)",   "Guide (6th)",   "Guide (7th)",
        "Guide (8th)",   "Guide (9th)",   "Pathfinder-1",  "Pathfinder-2",
        "Pathfinder-3",  "Ranger",        "High Ranger",   "Ranger Lord"
    };

    public override string Class => "Ranger";
    public override int Strength => 2;
    public override int Intelligence => 2;
    public override int Wisdom => 0;
    public override int Dexterity => 1;
    public override int Constitution => 1;
    public override int Charisma => 1;
    public override SpellsEnum Spells => SpellsEnum.Mage;
    public override double ExperiencePenalty => 0.40;
    public override int Disarm => 30;
    public override int Search => 24;
    public override int Stealth => 3;
    public override int Perception => 24;
    public override int Fight => 56;
    public override int Bows => 72;
    public override int Save => 30;
    public override int HitDie => 4;
    public override (int Fight, int Bows, int Device, int Disarm, int Save) LevelIncrement => (3, 4, 3, 3, 3);
}