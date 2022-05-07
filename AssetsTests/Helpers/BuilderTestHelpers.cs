using Assets.Deeds;
using Assets.Mazes;
using Assets.Messaging;
using Assets.Resources;
using Assets.Rooms;
using AssetsTests.Fakes;
using log4net;
using Utils;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

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
        internal DieBuilder Random;
        internal ILog Logger;
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
            Random ??= new DieBuilder(TestName, reset);
            Logger ??= new FakeLogger(Output);
            ResourceBuilder ??= new ResourceBuilder(DispatchRegistry, ActionRegistry);
            RoomBuilder ??= new RoomBuilder(Random, Logger, DispatchRegistry, ActionRegistry, ResourceBuilder);
        }

        internal virtual void AssertTest(IMaze maze, string expected = "")
        {
            var actual = maze.Print(DispatchRegistry);

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