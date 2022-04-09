using Assets.Mazes;

namespace AssetsTests.Fakes
{
    static class FakeMazeDescriptorBuilder
    {
        internal static MazeDescriptor Build(int minRooms = 0, int maxRooms = 0)
        {
            var descriptor = new MazeDescriptor();
            descriptor.MazeDetailByLevel.Clear();
            descriptor.MazeDetailByLevel.Add(1, new MazeDetail(minRooms, maxRooms));
            return descriptor;
        }

        public static MazeDescriptor MazeWithTwoRooms()
        {
            return Build(2, 2);
        }

        public static MazeDescriptor MazeWithThreeRooms()
        {
            return Build(3, 3);
        }
    }
}
