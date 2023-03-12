using Assets.PlayerBuilder;
using Assets.PlayerClass;

namespace Assets.PlayerRace;

internal class Human : IRaceModifiers, IStartingCharacteristics
{
    public string Race => "Human";
    public int Strength => 0;
    public int Intelligence => 0;
    public int Wisdom => 0;
    public int Dexterity => 0;
    public int Constitution => 0;
    public int Charisma => 0;
    public PlayerClassEnum Class => PlayerClassEnum.Warrior | PlayerClassEnum.Mage | PlayerClassEnum.Priest | PlayerClassEnum.Ranger | PlayerClassEnum.Rogue | PlayerClassEnum.Paladin;
    public double ExperiencePenalty => 0.00;
    public int Disarm => 0;
    public int Search => 0;
    public int Stealth => 0;
    public int Perception => 0;
    public int Fight => 0;
    public int Bows => 0;
    public int Device => 0;
    public (int Start, string Die) Age => (14, "1d6");
    public (int Start, string Die) MaleHeight => (72, "1d6");
    public (int Start, string Die) MaleWeight => (180, "1d25");
    public (int Start, string Die) FemaleHeight => (66, "1d4");
    public (int Start, string Die) FemaleWeight => (150, "1d20");
    public int InfraVision => 0;
    public int HitDie => 10;
}