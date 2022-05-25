using Assets.Level;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.Helpers
{
    public abstract class  MazeTestHelper : LevelBuilderTestHelpers
    {
        public IGameLevel GameLevel { get; protected set; }

        protected MazeTestHelper(ITestOutputHelper output) 
            : base(output)
        {
        }

        protected virtual void TestArrange(IMazeExpectations expectations)
        {
            base.ArrangeTest();

            Output.OutputMazes(expectations.StartingMaze);

            GameLevel = LevelBuilder.BuildExistingLevel(1, expectations.StartingMaze);
        }

        protected abstract void TestAct();

        protected virtual void TestAssert(IMazeExpectations expectations)
        {
            var actual = GameLevel.Print(DispatchRegistry);
            Output.OutputMazes(expectations.ExpectedMaze, actual);

            Assert.Equal(expectations.ExpectedMaze, actual);
        }

    }
}