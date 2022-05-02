using Assets.Characters;
using Assets.Deeds;
using Utils.Dispatching;

namespace Assets.Resources
{
    internal static class CharacterBuilderMethods
    {
        public static IDispatched BuildMe(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
        {
            return new Me(dispatchRegistry, actionRegistry, actor, state);
        }

        public static IDispatched BuildMonster(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
        {
            return new Monster(dispatchRegistry, actionRegistry, actor, state);
        }
    }
}
