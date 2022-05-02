using Assets.Characters;
using Assets.Messaging;
using AssetsTests.Helpers;
using Utils;
using Utils.Display;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.ActionTests
{
    public class TeleportTestsForOneCharacter : MazeTestHelper
    {
        public IDispatcher Dispatcher;

        public TeleportTestsForOneCharacter(ITestOutputHelper output) : base(output)
        {
            Dispatcher = new Dispatcher(DispatchRegistry, ActionRegistry);
        }

        protected override void TestArrange(IMazeExpectations expectations)
        {
            base.TestArrange(expectations);

            var meBuilder = ResourceBuilder.MeBuilder();
            var me = meBuilder("");

            Dispatcher.EnqueueTeleport(me);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void WhenHaveDifferingNumbersOfFloorTiles_ShouldTeleportCharacter(int testNum)
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

    public class TeleportTestsForTwoCharacters : MazeTestHelper
    {
        public IDispatcher Dispatcher;

        public TeleportTestsForTwoCharacters(ITestOutputHelper output) : base(output)
        {
            Dispatcher = new Dispatcher(DispatchRegistry, ActionRegistry);
        }

        protected override void TestArrange(IMazeExpectations expectations)
        {
            base.TestArrange(expectations);

            var monsterBuilder = ResourceBuilder.MonsterBuilder();
            var monster = monsterBuilder("");

            Dispatcher.EnqueueTeleport(monster);

            var meBuilder = ResourceBuilder.MeBuilder();
            var me = meBuilder("");

            Dispatcher.EnqueueTeleport(me);
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
