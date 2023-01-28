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
1|║     ║|1
2|║     ║|2
3|║     ║|3
4|║     ║|4
5|║     ║|5
6|║     ║|6
7|║     ║|7
8|║     ║|8
9|╚═════╝|9
-----------
 |0123456| 
";

            public const string SquareWithTwoDoors = @"
 |0123456789| 
--------------
0|╔══2═════╗|0
1|║        ║|1
2|║        ║|2
3|1        ║|3
4|║        ║|4
5|║        ║|5
6|║        ║|6
7|║        ║|7
8|║        ║|8
9|╚════════╝|9
--------------
 |0123456789| 
";

            public const string LShapedWithThreeDoors = @"
 |0123456789| 
--------------
0|╔══2═╗####|0
1|║    ║####|1
2|3    ║####|2
3|1    ╚═══╗|3
4|║        ║|4
5|║        ║|5
6|║        ║|6
7|║        ║|7
8|║        ║|8
9|╚════════╝|9
--------------
 |0123456789| 
";
            public const string LShapedWithDoorsCappedAt12 = @"
 |0123456789| 
--------------
0|╔B729╗####|0
1|4    ║####|1
2|3    ║####|2
3|1    ╚══C╗|3
4|8        ║|4
5|5        ║|5
6|A        ║|6
7|6        ║|7
8|D        ║|8
9|╚════════╝|9
--------------
 |0123456789| 
";

            public const string OShapedWithFifteenDoors = @"
  |012345678901|  
------------------
0 |╔2══DFBC567╗|0 
1 |║          ║|1 
2 |3          ║|2 
3 |1   ╔══╗   ║|3 
4 |9   ║  ║   ║|4 
5 |4   ║      ║|5 
6 |║   ║  ║   ║|6 
7 |E   ╚══╝   ║|7 
8 |8          ║|8 
9 |A          ║|9 
10|╚══════════╝|10
------------------
  |012345678901|  
";
        }

        public void DoorPlacementTest(string roomName, int numDoors, string expectation)
        {
            var room = ArrangeTest(roomName, _testName, _output, Die.RandomiserReset.Index);

            for (int i = 1; i <= numDoors; i++)
            {
                room.AddDoor(i);
            }

            AssertTest(room, _output, expectation.Trim(CharHelpers.EndOfLine));
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