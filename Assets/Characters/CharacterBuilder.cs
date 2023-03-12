#nullable enable
using Assets.Deeds;
using Utils.Dispatching;
using Utils.Random;

namespace Assets.Characters;

internal interface ICharacterBuilder
{
    ICharacter LoadCharacter(string actor, string state);
    ICharacter RandomCharacter(string actor, CharacterConfig characterConfig);
}

internal class CharacterBuilder : ICharacterBuilder
{
    private readonly IDispatchRegistry _dispatchRegistry;
    private readonly IActionRegistry _actionRegistry;
    private readonly IDieBuilder _dieBuilder;

    internal CharacterBuilder(IDieBuilder dieBuilder, IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
    {
        _dieBuilder = dieBuilder;
        _dispatchRegistry = dispatchRegistry;
        _actionRegistry = actionRegistry;
    }

    public ICharacter LoadCharacter(string actor, string state)
    {
        return CharacterLoader.Load(_dispatchRegistry, _actionRegistry, actor, state);
    }

    public ICharacter RandomCharacter(string actor, CharacterConfig characterConfig)
    {
        return CharacterRandomiser.RandomCharacter(_dieBuilder, _dispatchRegistry, _actionRegistry, actor, characterConfig);
    }
}
