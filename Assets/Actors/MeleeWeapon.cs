using Assets.Messaging;
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

        public MeleeWeapon(string state, IRandomNumberGenerator randomNumbers, DispatchRegistry registry, Dispatcher dispatcher) : base(Coordinate.NotSet, registry)
        {
            _randomNumbers = randomNumbers;
            _dispatcher = dispatcher;

            var parameters = state.ToParameters();

            WeaponName = parameters.ToString("Name");
            Owner = parameters.ToString("Owner");
            Dice = parameters.ToString("Dice");
            MagicBonuses = parameters.ToString("MagicBonuses");
            Weight = parameters.ToValue<double>("Weight");
            Level = parameters.ToValue<int>("Level");

            (NumRolls, MaxPoints) = Dice.FromDice();
            (Hit, Damage) = MagicBonuses.FromBrackets();

            Cost = parameters.ToValue<double>("Cost");
            Cost += Hit * 100;
            Cost += Damage * 100;
        }

        public string WeaponName { get; }
        public string Owner { get; }
        public int Level { get; }
        public double Cost { get; }
        public double Weight { get; }
        public string MagicBonuses { get; }
        public string Dice { get; }

        internal int Hit { get; }
        internal int Damage { get; }

        internal int NumRolls { get; }
        internal int MaxPoints { get; }

        public MeleeWeapon(MeleeWeapon rhs) : base(rhs)
        {
        }

        public override IDispatchee Clone(string parameters = null)
        {
            return new MeleeWeapon(this);
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

        public static string BuildState(string name, string owner, int numRolls, int maxPoints, int hit, int damage, double weight, int level, double cost)
        {
            return $"Name [{name}] Owner [{owner}] Dice [{numRolls}d{maxPoints}] MagicBonuses [({hit:+#;-#;0},{damage:+#;-#;0})] Weight [{weight}] Level [{level}] Cost [{cost}]";
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
