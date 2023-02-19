#nullable enable
using Utils;

namespace Assets.Level
{
    internal record class LevelDetail(string NumRooms, string RoomSelection, string CharacterDie);

    internal interface ILevelDescriptor
    {
        LevelDetail this[int level] { get; }
    }

    internal class LevelDescriptor : ILevelDescriptor
    {
        internal readonly Dictionary<int, LevelDetail> MazeDetailByLevel = new Dictionary<int, LevelDetail>
        {
            { 1, new LevelDetail("1B1",  "1B1", "1B1")},
            { 2, new LevelDetail("2B2",  "1B2", "2B3")},
            { 3, new LevelDetail("3B3",  "1B3", "3B4")},
            { 4, new LevelDetail("4B4",  "1B4", "3B6")},
            { 5, new LevelDetail("5B5",  "1B4", "3B8")},
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
