#nullable enable
using Utils;

namespace Assets.Mazes
{
    internal record struct LevelDetail(int MinNumRooms, int MaxNumRooms, int Monst);

    internal interface ILevelDescriptor
    {
        LevelDetail this[int level] { get; }
    }

    internal class LevelDescriptor : ILevelDescriptor
    {
        internal readonly Dictionary<int, LevelDetail> MazeDetailByLevel = new Dictionary<int, LevelDetail>
        {
            { 1, new LevelDetail(1,  1, 1)},
            { 2, new LevelDetail(2,  2, 1)},
            { 3, new LevelDetail(3,  3, 2)},
            { 4, new LevelDetail(2,  4, 2)},
            { 5, new LevelDetail(2,  5, 2)},
            { 6, new LevelDetail(3,  5, 3)},
            { 7, new LevelDetail(3,  6, 3)},
            { 8, new LevelDetail(4,  6, 3)},
            { 9, new LevelDetail(4,  7, 3)},
            {10, new LevelDetail(4,  7, 1)},
            {11, new LevelDetail(5,  8, 1)},
            {12, new LevelDetail(5,  9, 1)},
            {13, new LevelDetail(5,  9, 1)},
            {14, new LevelDetail(5,  9, 1)},
            {15, new LevelDetail(6,  9, 1)},
            {16, new LevelDetail(6,  9, 1)},
            {17, new LevelDetail(6,  9, 1)},
            {18, new LevelDetail(6,  9, 1)},
            {19, new LevelDetail(6,  9, 1)},
            {20, new LevelDetail(6, 10, 1)},
            {21, new LevelDetail(6, 10, 1)},
            {22, new LevelDetail(6, 10, 1)},
            {23, new LevelDetail(6, 10, 1)},
            {24, new LevelDetail(6, 10, 1)},
            {25, new LevelDetail(6, 10, 1)},
            {26, new LevelDetail(6, 10, 1)},
            {27, new LevelDetail(6, 10, 1)},
            {28, new LevelDetail(6, 10, 1)},
            {29, new LevelDetail(6, 10, 1)},
            {30, new LevelDetail(6, 11, 1)},
            {31, new LevelDetail(6, 11, 1)},
            {32, new LevelDetail(6, 11, 1)},
            {33, new LevelDetail(6, 11, 1)},
            {34, new LevelDetail(6, 11, 1)},

        };

        public LevelDetail this[int level]
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
