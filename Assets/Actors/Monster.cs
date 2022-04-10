using Assets.Deeds;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Actors
{
    internal class Monster : Character<Monster>
    {
        public Monster(Coordinate coordinate, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string state) 
            : base(coordinate, dispatchRegistry, actionRegistry, state)
        {
        }

        private Monster(Monster monster) : base(monster.Coordinates, monster.DispatchRegistry, monster.ActionRegistry, "")
        {
        }

        public override string ToString()
        {
            return this.ToDisplayChar();
        }

        public override Monster Create()
        {
            return ActorBuilder.Build(this);
        }
    }
}
