using Assets.Actors;
using Assets.Deeds;
using Assets.Messaging;
using AssetsTests.Fakes;
using AssetsTests.Helpers;
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

        [Theory]
        [InlineData(1)]
        //[InlineData(2, 3)]
        public void WhenBuiltDispatcher_ShouldHaveMeInMaze(int level)
        {
            var dispatchRegistry = new DispatchRegistry();
            var actionRegistry = new ActionRegistry();
            var dispatcher = new Dispatcher(dispatchRegistry);
            var fakeRandomNumbers = new DieBuilder();
            var fakeLogger = new FakeLogger(_output);

            var builder = new LevelBuilder(fakeRandomNumbers, fakeLogger, dispatcher, dispatchRegistry, actionRegistry);
            var maze = builder.Build(level);
            var coordinates = new Coordinate(10, 10);
            var me = new Me(coordinates, dispatchRegistry, actionRegistry, "");
            dispatcher.EnqueueTeleport(me);
            dispatcher.Dispatch();

            //var maze = dispatchRegistry.GetDispatchee("Maze1");
            var actual = maze.ToString();

            _output.OutputMazes(actual);
        }
    }
}