#nullable enable
using Assets.Characters;
using Assets.Resources;
using Utils.Random;

namespace Assets.Level
{
    internal class LevelCharacters
    {
        private readonly Func<string, ICharacter> _meBuilder;
        private readonly Func<string, ICharacter> _monsterBuilder;
        private readonly int _monsterCount;

        public LevelCharacters(IResourceBuilder resourceBuilder, IDieBuilder dieBuilder, LevelDetail levelDetail)
        {
            _meBuilder = resourceBuilder.MeBuilder();
            _monsterBuilder = resourceBuilder.MonsterBuilder();
            _monsterCount = dieBuilder.Between(levelDetail.MonsterCount).Random;
        }

        public Me BuildMe(string state)
        {
            return (Me)_meBuilder(state);
        }

        public IReadOnlyList<ICharacter> BuildCharacters()
        {
            var monsters = new List<ICharacter>();

            for (int i = 0; i < _monsterCount; i++)
            {
                var monster = _monsterBuilder("");
                monsters.Add(monster);
            }

            return monsters;
        }
    }
}
