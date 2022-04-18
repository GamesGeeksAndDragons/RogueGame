using Assets.Deeds;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Actors
{
    internal class Me : Character<Me>
    {
        internal Me(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string state)
            : base(dispatchRegistry, actionRegistry, state)
        {
        }

        private Me(Me me) : base(me.DispatchRegistry, me.ActionRegistry, "")
        {
        }
    }
}