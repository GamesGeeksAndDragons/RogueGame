using AssetsTests.Fakes;
using AssetsTests.Helpers;

namespace AssetsTests.ActionTests
{
    public class TeleportTestsForOneCharacter : MazeTestHelper
    {
        public TeleportTestsForOneCharacter(ITestOutputHelper output) : base(output)
        {
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void WhenHaveDifferingNumbersOfFloorTiles_ShouldTeleportCharacter(int testNum)
        {
            DieBuilder = new FakeDieBuilder(1, testNum-1, 1, 1, 1);
            var expectations = ActionTestsDefinitions.GetExpectations(testNum);

            TestArrange(expectations);
            TestAct();
            TestAssert(expectations);
        }

        protected override void TestAct()
        {
            Dispatcher.Dispatch();
        }
    }

    public class TeleportTestsForTwoCharacters : MazeTestHelper
    {
        public TeleportTestsForTwoCharacters(ITestOutputHelper output) : base(output)
        {
        }

        protected override void TestArrange(IMazeExpectations expectations)
        {
            DieBuilder = new FakeDieBuilder(1, 1);
            base.TestArrange(expectations);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        public void WhenHaveDifferingNumbersOfFloorTiles_ShouldTeleportCharacters(int testNum)
        {
            var expectations = ActionTestsDefinitions.GetExpectations(testNum);

            TestArrange(expectations);
            TestAct();
            TestAssert(expectations);
        }

        protected override void TestAct()
        {
            Dispatcher.Dispatch();
        }
    }
}
