namespace Utils.Random;

public static class RandomHelpers
{
    private static readonly System.Random Random = new();
    
    public static int NextGaussian(int mean, int std)
    {
        var u1 = 1.0 - Random.NextDouble(); //uniform(0,1] random doubles
        var u2 = 1.0 - Random.NextDouble();

        var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) *
                            Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)

        var randNormal = (int)(mean + std * randStdNormal); //random normal(mean,stdDev^2)

        return randNormal;
    }

    public static bool RandomBool()
    {
        var r = Random.Next(2);

        return r > 0;
    }

}