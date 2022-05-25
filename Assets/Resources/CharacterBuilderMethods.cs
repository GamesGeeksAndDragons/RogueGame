using Assets.Deeds;
using Assets.Personas;
using Utils.Dispatching;

namespace Assets.Resources
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
