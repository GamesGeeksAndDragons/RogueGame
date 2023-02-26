using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Dispatching;
using Utils.Random;

namespace Assets.Props
{
    // https://beej.us/moria/items.txt

    internal abstract class Prop<T> : Dispatched<T> where T : class, IDispatched
    {
        protected Prop(IDieBuilder randomNumbers, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state, string uniqueId) 
            : base(dispatchRegistry, actionRegistry, actor, uniqueId)
        {
            randomNumbers.ThrowIfNull(nameof(randomNumbers));
            RandomNumbers = randomNumbers;

            PropName = Damage = "";

            var extracted = state.ToParameters();
            // ReSharper disable once VirtualMemberCallInConstructor
            UpdateState(extracted);
        }

        protected readonly IDieBuilder RandomNumbers;

        public string PropName { get; protected set; }
        public double Weight { get; protected set; }
        public int Level { get; protected set; }
        public double BaseCost { get; protected set; }
        public string Damage { get; protected set; }

        public override Parameters CurrentState()
        {
            var state = base.CurrentState();

            state
                .AddPropName(PropName)
                .AddWeight(Weight)
                .AddLevel(Level)
                .AddDamage(Damage)
                .AddBaseCost(BaseCost)
                ;

            return state;
        }

        public override void UpdateState(Parameters state)
        {
            PropName = state.GetPropName();
            Weight = state.GetWeight();
            Level = state.GetLevel();
            BaseCost = state.GetOriginalCost();
            Damage = state.GetDamage();

            base.UpdateState(state);
        }
    }
}
