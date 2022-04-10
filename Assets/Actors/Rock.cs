using Assets.Deeds;
using Assets.Messaging;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Actors
{
    internal class Rock : Dispatchee<Rock>
    {
        internal Rock(Coordinate coordinate, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry) 
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
            return this.ToDisplayChar();
        }
    }
}
