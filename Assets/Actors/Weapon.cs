using Assets.Messaging;
using Utils.Coordinates;

namespace Assets.Actors
{
    internal class Weapon : Dispatchee<Weapon>
    {
        public Weapon(DispatchRegistry registry) : base(Coordinate.NotSet, registry)
        {
        }

        public Weapon(Weapon rhs) : base(rhs)
        {
        }

        public override IDispatchee Clone(string parameters = null)
        {
            return new Weapon(this);
        }

        protected internal override void RegisterActions()
        {

        }
    }
}
