namespace Assets.PlayerBuilder;

// https://github.com/jhirschberg70/browser-based-umoria/blob/f9fcf9ce217922be4941c7397007f5635ff2f838/src/player.h#L100
// taken from flags in an attempt to give meaning

public class PlayerAbilities
{
    public bool CanSeeInvisible;       // Can see invisible
    public bool CanTeleport;            // Random teleportation
    public bool HasFreeAction;         // Never paralyzed
    public bool HasSlowDigestion;         // Lower food needs
    public bool CanAggravateMonsters;           // Aggravate monsters
    public bool IsResistantToFire;   // Resistance to fire
    public bool IsResistantToCold;   // Resistance to cold
    public bool IsResistantToAcid;   // Resistance to acid
    public bool IsRegenerateHp;       // Regenerate hit pts
    public bool IsResistantToLight;  // Resistance to light
    public bool CanFreeFall;           // No damage falling
    public bool CanSustainStr;         // Keep strength
    public bool CanSustainInt;         // Keep intelligence
    public bool CanSustainWis;         // Keep wisdom
    public bool CanSustainCon;         // Keep constitution
    public bool CanSustainDex;         // Keep dexterity
    public bool CanSustainChr;         // Keep charisma
    public bool CanConfuseMonster;     // Glowing hands.
}