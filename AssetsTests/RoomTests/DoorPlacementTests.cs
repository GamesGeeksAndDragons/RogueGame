using Assets.Rooms;
using Utils;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.RoomTests
{
    public class DoorPlacementTests : RoomTestHelpers
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
            public const string RectangleWithOneDoor = @"
 |0123456| 
-----------
0|╔═════╗|0
1|║¹¹¹¹¹║|1
2|║¹¹¹¹¹║|2
3|1¹¹¹¹¹║|3
4|║¹¹¹¹¹║|4
5|║¹¹¹¹¹║|5
6|║¹¹¹¹¹║|6
7|║¹¹¹¹¹║|7
8|║¹¹¹¹¹║|8
9|╚═════╝|9
-----------
 |0123456| 
";

            public const string SquareWithTwoDoors = @"
 |0123456789| 
--------------
0|╔1═══════╗|0
1|║¹¹¹¹¹¹¹¹║|1
2|║¹¹¹¹¹¹¹¹║|2
3|2¹¹¹¹¹¹¹¹║|3
4|║¹¹¹¹¹¹¹¹║|4
5|║¹¹¹¹¹¹¹¹║|5
6|║¹¹¹¹¹¹¹¹║|6
7|║¹¹¹¹¹¹¹¹║|7
8|║¹¹¹¹¹¹¹¹║|8
9|╚════════╝|9
--------------
 |0123456789| 
";

            public const string LShapedWithThreeDoors = @"
 |0123456789| 
--------------
0|╔1═══╗####|0
1|║¹¹¹¹║####|1
2|║¹¹¹¹║####|2
3|3¹¹¹¹╚══2╗|3
4|║¹¹¹¹¹¹¹¹║|4
5|║¹¹¹¹¹¹¹¹║|5
6|║¹¹¹¹¹¹¹¹║|6
7|║¹¹¹¹¹¹¹¹║|7
8|║¹¹¹¹¹¹¹¹║|8
9|╚════════╝|9
--------------
 |0123456789| 
";

            public const string OShapedWithCappedDoors = @"
  |012345678901|  
------------------
0 |╔6═1═D═7══3╗|0 
1 |9¹¹¹¹¹¹¹¹¹¹║|1 
2 |║¹¹¹¹¹¹¹¹¹¹║|2 
3 |2¹¹¹╔══╗¹¹¹║|3 
4 |║¹¹¹║¹¹║¹¹¹║|4 
5 |║¹¹¹║¹¹¹¹¹¹║|5 
6 |8¹¹¹║¹¹║¹¹¹║|6 
7 |║¹¹¹╚══╝¹¹¹║|7 
8 |A¹¹¹¹¹¹¹¹¹¹║|8 
9 |║¹¹¹¹¹¹¹¹¹¹5|9 
10|╚═════════4╝|10
------------------
  |012345678901|  
";
        }

        private void DoorPlacementTest(int roomIndex, int numDoors, string expectation)
        {
            var room = ArrangeTest(roomIndex, _testName, _output, Die.RandomiserReset.Index);

            for (int i = 1; i <= numDoors; i++)
            {
                room.AddDoor(i);
            }

            AssertTest(room, _output, expectation.Trim(CharHelpers.EndOfLine));
        }

        [Fact]
        public void PlaceOneDoorInSquareRoom_ShouldSuccessfullyPlaceDoor()
        {
            DoorPlacementTest(KnownRooms.Rectangle, 1, DoorPlacementExpectations.RectangleWithOneDoor);
        }

        [Fact]
        public void PlaceTwoDoorsInSquareRoom_ShouldSuccessfullyPlaceDoors()
        {
            DoorPlacementTest(KnownRooms.Square, 2, DoorPlacementExpectations.SquareWithTwoDoors);
        }

        [Fact]
        public void PlaceThreeDoorsInLShapedRoom_ShouldSuccessfullyPlaceDoors()
        {
            DoorPlacementTest(KnownRooms.LShaped, 3, DoorPlacementExpectations.LShapedWithThreeDoors);
        }

        [Fact]
        public void PlaceMoreDoorsThanRoomCanHave_ShouldResultCapTheNumberOfDoorsAdded()
        {
            DoorPlacementTest(KnownRooms.OShaped, 15, DoorPlacementExpectations.OShapedWithCappedDoors);
        }
    }
}