using System;
using Assets.Actors;
using Assets.Messaging;
using AssetsTests.Fakes;
using Utils;
using Utils.Coordinates;
using Utils.Enums;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.MazeTests
{
    public class Build2RoomMazes
    {
        private readonly ITestOutputHelper _output;

        public Build2RoomMazes(ITestOutputHelper output)
        {
            _output = output;
        }

        internal static int GetLevel(int testNum)
        {
            switch (testNum)
            {
                case 1:
                case 2:
                    return 1;

                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }
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
                   "2 |██╔════╗████████████" + Environment.NewLine +
                   "3 |██║    ║████████████" + Environment.NewLine +
                   "4 |██1    ║████████████" + Environment.NewLine +
                   "5 |██║    ║████████████" + Environment.NewLine +
                   "6 |██║    ║████████████" + Environment.NewLine +
                   "7 |██║    ║████████████" + Environment.NewLine +
                   "8 |██║    ║████████████" + Environment.NewLine +
                   "9 |██║    ║████████████" + Environment.NewLine +
                   "10|██║    ║████████████" + Environment.NewLine +
                   "11|██╚════╝████████████" + Environment.NewLine +
                   "12|████████████████████" + Environment.NewLine +
                   "13|████████████████████" + Environment.NewLine +
                   "14|████████████████████" + Environment.NewLine +
                   "15|████████████████████" + Environment.NewLine +
                   "16|██████████╔════╗████" + Environment.NewLine +
                   "17|██████████║    ║████" + Environment.NewLine +
                   "18|██████████║    ║████" + Environment.NewLine +
                   "19|██████████║    1████" + Environment.NewLine +
                   "20|██████████║    ║████" + Environment.NewLine +
                   "21|██████████║    ║████" + Environment.NewLine +
                   "22|██████████║    ║████" + Environment.NewLine +
                   "23|██████████║    ║████" + Environment.NewLine +
                   "24|██████████║    ║████" + Environment.NewLine +
                   "25|██████████╚════╝████" + Environment.NewLine +
                   "26|████████████████████" + Environment.NewLine +
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
                    return " |012345" + Environment.NewLine +
                           "--------" + Environment.NewLine +
                           "0|╔════╗" + Environment.NewLine +
                           "1|║    ║" + Environment.NewLine +
                           "2|║ @  ║" + Environment.NewLine +
                           "3|║    ║" + Environment.NewLine +
                           "4|║    ║" + Environment.NewLine +
                           "5|║    ║" + Environment.NewLine +
                           "6|║    ║" + Environment.NewLine +
                           "7|║    ║" + Environment.NewLine +
                           "8|║    ║" + Environment.NewLine +
                           "9|╚════╝";
                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }
        }

        [Fact]
        public void BuildFirstMaze()
        {
            var testNum = 1;
            var registry = new DispatchRegistry();
            var dispatcher = new Dispatcher(registry);

            var fakeRandomNumbers = new FakeRandomNumberGenerator()
                .AddRoomCount(2)
                .AddTestRoom(1)
                .AddTestRoom(1)
                    // 1st Room placement
                    .AddCoordindates(2, 2) // where to walk from to place door
                    .AddDirection(Compass8Points.West) // direction to walk to find a wall and place a door
                    .AddRandomNumbers(0) // index of room to connect to
                    // 2nd Room placement
                    .AddCoordindates(3, 3) // where to walk from to place door
                    .AddDirection(Compass8Points.East) // direction to walk to find a wall and place a door
                    .AddCoordindates(1,1) // where to attempt to place first room, too close to edge of maze, should fail
                    .AddCoordindates(3,3) // where to attempt to place first room, should succeed
                    .AddCoordindates(4,4) // where to attempt to place second room, overlaps first so should fail
                    .AddCoordindates(6,6) // where to attempt to place second room, too near first so should fail
                    .AddCoordindates(17,10) // where to attempt to place second room, does not overlap and not close so should succeed
                    .AddRandomNumbers(2)
                ;
            var fakeLogger = new FakeLogger(_output);
            var mazeDescriptor = FakeMazeDescriptorBuilder.Build(2, 2, 4, 2);

            var builder = new LevelBuilder(fakeRandomNumbers, mazeDescriptor, fakeLogger, dispatcher, registry);
            builder.Build(1);

            var expected = GetExpectation(testNum);
            var actual = registry.GetDispatchee("Maze1").ToString();

 //           _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);

            //Assert.Equal(expected, actual);
        }
    }
}
