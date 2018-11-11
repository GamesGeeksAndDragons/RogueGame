using System;
using Assets.Mazes;
using Assets.Messaging;
using AssetsTests.Fakes;
using Utils;
using Utils.Enums;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.MazeTests
{
    public class ConnectTwoRoomsInMaze
    {
        private readonly ITestOutputHelper _output;

        public ConnectTwoRoomsInMaze(ITestOutputHelper output)
        {
            _output = output;
        }

        internal static string GetExpectation(int testNum)
        {
switch (testNum)
{
case 1: return
@"  |01234567890123456789
-----------------------
0 |████████████████████
1 |████████████████████
2 |████████████████████
3 |███╔════╗███████████
4 |███║    ║███████████
5 |███║    ║███████████
6 |███║    ║███████████
7 |███║    ║███████████
8 |███║    ║███████████
9 |███║    ║███████████
10|███║    ║███████████
11|███║    ║███████████
12|███╚═╗1╔╝███████████
13|█████║ ║████████████
14|█████║ ║████████████
15|█████║ ║████████████
16|█████║ ║████████████
17|█████║ ║████████████
18|█████║ ║████████████
19|█████║ ║████████████
20|███╔═╝1╚╗███████████
21|███║    ║███████████
22|███║    ║███████████
23|███║    ║███████████
24|███║    ║███████████
25|███║    ║███████████
26|███║    ║███████████
27|███║    ║███████████
28|███║    ║███████████
29|███╚════╝███████████
30|████████████████████
31|████████████████████
32|████████████████████
33|████████████████████
34|████████████████████
35|████████████████████";
case 2: return
@"  |01234567890123456789
-----------------------
0 |████████████████████
1 |████████████████████
2 |████████████████████
3 |███╔════╗███████████
4 |███║    ║███████████
5 |███║    ║███████████
6 |███║    ║███████████
7 |███║    ║███████████
8 |███║    ║███████████
9 |███║    ║███████████
10|███║    ║███████████
11|███║    ║███████████
12|███╚══1═╝███████████
13|██████ █████████████
14|██████ █████████████
15|██████ █████████████
16|██████ █████████████
17|██████ ███╔════╗████
18|██████ ███║    ║████
19|██████ ███║    ║████
20|██████    1    ║████
21|██████████║    ║████
22|██████████║    ║████
23|██████████║    ║████
24|██████████║    ║████
25|██████████║    ║████
26|██████████╚════╝████
27|████████████████████
28|████████████████████
29|████████████████████
30|████████████████████
31|████████████████████
32|████████████████████
33|████████████████████
34|████████████████████
35|████████████████████";
            }
            throw new ArgumentException($"Didn't have Generator for [{testNum}]");
        }

        [Fact]
        public void SimplestCase_DoorsAboveEachOther()
        {
            var fakeRandomNumbers = new FakeRandomNumberGenerator();

            void SetupTwoRooms()
            {
                fakeRandomNumbers
                    .PopulateRandomForRoomCount(2)
                    .PopulateRandomForTestRoom(StandardTestRoom.First)
                    .PopulateRandomForTestRoom(StandardTestRoom.First)
                    ;
            }

            void SetupDoors()
            {
                fakeRandomNumbers
                    .AddCoordinates(3, 3) // Door placement, empty tile to start walk from
                    .AddDirection(Compass8Points.South) // direction to walk to find a wall and place a door
                    .AddCoordinates(3, 3) // Door placement, empty tile to start walk from
                    .AddDirection(Compass8Points.North) // direction to walk to find a wall and place a door
                    ;
            }

            void SetupForPlacingRooms()
            {
                fakeRandomNumbers.AddCoordinates(3, 3);
                fakeRandomNumbers.AddCoordinates(20, 3);
            }

            var registry = new DispatchRegistry();
            var dispatcher = new Dispatcher(registry);

            SetupTwoRooms();
            SetupDoors();
            SetupForPlacingRooms();

            var fakeLogger = new FakeLogger(_output);
            var mazeDescriptor = FakeMazeDescriptorBuilder.MazeRoomsWithTwoBlocks();

            var builder = new LevelBuilder(fakeRandomNumbers, mazeDescriptor, fakeLogger, dispatcher, registry);
            builder.Build(1, connectTunnels: true);

            var expected = GetExpectation(1);
            var actual = (Maze)registry.GetDispatchee("Maze1");
            var actualString = actual.ToString();

            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actualString);
            _output.WriteLine('='.ToPaddedString(10));

            //Assert.Equal(expected, actualString);
        }

        //[Fact]
        //public void SimplestCase_TwoTunnelsIntersecting()
        //{
        //    var fakeRandomNumbers = new FakeRandomNumberGenerator();

        //    void SetupTwoRooms()
        //    {
        //        fakeRandomNumbers
        //            .PopulateRandomForRoomCount(2)
        //            .PopulateRandomForTestRoom(StandardTestRoom.First)
        //            .PopulateRandomForTestRoom(StandardTestRoom.First)
        //            ;
        //    }

        //    void SetupDoorForFirstRoom()
        //    {
        //        fakeRandomNumbers
        //            .AddCoordinates(3, 3) // Door placement, empty tile to start walk from
        //            .AddDirection(Compass8Points.South) // direction to walk to find a wall and place a door
        //            ;
        //    }

        //    void SetupDoorForSecondRoom()
        //    {
        //        fakeRandomNumbers
        //            .AddCoordinates(3, 3) // Door placement, empty tile to start walk from
        //            .AddDirection(Compass8Points.West) // direction to walk to find a wall and place a door
        //            ;
        //    }

        //    void SetupCoordinatesForPlacingFirstRoom()
        //    {
        //        fakeRandomNumbers.AddCoordinates(3, 3);
        //    }

        //    void SetupCoordinatesForPlacingSecondRoom()
        //    {
        //        fakeRandomNumbers.AddCoordinates(25, 11);
        //    }

        //    var registry = new DispatchRegistry();
        //    var dispatcher = new Dispatcher(registry);

        //    SetupTwoRooms();
        //    SetupDoorForFirstRoom();
        //    SetupDoorForSecondRoom();
        //    SetupCoordinatesForPlacingFirstRoom();
        //    SetupCoordinatesForPlacingSecondRoom();

        //    var fakeLogger = new FakeLogger(_output);
        //    var mazeDescriptor = FakeMazeDescriptorBuilder.MazeRoomsWithTwoBlocks();

        //    var builder = new LevelBuilder(fakeRandomNumbers, mazeDescriptor, fakeLogger, dispatcher, registry);
        //    builder.Build(1, connectTunnels: false);

        //    var expected = GetExpectation(1);
        //    var actual = (Maze)registry.GetDispatchee("Maze1");
        //    var actualString = actual.ToString();

        //    //_output.WriteLine(expected);
        //    _output.WriteLine('='.ToPaddedString(10));
        //    _output.WriteLine(actualString);
        //    _output.WriteLine("Maze (R,C) " + actual.MazeUpperBounds);

        //    Assert.Equal(expected, actualString);
        //}
    }
}
