using Assets.Deeds;
using Assets.Messaging;
using Utils.Coordinates;

namespace Assets.Actors
{
    class Null : Dispatchee<Null>
    {
        public Null(Coordinate coordinates, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry) 
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
            return ActorDisplay.Null.ToString();
        }
    }
}
