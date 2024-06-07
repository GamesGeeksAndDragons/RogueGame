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

    public long Status;          // Status of player
    public int Rest;             // Rest counter
    public int Blind;            // Blindness counter
    public int Paralysis;        // Paralysis counter
    public int Confused;         // Confusion counter
    public int QuantityOfFood;   // Food counter
    public int FoodPerTurn;        // Food per round
    public int ProtectionFromEvil;       // Protection fr. evil
    public int CurrentSeed;            // Cur speed adjust
    public int Fast;             // Temp speed change
    public int Slow;             // Temp speed change
    public int Afraid;           // Fear
    public int Poisoned;         // Poisoned
    public int Hallucinate;            // Hallucinate / image
    public int ProtectEvil;     // Protect VS evil
    public int Invulnerability;  // Increases AC
    public int Heroism;          // Heroism
    public int SuperHeroism;    // Super Heroism
    public int Blessed;          // Blessed
    public int HeatResistance;  // Timed heat resist
    public int ColdResistance;  // Timed cold resist
    public int DetectInvisible; // Timed see invisible
    public int WordOfRecall;   // Timed teleport level
    public int SeeInfra;        // See warm creatures
    public int TimedInfra;      // Timed infra vision    
}