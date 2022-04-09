using Assets.Actors;
using Assets.Deeds;
using Assets.Messaging;
using AssetsTests.Fakes;
using AssetsTests.MazeTests.Helpers;
using Utils.Coordinates;
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

        internal static IDieBuilder GetFakeRandomGenerator(int testNum)
        {
            return new DieBuilder();
        }

        [Theory]
        [InlineData(1, 2)]
        //[InlineData(2, 3)]
        public void WhenBuiltDispatcher_ShouldHaveMeInMaze(int testNum, int blocksInRoom)
        {
            var dispatchRegistry = new DispatchRegistry();
            var actionRegistry = new ActionRegistry();
            var dispatcher = new Dispatcher(dispatchRegistry);
            var fakeRandomNumbers = GetFakeRandomGenerator(testNum);
            var fakeLogger = new FakeLogger(_output);

            var builder = new LevelBuilder(fakeRandomNumbers, fakeLogger, dispatcher, dispatchRegistry, actionRegistry);
            builder.Build(testNum);
            var me = new Me(Coordinate.NotSet, dispatchRegistry, actionRegistry, Me.FormatState(10, 10));
            dispatcher.EnqueueTeleport(me);
            dispatcher.Dispatch();

            var maze = dispatchRegistry.GetDispatchee("Maze1");
            var actual = maze.ToString();

            _output.OutputMazes(actual);
        }
    }
}