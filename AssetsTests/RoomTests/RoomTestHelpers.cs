using Assets.Deeds;
using Assets.Messaging;
using Assets.Resources;
using Assets.Rooms;
using AssetsTests.Fakes;
using AssetsTests.Helpers;
using log4net;
using Utils;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.RoomTests
{
    public class RoomTestHelpers
    {
        public static readonly string Divider = '='.ToPaddedString(10);
        public RoomBuilder Builder;

        internal DispatchRegistry DispatchRegistry;
        internal ActionRegistry ActionRegistry;
        internal DieBuilder Random;
        internal ILog Logger;
        internal ResourceBuilder ResourceBuilder;

        internal Room ArrangeTest(string testName, ITestOutputHelper output, Die.RandomiserReset reset = Die.RandomiserReset.None)
        {
            DispatchRegistry ??= new DispatchRegistry();
            ActionRegistry ??= new ActionRegistry();
            Random ??= new DieBuilder(testName, reset);
            Logger ??= new FakeLogger(output);
            ResourceBuilder ??= new ResourceBuilder(DispatchRegistry, ActionRegistry);
            Builder ??= new RoomBuilder(Random, Logger, DispatchRegistry, ActionRegistry, ResourceBuilder);

            return Builder.BuildRoom(1);
        }

        internal void AssertTest(Room room, ITestOutputHelper output, string expected = "")
        {
            var actual = room.Maze.Print(room.DispatchRegistry);

            output.WriteLine(Divider + " expected " + Divider);
            output.WriteLine(expected);
            output.WriteLine(Divider + " expected " + Divider);
            output.WriteLine(Divider + " actual ==" + Divider);
            output.WriteLine(actual);
            output.WriteLine(Divider + " actual ==" + Divider);

            Assert.Equal(expected, actual);
        }
    }
}