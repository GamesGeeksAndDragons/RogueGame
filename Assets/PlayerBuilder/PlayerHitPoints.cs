using System;
using Utils.Random;

namespace Assets.PlayerBuilder;

public class PlayerHitPoints
{
    public int Die { get; set; }
    public int Current { get; set; }
    public int Maximum { get; set; }

    public int[] BaseLevels = new int[Player.MaxLevel];

    public PlayerHitPoints(IDice die, int hitDie)
    {
        Die = Current = hitDie;
        BaseLevels[0] = hitDie;

        PopulateBaseLevels();

        void PopulateBaseLevels()
        {
            var (min, max) = CalcAllowedRange();

            do
            {
                for (var i = 1; i < Player.MaxLevel; i++)
                {
                    BaseLevels[i] = die.Random + BaseLevels[i - 1];
                }
            } while (BaseLevels[Player.MaxLevel - 1] < min || BaseLevels[Player.MaxLevel - 1] > max);
        }

        (int min, int max) CalcAllowedRange()
        {
            var min = (Player.MaxLevel * 3 / 8 * (hitDie - 1)) + Player.MaxLevel;
            var max = (Player.MaxLevel * 5 / 8 * (hitDie - 1)) + Player.MaxLevel;

            return (min, max);
        }
    }
}