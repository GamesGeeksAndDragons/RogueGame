#nullable enable
using Utils.Enums;

namespace Utils.Random
{
    public interface IDieBuilder
    {
        IDice Between(int min, int max);
        IDice Between(string between);

        ICompassDice<Compass4Points> Compass4Die { get; }
        ICompassDice<Compass8Points> Compass8Die { get; }

        void NextTurn();
    }
}