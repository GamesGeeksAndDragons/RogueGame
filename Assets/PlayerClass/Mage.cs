using Assets.PlayerBuilder;

namespace Assets.PlayerClass;

// https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/data_player.cpp

internal class Mage : PlayerClassBase
{
    protected new static readonly List<string> GetTitles = new()
    {
        "Novice",       "Apprentice",   "Trickster-1",  "Trickster-2",
        "Trickster-3",  "Cabalist-1",   "Cabalist-2",   "Cabalist-3",
        "Visionist",    "Phantasmist",  "Shadowist",    "Spellbinder",
        "Illusionist",  "Evoker (1st)", "Evoker (2nd)", "Evoker (3rd)",
        "Evoker (4th)", "Conjurer",     "Theurgist",    "Thaumaturge",
        "Magician",     "Enchanter",    "Warlock",      "Sorcerer",
        "Necromancer",  "Mage (1st)",   "Mage (2nd)",   "Mage (3rd)",
        "Mage (4th)",   "Mage (5th)",   "Wizard (1st)", "Wizard (2nd)",
        "Wizard (3rd)", "Wizard (4th)", "Wizard (5th)", "Wizard (6th)",
        "Wizard (7th)", "Wizard (8th)", "Wizard (9th)", "Wizard Lord"
    };

    public override string Class => "Mage";
    public override int Strength => -5;
    public override int Intelligence => 3;
    public override int Wisdom => 0;
    public override int Dexterity => 1;
    public override int Constitution => -2;
    public override int Charisma => 1;
    public override SpellsEnum Spells => SpellsEnum.Mage;
    public override double ExperiencePenalty => 0.30;
    public override int Disarm => 30;
    public override int Search => 16;
    public override int Stealth => 2;
    public override int Perception => 20;
    public override int Fight => 34;
    public override int Bows => 20;
    public override int Save => 36;
    public override int HitDie => 0;
    public override (int Fight, int Bows, int Device, int Disarm, int Save) LevelIncrement => (2, 2, 4, 3, 3);
}