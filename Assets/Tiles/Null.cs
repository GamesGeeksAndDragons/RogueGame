#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils.Dispatching;

namespace Assets.Tiles;

class Null : Dispatched<Null>
{
    public Null(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor)
        : base(dispatchRegistry, actionRegistry, actor, "")
    {
    }
}
