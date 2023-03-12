using Assets.PlayerBuilder;
using Assets.PlayerClass;

namespace Assets.PlayerRace;

internal class HalfOrc : IRaceModifiers, IStartingCharacteristics
{
    public string Race => "Half-Orc";
    public int Strength => 2;
    public int Intelligence => -1;
    public int Wisdom => 0;
    public int Dexterity => 0;
    public int Constitution => 1;
    public int Charisma => -4;
    public PlayerClassEnum Class => PlayerClassEnum.Warrior | PlayerClassEnum.Priest | PlayerClassEnum.Rogue;
    public double ExperiencePenalty => 0.10;
    public int Disarm => -3;
    public int Search => 0;
    public int Stealth => -1;
    public int Perception => 3;
    public int Fight => 12;
    public int Bows => -5;
    public int Device => -3;
    public (int Start, string Die) Age => (11, "1d4");
    public (int Start, string Die) MaleHeight => (67, string.Empty);
    public (int Start, string Die) MaleWeight => (150, "1d5");
    public (int Start, string Die) FemaleHeight => (63, string.Empty);
    public (int Start, string Die) FemaleWeight => (120, "1d5");
    public int InfraVision => 3;
    public int HitDie => 10;
}