using Assets.PlayerBuilder;

namespace Assets.StartingPlayerStatistics;

// https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/character.h#L60
// https://github.com/jhirschberg70/browser-based-umoria/blob/f9fcf9ce217922be4941c7397007f5635ff2f838/src/data_player.cpp#L262

public interface IPlayerClass : IPlayerStats
{
    public SpellsEnum Spells { get; set; }
    public int SpellMinLevel { get; set; }

    public LevelIncrement LevelIncrement { get; set; }

    public string[] Titles { get; set; }
}

internal class PlayerClass : PlayerStats, IPlayerClass
{
    public SpellsEnum Spells { get; set; }
    public int SpellMinLevel { get; set; }

    public LevelIncrement LevelIncrement { get; set; } = null!;

    public string[] Titles { get; set; } = null!;
}