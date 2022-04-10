using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Parameters = System.Collections.Generic.List<(string Name, string Value)>;

namespace Assets.Actors
{
    internal class Floor : Dispatchee<Floor>
    {
        internal Floor(Coordinate coordinates, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry) 
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