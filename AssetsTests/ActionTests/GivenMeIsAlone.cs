#nullable enable
using AssetsTests.Helpers;
using Utils.Enums;

namespace AssetsTests.ActionTests
{
    public class GivenMeIsAlone : MazeTestHelper
    {
        public GivenMeIsAlone(ITestOutputHelper output)
        : base(output)
        {
        }

        [Theory]
        [InlineData(WalkAloneTest.North, Compass8Points.North, Compass8Points.North, Compass8Points.North, Compass8Points.North, Compass8Points.North, Compass8Points.North, Compass8Points.North)]
        [InlineData(WalkAloneTest.South, Compass8Points.South, Compass8Points.South, Compass8Points.South, Compass8Points.South, Compass8Points.South, Compass8Points.South, Compass8Points.South)]
        [InlineData(WalkAloneTest.East, Compass8Points.East, Compass8Points.East, Compass8Points.East, Compass8Points.East, Compass8Points.East, Compass8Points.East, Compass8Points.East, Compass8Points.East, Compass8Points.East, Compass8Points.East, Compass8Points.East, Compass8Points.East, Compass8Points.East, Compass8Points.East)]
        [InlineData(WalkAloneTest.West, Compass8Points.West, Compass8Points.West, Compass8Points.West, Compass8Points.West, Compass8Points.West, Compass8Points.West, Compass8Points.West, Compass8Points.West, Compass8Points.West, Compass8Points.West, Compass8Points.West, Compass8Points.West)]
        [InlineData(WalkAloneTest.NorthEast, Compass8Points.NorthEast, Compass8Points.NorthEast, Compass8Points.NorthEast, Compass8Points.NorthEast, Compass8Points.NorthEast, Compass8Points.NorthEast, Compass8Points.NorthEast)]
        [InlineData(WalkAloneTest.SouthEast, Compass8Points.SouthEast, Compass8Points.SouthEast, Compass8Points.SouthEast, Compass8Points.SouthEast, Compass8Points.SouthEast, Compass8Points.SouthEast, Compass8Points.SouthEast)]
        [InlineData(WalkAloneTest.MultipleDirections, Compass8Points.West, Compass8Points.NorthWest, Compass8Points.North, Compass8Points.NorthEast, Compass8Points.East, Compass8Points.SouthEast, Compass8Points.South, Compass8Points.SouthWest, Compass8Points.West)]
        public void WhenMeWalks_ThenMeCanWalkThroughSpace_AndNotWalls(WalkAloneTest test, params Compass8Points[] directions)
        {
            var expectations = test.GetExpectations();
            base.TestArrange(expectations);
            ArrangeMovingMeCharacter();

            TestAct();

            AssertTest(GameLevel, expectations);

            void ArrangeMovingMeCharacter()
            {
                foreach (var direction in directions)
                {
                    GameLevel.Dispatcher.EnqueueMove(GameLevel, GameLevel.Me, direction);
                }
            }
        }

        protected override void TestAct()
        {
            Dispatcher.Dispatch();
        }
    }
}
