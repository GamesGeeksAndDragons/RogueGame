using Assets.Actors;
using Assets.Tiles;
using Utils.Enums;

namespace Assets.Actions
{
    public interface IMoveAction : IAction
    {
        bool CanMove(IActor actor, ITile from, Compass8Points compass8Point);
        (ITile, ITile) Move(IActor actor, ITile from, Compass8Points compass8Point);
    }
}