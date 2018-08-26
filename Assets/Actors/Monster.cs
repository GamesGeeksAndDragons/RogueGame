using Assets.Messaging;
using Utils.Coordinates;

namespace Assets.Actors
{
    internal class Monster : Character<Monster>
    {
        public Monster(Coordinate coordinate, DispatchRegistry registry, string state) 
            : base(coordinate, registry, state)
        {
        }

        private Monster(Monster monster) : base(monster.Coordinates, monster.Registry, "")
        {
        }

        public override string ToString()
        {
            return "M";
        }

        public override Monster Create()
        {
            return new Monster(this);
        }
    }
}
