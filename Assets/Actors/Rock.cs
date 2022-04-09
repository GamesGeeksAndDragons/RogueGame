using Assets.Deeds;
using Assets.Messaging;
using Utils.Coordinates;

namespace Assets.Actors
{
    internal class Rock : Dispatchee<Rock>
    {
        internal Rock(Coordinate coordinate, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry) 
            : base(coordinate, dispatchRegistry, actionRegistry)
        {
        }

        internal Rock(Rock rock) : base(rock.Coordinates, rock.DispatchRegistry, rock.ActionRegistry)
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
