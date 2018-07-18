using System;
using Assets.Rooms;
using AssetsTests.Fakes;
using Utils.Random;
using Utils.Enums;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests
{
    static class FakeRandomNumberTestFactory
    {
        internal static IRandomNumberGenerator CreateGenerator(int numBlocks)
        {
            var random = new FakeRandomNumberGenerator();
            random.PopulateEnum(
                Compass4Points.East,
                Compass4Points.East,
                Compass4Points.South,
                Compass4Points.North,
                Compass4Points.East,
                Compass4Points.North,
                Compass4Points.West,
                Compass4Points.South,
                Compass4Points.South,
                Compass4Points.West,
                Compass4Points.West
            );

            var start = 1;
            var posOfNonConnected = numBlocks - 1;

            random.PopulateDice(start, start, posOfNonConnected, posOfNonConnected, - 1, 0, 1, 3, 0, 0, 2);
            return random;
        }
    }

    public class RandomRoomBuilderTests
    {
        private readonly ITestOutputHelper _output;

        public RandomRoomBuilderTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private string GetExpectedLayout(int numBlocks)
        {
            switch (numBlocks)
            {
                case 3:
                    return
                        "...|" + Environment.NewLine +
                        ".**|" + Environment.NewLine +
                        "..*|" + Environment.NewLine;
                case 4:
                    return "....|" + Environment.NewLine +
                           ".***|" + Environment.NewLine +
                           "...*|" + Environment.NewLine +
                           "....|" + Environment.NewLine;
                case 5:
                    return ".....|" + Environment.NewLine +
                           ".****|" + Environment.NewLine +
                           "...*.|" + Environment.NewLine +
                           ".....|" + Environment.NewLine +
                           ".....|" + Environment.NewLine;
                case 6:
                    return "....*.|" + Environment.NewLine +
                           ".****.|" + Environment.NewLine +
                           "...*..|" + Environment.NewLine +
                           "......|" + Environment.NewLine +
                           "......|" + Environment.NewLine +
                           "......|" + Environment.NewLine;
                case 7:
                    return "...**..|" + Environment.NewLine +
                           ".****..|" + Environment.NewLine +
                           "...*...|" + Environment.NewLine +
                           ".......|" + Environment.NewLine +
                           ".......|" + Environment.NewLine +
                           ".......|" + Environment.NewLine +
                           ".......|" + Environment.NewLine;
            }

            throw new ArgumentException($"Didn't have Expected Layout for [{numBlocks}] blocks");
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void DecideLayout_ShouldHaveConnectedBlocks(int numBlocks)
        {
            var fakeRandomNumbers = FakeRandomNumberTestFactory.CreateGenerator(numBlocks);
            var builder = new RandomRoomBuilder(fakeRandomNumbers, new FakeLogger(_output));
            var blocks = builder.DecideLayout(numBlocks);

            var expected = GetExpectedLayout(numBlocks);
            var actual = blocks.ToString();

            _output.WriteLine(expected);
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }
    }
}