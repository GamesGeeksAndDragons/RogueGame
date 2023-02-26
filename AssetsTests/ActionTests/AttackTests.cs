#nullable enable
using Utils.Random;

// https://beej.us/moria/mmspoilers/items.html#weapons

namespace AssetsTests.ActionTests
{
    public class AttackTests
    {
        private readonly ITestOutputHelper _output;

        public AttackTests(ITestOutputHelper output)
        {
            _output = output;
        }

        internal static IDieBuilder GetGenerator(int testNum)
        {
            var name = $"{nameof(AttackTests)}_{testNum}";
            return new DieBuilder(name);
        }

        [Fact(Skip = "Need to re-implement")]
        public void AttachingAMonster_WillReduceItsHitPoints()
        {
            Assert.False(true);
        }
    }
}
