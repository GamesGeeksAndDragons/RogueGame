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
    public class BuildSingleRoomMazeTests
    {
        private readonly ITestOutputHelper _output;

        public BuildSingleRoomMazeTests(ITestOutputHelper output)
        {
            _output = output;
        }

        internal static int GetLevel(int testNum)
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
                           "1|║    ║" + Environment.NewLine +
                           "2|║    ║" + Environment.NewLine +
                           "3|║    ║" + Environment.NewLine +
                           "4|║    ║" + Environment.NewLine +
                           "5|║    ║" + Environment.NewLine +
                           "6|║    ║" + Environment.NewLine +
                           "7|║    ║" + Environment.NewLine +
                           "8|║    ║" + Environment.NewLine +
                           "9|╚════╝";
                case 2:
                    return " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|╔════╗■■■■" + Environment.NewLine +
                           "1|║    ║■■■■" + Environment.NewLine +
                           "2|║    ║■■■■" + Environment.NewLine +
                           "3|║    ║■■■■" + Environment.NewLine +
                           "4|║    ╚═══╗" + Environment.NewLine +
                           "5|║        ║" + Environment.NewLine +
                           "6|║        ║" + Environment.NewLine +
                           "7|║        ║" + Environment.NewLine +
                           "8|║        ║" + Environment.NewLine +
                           "9|╚════════╝";
                case 3:
                    return " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|╔════════╗" + Environment.NewLine +
                           "1|║        ║" + Environment.NewLine +
                           "2|║        ║" + Environment.NewLine +
                           "3|║        ║" + Environment.NewLine +
                           "4|║        ║" + Environment.NewLine +
                           "5|╚═══╗    ║" + Environment.NewLine +
                           "6|■■■■║    ║" + Environment.NewLine +
                           "7|■■■■║    ║" + Environment.NewLine +
                           "8|■■■■║    ║" + Environment.NewLine +
                           "9|■■■■╚════╝";
                case 4:
                    return " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|╔════════╗" + Environment.NewLine +
                           "1|║        ║" + Environment.NewLine +
                           "2|║        ║" + Environment.NewLine +
                           "3|║        ║" + Environment.NewLine +
                           "4|║        ║" + Environment.NewLine +
                           "5|║    ╔═══╝" + Environment.NewLine +
                           "6|║    ║■■■■" + Environment.NewLine +
                           "7|║    ║■■■■" + Environment.NewLine +
                           "8|║    ║■■■■" + Environment.NewLine +
                           "9|╚════╝■■■■";
                case 5:
                    return " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|■■■■╔════╗" + Environment.NewLine +
                           "1|■■■■║    ║" + Environment.NewLine +
                           "2|■■■■║    ║" + Environment.NewLine +
                           "3|■■■■║    ║" + Environment.NewLine +
                           "4|╔═══╝    ║" + Environment.NewLine +
                           "5|║        ║" + Environment.NewLine +
                           "6|║        ║" + Environment.NewLine +
                           "7|║        ║" + Environment.NewLine +
                           "8|║        ║" + Environment.NewLine +
                           "9|╚════════╝";

                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }
        }

        [Theory]
        [InlineData(1, 1, 1, 4, 2)]
        [InlineData(2, 1, 1, 4, 3)]
        [InlineData(3, 1, 1, 4, 3)]
        [InlineData(4, 1, 1, 4, 3)]
        [InlineData(5, 1, 1, 4, 3)]
        public void BuildMaze_ShouldBuildASingleRoomMaze_FromConnectedBlocks(int testNum, int minRooms, int maxRooms, int tileInBlock, int blocksInRoom)
        {
            var fakeRandomNumbers = GetGenerator(testNum);
            var mazeDescriptor = FakeMazeDescriptorBuilder.Build(minRooms, maxRooms, tileInBlock, blocksInRoom);
            var builder = new RandomMazeBuilder(fakeRandomNumbers, mazeDescriptor, new FakeLogger(_output), new DispatchRegistry());

            var maze = builder.BuildMaze(GetLevel(testNum));
            var actual = maze.ToString();
            
            var expected = GetExpectation(testNum);

            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }
    }
}
