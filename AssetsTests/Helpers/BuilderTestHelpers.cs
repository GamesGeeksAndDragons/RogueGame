using Assets.Deeds;
using Assets.Level;
using Assets.Resources;
using Assets.Rooms;
using AssetsTests.Fakes;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Utils;
using Utils.Dispatching;
using Utils.Random;

namespace AssetsTests.Helpers
{
    public class BuilderTestHelpers
    {
        public static readonly string Divider = '='.ToPaddedString(10);

        public string TestName { get; }
        internal ITestOutputHelper Output;

        protected readonly GameLevelBuilderFactory MazeBuilderFactory = new GameLevelBuilderFactory();

        protected IRoomBuilder RoomBuilder { get; set; }
        protected IResourceBuilder ResourceBuilder => MazeBuilderFactory.ResourceBuilder;
        protected IDispatchRegistry DispatchRegistry => MazeBuilderFactory.DispatchRegistry;
        protected IActionRegistry ActionRegistry => MazeBuilderFactory.ActionRegistry;

        protected IDieBuilder DieBuilder { get; set; }
        protected FakeLogger FakeLogger { get; }

        public BuilderTestHelpers(ITestOutputHelper output, string testName = FileAndDirectoryHelpers.LoadFolder)
        {
            TestName = testName;
            Output = output;

            DieBuilder = new DieBuilder(TestName, Die.RandomiserReset.None);
            FakeLogger = new FakeLogger(Output);
        }

        internal virtual void TestArrange(Die.RandomiserReset reset = Die.RandomiserReset.None)
        {
            RoomBuilder = MazeBuilderFactory.CreateRoomBuilder(DieBuilder, FakeLogger);
        }

        internal virtual void AssertTest(IGameLevel gameLevel, string expected)
        {
            var actual = gameLevel.Print(DispatchRegistry);

            Output.WriteLine(Divider + " expected " + Divider);
            Output.WriteLine(expected);
            Output.WriteLine(Divider + " expected " + Divider);
            Output.WriteLine(Divider + " actual ==" + Divider);
            Output.WriteLine(actual);
            Output.WriteLine(Divider + " actual ==" + Divider);

            Assert.Equal(expected, actual);
        }

        internal virtual void AssertTest(IGameLevel gameLevel, IMazeExpectations expectations)
        {
            var expected = expectations.ExpectedMaze;
            AssertTest(gameLevel, expected);
        }

        internal virtual void AssertTest(IRoom room, IMazeExpectations expectations)
        {
            var expected = expectations.ExpectedMaze;
            AssertTest(room, expected);
        }

        internal virtual void AssertTest(IRoom room, string expected)
        {
            var actual = room.Print(DispatchRegistry);

            Output.WriteLine(Divider + " expected " + Divider);
            Output.WriteLine(expected);
            Output.WriteLine(Divider + " expected " + Divider);
            Output.WriteLine(Divider + " actual ==" + Divider);
            Output.WriteLine(actual);
            Output.WriteLine(Divider + " actual ==" + Divider);

            Assert.Equal(expected, actual);
        }
    }
}