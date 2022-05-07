#nullable enable
using Assets.Characters;
using Assets.Resources;
using Utils.Random;

namespace Assets.Level
{
    internal class LevelCharacters
    {
        public Me Me { get; }

        private readonly List<ICharacter> _levelCharacters = new List<ICharacter>();

        public LevelCharacters(string meState, IResourceBuilder resourceBuilder, IDieBuilder dieBuilder, LevelDetail levelDetail)
        {
            var meBuilder = resourceBuilder.MeBuilder();
            Me = (Me)meBuilder("");

            var monsterBuilder = resourceBuilder.MonsterBuilder();

            var numMonsters = dieBuilder.Between(levelDetail.MonsterCount).Random;
            for (int i = 0; i < numMonsters; i++)
            {
                var monster = monsterBuilder("");
                _levelCharacters.Add(monster);
            }
        }

        public IEnumerable<ICharacter> Characters()
        {
            foreach (var levelCharacter in _levelCharacters)
            {
                yield return levelCharacter;
            }
        }
    }
}
