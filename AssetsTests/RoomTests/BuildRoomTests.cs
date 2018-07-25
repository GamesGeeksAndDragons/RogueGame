using System;
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

        internal static IRandomNumberGenerator GetGenerator(int testNum)
        {
            var generator = new FakeRandomNumberGenerator();

            switch (testNum)
            {
                case 2:
                    generator.PopulateEnum(Compass4Points.West, Compass4Points.North, Compass4Points.East, Compass4Points.East, Compass4Points.East);
                    generator.PopulateDice(0, 1);
                    break;

                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }

            return generator;
        }

        [Fact]
        public void BuildRoom_ShouldBuildARoom_FromConnectedBlocks()
        {
            var fakeRandomNumbers = GetGenerator(2);
            var builder = new RandomRoomBuilder(fakeRandomNumbers, new FakeLogger(_output));
            var room = builder.BuildRoom(2);

            var expected = " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|╔════════╗" + Environment.NewLine +
                           "1|║00000000║" + Environment.NewLine +
                           "2|║00000000║" + Environment.NewLine +
                           "3|║00000000║" + Environment.NewLine +
                           "4|║00000000║" + Environment.NewLine +
                           "5|╚════════╝";

            var actual = room.ToString();

            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }
    }
}
