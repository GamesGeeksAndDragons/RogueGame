#nullable enable
using Assets.Deeds;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Personas;

internal class Me : Character<Me>
{
    internal Me(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
        : base(dispatchRegistry, actionRegistry, actor, state)
    {
    }

    public Me(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, Coordinate position, int armourClass, int hitPoints)
        : base(dispatchRegistry, actionRegistry, actor, position, armourClass, hitPoints)
    {
    }
}
