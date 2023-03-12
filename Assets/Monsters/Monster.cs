#nullable enable
using Assets.Characters;
using Assets.Deeds;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Monsters;

internal class Monster : Character<Monster>
{
    public Monster(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
        : base(dispatchRegistry, actionRegistry, actor, state)
    {
    }
}
