using Assets.Deeds;
using Assets.Level;
using Assets.Messaging;
using Assets.Resources;
using Assets.Rooms;
using AssetsTests.Fakes;
using log4net;
using Utils;
using Utils.Random;

namespace AssetsTests.Helpers
{
    public class BuilderTestHelpers
    {
        public static readonly string Divider = '='.ToPaddedString(10);

        public string TestName { get; }
        internal ITestOutputHelper Output;
        public RoomBuilder RoomBuilder;

        internal DispatchRegistry DispatchRegistry;
        internal ActionRegistry ActionRegistry;
        internal DieBuilder DieBuilder;
        internal ILog FakeLogger;
        internal ResourceBuilder ResourceBuilder;

        public BuilderTestHelpers(ITestOutputHelper output, string testName = FileAndDirectoryHelpers.LoadFolder)
        {
            TestName = testName;
            Output = output;
        }

        internal virtual void ArrangeTest(Die.RandomiserReset reset = Die.RandomiserReset.None)
        {
            DispatchRegistry ??= new DispatchRegistry();
            ActionRegistry ??= new ActionRegistry();
            DieBuilder ??= new DieBuilder(TestName, reset);
            FakeLogger ??= new FakeLogger(Output);
            ResourceBuilder ??= new ResourceBuilder(DispatchRegistry, ActionRegistry);
            RoomBuilder ??= new RoomBuilder(DieBuilder, FakeLogger, DispatchRegistry, ActionRegistry, ResourceBuilder);
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