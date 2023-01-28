#nullable enable
using Assets.Level;
using Assets.Personas;

namespace Assets.Deeds;

internal abstract class Action : IAction
{
    public abstract void Act(IGameLevel level, ICharacter who, string actionValue);
}