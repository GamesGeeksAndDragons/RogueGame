using Assets.Messaging;
using Utils.Coordinates;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string name, string value)>;

namespace Assets.Actors
{
    internal abstract class Character<T> : Dispatchee<T> 
        where T : IDispatchee
    {
        protected Character(string state, Coordinate coordinates, DispatchRegistry registry) : base(coordinates, registry)
        {
            var extracted = state.ToParameters();
            UpdateState(extracted);
        }
        
        protected Character(Character<T> rhs) : base(rhs)
        {
            HitPoints = rhs.HitPoints;
            ArmourClass = rhs.ArmourClass;
        }

        protected override void UpdateState(ExtractedParameters parameters)
        {
            if (parameters.Count > 0)
            {
                if (parameters.HasValue("HitPoints"))
                {
                    HitPoints = parameters.ToValue<int>("HitPoints");
                }

                if (parameters.HasValue("ArmourClass"))
                {
                    ArmourClass = parameters.ToValue<int>("ArmourClass");
                }
            }

            base.UpdateState(parameters);
        }

        public static string CharacterState(int? hitPoints = null, int? armourClass = null)
        {
            var state = string.Empty;

            if (hitPoints.HasValue)
            {
                state += $"HitPoints [{hitPoints}] ";
            }

            if (armourClass.HasValue)
            {
                state += $"ArmourClass [{armourClass}] ";
            }

            return state;
        }

        public abstract override IDispatchee Clone(string parameters = null);

        public int ArmourClass { get; protected internal set; }
        public int HitPoints { get; protected internal set; }

        protected internal override void RegisterActions()
        {
            RegisterAction(Actions.Strike, StrikeImpl);

            base.RegisterActions();
        }

        private void StrikeImpl(ExtractedParameters parameters)
        {
            var coordindates = parameters.ToValue<Coordinate>("Coordinates");
            if (coordindates != Coordinates) return;

            var hit = parameters.ToValue<int>("Hit");
            if (hit < ArmourClass) return;

            var damage = parameters.ToValue<int>("Damage");
            var newHitPoints = HitPoints - damage;

            var newState = CharacterState(hitPoints: newHitPoints);
            var newCharacter = Clone(newState);

            Registry.Swap(this, newCharacter);
        }
    }
}
