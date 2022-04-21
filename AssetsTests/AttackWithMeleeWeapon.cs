using Utils.Random;
using Xunit;
using Xunit.Abstractions;

// https://beej.us/moria/mmspoilers/items.html#weapons

namespace AssetsTests
{
    public class AttackWithMeleeWeapon
    {
        private readonly ITestOutputHelper _output;

        public AttackWithMeleeWeapon(ITestOutputHelper output)
        {
            _output = output;
        }

        internal static IDieBuilder GetGenerator(int testNum)
        {
            var name = $"{nameof(AttackWithMeleeWeapon)}_{testNum}";
            return new DieBuilder(name);
        }

        [Fact (Skip="Need to re-implement")]
        public void AttachingAMonster_WillReduceItsHitPoints()
        {
            Assert.False(true);
        }
    }
}
