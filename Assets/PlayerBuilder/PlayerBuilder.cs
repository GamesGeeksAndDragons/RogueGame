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

    public IPlayer Build(string playerClass, string playerRace, Gender gender)
    {
        var race = PlayerRaces.Get()[playerRace];
        var pClass = PlayerClasses.Get()[playerClass];

        var maxStatsBuilder = new MaxStatsBuilder(_dieBuilder);
        var maxStats = maxStatsBuilder.GenerateMaxPlayerStats(race);

        return new Player(gender, race, pClass, maxStats, GetHeight(), GetWeight());

        int GetHeight()
        {
            var startStatistic = Gender.Male == gender ? race!.MaleHeight : race!.FemaleHeight;
            return GetModifiedStatistic(startStatistic.Base, startStatistic.Modifier);
        }

        int GetWeight()
        {
            var startStatistic = Gender.Male == gender ? race!.MaleHeight : race!.FemaleHeight;
            return GetModifiedStatistic(startStatistic.Base, startStatistic.Modifier);
        }

        int GetModifiedStatistic(int statBase, int modifier)
        {
            return RandomHelpers.NextGaussian(statBase, modifier);
        }
    }
}