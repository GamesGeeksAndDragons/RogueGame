using System.Collections.Generic;
using Assets.Messaging;
using Utils.Coordinates;

namespace Assets.Actors
{
    internal class Me : Character<Me>
    {
        internal Me(Coordinate coordinates, DispatchRegistry registry, string state) 
            : base(coordinates, registry, state)
        {
        }

        private Me(Me me) : base(me.Coordinates, me.Registry, "")
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
