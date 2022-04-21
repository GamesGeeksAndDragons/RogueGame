using Assets.Deeds;
using Assets.Messaging;
using Utils.Dispatching;

namespace Assets.Actors
{
    class Null : Dispatchee<Null>
    {
        public Null(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry) 
            : base(dispatchRegistry, actionRegistry)
        {
        }

        public Null(Null tile) : base(tile.DispatchRegistry, tile.ActionRegistry)
        {
        }

        public override string ToString()
        {
            return this.ToDisplayChar();
        }
    }
}
