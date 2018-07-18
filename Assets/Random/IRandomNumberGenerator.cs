namespace Assets.Random
{
    public interface IRandomNumberGenerator
    {
        bool Boolean { get; }

        int Dice(int points);

        T Enum<T>() where T : struct;
    }
}