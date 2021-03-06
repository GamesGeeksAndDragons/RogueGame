﻿using System;
using Assets.Mazes;
using Assets.Messaging;
using AssetsTests.Fakes;
using Utils.Enums;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.RoomTests
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

    public class BlockLayoutTests
    {
        private readonly ITestOutputHelper _output;

        public BlockLayoutTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private string GetExpectedLayout(int numBlocks)
        {
            switch (numBlocks)
            {
                case 3:
                    return
                        " |012" + Environment.NewLine +
                        "-----" + Environment.NewLine +
                        "0|..." + Environment.NewLine +
                        "1|.**" + Environment.NewLine +
                        "2|..*";
                case 4:
                    return
                        " |0123" + Environment.NewLine +
                        "------" + Environment.NewLine +
                        "0|...." + Environment.NewLine +
                        "1|.***" + Environment.NewLine +
                        "2|...*" + Environment.NewLine +
                        "3|....";
                case 5:
                    return
                        " |01234" + Environment.NewLine +
                        "-------" + Environment.NewLine +
                        "0|....." + Environment.NewLine +
                        "1|.****" + Environment.NewLine +
                        "2|...*." + Environment.NewLine +
                        "3|....." + Environment.NewLine +
                        "4|.....";
                case 6:
                    return
                        " |012345" + Environment.NewLine +
                        "--------" + Environment.NewLine +
                        "0|....*." + Environment.NewLine +
                        "1|.****." + Environment.NewLine +
                        "2|...*.." + Environment.NewLine +
                        "3|......" + Environment.NewLine +
                        "4|......" + Environment.NewLine +
                        "5|......";
                case 7:
                    return
                        " |0123456" + Environment.NewLine +
                        "---------" + Environment.NewLine +
                        "0|...**.." + Environment.NewLine +
                        "1|.****.." + Environment.NewLine +
                        "2|...*..." + Environment.NewLine +
                        "3|......." + Environment.NewLine +
                        "4|......." + Environment.NewLine +
                        "5|......." + Environment.NewLine +
                        "6|.......";
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
            var builder = new RoomBuilder(fakeRandomNumbers, new FakeLogger(_output), new DispatchRegistry());
            var blocks = builder.DecideLayout(numBlocks);

            var expected = GetExpectedLayout(numBlocks);
            var actual = blocks.ToString();

            _output.WriteLine(expected);
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }
    }
}