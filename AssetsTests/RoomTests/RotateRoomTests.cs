using Assets.Rooms;
using AssetsTests.Helpers;
using Utils;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.RoomTests
{
    public class RotateRoomTests : RoomTestHelpers
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
            public const string RectangleRotated = @"
 |0123456789| 
--------------
0|╔════════╗|0
1|║¹¹¹¹¹¹¹¹║|1
2|║¹¹¹¹¹¹¹¹║|2
3|║¹¹¹¹¹¹¹¹║|3
4|║¹¹¹¹¹¹¹¹║|4
5|║¹¹¹¹¹¹¹¹║|5
6|╚════════╝|6
--------------
 |0123456789| 
";

            public const string LShapedRotatedOnce = @"
 |0123456789| 
--------------
0|╔════════╗|0
1|║¹¹¹¹¹¹¹¹║|1
2|║¹¹¹¹¹¹¹¹║|2
3|║¹¹¹¹¹¹¹¹║|3
4|║¹¹¹¹¹¹¹¹║|4
5|║¹¹¹¹¹╔══╝|5
6|║¹¹¹¹¹║###|6
7|║¹¹¹¹¹║###|7
8|║¹¹¹¹¹║###|8
9|╚═════╝###|9
--------------
 |0123456789| 
";

            public const string LShapedRotatedTwice = @"
 |0123456789| 
--------------
0|╔════════╗|0
1|║¹¹¹¹¹¹¹¹║|1
2|║¹¹¹¹¹¹¹¹║|2
3|║¹¹¹¹¹¹¹¹║|3
4|║¹¹¹¹¹¹¹¹║|4
5|║¹¹¹¹¹¹¹¹║|5
6|╚═══╗¹¹¹¹║|6
7|####║¹¹¹¹║|7
8|####║¹¹¹¹║|8
9|####╚════╝|9
--------------
 |0123456789| 
";

            public const string LShapedRotatedThreeTimes = @"
 |0123456789| 
--------------
0|###╔═════╗|0
1|###║¹¹¹¹¹║|1
2|###║¹¹¹¹¹║|2
3|###║¹¹¹¹¹║|3
4|╔══╝¹¹¹¹¹║|4
5|║¹¹¹¹¹¹¹¹║|5
6|║¹¹¹¹¹¹¹¹║|6
7|║¹¹¹¹¹¹¹¹║|7
8|║¹¹¹¹¹¹¹¹║|8
9|╚════════╝|9
--------------
 |0123456789| 
";

            public const string OShapedRotatedOnce = @"
  |01234567890|  
-----------------
0 |╔═════════╗|0 
1 |║¹¹¹¹¹¹¹¹¹║|1 
2 |║¹¹¹¹¹¹¹¹¹║|2 
3 |║¹¹¹¹¹¹¹¹¹║|3 
4 |║¹¹╔═══╗¹¹║|4 
5 |║¹¹║¹¹¹║¹¹║|5 
6 |║¹¹║¹¹¹║¹¹║|6 
7 |║¹¹╚═¹═╝¹¹║|7 
8 |║¹¹¹¹¹¹¹¹¹║|8 
9 |║¹¹¹¹¹¹¹¹¹║|9 
10|║¹¹¹¹¹¹¹¹¹║|10
11|╚═════════╝|11
-----------------
  |01234567890|  
";

            public const string OShapedRotatedTwice = @"
  |012345678901|  
------------------
0 |╔══════════╗|0 
1 |║¹¹¹¹¹¹¹¹¹¹║|1 
2 |║¹¹¹¹¹¹¹¹¹¹║|2 
3 |║¹¹¹╔══╗¹¹¹║|3 
4 |║¹¹¹║¹¹║¹¹¹║|4 
5 |║¹¹¹¹¹¹║¹¹¹║|5 
6 |║¹¹¹║¹¹║¹¹¹║|6 
7 |║¹¹¹╚══╝¹¹¹║|7 
8 |║¹¹¹¹¹¹¹¹¹¹║|8 
9 |║¹¹¹¹¹¹¹¹¹¹║|9 
10|╚══════════╝|10
------------------
  |012345678901|  
";

            public const string OShapedRotatedThreeTimes = @"
  |01234567890|  
-----------------
0 |╔═════════╗|0 
1 |║¹¹¹¹¹¹¹¹¹║|1 
2 |║¹¹¹¹¹¹¹¹¹║|2 
3 |║¹¹¹¹¹¹¹¹¹║|3 
4 |║¹¹╔═¹═╗¹¹║|4 
5 |║¹¹║¹¹¹║¹¹║|5 
6 |║¹¹║¹¹¹║¹¹║|6 
7 |║¹¹╚═══╝¹¹║|7 
8 |║¹¹¹¹¹¹¹¹¹║|8 
9 |║¹¹¹¹¹¹¹¹¹║|9 
10|║¹¹¹¹¹¹¹¹¹║|10
11|╚═════════╝|11
-----------------
  |01234567890|  
";
        }

        private void RotateTestImpl(int roomIndex, int rotateTimes, string roomExpectation)
        {
            var room = ArrangeTest(roomIndex, _testName, _output);

            var before = room.Maze.Print(room.DispatchRegistry);
            _output.WriteLine(Divider + " before " + Divider);
            _output.WriteLine(before);

            room = Builder.BuildRotatedRoom(room, rotateTimes);

            AssertTest(room, _output, roomExpectation.Trim(CharHelpers.EndOfLine));
        }

        [Fact]
        public void RotateSquareRoomOnce_ShouldMakeNoDifference()
        {
            RotateTestImpl(KnownRooms.Square, 1, RoomBuilderTests.RoomBuilderExpectations.Square);
        }

        [Fact]
        public void RotateRectangleRoomOnce_ShouldRotateTheRoom()
        {
            RotateTestImpl(KnownRooms.Rectangle, 1, RotatedExpectedResults.RectangleRotated);
        }

        [Fact]
        public void RotateRectangleRoomTwice_ShouldMakeNoDifference()
        {
            RotateTestImpl(KnownRooms.Rectangle, 2, RoomBuilderTests.RoomBuilderExpectations.Rectangle);
        }

        [Fact]
        public void RotateLShapedRoomOnce_ShouldRotateTheRoom()
        {
            RotateTestImpl(KnownRooms.LShaped, 1, RotatedExpectedResults.LShapedRotatedOnce);
        }

        [Fact]
        public void RotateLShapedRoomTwice_ShouldRotateTheRoom()
        {
            RotateTestImpl(KnownRooms.LShaped, 2, RotatedExpectedResults.LShapedRotatedTwice);
        }

        [Fact]
        public void RotateLShapedRoomThreeTimes_ShouldRotateTheRoom()
        {
            RotateTestImpl(KnownRooms.LShaped, 3, RotatedExpectedResults.LShapedRotatedThreeTimes);
        }

        [Fact]
        public void RotateOShapedRoomOnce_ShouldRotateTheRoom()
        {
            RotateTestImpl(KnownRooms.OShaped, 1, RotatedExpectedResults.OShapedRotatedOnce);
        }

        [Fact]
        public void RotateOShapedTwice_ShouldRotateTheRoom()
        {
            RotateTestImpl(KnownRooms.OShaped, 2, RotatedExpectedResults.OShapedRotatedTwice);
        }

        [Fact]
        public void RotateOShapedRoomThreeTimes_ShouldRotateTheRoom()
        {
            RotateTestImpl(KnownRooms.OShaped, 3, RotatedExpectedResults.OShapedRotatedThreeTimes);
        }
    }
}