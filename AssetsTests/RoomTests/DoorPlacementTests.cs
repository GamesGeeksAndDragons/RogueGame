using System;
using Assets.Mazes;
using Assets.Messaging;
using AssetsTests.Fakes;
using Utils;
using Utils.Enums;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.RoomTests
{
    public class DoorPlacementTests
    {
        private readonly ITestOutputHelper _output;

        public DoorPlacementTests(ITestOutputHelper output)
        {
            _output = output;
        }

        private string GetExpectationForTestRoom(StandardTestRoom room)
        {
switch (room)
{
case StandardTestRoom.First: return
@" |012345
--------
0|╔══1═╗
1|║    ║
2|║    ║
3|║    ║
4|4    2
5|║    ║
6|║    ║
7|║    ║
8|║    ║
9|╚══3═╝";
case StandardTestRoom.Second: return
@" |0123456789
------------
0|╔══1═════╗
1|║        ║
2|║        ║
3|║        ║
4|4        2
5|╚══3═════╝";
case StandardTestRoom.Third: return
@" |0123456789
------------
0|╔═══1╗████
1|║    ║████
2|║    ║████
3|║    ║████
4|4    ╚═══╗
5|║        2
6|║        ║
7|║        ║
8|║        ║
9|╚════3═══╝";
}

            throw new ArgumentException($"No expectation for test room [{room}]");
        }

        [Theory]
        [InlineData(StandardTestRoom.First)]
        [InlineData(StandardTestRoom.Second)]
        public void PlaceDoor_InRectangularTestRooms(StandardTestRoom testRoom)
        {
            var fakeRandomNumbers = new FakeRandomNumberGenerator();

            void SetupStandardRoom()
            {
                fakeRandomNumbers.PopulateRandomForTestRoom(testRoom)
                    ;
            }

            void SetupDoorForStandardRoom()
            {
                fakeRandomNumbers
                    .AddCoordinates(0, 0) // Door placement, top left corner, will try again
                    .AddCoordinates(9, 0) // Door placement, btm left corner, will try again
                    .AddCoordinates(9, 5) // Door placement, btm right corner, will try again
                    .AddCoordinates(0, 5) // Door placement, top right corner, will try again
                    .AddCoordinates(0, 3) // Door placement, vertical wall, will try again
                    .AddCoordinates(4, 0) // Door placement, horizontal wall, will try again
                    .AddCoordinates(4, 3) // Door placement, vertical wall to start walk from
                    .AddDirection(Compass8Points.North) // direction to walk to find a wall and place a door
                    .AddCoordinates(4, 3) // Door placement, vertical wall to start walk from
                    .AddDirection(Compass8Points.East) // direction to walk to find a wall and place a door
                    .AddCoordinates(4, 3) // Door placement, vertical wall to start walk from
                    .AddDirection(Compass8Points.South) // direction to walk to find a wall and place a door
                    .AddCoordinates(4, 3) // Door placement, vertical wall to start walk from
                    .AddDirection(Compass8Points.West) // direction to walk to find a wall and place a door
                    ;
            }

            var registry = new DispatchRegistry();

            SetupStandardRoom();
            SetupDoorForStandardRoom();

            var fakeLogger = new FakeLogger(_output);

            var builder = new RoomBuilder(fakeRandomNumbers, fakeLogger, registry);

            var mazeDescriptor = FakeMazeDescriptorBuilder.MazeRoomsWithTwoBlocks();
            var detail = mazeDescriptor[1];
            var room = builder.BuildRoom(detail.BlocksPerRoom, detail.TilesPerBlock);
            room = room.AddDoor(1);
            room = room.AddDoor(2);
            room = room.AddDoor(3);
            room = room.AddDoor(4);

            var expected = GetExpectationForTestRoom(testRoom);
            var actual = room.ToString();

            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);
            _output.WriteLine('='.ToPaddedString(10));

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PlaceDoor_InLShapedTestRooms()
        {
            var fakeRandomNumbers = new FakeRandomNumberGenerator();

            void SetupStandardRoom()
            {
                fakeRandomNumbers.PopulateRandomForTestRoom(StandardTestRoom.Third)
                    ;
            }

            void SetupDoorForStandardRoom()
            {
                fakeRandomNumbers
                    // find walls
                    .AddCoordinates(0, 0) // Door placement, top left corner, will try again
                    .AddCoordinates(0, 3) // Door placement, top row, horizontal wall, will try again
                    .AddCoordinates(0, 5) // Door placement, top right corner of L, will try again
                    .AddCoordinates(4, 5) // Door placement, middle corner of L, will try again
                    .AddCoordinates(4, 6) // Door placement, horizontal wall in middle, will try again
                    .AddCoordinates(4, 9) // Door placement, far right corner of L, will try again
                    .AddCoordinates(6, 9) // Door placement, horizontal wall at far right L, will try again
                    .AddCoordinates(9, 9) // Door placement, bottom right corner of L, will try again
                    .AddCoordinates(9, 6) // Door placement, horizontal wall at bottom L, will try again
                    .AddCoordinates(9, 0) // Door placement, bottom left corner of L, will try again
                    .AddCoordinates(9, 5) // Door placement, horizontal wall at far left of L, will try again
                                          //find rocks
                    .AddCoordinates(0, 6) // Door placement, rock outside room, will try again
                    .AddCoordinates(0, 9) // Door placement, rock edge of space, will try again
                    .AddCoordinates(1, 8) // Door placement, rock surrounded by other rocks, will try again
                                          // find corner of L, starting from valid tile
                    .AddCoordinates(4, 4) // Door placement, valid tile
                    .AddDirection(Compass8Points.East) // direction to walk to find a wall and place a door, will find corner and try again
                    .AddCoordinates(5, 5) // Door placement, valid tile
                    .AddDirection(Compass8Points.North) // // direction to walk to find a wall and place a door, will find corner and try again
                                                        // place doors
                    .AddCoordinates(4, 4) // Door placement, vertical wall to start walk from
                    .AddDirection(Compass8Points.North) // direction to walk to find a wall and place a door
                    .AddCoordinates(5, 5) // Door placement, vertical wall to start walk from
                    .AddDirection(Compass8Points.East) // direction to walk to find a wall and place a door
                    .AddCoordinates(5, 5) // Door placement, vertical wall to start walk from
                    .AddDirection(Compass8Points.South) // direction to walk to find a wall and place a door
                    .AddCoordinates(4, 4) // Door placement, vertical wall to start walk from
                    .AddDirection(Compass8Points.West) // direction to walk to find a wall and place a door
                    ;
            }

            var registry = new DispatchRegistry();

            SetupStandardRoom();
            SetupDoorForStandardRoom();

            var fakeLogger = new FakeLogger(_output);

            var builder = new RoomBuilder(fakeRandomNumbers, fakeLogger, registry);

            var mazeDescriptor = FakeMazeDescriptorBuilder.MazeRoomsWithThreeBlocks();
            var detail = mazeDescriptor[1];
            var room = builder.BuildRoom(detail.BlocksPerRoom, detail.TilesPerBlock);
            room = room.AddDoor(1);
            room = room.AddDoor(2);
            room = room.AddDoor(3);
            room = room.AddDoor(4);

            var expected = GetExpectationForTestRoom(StandardTestRoom.Third);
            var actual = room.ToString();

            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);
            _output.WriteLine('='.ToPaddedString(10));

            Assert.Equal(expected, actual);
        }
    }
}