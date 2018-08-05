using Assets.Messaging;
using Utils.Coordinates;

namespace Assets.Actors
{
    internal class Me : Character<Me>
    {
        public Me(string state, Coordinate coordinates, DispatchRegistry registry) 
            : base(state, coordinates, registry)
        {
        }

        private Me(Me rhs) : base(rhs)
        {
        }

        public override IDispatchee Clone(string parameters=null)
        {
            var clone = new Me(this);
            clone.UpdateState(parameters.ToParameters());
            return clone;
        }

        public override string ToString()
        {
            return "@";
        }
    }
}
