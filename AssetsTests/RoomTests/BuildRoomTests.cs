using System;
using Assets.Messaging;
using Assets.Rooms;
using AssetsTests.Fakes;
using Utils;
using Utils.Enums;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.RoomTests
{
    public class BuildRoomTests
    {
        private readonly ITestOutputHelper _output;

        public BuildRoomTests(ITestOutputHelper output)
        {
            _output = output;
        }

        internal static int GetBlockCount(int testNum)
        {
            switch (testNum)
            {
                case 1: return 2;
                case 2:
                case 3:
                case 4:
                case 5:
                    return 3;

                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }
        }

        internal static IRandomNumberGenerator GetGenerator(int testNum)
        {
            var generator = new FakeRandomNumberGenerator();

            switch (testNum)
            {
                case 1:
                    generator.PopulateEnum(Compass4Points.South);
                    generator.PopulateDice(0, 1);
                    break;
                case 2:
                    generator.PopulateEnum(Compass4Points.South, Compass4Points.East);
                    generator.PopulateDice(0, 1);
                    break;
                case 3:
                    generator.PopulateEnum(Compass4Points.East, Compass4Points.South);
                    generator.PopulateDice(0, 1);
                    break;
                case 4:
                    generator.PopulateEnum(Compass4Points.West, Compass4Points.South);
                    generator.PopulateDice(0, 1);
                    break;
                case 5:
                    generator.PopulateEnum(Compass4Points.South, Compass4Points.West);
                    generator.PopulateDice(0, 1);
                    break;

                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }

            return generator;
        }

        internal static string GetExpectation(int testNum)
        {
            switch (testNum)
            {
                case 1:
                    return " |012345" + Environment.NewLine +
                           "--------" + Environment.NewLine +
                           "0|╔════╗" + Environment.NewLine +
                           "1|║....║" + Environment.NewLine +
                           "2|║....║" + Environment.NewLine +
                           "3|║....║" + Environment.NewLine +
                           "4|║....║" + Environment.NewLine +
                           "5|║....║" + Environment.NewLine +
                           "6|║....║" + Environment.NewLine +
                           "7|║....║" + Environment.NewLine +
                           "8|║....║" + Environment.NewLine +
                           "9|╚════╝";
                case 2:
                    return " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|╔════╗    " + Environment.NewLine +
                           "1|║....║    " + Environment.NewLine +
                           "2|║....║    " + Environment.NewLine +
                           "3|║....║    " + Environment.NewLine +
                           "4|║....╚═══╗" + Environment.NewLine +
                           "5|║........║" + Environment.NewLine +
                           "6|║........║" + Environment.NewLine +
                           "7|║........║" + Environment.NewLine +
                           "8|║........║" + Environment.NewLine +
                           "9|╚════════╝";
                case 3:
                    return " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|╔════════╗" + Environment.NewLine +
                           "1|║........║" + Environment.NewLine +
                           "2|║........║" + Environment.NewLine +
                           "3|║........║" + Environment.NewLine +
                           "4|║........║" + Environment.NewLine +
                           "5|╚═══╗....║" + Environment.NewLine +
                           "6|    ║....║" + Environment.NewLine +
                           "7|    ║....║" + Environment.NewLine +
                           "8|    ║....║" + Environment.NewLine +
                           "9|    ╚════╝";
                case 4:
                    return " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|╔════════╗" + Environment.NewLine +
                           "1|║........║" + Environment.NewLine +
                           "2|║........║" + Environment.NewLine +
                           "3|║........║" + Environment.NewLine +
                           "4|║........║" + Environment.NewLine +
                           "5|║....╔═══╝" + Environment.NewLine +
                           "6|║....║    " + Environment.NewLine +
                           "7|║....║    " + Environment.NewLine +
                           "8|║....║    " + Environment.NewLine +
                           "9|╚════╝    ";
                case 5:
                    return " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|    ╔════╗" + Environment.NewLine +
                           "1|    ║....║" + Environment.NewLine +
                           "2|    ║....║" + Environment.NewLine +
                           "3|    ║....║" + Environment.NewLine +
                           "4|╔═══╝....║" + Environment.NewLine +
                           "5|║........║" + Environment.NewLine +
                           "6|║........║" + Environment.NewLine +
                           "7|║........║" + Environment.NewLine +
                           "8|║........║" + Environment.NewLine +
                           "9|╚════════╝";

                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void BuildRoom_ShouldBuildARoom_FromConnectedBlocks(int testNum)
        {
            var fakeRandomNumbers = GetGenerator(testNum);
            var builder = new RandomRoomBuilder(fakeRandomNumbers, new FakeLogger(_output), new ActorRegistry());

            var room = builder.BuildRoom(GetBlockCount(testNum));
            var actual = room.ToString();

            var expected = GetExpectation(testNum);
            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }
    }
}
