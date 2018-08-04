using Assets.Messaging;
using Utils.Coordinates;

namespace Assets.Actors
{
    internal class Me : Dispatchee<Me>
    {
        public Me(Coordinate coordinates, DispatchRegistry registry) : base(coordinates, registry)
        {
        }

        private Me(Me rhs) : base(rhs)
        {
        }

        public override IDispatchee Clone(string parameters=null)
        {
            var me = new Me(this);

            if (parameters != null)
            {
                var extracted = parameters.ToParameters();
                var newCoordindates = extracted.GetParameter<Coordinate>("Coordinates");
                me.Coordinates = newCoordindates;
            }

            return me;
        }

        public override string ToString()
        {
            return "@";
        }
    }
}
