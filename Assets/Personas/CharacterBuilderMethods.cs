using Assets.Deeds;
using Utils.Dispatching;

namespace Assets.Personas
{
    internal static class CharacterBuilderMethods
    {
        public static ICharacter BuildMe(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
        {
            return new Me(dispatchRegistry, actionRegistry, actor, state);
        }

        public static ICharacter BuildMonster(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
        {
            return new Monster(dispatchRegistry, actionRegistry, actor, state);
        }
    }
}
