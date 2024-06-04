#nullable enable
using System.Data;
using Assets.Deeds;
using Assets.Monsters;
using Assets.Player;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Display;
using Utils.Random;

namespace Assets.Characters;

internal static class CharacterRandomiser
{
    public static ICharacter RandomCharacter(IDieBuilder dieBuilder, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, CharacterConfig config)
    {
        int Random(string die) => dieBuilder.Between(die).Random;

        var position = Coordinate.NotSet.ToCoordinateString();
        var armourClass = Random(config.ArmourClass).ToArmourClassString();
        var hitPoints = Random(config.HitPoints).ToHitPointsString();

        var state = position.ToState(armourClass, hitPoints);

        if (actor == CharacterDisplay.Me)
            return new Me(dispatchRegistry, actionRegistry, actor, state);

        return new Monster(dispatchRegistry, actionRegistry, actor, state);
    }
}
