using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Parameters = System.Collections.Generic.IReadOnlyList<(string Name, string Value)>;

namespace Assets.Actors
{
    internal abstract class Character<T> : Dispatchee<T> 
        where T : class, IDispatchee, ICloner<T>
    {
        protected Character(Coordinate coordinates, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry, string state) 
            : base(coordinates, dispatchRegistry, actionRegistry)
        {
            var extracted = state.ToParameters();

            UpdateState(this as T, extracted);
        }
        
        public override void UpdateState(T t, Parameters state)
        {
            var character = t as Character<T>;
            if (state.HasValue(nameof(HitPoints))) character.HitPoints = state.ToValue<int>(nameof(HitPoints));
            if (state.HasValue(nameof(ArmourClass))) character.ArmourClass = state.ToValue<int>(nameof(ArmourClass));

            base.UpdateState(t, state);
        }

        public static string FormatState(int? hitPoints = null, int? armourClass = null, Coordinate? coordinates = null, string uniqueId = null)
        {
            var state = string.Empty;

            if (hitPoints.HasValue) state += nameof(HitPoints).FormatParameter(hitPoints.Value);
            if (armourClass.HasValue) state += nameof(ArmourClass).FormatParameter(armourClass.Value);

            return state + FormatState(coordinates, uniqueId);
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
