using Assets.Deeds;
using Assets.Messaging;
using Utils.Coordinates;

namespace Assets.Actors
{
    internal class Me : Character<Me>
    {
        internal Me(Coordinate coordinates, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry, string state) 
            : base(coordinates, dispatchRegistry, actionRegistry, state)
        {
        }

        private Me(Me me) : base(me.Coordinates, me.DispatchRegistry, me.ActionRegistry, "")
        {

        }

        public override string ToString()
        {
            return "@";
        }

        public override Me Create()
        {
            return ActorBuilder.Build(this);
        }
    }
}
