using Assets.PlayerBuilder;
using Assets.PlayerClass;

namespace Assets.PlayerRace;

internal class Dwarf : IRaceModifiers, IStartingCharacteristics
{
    public string Race => "Dwarf";
    public int Strength => 2;
    public int Intelligence => -3;
    public int Wisdom => 1;
    public int Dexterity => -2;
    public int Constitution => 2;
    public int Charisma => -3;
    public PlayerClassEnum Class => PlayerClassEnum.Warrior | PlayerClassEnum.Priest;
    public double ExperiencePenalty => 0.20;
    public int Disarm => 2;
    public int Search => 7;
    public int Stealth => -1;
    public int Perception => 0;
    public int Fight => 15;
    public int Bows => 0;
    public int Device => 9;
    public (int Start, string Die) Age => (35, "1d15");
    public (int Start, string Die) MaleHeight => (48, "1d3");
    public (int Start, string Die) MaleWeight => (150, "1d10");
    public (int Start, string Die) FemaleHeight => (46, "1d3");
    public (int Start, string Die) FemaleWeight => (120, "1d10");
    public int InfraVision => 5;
    public int HitDie => 9;
}