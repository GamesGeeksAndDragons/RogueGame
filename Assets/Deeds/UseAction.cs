#nullable enable
using Assets.Characters;
using Assets.Level;
using Assets.Player;

namespace Assets.Deeds;

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

    public override void Act(IGameLevel level, ICharacter who, string actionValue)
    {
        throw new System.NotImplementedException();
    }
}

