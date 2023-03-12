using Assets.PlayerBuilder;
using Assets.PlayerClass;

namespace Assets.PlayerRace;

internal class Halfling : IRaceModifiers, IStartingCharacteristics
{
    public string Race => "Halfling";
    public int Strength => -2;
    public int Intelligence => 2;
    public int Wisdom => 1;
    public int Dexterity => 3;
    public int Constitution => 1;
    public int Charisma => 1;
    public PlayerClassEnum Class => PlayerClassEnum.Warrior | PlayerClassEnum.Mage | PlayerClassEnum.Rogue;
    public double ExperiencePenalty => 0.10;
    public int Disarm => 15;
    public int Search => 12;
    public int Stealth => 4;
    public int Perception => -5;
    public int Fight => -10;
    public int Bows => 20;
    public int Device => 6;
    public (int Start, string Die) Age => (21, "1d12");
    public (int Start, string Die) MaleHeight => (36, "1d3");
    public (int Start, string Die) MaleWeight => (60, "1d3");
    public (int Start, string Die) FemaleHeight => (33, "1d3");
    public (int Start, string Die) FemaleWeight => (50, "1d3");
    public int InfraVision => 4;
    public int HitDie => 6;
}