using System.Collections.Immutable;
using Assets.PlayerHelpers;
using Utils.Random;

namespace Assets.PlayerBuilder;

public class PlayerHitPoints
{
    public int HitDie { get; set; }
    public int Current { get; set; }
    public int Maximum { get; set; }

    public ImmutableList<int> BaseLevels { get; }

    public PlayerHitPoints(IPlayerStats stats, int hitDie, IDice die, int[] levels)
    {
        HitDie = hitDie;
        Current = Maximum = stats.CalcHitAdjustmentConstitution() + hitDie;
        BaseLevels = levels.ToImmutableList();
    }
}