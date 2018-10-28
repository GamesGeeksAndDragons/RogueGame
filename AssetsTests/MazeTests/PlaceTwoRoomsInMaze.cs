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
                case 1:
                    return
                        "  |01234567890123456789" + Environment.NewLine +
                        "-----------------------" + Environment.NewLine +
                        "0 |████████████████████" + Environment.NewLine +
                        "1 |████████████████████" + Environment.NewLine +
                        "2 |████████████████████" + Environment.NewLine +
                        "3 |███╔════╗███████████" + Environment.NewLine +
                        "4 |███║    ║███████████" + Environment.NewLine +
                        "5 |███1    ║███████████" + Environment.NewLine +
                        "6 |███║    ║███████████" + Environment.NewLine +
                        "7 |███║    ║███████████" + Environment.NewLine +
                        "8 |███║    ║███████████" + Environment.NewLine +
                        "9 |███║    ║███████████" + Environment.NewLine +
                        "10|███║    ║███████████" + Environment.NewLine +
                        "11|███║    ║███████████" + Environment.NewLine +
                        "12|███╚════╝███████████" + Environment.NewLine +
                        "13|████████████████████" + Environment.NewLine +
                        "14|████████████████████" + Environment.NewLine +
                        "15|████████████████████" + Environment.NewLine +
                        "16|████████████████████" + Environment.NewLine +
                        "17|██████████╔════╗████" + Environment.NewLine +
                        "18|██████████║    ║████" + Environment.NewLine +
                        "19|██████████║    ║████" + Environment.NewLine +
                        "20|██████████║    1████" + Environment.NewLine +
                        "21|██████████║    ║████" + Environment.NewLine +
                        "22|██████████║    ║████" + Environment.NewLine +
                        "23|██████████║    ║████" + Environment.NewLine +
                        "24|██████████║    ║████" + Environment.NewLine +
                        "25|██████████║    ║████" + Environment.NewLine +
                        "26|██████████╚════╝████" + Environment.NewLine +
                        "27|████████████████████" + Environment.NewLine +
                        "28|████████████████████" + Environment.NewLine +
                        "29|████████████████████" + Environment.NewLine +
                        "30|████████████████████" + Environment.NewLine +
                        "31|████████████████████" + Environment.NewLine +
                        "32|████████████████████" + Environment.NewLine +
                        "33|████████████████████" + Environment.NewLine +
                        "34|████████████████████" + Environment.NewLine +
                        "35|████████████████████";
                    ;
                case 2:
                    return
                        "  |0123456789012345678901234567890123456789012345678901234567890123456789" + Environment.NewLine +
                        "-------------------------------------------------------------------------" + Environment.NewLine +
                        "0 |██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "1 |██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "2 |██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "3 |███╔════════╗█████████████████████████████████████████████████████████" + Environment.NewLine +
                        "4 |███║        ║█████████████████████████████████████████████████████████" + Environment.NewLine +
                        "5 |███║        ║█████████████████████████████████████████████████████████" + Environment.NewLine +
                        "6 |███║        ║█████████████████████████████████████████████████████████" + Environment.NewLine +
                        "7 |███║        ║█████████████████████████████████████████████████████████" + Environment.NewLine +
                        "8 |███╚══1═════╝█████████████████████████████████████████████████████████" + Environment.NewLine +
                        "9 |██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "10|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "11|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "12|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "13|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "14|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "15|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "16|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "17|███████████████╔═══════1╗█████████████████████████████████████████████" + Environment.NewLine +
                        "18|███████████████║        ║█████████████████████████████████████████████" + Environment.NewLine +
                        "19|███████████████║        ║█████████████████████████████████████████████" + Environment.NewLine +
                        "20|███████████████║        ║█████████████████████████████████████████████" + Environment.NewLine +
                        "21|███████████████║        ║█████████████████████████████████████████████" + Environment.NewLine +
                        "22|███████████████╚════════╝█████████████████████████████████████████████" + Environment.NewLine +
                        "23|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "24|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "25|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "26|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "27|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "28|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "29|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "30|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "31|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "32|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "33|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "34|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "35|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "36|██████████████████████████████████████████████████████████████████████" + Environment.NewLine +
                        "37|██████████████████████████████████████████████████████████████████████" 
                        ;
                case 3:
                    return
                        "  |012345678901234567890123456789012345" + Environment.NewLine +
                        "---------------------------------------" + Environment.NewLine +
                        "0 |████████████████████████████████████" + Environment.NewLine +
                        "1 |████████████████████████████████████" + Environment.NewLine +
                        "2 |████████████████████████████████████" + Environment.NewLine +
                        "3 |███╔════╗███████████████████████████" + Environment.NewLine +
                        "4 |███║    ║███████████████████████████" + Environment.NewLine +
                        "5 |███║    ║███████████████████████████" + Environment.NewLine +
                        "6 |███║    ║███████████████████████████" + Environment.NewLine +
                        "7 |███║    ╚═══╗███████████████████████" + Environment.NewLine +
                        "8 |███║        ║███████████████████████" + Environment.NewLine +
                        "9 |███║        ║███████████████████████" + Environment.NewLine +
                        "10|███║        ║███████████████████████" + Environment.NewLine +
                        "11|███║        ║███████████████████████" + Environment.NewLine +
                        "12|███╚════1═══╝███████████████████████" + Environment.NewLine +
                        "13|████████████████████████████████████" + Environment.NewLine +
                        "14|████████████████████████████████████" + Environment.NewLine +
                        "15|████████████████████████████████████" + Environment.NewLine +
                        "16|████████████████████████████████████" + Environment.NewLine +
                        "17|███████████████╔════════╗███████████" + Environment.NewLine +
                        "18|███████████████║        ║███████████" + Environment.NewLine +
                        "19|███████████████║        ║███████████" + Environment.NewLine +
                        "20|███████████████1        ║███████████" + Environment.NewLine +
                        "21|███████████████║        ║███████████" + Environment.NewLine +
                        "22|███████████████╚═══╗    ║███████████" + Environment.NewLine +
                        "23|███████████████████║    ║███████████" + Environment.NewLine +
                        "24|███████████████████║    ║███████████" + Environment.NewLine +
                        "25|███████████████████║    ║███████████" + Environment.NewLine +
                        "26|███████████████████╚════╝███████████" + Environment.NewLine +
                        "27|████████████████████████████████████" + Environment.NewLine +
                        "28|████████████████████████████████████" + Environment.NewLine +
                        "29|████████████████████████████████████" + Environment.NewLine +
                        "30|████████████████████████████████████" + Environment.NewLine +
                        "31|████████████████████████████████████" + Environment.NewLine +
                        "32|████████████████████████████████████" + Environment.NewLine +
                        "33|████████████████████████████████████" + Environment.NewLine +
                        "34|████████████████████████████████████" + Environment.NewLine +
                        "35|████████████████████████████████████"
                    ;
                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }
        }

        [Fact]
        public void PlaceTwoRooms_WhereFirstAndSecondDoNotOverlap()
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

            void SetupDoorForFirstRoom()
            {
                fakeRandomNumbers
                    .AddCoordinates(0, 0) // Door placement, should be empty tile to walk from, but it is a wall, top left corner
                    .AddCoordinates(2, 2) // Door placement, empty tile to start walk from
                    .AddDirection(Compass8Points.West) // direction to walk to find a wall and place a door
                    ;
            }

            void SetupDoorForSecondRoom()
            {
                fakeRandomNumbers
                    .AddCoordinates(5, 9) // Door placement, should be empty tile to walk from, but it is a wall, bottom right corner
                    .AddCoordinates(3, 3) // Door placement, empty tile to start walk from
                    .AddDirection(Compass8Points.East) // direction to walk to find a wall and place a door
                    ;
            }

            void SetupCoordinatesForPlacingFirstRoom()
            {
                fakeRandomNumbers
                    .AddCoordinates(1, 1) // too close to edge of maze, should fail
                    .AddCoordinates(2, 2) // too close to edge of maze, should fail
                    .AddCoordinates(3, 3) // should succeed
                    ;
            }

            void SetupCoordinatesForPlacingSecondRoom()
            {
                fakeRandomNumbers
                    .AddCoordinates(4, 4) // overlaps first so should fail
                    .AddCoordinates(6, 6) // too near first so should fail
                    .AddCoordinates(17, 10) // does not overlap and not close so should succeed
                    ;
            }

            var registry = new DispatchRegistry();
            var dispatcher = new Dispatcher(registry);

            SetupTwoRooms();
            SetupDoorForFirstRoom();
            SetupDoorForSecondRoom();
            SetupCoordinatesForPlacingFirstRoom();
            SetupCoordinatesForPlacingSecondRoom();

            var fakeLogger = new FakeLogger(_output);
            var mazeDescriptor = FakeMazeDescriptorBuilder.MazeRoomsWithTwoBlocks();

            var builder = new LevelBuilder(fakeRandomNumbers, mazeDescriptor, fakeLogger, dispatcher, registry);
            builder.Build(1, false);

            var expected = GetExpectation(1);
            var actual = (Maze) registry.GetDispatchee("Maze1");
            var actualString = actual.ToString();

            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actualString);
            _output.WriteLine("Maze (R,C) " + actual.MazeUpperBounds);

            Assert.Equal(expected, actualString);
        }

        [Fact]
        public void PlaceTwoRooms_WherePlacingTheSecondRoomCausesMazeToGrow()
        {
            var fakeRandomNumbers = new FakeRandomNumberGenerator();

            void SetupTwoRooms()
            {
                fakeRandomNumbers
                    .PopulateRandomForRoomCount(2)
                    .PopulateRandomForTestRoom(StandardTestRoom.Second)
                    .PopulateRandomForTestRoom(StandardTestRoom.Second)
                    ;
            }

            void SetupDoorForFirstRoom()
            {
                fakeRandomNumbers
                    .AddCoordinates(0,
                        0) // Door placement, should be empty tile to walk from, but it is a wall, top left corner
                    .AddCoordinates(3, 3) // Door placement, empty tile to start walk from
                    .AddDirection(Compass8Points.South) // direction to walk to find a wall and place a door
                    ;
            }

            void SetupDoorForSecondRoom()
            {
                fakeRandomNumbers
                    .AddCoordinates(5, 9) // Door placement, should be empty tile to walk from, but it is a wall, bottom right corner
                    .AddCoordinates(4, 8) // Door placement, empty tile to start walk from
                    .AddDirection(Compass8Points.North) // direction to walk to find a wall and place a door
                    ;
            }

            void SetupCoordinatesForPlacingFirstRoom()
            {
                fakeRandomNumbers
                    .AddCoordinates(3, 3) // should succeed
                    ;
            }

            void SetupCoordinatesForPlacingSecondRoom()
            {
                fakeRandomNumbers
                    .AddCoordinates(17, 15) // placing room outside of maze
                    .AddCoordinates(17, 15) // placing room outside of maze
                    .AddCoordinates(17, 15) // placing room outside of maze, show grow after third attempt
                    .AddCoordinates(17, 15) // placing room outside of old maze, now inside new maze so succeeds
                    ;
            }

            var registry = new DispatchRegistry();
            var dispatcher = new Dispatcher(registry);

            SetupTwoRooms();
            SetupDoorForFirstRoom();
            SetupDoorForSecondRoom();
            SetupCoordinatesForPlacingFirstRoom();
            SetupCoordinatesForPlacingSecondRoom();

            var fakeLogger = new FakeLogger(_output);
            var mazeDescriptor = FakeMazeDescriptorBuilder.MazeRoomsWithTwoBlocks();

            var builder = new LevelBuilder(fakeRandomNumbers, mazeDescriptor, fakeLogger, dispatcher, registry);
            builder.Build(1, false);

            var expected = GetExpectation(2);
            var actual = (Maze) registry.GetDispatchee("Maze1");
            var actualString = actual.ToString();

            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actualString);
            _output.WriteLine("Maze (R,C) " + actual.MazeUpperBounds);

            Assert.Equal(expected, actualString);
        }

        [Fact]
        public void PlaceTwoRooms_WhereDoorPlacementRetriesAsTriedToPutInCorner()
        {
            var fakeRandomNumbers = new FakeRandomNumberGenerator();

            void SetupTwoRooms()
            {
                fakeRandomNumbers
                    .PopulateRandomForRoomCount(2)
                    .PopulateRandomForTestRoom(StandardTestRoom.Third)
                    .PopulateRandomForTestRoom(StandardTestRoom.Fourth)
                    ;
            }

            void SetupDoorForFirstRoom()
            {
                fakeRandomNumbers
                    .AddCoordinates(6, 5) // Door placement, empty tile to start walk from
                    .AddDirection(Compass8Points.North) // direction to walk will find corner and retry
                    .AddCoordinates(4, 4) // Door placement, empty tile to start walk from
                    .AddDirection(Compass8Points.East) // // direction to walk will find corner and retry
                    .AddCoordinates(6, 5) // Door placement, empty tile to start walk from
                    .AddDirection(Compass8Points.South) // will find wall and place door
                    ;
            }

            void SetupDoorForSecondRoom()
            {
                fakeRandomNumbers
                    .AddCoordinates(3, 4) // Door placement, empty tile to start walk from
                    .AddDirection(Compass8Points.South) // direction to walk to will find corner and retry
                    .AddCoordinates(5, 7) // Door placement, empty tile to start walk from
                    .AddDirection(Compass8Points.West) // direction to walk to will find corner and retry
                    .AddCoordinates(3, 4) // Door placement, empty tile to start walk from
                    .AddDirection(Compass8Points.West) // will find wall and place door
                    ;
            }

            void SetupCoordinatesForPlacingFirstRoom()
            {
                fakeRandomNumbers
                    .AddCoordinates(3, 3) // should succeed
                    ;
            }

            void SetupCoordinatesForPlacingSecondRoom()
            {
                fakeRandomNumbers
                    .AddCoordinates(17, 15) // placing room with one part outside of maze
                    .AddCoordinates(17, 15) // placing room with one part outside of maze
                    .AddCoordinates(17, 15) // placing room with one part outside of maze, show grow after third attempt
                    .AddCoordinates(17, 15) // placing room outside of old maze, now inside new maze so succeeds
                    ;
            }

            var registry = new DispatchRegistry();
            var dispatcher = new Dispatcher(registry);

            SetupTwoRooms();
            SetupDoorForFirstRoom();
            SetupDoorForSecondRoom();
            SetupCoordinatesForPlacingFirstRoom();
            SetupCoordinatesForPlacingSecondRoom();

            var fakeLogger = new FakeLogger(_output);
            var mazeDescriptor = FakeMazeDescriptorBuilder.MazeRoomsWithThreeBlocks();

            var builder = new LevelBuilder(fakeRandomNumbers, mazeDescriptor, fakeLogger, dispatcher, registry);
            builder.Build(1, false);

            var expected = GetExpectation(3);
            var actual = (Maze) registry.GetDispatchee("Maze1");
            var actualString = actual.ToString();

            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actualString);
            _output.WriteLine("Maze (R,C) " + actual.MazeUpperBounds);

            Assert.Equal(expected, actualString);
        }
    }
}