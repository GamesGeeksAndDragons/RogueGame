using Assets.Messaging;
using Assets.Rooms;
using Utils;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.RoomTests
{
    public class DoorPlacementTests
    {
        private readonly ITestOutputHelper _output;
        private readonly string _testName;

        public DoorPlacementTests(ITestOutputHelper output)
        {
            _output = output;
            _testName = nameof(DoorPlacementTests);
        }

        internal static class DoorPlacementExpectations
        {
            public const string RectangleWithOneDoor = @"╔═════╗
║     ║
║     ║
║     1
║     ║
║     ║
║     ║
║     ║
║     ║
╚═════╝";

            public const string SquareWithTwoDoors = @"╔════════╗
║        ║
║        ║
║        ║
1        ║
║        ║
║        ║
║        ║
║        ║
╚════2═══╝";

            public const string LShapedWithThreeDoors = @"╔════╗####
║    ║####
║    ║####
║    ╚═══╗
║        ║
║        ║
║        3
2        ║
║        ║
╚══1═════╝";
            public const string LShapedWithDoorsCappedAt12 = @"╔C═6═╗####
B    ║####
║    ║####
║    ╚═══╗
5        4
║        ║
║        3
2        ║
║        9
╚8═1═A═7═╝";

            public const string OShapedWithFifteenDoors = @"╔══E═4═3══2╗
║          ║
║          B
9   ╔══╗   ║
║   ║  ║   A
8   ║      ║
║   ║  ║   7
1   ╚══╝   ║
║          ║
║          C
╚═6═F═D═5══╝";
        }

        public void DoorPlacementTest(string roomName, int numDoors, string expectation)
        {
            var room = RoomTestHelpers.BuildTestRoom(roomName, _testName, _output, Die.RandomiserReset.Index);

            for (int i = 1; i <= numDoors; i++)
            {
                room.AddDoor(i);
            }

            RoomTestHelpers.AssertTest(room, _output, expectation);
        }

        [Fact]
        public void PlaceOneDoorInSquareRoom_ShouldSuccessfullyPlaceDoor()
        {
            DoorPlacementTest("Rectangle", 1, DoorPlacementExpectations.RectangleWithOneDoor);
        }

        [Fact]
        public void PlaceTwoDoorsInSquareRoom_ShouldSuccessfullyPlaceDoors()
        {
            DoorPlacementTest("Square", 2, DoorPlacementExpectations.SquareWithTwoDoors);
        }

        [Fact]
        public void PlaceThreeDoorsInLShapedRoom_ShouldSuccessfullyPlaceDoors()
        {
            DoorPlacementTest("LShaped", 3, DoorPlacementExpectations.LShapedWithThreeDoors);
        }

        [Fact]
        public void PlaceFifteenDoorsInOShapedRoom_ShouldSuccessfullyPlaceDoors()
        {
            DoorPlacementTest("OShaped", 15, DoorPlacementExpectations.OShapedWithFifteenDoors);
        }

        [Fact]
        public void PlaceMoreDoorsThanRoomCanHave_ShouldResultCapTheNumberOfDoorsAdded()
        {
            DoorPlacementTest("LShaped", 15, DoorPlacementExpectations.LShapedWithDoorsCappedAt12);
        }
    }
}