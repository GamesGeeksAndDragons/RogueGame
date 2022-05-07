using Assets.Level;
using Assets.Messaging;
using Utils.Random;
using Xunit.Abstractions;

namespace AssetsTests.Helpers
{
    public class LevelBuilderTestHelpers : BuilderTestHelpers
    {
        internal Dispatcher Dispatcher;
        internal LevelBuilder LevelBuilder;

        public LevelBuilderTestHelpers(ITestOutputHelper output)
            : base(output)
        {
        }

        internal override void ArrangeTest(Die.RandomiserReset reset = Die.RandomiserReset.None)
        {
            base.ArrangeTest(reset);

            Dispatcher = new Dispatcher(DispatchRegistry, ActionRegistry);
            LevelBuilder = new LevelBuilder(DieBuilder, FakeLogger, Dispatcher, DispatchRegistry, ActionRegistry, ResourceBuilder);
        }
    }
}
