using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Dispatching;
using Parameters = System.Collections.Generic.List<(string Name, string Value)>;

namespace Assets.Actors
{
    public interface IFloor : IDispatchee
    {
        IDispatchee Contains { get; set; }
    }

    internal class Floor : Dispatchee<Floor>, IFloor
    {
        internal Floor(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry) 
            : base(dispatchRegistry, actionRegistry)
        {
        }

        public override string ToString()
        {
            var display = Contains?.ToDisplayChar() ?? this.ToDisplayChar();
            return display;
        }

        public override void UpdateState(Parameters state)
        {
            if (state.HasValue(nameof(Contains)))
            {
                Contains = state.GetDispatchee(nameof(Contains), DispatchRegistry);
            }

            base.UpdateState(state);
        }

        public override Parameters CurrentState()
        {
            var parameters = base.CurrentState();

            if (Contains == null) return parameters;

            var onTheFloor = (Name: nameof(Contains), Value: Contains.UniqueId);
            parameters.Add(onTheFloor);

            return parameters;
        }

        public IDispatchee Contains { get; set; }
    }
}