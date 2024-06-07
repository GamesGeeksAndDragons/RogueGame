namespace Assets.PlayerHelpers;

public static class PlayerStatsHelpers
{
    // https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/player_stats.cpp#L116
    // playerStatAdjustmentWisdomIntelligence
    public static int GetStatAdjustmentForWisdomOrIntelligence(int stat)
    {
        return stat switch
        {
            > 117 => 7,
            > 107 => 6,
            > 87 => 5,
            > 67 => 4,
            > 17 => 3,
            > 14 => 2,
            > 7 => 1,
            _ => 0
        };
    }
}