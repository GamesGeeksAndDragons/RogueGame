using System;
using Assets.Messaging;
using Assets.Mazes;
using AssetsTests.Fakes;
using Utils;
using Utils.Enums;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.MazeTests
{
    public class BlockNavigationTests
    {
        private readonly ITestOutputHelper _output;
        public enum Test
        {
            TopLeft, TopRight, BottomRight, BottomLeft, Cornered
        }

        public BlockNavigationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        public static int GetNumBlocks(Test test)
        {
            switch (test)
            {
                case Test.TopLeft:
                case Test.TopRight:
                case Test.BottomRight:
                case Test.BottomLeft:
                    return 2;
                case Test.Cornered:
                    return 5;
            }

            throw new ArgumentException($"Didn't have Blocks for [{test}]");
        }

        public static string GetExpected(Test test)
        {
            switch (test)
            {
                case (Test)(-1):
                    return
                        " |01" + Environment.NewLine +
                        "----" + Environment.NewLine +
                        "0|.." + Environment.NewLine +
                        "1|..";
                case Test.TopLeft:
                    return
                        " |01" + Environment.NewLine +
                        "----" + Environment.NewLine +
                        "0|**" + Environment.NewLine +
                        "1|..";
                case Test.TopRight:
                    return
                        " |01" + Environment.NewLine +
                        "----" + Environment.NewLine +
                        "0|.*" + Environment.NewLine +
                        "1|.*";
                case Test.BottomRight:
                    return
                        " |01" + Environment.NewLine +
                        "----" + Environment.NewLine +
                        "0|.." + Environment.NewLine +
                        "1|**";
                case Test.BottomLeft:
                    return
                        " |01" + Environment.NewLine +
                        "----" + Environment.NewLine +
                        "0|*." + Environment.NewLine +
                        "1|*.";
                case Test.Cornered:
                    return
                        " |01234" + Environment.NewLine +
                        "-------" + Environment.NewLine +
                        "0|....." + Environment.NewLine +
                        "1|....." + Environment.NewLine +
                        "2|....." + Environment.NewLine +
                        "3|...**" + Environment.NewLine +
                        "4|..***";
            }

            throw new ArgumentException($"Didn't have Expected Layout for [{test}]");
        }

        internal static IRandomNumberGenerator GetGenerator(Test test)
        {
            var generator = new FakeRandomNumberGenerator();

            switch (test)
            {
                case Test.TopLeft:
                    generator.PopulateEnum(Compass4Points.West, Compass4Points.North, Compass4Points.East, Compass4Points.East, Compass4Points.East);
                    generator.PopulateDice(0, 1);
                    break;
                case Test.TopRight:
                    generator.PopulateEnum(Compass4Points.North, Compass4Points.East, Compass4Points.South, Compass4Points.East, Compass4Points.East);
                    generator.PopulateDice(1, 1);
                    break;
                case Test.BottomRight:
                    generator.PopulateEnum(Compass4Points.South, Compass4Points.West, Compass4Points.East, Compass4Points.East, Compass4Points.East);
                    generator.PopulateDice(1, 0);
                    break;
                case Test.BottomLeft:
                    generator.PopulateEnum(Compass4Points.South, Compass4Points.West, Compass4Points.North, Compass4Points.East, Compass4Points.East);
                    generator.PopulateDice(0, 0);
                    break;
                case Test.Cornered:
                    generator.PopulateEnum(Compass4Points.West, Compass4Points.South, Compass4Points.East, Compass4Points.East, Compass4Points.East);
                    generator.PopulateDice(3, 4, 0, 0, 1, 1, 4, 2);
                    break;

                default: throw new ArgumentException($"Didn't have Generator for [{test}]");
            }

            return generator;
        }

        [Theory]
        [InlineData(Test.TopLeft)]
        [InlineData(Test.TopRight)]
        [InlineData(Test.BottomRight)]
        [InlineData(Test.BottomLeft)]
        [InlineData(Test.Cornered)]
        public void DecideLayout_ForSimpleNavigation_ShouldMoveAsExpected(Test test)
        {
            var fakeRandomNumbers = GetGenerator(test);
            var builder = new RoomBuilder(fakeRandomNumbers, new FakeLogger(_output), new DispatchRegistry());
            var numBlocks = GetNumBlocks(test);

            var blocks = builder.DecideLayout(numBlocks);

            var expected = GetExpected(test);
            var actual = blocks.ToString();

            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }
    }
}