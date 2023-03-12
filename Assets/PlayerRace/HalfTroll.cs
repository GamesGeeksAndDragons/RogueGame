using Assets.PlayerBuilder;
using Assets.PlayerClass;

namespace Assets.PlayerRace;

internal class HalfTroll : IRaceModifiers, IStartingCharacteristics
{
    public string Race => "Half-Troll";
    public int Strength => 4;
    public int Intelligence => -4;
    public int Wisdom => -2;
    public int Dexterity => -4;
    public int Constitution => 3;
    public int Charisma => -6;
    public PlayerClassEnum Class => PlayerClassEnum.Warrior | PlayerClassEnum.Priest;
    public double ExperiencePenalty => 0.20;
    public int Disarm => -5;
    public int Search => -1;
    public int Stealth => -2;
    public int Perception => 5;
    public int Fight => 20;
    public int Bows => -10;
    public int Device => -8;
    public (int Start, string Die) Age => (20, "1d10");
    public (int Start, string Die) MaleHeight => (96, "1d10");
    public (int Start, string Die) MaleWeight => (255, "1d50");
    public (int Start, string Die) FemaleHeight => (84, "1d8");
    public (int Start, string Die) FemaleWeight => (225, "1d40");
    public int InfraVision => 3;
    public int HitDie => 12;
}

