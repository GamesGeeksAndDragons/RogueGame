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

            GameLevel = GameLevelBuilder.BuildExistingLevel(expectations.Level, expectations.StartingMaze, expectations.CharactersState);

            Output.WriteLine(BuilderTestHelpers.Divider + " setup " + BuilderTestHelpers.Divider);
            Output.OutputMazes(GameLevel.Print(DispatchRegistry));
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