#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Actors
{
    internal class Rock : Dispatched<Rock>
    {
        internal Rock(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry) 
            : base(dispatchRegistry, actionRegistry)
        {
        }

        internal Rock(Rock rock) : base(rock.DispatchRegistry, rock.ActionRegistry)
        {
        }

        public override string ToString()
        {
            return this.ToDisplayChar();
        }
    }
}
