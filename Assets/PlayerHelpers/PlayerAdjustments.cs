using Assets.PlayerBuilder;

namespace Assets.PlayerHelpers;

internal static class PlayerAdjustments
{
    // https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/player_stats.cpp#L479
    // playerDamageAdjustment
    public static int CalcDamageAdjustment(this IPlayerStats stats)
    {
        return stats.Strength switch
        {
            < 4 => -2,
            < 5 => -1,
            < 16 => 0,
            < 17 => 1,
            < 18 => 2,
            < 94 => 3,
            < 109 => 4,
            < 117 => 5,
            _ => 6
        };
    }

    // https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/player_stats.cpp#L444
    // playerDisarmAdjustment
    public static int CalcDisarmAdjustment(this IPlayerStats stats)
    {
        return stats.Dexterity switch
        {
            < 4 => -8,
            4 => -6,
            5 => -4,
            6 => -2,
            7 => -1,
            < 13 => 0,
            < 16 => 1,
            < 18 => 2,
            < 59 => 4,
            < 94 => 5,
            < 117 => 6,
            _ => 8
        };
    }

    // https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/player_stats.cpp#L413
    // playerArmorClassAdjustment
    public static int CalcArmorClassAdjustment(this IPlayerStats stats)
    {
        return stats.Dexterity switch
        {
            < 4 => -4,
            4 => -3,
            5 => -2,
            6 => -1,
            < 15 => 0,
            < 18 => 1,
            < 59 => 2,
            < 94 => 3,
            < 117 => 4,
            _ => 5
        };
    }

    // https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/player_stats.cpp#L366
    // playerToHitAdjustment
    public static int CalcToHitAdjustment(this IPlayerStats stats)
    {
        int bonus = CalculateDexterityBonus() + CalculateStrengthBonus();


        return bonus;

        int CalculateDexterityBonus()
        {
            return stats.Dexterity switch
            {
                < 4 => -3,
                < 6 => -2,
                < 8 => -1,
                < 16 => 0,
                < 17 => 1,
                < 18 => 2,
                < 69 => 3,
                < 118 => 4,
                _ => 5
            };
        }

        int CalculateStrengthBonus()
        {
            return stats.Strength switch
            {
                < 4 => -3,
                < 5 => -2,
                < 7 => -1,
                < 18 => 0,
                < 94 => 1,
                < 109 => 2,
                < 117 => 3,
                _ => 4
            };
        }
    }

    // https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/player_stats.cpp#L206
    // playerStatAdjustmentConstitution

    public static int CalcHitAdjustmentConstitution(this IPlayerStats stats)
    {
        return stats.Constitution switch
        {
            < 7 => (stats.Constitution - 7),
            < 17 => 0,
            17 => 1,
            < 94 => 2,
            < 117 => 3,
            _ => 4
        };
    }

    // https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/player_stats.cpp#L261
    internal static PlayerStats ApplyAdjustments(this PlayerBuilder.Player player)
    {
        var adjusted = new PlayerStats(player.Current);

        //recalculateBonuses.

        return adjusted;
    }

}