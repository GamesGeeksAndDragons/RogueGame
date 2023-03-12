using Assets.PlayerBuilder;
using Assets.PlayerClass;

namespace Assets.PlayerRace;

internal class Elf : IRaceModifiers, IStartingCharacteristics
{
    public string Race => "Elf";
    public int Strength => -1;
    public int Intelligence => 2;
    public int Wisdom => 1;
    public int Dexterity => 1;
    public int Constitution => -2;
    public int Charisma => 1;
    public PlayerClassEnum Class => PlayerClassEnum.Warrior | PlayerClassEnum.Mage | PlayerClassEnum.Priest | PlayerClassEnum.Ranger | PlayerClassEnum.Rogue;
    public double ExperiencePenalty => 0.20;
    public int Disarm => 5;
    public int Search => 8;
    public int Stealth => 1;
    public int Perception => -1;
    public int Fight => -1;
    public int Bows => 15;
    public int Device => 6;
    public (int Start, string Die) Age => (75, "1d75");
    public (int Start, string Die) MaleHeight => (60, "1d4");
    public (int Start, string Die) MaleWeight => (100, "1d6");
    public (int Start, string Die) FemaleHeight => (54, "1d4");
    public (int Start, string Die) FemaleWeight => (80, "1d6");
    public int InfraVision => 3;
    public int HitDie => 8;
}
