#nullable enable
using Assets.Level;
using Assets.Resources;
using Utils.Random;

namespace Assets.Personas
{
    public interface IPersonasBuilder
    {
        ICharacter BuildMe();
        ICharacter LoadMe(string state);
        IReadOnlyList<ICharacter> BuildCharacters(int numPersonas);
        IReadOnlyList<ICharacter> LoadCharacters(int numPersonas, string state);
    }

    internal class PersonasBuilder : IPersonasBuilder
    {
        private readonly IDieBuilder _dieBuilder;
        private readonly ICharacterBuilder _characterBuilder;

        private Me? _me;
        private readonly List<ICharacter> _characters = new List<ICharacter>();

        public PersonasBuilder(IDieBuilder dieBuilder, ICharacterBuilder characterBuilder)
        {
            _dieBuilder = dieBuilder;
            _characterBuilder = characterBuilder;
        }

        public ICharacter BuildMe()
        {
            return LoadMe("");
        }

        public ICharacter LoadMe(string state)
        {
            _me = (Me)_characterBuilder.BuildMe(state);
            return _me;
        }

        public IReadOnlyList<ICharacter> BuildCharacters(int numPersonas)
        {
            return LoadCharacters(numPersonas, "");
        }

        public IReadOnlyList<ICharacter> LoadCharacters(int numPersonas, string state)
        {
            for (int i = 0; i < numPersonas; i++)
            {
                var character = _characterBuilder.BuildCharacter(state);
                _characters.Add(character);
            }

            return _characters;
        }
    }
}
