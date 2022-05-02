using Assets.Rooms;
using Utils;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.RoomTests
{
    public class RoomBuilderTests : RoomTestHelpers
    {
        private readonly ITestOutputHelper _output;
        private readonly string _testName;

        public RoomBuilderTests(ITestOutputHelper output)
        {
            _output = output;
            _testName = nameof(RoomBuilderTests);
        }

        internal static class RoomBuilderExpectations
        {
            public const string Square = @"
 |0123456789| 
--------------
0|╔════════╗|0
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

            public const string Rectangle = @"
 |0123456| 
-----------
0|╔═════╗|0
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

            public const string LShaped = @"
 |0123456789| 
--------------
0|╔════╗####|0
1|║¹¹¹¹║####|1
2|║¹¹¹¹║####|2
3|║¹¹¹¹╚═══╗|3
4|║¹¹¹¹¹¹¹¹║|4
5|║¹¹¹¹¹¹¹¹║|5
6|║¹¹¹¹¹¹¹¹║|6
7|║¹¹¹¹¹¹¹¹║|7
8|║¹¹¹¹¹¹¹¹║|8
9|╚════════╝|9
--------------
 |0123456789| 
";

            public const string OShaped = @"
  |012345678901|  
------------------
0 |╔══════════╗|0 
1 |║¹¹¹¹¹¹¹¹¹¹║|1 
2 |║¹¹¹¹¹¹¹¹¹¹║|2 
3 |║¹¹¹╔══╗¹¹¹║|3 
4 |║¹¹¹║¹¹║¹¹¹║|4 
5 |║¹¹¹║¹¹¹¹¹¹║|5 
6 |║¹¹¹║¹¹║¹¹¹║|6 
7 |║¹¹¹╚══╝¹¹¹║|7 
8 |║¹¹¹¹¹¹¹¹¹¹║|8 
9 |║¹¹¹¹¹¹¹¹¹¹║|9 
10|╚══════════╝|10
------------------
  |012345678901|  
";
        }

        private void Should_BuildARoom_FromAFile(int roomIndex, string expectedRoom)
        {
            var room = ArrangeTest(roomIndex, _testName, _output);

            AssertTest(room, _output, expectedRoom.Trim(CharHelpers.EndOfLine));
        }

        [Fact]
        public void Should_BuildASquareRoom_FromAFile()
        {
            Should_BuildARoom_FromAFile(KnownRooms.Square, RoomBuilderExpectations.Square);
        }

        [Fact]
        public void Should_BuildARectangularRoom_FromAFile()
        {
            Should_BuildARoom_FromAFile(KnownRooms.Rectangle, RoomBuilderExpectations.Rectangle);
        }

        [Fact]
        public void Should_BuildALRoom_FromAFile()
        {
            Should_BuildARoom_FromAFile(KnownRooms.LShaped, RoomBuilderExpectations.LShaped);
        }

        [Fact]
        public void Should_BuildAORoom_FromAFile()
        {
            Should_BuildARoom_FromAFile(KnownRooms.OShaped, RoomBuilderExpectations.OShaped);
        }
    }
}
