#nullable enable
using Assets.Characters;
using Assets.Deeds;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Player;

internal class Me : Character<Me>
{
    internal Me(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
        : base(dispatchRegistry, actionRegistry, actor, state)
    {
    }
}
