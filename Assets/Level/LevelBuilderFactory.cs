using Assets.Deeds;
using Assets.Messaging;
using Assets.Personas;
using Assets.Resources;
using Assets.Rooms;
using log4net;
using Utils.Dispatching;
using Utils.Random;

namespace Assets.Level
{
    public class LevelBuilderFactory
    {
        internal IDispatchRegistry DispatchRegistry { get; }
        internal IActionRegistry ActionRegistry { get; }
        internal IResourceBuilder ResourceBuilder { get; }
        internal IDispatcher Dispatcher { get; }

        public LevelBuilderFactory()
        {
            DispatchRegistry = new DispatchRegistry();
            ActionRegistry = new ActionRegistry();
            ResourceBuilder = new ResourceBuilder(DispatchRegistry, ActionRegistry);
            Dispatcher = new Dispatcher(DispatchRegistry, ActionRegistry);
        }

        internal IRoomBuilder CreateRoomBuilder(IDieBuilder dieBuilder, ILog logger) =>
            new RoomBuilder(dieBuilder, logger, DispatchRegistry, ActionRegistry, ResourceBuilder);

        internal IPersonasBuilder CreatePersonasBuilder(IDieBuilder dieBuilder) =>
            new PersonasBuilder(dieBuilder, new CharacterBuilder(DispatchRegistry, ActionRegistry));

        internal ILevelBuilder CreateLevelBuilder(IDieBuilder dieBuilder, ILog logger) =>
            new GameLevelBuilder(dieBuilder, logger, Dispatcher, DispatchRegistry, ActionRegistry, ResourceBuilder, CreatePersonasBuilder(dieBuilder));
    }
}
