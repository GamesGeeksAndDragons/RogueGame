using Assets.PlayerHelpers;
using System.Security.Claims;
using Assets.StartingPlayerStatistics;

namespace Assets.PlayerBuilder;

// https://github.com/jhirschberg70/browser-based-umoria/blob/f9fcf9ce217922be4941c7397007f5635ff2f838/src/player.h#L100
// taken from flags in an attempt to give meaning

public class PlayerSpells
{
    internal readonly IPlayerStats CurrentStats;
    internal readonly IPlayerClass PlayerClass;
    private readonly Func<int> _getLevel;

    public PlayerSpells(IPlayerStats currentStats, IPlayerClass playerClass, Func<int> getLevel)
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