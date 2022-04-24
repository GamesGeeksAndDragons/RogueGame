#nullable enable
using Utils;
using Utils.Dispatching;
using Parameters = System.Collections.Generic.IReadOnlyList<(string Name, string Value)>;

namespace Assets.Deeds
{
    class UseAction : Action
    {
        private void UseImpl(Parameters parameters)
        {
            //var dispatched = parameters.ToString("Dispatched");
            //if (dispatched != UniqueId) return;

            //var direction = parameters.ToValue<Compass8Points>("Direction");
            //var owner = DispatchRegistry.GetDispatched(Owner);
            //var strikeCoordindates = owner.Coordinates.Move(direction);
            //var hit = RollDice() + Hit;
            //var damage = RollDice() + Damage;

            //_dispatcher.EnqueueStrike(Owner, hit, damage);
        }

        public override void Act(IDispatched dispatched, string actionValue)
        {
            throw new System.NotImplementedException();
        }
    }
}
