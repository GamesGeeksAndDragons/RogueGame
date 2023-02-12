using Assets.Rooms;
using Utils;
using Utils.Random;

namespace AssetsTests.Helpers
{
    public class RoomBuilderTestHelpers : BuilderTestHelpers
    {
        internal IRoom Room;
        public RoomBuilderTestHelpers(ITestOutputHelper output, string testName = FileAndDirectoryHelpers.LoadFolder)
        : base(output, testName)
        {
        }

        internal override void TestArrange(Die.RandomiserReset reset = Die.RandomiserReset.None)
        {
            base.TestArrange(reset);

            Room = RoomBuilder.BuildRoom(1);
        }
    }
}
