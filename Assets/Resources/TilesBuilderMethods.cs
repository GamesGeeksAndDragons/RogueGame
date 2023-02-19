using Assets.Deeds;
using Assets.Tiles;
using Utils.Dispatching;
using Utils.Enums;

namespace Assets.Resources;

internal static class TilesBuilderMethods
{
    internal static IDispatched BuildFloor(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
    {
        return new Floor(dispatchRegistry, actionRegistry, actor, state);
    }

    internal static IDispatched BuildDoor(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
    {
        return new Door(dispatchRegistry, actionRegistry, actor, state);
    }

    internal static IDispatched BuildRock(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
    {
        return new Rock(dispatchRegistry, actionRegistry, actor, state);
    }

    internal static IDispatched BuildWall(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
    {
        var direction = actor.ToWallDirection();

        return new Wall(dispatchRegistry, actionRegistry, actor, direction.ToString());
    }

    public static IDispatched BuildNull(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
    {
        return new Null(dispatchRegistry, actionRegistry, actor);
    }
}
