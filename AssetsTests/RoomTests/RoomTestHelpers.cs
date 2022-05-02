using Assets.Deeds;
using Assets.Messaging;
using Assets.Resources;
using Assets.Rooms;
using Assets.Tiles;
using AssetsTests.Fakes;
using AssetsTests.Helpers;
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

        internal Room ArrangeTest(string roomName, string testName, ITestOutputHelper output, Die.RandomiserReset reset = Die.RandomiserReset.None)
        {
            var dispatchRegistry = new DispatchRegistry();
            var actionRegistry = new ActionRegistry();
            var random = new DieBuilder(loadFolder: testName, reset: reset);
            var logger = new FakeLogger(output);
            var actorBuilder = new ResourceBuilder(dispatchRegistry, actionRegistry);

            Builder = new RoomBuilder(random, logger, dispatchRegistry, actionRegistry, actorBuilder);
            return Builder.BuildRoom(roomName, 1);
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