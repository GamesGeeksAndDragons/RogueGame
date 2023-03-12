namespace Assets.PlayerClass;

[Flags]
internal enum PlayerClassEnum
{
    Warrior = 0x00,
    Mage = 0x01,
    Priest = 0x02,
    Rogue = 0x04,
    Ranger = 0x08,
    Paladin = 0x10
}