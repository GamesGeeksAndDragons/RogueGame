using Assets.PlayerHelpers;
using Assets.StartingPlayerStatistics;

namespace Assets.PlayerBuilder;

// https://github.com/jhirschberg70/browser-based-umoria/blob/f9fcf9ce217922be4941c7397007f5635ff2f838/src/player.h#L100
// taken from flags in an attempt to give meaning

internal class PlayerSpells
{
    internal readonly PlayerStats CurrentStats;
    internal readonly PlayerClass PlayerClass;
    private readonly Func<int> _getLevel;

    internal PlayerSpells(PlayerStats currentStats, PlayerClass playerClass, Func<int> getLevel)
    {
        CurrentStats = currentStats;
        PlayerClass = playerClass;
        _getLevel = getLevel;
    }

    public int NumSpellsCanLearn => PlayerSpellHelpers.CalculateNumberOfAllowedSpells(CurrentStats, PlayerClass, _getLevel());
    public Stack<int> SpellsLearned = new();
    public Stack<int> SpellsForgotten = new();
    public List<int> SpellsWhichWorked = new();
    public List<int> OrderSpellsLearnedIn = new();
}