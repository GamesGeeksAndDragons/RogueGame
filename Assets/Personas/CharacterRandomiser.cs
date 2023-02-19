using Assets.Deeds;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Display;
using Utils.Random;

namespace Assets.Personas;

internal static class CharacterRandomiser
{
    public static ICharacter RandomCharacter(IDieBuilder dieBuilder, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, CharacterConfig config)
    {
        int Random(string die) => dieBuilder.Between(die).Random;

        var position = Coordinate.NotSet;
        var armourClass = Random(config.ArmourClass);
        var hitPoints = Random(config.HitPoints);

        if (actor == CharacterDisplay.Me)
            return new Me(dispatchRegistry, actionRegistry, actor, position, armourClass, hitPoints);

        return new Monster(dispatchRegistry, actionRegistry, actor, position, armourClass, hitPoints);
    }
}
