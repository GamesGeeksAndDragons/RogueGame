using Assets.Level;
using Assets.Messaging;
using Assets.Personas;
using Assets.Resources;
using Utils.Random;
using Xunit.Abstractions;

namespace AssetsTests.Helpers
{
    public class LevelBuilderTestHelpers : BuilderTestHelpers
    {
        internal Dispatcher Dispatcher;
        internal PersonasBuilder PersonasBuilder;
        internal LevelBuilder LevelBuilder;

        public LevelBuilderTestHelpers(ITestOutputHelper output)
            : base(output)
        {
        }

        internal override void ArrangeTest(Die.RandomiserReset reset = Die.RandomiserReset.None)
        {
            base.ArrangeTest(reset);

            Dispatcher = new Dispatcher(DispatchRegistry, ActionRegistry);
            var characterBuilder = new CharacterBuilder(DispatchRegistry, ActionRegistry);
            PersonasBuilder = new PersonasBuilder(DieBuilder, characterBuilder, "1B3");
            LevelBuilder = new LevelBuilder(DieBuilder, FakeLogger, Dispatcher, DispatchRegistry, ActionRegistry, ResourceBuilder, PersonasBuilder);
        }
    }
}
