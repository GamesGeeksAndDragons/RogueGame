using Assets.Deeds;
using Utils.Dispatching;
using Utils.Display;
using BuilderMethodType = System.Func<Utils.Dispatching.IDispatchRegistry, Assets.Deeds.IActionRegistry, string, string, Assets.Personas.ICharacter>;

namespace Assets.Personas;

public interface ICharacterBuilder
{
    ICharacter BuildMe(string state);
    ICharacter BuildCharacter(string state);
}

internal class CharacterBuilder : ICharacterBuilder
{
    private readonly Dictionary<string, BuilderMethodType> _builderMethods;
    private readonly IDispatchRegistry _dispatchRegistry;
    private readonly IActionRegistry _actionRegistry;

    internal CharacterBuilder(IDispatchRegistry dispatchRegistry, IActionRegistry actionRegistry)
    {
        _dispatchRegistry = dispatchRegistry;
        _actionRegistry = actionRegistry;

        _builderMethods = new Dictionary<string, BuilderMethodType>
            {
                {CharacterDisplay.Me, CharacterBuilderMethods.BuildMe},
                {CharacterDisplay.DebugMonster, CharacterBuilderMethods.BuildMonster},
            };
    }

    public ICharacter BuildMe(string state)
    {
        var builder = _builderMethods[CharacterDisplay.Me];

        return builder(_dispatchRegistry, _actionRegistry, CharacterDisplay.Me, state);
    }

    public ICharacter BuildCharacter(string state)
    {
        var builder = _builderMethods[CharacterDisplay.DebugMonster];

        return builder(_dispatchRegistry, _actionRegistry, CharacterDisplay.DebugMonster, state);
    }
}
