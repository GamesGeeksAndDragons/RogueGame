#nullable enable
using Assets.Deeds;
using Utils.Dispatching;

namespace Assets.Actors
{
    internal class Me : Character<Me>
    {
        internal Me(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
            : base(dispatchRegistry, actionRegistry, actor, state)
        {
        }
    }
}