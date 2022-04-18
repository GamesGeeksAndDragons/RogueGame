using System;
using Xunit.Abstractions;

namespace AssetsTests.MazeTests
{
    public class PlaceTwoRoomsInMaze
    {
        private readonly ITestOutputHelper _output;

        public PlaceTwoRoomsInMaze(ITestOutputHelper output)
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
5 |███1    ║███████████
6 |███║    ║███████████
7 |███║    ║███████████
8 |███║    ║███████████
9 |███║    ║███████████
10|███║    ║███████████
11|███║    ║███████████
12|███╚════╝███████████
13|████████████████████
14|████████████████████
15|████████████████████
16|████████████████████
17|██████████╔════╗████
18|██████████║    ║████
19|██████████║    ║████
20|██████████║    1████
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
case 2: return
@"  |0123456789012345678901234567890123456789012345678901234567890123456789
-------------------------------------------------------------------------
0 |██████████████████████████████████████████████████████████████████████
1 |██████████████████████████████████████████████████████████████████████
2 |██████████████████████████████████████████████████████████████████████
3 |███╔════════╗█████████████████████████████████████████████████████████
4 |███║        ║█████████████████████████████████████████████████████████
5 |███║        ║█████████████████████████████████████████████████████████
6 |███║        ║█████████████████████████████████████████████████████████
7 |███║        ║█████████████████████████████████████████████████████████
8 |███╚══1═════╝█████████████████████████████████████████████████████████
9 |██████████████████████████████████████████████████████████████████████
10|██████████████████████████████████████████████████████████████████████
11|██████████████████████████████████████████████████████████████████████
12|██████████████████████████████████████████████████████████████████████
13|██████████████████████████████████████████████████████████████████████
14|██████████████████████████████████████████████████████████████████████
15|██████████████████████████████████████████████████████████████████████
16|██████████████████████████████████████████████████████████████████████
17|███████████████╔═══════1╗█████████████████████████████████████████████
18|███████████████║        ║█████████████████████████████████████████████
19|███████████████║        ║█████████████████████████████████████████████
20|███████████████║        ║█████████████████████████████████████████████
21|███████████████║        ║█████████████████████████████████████████████
22|███████████████╚════════╝█████████████████████████████████████████████
23|██████████████████████████████████████████████████████████████████████
24|██████████████████████████████████████████████████████████████████████
25|██████████████████████████████████████████████████████████████████████
26|██████████████████████████████████████████████████████████████████████
27|██████████████████████████████████████████████████████████████████████
28|██████████████████████████████████████████████████████████████████████
29|██████████████████████████████████████████████████████████████████████
30|██████████████████████████████████████████████████████████████████████
31|██████████████████████████████████████████████████████████████████████
32|██████████████████████████████████████████████████████████████████████
33|██████████████████████████████████████████████████████████████████████
34|██████████████████████████████████████████████████████████████████████
35|██████████████████████████████████████████████████████████████████████
36|██████████████████████████████████████████████████████████████████████
37|██████████████████████████████████████████████████████████████████████";
            }

