using Assets.Deeds;
using Assets.Messaging;
using Utils.Coordinates;

namespace Assets.Actors
{
    internal class Monster : Character<Monster>
    {
        public Monster(Coordinate coordinate, DispatchRegistry dispatchRegistry, ActionRegistry actionRegistry, string state) 
            : base(coordinate, dispatchRegistry, actionRegistry, state)
        {
        }

        private Monster(Monster monster) : base(monster.Coordinates, monster.DispatchRegistry, monster.ActionRegistry, "")
        {
        }

        public override string ToString()
        {
            return "M";
        }

        public override Monster Create()
        {
            return ActorBuilder.Build(this);
        }
    }
}
