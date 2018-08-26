using Assets.Messaging;
using Utils.Coordinates;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string name, string value)>;

namespace Assets.Actors
{
    internal abstract class Character<T> : Dispatchee<T> 
        where T : class, IDispatchee
    {
        protected Character(Coordinate coordinates, DispatchRegistry registry, string state) : base(coordinates, registry)
        {
            var extracted = state.ToParameters();

            UpdateState(this as T, extracted);
        }
        
        public override void UpdateState(T t, ExtractedParameters state)
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

            return state + Dispatchee<T>.FormatState(coordinates, uniqueId);
        }

        public int ArmourClass { get; protected internal set; }
        public int HitPoints { get; protected internal set; }

        protected internal override void RegisterActions()
        {
            RegisterAction(Actions.Strike, StrikeImpl);

            base.RegisterActions();
        }

        private void StrikeImpl(ExtractedParameters parameters)
        {
            var coordindates = parameters.ToValue<Coordinate>(nameof(Coordinates));
            if (coordindates != Coordinates) return;

            var hit = parameters.ToValue<int>(nameof(HitPoints));
            if (hit < ArmourClass) return;

            var damage = parameters.ToValue<int>("Damage");
            var newHitPoints = HitPoints - damage;

            //var newState = FormatState(hitPoints: newHitPoints);
            //var newCharacter = Clone(newState);

            //Registry.Register(newCharacter);
        }
    }
}
