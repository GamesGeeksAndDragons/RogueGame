#nullable enable
using Utils.Random;

// https://beej.us/moria/mmspoilers/items.html#weapons

namespace AssetsTests.ActionTests
{
    public class GivenMePlusOneMonster_AndMeAttacks
    {
        private readonly ITestOutputHelper _output;

        public GivenMePlusOneMonster_AndMeAttacks(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [InlineData(AttackTest.MeAttacksOneMonster)]
        public void ThenMeCanKillMonster(AttackTest test)
        {
            //var expectations = test.GetExpectations();
            //base.TestArrange(expectations);
            //ArrangeMovingMeCharacter();
        }
    }
}
