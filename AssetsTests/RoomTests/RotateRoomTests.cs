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
1|║        ║|1
2|║        ║|2
3|║        ║|3
4|║        ║|4
5|║        ║|5
6|╚════════╝|6
--------------
 |0123456789| 
";

            public const string LShapedRotatedOnce = @"
 |0123456789| 
--------------
0|╔════════╗|0
1|║        ║|1
2|║        ║|2
3|║        ║|3
4|║        ║|4
5|║     ╔══╝|5
6|║     ║###|6
7|║     ║###|7
8|║     ║###|8
9|╚═════╝###|9
--------------
 |0123456789| 
";

            public const string LShapedRotatedTwice = @"
 |0123456789| 
--------------
0|╔════════╗|0
1|║        ║|1
2|║        ║|2
3|║        ║|3
4|║        ║|4
5|║        ║|5
6|╚═══╗    ║|6
7|####║    ║|7
8|####║    ║|8
9|####╚════╝|9
--------------
 |0123456789| 
";

            public const string LShapedRotatedThreeTimes = @"
 |0123456789| 
--------------
0|###╔═════╗|0
1|###║     ║|1
2|###║     ║|2
3|###║     ║|3
4|╔══╝     ║|4
5|║        ║|5
6|║        ║|6
7|║        ║|7
8|║        ║|8
9|╚════════╝|9
--------------
 |0123456789| 
";

            public const string OShapedRotatedOnce = @"
  |01234567890|  
-----------------
0 |╔═════════╗|0 
1 |║         ║|1 
2 |║         ║|2 
3 |║         ║|3 
4 |║  ╔═══╗  ║|4 
5 |║  ║   ║  ║|5 
6 |║  ║   ║  ║|6 
7 |║  ╚═ ═╝  ║|7 
8 |║         ║|8 
9 |║         ║|9 
10|║         ║|10
11|╚═════════╝|11
-----------------
  |01234567890|  
";

            public const string OShapedRotatedTwice = @"
  |012345678901|  
------------------
0 |╔══════════╗|0 
1 |║          ║|1 
2 |║          ║|2 
3 |║   ╔══╗   ║|3 
4 |║   ║  ║   ║|4 
5 |║      ║   ║|5 
6 |║   ║  ║   ║|6 
7 |║   ╚══╝   ║|7 
8 |║          ║|8 
9 |║          ║|9 
10|╚══════════╝|10
------------------
  |012345678901|  
";

            public const string OShapedRotatedThreeTimes = @"
  |01234567890|  
-----------------
0 |╔═════════╗|0 
1 |║         ║|1 
2 |║         ║|2 
3 |║         ║|3 
4 |║  ╔═ ═╗  ║|4 
5 |║  ║   ║  ║|5 
6 |║  ║   ║  ║|6 
7 |║  ╚═══╝  ║|7 
8 |║         ║|8 
9 |║         ║|9 
10|║         ║|10
11|╚═════════╝|11
-----------------
  |01234567890|  
";
        }

        private void RotateTestImpl(string roomSetup, int rotateTimes, string roomExpectation)
        {
            var room = ArrangeTest(roomSetup, _testName, _output);

            var before = room.Tiles.Print(room.DispatchRegistry);
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