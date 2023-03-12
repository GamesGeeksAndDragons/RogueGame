#nullable enable
using Assets.Characters;
using Assets.Level;

namespace Assets.Deeds;

public interface IAction
{
    void Act(IGameLevel level, ICharacter who, string actionValue);
}
