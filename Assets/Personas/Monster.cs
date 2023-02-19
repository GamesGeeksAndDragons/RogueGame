#nullable enable
using Assets.Deeds;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Personas;

internal class Monster : Character<Monster>
{
    public Monster(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
        : base(dispatchRegistry, actionRegistry, actor, state)
    {
    }

    public Monster(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, Coordinate position, int armourClass, int hitPoints)
        : base(dispatchRegistry, actionRegistry, actor, position, armourClass, hitPoints)
    {
    }
}
