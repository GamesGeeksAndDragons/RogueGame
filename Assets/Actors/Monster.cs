using Assets.Messaging;
using Utils.Coordinates;

namespace Assets.Actors
{
    internal class Monster : Dispatchee<Monster>
    {
        public Monster(Coordinate coordinate, DispatchRegistry registry) : base(coordinate, registry)
        {
        }

        public Monster(Monster rhs) : base(rhs)
        {
        }

        public override IDispatchee Clone(string parameters = null)
        {
            var clone = new Monster(this);

            return clone;
        }

        public override string ToString()
        {
            return "M";
        }
    }
}
