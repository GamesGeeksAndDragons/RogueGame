using System.Text.Json.Serialization;

namespace Assets.PlayerClass;

[Flags]
[JsonConverter(typeof(PlayerClassEnumConverter))]
public enum PlayerClassEnum
{
    None = 0x00,
    Warrior = 0x01,
    Mage = 0x02,
    Priest = 0x04,
    Rogue = 0x08,
    Ranger = 0x10,
    Paladin = 0x20
}