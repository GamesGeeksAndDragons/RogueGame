#nullable enable
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Display;
using Utils.Random;

namespace Assets.Props
{
    //TODO: Change to have a Weapon base class
    internal class Weapon : Prop<Weapon>
    {
        private readonly IDieBuilder _randomNumbers;

        public Weapon(IDieBuilder randomNumbers, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry, string state, string uniqueId)
            : base(dispatchRegistry, actionRegistry, TilesDisplay.DebugWeapon, state, uniqueId)
        {
            randomNumbers.ThrowIfNull(nameof(randomNumbers));
            _randomNumbers = randomNumbers;

            Dice = "";
            Level = NumRolls = MaxPoints = Hit = Damage = 0;
        }

        public string Dice { get; private set; }
        internal int Hit { get; private set; }

        internal int NumRolls { get; private set; }
        internal int MaxPoints { get; private set; }

        public override void UpdateState(Parameters state)
        {
            if (state.HasValue(nameof(Dice)))
            {
                Dice = state.ToString(nameof(Dice));
                (NumRolls, MaxPoints) = Dice.FromDice();
            }
            else
            {
                if (state.HasValue(nameof(NumRolls))) NumRolls = state.ToValue<int>(nameof(NumRolls));
                if (state.HasValue(nameof(MaxPoints))) MaxPoints = state.ToValue<int>(nameof(MaxPoints));
            }

            if (state.HasValue(nameof(Hit))) Hit = state.ToValue<int>(nameof(Hit));

            base.UpdateState(state);
        }

        public override Parameters CurrentState()
        {
            var state = base.CurrentState();

            if (!Dice.IsNullOrEmpty()) state.AppendParameter(nameof(Dice), Dice);
            if (NumRolls != 0) state.AppendParameter(nameof(NumRolls), NumRolls.ToString());
            if (MaxPoints != 0) state.AppendParameter(nameof(MaxPoints), MaxPoints);
            if (!DoubleExtensions.IsZero(Hit)) state.AppendParameter(nameof(Hit), Hit);

            return state;
        }

        internal int RollDice()
        {
            var sum = 0;

            for (var i = 0; i < NumRolls; i++)
            {
                sum += _randomNumbers.Between(1, MaxPoints).Random;
            }

            return sum;
        }
    }
}
