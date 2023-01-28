#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils.Dispatching;

namespace Assets.Tiles
{
    internal class Rock : Dispatched<Rock>
    {
        internal Rock(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string _) 
            : base(dispatchRegistry, actionRegistry, actor)
        {
        }
    }
}
