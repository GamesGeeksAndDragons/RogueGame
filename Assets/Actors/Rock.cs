using Assets.Messaging;
using Utils.Coordinates;

namespace Assets.Actors
{
    internal class Rock : Dispatchee<Rock>
    {
        public Rock(Coordinate coordinate, DispatchRegistry registry) : base(coordinate, registry)
        {
        }

        private Rock(Rock rock) : base(rock.Coordinates, rock.Registry)
        {
        }

        public override Rock Create()
        {
            return new Rock(this);
        }

        public override string ToString()
        {
            return "█";
        }
    }
}
