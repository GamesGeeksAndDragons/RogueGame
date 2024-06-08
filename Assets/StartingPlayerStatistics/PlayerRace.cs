using Assets.PlayerBuilder;

namespace Assets.StartingPlayerStatistics;

// https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/character.h#L31
// https://github.com/jhirschberg70/browser-based-umoria/blob/f9fcf9ce217922be4941c7397007f5635ff2f838/src/data_player.cpp#L81

internal class PlayerRace : PlayerStats
{
    public int Device { get; set; }
    public int InfraVision { get; set; }
    public int Experience { get; set; }

    public PlayerClassEnum PlayerClasses { get; set; }

    public StartingStatistic Age { get; set; } = null!;
    public StartingStatistic MaleHeight { get; set; } = null!;
    public StartingStatistic MaleWeight { get; set; } = null!;
    public StartingStatistic FemaleHeight { get; set; } = null!;
    public StartingStatistic FemaleWeight { get; set; } = null!;
}
