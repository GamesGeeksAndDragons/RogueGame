using Assets.Messaging;
using Utils.Coordinates;

namespace Assets.Actors
{
    internal class Rock : Dispatchee<Rock>
    {
        internal Rock(Coordinate coordinate, DispatchRegistry registry, string _) : base(coordinate, registry)
        {
        }

        internal Rock(Rock rock) : base(rock.Coordinates, rock.Registry)
        {
        }

        public override Rock Create()
        {
            return ActorBuilder.Build(this);
        }

        public override string ToString()
        {
            return ActorDisplay.Rock.ToString();
        }
    }
}
