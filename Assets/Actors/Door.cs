using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Parameters = System.Collections.Generic.List<(string Name, string Value)>;

namespace Assets.Actors
{
    internal class Door : Dispatchee<Door>
    {
        internal Door(Coordinate coordinates, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string state) 
            : base(coordinates, dispatchRegistry, actionRegistry)
        {
            var doorId = int.Parse(state);
            DoorId = doorId;
        }

        internal Door(Door door) : base(door.Coordinates, door.DispatchRegistry, door.ActionRegistry)
        {
            DoorId = door.DoorId;
        }

        public override Door Create()
        {
            return ActorBuilder.Build(this);
        }

        public override void UpdateState(Parameters state)
        {
            if (state.HasValue(nameof(DoorId)))
            {
                DoorId = state.ToValue<int>(nameof(DoorId));
            }

            base.UpdateState(state);
        }

        public override Parameters CurrentState()
        {
            var parameters = base.CurrentState();

            var door = (Name: nameof(DoorId), Value: DoorId.ToString());
            parameters.Add(door);

            return parameters;
        }

        public int DoorId { get; private set; }
    }
}
