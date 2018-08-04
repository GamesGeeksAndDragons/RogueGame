using Assets.Messaging;
using Utils.Coordinates;

namespace Assets.Actors
{
    internal class Rock : Dispatchee<Rock>
    {
        public Rock(Coordinate coordinate, DispatchRegistry registry) : base(coordinate, registry)
        {
        }

        private Rock(Rock rhs) : base(rhs)
        {
        }

        public override IDispatchee Clone(string parameters=null)
        {
            return new Rock(this);
        }

        public override string ToString()
        {
            return "■";
        }
    }
}
