using Assets.Rooms;
using AssetsTests.Fakes;
using AssetsTests.Helpers;
using Utils;
using Utils.Display;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.RoomTests
{
    public class DoorPlacementTests : RoomBuilderTestHelpers
    {
        public DoorPlacementTests(ITestOutputHelper output)
        : base(output, nameof(DoorPlacementTests))
        {
        }

        internal static class DoorPlacementExpectations
        {
            public const string RectangleWithOneDoor = @"
 |0123456| 
-----------
0|╔═════╗|0
1|║¹¹¹¹¹║|1
2|║¹¹¹¹¹║|2
3|║¹¹¹¹¹║|3
4|║¹¹¹¹¹1|4
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
0|╔════1══2╗|0
1|║¹¹¹¹¹¹¹¹║|1
2|║¹¹¹¹¹¹¹¹║|2
3|║¹¹¹¹¹¹¹¹║|3
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
0|╔════╗####|0
1|║¹¹¹¹║####|1
2|3¹¹¹¹║####|2
3|║¹¹¹¹╚═══╗|3
4|║¹¹¹¹¹¹¹¹║|4
5|║¹¹¹¹¹¹¹¹║|5
6|║¹¹¹¹¹¹¹¹║|6
7|║¹¹¹¹¹¹¹¹2|7
8|║¹¹¹¹¹¹¹¹║|8
9|╚══1═════╝|9
--------------
 |0123456789| 
";

            public const string OShapedWithCappedDoors = @"
  |012345678901|  
------------------
0 |╔Δ═Γ═3═2═Ε═╗|0 
1 |4¹¹¹¹¹¹¹¹¹¹9|1 
2 |║¹¹¹¹¹¹¹¹¹¹║|2 
3 |Η¹¹¹╔══╗¹¹¹║|3 
4 |║¹¹¹║¹¹║¹¹¹Α|4 
5 |1¹¹¹║¹¹¹¹¹¹║|5 
6 |║¹¹¹║¹¹║¹¹¹║|6 
7 |8¹¹¹╚══╝¹¹¹5|7 
8 |║¹¹¹¹¹¹¹¹¹¹║|8 
9 |Β¹¹¹¹¹¹¹¹¹¹7|9 
10|╚Θ═Κ═6═Ι═Ζ═╝|10
------------------
  |012345678901|  
";
        }

        private void DoorPlacementTest(int roomIndex, int numDoors, string expectation)
        {
            Random = new FakeDieBuilder(4, roomIndex, 1);
            ArrangeTest(Die.RandomiserReset.Index);

            for (int i = 1; i <= numDoors; i++)
            {
                Room.AddDoor(i);
            }

            AssertTest(Room.Maze, expectation.Trim(CharHelpers.EndOfLine));
        }

        [Fact]
        public void PlaceOneDoorInRectangleRoom_ShouldSuccessfullyPlaceDoor()
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
            DoorPlacementTest(KnownRooms.OShaped, TilesDisplay.Doors.Count-1, DoorPlacementExpectations.OShapedWithCappedDoors);
        }
    }
}