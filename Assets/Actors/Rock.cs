#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Actors
{
    internal class Rock : Dispatched<Rock>
    {
        internal Rock(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string _) 
            : base(dispatchRegistry, actionRegistry, actor)
        {
        }

        internal Rock(Rock rock) : base(rock.DispatchRegistry, rock.ActionRegistry, rock.Actor)
        {
        }
    }
}
