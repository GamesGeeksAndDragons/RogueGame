using Utils.Random;

namespace AssetsTests.Fakes
{
    internal sealed class FakeDieBuilder : DieBuilder
    {
        public FakeDieBuilder(int dieNumber, params int[] fakeRandomNumbers)
        {
            var die = new FakeDie(dieNumber, fakeRandomNumbers);
            var dieName = Die.NameFormat(1, dieNumber);
            Dice[dieName] = die;
        }
    }
}
