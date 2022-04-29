using Assets.Actors;
using Assets.Deeds;
using Assets.Maze;
using Assets.Messaging;
using Utils.Dispatching;
using Utils.Random;
using Xunit;
using Xunit.Abstractions;

namespace AssetsTests.Helpers
{
    public abstract class TilesTestHelper
    {
        public IDispatchRegistry DispatchRegistry = new DispatchRegistry();
        public IActionRegistry ActionRegistry = new ActionRegistry();
        public IActorBuilder ActorBuilder;
        public IDieBuilder DieBuilder = new DieBuilder();

        public ITiles Tiles { get; protected set; }

        protected readonly ITestOutputHelper Output;

        protected TilesTestHelper(ITestOutputHelper output)
        {
            Output = output;
            ActorBuilder = new ActorBuilder(DispatchRegistry, ActionRegistry);
        }

        protected virtual void TestArrange(IMazeExpectations expectations)
        {
            Output.OutputMazes(expectations.StartingMaze);
            Tiles = LoadTiles(expectations.StartingMaze);

            ITiles LoadTiles(string maze)
            {
                var dispatched = DispatchRegistry.Register(ActorBuilder, maze);
                var tilesRegistry = dispatched.ExtractTilesRegistry();
                var tiles = new Tiles(DispatchRegistry, ActionRegistry, DieBuilder, ActorBuilder, tilesRegistry);
                return tiles;
            }
        }

        protected abstract void TestAct();

        protected virtual void TestAssert(IMazeExpectations expectations)
        {
            var actual = Tiles.Print(DispatchRegistry);
            Output.OutputMazes(expectations.ExpectedMaze, actual);

            Assert.Equal(expectations.ExpectedMaze, actual);
        }

    }
}