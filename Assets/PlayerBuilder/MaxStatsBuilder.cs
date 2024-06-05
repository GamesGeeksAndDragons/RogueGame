using Assets.StartingPlayerStatistics;
using Utils.Random;

namespace Assets.PlayerBuilder;

public class MaxStatsBuilder
{
    private readonly IDieBuilder _dieBuilder;

    public MaxStatsBuilder(IDieBuilder dieBuilder)
    {
        _dieBuilder = dieBuilder;
    }

    //static void characterGenerateStats()
    //{
    //    int total;
    //    int dice[18];

    //    do
    //    {
    //        total = 0;
    //        for (auto i = 0; i < 18; i++)
    //        {
    //            // Roll 3,4,5 sided dice once each
    //            dice[i] = randomNumber(3 + i % 3);
    //            total += dice[i];
    //        }
    //    } while (total <= 42 || total >= 54);

    //    for (auto i = 0; i < 6; i++)
    //    {
    //        py.stats.max[i] = uint8_t(5 + dice[3 * i] + dice[3 * i + 1] + dice[3 * i + 2]);
    //    }
    //}

    // based on https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/character.cpp#L11

    private int[] CalculateBaseStats()
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

            var die = _dieBuilder.Between(1, 3);

            for (var i = 0; i < numberOfStats; i++)
            {
                rolls[i] = 3 + die.Random + die.Random + die.Random;
            }

            return rolls;
        }
    }

    public PlayerStats GenerateMaxPlayerStats(PlayerRace race)
    {
        var baseStats = CalculateBaseStats();

        return new PlayerStats
        {
            Strength = baseStats[0] + race.Strength,
            Intelligence = baseStats[1] + race.Intelligence,
            Wisdom = baseStats[2] + race.Wisdom,
            Dexterity = baseStats[3] + race.Dexterity,
            Constitution = baseStats[4] + race.Constitution,
            Charisma = baseStats[5] + race.Charisma
        };
    }

}