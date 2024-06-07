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

        var d3 = _dieBuilder.Between(1, 3);
        var starting = StartingStatsBuilder.Generate(race, d3);

        var hitPointsDie = _dieBuilder.Between(1, pClass.HitDie);
        var hitPoints = new PlayerHitPoints(hitPointsDie, pClass.HitDie);

        var height = GetHeight();
        var weight = GetWeight();

        return new Player(gender, race, pClass, starting, hitPoints, height, weight);

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