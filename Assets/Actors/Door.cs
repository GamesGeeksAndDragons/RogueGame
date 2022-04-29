#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Dispatching;
using Parameters = System.Collections.Generic.List<(string Name, string Value)>;

namespace Assets.Actors
{
    internal class Door : Dispatched<Door>
    {
        internal Door(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string state) 
            : base(dispatchRegistry, actionRegistry)
        {
            var doorId = state.FromHexString();

            DoorId = doorId;
        }

        internal Door(Door door) : base(door.DispatchRegistry, door.ActionRegistry)
        {
            DoorId = door.DoorId;
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

            var door = new[]
            {
                (Name: nameof(DoorId), Value: DoorId.ToString()),
            };

            parameters.AddRange(door);

            return parameters;
        }

        public int DoorId { get; private set; }
    }
}
