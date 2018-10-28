using System;
using Assets.Mazes;
using Assets.Messaging;
using AssetsTests.Fakes;
using Utils;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.RoomTests
{
    public class StandardRoomBuilderTests
    {
        private readonly ITestOutputHelper _output;

        public StandardRoomBuilderTests(ITestOutputHelper output)
        {
            _output = output;
        }

        internal static int GetNumBlocks(StandardTestRoom testRoom)
        {
            switch (testRoom)
            {
                case StandardTestRoom.First: 
                case StandardTestRoom.Second: return 2;
                case StandardTestRoom.Third:
                case StandardTestRoom.Fourth:
                case StandardTestRoom.Fifth:
                case StandardTestRoom.Sixth:
                    return 3;

                default: throw new ArgumentException($"Didn't have Generator for [{testRoom}]");
            }
        }


        [Theory]
        [InlineData(StandardTestRoom.First)]
        [InlineData(StandardTestRoom.Second)]
        [InlineData(StandardTestRoom.Third)]
        [InlineData(StandardTestRoom.Fourth)]
        [InlineData(StandardTestRoom.Fifth)]
        [InlineData(StandardTestRoom.Sixth)]
        public void BuildRoom_ShouldBuildARoom_WithWalls(StandardTestRoom testRoom)
        {
            var fakeRandomNumbers = new FakeRandomNumberGenerator().PopulateRandomForTestRoom(testRoom);
            var registry = new DispatchRegistry();
            var builder = new RoomBuilder(fakeRandomNumbers, new FakeLogger(_output), registry);

            var room = builder.BuildRoom(GetNumBlocks(testRoom), 4);
            var actual = room.ToString();
            
            var expected = StandardTestRooms.GetExpectation(testRoom);

            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine("Test Room " + testRoom);
            _output.WriteLine(expected);
            _output.WriteLine('='.ToPaddedString(10));
            _output.WriteLine(actual);

            Assert.Equal(expected, actual);
        }
    }
}
