using Assets.Deeds;
using Assets.Mazes;
using Assets.Messaging;
using Assets.Rooms;
using AssetsTests.Fakes;
using AssetsTests.Helpers;
using AssetsTests.MazeTests.Helpers;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.MazeTests
{
    public class MazeBuilderTests
    {
        private readonly ITestOutputHelper _output;
        private readonly string _testName;

        public MazeBuilderTests(ITestOutputHelper output)
        {
            _output = output;
            _testName = nameof(MazeBuilderTests);
        }

        private LevelBuilder Arrange()
        {
            var dispatchRegistry = new DispatchRegistry();
            var actionRegistry = new ActionRegistry();
            var dieBuilder = new DieBuilder(_testName, Die.RandomiserReset.Index);
            var logger = new FakeLogger(_output);
            var dispatcher = new Dispatcher(dispatchRegistry);
            return new LevelBuilder(dieBuilder, logger, dispatcher, dispatchRegistry, actionRegistry);
        }

        [Fact]
        public void WhenBuildLevelOneMaze_WithConnectedDoors_ShouldBeAbleToBuildIt()
        {
            var levelBuilder = Arrange();

            var maze = levelBuilder.Build(1);

            _output.OutputMazes(maze);
        }
    }
}
