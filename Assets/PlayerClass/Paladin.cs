using Assets.PlayerBuilder;

namespace Assets.PlayerClass;

// https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/data_player.cpp

internal class Paladin : PlayerClassBase
{
    protected new static readonly List<string> GetTitles = new()
    {
        "Gallant",      "Keeper (1st)", "Keeper (2nd)", "Keeper (3rd)",
        "Keeper (4th)", "Keeper (5th)", "Keeper (6th)", "Keeper (7th)",
        "Keeper (8th)", "Keeper (9th)", "Protector-1",  "Protector-2",
        "Protector-3",  "Protector-4",  "Protector-5",  "Protector-6",
        "Protector-7",  "Protector-8",  "Defender-1",   "Defender-2",
        "Defender-3",   "Defender-4",   "Defender-5",   "Defender-6",
        "Defender-7",   "Defender-8",   "Warder (1st)", "Warder (2nd)",
        "Warder (3rd)", "Warder (4th)", "Warder (5th)", "Warder (6th)",
        "Warder (7th)", "Warder (8th)", "Warder (9th)", "Guardian",
        "Chevalier",    "Justiciar",    "Paladin",      "High Lord"
    };

    public override string Class => "Paladin";
    public override int Strength => 3;
    public override int Intelligence => -3;
    public override int Wisdom => 1;
    public override int Dexterity => 0;
    public override int Constitution => 2;
    public override int Charisma => 2;
    public override SpellsEnum Spells => SpellsEnum.Priest;
    public override double ExperiencePenalty => 0.35;
    public override int Disarm => 20;
    public override int Search => 12;
    public override int Stealth => 1;
    public override int Perception => 38;
    public override int Fight => 68;
    public override int Bows => 40;
    public override int Save => 24;
    public override int HitDie => 6;
    public override (int Fight, int Bows, int Device, int Disarm, int Save) LevelIncrement => (3, 3, 3, 2, 3);
}