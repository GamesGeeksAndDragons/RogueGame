using Assets.Actors;
using Assets.Tiles;

namespace Assets.Actions
{
    public interface IMoveAction : IAction
    {
        bool CanMove(IActor actor, ITile from, Direction direction);
        (ITile, ITile) Move(IActor actor, ITile from, Direction direction);
    }
}