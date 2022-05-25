#nullable enable
using Assets.Level;
using Assets.Personas;
using Parameters = System.Collections.Generic.IReadOnlyList<(string Name, string Value)>;

namespace Assets.Deeds
{
    public interface IAction
    {
        void Act(IGameLevel level, ICharacter who, string actionValue);
    }
}