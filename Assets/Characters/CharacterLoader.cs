#nullable enable
using Assets.Deeds;
using Assets.Monsters;
using Assets.Player;
using Utils.Dispatching;
using Utils.Display;

namespace Assets.Characters;

internal static class CharacterLoader
{
    public static ICharacter Load(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
    {
        if (actor == CharacterDisplay.Me) return BuildMe(dispatchRegistry, actionRegistry, actor, state);

        return BuildMonster(dispatchRegistry, actionRegistry, actor, state);
    }

    private static ICharacter BuildMe(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
    {
        return new Me(dispatchRegistry, actionRegistry, actor, state);
    }

    private static ICharacter BuildMonster(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
    {
        return new Monster(dispatchRegistry, actionRegistry, actor, state);
    }
}
