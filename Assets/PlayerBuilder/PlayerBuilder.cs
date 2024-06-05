using Assets.StartingPlayerStatistics;
using System.Reflection.PortableExecutable;
using Utils.Random;

namespace Assets.PlayerBuilder;

// https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/character.cpp

public class PlayerBuilder
{
    private readonly IDieBuilder _dieBuilder;

    public PlayerBuilder(IDieBuilder dieBuilder)
    {
        _dieBuilder = dieBuilder;
    }

    public IPlayer Build(string playerClass, string playerRace)
    {
        var race = PlayerRaces.Get()[playerRace];
        var pClass = PlayerClasses.Get()[playerClass];

        var maxStatsBuilder = new MaxStatsBuilder(_dieBuilder);
        var maxStats = maxStatsBuilder.GenerateMaxPlayerStats(race);

        return new Player
        {
            Race = race,
            Class = pClass,
            Maximum = maxStats,
            Current = GenerateCurrent()
        };

        PlayerStats GenerateCurrent()
        {
            return new PlayerStats
            {
                Strength = maxStats.Strength,
                Intelligence = maxStats.Intelligence,
                Wisdom = maxStats.Wisdom,
                Dexterity = maxStats.Dexterity,
                Constitution = maxStats.Constitution,
                Charisma = maxStats.Charisma
            };
        }
    }
}