using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;
using Utils.Enums;
using Utils.Random;
using Xunit;

namespace AssetsTests
{
    public class RandomNumberTests
    {
        [Fact]
        public void RandomBoolean_ShouldBeApproxTheSame()
        {
            int numRolls = 2 * 1000;

            var rolls = new bool[numRolls];

            var generator = new RandomNumberGenerator();

            for (int i = 1; i < numRolls; i++)
            {
                rolls[i] = generator.Boolean;
            }

            var keys = new List<bool> {true, false};
            var samples = keys.Select(key =>
            {
                var count = rolls.Count(roll => key == roll);
                return (key: key, count: count);
            }).ToList();

            var midpoint = 1000;
            var min = midpoint - (midpoint * 5 / 100);
            var max = midpoint + (midpoint * 5 / 100);

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

        [Theory]
        [InlineData(3)]
        [InlineData(6)]
        [InlineData(12)]
        [InlineData(20)]
        public void ARandomDice_ShouldHaveAGoodDistribution(int points)
        {
            int numRolls = points * 1000;

            var rolls = new int[numRolls];

            var generator = new RandomNumberGenerator();

            for (int i = 1; i < numRolls; i++)
            {
                rolls[i] = generator.Dice(points);
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
        public void ARandomEnum_ShouldHaveAGoodDistribution()
        {
            var enumItems = EnumHelpers.Values<Compass4Points>().ToList();
            var enumCount = enumItems.Count;

            int numRolls = enumCount * 1000;

            var rolls = new Compass4Points[numRolls];

            var generator = new RandomNumberGenerator();

            for (int i = 1; i < numRolls; i++)
            {
                rolls[i] = generator.Enum<Compass4Points>();
            }

            var samples = enumItems.Select(enumItem =>
            {
                var count = rolls.Count(roll => enumItem == roll);
                return (enumItem: enumItem, count: count);
            }).ToList();

            var midpoint = 1000;
            var percent = 10.0 / 100;
            var min = midpoint - (midpoint * percent);
            var max = midpoint + (midpoint * percent);

            var errorMessage = new StringBuilder();
            foreach (var sample in samples)
            {
                errorMessage.AppendLine($"Expected [{sample.enumItem}] to be in the range [{min},{max}] and it was [{sample.count}]");
            }

            foreach (var sample in samples)
            {
                Assert.True(sample.count >= min && sample.count <= max, errorMessage.ToString());
            }
        }
    }
}
