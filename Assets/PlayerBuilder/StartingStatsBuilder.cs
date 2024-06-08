using Assets.StartingPlayerStatistics;
using Utils.Random;

namespace Assets.PlayerBuilder;

public static class StartingStatsBuilder
{
    // based on https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/character.cpp#L11
    private static int[] CalculateBaseStats(IDice d3)
    {
        var diceRolls = RoleDice();
        var total = diceRolls.Sum();

        const int lowerGuardRail = 32;
        const int upperGuardRail = 54;

        while (total is <= lowerGuardRail or >= upperGuardRail)
        {
            diceRolls = RoleDice();
            total = diceRolls.Sum();
        }

        return diceRolls;

        int[] RoleDice()
        {
            const int numberOfStats = 6;
            var rolls = new int[numberOfStats];

            for (var i = 0; i < numberOfStats; i++)
            {
                rolls[i] = 3 + d3.Random + d3.Random + d3.Random;
            }

            return rolls;
        }
    }

    internal static PlayerStats Generate(PlayerRace race, PlayerClass playerClass, IDice d3)
    {
        var baseStats = CalculateBaseStats(d3);

        return new PlayerStats
        {
            Strength = baseStats[0] + race.Strength,
            Intelligence = baseStats[1] + race.Intelligence,
            Wisdom = baseStats[2] + race.Wisdom,
            Dexterity = baseStats[3] + race.Dexterity,
            Constitution = baseStats[4] + race.Constitution,
            Charisma = baseStats[5] + race.Charisma,

            Fight = playerClass.Fight,
            Bows = playerClass.Bows,
            Search = playerClass.Search,
            Disarm = playerClass.Disarm,
            SearchFrequency = playerClass.SearchFrequency,
            Stealth = playerClass.Stealth,
            Saving = playerClass.Saving,
            ExperiencePenalty = playerClass.ExperiencePenalty
        };
    }
}