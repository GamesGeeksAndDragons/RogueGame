using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Assets.Mazes
{
    public struct MazeDetail
    {
        public MazeDetail(int minNumRooms, int maxNumRooms, int tilesPerBlock, int blocksPerRoom)
        {
            MinNumRooms = minNumRooms;
            MaxNumRooms = maxNumRooms;
            TilesPerBlock = tilesPerBlock;
            BlocksPerRoom = blocksPerRoom;
        }

        public int MinNumRooms { get; }
        public int MaxNumRooms { get; }
        public int TilesPerBlock { get; }
        public int BlocksPerRoom { get; }
    }

    public interface IMazeDescriptor
    {
        MazeDetail this[int level] { get; }
    }

    internal class MazeDescriptor : IMazeDescriptor
    {
        // the idea here is to have increasing number of rooms and bigger blocks until level thirty and then just have small blocks and larger bock count per room
        internal readonly Dictionary<int, MazeDetail> MazeDetailByLevel = new Dictionary<int, MazeDetail>
        {
            { 1, new MazeDetail(2,  3, 4, 4)},
            { 2, new MazeDetail(2,  3, 4, 4)},
            { 3, new MazeDetail(2,  4, 4, 4)},
            { 4, new MazeDetail(2,  4, 4, 4)},
            { 5, new MazeDetail(2,  5, 4, 5)},
            { 6, new MazeDetail(3,  5, 4, 5)},
            { 7, new MazeDetail(3,  6, 4, 5)},
            { 8, new MazeDetail(4,  6, 4, 5)},
            { 9, new MazeDetail(4,  7, 4, 6)},
            {10, new MazeDetail(4,  7, 4, 6)},
            {11, new MazeDetail(5,  8, 4, 6)},
            {12, new MazeDetail(5,  9, 4, 6)},
            {13, new MazeDetail(5,  9, 4, 6)},
            {14, new MazeDetail(5,  9, 4, 6)},
            {15, new MazeDetail(6,  9, 4, 6)},
            {16, new MazeDetail(6,  9, 4, 6)},
            {17, new MazeDetail(6,  9, 4, 6)},
            {18, new MazeDetail(6,  9, 4, 6)},
            {19, new MazeDetail(6,  9, 4, 6)},
            {20, new MazeDetail(6, 10, 4, 6)},
            {21, new MazeDetail(6, 10, 4, 6)},
            {22, new MazeDetail(6, 10, 4, 6)},
            {23, new MazeDetail(6, 10, 4, 6)},
            {24, new MazeDetail(6, 10, 4, 6)},
            {25, new MazeDetail(6, 10, 5, 6)},
            {26, new MazeDetail(6, 10, 5, 6)},
            {27, new MazeDetail(6, 10, 5, 6)},
            {28, new MazeDetail(6, 10, 5, 6)},
            {29, new MazeDetail(6, 10, 5, 6)},
            {30, new MazeDetail(6, 11, 4, 7)},
            {31, new MazeDetail(6, 11, 4, 7)},
            {32, new MazeDetail(6, 11, 4, 7)},
            {33, new MazeDetail(6, 11, 4, 7)},
            {34, new MazeDetail(6, 11, 4, 7)},

        };

        public MazeDetail this[int level]
        {
            get
            {
                level.ThrowIfBelow(1, nameof(level));

                if (MazeDetailByLevel.ContainsKey(level))
                {
                    return MazeDetailByLevel[level];
                }

                return MazeDetailByLevel.Values.Last();
            }
        }
    }
}
