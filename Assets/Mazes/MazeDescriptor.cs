using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Assets.Mazes
{
    internal struct MazeDetail
    {
        public MazeDetail(int minNumRooms, int maxNumRooms)
        {
            MinNumRooms = minNumRooms;
            MaxNumRooms = maxNumRooms;
        }

        public int MinNumRooms { get; }
        public int MaxNumRooms { get; }
    }

    internal interface IMazeDescriptor
    {
        MazeDetail this[int level] { get; }
    }

    internal class MazeDescriptor : IMazeDescriptor
    {
        // the idea here is to have increasing number of rooms and bigger blocks until level thirty and then just have small blocks and larger bock count per room
        internal readonly Dictionary<int, MazeDetail> MazeDetailByLevel = new Dictionary<int, MazeDetail>
        {
            { 1, new MazeDetail(1,  1)},
            { 2, new MazeDetail(2,  2)},
            { 3, new MazeDetail(3,  3)},
            { 4, new MazeDetail(2,  4)},
            { 5, new MazeDetail(2,  5)},
            { 6, new MazeDetail(3,  5)},
            { 7, new MazeDetail(3,  6)},
            { 8, new MazeDetail(4,  6)},
            { 9, new MazeDetail(4,  7)},
            {10, new MazeDetail(4,  7)},
            {11, new MazeDetail(5,  8)},
            {12, new MazeDetail(5,  9)},
            {13, new MazeDetail(5,  9)},
            {14, new MazeDetail(5,  9)},
            {15, new MazeDetail(6,  9)},
            {16, new MazeDetail(6,  9)},
            {17, new MazeDetail(6,  9)},
            {18, new MazeDetail(6,  9)},
            {19, new MazeDetail(6,  9)},
            {20, new MazeDetail(6, 10)},
            {21, new MazeDetail(6, 10)},
            {22, new MazeDetail(6, 10)},
            {23, new MazeDetail(6, 10)},
            {24, new MazeDetail(6, 10)},
            {25, new MazeDetail(6, 10)},
            {26, new MazeDetail(6, 10)},
            {27, new MazeDetail(6, 10)},
            {28, new MazeDetail(6, 10)},
            {29, new MazeDetail(6, 10)},
            {30, new MazeDetail(6, 11)},
            {31, new MazeDetail(6, 11)},
            {32, new MazeDetail(6, 11)},
            {33, new MazeDetail(6, 11)},
            {34, new MazeDetail(6, 11)},

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
