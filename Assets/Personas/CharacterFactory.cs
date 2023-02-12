#nullable enable
using Utils;
using Utils.Display;
using Utils.Random;

namespace Assets.Personas
{
    public interface ICharacterFactory
    {
        IReadOnlyList<ICharacter> BuildCharacters(int numPersonas);
        ICharacter LoadCharacter(string actor, string state);
    }

    internal class CharacterFactory : ICharacterFactory
    {
        private readonly IDieBuilder _dieBuilder;
        private readonly ICharacterBuilder _characterBuilder;


        public CharacterFactory(IDieBuilder dieBuilder, ICharacterBuilder characterBuilder)
        {
            _dieBuilder = dieBuilder;
            _characterBuilder = characterBuilder;
        }

        private ICharacter LoadMe(string state)
        {
            return _characterBuilder.BuildMe(state);
        }

        public IReadOnlyList<ICharacter> BuildCharacters(int numPersonas)
        {
            var character = LoadMe("");
            var characters = new List<ICharacter> { character };

            numPersonas -= 1;
            for (var i = 0; i < numPersonas; i++)
            {
                character = LoadCharacter("M", "");
                characters.Add(character);
            }

            return characters.AsReadOnly();
        }

        public ICharacter LoadCharacter(string actor, string state)
        {
            if (actor.IsSame(CharacterDisplay.Me))
            {
                return LoadMe(state);
            }
            else
            {
                var character = _characterBuilder.BuildCharacter(state);
                return character;
            }
        }
    }
}
