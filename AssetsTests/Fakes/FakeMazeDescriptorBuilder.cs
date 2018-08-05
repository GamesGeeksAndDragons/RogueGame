using Assets.Mazes;

namespace AssetsTests.Fakes
{
    static class FakeMazeDescriptorBuilder
    {
        public static MazeDescriptor Build(int minRooms = 0, int maxRooms = 0, int tileInBlock = 0, int blocksInRoom = 0)
        {
            var descriptor = new MazeDescriptor();
            descriptor.MazeDetailByLevel.Clear();
            descriptor.MazeDetailByLevel.Add(1, new MazeDetail(minRooms, maxRooms, tileInBlock, blocksInRoom));
            return descriptor;
        }
    }
}
