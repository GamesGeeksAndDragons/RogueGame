#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Dispatching;
using Parameters = System.Collections.Generic.List<(string Name, string Value)>;

namespace Assets.Actors
{
    public interface IFloor : IDispatched
    {
        IDispatched Contained { get; }
        void Contains(IDispatched dispatched);
        void SetEmpty();
    }

    internal class Floor : Dispatched<Floor>, IFloor
    {
        internal Floor(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry) 
            : base(dispatchRegistry, actionRegistry)
        {
            Contained = new Null(DispatchRegistry, ActionRegistry);
        }

        public override string ToString()
        {
            var display = Contained.IsNull() ? this.ToDisplayChar() : Contained.ToDisplayChar();
            return display;
        }

        public void SetEmpty()
        {
            Contained = new Null(DispatchRegistry, ActionRegistry);
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

        public void Contains(IDispatched dispatched)
        {
            Contained = dispatched;
        }
    }
}