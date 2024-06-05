namespace Assets.PlayerBuilder;

// https://github.com/jhirschberg70/browser-based-umoria/blob/f9fcf9ce217922be4941c7397007f5635ff2f838/src/player.h#L100
// taken from flags in an attempt to give meaning

public class PlayerTurn
{
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