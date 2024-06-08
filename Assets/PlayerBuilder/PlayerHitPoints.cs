using System.Collections.Immutable;
using Assets.PlayerHelpers;
using Utils.Random;

namespace Assets.PlayerBuilder;

internal class PlayerHitPoints
{
    public int HitDie { get; set; }
    public int Current { get; set; }
    public int Maximum { get; set; }

    public ImmutableList<int> BaseLevels { get; }

    internal PlayerHitPoints(PlayerStats stats, int hitDie, int[] levels)
    {
        HitDie = hitDie;
        Current = Maximum = stats.CalcHitAdjustmentConstitution() + hitDie;
        BaseLevels = levels.ToImmutableList();
    }
}