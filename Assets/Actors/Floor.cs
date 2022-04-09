using Assets.Deeds;
using Assets.Messaging;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Actors
{
    internal class Floor : Dispatchee<Floor>
    {
        internal Floor(Coordinate coordinates, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry) 
            : base(coordinates, dispatchRegistry, actionRegistry)
        {
        }

        internal Floor(Floor floor) 
            : base(floor.Coordinates, floor.DispatchRegistry, floor.ActionRegistry)
        {
        }

        public override Floor Create()
        {
            return ActorBuilder.Build(this);
        }

        public override string ToString()
        {
            return ActorDisplay.Floor.ToString();
        }

        public IDispatchee OnFloor { get; }
    }
}