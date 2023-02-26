#nullable enable
using Assets.Level;
using Assets.Personas;

namespace Assets.Deeds;

public interface IAction
{
    void Act(IGameLevel level, ICharacter who, string actionValue);
}
