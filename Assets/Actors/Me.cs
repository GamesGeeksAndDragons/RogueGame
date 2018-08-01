using Assets.Messaging;
using Utils.Coordinates;

namespace Assets.Actors
{
    internal class Me : Actor<Me>, IActor
    {
        public Me(Coordinate coordinates, ActorRegistry registry) : base(coordinates, registry)
        {
        }

        private Me(Me rhs) : base(rhs)
        {
        }

        public override IActor Clone(string parameters=null)
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
