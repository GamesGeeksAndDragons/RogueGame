using System;
using Assets.Actors;
using Assets.Messaging;
using AssetsTests.Fakes;
using Utils;
using Utils.Coordinates;
using Utils.Enums;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.DispatcherTests
{
    public class LevelBuilderTests
    {
        private readonly ITestOutputHelper _output;

        public LevelBuilderTests(ITestOutputHelper output)
        {
            _output = output;
        }

        internal static IRandomNumberGenerator GetGenerator(int testNum)
        {
            var generator = new FakeRandomNumberGenerator();

            switch (testNum)
            {
                case 1:
                    generator.PopulateEnum(Compass4Points.South);
                    generator.PopulateDice(0, 1, 1, 1);
                    break;
                case 2:
                    generator.PopulateEnum(Compass4Points.South, Compass4Points.East);
                    generator.PopulateDice(0, 1, 0, 9, 4, 5, 5, 8);
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
                           "1|║@   ║" + Environment.NewLine +
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
                           "5|║       @║" + Environment.NewLine +
                           "6|║        ║" + Environment.NewLine +
                           "7|║        ║" + Environment.NewLine +
                           "8|║        ║" + Environment.NewLine +
                           "9|╚════════╝";

                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }
        }

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        public void WhenBuiltDispatcher_ShouldHaveMeInMaze(int testNum, int blocksPerTile)
        {
            var registry = new DispatchRegistry();
            var dispatcher = new Dispatcher(registry);

            var fakeRandomNumbers = GetGenerator(testNum);
            var fakeLogger = new FakeLogger(_output);
            var mazeDescriptor = FakeMazeDescriptorBuilder.Build(1, 1, 4, blocksPerTile);

            var builder = new LevelBuilder(fakeRandomNumbers, mazeDescriptor, fakeLogger, dispatcher, registry);
            builder.Build(testNum);
            var me = new Me(Coordinate.NotSet, registry, Me.FormatState(10, 10));
            dispatcher.EnqueueTeleport(me);
            dispatcher.Dispatch();

            var maze = registry.GetDispatchee("Maze1");
            var actual = maze.ToString();

            var expected = GetExpectation(testNum);

            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }
    }
}