using Assets.Mazes;

namespace AssetsTests.Fakes
{
    static class FakeMazeDescriptorBuilder
    {
        internal static MazeDescriptor Build(int minRooms = 0, int maxRooms = 0, int tileInBlock = 0, int blocksInRoom = 0)
        {
            var descriptor = new MazeDescriptor();
            descriptor.MazeDetailByLevel.Clear();
            descriptor.MazeDetailByLevel.Add(1, new MazeDetail(minRooms, maxRooms, tileInBlock, blocksInRoom));
            return descriptor;
        }

        public static MazeDescriptor MazeRoomsWithTwoBlocks()
        {
            return Build(2, 2, 4, 2);
        }

        public static MazeDescriptor MazeRoomsWithThreeBlocks()
        {
            return Build(2, 2, 4, 3);
        }
    }
}
