using Assets.Level;
using Assets.Messaging;
using Utils.Random;

namespace AssetsTests.Helpers
{
    public class LevelBuilderTestHelpers : BuilderTestHelpers
    {
        protected ILevelBuilder LevelBuilder;
        protected IDispatcher Dispatcher => MazeBuilderFactory.Dispatcher;

        public LevelBuilderTestHelpers(ITestOutputHelper output)
            : base(output)
        {
        }

        internal override void TestArrange(Die.RandomiserReset reset = Die.RandomiserReset.None)
        {
            base.TestArrange(reset);

            LevelBuilder = MazeBuilderFactory.CreateLevelBuilder(DieBuilder, FakeLogger);
        }
    }
}
