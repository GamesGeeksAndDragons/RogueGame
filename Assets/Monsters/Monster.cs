#nullable enable
using Assets.Characters;
using Assets.Deeds;
using Utils.Dispatching;

namespace Assets.Monsters;

// https://github.com/dungeons-of-moria/umoria/blob/master/src/data_creatures.cpp
// https://github.com/dungeons-of-moria/umoria/blob/master/src/monster.cpp

internal class Monster : Character<Monster>
{
    public Monster(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry, string actor, string state)
        : base(dispatchRegistry, actionRegistry, actor, state)
    {
    }
}
