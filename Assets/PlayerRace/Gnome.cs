using Assets.PlayerBuilder;
using Assets.PlayerClass;

namespace Assets.PlayerRace;

internal class Gnome : IRaceModifiers, IStartingCharacteristics
{
    public string Race => "Gnome";
    public int Strength => -1;
    public int Intelligence => 2;
    public int Wisdom => 0;
    public int Dexterity => 2;
    public int Constitution => 1;
    public int Charisma => -2;
    public PlayerClassEnum Class => PlayerClassEnum.Warrior | PlayerClassEnum.Mage | PlayerClassEnum.Priest | PlayerClassEnum.Rogue;
    public double ExperiencePenalty => 0.25;
    public int Disarm => 10;
    public int Search => 6;
    public int Stealth => 3;
    public int Perception => -3;
    public int Fight => -8;
    public int Bows => 12;
    public int Device => 12;
    public (int Start, string Die) Age => (50, "1d40");
    public (int Start, string Die) MaleHeight => (42, "1d3");
    public (int Start, string Die) MaleWeight => (90, "1d6");
    public (int Start, string Die) FemaleHeight => (39, "1d3");
    public (int Start, string Die) FemaleWeight => (50, "1d3");
    public int InfraVision => 4;
    public int HitDie => 7;
}