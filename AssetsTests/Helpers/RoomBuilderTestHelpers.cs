using Assets.Rooms;
using Utils;
using Utils.Random;
using Xunit.Abstractions;

namespace AssetsTests.Helpers
{
    public class RoomBuilderTestHelpers : BuilderTestHelpers
    {
        internal Room Room;
        public RoomBuilderTestHelpers(ITestOutputHelper output, string testName = FileAndDirectoryHelpers.LoadFolder)
        : base(output, testName)
        {
        }

        internal override void ArrangeTest(Die.RandomiserReset reset = Die.RandomiserReset.None)
        {
            base.ArrangeTest(reset);

            Room = RoomBuilder.BuildRoom(1);
        }
    }
}
