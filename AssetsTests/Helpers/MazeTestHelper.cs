using Assets.Actors;
using Assets.Deeds;
using Assets.Mazes;
using Assets.Messaging;
using Utils.Dispatching;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.Helpers
{
    public abstract class MazeTestHelper
    {
        public IDispatchRegistry DispatchRegistry = new DispatchRegistry();
        public IActionRegistry ActionRegistry = new ActionRegistry();
        public IActorBuilder ActorBuilder;
        public IDieBuilder DieBuilder = new DieBuilder();

        public IMaze Maze { get; protected set; }

        protected readonly ITestOutputHelper Output;

        protected MazeTestHelper(ITestOutputHelper output)
        {
            Output = output;
            ActorBuilder = new ActorBuilder(DispatchRegistry, ActionRegistry);
        }

        protected virtual void TestArrange(IMazeExpectations expectations)
        {
            Output.OutputMazes(expectations.StartingMaze);
            Maze = LoadMaze(expectations.StartingMaze);

            IMaze LoadMaze(string loaded)
            {
                var tiles = DispatchRegistry.Register(ActorBuilder, loaded);
                var maze = new Maze(DispatchRegistry, ActionRegistry, DieBuilder, ActorBuilder, tiles);
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