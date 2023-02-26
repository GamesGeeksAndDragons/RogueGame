using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Dispatching;

namespace Assets.Props
{
    // https://beej.us/moria/items.txt

    internal abstract class Prop<T> : Dispatched<T> where T : class, IDispatched
    {
        protected Prop(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state, string uniqueId) 
            : base(dispatchRegistry, actionRegistry, actor, uniqueId)
        {
            PropName = "";
            OriginalCost = Weight = 0.0;
            Level = Damage = 0;

            var extracted = state.ToParameters();
            // ReSharper disable once VirtualMemberCallInConstructor
            UpdateState(extracted);
        }

        public string PropName { get; protected set; }
        public double Weight { get; protected set; }
        public int Level { get; protected set; }
        public double OriginalCost { get; protected set; }
        public int Damage { get; protected set; }

        public override Parameters CurrentState()
        {
            var state = base.CurrentState();

            if (!PropName.IsNullOrEmpty()) state.AppendParameter(nameof(PropName), PropName);
            if (!Weight.IsZero()) state.AppendParameter(nameof(Weight), Weight);
            if (Level != 0) state.AppendParameter(nameof(Level), Level);
            if (Damage != 0) state.AppendParameter(nameof(Damage), Damage);
            if (OriginalCost != 0) state.AppendParameter(nameof(OriginalCost), Damage);

            return state;
        }

        public override void UpdateState(Parameters state)
        {
            if (state.HasValue(nameof(PropName))) PropName = state.ToString(nameof(PropName));
            if (state.HasValue(nameof(Weight))) Weight = state.ToValue<double>(nameof(Weight));
            if (state.HasValue(nameof(Level))) Level = state.ToValue<int>(nameof(Level));
            if (state.HasValue(nameof(OriginalCost))) OriginalCost = state.ToValue<double>(nameof(OriginalCost));
            if (state.HasValue(nameof(Damage))) Damage = state.ToValue<int>(nameof(Damage));

            base.UpdateState(state);
        }
    }
}
