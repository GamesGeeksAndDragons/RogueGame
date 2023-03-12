#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils.Display;
using Utils.Random;

namespace Assets.Props
{
    internal class HandWeaponBuilder
    {
        private readonly IDieBuilder _randomNumbers;
        private readonly DispatchRegistry _dispatchRegistry;
        private readonly ActionRegistry _actionRegistry;

        public HandWeaponBuilder(IDieBuilder randomNumbers, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry)
        {
            _randomNumbers = randomNumbers;
            _dispatchRegistry = dispatchRegistry;
            _actionRegistry = actionRegistry;
        }

        MagicalMeleeWeapon BuildSmallSword()
        {
            return new MagicalMeleeWeapon(_randomNumbers, _dispatchRegistry, _actionRegistry, TilesDisplay.DebugWeapon);
        }
    }
}