            throw new ArgumentException($"Didn't have Generator for [{testNum}]");
        }

        //[Fact]
        //public void PlaceTwoRooms_WhereFirstAndSecondDoNotOverlap()
        //{
        //    var fakeRandomNumbers = new DieBuilder();

        //    void SetupTwoRooms()
        //    {
        //        fakeRandomNumbers
        //            .PopulateRandomForRoomCount(2)
        //            .PopulateRandomForTestRoom(StandardTestRoom.First)
        //            .PopulateRandomForTestRoom(StandardTestRoom.First)
        //            ;
        //    }

        //    void SetupDoors()
        //    {
        //        fakeRandomNumbers
        //            .AddCoordinates(2, 2) // Door placement, empty tile to start walk from
        //            .AddDirection(Compass8Points.West) // direction to walk to find a wall and place a door
        //            .AddCoordinates(3, 3) // Door placement, empty tile to start walk from
        //            .AddDirection(Compass8Points.East) // direction to walk to find a wall and place a door
        //            ;
        //    }

        //    void SetupCoordinatesForPlacingFirstRoom()
        //    {
        //        fakeRandomNumbers
        //            .AddCoordinates(1, 1) // too close to edge of maze, should fail
        //            .AddCoordinates(2, 2) // too close to edge of maze, should fail
        //            .AddCoordinates(3, 3) // should succeed
        //            ;
        //    }

        //    void SetupCoordinatesForPlacingSecondRoom()
        //    {
        //        fakeRandomNumbers
        //            .AddCoordinates(4, 4) // overlaps first so should fail
        //            .AddCoordinates(6, 6) // too near first so should fail
        //            .AddCoordinates(17, 10) // does not overlap and not close so should succeed
        //            ;
        //    }

        //    var registry = new DispatchRegistry();
        //    var dispatcher = new Dispatcher(registry);

        //    SetupTwoRooms();
        //    SetupDoors();
        //    SetupCoordinatesForPlacingFirstRoom();
        //    SetupCoordinatesForPlacingSecondRoom();

        //    var fakeLogger = new FakeLogger(_output);
        //    var mazeDescriptor = FakeMazeDescriptorBuilder.MazeWithTwoRooms();

        //    var builder = new LevelBuilder(fakeRandomNumbers, mazeDescriptor, fakeLogger, dispatcher, registry);
        //    builder.Build(1, connectTunnels: false);

        //    var expected = GetExpectation(1);
        //    var actual = (Maze) registry.GetDispatchee("Maze1");
        //    var actualString = actual.ToString();

        //    _output.WriteLine('='.ToPaddedString(10));
        //    _output.WriteLine(expected);
        //    _output.WriteLine('='.ToPaddedString(10));
        //    _output.WriteLine(actualString);
        //    _output.WriteLine('='.ToPaddedString(10));

        //    Assert.Equal(expected, actualString);
        //}

        //[Fact]
        //public void PlaceTwoRooms_WherePlacingTheSecondRoomCausesMazeToGrow()
        //{
        //    var fakeRandomNumbers = new FakeRandomNumberGenerator();

        //    void SetupTwoRooms()
        //    {
        //        fakeRandomNumbers
        //            .PopulateRandomForRoomCount(2)
        //            .PopulateRandomForTestRoom(StandardTestRoom.Second)
        //            .PopulateRandomForTestRoom(StandardTestRoom.Second)
        //            ;
        //    }

        //    void SetupDoorForFirstRoom()
        //    {
        //        fakeRandomNumbers
        //            .AddCoordinates(0, 0) // Door placement, should be empty tile to walk from, but it is a wall, top left corner
        //            .AddCoordinates(3, 3) // Door placement, empty tile to start walk from
        //            .AddDirection(Compass8Points.South) // direction to walk to find a wall and place a door
        //            ;
        //    }

        //    void SetupDoorForSecondRoom()
        //    {
        //        fakeRandomNumbers
        //            .AddCoordinates(5, 9) // Door placement, should be empty tile to walk from, but it is a wall, bottom right corner
        //            .AddCoordinates(4, 8) // Door placement, empty tile to start walk from
        //            .AddDirection(Compass8Points.North) // direction to walk to find a wall and place a door
        //            ;
        //    }

        //    void SetupCoordinatesForPlacingFirstRoom()
        //    {
        //        fakeRandomNumbers
        //            .AddCoordinates(3, 3) // should succeed
        //            ;
        //    }

        //    void SetupCoordinatesForPlacingSecondRoom()
        //    {
        //        fakeRandomNumbers
        //            .AddCoordinates(17, 15) // placing room outside of maze
        //            .AddCoordinates(17, 15) // placing room outside of maze
        //            .AddCoordinates(17, 15) // placing room outside of maze, show grow after third attempt
        //            .AddCoordinates(17, 15) // placing room outside of old maze, now inside new maze so succeeds
        //            ;
        //    }

        //    var registry = new DispatchRegistry();
        //    var dispatcher = new Dispatcher(registry);

        //    SetupTwoRooms();
        //    SetupDoorForFirstRoom();
        //    SetupDoorForSecondRoom();
        //    SetupCoordinatesForPlacingFirstRoom();
        //    SetupCoordinatesForPlacingSecondRoom();

        //    var fakeLogger = new FakeLogger(_output);
        //    var mazeDescriptor = FakeMazeDescriptorBuilder.MazeWithTwoRooms();

        //    var builder = new LevelBuilder(fakeRandomNumbers, mazeDescriptor, fakeLogger, dispatcher, registry);
        //    builder.Build(1, connectTunnels: false);

        //    var expected = GetExpectation(2);
        //    var actual = (Maze) registry.GetDispatchee("Maze1");
        //    var actualString = actual.ToString();

        //    _output.WriteLine('='.ToPaddedString(10));
        //    _output.WriteLine(expected);
        //    _output.WriteLine('='.ToPaddedString(10));
        //    _output.WriteLine(actualString);
        //    _output.WriteLine('='.ToPaddedString(10));

        //    Assert.Equal(expected, actualString);
        //}
    }
}