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

    public IPlayer Build(string @class, string race, Gender gender)
    {
        IPlayerRace playerRace = PlayerRaces.Get()[race];
        IPlayerClass playerClass = PlayerClasses.Get()[@class];

        IDice d3 = _dieBuilder.Between(1, 3);
        IPlayerStats starting = StartingStatsBuilder.Generate(playerRace, playerClass, d3);

        IDice hitDie = _dieBuilder.Between(1, playerClass.HitDie);
        var baseLevels = CalculateBaseLevels();
        var hitPoints = new PlayerHitPoints(starting, playerClass.HitDie, hitDie, baseLevels);

        var height = GetHeight();
        var weight = GetWeight();


        return new Player(gender, playerRace, playerClass, starting, hitPoints, height, weight);

        int GetHeight()
        {
            var startStatistic = Gender.Male == gender ? playerRace!.MaleHeight : playerRace!.FemaleHeight;
            return GetModifiedStatistic(startStatistic.Base, startStatistic.Modifier);
        }

        int GetWeight()
        {
            var startStatistic = Gender.Male == gender ? playerRace!.MaleHeight : playerRace!.FemaleHeight;
            return GetModifiedStatistic(startStatistic.Base, startStatistic.Modifier);
        }

        int GetModifiedStatistic(int statBase, int modifier)
        {
            return RandomHelpers.NextGaussian(statBase, modifier);
        }

        int[] CalculateBaseLevels()
        {
            var (min, max) = CalcAllowedRange();

            var levels = new List<int>(Player.MaxAchievableLevel)
            {
                [0] = playerClass.HitDie
            };

            do
            {
                for (var i = 1; i < Player.MaxAchievableLevel; i++)
                {
                    levels[i] = d3.Random + levels[i - 1];
                }
            } while (levels[Player.MaxAchievableLevel - 1] < min || levels[Player.MaxAchievableLevel - 1] > max);

            return levels.ToArray();

            (int min, int max) CalcAllowedRange()
            {
                var min = (Player.MaxAchievableLevel * 3 / 8 * (playerClass.HitDie - 1)) + Player.MaxAchievableLevel;
                var max = (Player.MaxAchievableLevel * 5 / 8 * (playerClass.HitDie - 1)) + Player.MaxAchievableLevel;

                return (min, max);
            }
        }
    }
}