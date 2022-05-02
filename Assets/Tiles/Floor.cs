#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Dispatching;
using Utils.Display;
using Parameters = System.Collections.Generic.List<(string Name, string Value)>;

namespace Assets.Tiles
{
    public interface IFloor : IDispatched
    {
        IDispatched Contained { get; }
        int RoomNumber { get; }
        bool IsTunnel { get; }
        void Contains(IDispatched dispatched);
        void SetEmpty();
    }

    internal class Floor : Dispatched<Floor>, IFloor
    {
        internal Floor(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string _) 
            : base(dispatchRegistry, actionRegistry, actor)
        {
            if (actor.IsSame(TilesDisplay.Floor))
            {
                RoomNumber = 0;
            }
            else
            {
                RoomNumber = actor.FromRoomNumberString();
            }

            Contained = new Null(DispatchRegistry, ActionRegistry, TilesDisplay.Null);
        }

        public override string ToString()
        {
            var display = Contained.IsNull() ? Actor : Contained.Actor;
            return display;
        }

        public void SetEmpty()
        {
            Contained = new Null(DispatchRegistry, ActionRegistry, TilesDisplay.Null);
        }

        public override void UpdateState(Parameters state)
        {
            if (state.HasValue(nameof(Contained)))
            {
                Contained = state.GetDispatched(nameof(Contained), DispatchRegistry);
            }

            base.UpdateState(state);
        }

        public override Parameters CurrentState()
        {
            var parameters = base.CurrentState();

            if (Contained.IsNull()) return parameters;

            var onTheFloor = (Name: nameof(Contained), Value: Contained.UniqueId);
            parameters.Add(onTheFloor);

            return parameters;
        }

        public IDispatched Contained { get; private set; }
        public int RoomNumber { get; }
        public bool IsTunnel => RoomNumber == 0;

        public void Contains(IDispatched dispatched)
        {
            Contained = dispatched;
        }
    }
}