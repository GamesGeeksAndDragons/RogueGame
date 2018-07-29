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

        public override IActor Clone()
        {
            return new Me(this);
        }

        public override string ToString()
        {
            return "@";
        }
    }
}
