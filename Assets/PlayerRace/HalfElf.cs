using Assets.PlayerBuilder;
using Assets.PlayerClass;

namespace Assets.PlayerRace;

internal class HalfElf : IRaceModifiers, IStartingCharacteristics
{
    public string Race => "Half-Elf";
    public int Strength => -1;
    public int Intelligence => 1;
    public int Wisdom => 0;
    public int Dexterity => 1;
    public int Constitution => -1;
    public int Charisma => 1;
    public PlayerClassEnum Class => PlayerClassEnum.Warrior | PlayerClassEnum.Mage | PlayerClassEnum.Priest | PlayerClassEnum.Ranger | PlayerClassEnum.Rogue | PlayerClassEnum.Paladin;
    public double ExperiencePenalty => 0.10;
    public int Disarm => 2;
    public int Search => 6;
    public int Stealth => 1;
    public int Perception => -1;
    public int Fight => -1;
    public int Bows => 5;
    public int Device => 3;
    public (int Start, string Die) Age => (24, "1d16");
    public (int Start, string Die) MaleHeight => (66, "1d6");
    public (int Start, string Die) MaleWeight => (130, "1d15");
    public (int Start, string Die) FemaleHeight => (62, "1d6");
    public (int Start, string Die) FemaleWeight => (100, "1d10");
    public int InfraVision => 2;
    public int HitDie => 9;
}