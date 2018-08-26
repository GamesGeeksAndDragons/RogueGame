namespace Utils.Random
{
    public interface IRandomNumberGenerator
    {
        bool Boolean { get; }

        int Dice(int points);

        int Between(int min, int max);

        T Enum<T>() where T : struct;
    }
}