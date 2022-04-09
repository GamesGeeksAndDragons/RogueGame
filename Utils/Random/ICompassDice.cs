namespace Utils.Random
{
    public interface ICompassDice<out T> where T : struct
    {
        string Name { get; }
        T Random { get; }
    }
}