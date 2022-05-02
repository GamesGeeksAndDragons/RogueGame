#nullable enable
using Utils.Dispatching;
using Parameters = System.Collections.Generic.IReadOnlyList<(string Name, string Value)>;

namespace Assets.Deeds
{
    class StrikeAction : Action
    {
        public override void Act(IDispatchRegistry dispatchRegistry, IDispatched dispatched, string actionValue)
        {
            //var coordindates = parameters.ToValue<Coordinate>(nameof(Coordinates));
            //if (coordindates != Coordinates) return;

            //var hit = parameters.ToValue<int>(nameof(HitPoints));
            //if (hit < ArmourClass) return;

            //var damage = parameters.ToValue<int>("Damage");
            //var newHitPoints = HitPoints - damage;

            // below was commented out before above

            //var newState = CurrentState(hitPoints: newHitPoints);
            //var newCharacter = Clone(newState);

            //DispatchRegistry.Register(newCharacter);
        }
    }
}
