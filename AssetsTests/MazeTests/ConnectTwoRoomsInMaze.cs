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
                   "  |01234567890123456789" + Environment.NewLine +
                   "-----------------------" + Environment.NewLine +
                   "0 |████████████████████" + Environment.NewLine +
                   "1 |████████████████████" + Environment.NewLine +
                   "2 |████████████████████" + Environment.NewLine +
                   "3 |███╔════╗███████████" + Environment.NewLine +
                   "4 |███║    ║███████████" + Environment.NewLine +
                   "5 |███║    ║███████████" + Environment.NewLine +
                   "6 |███║    ║███████████" + Environment.NewLine +
                   "7 |███║    ║███████████" + Environment.NewLine +
                   "8 |███║    ║███████████" + Environment.NewLine +
                   "9 |███║    ║███████████" + Environment.NewLine +
                   "10|███║    ║███████████" + Environment.NewLine +
                   "11|███║    ║███████████" + Environment.NewLine +
                   "12|███╚══1═╝███████████" + Environment.NewLine +
                   "13|██████ █████████████" + Environment.NewLine +
                   "14|██████ █████████████" + Environment.NewLine +
                   "15|██████ █████████████" + Environment.NewLine +
                   "16|██████ █████████████" + Environment.NewLine +
                   "3 |███╔══1═╗███████████" + Environment.NewLine +
                   "4 |███║    ║███████████" + Environment.NewLine +
                   "5 |███║    ║███████████" + Environment.NewLine +
                   "6 |███║    ║███████████" + Environment.NewLine +
                   "7 |███║    ║███████████" + Environment.NewLine +
                   "8 |███║    ║███████████" + Environment.NewLine +
                   "9 |███║    ║███████████" + Environment.NewLine +
                   "10|███║    ║███████████" + Environment.NewLine +
                   "11|███║    ║███████████" + Environment.NewLine +
                   "12|███╚════╝███████████" + Environment.NewLine +
                   "27|████████████████████" + Environment.NewLine +
                   "28|████████████████████" + Environment.NewLine +
                   "29|████████████████████" + Environment.NewLine +
                   "30|████████████████████" + Environment.NewLine +
                   "31|████████████████████" + Environment.NewLine +
                   "32|████████████████████" + Environment.NewLine +
                   "33|████████████████████" + Environment.NewLine +
                   "34|████████████████████" + Environment.NewLine +
                   "35|████████████████████"
                   ;

                case 2: return
                    "  |01234567890123456789" + Environment.NewLine +
                   "-----------------------" + Environment.NewLine +
                   "0 |████████████████████" + Environment.NewLine +
                   "1 |████████████████████" + Environment.NewLine +
                   "2 |████████████████████" + Environment.NewLine +
                   "3 |███╔════╗███████████" + Environment.NewLine +
                   "4 |███║    ║███████████" + Environment.NewLine +
                   "5 |███║    ║███████████" + Environment.NewLine +
                   "6 |███║    ║███████████" + Environment.NewLine +
                   "7 |███║    ║███████████" + Environment.NewLine +
                   "8 |███║    ║███████████" + Environment.NewLine +
                   "9 |███║    ║███████████" + Environment.NewLine +
                   "10|███║    ║███████████" + Environment.NewLine +
                   "11|███║    ║███████████" + Environment.NewLine +
                   "12|███╚══1═╝███████████" + Environment.NewLine +
                   "13|██████ █████████████" + Environment.NewLine +
                   "14|██████ █████████████" + Environment.NewLine +
                   "15|██████ █████████████" + Environment.NewLine +
                   "16|██████ █████████████" + Environment.NewLine +
                   "17|██████ ███╔════╗████" + Environment.NewLine +
                   "18|██████ ███║    ║████" + Environment.NewLine +
                   "19|██████ ███║    ║████" + Environment.NewLine +
                   "20|██████    1    ║████" + Environment.NewLine +
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
                   "35|████████████████████"
                   ;
                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }
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

            void SetupDoorForFirstRoom()
            {
                fakeRandomNumbers
                    .AddCoordinates(3, 3) // Door placement, empty tile to start walk from
                    .AddDirection(Compass8Points.South) // direction to walk to find a wall and place a door
                    ;
            }

            void SetupDoorForSecondRoom()
            {
                fakeRandomNumbers
                    .AddCoordinates(3, 3) // Door placement, empty tile to start walk from
                    .AddDirection(Compass8Points.North) // direction to walk to find a wall and place a door
                    ;
            }

            void SetupCoordinatesForPlacingFirstRoom()
            {
                fakeRandomNumbers.AddCoordinates(3, 3);
            }

            void SetupCoordinatesForPlacingSecondRoom()
            {
                fakeRandomNumbers.AddCoordinates(20, 3);
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
            builder.Build(1, connectTunnels: true);

            var expected = GetExpectation(1);
            var actual = (Maze)registry.GetDispatchee("Maze1");
            var actualString = actual.ToString();

            //_output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actualString);
            _output.WriteLine("Maze (R,C) " + actual.MazeUpperBounds);

            Assert.Equal(expected, actualString);
        }

        [Fact]
        public void SimplestCase_TwoTunnelsIntersecting()
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
                    .AddCoordinates(3, 3) // Door placement, empty tile to start walk from
                    .AddDirection(Compass8Points.South) // direction to walk to find a wall and place a door
                    ;
            }

            void SetupDoorForSecondRoom()
            {
                fakeRandomNumbers
                    .AddCoordinates(3, 3) // Door placement, empty tile to start walk from
                    .AddDirection(Compass8Points.West) // direction to walk to find a wall and place a door
                    ;
            }

            void SetupCoordinatesForPlacingFirstRoom()
            {
                fakeRandomNumbers.AddCoordinates(3, 3);
            }

            void SetupCoordinatesForPlacingSecondRoom()
            {
                fakeRandomNumbers.AddCoordinates(25, 11);
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
            builder.Build(1, connectTunnels: false);

            var expected = GetExpectation(1);
            var actual = (Maze)registry.GetDispatchee("Maze1");
            var actualString = actual.ToString();

            //_output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actualString);
            _output.WriteLine("Maze (R,C) " + actual.MazeUpperBounds);

            Assert.Equal(expected, actualString);
        }
    }
}
