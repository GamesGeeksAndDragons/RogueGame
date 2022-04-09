using Assets.Deeds;
using Assets.Messaging;
using Assets.Rooms;
using AssetsTests.Fakes;
using Utils;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.RoomTests
{
    public static class RoomTestHelpers
    {
        public static string Divider = '='.ToPaddedString(10);

        internal static Room BuildTestRoom(string roomName, string testName, ITestOutputHelper output, Die.RandomiserReset reset = Die.RandomiserReset.None)
        {
            var dispatchRegistry = new DispatchRegistry();
            var actionRegistry = new ActionRegistry();
            var random = new DieBuilder(loadFolder: testName, reset: reset);
            var logger = new FakeLogger(output);

            var builder = new RoomBuilder(random, logger, dispatchRegistry, actionRegistry);
            return builder.BuildRoom(roomName);
        }

        internal static void AssertTest(Room room, ITestOutputHelper output, string expected = "")
        {
            var actual = room.ToString();

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