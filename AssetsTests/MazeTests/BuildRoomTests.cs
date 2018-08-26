using System;
using Assets.Messaging;
using Assets.Mazes;
using AssetsTests.Fakes;
using Utils;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.MazeTests
{
    public class BuildRoomTests
    {
        private readonly ITestOutputHelper _output;

        public BuildRoomTests(ITestOutputHelper output)
        {
            _output = output;
        }

        internal static int GetLevel(int testNum)
        {
            switch (testNum)
            {
                case 1: 
                case 2: return 2;
                case 3:
                case 4:
                case 5:
                case 6:
                    return 3;

                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }
        }

        internal static string GetExpectation(int testNum)
        {
            switch (testNum)
            {
                case 1:
                    return " |012345" + Environment.NewLine +
                           "--------" + Environment.NewLine +
                           "0|╔════╗" + Environment.NewLine +
                           "1|║    ║" + Environment.NewLine +
                           "2|║    ║" + Environment.NewLine +
                           "3|║    ║" + Environment.NewLine +
                           "4|║    ║" + Environment.NewLine +
                           "5|║    ║" + Environment.NewLine +
                           "6|║    ║" + Environment.NewLine +
                           "7|║    ║" + Environment.NewLine +
                           "8|║    ║" + Environment.NewLine +
                           "9|╚════╝";
                case 2:
                    return " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|╔════════╗" + Environment.NewLine +
                           "1|║        ║" + Environment.NewLine +
                           "2|║        ║" + Environment.NewLine +
                           "3|║        ║" + Environment.NewLine +
                           "4|║        ║" + Environment.NewLine +
                           "5|╚════════╝";
                case 3:
                    return " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|╔════╗████" + Environment.NewLine +
                           "1|║    ║████" + Environment.NewLine +
                           "2|║    ║████" + Environment.NewLine +
                           "3|║    ║████" + Environment.NewLine +
                           "4|║    ╚═══╗" + Environment.NewLine +
                           "5|║        ║" + Environment.NewLine +
                           "6|║        ║" + Environment.NewLine +
                           "7|║        ║" + Environment.NewLine +
                           "8|║        ║" + Environment.NewLine +
                           "9|╚════════╝";
                case 4:
                    return " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|╔════════╗" + Environment.NewLine +
                           "1|║        ║" + Environment.NewLine +
                           "2|║        ║" + Environment.NewLine +
                           "3|║        ║" + Environment.NewLine +
                           "4|║        ║" + Environment.NewLine +
                           "5|╚═══╗    ║" + Environment.NewLine +
                           "6|████║    ║" + Environment.NewLine +
                           "7|████║    ║" + Environment.NewLine +
                           "8|████║    ║" + Environment.NewLine +
                           "9|████╚════╝";
                case 5:
                    return " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|╔════════╗" + Environment.NewLine +
                           "1|║        ║" + Environment.NewLine +
                           "2|║        ║" + Environment.NewLine +
                           "3|║        ║" + Environment.NewLine +
                           "4|║        ║" + Environment.NewLine +
                           "5|║    ╔═══╝" + Environment.NewLine +
                           "6|║    ║████" + Environment.NewLine +
                           "7|║    ║████" + Environment.NewLine +
                           "8|║    ║████" + Environment.NewLine +
                           "9|╚════╝████";
                case 6:
                    return " |0123456789" + Environment.NewLine +
                           "------------" + Environment.NewLine +
                           "0|████╔════╗" + Environment.NewLine +
                           "1|████║    ║" + Environment.NewLine +
                           "2|████║    ║" + Environment.NewLine +
                           "3|████║    ║" + Environment.NewLine +
                           "4|╔═══╝    ║" + Environment.NewLine +
                           "5|║        ║" + Environment.NewLine +
                           "6|║        ║" + Environment.NewLine +
                           "7|║        ║" + Environment.NewLine +
                           "8|║        ║" + Environment.NewLine +
                           "9|╚════════╝";

                default: throw new ArgumentException($"Didn't have Generator for [{testNum}]");
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        public void BuildRoom_ShouldBuildARoom_WithWalls(int testNum)
        {
            var fakeRandomNumbers = new FakeRandomNumberGenerator().AddTestRoom(testNum);
            var registry = new DispatchRegistry();
            var builder = new RoomBuilder(fakeRandomNumbers, new FakeLogger(_output), registry);

            var room = builder.BuildRoom(GetLevel(testNum), 4);
            var actual = room.ToString();
            
            var expected = GetExpectation(testNum);

            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }
    }
}
