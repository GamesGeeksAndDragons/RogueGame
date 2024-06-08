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

    internal Player Build(string @class, string race, Gender gender)
    {
        PlayerRace playerRace = PlayerRacesLoader.Load()[race];
        PlayerClass playerClass = PlayerClassesLoader.Load()[@class];

        var spellsToUse = playerClass.Spells.ToString();
        var spellNames = SpellNamesLoader.Load()[spellsToUse];
        PopulateMagicBookWithSpells();

        IDice d3 = _dieBuilder.Between(1, 3);
        PlayerStats starting = StartingStatsBuilder.Generate(playerRace, playerClass, d3);

        var baseLevels = CalculateBaseHitPointsPerLevel();
        var hitPoints = new PlayerHitPoints(starting, playerClass.HitDie, baseLevels);

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

        // https://github.com/jhirschberg70/browser-based-umoria/blob/master/src/character.cpp#L390
        int[] CalculateBaseHitPointsPerLevel()
        {
            var (min, max) = CalcAllowedRange();

            var levels = new int[Player.MaxAchievableLevel];
            levels[0] = playerClass.HitDie;
            IDice hitDie = _dieBuilder.Between(1, playerClass.HitDie);

            do
            {
                for (var i = 1; i < Player.MaxAchievableLevel; i++)
                {
                    levels[i] = hitDie.Random + levels[i - 1];
                }
            } while (levels[Player.MaxAchievableLevel - 1] < min || levels[Player.MaxAchievableLevel - 1] > max);

            return levels;

            (int min, int max) CalcAllowedRange()
            {
                var minGuardRail = (Player.MaxAchievableLevel * 3 / 8 * (playerClass.HitDie - 1)) + Player.MaxAchievableLevel;
                var maxGuardRail = (Player.MaxAchievableLevel * 5 / 8 * (playerClass.HitDie - 1)) + Player.MaxAchievableLevel;

                return (minGuardRail, maxGuardRail);
            }
        }

        void PopulateMagicBookWithSpells()
        {
            foreach (var bookSpell in playerClass.MagicBook)
            {
                var index = bookSpell.Key;
                var spell = bookSpell.Value;

                var namedSpell = spellNames[index];
                spell.Name = namedSpell.Name;
                spell.Description = namedSpell.Description;
            }
        }
    }
}