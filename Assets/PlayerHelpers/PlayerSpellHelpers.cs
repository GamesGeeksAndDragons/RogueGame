using Assets.PlayerBuilder;
using Assets.StartingPlayerStatistics;

namespace Assets.PlayerHelpers;

public static class PlayerSpellHelpers
{

    // https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/player.cpp#L1607
    // playerCalculateAllowedSpellsCount
    internal static int CalculateNumberOfAllowedSpells(IPlayerStats usedStats, IPlayerClass playerClass, int currentLevel)
    {
        var stat = playerClass.Spells == SpellsEnum.Mage ? usedStats.Intelligence : usedStats.Wisdom;
        var minLevel = playerClass.SpellMinLevel;

        var numAllowed = CalcNumSpellsAllowedForLevel();
        return numAllowed;

        // https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/player.cpp#L1474
        // numberOfSpellsAllowed
        int CalcNumSpellsAllowedForLevel()
        {
            var levels = currentLevel - minLevel + 1;

            return PlayerStatsHelpers.GetStatAdjustmentForWisdomOrIntelligence(stat) switch
            {
                1 => 1 * levels,
                2 => 1 * levels,
                3 => 1 * levels,
                4 => 3 * levels / 2,
                5 => 3 * levels / 2,
                6 => 2 * levels,
                7 => 5 * levels / 2,
                _ => 0
            };
        }
    }

    internal static void SetupForTurn(this PlayerSpells spells)
    {
        var maxNumSpellsCanLearn = spells.NumSpellsCanLearn;
        var numSpellsKnown = spells.SpellsLearned.Count;
        var numSpellsCanLearn = maxNumSpellsCanLearn - numSpellsKnown;

        if (numSpellsCanLearn > 0)
        {
            RememberForgottenSpells();
        }

        void RememberForgottenSpells()
        {
            while (spells.SpellsForgotten.Count != 0 && numSpellsCanLearn != 0)
            {
                var spell = spells.SpellsForgotten.Pop();
                spells.SpellsLearned.Push(spell);
            }
        }
    }
}