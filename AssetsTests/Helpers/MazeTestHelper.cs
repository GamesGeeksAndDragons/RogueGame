using Assets.Mazes;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.Helpers
{
    public abstract class  MazeTestHelper : LevelBuilderTestHelpers
    {
        public IMaze Maze { get; protected set; }

        protected MazeTestHelper(ITestOutputHelper output) 
            : base(output)
        {
        }

        protected virtual void TestArrange(IMazeExpectations expectations)
        {
            base.ArrangeTest();

            Output.OutputMazes(expectations.StartingMaze);
            Maze = LoadMaze(expectations.StartingMaze);

            IMaze LoadMaze(string loaded)
            {
                var maze = new Maze(DispatchRegistry, ActionRegistry, DieBuilder, ResourceBuilder, loaded);
                return maze;
            }
        }

        protected abstract void TestAct();

        protected virtual void TestAssert(IMazeExpectations expectations)
        {
            var actual = Maze.Print(DispatchRegistry);
            Output.OutputMazes(expectations.ExpectedMaze, actual);

            Assert.Equal(expectations.ExpectedMaze, actual);
        }

    }
}