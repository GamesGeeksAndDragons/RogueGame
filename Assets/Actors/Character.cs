#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Parameters = System.Collections.Generic.List<(string Name, string Value)>;

namespace Assets.Actors
{
    internal interface ICharacter
    {
        Parameters CurrentState();
        int ArmourClass { get; set; }
        int HitPoints { get; set; }
        Coordinate Position { get; set; }
    }

    internal abstract class Character<T> : Dispatched<T>, ICharacter
        where T : class, IDispatched
    {
        protected Character(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string state) 
            : base(dispatchRegistry, actionRegistry)
        {
            var extracted = state.ToParameters();

            // ReSharper disable once VirtualMemberCallInConstructor
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

        public int ArmourClass { get; set; }
        public int HitPoints { get; set; }
        public Coordinate Position { get; set; }

        protected internal override void RegisterActions()
        {
            ActionRegistry.RegisterAction(this, Deed.Teleport);
            ActionRegistry.RegisterAction(this, Deed.Move);
            ActionRegistry.RegisterAction(this, Deed.Strike);

            base.RegisterActions();
        }
    }
}
