#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Dispatching;
using Parameters = System.Collections.Generic.List<(string Name, string Value)>;

namespace Assets.Tiles
{
    internal class Door : Dispatched<Door>
    {
        internal Door(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state) 
            : base(dispatchRegistry, actionRegistry, actor)
        {
            var doorId = actor.FromHexString();

            DoorId = doorId;
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
