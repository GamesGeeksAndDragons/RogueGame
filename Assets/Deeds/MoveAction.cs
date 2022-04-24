#nullable enable
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
        public void SetTiles(ITiles tiles) => Tiles = tiles;

        protected ITiles? Tiles { get; private set; }

        public abstract void Act(IDispatched dispatched, string actionValue);
    }

    class MoveAction : Action
    {
        public override void Act(IDispatched dispatched, string actionValue)
        {
            var character = (ICharacter)dispatched;

            var oldCoordinates = character.Position;
            var direction = actionValue.ToEnum<Compass8Points>();
            var newCoordinates = oldCoordinates.Move(direction);

            if (Tiles == null) throw new ArgumentException($"Tried to Move when Tiles is null");

            var newTile = Tiles.GetDispatched(newCoordinates);
            if (!newTile.IsFloor()) return;
            var to = (Floor)newTile;

            var moved = Tiles.MoveOnto(dispatched.UniqueId, to);
            if (!moved) return;

            character.Position = newCoordinates;
            var from = (Floor)Tiles.GetDispatched(oldCoordinates);
            from.SetEmpty();
        }
    }
}
