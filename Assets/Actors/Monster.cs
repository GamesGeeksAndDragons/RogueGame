#nullable enable
using Assets.Deeds;
using Utils.Dispatching;

namespace Assets.Actors
{
    internal class Monster : Character<Monster>
    {
        public Monster(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state) 
            : base(dispatchRegistry, actionRegistry, actor, state)
        {
        }
    }
}
