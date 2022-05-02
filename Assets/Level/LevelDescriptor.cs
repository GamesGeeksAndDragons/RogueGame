#nullable enable
using Utils;

namespace Assets.Level
{
    internal record struct LevelDetail(string NumRooms, string MonsterCount);

    internal interface ILevelDescriptor
    {
        LevelDetail this[int level] { get; }
    }

    internal class LevelDescriptor : ILevelDescriptor
    {
        internal readonly Dictionary<int, LevelDetail> MazeDetailByLevel = new Dictionary<int, LevelDetail>
        {
            { 1, new LevelDetail("1B1",  "1B1")},
            { 2, new LevelDetail("2B2",  "2B2")},
            { 3, new LevelDetail("3B3",  "3B3")},
            { 4, new LevelDetail("3B4",  "3B4")},
            { 5, new LevelDetail("3B5",  "3B5")},
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
