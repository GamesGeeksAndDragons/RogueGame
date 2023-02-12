using Assets.Level;

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
            base.TestArrange();

            GameLevel = GameLevelBuilder.BuildExistingLevel(expectations.Level, expectations.StartingMaze);

            Output.OutputMazes(GameLevel.Maze.ToString());
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