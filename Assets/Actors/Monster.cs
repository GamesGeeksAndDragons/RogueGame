using Assets.Messaging;
using Utils.Coordinates;

namespace Assets.Actors
{
    internal class Monster : Character<Monster>
    {
        public Monster(string state, Coordinate coordinate, DispatchRegistry registry) : base(state, coordinate, registry)
        {
        }

        public Monster(Monster rhs) : base(rhs)
        {
        }

        public override IDispatchee Clone(string parameters = null)
        {
            var clone = new Monster(this);
            clone.UpdateState(parameters.ToParameters());
            return clone;
        }

        public override string ToString()
        {
            return "M";
        }
    }
}
