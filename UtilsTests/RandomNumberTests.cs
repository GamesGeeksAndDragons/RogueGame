using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using Utils.Enums;
using Utils.Random;
using Xunit;

namespace UtilsTests
{
    public class RandomNumberTests
    {
        [Theory]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(6)]
        [InlineData(12)]
        [InlineData(20)]
        public void ARandomDice_ShouldHaveAGoodDistribution(int points)
        {
            int numRolls = points * 10000;

            var rolls = new int[numRolls];

            var generator = new DieBuilder(nameof(RandomNumberTests), Die.RandomiserReset.Full);

            for (int i = 0; i < numRolls; i++)
            {
                rolls[i] = generator.Dice(points).Random;
            }

            var keys = new List<int>();
            var maxPoints = Math.Max(points, rolls.Max());
            for (int i = 1; i <= maxPoints; i++)
            {
                keys.Add(i);
            }


            var samples = keys.Select(key =>
            {
                var count = rolls.Count(roll => key == roll);
                return (key: key, count: count);
            }).ToList();

            var midpoint = numRolls / points;
            var percent = 10.0 / 100;
            var min = midpoint - (midpoint * percent);
            var max = midpoint + (midpoint * percent);

            var errorMessage = new StringBuilder();
            foreach (var sample in samples)
            {
                errorMessage.AppendLine($"Expected [{sample.key}] to be in the range [{min},{max}] and it was [{sample.count}]");
            }

            foreach (var sample in samples)
            {
               Assert.True(sample.count >= min && sample.count <= max, errorMessage.ToString());
            }
        }

        [Fact]
        public void ARandomCompassDirection_ShouldHaveAGoodDistribution()
        {
            var enumItems = EnumHelpers.Values<Compass8Points>().ToList();
            enumItems.Remove(Compass8Points.Undefined);
            var enumCount = enumItems.Count;

            int numRolls = enumCount * 1000;

            var rolls = new Compass8Points[numRolls];

            var generator = new DieBuilder(nameof(RandomNumberTests), Die.RandomiserReset.Full);

            for (int i = 0; i < numRolls; i++)
            {
                rolls[i] = generator.Compass8Die.Random;
            }

            var samples = enumItems.Select(enumItem =>
            {
                var count = rolls.Count(roll => enumItem == roll);
                return (enumItem, count);
            }).ToList();

            var midpoint = 1000;
            var percent = 10.0 / 100;
            var min = midpoint - (midpoint * percent);
            var max = midpoint + (midpoint * percent);

            var errorMessage = new StringBuilder();
            foreach (var sample in samples)
            {
                errorMessage.AppendLine($"Expected [{sample.Item1}] to be in the range [{min},{max}] and it was [{sample.Item2}]");
            }

            foreach (var sample in samples)
            {
                Assert.True(sample.Item2 >= min && sample.Item2 <= max, errorMessage.ToString());
            }
        }
    }
}
