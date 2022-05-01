#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils.Dispatching;

namespace Assets.Actors
{
    class Null : Dispatched<Null>
    {
        public Null(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor) 
            : base(dispatchRegistry, actionRegistry, actor)
        {
        }

        public Null(Null tile) : base(tile.DispatchRegistry, tile.ActionRegistry, tile.Actor)
        {
        }
    }
}
