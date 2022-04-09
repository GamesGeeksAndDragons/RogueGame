using Assets.Rooms;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.RoomTests
{
    public class RotateRoomTests
    {
        private readonly ITestOutputHelper _output;
        private readonly string _testName;

        public RotateRoomTests(ITestOutputHelper output)
        {
            _output = output;
            _testName = nameof(RotateRoomTests);
        }

        static class RotatedExpectedResults
        {
            public const string RectangleRotated = @"╔════════╗
║        ║
║        ║
║        ║
║        ║
║        ║
╚════════╝";

            public const string LShapedRotatedOnce = @"╔════════╗
║        ║
║        ║
║        ║
║        ║
║     ╔══╝
║     ║###
║     ║###
║     ║###
╚═════╝###";

            public const string LShapedRotatedTwice = @"╔════════╗
║        ║
║        ║
║        ║
║        ║
║        ║
╚═══╗    ║
####║    ║
####║    ║
####╚════╝";

            public const string LShapedRotatedThreeTimes = @"###╔═════╗
###║     ║
###║     ║
###║     ║
╔══╝     ║
║        ║
║        ║
║        ║
║        ║
╚════════╝";

            public const string OShapedRotatedOnce = @"╔═════════╗
║         ║
║         ║
║         ║
║  ╔═══╗  ║
║  ║   ║  ║
║  ║   ║  ║
║  ╚═ ═╝  ║
║         ║
║         ║
║         ║
╚═════════╝";

            public const string OShapedRotatedTwice = @"╔══════════╗
║          ║
║          ║
║   ╔══╗   ║
║   ║  ║   ║
║      ║   ║
║   ║  ║   ║
║   ╚══╝   ║
║          ║
║          ║
╚══════════╝";

            public const string OShapedRotatedThreeTimes = @"╔═════════╗
║         ║
║         ║
║         ║
║  ╔═ ═╗  ║
║  ║   ║  ║
║  ║   ║  ║
║  ╚═══╝  ║
║         ║
║         ║
║         ║
╚═════════╝";
        }

        [Fact]
        public void RotateSquareRoomOnce_ShouldMakeNoDifference()
        {
            var room = RoomTestHelpers.BuildTestRoom(KnownRooms.Square, _testName, _output);
            room = room.Rotate(1);

            RoomTestHelpers.AssertTest(room, _output, RoomBuilderTests.RoomBuilderExpectations.Square);
        }

        [Fact]
        public void RotateRectangleRoomOnce_ShouldRotateTheRoom()
        {
            var room = RoomTestHelpers.BuildTestRoom(KnownRooms.Rectangle, _testName, _output);
            room = room.Rotate(1);

            RoomTestHelpers.AssertTest(room, _output, RotatedExpectedResults.RectangleRotated);
        }

        [Fact]
        public void RotateRectangleRoomTwice_ShouldMakeNoDifference()
        {
            var room = RoomTestHelpers.BuildTestRoom(KnownRooms.Rectangle, _testName, _output);
            room = room.Rotate(2);

            RoomTestHelpers.AssertTest(room, _output, RoomBuilderTests.RoomBuilderExpectations.Rectangle);
        }

        [Fact]
        public void RotateLShapedRoomOnce_ShouldRotateTheRoom()
        {
            var room = RoomTestHelpers.BuildTestRoom(KnownRooms.LShaped, _testName, _output);

            room = room.Rotate(1);

            RoomTestHelpers.AssertTest(room, _output, RotatedExpectedResults.LShapedRotatedOnce);
        }

        [Fact]
        public void RotateLShapedRoomTwice_ShouldRotateTheRoom()
        {
            var room = RoomTestHelpers.BuildTestRoom(KnownRooms.LShaped, _testName, _output);

            room = room.Rotate(2);

            RoomTestHelpers.AssertTest(room, _output, RotatedExpectedResults.LShapedRotatedTwice);
        }

        [Fact]
        public void RotateLShapedRoomThreeTimes_ShouldRotateTheRoom()
        {
            var room = RoomTestHelpers.BuildTestRoom(KnownRooms.LShaped, _testName, _output);

            room = room.Rotate(3);

            RoomTestHelpers.AssertTest(room, _output, RotatedExpectedResults.LShapedRotatedThreeTimes);
        }

        [Fact]
        public void RotateOShapedRoomOnce_ShouldRotateTheRoom()
        {
            var room = RoomTestHelpers.BuildTestRoom(KnownRooms.OShaped, _testName, _output);
            room = room.Rotate(1);

            RoomTestHelpers.AssertTest(room, _output, RotatedExpectedResults.OShapedRotatedOnce);
        }

        [Fact]
        public void RotateOShapedTwice_ShouldRotateTheRoom()
        {
            var room = RoomTestHelpers.BuildTestRoom(KnownRooms.OShaped, _testName, _output);
            room = room.Rotate(2);

            RoomTestHelpers.AssertTest(room, _output, RotatedExpectedResults.OShapedRotatedTwice);
        }

        [Fact]
        public void RotateOShapedRoomThreeTimes_ShouldRotateTheRoom()
        {
            var room = RoomTestHelpers.BuildTestRoom(KnownRooms.OShaped, _testName, _output);
            room = room.Rotate(3);

            RoomTestHelpers.AssertTest(room, _output, RotatedExpectedResults.OShapedRotatedThreeTimes);
        }
    }
}