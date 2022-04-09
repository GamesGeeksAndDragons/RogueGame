using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string Name, string Value)>;

namespace Assets.Actors
{
    internal class Door : Dispatchee<Door>
    {
        internal Door(Coordinate coordinates, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry, string state) : base(coordinates, dispatchRegistry, actionRegistry)
        {
            var doorId = int.Parse(state);
            DoorId = doorId;
        }

        internal Door(Door door) : base(door.Coordinates, door.DispatchRegistry, door.ActionRegistry)
        {
            DoorId = door.DoorId;
        }

        public static string FormatState(int? doorId = null, Coordinate? coordinates = null, string uniqueId = null)
        {
            var state = string.Empty;

            if (doorId.HasValue) state += nameof(DoorId).FormatParameter(doorId.Value);

            return state + Dispatchee<Door>.FormatState(coordinates, uniqueId);
        }

        public override Door Create()
        {
            return ActorBuilder.Build(this);
        }

        public override void UpdateState(Door door, ExtractedParameters state)
        {
            if (state.HasValue(nameof(DoorId))) door.DoorId = state.ToValue<int>(nameof(DoorId));

            base.UpdateState(door, state);
        }

        public int DoorId { get; private set; }

        public override string ToString()
        {
            return DoorId.ToString("X1");
        }
    }
}
