using Assets.Deeds;
using Assets.Messaging;
using Assets.Personas;
using Assets.Resources;
using Assets.Rooms;
using Utils.Dispatching;
using Utils.Random;

namespace Assets.Level;

public class GameLevelBuilderFactory
{
    internal IDispatchRegistry DispatchRegistry { get; }
    internal IActionRegistry ActionRegistry { get; }
    internal IResourceBuilder ResourceBuilder { get; }
    internal IDispatcher Dispatcher { get; }

    public GameLevelBuilderFactory()
    {
        DispatchRegistry = new DispatchRegistry();
        ActionRegistry = new ActionRegistry();
        ResourceBuilder = new ResourceBuilder(DispatchRegistry, ActionRegistry);
        Dispatcher = new Dispatcher(DispatchRegistry, ActionRegistry);
    }

    internal IRoomBuilder CreateRoomBuilder(IDieBuilder dieBuilder, ILog logger) =>
        new RoomBuilder(dieBuilder, logger, DispatchRegistry, ActionRegistry, ResourceBuilder);

    internal ICharacterFactory CreateCharacterFactory(IDieBuilder dieBuilder) =>
        new CharacterFactory(dieBuilder, new CharacterBuilder(DispatchRegistry, ActionRegistry));

    internal IGameCharacters CreateGameCharacters(IDieBuilder dieBuilder) =>
        new GameCharacters(DispatchRegistry, CreateCharacterFactory(dieBuilder));

    internal IGameLevelBuilder CreateLevelBuilder(IDieBuilder dieBuilder, ILog logger) =>
        new GameLevelBuilder(CreateGameCharacters(dieBuilder), dieBuilder, logger, Dispatcher, DispatchRegistry, ActionRegistry, ResourceBuilder);
}
