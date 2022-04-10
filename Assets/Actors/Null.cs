using Assets.Deeds;
using Assets.Messaging;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Actors
{
    class Null : Dispatchee<Null>
    {
        public Null(Coordinate coordinates, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry) 
            : base(coordinates, dispatchRegistry, actionRegistry)
        {
        }

        public Null(Null tile) : base(tile.Coordinates, tile.DispatchRegistry, tile.ActionRegistry)
        {
        }

        public override Null Create()
        {
            return ActorBuilder.Build(this);
        }

        public override string ToString()
        {
            return this.ToDisplayChar();
        }
    }
}
