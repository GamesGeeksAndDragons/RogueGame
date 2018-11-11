using Assets.Messaging;
using Utils;
using Utils.Coordinates;
using Utils.Enums;
using Utils.Random;
using ExtractedParameters = System.Collections.Generic.IReadOnlyList<(string name, string value)>;

namespace Assets.Actors
{
    internal class MeleeWeapon : Dispatchee<MeleeWeapon>
    {
        private readonly IRandomNumberGenerator _randomNumbers;
        private readonly Dispatcher _dispatcher;

        public MeleeWeapon(IRandomNumberGenerator randomNumbers, DispatchRegistry registry, Dispatcher dispatcher, string state) 
            : base(Coordinate.NotSet, registry)
        {
            randomNumbers.ThrowIfNull(nameof(randomNumbers));
            dispatcher.ThrowIfNull(nameof(dispatcher));

            _randomNumbers = randomNumbers;
            _dispatcher = dispatcher;

            var parameters = state.ToParameters();
            UpdateState(this, parameters);
        }

        private MeleeWeapon(MeleeWeapon weapon)
            : this(weapon._randomNumbers, weapon.Registry, weapon._dispatcher, 
                FormatState(weapon.WeaponName, weapon.Owner, weapon.NumRolls, weapon.MaxPoints,
                    weapon.Hit, weapon.Damage, weapon.Weight, weapon.Level, weapon.OriginalCost, weapon.Coordinates))
        {
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

        public override void UpdateState(MeleeWeapon weapon, ExtractedParameters state)
        {
            if (state.HasValue(nameof(WeaponName))) weapon.WeaponName = state.ToString(nameof(WeaponName));
            if (state.HasValue(nameof(Owner))) weapon.Owner = state.ToString(nameof(Owner));
            if (state.HasValue(nameof(Weight))) weapon.Weight = state.ToValue<double>(nameof(Weight));
            if (state.HasValue(nameof(Level))) weapon.Level = state.ToValue<int>(nameof(Level));
            if (state.HasValue(nameof(OriginalCost))) weapon.OriginalCost = state.ToValue<double>(nameof(OriginalCost));

            if (state.HasValue(nameof(Dice)))
            {
                weapon.Dice = state.ToString(nameof(Dice));
                (weapon.NumRolls, weapon.MaxPoints) = weapon.Dice.FromDice();
            }
            else
            {
                if (state.HasValue(nameof(NumRolls))) weapon.NumRolls = state.ToValue<int>(nameof(NumRolls));
                if (state.HasValue(nameof(MaxPoints))) weapon.MaxPoints = state.ToValue<int>(nameof(MaxPoints));
            }

            if (state.HasValue(nameof(MagicBonuses)))
            {
                weapon.MagicBonuses = state.ToString(nameof(MagicBonuses));
                (weapon.Hit, weapon.Damage) = MagicBonuses.FromBrackets();
            }
            else
            {
                if (state.HasValue(nameof(Hit))) weapon.Hit = state.ToValue<int>(nameof(Hit));
                if (state.HasValue(nameof(Damage))) weapon.Damage = state.ToValue<int>(nameof(Damage));
            }

            base.UpdateState(weapon, state);
        }

        public static string FormatState(string weaponName = null, string owner = null, int? numRolls = null, int? maxPoints = null, int? hit = null, int? damage = null, double? weight = null, int? level = null, double? cost = null, Coordinate? coordinates = null, string uniqueId = null)
        {
            var state = string.Empty;

            if (!weaponName.IsNullOrEmpty()) state += nameof(WeaponName).FormatParameter(weaponName);
            if (!owner.IsNullOrEmpty()) state += nameof(Owner).FormatParameter(owner);
            if (weight.HasValue) state += nameof(Weight).FormatParameter(weight.Value);
            if (level.HasValue) state += nameof(Level).FormatParameter(level.Value);
            if (cost.HasValue) state += nameof(OriginalCost).FormatParameter(cost.Value);
            if (numRolls.HasValue) state += nameof(NumRolls).FormatParameter(numRolls.Value);
            if (maxPoints.HasValue) state += nameof(MaxPoints).FormatParameter(maxPoints.Value);
            if (hit.HasValue) state += nameof(Hit).FormatParameter(hit.Value);
            if (damage.HasValue) state += nameof(Damage).FormatParameter(damage.Value);

            return state + Dispatchee<MeleeWeapon>.FormatState(coordinates, uniqueId);
        }

        protected internal override void RegisterActions()
        {
            RegisterAction(Actions.Use, UseImpl);
        }

        internal int RollDice()
        {
            var sum = 0;

            for (var i = 0; i < NumRolls; i++)
            {
                sum += _randomNumbers.Dice(MaxPoints);
            }

            return sum;
        }

        private void UseImpl(ExtractedParameters parameters)
        {
            var dispatchee = parameters.ToString("Dispatchee");
            if (dispatchee != UniqueId) return;

            var direction = parameters.ToValue<Compass8Points>("Direction");
            var owner = Registry.GetDispatchee(Owner);
            var strikeCoordindates = owner.Coordinates.Move(direction);
            var hit = RollDice() + Hit;
            var damage = RollDice() + Damage;

            _dispatcher.EnqueueStrike(strikeCoordindates, hit, damage);
        }
    }
}
