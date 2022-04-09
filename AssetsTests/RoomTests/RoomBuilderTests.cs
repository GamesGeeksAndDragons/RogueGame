using Assets.Rooms;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.RoomTests
{
    public class RoomBuilderTests
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
            public const string Square = @"╔════════╗
║        ║
║        ║
║        ║
║        ║
║        ║
║        ║
║        ║
║        ║
╚════════╝";

            public const string Rectangle = @"╔═════╗
║     ║
║     ║
║     ║
║     ║
║     ║
║     ║
║     ║
║     ║
╚═════╝";

            public const string LShaped = @"╔════╗####
║    ║####
║    ║####
║    ╚═══╗
║        ║
║        ║
║        ║
║        ║
║        ║
╚════════╝";

            public const string OShaped = @"╔══════════╗
║          ║
║          ║
║   ╔══╗   ║
║   ║  ║   ║
║   ║      ║
║   ║  ║   ║
║   ╚══╝   ║
║          ║
║          ║
╚══════════╝";
        }

        [Fact]
        public void Should_BuildASquareRoom_FromAFile()
        {
            var room = RoomTestHelpers.BuildTestRoom(KnownRooms.Square, _testName, _output);

            RoomTestHelpers.AssertTest(room, _output, RoomBuilderExpectations.Square);
        }

        [Fact]
        public void Should_BuildARectangularRoom_FromAFile()
        {
            var room = RoomTestHelpers.BuildTestRoom("Rectangle", _testName, _output);

            RoomTestHelpers.AssertTest(room, _output, RoomBuilderExpectations.Rectangle);
        }

        [Fact]
        public void Should_BuildALRoom_FromAFile()
        {
            var room = RoomTestHelpers.BuildTestRoom("LShaped", _testName, _output);

            RoomTestHelpers.AssertTest(room, _output, RoomBuilderExpectations.LShaped);
        }

        [Fact]
        public void Should_BuildAORoom_FromAFile()
        {
            var room = RoomTestHelpers.BuildTestRoom("OShaped", _testName, _output);

            RoomTestHelpers.AssertTest(room, _output, RoomBuilderExpectations.OShaped);
        }
    }
}
