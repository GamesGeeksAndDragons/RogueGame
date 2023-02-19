#nullable enable
using Utils.Display;
using Utils.Random;

namespace Assets.Personas
{
    public interface ICharacterFactory
    {
        ICharacter LoadCharacter(string actor, string state);
        ICharacter RandomCharacter(string actor, int level);
    }

    internal record class CharacterConfig(int Level, int NumberOfInstances, string ArmourClass, string HitPoints);

    internal class CharacterFactory : ICharacterFactory
    {
        private readonly IDieBuilder _dieBuilder;
        private readonly ICharacterBuilder _characterBuilder;

        private readonly Dictionary<string, List<CharacterConfig>> _characterConfig =
            new Dictionary<string, List<CharacterConfig>>();


        public CharacterFactory(IDieBuilder dieBuilder, ICharacterBuilder characterBuilder)
        {
            _dieBuilder = dieBuilder;
            _characterBuilder = characterBuilder;

            var meConfig = new List<CharacterConfig>()
            {
                new CharacterConfig(1, 1, "1B3", "1B3"),
                new CharacterConfig(2, 1, "1B3", "1B3"),
                new CharacterConfig(3, 1, "1B3", "1B3"),
            };
            _characterConfig.Add(CharacterDisplay.Me, meConfig);

            var monsterConfig = new List<CharacterConfig>()
            {
                new CharacterConfig(1, 1, "1B3", "1B3"),
                new CharacterConfig(2, 2, "1B3", "1B3"),
                new CharacterConfig(3, 3, "1B3", "1B3"),
            };
            _characterConfig.Add(CharacterDisplay.DebugMonster, meConfig);
        }

        public ICharacter LoadCharacter(string actor, string state)
        {
            return _characterBuilder.LoadCharacter(actor, state);
        }

        public ICharacter RandomCharacter(string actor, int level)
        {
            var characterConfig = _characterConfig[actor];
            var levelConfig = characterConfig.Single(config => config.Level == level);

            return _characterBuilder.RandomCharacter(actor, levelConfig);
        }
    }
}
