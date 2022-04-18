using Assets.Deeds;
using Utils.Coordinates;
using Utils.Dispatching;

namespace Assets.Actors
{
    internal class Monster : Character<Monster>
    {
        public Monster(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string state) 
            : base(dispatchRegistry, actionRegistry, state)
        {
        }

        private Monster(Monster monster) : base(monster.DispatchRegistry, monster.ActionRegistry, "")
        {
        }

        public override string ToString()
        {
            return this.ToDisplayChar();
        }
    }
}
