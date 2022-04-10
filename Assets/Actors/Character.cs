using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Parameters = System.Collections.Generic.List<(string Name, string Value)>;

namespace Assets.Actors
{
    internal abstract class Character<T> : Dispatchee<T> 
        where T : class, IDispatchee
    {
        protected Character(Coordinate coordinates, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string state) 
            : base(coordinates, dispatchRegistry, actionRegistry)
        {
            var extracted = state.ToParameters();

            UpdateState(extracted);
        }

        public override void UpdateState(Parameters state)
        {
            if (state.HasValue(nameof(HitPoints))) HitPoints = state.ToValue<int>(nameof(HitPoints));
            if (state.HasValue(nameof(ArmourClass))) ArmourClass = state.ToValue<int>(nameof(ArmourClass));

            base.UpdateState(state);
        }

        public override Parameters CurrentState()
        {
            var state = base.CurrentState();

            if (!IsZero(HitPoints)) state.AppendParameter(nameof(HitPoints), HitPoints);
            if (!IsZero(ArmourClass)) state.AppendParameter(nameof(ArmourClass), ArmourClass);

            return state;
        }

        public int ArmourClass { get; protected internal set; }
        public int HitPoints { get; protected internal set; }

        protected internal override void RegisterActions()
        {
            ActionRegistry.RegisterAction(this, Deed.Teleport);
            ActionRegistry.RegisterAction(this, Deed.Move);
            ActionRegistry.RegisterAction(this, Deed.Strike);

            base.RegisterActions();
        }
    }
}
