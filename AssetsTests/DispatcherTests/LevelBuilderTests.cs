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

        [Fact]
        public void Test()
        {
            WhenBuiltDispatcher_ShouldHaveMeInMaze(2);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void WhenBuiltDispatcher_ShouldHaveMeInMaze(int level)
        {
            var dispatchRegistry = new DispatchRegistry();
            var actionRegistry = new ActionRegistry();
            var dispatcher = new Dispatcher(dispatchRegistry);
            var fakeRandomNumbers = new DieBuilder();
            var fakeLogger = new FakeLogger(_output);
            var actorBuilder = new ActorBuilder(dispatchRegistry, actionRegistry);

            var builder = new LevelBuilder(fakeRandomNumbers, fakeLogger, dispatcher, dispatchRegistry, actionRegistry, actorBuilder);
            var maze = builder.Build(level);
            var actual = maze.Tiles.Print(dispatchRegistry);
            _output.OutputMazes(actual);

            var me = new Me(dispatchRegistry, actionRegistry, "");
            dispatcher.EnqueueTeleport(me);
            dispatcher.Dispatch();

            //var maze = dispatchRegistry.GetDispatchee("Maze1");
            actual = maze.Tiles.Print(dispatchRegistry);
            _output.OutputMazes(actual);
        }
    }
}