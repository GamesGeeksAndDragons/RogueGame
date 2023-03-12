#nullable enable
using Assets.Characters;
using Assets.Level;

namespace Assets.Deeds;

internal abstract class Action : IAction
{
    public abstract void Act(IGameLevel level, ICharacter who, string actionValue);
}