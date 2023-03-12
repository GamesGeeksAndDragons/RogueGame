using Assets.Characters;
using Assets.Level;
using Assets.Resources;
using Utils.Coordinates;

namespace AssetsTests.Helpers
{
    internal class CharactersExpectations : MazeExpectations
    {
        private readonly IGameLevel _gameLevel;
        private List<ICharacter> _characters = new List<ICharacter>();

        public CharactersExpectations(IGameLevel gameLevel)
        {
            _gameLevel = gameLevel;
        }

        void AddCharacter(string character, Coordinate position)
        {
            //var characterFactory = new CharacterFactory(_gameLevel.DispatchRegistry, _gameLevel.)
            var state = position.FormatParameter();

            //_gameLevel.Dispatcher.EnqueueTeleport(_gameLevel, state);
        }
    }
}
