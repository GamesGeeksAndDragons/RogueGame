#nullable enable
using Utils.Enums;

namespace Utils.Random
{
    public interface IDieBuilder
    {
        IDice D2 { get; }
        IDice D3 { get; }
        IDice D4 { get; }
        IDice D5 { get; }
        IDice D6 { get; }
        IDice D7 { get; }
        IDice D8 { get; }
        IDice D9 { get; }
        IDice D10 { get; }
        IDice D12 { get; }
        IDice D20 { get; }
        IDice Dice(int max);
        IDice Between(int min, int max);
        IDice Between(string between);

        ICompassDice<Compass4Points> Compass4Die { get; }
        ICompassDice<Compass8Points> Compass8Die { get; }

        void NextTurn();
    }
}