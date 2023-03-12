#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Dispatching;
using Utils.Display;

namespace Assets.Tiles;

public interface IFloor : IDispatched
{
    int RoomNumber { get; }
    bool IsTunnel { get; }
}

internal class Floor : Dispatched<Floor>, IFloor
{
    public int RoomNumber { get; }
    public bool IsTunnel => RoomNumber == 0;

    internal Floor(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
        : base(dispatchRegistry, actionRegistry, actor, state)
    {
        if (actor.IsSame(TilesDisplay.Tunnel))
        {
            RoomNumber = 0;
        }
        else
        {
            RoomNumber = actor.FromRoomNumberString();
        }
    }

    public override Parameters CurrentState => base.CurrentState;
}