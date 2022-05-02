#nullable enable
using Assets.Characters;
using Assets.Mazes;
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
        public abstract void Act(IDispatchRegistry dispatchRegistry, IDispatched dispatched, string actionValue);
    }

    class MoveAction : Action
    {
        public override void Act(IDispatchRegistry dispatchRegistry, IDispatched dispatched, string actionValue)
        {
            var character = (ICharacter)dispatched;

            var oldCoordinates = character.Position;
            var direction = actionValue.ToEnum<Compass8Points>();
            var newCoordinates = oldCoordinates.Move(direction);

            var maze = (IMaze)dispatchRegistry.GetDispatched(Maze.DispatchedName);

            var newTile = maze.GetDispatched(newCoordinates);
            if (!newTile.IsFloor()) return;
            var to = (Floor)newTile;

            var moved = maze.MoveOnto(dispatched.UniqueId, to);
            if (!moved) return;

            character.Position = newCoordinates;
            var from = (Floor)maze.GetDispatched(oldCoordinates);
            from.SetEmpty();
        }
    }
}
