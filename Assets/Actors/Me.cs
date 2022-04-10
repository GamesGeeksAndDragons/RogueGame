using Assets.Deeds;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Actors
{
    internal class Me : Character<Me>
    {
        internal Me(Coordinate coordinates, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry,
            string state)
            : base(coordinates, dispatchRegistry, actionRegistry, state)
        {
        }

        private Me(Me me) : base(me.Coordinates, me.DispatchRegistry, me.ActionRegistry, "")
        {
        }

        public override Me Create()
        {
            return ActorBuilder.Build(this);
        }
    }
}