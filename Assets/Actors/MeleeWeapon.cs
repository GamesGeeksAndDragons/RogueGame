using System;
using System.Globalization;
using Assets.Deeds;
using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Random;
using Parameters = System.Collections.Generic.List<(string Name, string Value)>;

namespace Assets.Actors
{
    internal class MeleeWeapon : Dispatchee<MeleeWeapon>
    {
        private readonly IDieBuilder _randomNumbers;
        private readonly Dispatcher _dispatcher;

        public MeleeWeapon(IDieBuilder randomNumbers, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry, Dispatcher dispatcher, string state) 
            : base(Coordinate.NotSet, dispatchRegistry, actionRegistry)
        {
            randomNumbers.ThrowIfNull(nameof(randomNumbers));
            dispatcher.ThrowIfNull(nameof(dispatcher));

            _randomNumbers = randomNumbers;
            _dispatcher = dispatcher;

            var parameters = state.ToParameters();
            UpdateState(parameters);
        }

        public string WeaponName { get; private set; }
        public string Owner { get; private set; }
        public int Level { get; private set; }
        private double OriginalCost { get; set; }

        public double Cost
        {
            get
            {
                var cost = OriginalCost + Hit * 100;
                cost += Damage * 100;
                return cost;
            }
        }

        public double Weight { get; private set; }
        public string MagicBonuses { get; private set; }
        public string Dice { get; private set; }

        internal int Hit { get; private set; }
        internal int Damage { get; private set; }

        internal int NumRolls { get; private set; }
        internal int MaxPoints { get; private set; }

        public override MeleeWeapon Create()
        {
            return ActorBuilder.Build(this);
        }

        public override void UpdateState(Parameters state)
        {
            if (state.HasValue(nameof(WeaponName))) WeaponName = state.ToString(nameof(WeaponName));
            if (state.HasValue(nameof(Owner))) Owner = state.ToString(nameof(Owner));
            if (state.HasValue(nameof(Weight))) Weight = state.ToValue<double>(nameof(Weight));
            if (state.HasValue(nameof(Level))) Level = state.ToValue<int>(nameof(Level));
            if (state.HasValue(nameof(OriginalCost))) OriginalCost = state.ToValue<double>(nameof(OriginalCost));

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

            if (state.HasValue(nameof(MagicBonuses)))
            {
                MagicBonuses = state.ToString(nameof(MagicBonuses));
                (Hit, Damage) = MagicBonuses.FromBrackets();
            }
            else
            {
                if (state.HasValue(nameof(Hit))) Hit = state.ToValue<int>(nameof(Hit));
                if (state.HasValue(nameof(Damage))) Damage = state.ToValue<int>(nameof(Damage));
            }

            base.UpdateState(state);
        }

        public override Parameters CurrentState()
        {
            var state = base.CurrentState();

            if (!WeaponName.IsNullOrEmpty()) state.AppendParameter(nameof(WeaponName), WeaponName);
            if (!Owner.IsNullOrEmpty()) state.AppendParameter(nameof(Owner), Owner);
            if (!IsZero(Weight)) state.AppendParameter(nameof(Weight), Weight);
            if (Level != 0) state.AppendParameter(nameof(Level), Level);
            if (!IsZero(Cost)) state.AppendParameter(nameof(OriginalCost), Cost);
            if (!IsZero(NumRolls)) state.AppendParameter(nameof(NumRolls), NumRolls.ToString());
            if (!IsZero(MaxPoints)) state.AppendParameter(nameof(MaxPoints), MaxPoints);
            if (!IsZero(Hit)) state.AppendParameter(nameof(Hit), Hit);
            if (!IsZero(Damage)) state.AppendParameter(nameof(Damage), Damage);

            return state;
        }

        protected internal override void RegisterActions()
        {
            ActionRegistry.RegisterAction(this, Deed.Use);
        }

        internal int RollDice()
        {
            var sum = 0;

            for (var i = 0; i < NumRolls; i++)
            {
                sum += _randomNumbers.Dice(MaxPoints).Random;
            }

            return sum;
        }
    }
}
