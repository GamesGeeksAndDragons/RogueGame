﻿using Assets.Rooms;
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
1|║        ║|1
2|║        ║|2
3|║        ║|3
4|║        ║|4
5|║        ║|5
6|║        ║|6
7|║        ║|7
8|║        ║|8
9|╚════════╝|9
--------------
 |0123456789| 
";

            public const string Rectangle = @"
 |0123456| 
-----------
0|╔═════╗|0
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

            public const string LShaped = @"
 |0123456789| 
--------------
0|╔════╗####|0
1|║    ║####|1
2|║    ║####|2
3|║    ╚═══╗|3
4|║        ║|4
5|║        ║|5
6|║        ║|6
7|║        ║|7
8|║        ║|8
9|╚════════╝|9
--------------
 |0123456789| 
";

            public const string OShaped = @"
  |012345678901|  
------------------
0 |╔══════════╗|0 
1 |║          ║|1 
2 |║          ║|2 
3 |║   ╔══╗   ║|3 
4 |║   ║  ║   ║|4 
5 |║   ║      ║|5 
6 |║   ║  ║   ║|6 
7 |║   ╚══╝   ║|7 
8 |║          ║|8 
9 |║          ║|9 
10|╚══════════╝|10
------------------
  |012345678901|  
";
        }

        public void Should_BuildARoom_FromAFile(string roomName, string expectedRoom)
        {
            var room = ArrangeTest(roomName, _testName, _output);

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