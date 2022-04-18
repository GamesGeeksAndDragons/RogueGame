using System.Linq;
using Assets.Actors;
using Assets.Messaging;
using Assets.Tiles;
using Utils;
using Utils.Coordinates;
using Utils.Dispatching;
using Utils.Enums;

namespace Assets.Deeds
{
    internal abstract class Action : IAction
    {
        internal ITiles Tiles { get; set; }

        public abstract void Act(IDispatchee dispatchee, string actionValue);
    }

    class MoveAction : Action
    {
        public override void Act(IDispatchee dispatchee, string actionValue)
        {
            var character = (ICharacter)dispatchee;

            var oldCoordinates = character.Position;
            var direction = actionValue.ToEnum<Compass8Points>();
            var newCoordinates = oldCoordinates.Move(direction);

            var newTile = Tiles.GetDispatchee(newCoordinates);
            if (!newTile.IsFloor()) return;
            var to = (Floor)newTile;

            var moved = Tiles.MoveOnto(dispatchee.UniqueId, to);
            if (moved)
            {
                var from = (Floor)Tiles.GetDispatchee(oldCoordinates);
                from.Contains = null;
            }
        }
    }
}
