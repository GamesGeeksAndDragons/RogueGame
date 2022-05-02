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
0|╔══1══╗|0
1|║¹¹¹¹¹║|1
2|║¹¹¹¹¹║|2
3|║¹¹¹¹¹║|3
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
0|╔══2═════╗|0
1|║¹¹¹¹¹¹¹¹║|1
2|║¹¹¹¹¹¹¹¹║|2
3|1¹¹¹¹¹¹¹¹║|3
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
0|╔══2═╗####|0
1|║¹¹¹¹║####|1
2|3¹¹¹¹║####|2
3|1¹¹¹¹╚═══╗|3
4|║¹¹¹¹¹¹¹¹║|4
5|║¹¹¹¹¹¹¹¹║|5
6|║¹¹¹¹¹¹¹¹║|6
7|║¹¹¹¹¹¹¹¹║|7
8|║¹¹¹¹¹¹¹¹║|8
9|╚════════╝|9
--------------
 |0123456789| 
";
            public const string LShapedWithDoorsCappedAt12 = @"
 |0123456789| 
--------------
0|╔B729╗####|0
1|4¹¹¹¹║####|1
2|3¹¹¹¹║####|2
3|1¹¹¹¹╚══C╗|3
4|8¹¹¹¹¹¹¹¹║|4
5|5¹¹¹¹¹¹¹¹║|5
6|A¹¹¹¹¹¹¹¹║|6
7|6¹¹¹¹¹¹¹¹║|7
8|D¹¹¹¹¹¹¹¹║|8
9|╚════════╝|9
--------------
 |0123456789| 
";

            public const string OShapedWithFifteenDoors = @"
  |012345678901|  
------------------
0 |╔2══DFBC567╗|0 
1 |║¹¹¹¹¹¹¹¹¹¹║|1 
2 |3¹¹¹¹¹¹¹¹¹¹║|2 
3 |1¹¹¹╔══╗¹¹¹║|3 
4 |9¹¹¹║¹¹║¹¹¹║|4 
5 |4¹¹¹║¹¹¹¹¹¹║|5 
6 |║¹¹¹║¹¹║¹¹¹║|6 
7 |E¹¹¹╚══╝¹¹¹║|7 
8 |8¹¹¹¹¹¹¹¹¹¹║|8 
9 |A¹¹¹¹¹¹¹¹¹¹║|9 
10|╚══════════╝|10
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
        public void PlaceFifteenDoorsInOShapedRoom_ShouldSuccessfullyPlaceDoors()
        {
            DoorPlacementTest(KnownRooms.OShaped, 15, DoorPlacementExpectations.OShapedWithFifteenDoors);
        }

        [Fact]
        public void PlaceMoreDoorsThanRoomCanHave_ShouldResultCapTheNumberOfDoorsAdded()
        {
            DoorPlacementTest(KnownRooms.LShaped, 15, DoorPlacementExpectations.LShapedWithDoorsCappedAt12);
        }
    }
}