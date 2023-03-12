#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Dispatching;
using Utils.Display;

namespace Assets.Tiles;

internal class Door : Dispatched<Door>
{
    internal Door(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
        : base(dispatchRegistry, actionRegistry, actor, state)
    {
        var doorId = actor.FromDoorNumberString();

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

    public override Parameters CurrentState
    {
        get
        {
            var state = base.CurrentState;

            var door = new[]
            {
                (Name: nameof(DoorId), Value: DoorId.ToString()),
            };

            state.AddRange(door);

            return state;
        }
    }

    public int DoorId { get; private set; }
}
