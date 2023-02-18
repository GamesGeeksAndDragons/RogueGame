#nullable enable
using AssetsTests.Helpers;
using Utils.Enums;

namespace AssetsTests.ActionTests
{
    public class WalkingTests : MazeTestHelper
    {
        public WalkingTests(ITestOutputHelper output)
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
        public void WhenIWalkAlone_ICanWalkThroughSpace_ButNotWalls(WalkAloneTest test, params Compass8Points[] directions)
        {
            var expectations = test.GetExpectations();
            base.TestArrange(expectations);
            ArrangeMovingMeCharacter();

            TestAct();

            AssertTest(GameLevel, expectations);

            void ArrangeMovingMeCharacter()
            {
                GameLevel.Dispatcher.Dispatch();

                GameLevelBuilder.AddCharacter(GameLevel, expectations.Me);

                var maze = GameLevel.Print(DispatchRegistry);
                Output.WriteLine(BuilderTestHelpers.Divider + " start " + BuilderTestHelpers.Divider);
                Output.WriteLine(maze);

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
